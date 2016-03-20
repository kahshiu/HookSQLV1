using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace HookSQLV1
{
    public class ConfigManager
    {
        private Dictionary<string, Config> _configs;

        public ConfigManager(string superConfigPath)
        {
            init(superConfigPath);
        }

        public void init(string superConfigPath)
        {
            _configs = new Dictionary<string, Config>();

            // on single file, points to config for each directory
            if (File.Exists(superConfigPath))
            {
                using (StreamReader r = new StreamReader(superConfigPath))
                {
                    string configPath;
                    while ((configPath = r.ReadLine()) != null)
                    {
                        AddConfig(configPath);
                    }
                }
            }
        }

        public void AddConfig(string configPath)
        {
            string[] frags = configPath.Split('|');
            if (frags.Length != 2) return;

            frags[0] = frags[0].Trim();
            frags[1] = frags[1].Trim();

            var temp = new Config(frags[1]);
            if (temp.isInit)
            {
                _configs.Add(frags[0], temp);
            }
        }

        public Config GetTarget(string svnCWD)
        {
            Config temp = null;
            string pattern;
            foreach (KeyValuePair<string, Config> config in _configs)
            {
                pattern = config.Key.ToLower();
                if (svnCWD.ToLower().StartsWith(pattern))
                {
                    temp = config.Value;
                }
            }
            return temp;
        }

        public string SetTargetInData(string svnCWD,string svnPATH, string targetExt)
        {
            Config temp = GetTarget(svnCWD);
            string dataString = "";
            if(temp != null)
            {
                dataString = temp.SetInData(svnPATH, targetExt);
            }
            return dataString;
        }

        public string CompileTargetConfigs(string svnCWD)
        {
            Config temp = GetTarget(svnCWD);
            string dataString = "";
            if (temp != null)
            {
                dataString = temp.CompileConfigs();
            }
            return dataString;
        }

        public int RunSqlMana ()
        {
            int flag = -1;
            return flag;
        }
    }
}
