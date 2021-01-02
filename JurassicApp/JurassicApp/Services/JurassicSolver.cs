using JurassicApp.IO;
using JurassicApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JurassicApp.Services
{
    public interface IJurassicSolver
    {
        public (bool result, long cornerProduct) SolveForFile(string filename, bool verbose = true);
    }

    /// <summary>
    /// Class to support solving the problem re input to final answer (file, to multiple of the corner tile ids).
    /// Coordinates between the FileReader, and the SearchService. 
    /// Verbose output, suppressable. 
    /// 
    /// In the end, this only solves for Part I of the challenge. 
    /// </summary>
    public class JurassicSolver : IJurassicSolver
    {
        IJurassicFileReader _jurassicFileReader;
        ITileFrameSearchService _tileFrameSearchService;

        /// <summary>
        /// TileFrameSet of the last set we solved for. Convenience method for state-checking after run.
        /// </summary>
        TileFrameSet _tileFrameSet;

        public JurassicSolver(IJurassicFileReader fileReader, ITileFrameSearchService tileFrameSearchService)
        {
            _jurassicFileReader = fileReader ?? throw new ArgumentNullException(nameof(fileReader));
            _tileFrameSearchService = tileFrameSearchService ?? throw new ArgumentNullException(nameof(tileFrameSearchService));
        }

        public (bool result, long cornerProduct) SolveForFile(string filename, bool verbose = true)
        {
            // read tiles from file
            var tiles = _jurassicFileReader.Read(filename);

            // init frame, framset 
            var arbitrarySkipCount = 0; // modify this for different initial conditions, in case that matters

            var initialTile = tiles.Skip(arbitrarySkipCount).FirstOrDefault();

            tiles.Remove(initialTile);

            var initialTileFrame = new TileFrame(initialTile);
            _tileFrameSet = new TileFrameSet(initialTileFrame);

            // solve the search, arrangement part
            var searchSuccess = _tileFrameSearchService.FillTileFrameSet(_tileFrameSet, tiles, allowTransforms: true, verbose: verbose);

            if (!searchSuccess) return (false, 0);

            // get product from cornerids
            long accumulateProduct = 1;
            foreach(var cornerId in _tileFrameSet.GetCornerTileIds())
            {
                accumulateProduct *= cornerId;
            }

            return (true, accumulateProduct);
        }

        public TileFrameSet GetTileFrameSet()
        {
            return _tileFrameSet;
        }

        /// <summary>
        /// Factory type method to get a solver, with DI for needed params. 
        /// </summary>
        /// <returns></returns>
        public static JurassicSolver GetDefaultSolver()
        {
            // Since we don't have a full DI service depency thing here, do some approximation to handle providing dependencies. 

            return new JurassicSolver(
                new JurassicFileReader(),
                new TileFrameSearchService()
                );
        }
    }
}
