using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;

namespace GameOfLife
{
    class Game
    {
        // fields
        private static long step = 0;                           // game step
        private Board gameBoard;                                // game board
        private CellMap mapOne;                                 // first game map
        private CellMap mapTwo;                                 // second game map
        private MainWindow mainWindow;                          // application window
        private int drawingInterval = 100;                      // drawing interval
        private DispatcherTimer timer = new DispatcherTimer();  // display board lines
        private bool linesOn = true;


        // constructor
        public Game(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;

        }

        // build map and draw board
        internal void Initialize()
        {
            mapOne = new CellMap(100, 100);
            mapTwo = (CellMap)mapOne.Clone();
            mapOne.InitialPopulation(50);
            gameBoard = new Board(Colors.WhiteSmoke, Colors.Gray, mapOne.X, mapOne.Y);
            gameBoard.DrawLines(true);
            gameBoard.DrawPopulation(mapOne, mapTwo);
        }

        // registering events
        internal void Play()
        {
            if (drawingInterval == 0)
                drawingInterval = 100;

            this.mainWindow.KeyDown += new KeyEventHandler(Game_KeyDown);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, drawingInterval);
            timer.Start();
        }

        // calculate next map and draw it on timer tick
        private void timer_Tick(object sender, EventArgs e)
        {
            gameBoard.SetInfo(step.ToString(), this.timer.IsEnabled, drawingInterval);

            if (step % 2 == 0)
            {
                mapOne.SetNeighbors();
                Step(mapOne, mapTwo);
                gameBoard.DrawPopulation(mapTwo, mapOne);
            }
            else
            {
                mapTwo.SetNeighbors();
                Step(mapTwo, mapOne);
                gameBoard.DrawPopulation(mapOne, mapTwo);
            }
            ++step;
            CommandManager.InvalidateRequerySuggested();
        }

        // keyboard input update
        private void Game_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    {
                        Application.Current.Shutdown(0);
                        break;
                    }
                case Key.Space:
                    {
                        if (this.timer.IsEnabled)
                        {
                            this.timer.Stop();
                            gameBoard.SetInfo(step.ToString(), this.timer.IsEnabled, 0);
                        }
                        else
                            this.timer.Start();
                        break;
                    }
                case Key.Up:
                    {
                        drawingInterval /= 2;
                        if (drawingInterval == 0)
                            drawingInterval = 1;
                        this.timer.Interval = new TimeSpan(0, 0, 0, 0, drawingInterval);
                        break;
                    }
                case Key.Down:
                    {
                        if (drawingInterval <= int.MaxValue / 2)
                            drawingInterval *= 2;
                        this.timer.Interval = new TimeSpan(0, 0, 0, 0, drawingInterval);
                        break;
                    }
                case Key.L:
                    {
                        if (this.linesOn == false)
                        {
                            this.gameBoard.DrawLines(true);
                            this.linesOn = true;
                        }
                        else
                        {
                            this.gameBoard.DrawLines(false);
                            this.linesOn = false;
                        }
                        break;
                    }
                default:
                    break;
            }
        }

        // next step of the game
        private void Step(CellMap first, CellMap second)
        {
            for (int i = 0; i < first.X; ++i)
            {
                for (int j = 0; j < first.Y; ++j)
                {
                    if (first[i, j].IsAlive == false && first[i, j].Neighboors != 3)
                    {
                        second[i, j].IsAlive = false;
                    }
                    else if (first[i, j].IsAlive == true && (first[i, j].Neighboors != 2 && first[i, j].Neighboors != 3))
                    {
                        second[i, j].IsAlive = false;
                    }
                    else
                    {
                        second[i, j].IsAlive = true;
                    }
                }
            }
        }


    }
}
