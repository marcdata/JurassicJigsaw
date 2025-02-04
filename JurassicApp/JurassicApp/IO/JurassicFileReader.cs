﻿using System;
using System.Collections.Generic;
using System.Text;
using JurassicApp.Models;

namespace JurassicApp.IO
{
    public interface IJurassicFileReader
    {
        public List<Tile> Read(string filename);
    }

    public class JurassicFileReader : IJurassicFileReader
    {
        public List<Tile> Read(string filename)
        {
            var tiles = new List<Tile>();
            string line;

            var tileMiddleBuilder = new TileMiddleBuilder();

            using (var filestream = new System.IO.StreamReader(filename))
            {

                while ((line = filestream.ReadLine()) != null)
                {
                    if(line != "")
                    {
                        tileMiddleBuilder.AddRow(line);
                    }
                    else
                    {
                        var tile = tileMiddleBuilder.ToTile();
                        tiles.Add(tile);

                        tileMiddleBuilder = new TileMiddleBuilder();
                    }
                }

                try
                {
                    var tile = tileMiddleBuilder.ToTile();
                    tiles.Add(tile);
                }
                catch(Exception) { 
                    // no op 
                }
            };

            return tiles;
        }

    }
    
}
