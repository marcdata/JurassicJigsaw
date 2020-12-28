using JurassicApp.Models.enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JurassicApp.Models
{
    public class TileFrameSet
    {
        public TileFrame InitialTileFrame { get; }

        public List<TileFrame> TileFrames { get; set; }

        public TileFrameSet(TileFrame initial)
        {
            InitialTileFrame = initial ?? throw new ArgumentNullException(nameof(initial));

            InitialTileFrame.AbsoluteLocation = (0, 0);

            TileFrames = new List<TileFrame>();
            TileFrames.Add(initial);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="existingTileFrame">The TileFrame already in the Set.</param>
        /// <param name="newTileFrame">The new TileFrame to add.</param>
        /// <param name="side">Side of the existingTimeFrame we are adding onto</param>
        public void Attach(TileFrame existingTileFrame, TileFrame newTileFrame, TileSide side)
        {
            // 1) set neighbor of the existingTileFrame
            // 2) set the AbsoluteLocation on the new TileFrame

            SetNeighbor(existingTileFrame, newTileFrame, side);
            // attach to TileFrames
            TileFrames.Add(newTileFrame);

            // propagate absolute location from known openFrame item to new TileFrame
            newTileFrame.AbsoluteLocation = existingTileFrame.GetNeighborLocation(side);
        }

        private void SetNeighbor(TileFrame openFrame, TileFrame newFrame, TileSide side)
        {
            if (side == TileSide.Upper && openFrame.Upper == null)
            {
                openFrame.Upper = newFrame;
            }
            else if (side == TileSide.Lower && openFrame.Lower == null)
            {
                openFrame.Lower = newFrame;
            }
            else if (side == TileSide.Right && openFrame.Right == null)
            {
                openFrame.Right = newFrame;
            }
            else if (side == TileSide.Left && openFrame.Left == null)
            {
                openFrame.Left = newFrame;
            }
            else
            {
                throw new InvalidOperationException("Error attaching tile; tile probably already in use.");
            }

        }


        public TileFrame GetUpperRightCorner()
        {
            var tileUR = this.TileFrames
                .OrderByDescending(z => z.AbsoluteLocation.x)
                .OrderByDescending(z => z.AbsoluteLocation.y)
                .FirstOrDefault();

            return tileUR;
        }

        public TileFrame GetUpperLeftCorner()
        {
            var tileUR = this.TileFrames
                .OrderBy(z => z.AbsoluteLocation.x)
                .OrderByDescending(z => z.AbsoluteLocation.y)
                .FirstOrDefault();

            return tileUR;
        }

        public TileFrame GetLowerLeftCorner()
        {
            var tileUR = this.TileFrames
                .OrderBy(z => z.AbsoluteLocation.x)
                .OrderBy(z => z.AbsoluteLocation.y)
                .FirstOrDefault();

            return tileUR;
        }

        public TileFrame GetLowerRightCorner()
        {
            var tileUR = this.TileFrames
                .OrderByDescending(z => z.AbsoluteLocation.x)
                .OrderBy(z => z.AbsoluteLocation.y)
                .FirstOrDefault();

            return tileUR;
        }

        // ... something not right here, bc we aren't attaching every tile to all the neighbors on add.

        //public TileFrame GetUpperLeftCornerByWalking()
        //{
        //    var currTile = this.InitialTileFrame;

        //    while (currTile.Left != null)
        //    {
        //        currTile = currTile.Left;
        //    }

        //    while (currTile.Upper != null)
        //    {
        //        currTile = currTile.Upper;
        //    }

        //    return currTile;
        //}

        //public TileFrame GetUpperRightCornerByWalking()
        //{
        //    var currTile = this.InitialTileFrame;

        //    while (currTile.Right != null)
        //    {
        //        currTile = currTile.Right;
        //    }
        //    while (currTile.Upper != null)
        //    {
        //        currTile = currTile.Upper;
        //    }
        //    return currTile;
        //}

        //public TileFrame GetLowerLeftCornerByWalking()
        //{
        //    var currTile = this.InitialTileFrame;

        //    while (currTile.Left != null)
        //    {
        //        currTile = currTile.Left;
        //    }

        //    while (currTile.Lower != null)
        //    {
        //        currTile = currTile.Lower;
        //    }

        //    return currTile;

        //}

        //public TileFrame GetLowerRightCornerByWalking()
        //{
        //    var currTile = this.InitialTileFrame;

        //    while (currTile.Right != null)
        //    {
        //        currTile = currTile.Right;
        //    }
        //    while (currTile.Lower != null)
        //    {
        //        currTile = currTile.Lower;
        //    }
        //    return currTile;

        //}

        public List<int> GetCornerTileIds()
        {
            var ul = this.GetUpperLeftCorner();
            var ur = this.GetUpperRightCorner();
            var ll = this.GetLowerLeftCorner();
            var lr = this.GetLowerRightCorner();

            return new List<int> { ul.Tile.TileNumber, ur.Tile.TileNumber, ll.Tile.TileNumber, lr.Tile.TileNumber };
        }
        
        /// <summary>
        /// For Part 2; stitch together tiles, so a TileFrameSet of 3x3 tiles (size each as n x n) becomes a single tile (with grid as 3n x 3n).
        /// </summary>
        /// <returns></returns>
        public Tile AsSingleTile()
        {
            // outline: 
            // iterate over all tiles
            //   - by row in the TileFrameSet
            //   - "deframe" every tile
            //   - concatenate rows in each tile
            //   - track seprate indexes; SuperRows in TileFrameSet, and SubRows (per tile)
            // export as new tile with id = 0

            var newContent = new List<List<CellValue>>();

            var rowsPerTile = this.InitialTileFrame.Tile.NumRows;

            (var ymin, var ymax) = this.GetAbsoluteBoundsOnRows();
            var yrange = Enumerable.Range(ymin, (ymax - ymin)+1).OrderByDescending(x => x).ToList();

            // read each "super row" once, and then done with it.
            // just go vertically top-to-bottom; handle the super-row to sub-row different in data representation.
            // SuperRow -> Tiles -> rowdata -> Concatenate back to new collection.
            // (Otherwise, we'd revisit each Tile multiple times to handle slices at the subrow level.)
            foreach(var superRowAbsoluteY in yrange)
            {
                var tilesInRow = this.TilesForSuperRow(superRowAbsoluteY);
                var subsetContentRows = this.ConcatenateTiles(tilesInRow.Select(x => x.Tile.DeFramed()).ToList());

                foreach(var contentRow in subsetContentRows)
                {
                    newContent.Add(contentRow);
                }
            }
            
            return new Tile(0, newContent);
        }

        (int ymin, int ymax) GetAbsoluteBoundsOnRows()
        {
            // could reduce a few iterations here, but shouldn't really matter.

            var ymin = TileFrames.Min(x => x.AbsoluteLocation.y);
            var ymax = TileFrames.Max(x => x.AbsoluteLocation.y);
            return (ymin, ymax);
        }

        (int xmin, int xmax) GetAbsoluteBoundsOnCols()
        {
            var xmin = TileFrames.Min(x => x.AbsoluteLocation.x);
            var xmax = TileFrames.Max(x => x.AbsoluteLocation.x);
            return (xmin, xmax);
        }

        private List<TileFrame> TilesForSuperRow(int superRowAbsoluteY)
        {
            return TileFrames
               .Where(x => x.AbsoluteLocation.y == superRowAbsoluteY)
               .OrderBy(x => x.AbsoluteLocation.x).ToList();
        }


        /// <summary>
        /// Concatenate tiles into raw contents; concatenate in order as left-to-right.
        /// Leave as lower level data, to support eventual row-wise concatenation.
        /// </summary>
        /// <param name="tiles">Tiles should be in left-to-right order (sorted by AbsoluteLocation.X, within the TileFrameSet).</param>
        /// <returns></returns>
        private List<List<CellValue>> ConcatenateTiles(List<Tile> tiles)
        {
            var newRows = new List<List<CellValue>>();
            var numRowsPerTile = tiles.FirstOrDefault()?.NumRows ?? 0;
            // init newRows
            for(int a = 0; a < numRowsPerTile; ++a)
            {
                newRows.Add(new List<CellValue>());
            }

            foreach(var tile in tiles)
            {
                var newRowIndex = 0;
                foreach(var row in tile.GetContentRows())
                {
                    newRows[newRowIndex].AddRange(row);
                    newRowIndex++;
                }
            }

            return newRows;

        }

        //private class LocationReader
        //{
        //    private Dictionary<(int, int), TileFrame> _tileFramesByLocation;

        //    public LocationReader(List<TileFrame> tileFrames)
        //    {
        //        _tileFramesByLocation = tileFrames.ToDictionary(x => x.AbsoluteLocation);
        //    }

        //    public TileFrame GetByLocation(int row, int col)
        //    {
        //        return _tileFramesByLocation[(row, col)];
        //    }

        //}
    }
}
