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

namespace PongPlay
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D white;
        Rectangle ball;
        Rectangle line;
        Rectangle lPad;
        Rectangle rPad;
        int width;
        int height;
        int xS;
        int yS;
        int leftScore;
        int rightScore;
        int lTime;
        int rTime;
        int hitsSinceScore;
        SpriteFont score;
        SoundEffect low;
        SoundEffect high;
        SoundEffect win;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            width = GraphicsDevice.Viewport.Width;
            height = GraphicsDevice.Viewport.Height;
            // TODO: Add your initialization logic here
            ball = new Rectangle ((width/2)-10,(height/2)-10,20,20);
            line = new Rectangle(width / 2 -2, 0, 5, height);
            lPad = new Rectangle(20, height / 2 - 20, 20, 60);
            rPad = new Rectangle(width-40, height / 2 - 20, 20, 60);
            xS = 5;
            yS = 5;
            leftScore = 0;
            rightScore = 0;
            base.Initialize();
            lTime = 0;
            rTime = 0;
            hitsSinceScore = 0;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            white = this.Content.Load<Texture2D>("square");
            score = this.Content.Load<SpriteFont>("SpriteFont1");
            low = this.Content.Load<SoundEffect>("low");
            high = this.Content.Load<SoundEffect>("high");
            win = this.Content.Load<SoundEffect>("highest");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            KeyboardState kb = Keyboard.GetState();

            if (ball.Intersects(lPad)) {
                hitsSinceScore++;
                high.Play();
                if (ball.X <= lPad.X + lPad.Width - 5) {
                    yS *= -1;
                    //lTime = 15;
                    if (ball.Y < lPad.Y) {
                        ball.Y = lPad.Y - ball.Height;
                    }
                    else {
                        ball.Y = lPad.Y + lPad.Height;
                    }
                }
                else {
                    xS *= -1;
                }
            }

            if (ball.Intersects(rPad)) {
                hitsSinceScore++;
                high.Play();
                if (ball.X + ball.Width >= rPad.X + 5) {
                    yS *= -1;
                    //rTime = 15;
                    if (ball.Y < rPad.Y) {
                        ball.Y = rPad.Y - ball.Height;
                    } else {
                        ball.Y = rPad.Y + rPad.Height;
                    }
                }
                else {
                    xS *= -1;
                }
            }


            if (kb.IsKeyDown(Keys.Down) && rPad.Y+rPad.Height < height && rTime <= 0)
            {
                rPad.Y += 4;
            }
            if (kb.IsKeyDown(Keys.Up) && rPad.Y > 0 && rTime <= 0)
            {
                rPad.Y-= 4;
            }
            /*if (kb.IsKeyDown(Keys.S) && lPad.Y + lPad.Height < height && lTime <= 0)
            {
                lPad.Y+= 4;
            }
            if (kb.IsKeyDown(Keys.W) && lPad.Y > 0 && lTime <= 0)
            {
                lPad.Y-= 4;
            *///}
            
            if(ball.X + (ball.Width / 2) < width / 2) {
                if (ball.Y + (ball.Height / 2) > lPad.Y + (lPad.Height) / 2) {
                    if (lPad.Y + lPad.Height < height) {
                        lPad.Y += 4;
                    }
                }
                else {
                    if (lPad.Y > 0) {
                        lPad.Y -= 4;
                    }
                }
            }


            if (ball.X + ball.Width <= 0 || ball.X >= width) {
                win.Play();
                if (ball.X >= width) {
                    hitsSinceScore = 0;
                    leftScore++;
                } else {
                    hitsSinceScore = 0;
                    rightScore++;
                }
                Random r = new Random();
                if (r.Next(2) == 0) {
                    xS = r.Next(6) + 2;
                    yS = r.Next(4) + 4;

                } else {
                    xS = (r.Next(6) + 2) * -1;
                    yS = r.Next(4) + 4;

                }
                ball.X = width / 2 - 10;
                ball.Y = r.Next(height - 40) + 20;
            }

            if (ball.Y <= 0 || ball.Y + ball.Height >= height) {
                low.Play();
                yS *= -1;
            }
            ball.X += xS;
            ball.Y += yS;
            if (rTime > 0) {
                rTime--;
            }
            if (lTime > 0) {
                lTime--;
            }
            if (hitsSinceScore > 8) {
                Random r = new Random();
                if (r.Next(2) == 0) {
                    xS *= 2;
                    hitsSinceScore = 0;
                }
                else {
                    hitsSinceScore = 0;
                    yS *= 2;
                }
            }
            base.Update(gameTime);
        }







        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.DrawString(score,leftScore + "",new Vector2(width/4,0), Color.White);
            spriteBatch.DrawString(score, rightScore + "", new Vector2((width/4)*3, 0), Color.White);
            spriteBatch.Draw(white,ball,Color.White);
            spriteBatch.Draw(white, line, Color.White);
            spriteBatch.Draw(white, lPad, Color.White);
            spriteBatch.Draw(white, rPad, Color.White);
            spriteBatch.End();
   


            base.Draw(gameTime);
        }
    }
}
