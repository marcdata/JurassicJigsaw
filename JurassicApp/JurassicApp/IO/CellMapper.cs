using System;
using JurassicApp.Models.enums;

namespace JurassicApp.IO
{
    public class CellMapper
    {
        public CellMapper()
        {

        }

        public CellValue Map(string s)
        {
            if (s == "#") return CellValue.Pound;
            if (s == ".") return CellValue.Dot;
            throw new ArgumentOutOfRangeException(s);
        }

        public string Map(CellValue c)
        {
            if (c == CellValue.Pound) return "#";
            if (c == CellValue.Dot) return ".";
            throw new ArgumentOutOfRangeException(c.ToString());
        }
    }
    
}
