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
            // walk thru setup and invocation for a problem solver for Jurassic App
            // handle both parts 1 & 2 of their Advent Day challenge, day 20

            Console.WriteLine($"Begin Jurassic App. Timestamp: {DateTime.Now}");

            // use sample file (known answer), or their test file
            // small sample input, 9 tiles; known water roughness: 273

            var toggleUseTestFile = false;
            var filename = toggleUseTestFile
                ? @"..\..\..\..\..\..\JurassicJigsaw\inputdata\sampleinput.txt"
                : @"..\..\..\..\..\..\JurassicJigsaw\inputdata\input.txt";

            // if filename passed in thru CLI, use that tho
            if (args.ToList().Any())
            {
                var fileArgIn = args[0];
                filename = fileArgIn;
            }

            Console.WriteLine($"Solving Jurassic tile detection for file: {filename}");

            // Part 1
            var solver = JurassicSolver.GetDefaultSolver();

            (var result, var cornerProduct) = solver.SolveForFile(filename);

            Console.WriteLine($"Search service organizing result: {result}");
            Console.WriteLine($"Solved answer: {cornerProduct}");

            var tileIdsAndCoordinates = solver.GetTileFrameSet().TileFrames.Select(x => (x.TileId, x.AbsoluteLocation.x, x.AbsoluteLocation.y)).ToList();

            Console.WriteLine($"TileIds and locations:");
            tileIdsAndCoordinates.ForEach(x => Console.WriteLine($"TileId: {x.TileId}, xloc: {x.x}, yloc: {x.y}"));

            var cornerTileIds = solver.GetTileFrameSet().GetCornerTileIds();

            Console.WriteLine("Corner tile ids:");
            cornerTileIds.ForEach(x => Console.WriteLine($"TileId: {x}"));

            Console.WriteLine($"Corner product: {cornerProduct}");

            // Part 2
            var asSingleTile = solver.GetTileFrameSet().AsSingleTile();

            // Do Detection 
            var monsterCount = new DetectionService().CountOccurences(asSingleTile, DefaultTargetPatterns.SeaMonster(), searchRotations: true);

            Console.WriteLine($"Sea monster count: {monsterCount}");

            // Count Roughness
            var waterRoughness = new RoughnessSolver().WaterRoughness(asSingleTile, monsterCount, DefaultTargetPatterns.SeaMonster());

            Console.WriteLine($"Water roughness (final answer): {waterRoughness}");

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
