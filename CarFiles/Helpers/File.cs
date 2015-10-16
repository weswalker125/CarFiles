using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarFiles.Helpers
{
    public class File
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(File));

        static File()
        {
            //configure logging:
            log4net.Config.BasicConfigurator.Configure();
        }

        public static void CorrectFiles(string root, bool makeChanges = true)
        {
            string[] files = System.IO.Directory.GetFiles(root, "*", System.IO.SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; ++i)
            {
                string name = files[i].Substring(files[i].LastIndexOf(Constant.FILE_DELIMITER) + 1);
                string corrected = Helpers.General.CorrectName(name);

                log.InfoFormat("MOVED FILE: {0}", corrected);

                corrected = files[i].Replace(name, corrected);

                log.InfoFormat("MOVED FILE: {0}", corrected);

                if (makeChanges)
                {
                    if(files[i] != corrected)
                        System.IO.File.Move(files[i], corrected);
                }

                FixTags(corrected);
            }
        }

        public static void FixTags(string filePath)
        {
            string uri = filePath;
            TagLib.File file = null;
            try
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
                //file = TagLib.File.Create(new TagLib.f() filePath);

            }
            catch(Exception ex)
            {

            }
        }
    }
}
