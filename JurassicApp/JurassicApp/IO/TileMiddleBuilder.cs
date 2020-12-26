using System;
using System.Collections.Generic;
using System.Linq;
using JurassicApp.Models;
using JurassicApp.Models.enums;

namespace JurassicApp.IO
{
    public class TileMiddleBuilder
    {
        private List<string> rawRows;

        public TileMiddleBuilder()
        {
            rawRows = new List<string>();
        }

        public void AddRow(string line)
        {
            rawRows.Add(line);
        }

        public Tile ToTile()
        {
            var number = int.Parse(rawRows.FirstOrDefault().Split(" ")[1].Trim(':'));
            //var contentRows = rawRows.Skip(1);
            //return new Tile(number, contentRows.ToList());

            var contentRows = new List<List<CellValue>>();
            var mapper = new CellMapper();

            // skip the "Tile 1234 type of row. 
            foreach (var rawRow in rawRows.Skip(1))
            {
                var someCells = new List<CellValue>();
                var rowtemp = rawRow.ToList().Select(x => mapper.Map(x.ToString()));
                contentRows.Add(rowtemp.ToList());
            }

            return new Tile(number, contentRows);
        }
    }
}
