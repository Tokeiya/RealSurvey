using System;
using System.Data.SQLite;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Dapper;

namespace Analyze
{
	public static class ResultInserter
	{
		public static bool ExistsCheck(long sessionId, SQLiteConnection connection)
		{
			var ret = connection.ExecuteScalar("SELECT id FROM session WHERE id==@id", new {id = sessionId});

			return ret != null;
		}


		private static (long sessionId,bool createNew) InsertSession(TextReader reader, SQLiteConnection connection)
		{
			reader.ReadLine();
			long sessionId = long.Parse(reader.ReadLine().Split('=')[1]);

			if(ExistsCheck(sessionId,connection))
			{
				return (sessionId, false);
			}

			var cpu = reader.ReadLine().Split('=')[1];
			var os = reader.ReadLine().Split('=')[1];
			reader.ReadLine();
			var framework = reader.ReadLine().Split('=')[1];
			var processArchitecture = reader.ReadLine().Split('=')[1];

			var timestamp = new DateTimeOffset(sessionId, new TimeSpan(0, 9, 0, 0));

			connection.ExecuteScalar(
				"INSERT INTO session(id,timestamp,cpu,os,framework,process_architecture) VALUES(@id,@timestamp,@cpu,@os,@framework,@processArchitecture);",
				new { id = sessionId, timestamp, cpu, os, framework, processArchitecture });

			return (sessionId, true);
		}

		public static void InsertResult(TextReader reader, SQLiteConnection connection, long sessionId)
		{
			using (var tran = connection.BeginTransaction())
			{
				reader.ReadLine();
				string cursor;

				while ((cursor = reader.ReadLine()) != null)
				{
					var value = cursor.Split('\t').Select(x => double.Parse(x)).ToArray();
					var param = new
					{
						sessionId,
						inputDegrees = value[1],
						inputRadians = value[2],
						radians = value[3],
						highPrecisionRadians = value[4],
						sin = value[5],
						cos = value[6],
						tan = value[7],
						sinCos = value[8],
						payneHanekSin = value[9],
						payneHanekCos = value[10],
						payneHanekTan = value[11]
					};

					connection.Execute(
						"INSERT INTO result(session_id,input_degrees,input_radians,radians,high_precision_radians,sin,cos,tan,sin_cos,payne_hanek_sin,payne_hanek_cos,payne_hanek_tan)" +
						"VALUES(@sessionId,@inputDegrees,@inputRadians,@radians,@highPrecisionRadians,@sin,@cos,@tan,@sinCos,@payneHanekSin,@payneHanekCos,@payneHanekTan)",
						param, tran);

				}

				tran.Commit();
			}

		}


		public static void Insert(string archivePath,SQLiteConnection connection)
		{
			using (var archive = ZipFile.OpenRead(archivePath))
			{
				var entries = archive.Entries.ToArray();

				var entry = entries.FirstOrDefault(x => x.FullName.Contains("Environment.txt"));

				(long, bool) sessionId;
				using (var str = entry.Open())
				using (var reader = new StreamReader(str))
				{
					sessionId = InsertSession(reader, connection);
				}

				if (!sessionId.Item2) return;

				entry = entries.FirstOrDefault(x => x.FullName.Contains("Result.tsv"));

				using (var str = entry.Open())
				using (var reader = new StreamReader(str))
				{
					InsertResult(reader, connection, sessionId.Item1);
				}
			}
		}

	}
}
