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

        public TileFrame GetUpperLeftCorner()
        {
            var currTile = this.InitialTileFrame;

            while(currTile.Left != null)
            {
                currTile = currTile.Left;
            }

            while(currTile.Upper != null)
            {
                currTile = currTile.Upper;
            }

            return currTile;
        }

        public TileFrame GetUpperRightCorner()
        {
            var currTile = this.InitialTileFrame;

            while(currTile.Right != null)
            {
                currTile = currTile.Right;
            }
            while(currTile.Upper != null)
            {
                currTile = currTile.Upper;
            }
            return currTile;
        }

        public TileFrame GetUpperRightCorner_Alt()
        {
            var tileUR = this.TileFrames
                .OrderByDescending(z => z.AbsoluteLocation.x)
                .OrderByDescending(z => z.AbsoluteLocation.y)
                .FirstOrDefault();

            return tileUR;
        }

        public TileFrame GetLowerLeftCorner()
        {
            var currTile = this.InitialTileFrame;

            while (currTile.Left != null)
            {
                currTile = currTile.Left;
            }

            while (currTile.Lower != null)
            {
                currTile = currTile.Lower;
            }

            return currTile;

        }

        public TileFrame GetLowerRightCorner()
        {
            var currTile = this.InitialTileFrame;

            while (currTile.Right != null)
            {
                currTile = currTile.Right;
            }
            while (currTile.Lower != null)
            {
                currTile = currTile.Lower;
            }
            return currTile;

        }

        public List<int> GetCornerTileIds()
        {
            var ul = this.GetUpperLeftCorner();
            var ur = this.GetUpperRightCorner();
            var ll = this.GetLowerLeftCorner();
            var lr = this.GetLowerRightCorner();

            return new List<int> { ul.Tile.TileNumber, ur.Tile.TileNumber, ll.Tile.TileNumber, lr.Tile.TileNumber };
        }
        
    }
}
