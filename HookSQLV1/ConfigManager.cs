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
                foreach (string configPath in File.ReadLines(superConfigPath))
                {
                    AddConfig(configPath);
                }
            }
        }

        public void AddConfig(string configPath)
        {
            var temp = new Config(configPath);
            if (temp.isInit)
            {
                _configs.Add(configPath, temp);
            }
        }

        public Config GetTarget(string svnCWD)
        {
            Config temp = null;
            string pattern = svnCWD.ToLower();
            foreach (KeyValuePair<string, Config> config in _configs)
            {
                if (Regex.IsMatch(config.Key.ToLower(), pattern))
                {
                    temp = config.Value;
                }
            }
            return temp;
        }

        public string SetTargetInData(string svnCWD,string svnPATH)
        {
            Config temp = GetTarget(svnCWD);
            string dataString = "";
            if(temp != null)
            {
                dataString = temp.SetInData(svnPATH);
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
