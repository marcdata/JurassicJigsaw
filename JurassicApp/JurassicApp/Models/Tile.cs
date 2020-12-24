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

        private List<string> RawRows { get; set; }

        private List<List<CellValue>> ContentRows { get; set; }

        /* A tile really has:
         * as number (id, int)
         * four line values (upper, left, right, lower)
         * hidden, internal data, whatever raw strings were used to create it.
         * 
         * according to instructions, probably have to support rotations, flipping, etc
         */

        [Obsolete("Remove if not using.")]
        public Tile()
        {

        }

        public Tile(int tileNumber)
        {
            TileNumber = tileNumber;
        }

        public Tile(int tileNumber, List<string> rawRows)
        {
            TileNumber = tileNumber;
            RawRows = rawRows ?? throw new ArgumentNullException(nameof(rawRows));

            // need also read these into our normal ContentRows

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
    }

    public static class TileExtensions
    {
        public static bool Match(this List<CellValue> lhs, List<CellValue> rhs)
        {
            var len = lhs.Count;
            for(int k = 0; k < len; k++)
            {
                if(lhs[k] != rhs[k]) { return false; }
            }
            return true;
        }
    }
}
