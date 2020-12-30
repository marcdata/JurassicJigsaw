using JurassicApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using JurassicApp.Models.enums;

namespace JurassicApp.Services
{
    /// <summary>
    /// Help with find pattern X within tile Y type of questions.
    /// </summary>
    public class DetectionService
    {
        public DetectionService() { }

        /// <summary>
        /// Return max occurences for a rotation on the source tile. 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="searchRotations"></param>
        /// <returns></returns>
        public int CountOccurences(Tile source, Tile target, bool searchRotations = false)
        {
            if (!searchRotations)
            {
                return this.CountOccurences(source, target);
            }

            var maxMonstersFound = 0;
            var originalTile = source;

            var simpleMonstersDetected = CountOccurences(source, target);
            if(simpleMonstersDetected > 0) 
            { 
                maxMonstersFound = simpleMonstersDetected; 
            }

            // var workingTile = originalTile.GetSubsection(0, 0, originalTile.NumRows, originalTile.NumCols);

            // for each rotation, try one normal, and one flipped; should cover total space.
            for(int k = 0; k < 4; ++k)
            {
                var rotated = (k == 0) ? originalTile : Tile.Rotate(originalTile, k);

                var countInRotated = CountOccurences(rotated, target);
                if(countInRotated > maxMonstersFound) 
                { 
                    maxMonstersFound = countInRotated; 
                }

                var rotatedAndFlipped = Tile.FlipLR(rotated);

                var countInFlipped = CountOccurences(rotatedAndFlipped, target);
                if(countInFlipped > maxMonstersFound) 
                { 
                    maxMonstersFound = countInFlipped; 
                }

                // flipUD
                var rotatedAndFlippedUd = Tile.FlipUD(rotated);

                var countInFlippedUd = CountOccurences(rotatedAndFlippedUd, target);
                if (countInFlippedUd > maxMonstersFound)
                {
                    maxMonstersFound = countInFlippedUd;
                }

            }

            return maxMonstersFound;
        }

        public int CountOccurences(Tile source, Tile target)
        {
            // get source.SubMatrix of size matching target (row x col)
            // iterate over possible matching areas (row and col, less size of target)
            // detect a match, if all contents of target match source
            // alt, detect a match if, all contents of target and those contents match another filter (arg in), then that is a detection

            // outline:
            // iterate over upperleft starting coordinates (source rowsize, less target size)
            // get submatrix of the source
            // if that matches target, then a match

            return FindOccurences(source, target).Count;
        }

        /// <summary>
        /// Returns tuples of (x,y) coordinates of the upper-left corner of any detected patterns of Target within Source.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public List<(int xcoord, int ycoord)> FindOccurences(Tile source, Tile target)
        {
            var numRowsToSearch = source.NumRows - target.NumRows + 1;
            var numColsToSearch = source.NumCols - target.NumCols + 1;

            var matchingCoordinates = new List<(int xcoord, int ycoord)>();

            for(int r = 0; r < numRowsToSearch; ++r)
            {
                for(int c = 0; c < numColsToSearch; ++c)
                {
                    // get subsection of source
                    // check match

                    var subsection = source.GetSubsection(r, c, target.NumRows, target.NumCols);
                    if(MatchOnValue(subsection, target, CellValue.Pound))
                    {
                        matchingCoordinates.Add((xcoord: c, ycoord: r));
                    }
                }
            }

            return matchingCoordinates;
        }

        public bool MatchExact(Tile source, Tile comp)
        {
            var sourceAllCells = source.GetContentRows().SelectMany(x => x);
            var compAllCells = comp.GetContentRows().SelectMany(x => x);

            var zipped = sourceAllCells.Zip(compAllCells, (x, y) => (x,y));
            if(zipped.Any(z => z.x != z.y)) { return false; }
            return true;
        }

        /// <summary>
        /// Matching logic that also only requires that cells match if the value in comp equals that of the filter value.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="comp"></param>
        /// <param name="filterValue"></param>
        /// <returns></returns>
        public bool MatchOnValue(Tile source, Tile comp, CellValue filterValue)
        {
            var sourceAllCells = source.GetContentRows().SelectMany(x => x);
            var compAllCells = comp.GetContentRows().SelectMany(x => x);

            var zipped = sourceAllCells.Zip(compAllCells, (x, y) => (x, y));
            if (zipped.Where(z => z.y == filterValue).Any(z => z.x != z.y)) { return false; }
            return true;
        }
    }

    public static class DefaultTargetPatterns
    {
        public static Tile SeaMonster()
        {
            return new Tile(0, new List<string> {
                "                  # ".Replace(" ", "."),
                "#    ##    ##    ###".Replace(" ", "."),
                " #  #  #  #  #  #   ".Replace(" ", ".") });
        }
    }
}
