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
        public bool FillTileFrameSet(TileFrameSet tileframeset, IEnumerable<Tile> tiles, bool allowTransforms = false, bool verbose = true);
    }

    public class TileFrameSearchService : ITileFrameSearchService
    {
        /// <summary>
        /// Return whether or not the service was able to match all the tiles to the grid. 
        /// </summary>
        /// <param name="tileframeset"></param>
        /// <param name="tiles"></param>
        /// <param name="allowTransformations">Whether search allows rotations and flips (useful for simpler test cases)</param>
        /// <returns></returns>
        public bool FillTileFrameSet(TileFrameSet tileFrameSet, IEnumerable<Tile> tiles, bool allowTransformations = true, bool verbose = true)
        {
            var tileQueue = new Queue<Tile>(tiles);
            var maxIter = 100_000; // 1000000;
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
                    // let allowTransforms flag toggle simple/advanced mode

                    if (allowTransformations == false)
                    {
                        if (this.FindMatchingSide(openFrame, tile, out var side))
                        {
                            tileFrameSet.Attach(openFrame, new TileFrame(tile), side.Value);
                            foundMatch = true;

                            // collect dbg statements

                            break;
                        }
                    }
                    else
                    {
                        if (this.FindMatchingSide(openFrame, tile, out var side, out var tileWithTransforms))
                        {
                            tileFrameSet.Attach(openFrame, new TileFrame(tileWithTransforms), side.Value);
                            foundMatch = true;

                            // collect dbg statements (as needed)

                            break;
                        }
                    }
                }

                if (!foundMatch)
                {
                    tileQueue.Enqueue(tile);
                }

                if(iterCount % 1000 == 0)
                {
                    Console.WriteLine($"Checkpoint, iterations: {iterCount}. Number in set: {tileFrameSet.TileFrames.Count}, number in queue: {tileQueue.Count}");
                }

                ++iterCount;
            }

            if(iterCount >= maxIter) { throw new InvalidOperationException($"Max iterations reached in search. Num remaining in queue: {tileQueue.Count}"); }

            if(tileQueue.Count == 0)
            {
                return true;
            }
            return false;
        }

        private bool FindMatchingSide(TileFrame openFrame, Tile tile, out TileSide? side, out Tile tileWithTransforms)
        {
            // if no transforms, then behavior will match the simpler FindMatchingSide & tile in will be the same as tileWithTransforms

            // steps: 
            // with unrotated, do simpler FindMatchingCase 
            // over rotations 90, 180, 270, try simpler FindMatchingCase 
            // over flips (UD, LR), try simpler FindMatchingCase 

            tileWithTransforms = null;

            if (FindMatchingSide(openFrame, tile, out side))
            {
                tileWithTransforms = tile;
                return true;
            }

            var rotationCounts = new List<int> { 1, 2, 3 };
            foreach(var rotationNumber in rotationCounts)
            {
                var tileWithRotations = Tile.Rotate(tile, rotationNumber);

                if(FindMatchingSide(openFrame, tileWithRotations, out side))
                {
                    tileWithTransforms = tileWithRotations;
                    return true;
                }
            }

            // two flips, LR, UD

            var flippedLR = Tile.FlipLR(tile);
            if(FindMatchingSide(openFrame, flippedLR, out side))
            {
                tileWithTransforms = flippedLR;
                return true;
            }

            var flippedUD = Tile.FlipUD(tile);
            if (FindMatchingSide(openFrame, flippedUD, out side))
            {
                tileWithTransforms = flippedUD;
                return true;
            }

            // combination rotation+flip
            foreach (var rotationNumber in rotationCounts)
            {
                var tileWithRotations = Tile.Rotate(flippedUD, rotationNumber);

                if (FindMatchingSide(openFrame, tileWithRotations, out side))
                {
                    tileWithTransforms = tileWithRotations;
                    return true;
                }
            }


            return false;
        }

        // Handles simple case only; no rotations, no flips
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

        [Obsolete("Use new refactored method of the TileFrameSet instead.", true)]
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
