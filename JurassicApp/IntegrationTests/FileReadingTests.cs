using System;
using Xunit;
using JurassicApp;
using JurassicApp.IO;

namespace IntegrationTests
{
    public class FileReadingTests
    {
        [Fact]
        public void ReadSampleFile()
        {
            // story: the sample file has 9 tiles, and at time of test, we are only reading 8

            // arrange
            var filename = @"C:\Users\marc\source\repos\marcdata\JurassicJigsaw\inputdata\sampleinput.txt";

            var jurassicFileReader = new JurassicFileReader();

            // act
            var tiles = jurassicFileReader.Read(filename);

            // assert

            // assert num tiles read == 9
            Assert.Equal(9, tiles.Count);
        }
    }
}
