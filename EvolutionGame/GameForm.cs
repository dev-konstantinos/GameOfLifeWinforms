using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EvolutionGame
{
    /// <summary>
    /// Evolution class represents the main form of the game, handling user interactions and game logic
    /// </summary>
    public partial class GameForm : Form
    {
        private Graphics graphics;
        private int resolution;
        private GameEngine gameEngine;

        public GameForm()
        {
            InitializeComponent();
            this.Load += FormEvolutionLoad;
        }
        /// <summary>
        /// Starts the game by initializing the game engine, disabling controls, and starting the timer for game updates
        /// </summary>
        private void StartGame()
        {
            if (timerMain.Enabled)
                return;

            nudResolution.Enabled = false;
            nudDensity.Enabled = false;
            nudSpeed.Enabled = false;
            buttonStart.Enabled = false;

            resolution = (int)nudResolution.Value;

            gameEngine = new GameEngine
                (
                rows: pictureBoxMain.Height / resolution,
                cols: pictureBoxMain.Width / resolution,
                (int)nudDensity.Value
                );

            Text = $"Generation: {gameEngine.CurrentGeneration}";

            pictureBoxMain.Image = new Bitmap(pictureBoxMain.Width, pictureBoxMain.Height);
            graphics = Graphics.FromImage(pictureBoxMain.Image);

            timerMain.Start();
        }
        /// <summary>
        /// Draws the next generation of cells in the game by clearing the graphics context and filling rectangles for live cells
        /// </summary>
        private void DrawNextGen()
        {
            graphics.Clear(Color.Black);

            bool[,] field = gameEngine.GetCurrentGeneration();
            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    if (field[x, y])
                        graphics.FillRectangle(Brushes.Crimson, x * resolution, y * resolution, resolution - 1, resolution - 1);
                }
            }
            pictureBoxMain.Refresh();
            Text = $"Generation: {gameEngine.CurrentGeneration}";
            gameEngine.NextGen();
        }
        /// <summary>
        /// Stops the game and resets the controls to allow for a new game to be started
        /// </summary>
        private void StopGame()
        {
            if (!timerMain.Enabled)
                return;

            timerMain.Stop();

            nudResolution.Enabled = true;
            nudDensity.Enabled = true;
            nudSpeed.Enabled = true;
            buttonStart.Enabled = true;
        }
        /// <summary>
        /// Handles the speed adjustment for the game timer based on user input from the numeric up-down control
        /// </summary>
        private void NudSpeed_ValueChanged(object sender, EventArgs e)
        {
            timerMain.Interval = (int)nudSpeed.Value;
        }
        /// <summary>
        /// Handles the timer tick event to draw the next generation of cells in the game
        /// </summary>
        private void timerMain_Tick(object sender, EventArgs e)
        {
            DrawNextGen();
        }
        /// <summary>
        /// Handles the start button click event to initialize and start the game
        /// </summary>
        private void buttonStart_Click(object sender, EventArgs e)
        {
            StartGame();
        }
        /// <summary>
        /// Handles the pause button click event to stop the game and reset controls
        /// </summary>
        private void buttonPause_Click(object sender, EventArgs e)
        {
            StopGame();
        }
        /// <summary>
        /// Handles mouse movement on the picture box to add or remove cells based on mouse clicks
        /// </summary>
        private void pictureBoxMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (!timerMain.Enabled)
                return;

            if (e.Button == MouseButtons.Left)
            {
                int x = e.Location.X / resolution;
                int y = e.Location.Y / resolution;
                gameEngine.AddCell(x, y);
            }
            if (e.Button == MouseButtons.Right)
            {
                int x = e.Location.X / resolution;
                int y = e.Location.Y / resolution;
                gameEngine.RemoveCell(x, y);
            }
        }
        /// <summary>
        /// Initializes the form and sets default values for controls
        /// </summary>
        private void FormEvolutionLoad(object sender, EventArgs e)
        {
            nudResolution.Value = 5;
            nudDensity.Value = 5;
            nudSpeed.Value = 100;
            timerMain.Interval = (int)nudSpeed.Value;
            nudSpeed.ValueChanged += NudSpeed_ValueChanged;
        }

    }
}
