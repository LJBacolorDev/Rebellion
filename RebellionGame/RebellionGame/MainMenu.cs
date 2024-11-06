using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;

namespace RebellionGame
{
    public class MainMenu : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        private Texture2D rebellion;
        private Texture2D feuA;
        private SpriteFont _font;
        private SpriteFont text;
        private bool isLoading = false;

        private SoundEffect bgMusic;
        private SoundEffectInstance soundEffectInstance;

        private SoundEffect selectSFX;
        private SoundEffectInstance selectIns;

        //Rectangles
        private Texture2D pixel;
        private Rectangle newRec;
        private Rectangle loadRec;
        private Rectangle exitRec;
        private Color newRecColor;
        private Color loadRecColor;
        private Color exitRecColor;

        private Rectangle oneRec;
        private Rectangle twoRec;
        private Rectangle threeRec;
        private Rectangle backRec;
        private Color oneColor;
        private Color twoColor;
        private Color threeColor;
        private Color backColor;

        private MouseState mState;
        private Vector2 cursorPos;

        public MainMenu(Game1 game) : base(game) { }

        public override void Initialize()
        {
            Game._graphics.PreferredBackBufferWidth = 1280;
            Game._graphics.PreferredBackBufferHeight = 720;
            Game._graphics.ApplyChanges();
            base.Initialize();
        }

