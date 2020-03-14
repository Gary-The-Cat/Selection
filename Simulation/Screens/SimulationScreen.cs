using Game.Factories;
using Game.Helpers;
using Game.SFML_Text;
using Game.ExtensionMethods;
using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;
using System.IO;
using Game.GeneticAlgorithm;

namespace Game.Screens
{
    public class SimulationScreen : Screen
    {
        private List<RectangleShape> townVisuals;

        private RenderTexture texture;

        private FontText totalDistanceString;

        private int frame = 0;

        public SimulationScreen(
            RenderWindow window,
            FloatRect configuration)
            : base(window, configuration)
        {
            this.townVisuals = new List<RectangleShape>();
            this.pathLines = new List<ConvexShape>();

            var towns = TownFactory.GetTowns();

            foreach(var town in towns)
            {
                this.townVisuals.Add(town.Shape);
            }

            // Rendering to a texture before we render to the screen allows us to save the rendered image to file.
            texture = new RenderTexture(Configuration.Width, Configuration.Height);

            totalDistanceString = new FontText(new Font("font.ttf"), "test", Color.Black, 4);
        }

        List<ConvexShape> pathLines;

        public void UpdateSequence(Neighbour neighbour)
        {
            pathLines = TownHelper.GetTownSequencePath(neighbour.Sequence);
            totalDistanceString.StringText = neighbour.GetFitness().ToString("#.##");
        }
        
        /// <summary>
        /// Draw - Here we don't update any of the components, only draw them in their current state to the screen.
        /// </summary>
        public void Draw()
        {
            if (Configuration.DrawToFile)
            {
                this.DrawToFile();
            }
            else
            {
                foreach (var pathLine in pathLines)
                {
                    window.Draw(pathLine);
                }

                foreach (var town in townVisuals)
                {
                    window.Draw(town);
                }
            }

            window.DrawString(totalDistanceString, new Vector2f(Configuration.Width / 2, 20));
        }

        private void DrawToFile()
        {
            texture.Clear(Configuration.Background);
            window.Clear();

            foreach (var pathLine in pathLines)
            {
                texture.Draw(pathLine);
            }

            foreach (var town in townVisuals)
            {
                texture.Draw(town);
            }

            texture.Display();

            if (!Directory.Exists($"C:\\Simulation\\Captures\\"))
            {
                Directory.CreateDirectory($"C:\\Simulation\\Captures\\");
            }

            texture.Texture.CopyToImage().SaveToFile($"C:\\Simulation\\Captures\\{(frame).ToString("000000")}.png");

            frame++;
            var sprite = new Sprite(texture.Texture);
            window.Draw(sprite);
        }
    }
}