using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GameOfLife
{
    class Cell
    {
        // fields
        public static Color deadCol = Colors.WhiteSmoke;    // dead cells color
        public static Color livingCol = Colors.Green;       // living cells color

        // properties
        public byte Neighboors { get; set; }                // cells neighboors
        public bool IsAlive { get; set; }                   // is the cell currently alive
        public static Color DeadCol
        {
            get { return deadCol; }
            set
            {
                if (value != LivingCol)
                    deadCol = value;
            }
        }                       
        public static Color LivingCol
        {
            get { return livingCol; }
            set
            {
                if (value != DeadCol)
                    livingCol = value;
            }
        }

        // contructors
        public Cell() { IsAlive = false; Neighboors = 0; }
    }
}
