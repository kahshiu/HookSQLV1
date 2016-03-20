using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HookSQLV1
{
    public class Config
    {
        public string Server = "";
        public string DB = "";
        public string AuthType = "";
        public string Username = "";
        public string Password = "";
        public string SeqAction = "";
        public string DBAction = "";
        public string FileAction = "";
        public string RepoPath = "";
        public string InData = "";
        public string LogPath = "";
        public string LogSuffix = "";
        public bool isInit = false;

        public Config(string path)
        {
            isInit = ReadConfigFile(path);
        }

        public bool ReadConfigFile(string path)
        {
            bool hit = false;
            if (File.Exists(path))
            {
                foreach (string line in File.ReadLines(path)) hit = hit || AssignVar(line);
            }
            return hit;
        }

        public bool AssignVar(string line)
        {
            string[] brokenFrags = line.Split('|');
            bool hit = false;
            for (int i = 0; i < brokenFrags.Length; i++)
            {
                if (i == 0) brokenFrags[i] = brokenFrags[i].ToLower();
                brokenFrags[i] = brokenFrags[i].Trim();
            }
            if (brokenFrags[0] == "server") { Server = brokenFrags[1]; hit = true; }
            else if (brokenFrags[0] == "database") { DB = brokenFrags[1]; hit = true; }
            else if (brokenFrags[0] == "authtype") { AuthType = brokenFrags[1]; hit = true; }
            else if (brokenFrags[0] == "username") { Username = brokenFrags[1]; hit = true; }
            else if (brokenFrags[0] == "password") { Password = brokenFrags[1]; hit = true; }
            else if (brokenFrags[0] == "seqaction") { SeqAction = brokenFrags[1]; hit = true; }
            else if (brokenFrags[0] == "dbeaction") { DBAction = brokenFrags[1]; hit = true; }
            else if (brokenFrags[0] == "fileaction") { FileAction = brokenFrags[1]; hit = true; }
            else if (brokenFrags[0] == "repopath") { RepoPath = brokenFrags[1]; hit = true; }
            else if (brokenFrags[0] == "indata") { InData = brokenFrags[1]; hit = true; }
            else if (brokenFrags[0] == "logpath") { LogPath = brokenFrags[1]; hit = true; }
            else if (brokenFrags[0] == "logsuffix") { LogSuffix = brokenFrags[1]; hit = true; }
            return hit;
        }

        // read files for SVN to operate
        public string SetInData(string svnPATH, string filterExtension = "")
        {
            string temp = "";
            string targetFile = "";
            if (File.Exists(svnPATH))
            {
                // scanning lines in svn temp file (contains files to operate on)
                foreach (string file4Action in File.ReadLines(svnPATH))
                {
                    // each file to act on
                    if (File.Exists(file4Action))
                    {
                        if (filterExtension != "")
                        {
                            targetFile = (Path.GetExtension(file4Action) == filterExtension) ? file4Action : "";
                        }
                        else
                        {
                            targetFile = file4Action;
                        }

                        if (targetFile != "")
                        {
                            temp = temp + targetFile + ",";
                        }
                    }
                }
            }
            if (temp.Length > 0) temp = temp.Substring(1);
            InData = temp;
            return temp;
        }

        public string CompileConfigs(string template = @"{0}|{1}")
        {
            string temp = "";
            temp = " " + temp + string.Format(template, "Server", Server);
            temp = " " + temp + string.Format(template, "DB", DB);
            temp = " " + temp + string.Format(template, "AuthType", AuthType);
            temp = " " + temp + string.Format(template, "Username", Username);
            temp = " " + temp + string.Format(template, "Password", Password);
            temp = " " + temp + string.Format(template, "SeqAction", SeqAction);
            temp = " " + temp + string.Format(template, "DBAction", DBAction);
            temp = " " + temp + string.Format(template, "FileAction", FileAction);
            temp = " " + temp + string.Format(template, "InData", InData);
            temp = " " + temp + string.Format(template, "LogPath", LogPath);
            temp = " " + temp + string.Format(template, "LogSuffix", LogSuffix);
            return temp;
        }
    }
}
