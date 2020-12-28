using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using JurassicApp.Models;
using JurassicApp.Models.enums;

namespace XUnitTests
{
    public class TileFrameSetTests
    {

        [Fact]
        public void CheckMergeToSingleTile()
        {
            // story: a 2x2 set of tiles; each tile is 4x4 in size
            // after deframe and stitching together, should have a single tile, 4x4 in size
            // subpoint: outer frame data, diff from inner contents, for ease of test.

            // arrange
            var tile1 = new Tile(1, new List<string> { "####", "#..#", "#..#", "####" });
            var tile2 = new Tile(2, new List<string> { "####", "#..#", "#..#", "####" });
            var tile3 = new Tile(3, new List<string> { "####", "#..#", "#..#", "####" });
            var tile4 = new Tile(4, new List<string> { "####", "#..#", "#..#", "####" });
            var initialTileFrame = new TileFrame(tile1);
            var tileset = new TileFrameSet(initialTileFrame);
            tileset.Attach(initialTileFrame, new TileFrame(tile2), side: TileSide.Right);
            tileset.Attach(initialTileFrame, new TileFrame(tile3), TileSide.Lower);
            tileset.Attach(initialTileFrame.Right, new TileFrame(tile4), TileSide.Lower);

            // act 
            var singleTile = tileset.AsSingleTile();

            // assert
            Assert.True(singleTile.TopExposure.Match(new List<CellValue> { CellValue.Dot, CellValue.Dot, CellValue.Dot, CellValue.Dot }));
            Assert.True(singleTile.LowerExposure.Match(new List<CellValue> { CellValue.Dot, CellValue.Dot, CellValue.Dot, CellValue.Dot }));
            Assert.Equal(4, singleTile.NumRows);

        }
    }
}
