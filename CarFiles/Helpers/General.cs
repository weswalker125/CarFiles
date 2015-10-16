using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarFiles.Helpers
{
    public class General
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(General));

        static General()
        {
            //configure logging:
            log4net.Config.BasicConfigurator.Configure();
        }

        public static string CorrectName(string name)
        {
            string corrected = name;
            if (corrected.ToLower().StartsWith("the "))
            {
                corrected = corrected.Substring(4) + ", The";
                log.DebugFormat("NAME CHANGE FROM: [{0}] TO [{1}]", name, corrected);
            }
            if (name.Contains(""))
            {
                corrected = corrected.Replace("", "");
                log.DebugFormat("REMOVING ODD CHARACTER FROM: {0}", name);
            }
            if (corrected.Contains("[Explicit]"))
            {
                corrected = corrected.Replace("[Explicit]", "");
                log.DebugFormat("REMOVED [Explicit] FROM: {0}", name);
            }
            if (corrected.Contains("[+Digital Booklet"))
            {
                corrected = corrected.Replace("[+Digital Booklet]", "");
                log.DebugFormat("REMOVED [+Digital Booklet] FROM: {0}", name);
            }

            return corrected;
            
        }
    }
}
