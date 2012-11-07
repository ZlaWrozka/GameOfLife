using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameOfLife
{
    class Point
    {
        public Point() { }
        public Point(int XPos, int YPos)
        {
            this.X = XPos;
            this.Y = YPos;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }
}
