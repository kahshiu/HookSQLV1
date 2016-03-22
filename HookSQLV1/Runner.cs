using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace HookSQLV1
{
    public class Runner
    {
        public static void Explore(string filepath)
        {
            Runner.runExe("explorer.exe", @"/Select, " + filepath, false);
        }

        public static List<string> runExe(string exePath, string arguments, Boolean hasOutput)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = exePath;
            startInfo.Arguments = arguments;
            startInfo.RedirectStandardOutput = hasOutput;

            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            List<string> output = new List<string>();
            if (hasOutput)
            {
                string temp = process.StandardOutput.ReadLine();
                while (temp != null)
                {
                    output.Add(temp);
                    temp = process.StandardOutput.ReadLine();
                }
            }
            process.WaitForExit();
            process.Close();

            return output;
        }
    }
}
