using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFiles.Models.Simple
{
    public class Song
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Artist { get; set; }
        public virtual string Album { get; set; }
        public virtual long Size { get; set; }
        public virtual string FilePath { get; set; }
        public virtual string FileExtension
        {
            get
            {
                if (!string.IsNullOrEmpty(FilePath))
                    return FilePath.Substring(FilePath.LastIndexOf('.') + 1);
                return string.Empty;     
            }
        }
        public virtual Dictionary<string, string> Tags { get; set; }

        public Song()
        {
            Tags = new Dictionary<string, string>();
        }

        public string toJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}
