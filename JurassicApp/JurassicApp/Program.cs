using System;
using JurassicApp.IO;
using System.Linq;
using JurassicApp.Models;
using JurassicApp.Services;

namespace JurassicApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Jurassic World!");

            // var filename = @"C:\Users\marc\source\repos\marcdata\JurassicJigsaw\inputdata\input.txt";
            var filename = @"C:\Users\marc\source\repos\marcdata\JurassicJigsaw\inputdata\sampleinput.txt";

            // echo input:
            // Echo(filename);

            // TBD, do fileread; tile arrangement; calc the corner multiplier

            // steps outline: 
            // get tiles
            // arrange tiles into a TileGrid
            // select out the four corners
            // -- multiply that value out for the answer.

            var arbitrarySkipCount = 0;


            // this is off by one. On sample data is 8, should be 9.
            var tileInput = new JurassicFileReader().Read(filename);

            var initialTile = tileInput.Skip(arbitrarySkipCount).FirstOrDefault();

            tileInput.Remove(initialTile);

            var initialTileFrame = new TileFrame(initialTile);
            var tileFrameSet = new TileFrameSet(initialTileFrame);

            var searchService = new TileFrameSearchService();

            var result = searchService.FillTileFrameSet(tileFrameSet, tileInput, allowTransformations: true);

            Console.WriteLine($"Search service organizing result: {result}");

            var cornerTileIds = tileFrameSet.GetCornerTileIds();

            Console.WriteLine("Corner tile ids:");
            cornerTileIds.ForEach(x => Console.WriteLine($"TileId: {x}"));
        }

        public static void Echo(string filename)
        {
            var fileReader = new JurassicFileReader();

            var inputTiles = fileReader.Read(filename);

            foreach(var tile in inputTiles)
            {
                Console.WriteLine($"Tile: {tile.TileNumber}");
            }

            // last Tile

            Console.WriteLine($"Last Tile:");
            Console.WriteLine(inputTiles.LastOrDefault().ToString());
        }
    }
}
