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
    public class Gameover : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        private Texture2D bg;
        private Texture2D pixel;

        private SoundEffect bgMusic;
        private SoundEffectInstance soundEffectInstance;

        private SoundEffect selectSFX;
        private SoundEffect select2SFX;
        private SoundEffectInstance selectIns;
        private SoundEffectInstance selectIns2;

        private SoundEffect pressSFX;
        private SoundEffectInstance pressIns;

        private bool willContinue = false;

        public Gameover(Game1 game) : base(game) { }

        public override void Initialize()
        {
            
            base.Initialize();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            bg = Content.Load<Texture2D>("gameover");

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });

            bgMusic = Content.Load<SoundEffect>("gameoverbgm");
            soundEffectInstance = bgMusic.CreateInstance();
            soundEffectInstance.IsLooped = true;
            soundEffectInstance.Play();

            selectSFX = Content.Load<SoundEffect>("select");
            selectIns = selectSFX.CreateInstance();
            select2SFX = Content.Load<SoundEffect>("select");
            selectIns2 = select2SFX.CreateInstance();

            pressSFX = Content.Load<SoundEffect>("press");
            pressIns = pressSFX.CreateInstance();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState kState = Keyboard.GetState();

            if(kState.IsKeyDown(Keys.D))
            {
                if(willContinue)
                {
                    playSelect2();
                    willContinue = false;
                }
            }
            if (kState.IsKeyDown(Keys.A))
            {
                if (!willContinue)
                {
                    playSelect();
                    willContinue = true;
                }
            }
            if (kState.IsKeyDown(Keys.Enter))
            {
                pressIns.Play();
                soundEffectInstance.Stop();
                if (willContinue)
                {
                    if (Game.Currentlvl == "1")
                    {
                        Game.LoadLevel1();
                    }
                    else if (Game.Currentlvl == "2")
                    {
                        Game.LoadLevel2();
                    }
                    else if (Game.Currentlvl == "3")
                    {
                        Game.LoadLevel3();
                    }
                    else
                    {
                        Game.LoadMainMenu();
                    }
                }
                else
                {
                    Game.LoadMainMenu();
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Game._spriteBatch.Begin();
            Game._spriteBatch.Draw(bg,new Vector2(0,0),Color.White);
            if (willContinue)
            {
                Game._spriteBatch.Draw(pixel, new Rectangle(340, 620, 200, 40), Color.White);
            }
            else
            {
                Game._spriteBatch.Draw(pixel, new Rectangle(760, 620, 200, 40), Color.White);
            }
            Game._spriteBatch.End();
        }
        public void playSelect()
        {
            if (selectIns.State == SoundState.Playing)
            {
                selectIns.Stop();
            }
            else
            {
                selectIns.Play();
            }
        }

        public void playSelect2()
        {
            if (selectIns.State == SoundState.Playing)
            {
                selectIns2.Stop();
            }
            else
            {
                selectIns2.Play();
            }
        }
    }
}
