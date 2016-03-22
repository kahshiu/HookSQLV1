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

        public void Trace(string x)
        {
            Console.Error.WriteLine(x);
        }

        public Config(string path)
        {
            isInit = ReadConfigFile(path);
            //Trace(CompileConfigs());
        }

        public bool ReadConfigFile(string path)
        {
            bool hit, total = false;
            string line;
            path = Uri.UnescapeDataString(path);
            if (File.Exists(path))
            {
                using (StreamReader r = new StreamReader(path))
                {
                    while ((line = r.ReadLine()) != null)
                    {
                        // check redundant keys
                        hit = AssignVar(line);
                        total = total || hit;
                    }
                }
            }
            return total;
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
            else if (brokenFrags[0] == "dbaction") { DBAction = brokenFrags[1]; hit = true; }
            else if (brokenFrags[0] == "fileaction") { FileAction = brokenFrags[1]; hit = true; }
            else if (brokenFrags[0] == "repopath") { RepoPath = brokenFrags[1]; hit = true; }
            else if (brokenFrags[0] == "indata") { InData = brokenFrags[1]; hit = true; }
            else if (brokenFrags[0] == "logpath") { LogPath = brokenFrags[1]; hit = true; }
            else if (brokenFrags[0] == "logsuffix") { LogSuffix = brokenFrags[1]; hit = true; }
            return hit;
        }

        // read files for SVN to operate
        // TODO: check file is in repo
        // TODO: input in which form? full path/ filename?
        public string SetInData(string svnPATH, string filterExtension = "")
        {
            string temp = "";
            svnPATH = Uri.UnescapeDataString(svnPATH);
            if (File.Exists(svnPATH))
            {
                // scanning lines in svn temp file (contains files to operate on)
                using (StreamReader r = new StreamReader(svnPATH))
                {
                    string path4Action, file4Action, dir4Action, ext4Action, repoFile4Action;
                    while ((path4Action = r.ReadLine()) != null)
                    {
                        // filter out irrelevant files
                        ext4Action = Path.GetExtension(path4Action);
                        file4Action = Path.GetFileName(path4Action);
                        dir4Action = Path.GetDirectoryName(path4Action);

                        // reject files
                        if (filterExtension != "")
                        {
                            if (ext4Action != filterExtension) continue;
                        }
                        if (dir4Action != RepoPath) continue;

                        repoFile4Action = string.Format(@"{0}/{1}", RepoPath, file4Action);

                        // ensure only file in ssp repo is selected
                        if (File.Exists(repoFile4Action))
                        {
                            temp = temp + "," + file4Action;
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
            string temp = "data";
            if (Server != "")
                temp = temp + " " + string.Format(template, "Server", Server);
            if (DB != "")
                temp = temp + " " + string.Format(template, "Database", DB);
            if (AuthType != "")
                temp = temp + " " + string.Format(template, "AuthType", AuthType);
            if (Username != "")
                temp = temp + " " + string.Format(template, "Username", Username);
            if (Password != "")
                temp = temp + " " + string.Format(template, "Password", Password);
            if (SeqAction != "")
                temp = temp + " " + string.Format(template, "SeqAction", SeqAction);
            if (DBAction != "")
                temp = temp + " " + string.Format(template, "DBAction", DBAction);
            if (FileAction != "")
                temp = temp + " " + string.Format(template, "FileAction", FileAction);
            if (RepoPath != "")
                temp = temp + " " + string.Format(template, "RepoPath", RepoPath);
            if (InData != "")
                temp = temp + " " + string.Format(template, "InData", InData);
            if (LogPath != "")
                temp = temp + " " + string.Format(template, "LogPath", LogPath);
            if (LogSuffix != "")
                temp = temp + " " + string.Format(template, "LogSuffix", LogSuffix);
            return temp;
        }
    }
}
