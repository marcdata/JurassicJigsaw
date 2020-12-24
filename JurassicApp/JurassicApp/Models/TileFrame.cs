using JurassicApp.Models.enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace JurassicApp.Models
{
    public class TileFrame
    {
        public Tile Tile { get; set; }

        // neighbors
        public TileFrame Upper { get; set; }
        public TileFrame Lower { get; set; }
        public TileFrame Right { get; set; }
        public TileFrame Left { get; set; }

        public bool AnyOpenSides { get { return (Upper == null || Lower == null || Right == null || Left == null); } }

        public TileFrame()
        {
        }

        public TileFrame(Tile tile)
        {
            Tile = tile ?? throw new ArgumentNullException(nameof(tile));
        }

        public TileFrame GetNeighbor(TileSide side)
        {
            return side switch {
                TileSide.Upper => this.Upper,
                TileSide.Lower => this.Lower,
                TileSide.Right => this.Right,
                TileSide.Left => this.Left,
                _ => throw new ArgumentOutOfRangeException("TileSide")
            };
        }

    }

    public class TileFrameSet
    {
        public TileFrame InitialTileFrame { get; }

        public List<TileFrame> TileFrames { get; set; }

        public TileFrameSet(TileFrame initial)
        {
            InitialTileFrame = initial ?? throw new ArgumentNullException(nameof(initial));

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
