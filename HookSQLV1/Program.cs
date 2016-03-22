using System;
using System.IO;
using System.Reflection;

namespace HookSQLV1
{
    public class Program
    {
        const string BUILD = "pre-commit";
        private static string selfParent;
        private static string targetExt;
        private static string superConfigFile;
        private static int superFlag = 0;
        private static int SVNFlag;
        private static ConfigManager CMan;

        static void Trace(string x)
        {
            Console.Error.WriteLine(x);
        }

        static void Main(string[] args)
        {
            selfParent = GetMyParent();
            targetExt = ".sql";
            superConfigFile = "super.config";
            superFlag = -1;

            if (BUILD == "pre-commit" || BUILD == "pre-update")
            {
                if (args.Length != 4) Environment.Exit(superFlag);
            }
            CMan = new ConfigManager(selfParent, superConfigFile);
            if (BUILD == "pre-commit" || BUILD == "pre-update")
            {
                CMan.SetTargetInData(args[3], args[0], targetExt);
                superFlag = CMan.RunSqlMana(args[3]);
            }

            // Diagnostics: test by writing to a sample file
            //using (StreamWriter w = new StreamWriter(@"C:\hook_junk\sample.txt"))
            //using (StreamReader r = new StreamReader(args[0]))
            //{
            //    w.WriteLine("------------------");

            //    string x = CMan.GetTarget(args[3]).CompileConfigs();
            //    w.WriteLine(x);

            //    foreach (string a in args)
            //    {
            //        w.WriteLine(a);
            //    }
            //    string svnFilePath;
            //    while ((svnFilePath = r.ReadLine()) != null)
            //    {
            //        w.WriteLine("hello");
            //    }
            //}

            //TODO: if error open error log folder

            // SVN success flag => 0
            if (superFlag < 0)
            {
                SVNFlag = superFlag;

                string temp = CMan.GetTarget(args[3]).LogPath;

                // get latest modified file
                DateTime latest = new DateTime(1900, 1, 1);
                string latestFile = "";
                foreach (FileInfo fInfo in new DirectoryInfo(temp).GetFiles())
                {
                    if (fInfo.LastWriteTime > latest)
                    {
                        latest = fInfo.LastWriteTime;
                        latestFile = fInfo.Name;
                    }
                }
                temp = Uri.UnescapeDataString(string.Format(@"{0}\{1}", temp, latestFile));
                Console.Error.WriteLine("[HookSQL] Error in uploading SSP to databse");
                Console.Error.WriteLine("[HookSQL] Check log in popped window");
                Runner.Explore(temp);
            }
            Environment.Exit(SVNFlag);
        }

        static string GetMyParent()
        {
            var tempLoc = new Uri(Assembly.GetEntryAssembly().GetName().CodeBase);
            return new FileInfo(tempLoc.AbsolutePath).Directory.FullName;
        }
    }
}