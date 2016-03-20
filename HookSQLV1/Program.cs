using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;

namespace HookSQLV1
{
    public class Program
    {
        const string BUILD = "pre-commit";
        private static string selfParent;
        private static string targetExt;
        private static string superConfigFile;
        private static int superFlag;
        private static ConfigManager CMan;
        //const string NotForRepoMarker = "NOT_FOR_REPO";

        static void Main(string[] args)
        {
            selfParent = GetMyParent();
            targetExt = ".cfc";
            superConfigFile = "super.config";
            superFlag = -1;

            if(BUILD == "pre-commit")
            {
                if (args.Length != 4) Environment.Exit(superFlag);
            }
            else if(BUILD == "pre-update")
            {
                if (args.Length != 4) Environment.Exit(superFlag);
            }

            CMan = new ConfigManager(superConfigFile);

            if (BUILD == "pre-commit")
            {
                CMan.SetTargetInData(args[3], args[0], targetExt);
                superFlag = CMan.RunSqlMana();
            }

            //string[] affectedPaths = File.ReadAllLines();

            //using (StreamWriter w = new StreamWriter(@"C:\hook_junk\sample.txt"))
            //using (StreamReader r = new StreamReader(args[0]))
            //{
            //    w.WriteLine("------------------");

            //    string svnFilePath;
            //    while ((svnFilePath = r.ReadLine()) != null)
            //    {
            //        w.WriteLine(svnFilePath);
            //    }
            //}

            Environment.Exit(superFlag);
        }

        static string GetMyParent()
        {
            var tempLoc = new Uri(Assembly.GetEntryAssembly().GetName().CodeBase);
            return new FileInfo(tempLoc.AbsolutePath).Directory.FullName;
        }

        //public static bool ContainsNotForRepoMarker(string path)
        //{
        //    StreamReader reader = File.OpenText(path);

        //    try
        //    {
        //        string line = reader.ReadLine();

        //        while (line != null)
        //        {
        //            if (line.Contains(NotForRepoMarker))
        //            {
        //                return true;
        //            }

        //            line = reader.ReadLine();
        //        }
        //    }
        //    finally
        //    {
        //        reader.Close();
        //    }

        //    return false;
        //}
    }
}