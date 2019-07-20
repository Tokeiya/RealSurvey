using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Dapper;

namespace Analyze
{
	public static class DataStrageManager
	{
		public static SQLiteConnection CreateConnection()
		{
			DefaultTypeMap.MatchNamesWithUnderscores = true;

			var bld = new SQLiteConnectionStringBuilder();
			bld.DataSource = "../../../../ResultStrage.sqlite3";
			bld.DateTimeFormat = SQLiteDateFormats.UnixEpoch;
			bld.ForeignKeys = true;

			return new SQLiteConnection(bld.ToString());
		}

		public static (Result[] results, Session session) Load(long sessionId, SQLiteConnection connection)
		{
			var session = connection.Query<Session>("SELECT * FROM session WHERE id==@sessionId", new {sessionId})
				.ToArray().FirstOrDefault();

			var results =
				connection.Query<Result>("SELECT * FROM result WHERE session_id==@sessionId", new {sessionId})
					.ToArray();

			return (results, session);
		}

		public static readonly Expression<Func<Result, double>>[] PayneHanekTrigonometric =
		{
			x=>x.HighPrecisionRadians,
			x => x.PayneHanekSin,
			x => x.PayneHanekCos,
			x => x.PayneHanekTan
		};

		public static readonly Expression<Func<Result, double>>[] NormalTrigonometric =
		{
			x => x.Radians,
			x => x.Sin,
			x => x.Cos,
			x => x.Tan,
			x => x.SinCos,
		};

		public static readonly Expression<Func<Result, double>>[] AllTrigonometric = DataStrageManager.PayneHanekTrigonometric.Concat(NormalTrigonometric).ToArray();
	}
}
