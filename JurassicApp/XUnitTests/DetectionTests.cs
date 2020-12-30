using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using JurassicApp.Models;
using JurassicApp.Services;
using System.Linq;

namespace XUnitTests
{
    public class DetectionTests
    {

        /// <summary>
        /// Test case based on the example for Part 2. 
        /// </summary>
        [Fact]
        public void TryDetectExampleCase()
        {
            // arrange 

            var tile = new Tile(0, new List<string>
            {
                ".#.#..#.##...#.##..#####" ,
                "###....#.#....#..#......",
                "##.##.###.#.#..######...",
                "###.#####...#.#####.#..#",
                "##.#....#.##.####...#.##",
                "...########.#....#####.#",
                "....#..#...##..#.#.###..",
                ".####...#..#.....#......",
                "#..#.##..#..###.#.##....",
                "#.####..#.####.#.#.###..",
                "###.#.#...#.######.#..##",
                "#.####....##..########.#",
                "##..##.#...#...#.#.#.#..",
                "...#..#..#.#.##..###.###",
                ".#.#....#.##.#...###.##.",
                "###.#...#..#.##.######..",
                ".#.#.###.##.##.#..#.##..",
                ".####.###.#...###.#..#.#",
                "..#.#..#..#.#.#.####.###",
                "#..####...#.#.#.###.###.",
                "#####..#####...###....##",
                "#.##..#..#...#..####...#",
                ".#.###..##..##..####.##.",
                "...###...##...#...#..###"
            });

            // transforms; flip and 1 rotation CCW
            tile = Tile.FlipLR(tile);
            tile = Tile.Rotate(tile, 1);

            var detectionService = new DetectionService();

            // TargetTile from sample
            //# 
            //#    ##    ##    ###
            //#  #  #  #  #  #   

            var targetTile = new Tile(0, new List<string> {
                "..................#.",
                "#....##....##....###",
                ".#..#..#..#..#..#..." });

            // act
            var numDetected = detectionService.CountOccurences(tile, targetTile);

            // assert
            Assert.Equal(2, numDetected);
        }

        /// <summary>
        /// Test case based on the example for Part 2. 
        /// </summary>
        [Fact]
        public void TryDetectExampleCase_Variant2()
        {
            // arrange 

            var tile = new Tile(0, new List<string>
            {
                ".#.#..#.##...#.##..#####" ,
                "###....#.#....#..#......",
                "##.##.###.#.#..######...",
                "###.#####...#.#####.#..#",
                "##.#....#.##.####...#.##",
                "...########.#....#####.#",
                "....#..#...##..#.#.###..",
                ".####...#..#.....#......",
                "#..#.##..#..###.#.##....",
                "#.####..#.####.#.#.###..",
                "###.#.#...#.######.#..##",
                "#.####....##..########.#",
                "##..##.#...#...#.#.#.#..",
                "...#..#..#.#.##..###.###",
                ".#.#....#.##.#...###.##.",
                "###.#...#..#.##.######..",
                ".#.#.###.##.##.#..#.##..",
                ".####.###.#...###.#..#.#",
                "..#.#..#..#.#.#.####.###",
                "#..####...#.#.#.###.###.",
                "#####..#####...###....##",
                "#.##..#..#...#..####...#",
                ".#.###..##..##..####.##.",
                "...###...##...#...#..###"
            });

            var detectionService = new DetectionService();

            // TargetTile from sample
            //# 
            //#    ##    ##    ###
            //#  #  #  #  #  #   

            var targetTile = new Tile(0, new List<string> {
                "..................#.",
                "#....##....##....###",
                ".#..#..#..#..#..#..." });

            // act
            var numDetected = detectionService.CountOccurences(tile, targetTile, searchRotations: true);

            // assert
            Assert.Equal(2, numDetected);
        }

        [Fact]
        public void TryDetectSimpler()
        {
            // story: upper left corner of the 2x2 ##/## pattern should be at row=1, col=3.

            // arrange 

            var tile = new Tile(0, new List<string>
            {
                ".#.#...#####" ,
                "..###.......",
                "#..######...",
            });

            var target = new Tile(1, new List<string> { "##", "##" });
            
            var detectionService = new DetectionService();

            // act
            var numDetected = detectionService.CountOccurences(tile, target);

            // assert
            Assert.Equal(1, numDetected);
        }

        [Fact]
        public void TryDetectSimpler_Case2()
        {
            // story: upper left corner of the 2x2 ##/## pattern should be at row=1, col=3.

            // arrange 

            var tile = new Tile(0, new List<string>
            {
                ".#.#...#" ,
                "..###...",
                "#..#####",
            });

            var target = new Tile(1, new List<string> { "##", "##" });

            var detectionService = new DetectionService();

            // act
            var foundCoordinates = detectionService.FindOccurences(tile, target);

            // assert
            var first = foundCoordinates.FirstOrDefault();
            Assert.Equal(3, first.xcoord);
            Assert.Equal(1, first.ycoord);
        }


        [Fact]
        public void TryDetectSimpler_Case3()
        {
            // story: upper left corner of the 2x2 #./## pattern should be:
            // found twice
            // at row,col (0, 3) and (1, 4)

            // arrange 

            var tile = new Tile(0, new List<string>
            {
                ".#.#...#" ,
                "..###...",
                "#...####",
            });

            var target = new Tile(1, new List<string> { "#.", "##" });

            var detectionService = new DetectionService();

            // act
            var foundCoordinates = detectionService.FindOccurences(tile, target);

            // assert
            Assert.Equal(2, foundCoordinates.Count);

            Assert.Equal(3, foundCoordinates.FirstOrDefault().xcoord);
            Assert.Equal(0, foundCoordinates.FirstOrDefault().ycoord);

            Assert.Equal(4, foundCoordinates.Skip(1).FirstOrDefault().xcoord);
            Assert.Equal(1, foundCoordinates.Skip(1).FirstOrDefault().ycoord);
        }
    }
}
