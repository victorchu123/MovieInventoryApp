

using MovieInventoryApp.Models;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ratings
{
    public class IMDBRatings
    {
        public string example_line;

        public IMDBRatings()
        {
            example_line = "      0000000125  1669394   9.2  The Shawshank Redemption (1994)";
        }

        private MovieDBContext db = new MovieDBContext();
        public int MAX = 500; // Formula: number of 'top' ratings from IMDB * 2 
                              // **As long as the IMDB top 250 ratings are within the first 500 lines,
                              // then this is correct.**

		public void ParseTxtFile(string filePath)
        {
            string[] all_lines = new string[MAX]; //only allocate memory here
            using (StreamReader sr = File.OpenText(filePath))
            {
                int x = 0;
                while (!sr.EndOfStream)
                {
                    all_lines[x] = sr.ReadLine();
                    x += 1;
                }
            }

            //CLOSE THE FILE because we are now DONE with it.
            Parallel.For(0, all_lines.Length, x =>
            {
                ProcessLinesFromFile(all_lines[x]);
            });
        }

		//takes out the unnecessary parts of the provided imdb movie entry (from .txt file)
		public string ParseString(string line)
        {
            string new_line = line.Trim();

            string[] split_line = Regex.Split(new_line, @"\s{2,}"); //splits into distribution #, votes, rank, title, and release_year

            string parsed_line = "";
            bool separation_spaces = false; //determines if separation spaces is needed

            //gets rid of distribution #
            for (int i = 0; i < split_line.Length; i++)
            {
                if (i != 0)
                {
                    if (i != 1)
                    {
                        separation_spaces = true;
                    }
                    parsed_line = parsed_line + (separation_spaces ? "  " : "") + split_line[i];
                } 
            }

            return parsed_line;
        }

        private void UpdateDB(Rank rank, Movie movie)
        {
            var sql_querier = new MovieSQLQuerier();
            //TODO: insert rank to sql database here
            db.Ranks.Add(rank);

            //TODO: insert movie to sql database here
            db.Movies.Add(movie);
            db.SaveChanges();
        }

        public bool ProcessLinesFromFile(string current_line)
        {
            bool votes_correct = false; //checks if vote's format is correct
            bool rating_correct = false; //checks if rank's format is correct
            bool title_correct = false; //checks if title's format is correct
            bool date_correct = false; //checks if release date's format is correct

            string new_line = ParseString(current_line);
            string[] split_line = Regex.Split(new_line, @"\s{2,}"); //splits into votes, rank, title, and release_year

            Console.WriteLine("split_line array:");
            foreach (string str in split_line)
            {
                Console.WriteLine(str);
            }

            Rank rank = new Rank();
            rank.Votes = Convert.ToInt64(split_line[0]);

            rank.Rating = Convert.ToDecimal(split_line[1]);

            if (Regex.IsMatch(split_line[0], @"\d+"))
            {
                votes_correct = true;
            }

            if (Regex.IsMatch(split_line[1], @"\d\.\d") &&
                rank.Rating <= 10 && rank.Rating >= 0)
            {
                rating_correct = true;
            }

            Movie movie = new Movie();
            string[] title = Regex.Split(split_line[2], @"\s+");

            Console.WriteLine("title array:");
            foreach (string str in title)
            {
                Console.WriteLine(str);
            }

            int title_arr_length = title.Length;

            //sets the release date as the last entry in the parsed title
            string release_date = title[title_arr_length - 1];

            //any item in mc is a string, Matches postcondition
            MatchCollection mc = Regex.Matches(release_date, @"\d{4}");

            release_date = mc[0].Value;

            string official_title = "";

            for (int i = 0; i < title_arr_length - 1; i++)
            {
                official_title += " " + title[i];
            }

            movie.Title = official_title;

            Console.WriteLine("title for db:" + official_title);
            Console.WriteLine("release_date for db:" + release_date);

            Date date = new Date();
            date.Year = Convert.ToInt16(release_date);
            movie.ReleaseDate = date;
            movie.IMDBRank = rank;

            if (Regex.IsMatch(official_title, @"([a-z]+\s*)*"))
            {
                title_correct = true;
            }

            if (Regex.IsMatch(release_date, @"\d{4}"))
            {
                date_correct = true;
            }

            Console.WriteLine(votes_correct);
            Console.WriteLine(rating_correct);
            Console.WriteLine(title_correct);
            Console.WriteLine(date_correct);



            if (votes_correct && title_correct && date_correct && rating_correct)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class SQLScript
    {
        private static string directory_path = @"C:\Users\Victor\Documents\Visual Studio 2015\Projects\MovieInventoryApp\MovieInventoryApp\App_Data";

        //unzips .gz file
        public static void Decompress(FileInfo file_to_decompress)
        {
            using (FileStream original_file_stream = file_to_decompress.OpenRead())
            {
                string current_file_name = file_to_decompress.FullName;
                string newFileName = current_file_name.Remove(current_file_name.Length - file_to_decompress.Extension.Length);

                using (FileStream decompressed_file_stream = File.Create(newFileName))
                {
                    using (GZipStream decompressionStream = new GZipStream(original_file_stream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressed_file_stream);
                        Console.WriteLine("Decompressed: {0}", file_to_decompress.Name);
                    }
                }
            }
        }

        public static void Main()
        {
            IMDBRatings imdb_ratings = new IMDBRatings();

            using (WebClient client = new WebClient())
            {
                client.DownloadFile("ftp://ftp.fu-berlin.de/pub/misc/movies/database/ratings.list.gz", "IMDB-ratings.list.gz");
            }

            DirectoryInfo directory_selected = new DirectoryInfo(directory_path);
            FileInfo[] gz_files = directory_selected.GetFiles("*.gz");
            Decompress(gz_files[0]);
            imdb_ratings.ParseTxtFile(@"C:\Users\Victor\Documents\Visual Studio 2015\Projects\MovieInventoryApp\MovieInventoryApp\App_Data\ratings.list");
        }
    }
}


