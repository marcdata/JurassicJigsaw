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
            Console.WriteLine($"Begin Jurassic App. Timestamp: {DateTime.Now}");

            // small sample input, 9 tiles
            // var filename = @"C:\Users\marc\source\repos\marcdata\JurassicJigsaw\inputdata\sampleinput.txt";
            // known water roughness: 273

            // the test case, ie, the problem to solve for, unknown answer.
            var filename = @"C:\Users\marc\source\repos\marcdata\JurassicJigsaw\inputdata\input.txt";

            // echo input:
            // Echo(filename);

            var solver = JurassicSolver.GetDefaultSolver();

            (var result, var cornerProduct) = solver.SolveForFile(filename);

            Console.WriteLine($"Search service organizing result: {result}");

            Console.Write($"Solved answer: {cornerProduct}");

            var tileIdsAndCoordinates = solver.GetTileFrameSet().TileFrames.Select(x => (x.TileId, x.AbsoluteLocation.x, x.AbsoluteLocation.y)).ToList();

            Console.WriteLine($"TileIds and locations:");
            tileIdsAndCoordinates.ForEach(x => Console.WriteLine($"TileId: {x.TileId}, xloc: {x.x}, yloc: {x.y}"));

            var cornerTileIds = solver.GetTileFrameSet().GetCornerTileIds();

            Console.WriteLine("Corner tile ids:");
            cornerTileIds.ForEach(x => Console.WriteLine($"TileId: {x}"));

            Console.WriteLine($"Corner product: {cornerProduct}");

            // Part 2

            var asSingleTile = solver.GetTileFrameSet().AsSingleTile();

            // do detection 

            var monsterCount = new DetectionService().CountOccurences(asSingleTile, DefaultTargetPatterns.SeaMonster(), searchRotations: true);

            Console.WriteLine($"Sea monster count: {monsterCount}");

            // count Roughness
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
