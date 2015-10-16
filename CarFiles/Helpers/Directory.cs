using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarFiles.Helpers
{
    public class Directory
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Directory));

        static Directory() {
            //configure logging:
            log4net.Config.BasicConfigurator.Configure();
        }

        public static void CorrectDirectories(string root, bool makeChanges = true)
        {
            string[] directories = System.IO.Directory.GetDirectories(root, "*", System.IO.SearchOption.AllDirectories);

            for (int i = 0; i < directories.Length; ++i)
            {
                string name = directories[i].Substring(directories[i].LastIndexOf(Constant.FILE_DELIMITER) + 1);
                string corrected = Helpers.General.CorrectName(name);

                log.InfoFormat("MOVED DIRECTORY: {0}", corrected);

                corrected = directories[i].Replace(name, corrected);

                if (makeChanges)
                {
                    if(directories[i] != corrected)
                        System.IO.Directory.Move(directories[i], corrected);
                }
            }
        }
    }
}
