using JurassicApp.IO;
using JurassicApp.Models.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JurassicApp.Models
{
    public class Tile
    {
        public int TileNumber { get; private set; }

        private List<List<CellValue>> ContentRows { get; set; }

        /* A tile really has:
         * as number (id, int)
         * four line values (upper, left, right, lower)
         * hidden, internal data, whatever raw strings were used to create it.
         * 
         * according to instructions, probably have to support rotations, flipping, etc
         */

        //[Obsolete("Remove if not using.")]
        //public Tile()
        //{

        //}

        public Tile(int tileNumber)
        {
            TileNumber = tileNumber;
        }

        public Tile(int tileNumber, List<string> rawRows)
        {
            TileNumber = tileNumber;
            //RawRows = rawRows ?? throw new ArgumentNullException(nameof(rawRows));

            var mapper = new CellMapper();

            ContentRows = new List<List<CellValue>>();

            foreach(var row in rawRows)
            {
                var contentsRow = row.Select(x => mapper.Map(x.ToString())).ToList();
                ContentRows.Add(contentsRow);
            }
        }

        public Tile(int tileNumber, List<List<CellValue>> contentRows)
        {
            TileNumber = tileNumber;
            ContentRows = contentRows ?? throw new ArgumentNullException(nameof(contentRows));

            // set other values, Upper, Lower, Left, Right... 
        }


        // Exposures: Upper and Lower orient from Left to Right;
        // Left and Right, orient from top to bottom

        public List<CellValue> RightExposure
        {
            get
            {
                return ContentRows.Select(r => r.Last()).ToList();
            }
        }

        /// <summary>
        /// Get subsection as separate Tile. 
        /// R and C count rows, cols from upper left corner.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="c"></param>
        /// <param name="numRows"></param>
        /// <param name="numCols"></param>
        /// <returns></returns>
        public Tile GetSubsection(int r, int c, int numRows, int numCols)
        {
            // consider bounds check 

            var rows = this.ContentRows.Skip(r).Take(numRows);

            var contentsOut = new List<List<CellValue>>();

            foreach(var row in rows)
            {
                var newRow = row.Skip(c).Take(numCols).ToList();
                contentsOut.Add(newRow);
            }

            return new Tile(0, contentsOut);
            
        }

        /// <summary>
        /// Left exposure, in order of top-down
        /// </summary>
        public List<CellValue> LeftExposure
        {
            get
            {
                return ContentRows.Select(r => r.First()).ToList();
            }
        }

        public List<CellValue> TopExposure
        {
            get
            {
                return ContentRows.First().Select(x => x).ToList();
            }
        }

        public List<CellValue> LowerExposure
        {
            get
            {
                return ContentRows.Last().Select(x => x).ToList();
            }
        }

        public int NumRows
        {
            get
            {
                return ContentRows.Count();
            }
        }

        /// <summary>
        /// Number of columns in the first row (elements in first row).
        /// </summary>
        public int NumCols
        {
            get
            {
                return ContentRows.FirstOrDefault()?.Count() ?? 0;
            }
        }

        public int NumElementsEqual(CellValue comparisonValue)
        {
            return ContentRows.SelectMany(x => x).Where(y => y == comparisonValue).Count();
        }

        /// <summary>
        /// Return a new Tile, with the contents of the source tile, with the outer edges removed.
        /// </summary>
        /// <returns></returns>
        public Tile DeFramed()
        {
            // outline: 
            // for rows (except first, and last row)
            // for columns (except first and last element)
            // pipe to output

            List<List<CellValue>> newContents = new List<List<CellValue>>();
            var rowLen = this.ContentRows.Count;

            foreach(var row in ContentRows.Skip(1).Where (x => x != ContentRows.LastOrDefault()))
            {
                var newRow = row.Skip(1).Take(rowLen - 2).ToList();
                newContents.Add(newRow);
            }

            return new Tile(this.TileNumber, newContents);
        }

        public override string ToString()
        {
            var mapper = new CellMapper();
            var sb = new StringBuilder();
            foreach(var row in ContentRows)
            {
                var lineSb = new StringBuilder();

                foreach(var elem in row)
                {
                    lineSb.Append(mapper.Map(elem));
                }

                sb.Append(lineSb.ToString());
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public static Tile Copy(Tile tile)
        {
            var newContent = new List<List<CellValue>>();

            foreach (var row in tile.ContentRows)
            {
                newContent.Add(row.Select(x => x).ToList());
            }
            return new Tile(tile.TileNumber, newContent);
        }



        /// <summary>
        /// Do one rotation, counter clockwise on a tile.
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        public static Tile Rotate(Tile tile)
        {

            // return back the tile with a 1 quarter counter-clockwise rotation performed on it.
            //
            // eg
            // A B
            // C D
            //
            // rotated is
            //
            // B D 
            // A C 

            var numCols = tile.ContentRows.Count;

            // over each row, find the xth element, read that into a new list
            // read from rightmost to left most columns
            // each of those columns becomes the next row in the new ContentRows;

            var contentRows = new List<List<CellValue>>();

            for (int j = numCols - 1; j > -1; --j)
            {
                var newRow = new List<CellValue>();
                foreach (var originalRow in tile.ContentRows)
                {
                    newRow.Add(originalRow[j]);
                }
                contentRows.Add(newRow);
            }

            return new Tile(tile.TileNumber, contentRows);
        }


        public static Tile Rotate(Tile tile, int rotationNumber)
        {
            if (rotationNumber < 1 || rotationNumber > 3) { throw new ArgumentOutOfRangeException(nameof(rotationNumber)); }

            Tile tileOut = tile;

            for (int k = rotationNumber; k > 0; --k)
            {
                tileOut = Tile.Rotate(tileOut);
            }

            return tileOut;
        }

        public static Tile FlipUD(Tile tile)
        {
            var newContents = new List<List<CellValue>>();
            var numElem = tile.ContentRows.Count;
            // initialize newContents with empty rows
            for(int j = 0; j < numElem; ++j)
            {
                newContents.Add(new List<CellValue>());
            }

            // go col by col, left to right; 
            // read vertically down; reverse it (vertically)
            // then append it to the right side of all rows in the newContents

            for(int k = 0; k < numElem; ++k)
            {
                var col = new List<CellValue>();
                foreach (var row in tile.ContentRows)
                {
                    col.Add(row[k]);
                }
                col.Reverse();
                AppendRight(newContents, col);
            }

            return new Tile(tile.TileNumber, newContents);
        }

        /// <summary>
        /// Expose internal data for re-structuring of data within the TileFrameSet.
        /// </summary>
        /// <returns></returns>
        internal IReadOnlyList<IReadOnlyList<CellValue>> GetContentRows()
        {
            return this.ContentRows;
        }

        private static void AppendRight(List<List<CellValue>> newContents, List<CellValue> col)
        {
            var colItemIndex = 0;
            foreach(var row in newContents)
            {
                row.Add(col[colItemIndex]);
                colItemIndex++;
            }
        }

        public static Tile FlipLR(Tile tile)
        {
            var newContents = new List<List<CellValue>>();

            foreach(var originalrow in tile.ContentRows)
            {
                var newRow = originalrow.ToList();
                newRow.Reverse();
                newContents.Add(newRow);
            }
            return new Tile(tile.TileNumber, newContents);
        }
    }

    public static class TileExtensions
    {
        public static bool Match(this List<CellValue> lhs, List<CellValue> rhs)
        {
            if(lhs.Count != rhs.Count) { throw new InvalidOperationException($"Lhs and Rhs have different lengths. {lhs.Count}, {rhs.Count}"); }

            var len = lhs.Count;
            for(int k = 0; k < len; k++)
            {
                if(lhs[k] != rhs[k]) { return false; }
            }
            return true;
        }
    }
}
