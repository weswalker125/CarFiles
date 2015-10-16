using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarFiles.Helpers
{
    public class Constant
    {
#if WINDOWS
        public static readonly char FILE_DELIMITER = '\\';
#else
        public static readonly char FILE_DELIMITER = '/';
#endif
    }
}
