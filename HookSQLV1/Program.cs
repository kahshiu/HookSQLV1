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
        private static int superFlag = -1;
        private static ConfigManager CMan;
        //const string NotForRepoMarker = "NOT_FOR_REPO";

        static void Main(string[] args)
        {
            selfParent = GetMyParent();
            CMan = new ConfigManager(args[0]);

            if (BUILD == "pre-commit")
            {
                CMan.SetTargetInData(args[3], args[0]);
                superFlag = CMan.RunSqlMana();
            }

            //string[] affectedPaths = File.ReadAllLines();
            //using (StreamWriter w = new StreamWriter(@"C:\hook_junk\sample.txt"))
            //{
            //    w.WriteLine("------------------");
            //    w.WriteLine(x);
            //    for (int i = 0; i < args.Length; i++)
            //    {

            //        w.WriteLine(args[i]);
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