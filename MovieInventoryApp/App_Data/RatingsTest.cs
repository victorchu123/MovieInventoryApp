using System;
using MovieInventoryApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RatingTests
{
    [TestClass]
    public class RatingsTests
    {
        //tests if lines are processed correctly into the correct movie and rank format
        [TestMethod]
        public void TestProcessLinesFromFile()
        {
            IMDBRatings ratings = new IMDBRatings();
            Assert.IsTrue(ProcessLinesFromFile(ratings.example_line));
        }

        [TestMethod]
        //tests if function gets rid of leading zeros in the distribution portion of the txt file.
        public void TestParseString()
        {
            IMDBRatings ratings = new IMDBRatings();
            Assert.IsTrue(Regex.IsMatch(ParseString(ratings.example_line), @"(?!\d{10})"));
        }
    }

}
