using System;
using System.IO;
using System.Text.RegularExpressions;

namespace NotForRepoPreCommitHook
{
    class Program
    {
        const string NotForRepoMarker = "NOT_FOR_REPO";

        static void Main(string[] args)
        {
            //string[] affectedPaths = File.ReadAllLines();

            Console.Error.WriteLine("killer");
            Environment.Exit(1);

        }

        static bool ContainsNotForRepoMarker(string path)
        {
            StreamReader reader = File.OpenText(path);

            try
            {
                string line = reader.ReadLine();

                while (line != null)
                {
                    if (line.Contains(NotForRepoMarker))
                    {
                        return true;
                    }

                    line = reader.ReadLine();
                }
            }
            finally
            {
                reader.Close();
            }

            return false;
        }
    }
}