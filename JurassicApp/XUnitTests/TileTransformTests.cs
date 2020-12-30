using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using JurassicApp.Models;
using JurassicApp.Models.enums;

namespace XUnitTests
{
    public class TileTransformTests
    {
        [Fact]
        public void TestRotation_1()
        {
            // arrange 
            var tileIn = new Tile(1, new List<string> { "##", ".." });

            // act 
            var rotated = Tile.Rotate(tileIn);

            // assert 
            Assert.True(rotated.TopExposure.Match(new List<CellValue> { CellValue.Pound, CellValue.Dot }));
            Assert.True(rotated.LowerExposure.Match(new List<CellValue> { CellValue.Pound, CellValue.Dot }));

            var _ = rotated.ToString();
        }

        [Fact]
        public void TestRotation_2()
        {
            // arrange 
            var tileIn = new Tile(1, new List<string> { "##", ".." });

            // act 
            var rotated = Tile.Rotate(tileIn, 2);

            // assert 
            Assert.True(rotated.TopExposure.Match(new List<CellValue> { CellValue.Dot, CellValue.Dot }));
            Assert.True(rotated.LowerExposure.Match(new List<CellValue> { CellValue.Pound, CellValue.Pound }));

            var _ = rotated.ToString();
        }

        [Fact]
        public void TestRotation_3()
        {
            // arrange 
            var tileIn = new Tile(1, new List<string> { "##", ".." });

            // out:
            // .#
            // .#

            // act 
            var rotated = Tile.Rotate(tileIn, 3);

            // assert 
            Assert.True(rotated.TopExposure.Match(new List<CellValue> { CellValue.Dot, CellValue.Pound }));
            Assert.True(rotated.LowerExposure.Match(new List<CellValue> { CellValue.Dot, CellValue.Pound }));

            var _ = rotated.ToString();
        }

        [Fact]
        public void TestFlipUD()
        {
            // arrange 
            var tileIn = new Tile(1, new List<string> { "##", ".." });


            // act 
            var flipped = Tile.FlipUD(tileIn);

            // assert 
            Assert.True(flipped.TopExposure.Match(new List<CellValue> { CellValue.Dot, CellValue.Dot }));
            Assert.True(flipped.LowerExposure.Match(new List<CellValue> { CellValue.Pound, CellValue.Pound }));

            Assert.Equal(1, flipped.TileNumber);

            var _ = flipped.ToString();
        }

        [Fact]
        public void TestFlipLR()
        {
            // arrange 
            var tileIn = new Tile(1, new List<string> { "##", ".." });

            // act 
            var flipped = Tile.FlipLR(tileIn);

            // assert 
            Assert.True(flipped.TopExposure.Match(new List<CellValue> { CellValue.Pound, CellValue.Pound }));
            Assert.True(flipped.LowerExposure.Match(new List<CellValue> { CellValue.Dot, CellValue.Dot }));

            Assert.Equal(1, flipped.TileNumber);

            var _ = flipped.ToString();
        }

        [Fact]
        public void VerifyDeFraming()
        {
            // arrange 
            var tileIn = new Tile(1, new List<string> { "####", "#..#", "#..#.", "####" });

            // act
            var tileOut = tileIn.DeFramed();

            // assert
            Assert.True(tileOut.TopExposure.Match(new List<CellValue> { CellValue.Dot, CellValue.Dot }));
            Assert.True(tileOut.LowerExposure.Match(new List<CellValue> { CellValue.Dot, CellValue.Dot }));
            Assert.True(tileOut.RightExposure.Match(new List<CellValue> { CellValue.Dot, CellValue.Dot }));
            Assert.True(tileOut.LeftExposure.Match(new List<CellValue> { CellValue.Dot, CellValue.Dot }));

        }

        [Fact]
        public void GetSubsectionTest()
        {
            // arrange 
            var tileIn = new Tile(1, new List<string> { "###", "#..", "#.." });

            // act 
            var subsection = tileIn.GetSubsection(1, 1, 2, 2);

            // assert
            Assert.True(subsection.TopExposure.Match(new List<CellValue> { CellValue.Dot, CellValue.Dot }));
            Assert.True(subsection.LowerExposure.Match(new List<CellValue> { CellValue.Dot, CellValue.Dot }));

        }
    }
}
