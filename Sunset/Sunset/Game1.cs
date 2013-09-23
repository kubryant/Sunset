using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Sunset
{
    public class Ball
    {
        public Texture2D ball;
        public Vector2 ballPosition = Vector2.Zero;
        public int verticalSpeed;
        public int horizontalSpeed;
        public float barY;
        public float ballHeight;
        public float width;
        Random rnd = new Random();
        public Ball()
        {
            verticalSpeed = 7;
            horizontalSpeed = 7;
        }
        public void update()
        {
            ballPosition.X += horizontalSpeed;
            ballPosition.Y += verticalSpeed;
        }
        public void hitWall()
        {
            if (ballPosition.X <= 0)
                ballPosition.X = 1;
            else
                ballPosition.X = width - 1 - ballHeight;
            
            horizontalSpeed *= -1;

            if (horizontalSpeed < 0)
                horizontalSpeed = rnd.Next(-9, -6);
            else
                horizontalSpeed = rnd.Next(6, 9);
        }
        public void hitBar()
        {
            ballPosition.Y = barY - 1 - ballHeight;
            verticalSpeed *= -1;

            if (verticalSpeed < 0)
                verticalSpeed = rnd.Next(-9, -6);
            else
                verticalSpeed = rnd.Next(6, 9);
        }
        public void setHeight(float bar, float ballH, float wid)
        {
            barY = bar;
            ballHeight = ballH;
            width = wid;
        }
    }

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D bar;
        Vector2 barPosition = Vector2.Zero;
        int barSpeed = 10;

        Ball ball1 = new Ball();

        Texture2D blueBar;
        Texture2D darkBlueBar;
        Texture2D darkRedBar;
        Texture2D redBar;
        Texture2D purpleBar;
        List<int> barColor = new List<int>();

        Texture2D sun;
        Vector2 sunPosition = Vector2.Zero;
        Boolean gameOver = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            bar = Content.Load<Texture2D>("bar");
            barPosition = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - bar.Width / 2, graphics.GraphicsDevice.Viewport.Height - 20);
            
            ball1.ball = Content.Load<Texture2D>("ball");
            ball1.setHeight(barPosition.Y, ball1.ball.Height, graphics.GraphicsDevice.Viewport.Width);

            blueBar = Content.Load<Texture2D>("blueBar");
            darkBlueBar = Content.Load<Texture2D>("darkBlueBar");
            darkRedBar = Content.Load<Texture2D>("darkRedBar");
            redBar = Content.Load<Texture2D>("redBar");
            purpleBar = Content.Load<Texture2D>("purpleBar");

            sun = Content.Load<Texture2D>("sun");
            sunPosition = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - sun.Width / 2, graphics.GraphicsDevice.Viewport.Height);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Escape))
                this.Exit();

            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Left) && barPosition.X >= 0)
                barPosition.X -= barSpeed;
            else if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Right) && barPosition.X <= graphics.GraphicsDevice.Viewport.Width - bar.Width)
                barPosition.X += barSpeed;

            ball1.update();

            if (ball1.ballPosition.X <= 0 || ball1.ballPosition.X + ball1.ball.Width >= graphics.GraphicsDevice.Viewport.Width)
                ball1.hitWall();
            if (ball1.ballPosition.Y + ball1.ball.Height >= barPosition.Y && ball1.ballPosition.Y + ball1.ball.Height <= graphics.GraphicsDevice.Viewport.Height
                    && ball1.ballPosition.X + ball1.ball.Width / 2 >= barPosition.X && ball1.ballPosition.X + ball1.ball.Width / 2 <= barPosition.X + bar.Width)
                ball1.hitBar();

            if (ball1.ballPosition.Y <= 0 && barColor.Count <= 4)
            {
                if (ball1.ballPosition.X <= graphics.GraphicsDevice.Viewport.Width / 5)
                    barColor.Add(1);
                else if (ball1.ballPosition.X <= 2 * graphics.GraphicsDevice.Viewport.Width / 5)
                    barColor.Add(2);
                else if (ball1.ballPosition.X <= 3 * graphics.GraphicsDevice.Viewport.Width / 5)
                    barColor.Add(3);
                else if (ball1.ballPosition.X <= 4 * graphics.GraphicsDevice.Viewport.Width / 5)
                    barColor.Add(4);
                else
                    barColor.Add(5);

                if (barColor.Count <= 4)
                    ball1.verticalSpeed *= -1;
            }

            if (barColor.Count > 4 && barPosition.Y < graphics.GraphicsDevice.Viewport.Height)
                barPosition.Y += 3;
            if (barPosition.Y > graphics.GraphicsDevice.Viewport.Height && sunPosition.Y > 0 + graphics.GraphicsDevice.Viewport.Height - sun.Height)
                sunPosition.Y -= 3;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            DrawBars();
            spriteBatch.Draw(bar, barPosition, Color.White);
            spriteBatch.Draw(ball1.ball, ball1.ballPosition, Color.White);
            spriteBatch.Draw(sun, sunPosition, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawBars()
        {
            Rectangle bar1 = new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height / 5);
            Rectangle bar2 = new Rectangle(0, graphics.GraphicsDevice.Viewport.Height / 5, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height / 5);
            Rectangle bar3 = new Rectangle(0, 2 * graphics.GraphicsDevice.Viewport.Height / 5, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height / 5);
            Rectangle bar4 = new Rectangle(0, 3 * graphics.GraphicsDevice.Viewport.Height / 5, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height / 5);
            Rectangle bar5 = new Rectangle(0, 4 * graphics.GraphicsDevice.Viewport.Height / 5, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height / 5);

            for (int barIndex = 0; barIndex < barColor.Count; barIndex++)
            {
                if (barIndex == 0)
                {
                    if (barColor[barIndex] == 1)
                        spriteBatch.Draw(blueBar, bar1, Color.White);
                    else if (barColor[barIndex] == 2)
                        spriteBatch.Draw(darkBlueBar, bar1, Color.White);
                    else if (barColor[barIndex] == 3)
                        spriteBatch.Draw(darkRedBar, bar1, Color.White);
                    else if (barColor[barIndex] == 4)
                        spriteBatch.Draw(redBar, bar1, Color.White);
                    else
                        spriteBatch.Draw(purpleBar, bar1, Color.White);
                }
                else if (barIndex == 1)
                {
                    if (barColor[barIndex] == 1)
                        spriteBatch.Draw(blueBar, bar2, Color.White);
                    else if (barColor[barIndex] == 2)
                        spriteBatch.Draw(darkBlueBar, bar2, Color.White);
                    else if (barColor[barIndex] == 3)
                        spriteBatch.Draw(darkRedBar, bar2, Color.White);
                    else if (barColor[barIndex] == 4)
                        spriteBatch.Draw(redBar, bar2, Color.White);
                    else
                        spriteBatch.Draw(purpleBar, bar2, Color.White);
                }
                else if (barIndex == 2)
                {
                    if (barColor[barIndex] == 1)
                        spriteBatch.Draw(blueBar, bar3, Color.White);
                    else if (barColor[barIndex] == 2)
                        spriteBatch.Draw(darkBlueBar, bar3, Color.White);
                    else if (barColor[barIndex] == 3)
                        spriteBatch.Draw(darkRedBar, bar3, Color.White);
                    else if (barColor[barIndex] == 4)
                        spriteBatch.Draw(redBar, bar3, Color.White);
                    else
                        spriteBatch.Draw(purpleBar, bar3, Color.White);
                }
                else if (barIndex == 3)
                {
                    if (barColor[barIndex] == 1)
                        spriteBatch.Draw(blueBar, bar4, Color.White);
                    else if (barColor[barIndex] == 2)
                        spriteBatch.Draw(darkBlueBar, bar4, Color.White);
                    else if (barColor[barIndex] == 3)
                        spriteBatch.Draw(darkRedBar, bar4, Color.White);
                    else if (barColor[barIndex] == 4)
                        spriteBatch.Draw(redBar, bar4, Color.White);
                    else
                        spriteBatch.Draw(purpleBar, bar4, Color.White);
                }
                else
                {
                    if (barColor[barIndex] == 1)
                        spriteBatch.Draw(blueBar, bar5, Color.White);
                    else if (barColor[barIndex] == 2)
                        spriteBatch.Draw(darkBlueBar, bar5, Color.White);
                    else if (barColor[barIndex] == 3)
                        spriteBatch.Draw(darkRedBar, bar5, Color.White);
                    else if (barColor[barIndex] == 4)
                        spriteBatch.Draw(redBar, bar5, Color.White);
                    else
                        spriteBatch.Draw(purpleBar, bar5, Color.White);
                }
            }
        }
    }
}
