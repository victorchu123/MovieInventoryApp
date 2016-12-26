using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQL;
using System.Text.RegularExpressions;

namespace UnitTestProject1
{
    [TestClass]
    public class IMDBRatingsTests
    {
        //tests if lines are processed correctly into the correct movie and rank format
        [TestMethod]
        public void TestProcessLinesFromFile()
        {
            IMDBRatings ratings = new IMDBRatings();
            Assert.IsTrue(ratings.ProcessLinesFromFile(ratings.example_line));
        }

        [TestMethod]
        //tests if function gets rid of leading zeros in the distribution portion of the txt file.
        public void TestParseString()
        {
            IMDBRatings ratings = new IMDBRatings();
            //Console.WriteLine(ratings.ParseString(ratings.example_line));
            Assert.IsTrue(Regex.IsMatch(ratings.ParseString(ratings.example_line), @"(?!\d{10})"));
        }
    }
}
