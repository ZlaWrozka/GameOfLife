using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameOfLife
{
    class CellMap : ICloneable
    {
        // fields
        private Cell[,] map;                    // cell map
        private const int MAX_X = 100;          // max x size
        private const int MAX_Y = 100;          // max y size
        private const int MIN_X = 10;           // min x size
        private const int MIN_Y = 10;           // max y size
        private int x;                          // x size
        private int y;                          // y size
        private int initialPopulationSize;      // initial population size

        // properties
        public int X
        {
            get { return x; }
            set
            {
                if (value >= MIN_X && value <= MAX_X)
                    x = value;
                else
                    x = MAX_X;
            }
        }
        public int Y
        {
            get { return y; }
            set
            {
                if (value >= MIN_Y && value <= MAX_Y)
                    y = value;
                else
                    y = MAX_Y;
            }
        }
        public int PopulationSize
        {
            set
            {
                if (value > X - (0.5 * X))
                    initialPopulationSize = (int)(X - (0.5 * X));
                else
                    initialPopulationSize = value;
            }
        }

        // constructors
        public CellMap() { }
        public CellMap(int x, int y)
        {
            this.X = x;
            this.Y = y;
            map = new Cell[X, Y];
            for (int i = 0; i < X; ++i)
            {
                for (int j = 0; j < Y; ++j)
                {
                    map[i, j] = new Cell();
                }
            }
        }

        // randomly choose initial population for given population size
        public void InitialPopulation(int size)
        {
            PopulationSize = size;
            Random random = new Random();

            for (int x = (int)(X / 2.0) - this.initialPopulationSize; x < (int)(X / 2.0) + this.initialPopulationSize; ++x)
            {
                for (int y = (int)(Y / 2.0) - this.initialPopulationSize; y < (int)(Y / 2.0) + this.initialPopulationSize; ++y)
                {
                    int number = random.Next();
                    if (number % 2 == 1)
                    {
                        map[x, y].IsAlive = true;
                    }
                }
            }
        }

        // set neighboors count for the map
        public void SetNeighbors()
        {
            for (int x = 0; x < X; ++x)
            {
                for (int y = 0; y < Y; ++y)
                {
                    map[x, y].Neighboors = CountNeighbors(x, y);
                }
            }
        }

        // count neighboors of single organism
        private byte CountNeighbors(int x, int y)
        {
            byte count = 0;

            // left 
            if (map[(x - 1 + X) % X, y].IsAlive)
            {
                count++;
            }
            // right 
            if (map[(x + 1) % X, y].IsAlive)
            {
                count++;
            }
            // upper
            if (map[x, (y - 1 + Y) % Y].IsAlive)
            {
                count++;
            }
            // bottom
            if (map[x, (y + 1) % Y].IsAlive)
            {
                count++;
            }
            // corners
            // upper left
            if (map[(x - 1 + X) % X, (y - 1 + Y) % Y].IsAlive)
            {
                count++;
            }
            // bottom left
            if (map[(x - 1 + X) % X, (y + 1) % Y].IsAlive)
            {
                count++;
            }
            // upper right
            if (map[(x + 1) % X, (y - 1 + Y) % Y].IsAlive)
            {
                count++;
            }
            // bottom right
            if (map[(x + 1) % X, (y + 1) % Y].IsAlive)
            {
                count++;
            }

            return count;
        }

        // deep copy of a map
        public object Clone()
        {
            CellMap newMap = (CellMap)this.MemberwiseClone();
            newMap.map = new Cell[X, Y];
            for (int i = 0; i < X; ++i)
            {
                for (int j = 0; j < Y; ++j)
                {
                    newMap.map[i, j] = new Cell();
                    newMap.map[i, j].IsAlive = this.map[i, j].IsAlive;
                    newMap.map[i, j].Neighboors = this.map[i, j].Neighboors;
                }
            }
            return newMap;
        }

        // indexer (enables calling a field of a map like: map_name[i][j])
        public Cell this[int x, int y]
        {
            get { return (Cell)map[x, y]; }
            set { map[x, y] = value; }
        }
    }
}
