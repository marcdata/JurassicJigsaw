using JurassicApp.Models;
using JurassicApp.Models.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JurassicApp.Services
{
    public interface ITileFrameSearchService
    {
        public bool FillTileFrameSet(TileFrameSet tileframeset, IEnumerable<Tile> tiles);
    }

    public class TileFrameSearchService : ITileFrameSearchService
    {
        /// <summary>
        /// Return whether or not the service was able to match all the tiles to the grid. 
        /// </summary>
        /// <param name="tileframeset"></param>
        /// <param name="tiles"></param>
        /// <returns></returns>
        public bool FillTileFrameSet(TileFrameSet tileFrameSet, IEnumerable<Tile> tiles)
        {
            var tileQueue = new Queue<Tile>(tiles);
            var maxIter = 100; // 1000000;
            var iterCount = 0;

            // steps
            // for every item in the queue
                // try to match it against all the open Tiles in the current Set.
                // if match, attach to TileFrame item, and leave removed from Queue
                    // attach at whatever side we match on
                // if no match, re-queue for try again later.

            while(tileQueue.Count > 0 && iterCount < maxIter)
            {
                var tile = tileQueue.Dequeue();

                var foundMatch = false;

                // for all open tiles
                var thisIterationSearchCollection = tileFrameSet.TileFrames.Where(x => x.AnyOpenSides).ToList();
                foreach (var openFrame in thisIterationSearchCollection)
                {
                    if(this.FindMatchingSide(openFrame, tile, out var side))
                    {
                        Attach(openFrame, tile, side.Value);
                        // attach to TileFrames
                        tileFrameSet.TileFrames.Add(openFrame.GetNeighbor(side.Value));
                        foundMatch = true;

                        // collect dbg statements

                        break;
                    }
                }

                //if (tileQueue.Count == 0)
                //{
                //    throw new Exception("Infinite loop probably happening; not matching final element.");
                //}

                if (!foundMatch)
                {
                    tileQueue.Enqueue(tile);
                }

                ++iterCount;
            }

            if(tileQueue.Count == 0)
            {
                return true;
            }
            return false;
        }


        private bool FindMatchingSide(TileFrame openFrame, Tile tile, out TileSide? side)
        {
            // check tileFrame.RightExposure to tile.LeftExposure
            // check tileFrame.UpperExposure to tile.LowerExposure

            if (openFrame.Right == null && openFrame.Tile.RightExposure.Match(tile.LeftExposure))
            {
                side = TileSide.Right;
                return true;
            }
            if (openFrame.Left == null && openFrame.Tile.LeftExposure.Match(tile.RightExposure))
            {
                side = TileSide.Left;
                return true;
            }
            if (openFrame.Upper == null && openFrame.Tile.TopExposure.Match(tile.LowerExposure))
            {
                side = TileSide.Upper;
                return true;
            }
            if (openFrame.Lower == null && openFrame.Tile.LowerExposure.Match(tile.TopExposure))
            {
                side = TileSide.Lower;
                return true;
            }

            // check other sides
            side = null;
            return false;

        }

        private void Attach(TileFrame openFrame, Tile tile, TileSide side)
        {
            if (side == TileSide.Upper && openFrame.Upper == null)
            {
                openFrame.Upper = new TileFrame(tile);
            }
            else if (side == TileSide.Lower && openFrame.Lower == null)
            {
                openFrame.Lower = new TileFrame(tile);
            }
            else if (side == TileSide.Right && openFrame.Right == null)
            {
                openFrame.Right = new TileFrame(tile);
            }
            else if (side == TileSide.Left && openFrame.Left == null)
            {
                openFrame.Left = new TileFrame(tile);
            }
            else
            {
                throw new InvalidOperationException("Error attaching tile; tile probably already in use.");
            }

        }

    }
}
