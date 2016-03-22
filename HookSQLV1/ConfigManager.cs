using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;

namespace HookSQLV1
{
    public class ConfigManager
    {
        private Dictionary<string, Config> _configs;
        public string SqlManPath;
        public string dir;
        public string configFile;

        public void Trace(string x)
        {
            Console.Error.WriteLine(x);
        }

        public ConfigManager(string folder, string filename)
        {
            dir = folder;
            configFile = filename;
            init(string.Format(@"{0}\{1}", dir, configFile));
        }

        public void init(string path)
        {
            _configs = new Dictionary<string, Config>();
            bool isRegistered;

            path = Uri.UnescapeDataString(path);
            if (File.Exists(path))
            {
                using (StreamReader r = new StreamReader(path))
                {
                    string configPath;
                    while ((configPath = r.ReadLine()) != null)
                    {
                        isRegistered = RegisterSqlMana(configPath);
                        if (isRegistered) continue;

                        AddConfig(configPath);
                    }
                }
            }
        }

        public bool RegisterSqlMana(string configPath)
        {
            string[] items = configPath.Split('|');
            if (items.Length != 2) return false;

            items[0] = items[0].Trim().ToLower();
            items[1] = items[1].Trim();

            if (items[0] != "sqlmanapath") return false;

            SqlManPath = items[1];
            return true;
        }

        public void AddConfig(string configPath)
        {
            string[] frags = configPath.Split('|');
            string path;
            if (frags.Length != 2) return;

            frags[0] = frags[0].Trim();
            frags[1] = frags[1].Trim();
            path = string.Format(@"{0}\{1}", dir, frags[1]);
            var temp = new Config(path);
            if (temp.isInit)
            {
                _configs.Add(frags[0], temp);
            }
        }

        public Config GetTarget(string svnCWD)
        {
            svnCWD = Uri.UnescapeDataString(svnCWD);
            Config temp = null;
            int hitCount = 0;
            string pattern;
            foreach (KeyValuePair<string, Config> config in _configs)
            {
                pattern = config.Key.ToLower();
                if (pattern.StartsWith(svnCWD.ToLower()))
                {
                    temp = config.Value;
                    hitCount += 1;
                }
            }
            if (hitCount > 1)
            {
                // TODO: resolve ambiguity later, log return to SVN client
                Console.Error.WriteLine("Ambiguous setting: 2 potential setting nest within this commit");
                Console.Error.WriteLine("Ensure only 1 SQL setting in a commit");
                return null;
            }
            return temp;
        }

        public string SetTargetInData(string svnCWD, string svnPATH, string targetExt)
        {
            svnCWD = Uri.UnescapeDataString(svnCWD);
            svnPATH = Uri.UnescapeDataString(svnPATH);

            Config temp = GetTarget(svnCWD);
            string dataString = "";
            if (temp != null)
            {
                dataString = temp.SetInData(svnPATH, targetExt);
            }
            return dataString;
        }

        public string CompileTargetConfigs(string svnCWD)
        {
            svnCWD = Uri.UnescapeDataString(svnCWD);

            Config temp = GetTarget(svnCWD);
            string dataString = "";
            if (temp != null)
            {
                dataString = temp.CompileConfigs();
            }
            return dataString;
        }

        public int RunSqlMana(string svnCWD)
        {
            int flag = -1;

            if (SqlManPath != "")
            {
                svnCWD = Uri.UnescapeDataString(svnCWD);

                //data Server|SQL2008KL Database|seng_test AuthType|serverauth DBAction|selectSSP FileAction|writeSSP username|sa password|password SeqAction|db,file repoPath|c:\inetpub2_ssp\DEV\seng_test2 
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = SqlManPath;
                startInfo.Arguments = CompileTargetConfigs(svnCWD);

                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();

                process.WaitForExit();
                flag = process.ExitCode;
                process.Close();                
            }

            return flag;
        }
    }
}
