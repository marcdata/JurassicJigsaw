using JurassicApp.Models.enums;
using System;
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

        /// <summary>
        /// Pass-thru TileId. (For easier tracking across layers.)
        /// </summary>
        public int TileId { get
            {
                return this.Tile.TileNumber;
            } }

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
}
