using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using System.IO;
using System.Xml;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace RebellionGame
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;
        public bool lvl1 = false;
        public bool lvl2 = false;
        public bool lvl3 = false;
        public int lvl1hs;
        public int lvl2hs;
        public int lvl3hs;
        public int lvl1s;
        public int lvl2s;
        public int lvl3s;

        public string Currentlvl;

        private string xmlFile = "C:\\Users\\Wacky\\Documents\\RebellionSave.xml";

        private readonly ScreenManager _screenManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _screenManager = new ScreenManager();
            Components.Add(_screenManager);
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;

            // TODO: Add your initialization logic here
            CreateSave();
            LoadLevel();
            LoadMainMenu();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public void LoadMainMenu()
        {
            _screenManager.LoadScreen(new MainMenu(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void LoadLevel1()
        {
            _screenManager.LoadScreen(new Level1(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void LoadLevel2()
        {
            _screenManager.LoadScreen(new Level2(this), new FadeTransition(GraphicsDevice, Color.Black));
        }
        public void LoadLevel3()
        {
            _screenManager.LoadScreen(new Level3(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void LoadGameover()
        {
            _screenManager.LoadScreen(new Gameover(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        private void CreateSave()
        {
            try
            {
                XmlReader reader = XmlReader.Create(xmlFile, null);
            }
            catch (FileNotFoundException)
            {
                XmlTextWriter textWriter = new XmlTextWriter(xmlFile, null);
                // Opens the document
                textWriter.WriteStartDocument();
                // Write comments
                textWriter.WriteComment("Game Progress");
                textWriter.WriteComment(":)");

                // Write first element
                textWriter.WriteStartElement("lvl1");
                textWriter.WriteString("yes");

                textWriter.WriteStartElement("GameProgress");

                textWriter.WriteStartElement("lvl1hs");
                textWriter.WriteString("0");
                textWriter.WriteEndElement();

                textWriter.WriteStartElement("lvl2");
                textWriter.WriteString("no");
                textWriter.WriteEndElement();

                textWriter.WriteStartElement("lvl2hs");
                textWriter.WriteString("0");
                textWriter.WriteEndElement();

                textWriter.WriteStartElement("lvl3");
                textWriter.WriteString("no");
                textWriter.WriteEndElement();

                textWriter.WriteStartElement("lvl3hs");
                textWriter.WriteString("0");
                textWriter.WriteEndElement();

                textWriter.WriteEndElement();

                textWriter.WriteEndDocument();

                textWriter.Close();
            }
        }

        public void LoadLevel()
        {
            using (XmlReader reader = XmlReader.Create(xmlFile, null))
            {

                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        //return only when you have START tag  
                        switch (reader.Name.ToString())
                        {
                            case "lvl1":
                                if (reader.ReadString() == "yes")
                                {
                                    lvl1 = true;
                                }
                                else
                                {
                                    lvl1 = false;
                                }
                                break;

                            case "lvl1hs":
                                lvl1hs = Int32.Parse(reader.ReadString());
                                break;

                            case "lvl2":
                                if (reader.ReadString() == "yes")
                                {
                                    lvl2 = true;
                                }
                                else
                                {
                                    lvl2 = false;
                                }
                                break;

                            case "lvl2hs":
                                lvl2hs = Int32.Parse(reader.ReadString());
                                break;

                            case "lvl3":
                                if (reader.ReadString() == "yes")
                                {
                                    lvl3 = true;
                                }
                                else
                                {
                                    lvl3 = false;
                                }
                                break;

                            case "lvl3hs":
                                lvl3hs = Int32.Parse(reader.ReadString());
                                break;
                        }
                    }
                }
            }
        }

        public void UpdateProgress() 
        {
            XmlTextWriter textWriter = new XmlTextWriter(xmlFile, null);
            // Opens the document
            textWriter.WriteStartDocument();
            // Write comments
            textWriter.WriteComment("Game Progress");
            textWriter.WriteComment(":)");

            // Write first element
            textWriter.WriteStartElement("lvl1");
            if (lvl1)
            {
                textWriter.WriteString("yes");
            }
            else
            {
                textWriter.WriteString("no");
            }

            textWriter.WriteStartElement("GameProgress");

            textWriter.WriteStartElement("lvl1hs");
            if(lvl1s > lvl1hs)
            {
                textWriter.WriteString(lvl1s.ToString());
            }
            else
            {
                textWriter.WriteString(lvl1hs.ToString());
            }
            textWriter.WriteEndElement();

            textWriter.WriteStartElement("lvl2");
            if (lvl2)
            {
                textWriter.WriteString("yes");
            }
            else
            {
                textWriter.WriteString("no");
            }
            textWriter.WriteEndElement();

            textWriter.WriteStartElement("lvl2hs");
            if (lvl2s > lvl2hs)
            {
                textWriter.WriteString(lvl2s.ToString());
            }
            else
            {
                textWriter.WriteString(lvl2hs.ToString());
            }
            textWriter.WriteEndElement();

            textWriter.WriteStartElement("lvl3");
            if (lvl3)
            {
                textWriter.WriteString("yes");
            }
            else
            {
                textWriter.WriteString("no");
            }
            textWriter.WriteEndElement();

            textWriter.WriteStartElement("lvl3hs");
            if (lvl3s > lvl3hs)
            {
                textWriter.WriteString(lvl3s.ToString());
            }
            else
            {
                textWriter.WriteString(lvl3hs.ToString());
            }
            textWriter.WriteEndElement();

            textWriter.WriteEndElement();

            textWriter.WriteEndDocument();

            textWriter.Close();
        }
    }
}