using System;
using JurassicApp.IO;
using System.Linq;

namespace JurassicApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Jurassic World!");

            var filename = @"C:\Users\marc\source\repos\marcdata\JurassicJigsaw\inputdata\input.txt";

            // echo input:
            Echo(filename);

            // TBD, do fileread; tile arrangement; calc the corner multiplier

            // steps outline: 
            // get tiles
            // arrange tiles into a TileGrid
            // select out the four corners
            // -- multiply that value out for the answer.

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
