using JurassicApp.Models;
using JurassicApp.Models.enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace JurassicApp.Services
{
    
    public class RoughnessSolver
    {
        public int WaterRoughness(Tile source, int monstersDetected, Tile target)
        {
            // outline: 
            // get count of source tile
            // get count of target
            // subtract count_target * monstersDetected from count_source

            var poundCount = source.NumElementsEqual(CellValue.Pound);
            var targetCount = target.NumElementsEqual(CellValue.Pound);

            return poundCount - (monstersDetected * targetCount);
        }
    }
}
