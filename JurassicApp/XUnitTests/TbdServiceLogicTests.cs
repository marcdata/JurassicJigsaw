using JurassicApp.Models;
using JurassicApp.Models.enums;
using JurassicApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTests
{
    public class TbdServiceLogicTests
    {
        /* A test of 1) simple 3x3 tile case;
         * check the TileFrame + Tile + Service logic on that...
         * Where TileFrame contains a Tile (inner data), and the TileFrame handles IsConnected, HasOpenSides, linking tiles next to each other,
         * and otherwise helps support the search process. 
         * 
         * Tiles can GetRotated, GetFlipped, etc
         * TileFrame can grow dynamically as we add more Tiles to them, 
         * TileFrame item knows its neighbors, and the Tile it contains. 
         * TileFrameSet (?) has a list of TileFrames that have exposed sides or not. 
         * 
         * So: TileFrameSet (outer collection)
         *          TileFrame (the linking object) (consistent orientation)
         *              Tile (contents) (can change orientation)
         * 
         * Fin.
         */

        /* Idea for how we'd actually grow a TileFrameSet
         * loosely, at timesteps one T1 thru T4 below... where we have Tiles with Ids 1,2,3,4  that are connected to each other...
         * and so we grow the collection. 
         * 
         * 
         *                  1 
         *                  
         * ---------------------------------                 
         *                  
         *              2   1
         *              
         * ---------------------------------
         *               
         *                  3
         *              2   1
         *              
         * ---------------------------------              
         *              
         *                  3
         *              2   1
         *                  4
         *                  
         */

        protected TileFrameSearchService _tileFrameSearchService;

        public TbdServiceLogicTests()
        {
            _tileFrameSearchService = new TileFrameSearchService();
        }

        [Fact]
        public void ProofOfConceptOne()
        {
            // story: 3 tiles, pattern will be an internal "||" shape, markings on the inner vertical edges of the tiles. 
            // but bottom line open (all dots); so we get disambigous answer.

            // so indexes like: 
            // 1 2 3

            // arrange 

            var tile1 = new Tile(1, new List<string> { "..#", "..#", "..#" });
            var tile2 = new Tile(2, new List<string> { "#.#", "#.#", "#.#" });
            var tile3 = new Tile(3, new List<string> { "#..", "#..", "#.#" });

            var remainingTiles = new List<Tile> { tile2, tile3, };

            // tileFrameSet
            var startingTileFrame = new TileFrame(tile1);
            var tileFrameSet = new TileFrameSet(startingTileFrame);

            // act 
            var searchSucceded = _tileFrameSearchService.FillTileFrameSet(tileFrameSet, remainingTiles);

            // assert
            Assert.True(searchSucceded);
            var upperLeftId = tileFrameSet.GetUpperLeftCorner().Tile.TileNumber;
            var upperRightId = tileFrameSet.GetUpperRightCorner().Tile.TileNumber;
            var lowerLeftId = tileFrameSet.GetLowerLeftCorner().Tile.TileNumber;
            var lowerRightId = tileFrameSet.GetLowerRightCorner().Tile.TileNumber;

            Assert.Equal(1, upperLeftId);
            Assert.Equal(3, upperRightId);
            Assert.Equal(1, lowerLeftId);
            Assert.Equal(3, lowerRightId);

        }

        [Fact]
        public void ProofOfConceptTwo()
        {
            // story: 6 tiles, pattern will be an internal "||" shape, markings on the inner vertical edges of the tiles. 
            // but bottom line open (all dots); so we get disambigous answer.
            // added a couple # on the far right side, to help disambiguate.

            // so indexes like: 
            // 1 2 3
            // 4 5 6

            // arrange 

            var tile1 = new Tile(1, new List<string> { 
                "..#", 
                "..#", 
                "..#" });
            var tile2 = new Tile(2, new List<string> { 
                "#.#", 
                "#.#", 
                "#.#" });
            var tile3 = new Tile(3, new List<string> { 
                "###", 
                "#..", 
                "#.#" });
            var tile4 = new Tile(4, new List<string> { 
                "..#", 
                "..#", 
                "..." });
            var tile5 = new Tile(5, new List<string> { 
                "#.#", 
                "#.#", 
                "..." });
            var tile6 = new Tile(6, new List<string> { 
                "#.#", 
                "#..", 
                ".#." });

            // tile6 rotated
            //var tile6rotated = new Tile(6, new List<string> {  ".##", "...", "..."});


            var remainingTiles = new List<Tile> { tile2, tile3, tile4, tile5, tile6 };

            // tileFrameSet
            var startingTileFrame = new TileFrame(tile1);
            var tileFrameSet = new TileFrameSet(startingTileFrame);

            // act 
            var searchSucceded = _tileFrameSearchService.FillTileFrameSet(tileFrameSet, remainingTiles);

            // assert
            Assert.True(searchSucceded);
            var upperLeftId = tileFrameSet.GetUpperLeftCorner().Tile.TileNumber;
            var upperRightId = tileFrameSet.GetUpperRightCorner().Tile.TileNumber;
            var lowerLeftId = tileFrameSet.GetLowerLeftCorner().Tile.TileNumber;
            var lowerRightId = tileFrameSet.GetLowerRightCorner().Tile.TileNumber;

            Assert.Equal(1, upperLeftId);
            Assert.Equal(3, upperRightId);
            Assert.Equal(4, lowerLeftId);
            Assert.Equal(6, lowerRightId);

            var tileCorners = tileFrameSet.GetCornerTileIds();

        }


        [Fact]
        public void TestRotation_Simple1()
        {
            // story: 2 tiles, require one tile has to be rotated in order to match up

            // arrange 

            var tile1 = new Tile(1, new List<string> { 
                "###", 
                "...", 
                "..." 
            });

            // has to be rotated 180 degrees, so it's top side becomes a bottom side
            var tile2 = new Tile(2, new List<string> { 
                "###", 
                "...", 
                "#.#" });

            var remainingTiles = new List<Tile> { tile2, };

            // arrange - tileFrameSet
            var startingTileFrame = new TileFrame(tile1);
            var tileFrameSet = new TileFrameSet(startingTileFrame);

            // act 
            var searchSucceded = _tileFrameSearchService.FillTileFrameSet(tileFrameSet, remainingTiles);

            // assert
            Assert.True(searchSucceded);
            var upperLeftId = tileFrameSet.GetUpperLeftCorner().Tile.TileNumber;
            var lowerLeftId = tileFrameSet.GetLowerLeftCorner().Tile.TileNumber;

            Assert.Equal(1, lowerLeftId);
            Assert.Equal(2, upperLeftId);

        }


        [Fact]
        public void TestFlip_Simple()
        {
            // story: 2 tiles, require one tile has to be rotated in order to match up

            // arrange 

            var tile1 = new Tile(1, new List<string> {
                "###",
                "...",
                "##."
            });

            // has to be flipped LR
            var tile2 = new Tile(2, new List<string> {
                "###",
                "...",
                "#.." });

            // misc, also Tile1, has its Left neighbor set (not null), so it wontmatch on a simpler rotation on that side

            var remainingTiles = new List<Tile> { tile2, };

            // arrange - tileFrameSet
            var startingTileFrame = new TileFrame(tile1);
            startingTileFrame.Left = startingTileFrame; // overriding whatever logic used to checkit, so not really valid, just using to prep the test case
            startingTileFrame.Upper = startingTileFrame;

            // note: setting these dummy neighbors like this, causes bad things when we search for UpperCorner

            var tileFrameSet = new TileFrameSet(startingTileFrame);

            // act 
            var searchSucceded = _tileFrameSearchService.FillTileFrameSet(tileFrameSet, remainingTiles);

            // assert
            Assert.True(searchSucceded);

            Assert.Equal(2, startingTileFrame.Right.TileId);
            Assert.True(startingTileFrame.Right.Tile.LeftExposure.Match(new List<CellValue> { CellValue.Pound, CellValue.Dot, CellValue.Dot }));
        }
    }
}
