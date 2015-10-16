using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarFiles
{
    public class Program
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Program));

        private static string root = @"C:\Users\WesWa\Music\";
        static void Main(string[] args)
        {
            //configure logging:
            log4net.Config.BasicConfigurator.Configure();

            Menu();
        }

        private static void Menu()
        {
            int choice = -1;
            string s_choice = "-1";

            while (!string.IsNullOrEmpty(s_choice))
            {
                Console.WriteLine("**** CarFiles ****");
                Console.WriteLine("Selection an option:");
                Console.WriteLine("\t1. Correct file/directory names");
                Console.WriteLine("\t2. Get all songs");
                s_choice = Console.ReadLine();
                if (int.TryParse(s_choice, out choice))
                {
                    PromptForDirectory();
                    switch (choice)
                    {
                        case 1:
                            GetRidOfDiskNumbers();
                            GetRidOfDashes();

                            Helpers.Directory.CorrectDirectories(root, true);
                            Helpers.File.CorrectFiles(root);
                            break;
                        case 2:
                            List<Models.Simple.Song> songs = GetAllSongs();
                            WriteToFile("C:\\Temp\\Songs.json", songs);

                            break;
                        default:
                            Console.WriteLine("Unexpected input!");
                            break;
                    }

                }
            }
        }

        private static void WriteToFile(string filePath, object obj)
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filePath))
            {
                sw.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
            }
        }

        private static void PromptForDirectory()
        {
            string input = "";
            Console.WriteLine("Directory is set to {0}", root);
            Console.Write("Enter directory or press ENTER: ");
            input = Console.ReadLine();
            if(!String.IsNullOrEmpty(input) && System.IO.Directory.Exists(input))
            {
                root = input;
            }            
        }

        private static void AlphaSplit()
        {
            List<string> changers = new List<string>();

            //Get all files that start with _
            string[] directories = System.IO.Directory.GetDirectories(root, "*", System.IO.SearchOption.TopDirectoryOnly);
            List<string> bucket1 = new List<string>();
            List<string> bucket2 = new List<string>();
            List<string> bucket3 = new List<string>();

            string b1, b2, b3;
            b1 = root + "A-D\\";
            b2 = root + "E-L\\";
            b3 = root + "M-Z\\";

            for (int i = 0; i < directories.Length; ++i)
            {
                string name = directories[i].Substring(directories[i].LastIndexOf('\\') + 1);
                if (name[0] < 'D')
                {
                    bucket1.Add(name);
                    changers.Add(directories[i]);
                    string newDir = directories[i].Replace(root, b1);
                    changers.Add(newDir);

                    System.IO.Directory.Move(directories[i], newDir);
                }

                else if (name[0] < 'M')
                {
                    bucket2.Add(name);
                    changers.Add(directories[i]);
                    string newDir = directories[i].Replace(root, b2);
                    changers.Add(newDir);

                    System.IO.Directory.Move(directories[i], newDir);
                }

                else
                {
                    bucket3.Add(name);
                    changers.Add(directories[i]);
                    string newDir = directories[i].Replace(root, b3);
                    changers.Add(newDir);

                    System.IO.Directory.Move(directories[i], newDir);
                }
            }

            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"C:\Temp\alphaSplits-music.txt"))
            {
                sw.WriteLine("Bucket 1 ({0})", bucket1.Count);
                foreach (string i in bucket1)
                    sw.WriteLine(i);
                sw.WriteLine();

                sw.WriteLine("Bucket 2 ({0})", bucket2.Count);
                foreach (string i in bucket2)
                    sw.WriteLine(i);
                sw.WriteLine();

                sw.WriteLine("Bucket 3 ({0})", bucket3.Count);
                foreach (string i in bucket3)
                    sw.WriteLine(i);
                sw.WriteLine();
            } 
            
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"C:\Temp\alphaSplits2-music.txt"))
            {
                foreach (string i in changers)
                    sw.WriteLine(i);
            }

        }

        private static void WeirdCharacter_file()
        {
            List<string> changers = new List<string>();

            //Get all files that start with _
            string[] files = System.IO.Directory.GetFiles(root, "*", System.IO.SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; ++i)
            {
                string name = files[i].Substring(files[i].LastIndexOf('\\') + 1);
                if(name.Contains(""))
                {
                    changers.Add(files[i]);
                }  
            }

            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"C:\Temp\block-music.txt"))
            {
                foreach (string i in changers)
                    sw.WriteLine(i);
            }
        }

        private static void InvisibleFiles()
        {
            List<string> changers = new List<string>();

            //Get all files that start with _
            string[] files = System.IO.Directory.GetFiles(root, "*", System.IO.SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; ++i)
            {
                string name = files[i].Substring(files[i].LastIndexOf('\\') + 1);
                if(name.StartsWith("."))
                {
                    changers.Add(files[i]);

                    System.IO.File.Delete(files[i]);
                }
                
            }

            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"C:\Temp\invisible-music.txt"))
            {
                foreach (string i in changers)
                    sw.WriteLine(i);
            }
        }

        private static void FixDirectoryNames()
        {
            List<string> changers = new List<string>();
            string[] directories = System.IO.Directory.GetDirectories(root, "*", System.IO.SearchOption.AllDirectories);

            for (int i = 0; i < directories.Length; ++i)
            {
                if (directories[i].Contains("[Explicit] [+Digital Booklet]"))
                {
                    changers.Add(directories[i]);

                    string newName = directories[i].Replace("[Explicit] [+Digital Booklet]", "");
                    
                    changers.Add(newName);

                    System.IO.Directory.Move(directories[i], newName);
                }
                else if (directories[i].Contains("[Explicit]"))
                {
                    changers.Add(directories[i]);

                    string newName = directories[i].Replace("[Explicit]", "");

                    changers.Add(newName);
                    
                    System.IO.Directory.Move(directories[i], newName);
                }
            }

            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"C:\Temp\albumTitles-music.txt"))
            {
                foreach (string i in changers)
                    sw.WriteLine(i);
            }
        }

        private static void RemoveAlbumVersion()
        {
            List<string> changers = new List<string>();

            //Get all files that start with _
            string[] files = System.IO.Directory.GetFiles(root, "*", System.IO.SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; ++i)
            {
                try
                {
                    string name = files[i].Substring(files[i].LastIndexOf('\\') + 1).Replace(".mp3", "");
                    if (name.Contains("(Album Version)"))
                    {
                        changers.Add(files[i]);
                        string newName = name.Replace("(Album Version)", "").Trim();
                        changers.Add(files[i].Replace(name, newName));

                        System.IO.File.Move(files[i], files[i].Replace(name, newName));
                    }
                    if (name.Contains("[+Digital Booklet]"))
                    {
                        changers.Add(files[i]);
                        string newName = name.Replace("[+Digital Booklet]", "").Trim();
                        changers.Add(files[i].Replace(name, newName));

                        System.IO.File.Move(files[i], files[i].Replace(name, newName));
                    }
                    if (name.Contains("[Explicit]"))
                    {
                        changers.Add(files[i]);
                        string newName = name.Replace("[Explicit]", "").Trim();
                        changers.Add(files[i].Replace(name, newName));

                        System.IO.File.Move(files[i], files[i].Replace(name, newName));
                    }
                }
                catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }

            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"C:\Temp\albumVersion-music.txt"))
            {
                foreach (string i in changers)
                    sw.WriteLine(i);
            }
        }

        private static void GetRidOfDiskNumbers()
        {
            List<string> changers = new List<string>();

            //Get all files that start with _
            string[] files = System.IO.Directory.GetFiles(root, "*", System.IO.SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; ++i)
            {
                string name = files[i].Substring(files[i].LastIndexOf('\\') + 1);
                if (name.StartsWith("01-01-"))
                {
                    changers.Add(files[i]);

                    string newName = name.Replace("01-01-", "01").Replace("-", "");

                    changers.Add(files[i].Replace(name, newName));

                    System.IO.File.Move(files[i], files[i].Replace(name, newName));
                }
                else if (name.StartsWith("01-"))
                {
                    changers.Add(files[i]);

                    string newName = name.Replace("01-", "").Replace("-", "");

                    changers.Add(files[i].Replace(name, newName));

                    System.IO.File.Move(files[i], files[i].Replace(name, newName));
                }
            }

            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"C:\Temp\01-music.txt"))
            {
                foreach (string i in changers)
                    sw.WriteLine(i);
            }
        }

        private static void GetRidOfDashes()
        {
            List<string> changers = new List<string>();

            //Get all files that start with _
            string[] files = System.IO.Directory.GetFiles(root, "*", System.IO.SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; ++i)
            {
                string name = files[i].Substring(files[i].LastIndexOf('\\') + 1);

                string[] splitter = name.Split('-');
                if (splitter.Length > 1 && splitter[0].Length <= 3)
                {
                    string newName = name.Replace("-", "").Replace("  ", " ");

                    System.IO.File.Move(files[i], files[i].Replace(name, newName));
                }

            }
        }

        private static List<Models.Simple.Song> GetAllSongs()
        {
            TagLib.TagTypes[] types = { TagLib.TagTypes.Id3v1, TagLib.TagTypes.Id3v2, TagLib.TagTypes.FlacMetadata, TagLib.TagTypes.AudibleMetadata };

            List<Models.Simple.Song> ret = new List<Models.Simple.Song>();
            string[] files = System.IO.Directory.GetFiles(root, "*.mp3", System.IO.SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; ++i)
            {
                String[] tokens = files[i].Split(Helpers.Constant.FILE_DELIMITER);

                Models.Simple.Song song = new Models.Simple.Song();
                song.Title = tokens[tokens.Length - 1];
                song.Album = tokens[tokens.Length - 2];
                song.Artist = tokens[tokens.Length - 3];
                song.FilePath = files[i];

                System.IO.FileInfo f = new System.IO.FileInfo(files[i]);
                song.Size = f.Length;

                TagLib.File tlFile = TagLib.File.Create(files[i]);
                TagLib.Tag tag = null;
                
                foreach(TagLib.TagTypes t in types)
                {
                    tag = tlFile.GetTag(t);
                    if (tag != null)
                    {
                        if(tag.Performers != null && tag.Performers.Length > 0) 
                            song.Tags.Add(string.Format("{0}-Artists", t.ToString()), string.Join(", ", tag.Performers));

                        if(!string.IsNullOrEmpty(tag.Album))
                            song.Tags.Add(string.Format("{0}-Album", t.ToString()), tag.Album);

                        if (tag.AlbumArtists != null && tag.AlbumArtists.Length > 0)
                            song.Tags.Add(string.Format("{0}-AlbumArtists", t.ToString()), string.Join(", ", tag.AlbumArtists));

                        if (tag.BeatsPerMinute > 0)
                            song.Tags.Add(string.Format("{0}-BPM", t.ToString()), tag.BeatsPerMinute.ToString());

                        if (tag.Track > 0)
                            song.Tags.Add(string.Format("{0}-TrackNumber", t.ToString()), tag.Track.ToString());

                        if (tag.Year > 0)
                            song.Tags.Add(string.Format("{0}-Year", t.ToString()), tag.Year.ToString());

                        if (!string.IsNullOrEmpty(tag.Title))
                            song.Tags.Add(string.Format("{0}-Title", t.ToString()), tag.Title);

                        if (!string.IsNullOrEmpty(tag.Lyrics))
                            song.Tags.Add(string.Format("{0}-Lyrics", t.ToString()), tag.Lyrics);

                        if (tag.Genres != null && tag.Genres.Length > 0)
                            song.Tags.Add(string.Format("{0}-Genres", t.ToString()), string.Join(", ", tag.Genres));

                        if (tag.Disc > 0)
                            song.Tags.Add(string.Format("{0}-Disc", t.ToString()), tag.Disc.ToString());
                    }
                    ret.Add(song);
                }
            }

            return ret;
        }
    }
}
