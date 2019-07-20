using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace SurveyGrocery
{
	internal static class CpuInfo
	{
		static string[] RunCommand(string cmd, string arguments = "")
		{
			var info = new ProcessStartInfo
			{
				FileName = cmd,
				WorkingDirectory = "",
				Arguments = arguments,
				UseShellExecute = false,
				CreateNoWindow = true,
				RedirectStandardOutput = true,
				RedirectStandardError = true
			};
			var lst = new List<string>();

			using (var proc = Process.Start(info))
			{
				string s;

				while ((s = proc.StandardOutput.ReadLine()) != null)
				{
					lst.Add(s);
				}
			}

			return lst.ToArray();
		}

		static string GetCpuNameWindows()
		{
			using (var info = new ManagementObjectSearcher("SELECT * FROM Win32_Processor"))
			using (var tmp = info.Get().Cast<ManagementObject>().FirstOrDefault())
			{
				if (tmp == null) return "N/A";

				return tmp["Name"]?.ToString() ?? "N/A";
			}
		}

		static string GetCpuNameLinux()
		{
			var ret = RunCommand("cat", "/proc/cpuinfo");

			var dta = ret.Where(s => Regex.IsMatch(s, "^model name\\s*:.*?$")).FirstOrDefault();
			if (dta == null) return "N/A";

			return Regex.Replace(dta, "(^.*?:\\s*)(.*)$", "$2");
		}

		static string GetCpuNameOsx()
		{
			//sysctl machdep.cpu.brand_string
			//machdep.cpu.brand_string: Intel(R) Core(TM) i7 - 9750H CPU @ 2.60GHz

			var ret = RunCommand("sysctl", "machdep.cpu.brand_string");
			if (ret.Length == 0) return "N/A";

			return Regex.Replace(ret[0], "(^.*?:\\s*)(.*)$", "$2");
		}

		public static string GetCpuName()
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				return GetCpuNameWindows();
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				return GetCpuNameLinux();
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				return GetCpuNameOsx();
			}

			throw new InvalidOperationException("Unexpected platform...");

		}



	}
}
