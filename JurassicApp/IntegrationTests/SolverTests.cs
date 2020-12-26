using System;
using System.Collections.Generic;
using System.Text;
using JurassicApp.Services;
using Xunit;

namespace IntegrationTests
{

    /// <summary>
    /// Solve solutions for actual sample files (one small sample file, one problem test file).
    /// Ie, end-to-end test.
    /// </summary>
    public class SolverTests
    {
        [Fact]
        public void DoSmallSample()
        {
            var filename = @"C:\Users\marc\source\repos\marcdata\JurassicJigsaw\inputdata\sampleinput.txt";

            var solver = JurassicSolver.GetDefaultSolver();

            (var result, var cornerProduct) = solver.SolveForFile(filename);

            // Check that we match provided answer from the problem site.
            Assert.Equal(20899048083289, cornerProduct);
        }

        [Fact]
        public void SolveForTheChallenge()
        {
            var filename = @"C:\Users\marc\source\repos\marcdata\JurassicJigsaw\inputdata\input.txt";

            var solver = JurassicSolver.GetDefaultSolver();

            (var result, var cornerProduct) = solver.SolveForFile(filename);

            // No assertions, just check output. 

            _ = 0;
        }
    }
}