        public override void LoadContent()
        {
            base.LoadContent();
            _font = Game.Content.Load<SpriteFont>("font");
            text = Game.Content.Load<SpriteFont>("text");
            rebellion = Game.Content.Load<Texture2D>("rebellion");
            feuA = Game.Content.Load<Texture2D>("FEUA");

            bgMusic = Content.Load<SoundEffect>("mainmenubgm");
            soundEffectInstance = bgMusic.CreateInstance();
            soundEffectInstance.IsLooped = true;
            soundEffectInstance.Play();

            selectSFX = Content.Load<SoundEffect>("select");
            selectIns = selectSFX.CreateInstance();

            //Rectangles
            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });
            newRec = new Rectangle(820, 280, 500, 80);
            loadRec = new Rectangle(820, 430, 500, 80);
            exitRec = new Rectangle(820, 580, 500, 80);

            oneRec = new Rectangle(170, 340, 150, 150);
            twoRec = new Rectangle(370, 340, 150, 150);
            threeRec = new Rectangle(570, 340, 150, 150);
            backRec = new Rectangle(555, 560, 180, 80);
        }

        public override void Update(GameTime gameTime)
        {
            mState = Mouse.GetState();
            cursorPos = new Vector2(mState.X, mState.Y);
            if (isLoading)
            {
                oneButton();
                twoButton();
                threeButton();
                BackButton();
            }
            else
            {
                newButton();
                loadButton();
                exitButton();
            }
            
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Game._spriteBatch.Begin();
            //Background
            Game._spriteBatch.Draw(rebellion, new Rectangle(0, 0, 1280, 720), Color.White);
            Game._spriteBatch.Draw(feuA, new Rectangle(20,20,158,158), Color.White);

            if (isLoading)
            {
                //Loading Gui
                Game._spriteBatch.DrawString(text, "Load Level", new Vector2(250, 120), Color.White);
                Game._spriteBatch.Draw(pixel, new Rectangle(64,250,1156,420), Color.Gray * 0.8f);
                Game._spriteBatch.DrawRectangle(oneRec, oneColor, 7f);
                Game._spriteBatch.DrawRectangle(twoRec, twoColor, 7f);
                Game._spriteBatch.DrawRectangle(threeRec, threeColor, 7f);
                Game._spriteBatch.DrawRectangle(backRec, backColor, 7f);
                Game._spriteBatch.DrawRectangle(new Rectangle(770, 340, 150, 150), Color.Black * 0.5f, 7f);
                Game._spriteBatch.DrawRectangle(new Rectangle(970, 340, 150, 150), Color.Black * 0.5f, 7f);
                Game._spriteBatch.DrawString(text, "1", new Vector2(235,380), oneColor);
                Game._spriteBatch.DrawString(_font, "High Score: " + Game.lvl1hs.ToString(), new Vector2(175, 500), oneColor);
                Game._spriteBatch.DrawString(text, "2", new Vector2(435,380), twoColor);
                Game._spriteBatch.DrawString(_font, "High Score: " + Game.lvl2hs.ToString(), new Vector2(375, 500), twoColor);
                Game._spriteBatch.DrawString(text, "3", new Vector2(635,380), threeColor);
                Game._spriteBatch.DrawString(_font, "High Score: " + Game.lvl3hs.ToString(), new Vector2(575, 500), threeColor);
                Game._spriteBatch.DrawString(text, "?", new Vector2(835, 380), Color.Black * 0.5f);
                Game._spriteBatch.DrawString(text, "?", new Vector2(1035, 380), Color.Black * 0.5f);
                Game._spriteBatch.DrawString(text, "Back", new Vector2(590, 570), backColor);
            }
            else
            {
                //Main Gui
                Game._spriteBatch.Draw(pixel, newRec, newRecColor);
                Game._spriteBatch.Draw(pixel, loadRec, loadRecColor);
                Game._spriteBatch.Draw(pixel, exitRec, exitRecColor);
                Game._spriteBatch.DrawString(text, "New Game", new Vector2(840, 290), Color.White);
                Game._spriteBatch.DrawString(text, "Load Level", new Vector2(840, 440), Color.White);
                Game._spriteBatch.DrawString(text, "Exit", new Vector2(920, 590), Color.White);
                Game._spriteBatch.DrawString(_font,"Developed by:\nLyndon Justin M. Bacolor\nJohn M. Dilay\nLee Zachary D. Concepcion\nKim Ian D. Lim", new Vector2(60,580), Color.White);
            }
            Game._spriteBatch.End();
        }

        public void playSelect()
        {
            if(selectIns.State == SoundState.Playing)
            {
                selectIns.Stop();
            }
            else 
            { 
                selectIns.Play();
            }
        }

        public void newButton()
        {
            if (newRec.Contains(cursorPos))
            {
                newRecColor = Color.Black * 0.5f;
                if (mState.LeftButton == ButtonState.Pressed)
                {
                    newRecColor = Color.Black;
                    playSelect();
                    soundEffectInstance.Stop();
                    Game.LoadLevel1();
                }
            }
            else
            {
                newRecColor = Color.Transparent;
            }
        }

        public void loadButton()
        {
            if (loadRec.Contains(cursorPos))
            {
                loadRecColor = Color.Black * 0.5f;
                if (mState.LeftButton == ButtonState.Pressed)
                {
                    loadRecColor = Color.Black;
                    playSelect();
                    Game.LoadLevel();
                    isLoading = true;
                }
            }
            else
            {
                loadRecColor = Color.Transparent;
            }
        }

        public void exitButton()
        {
            if (exitRec.Contains(cursorPos))
            {
                exitRecColor = Color.Black * 0.5f;
                if (mState.LeftButton == ButtonState.Pressed)
                {
                    exitRecColor = Color.Black;
                    playSelect();
                    Game.Exit();
                }
            }
            else
            {
                exitRecColor = Color.Transparent;
            }
        }

        public void oneButton()
        {
            if (Game.lvl1)
            {
                if (oneRec.Contains(cursorPos))
                {
                    oneColor = Color.Black * 0.5f;
                    if (mState.LeftButton == ButtonState.Pressed)
                    {
                        oneColor = Color.Black;
                        playSelect();
                        soundEffectInstance.Stop();
                        Game.LoadLevel1();
                    }
                }
                else
                {
                    oneColor = Color.White;
                }
            }
            else
            {
                oneColor = Color.Black * 0.5f;
            }
        }
        public void twoButton()
        {
            if (Game.lvl2)
            {
                if (twoRec.Contains(cursorPos))
                {
                    twoColor = Color.Black * 0.5f;
                    if (mState.LeftButton == ButtonState.Pressed)
                    {
                        twoColor = Color.Black;
                        playSelect();
                        soundEffectInstance.Stop();
                        Game.LoadLevel2();
                    }
                }
                else
                {
                    twoColor = Color.White;
                }
            }
            else
            {
                twoColor = Color.Black * 0.5f;
            }
        }

        public void threeButton()
        {
            if (Game.lvl3)
            {
                if (threeRec.Contains(cursorPos))
                {
                    threeColor = Color.Black * 0.5f;
                    if (mState.LeftButton == ButtonState.Pressed)
                    {
                        threeColor = Color.Black;
                        playSelect();
                        soundEffectInstance.Stop();
                        Game.LoadLevel3();
                    }
                }
                else
                {
                    threeColor = Color.White;
                }
            }
            else
            {
                threeColor = Color.Black * 0.5f;
            }
        }

        public void BackButton()
        {
            if (backRec.Contains(cursorPos))
            {
                backColor = Color.Black * 0.5f;
                if (mState.LeftButton == ButtonState.Pressed)
                {
                    backColor = Color.Black;
                    playSelect();
                    isLoading = false;
                }
            }
            else
            {
                backColor = Color.White;
            }
        }
    }
}
