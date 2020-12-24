using System;
using System.Collections.Generic;
using System.Text;
using JurassicApp.Models;
using JurassicApp.Models.enums;
using JurassicApp.Services;
using Xunit;

namespace XUnitTests
{
    public class ComparisonLogicTests
    {
        [Fact]
        public void CheckExposuresMatch_true()
        {
            // arr
            var exposureRhs = new List<CellValue> { CellValue.Dot, CellValue.Dot, CellValue.Dot };
            var exposureLhs = new List<CellValue> { CellValue.Dot, CellValue.Dot, CellValue.Dot };

            // act
            var matchResult = exposureRhs.Match(exposureLhs);

            // assert
            Assert.True(matchResult);
        }

        [Fact]
        public void CheckExposuresMatch_false()
        {
            // arr
            var exposureRhs = new List<CellValue> { CellValue.Dot, CellValue.Dot, CellValue.Dot };
            var exposureLhs = new List<CellValue> { CellValue.Dot, CellValue.Dot, CellValue.Pound };

            // act 
            var matchResult = exposureRhs.Match(exposureLhs);

            // assert
            Assert.False(matchResult);

        }

    }
}
