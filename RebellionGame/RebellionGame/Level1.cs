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
using MonoGame.Extended.Content;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.ViewportAdapters;
using TiledSharp;
using MonoGame.Extended.Serialization;

namespace RebellionGame
{
    public class Level1 : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        private SpriteBatch _spriteBatch;
        private Player _player;
        private SoundEffect _jumpSFX;
        private SoundEffect _jump2SFX;
        private SoundEffect _dashSFX;
        private SoundEffect _deathSFX;
        private SoundEffectInstance _DeathIns;
        private SoundEffect _hurtSFX;
        private SoundEffectInstance _HurtIns;
        private SoundEffect _bgmSFX;
        private SoundEffectInstance _BgmIns;

        private SpriteSheet _spriteSheet;
        private TmxMap _tmxMap;
        private TileMapManager _mapManager;
        private List<Rectangle> collisionObjects;
        private List<Rectangle> checkpoint;
        private List<Rectangle> death;
        private List<Rectangle> exit;
        private List<Vector2> heal;
        private OrthographicCamera _camera;
        private SpriteFont _font;
        private SpriteFont text;
        private Texture2D _pixel;

        private int Score = 20000;

        private Vector2 _spawnpoint;
        private int _zoomX = 5;
        private int _zoomY = 5;

        private bool resume = false;
        private float pauseTimer = 0f;
        private bool paused = false;

        public Level1(Game1 game) : base(game) { }

        public override void Initialize()
        {
            base.Initialize();

            var viewportAdapter = new BoxingViewportAdapter(Game.Window, GraphicsDevice, 800, 480);
            _camera = new OrthographicCamera(viewportAdapter);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            Game.Currentlvl = "1";

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _tmxMap = new TmxMap("Content/level 1.tmx");
            var tileset = Content.Load<Texture2D>("AssetPack");
            var tileWidth = _tmxMap.Tilesets[0].TileWidth;
            var tileHeight = _tmxMap.Tilesets[0].TileHeight;
            var TileSetTilesWide = tileset.Width / tileWidth;
            _mapManager = new TileMapManager(_spriteBatch, _tmxMap, tileset, TileSetTilesWide, tileWidth, tileHeight);

            collisionObjects = new List<Rectangle>();
            checkpoint = new List<Rectangle>();
            death = new List<Rectangle>();
            exit = new List<Rectangle>();
            heal = new List<Vector2>();

            foreach (var o in _tmxMap.ObjectGroups["Collision"].Objects)
            {
                collisionObjects.Add(new Rectangle((int)o.X, (int)o.Y, (int)o.Width, (int)o.Height));
            }

            foreach (var o in _tmxMap.ObjectGroups["Checkpoints"].Objects)
            {
                checkpoint.Add(new Rectangle((int)o.X, (int)o.Y, (int)o.Width, (int)o.Height));
            }

            foreach (var o in _tmxMap.ObjectGroups["Death"].Objects)
            {
                death.Add(new Rectangle((int)o.X, (int)o.Y, (int)o.Width, (int)o.Height));
            }

            foreach (var o in _tmxMap.ObjectGroups["NextLVL"].Objects)
            {
                exit.Add(new Rectangle((int)o.X, (int)o.Y, (int)o.Width, (int)o.Height));
            }

            foreach (var o in _tmxMap.ObjectGroups["Heal"].Objects)
            {
                heal.Add(new Vector2((float)o.X, (float)o.Y));
            }

            _spriteSheet = Content.Load<SpriteSheet>("YueFullSheet.sf", new JsonContentLoader());
            _jumpSFX = Content.Load<SoundEffect>("YueJump");
            _jump2SFX = Content.Load<SoundEffect>("YueJump2");
            _dashSFX = Content.Load<SoundEffect>("YueDash");
            _hurtSFX = Content.Load<SoundEffect>("YueHurt");
            _HurtIns = _hurtSFX.CreateInstance();
            _deathSFX = Content.Load<SoundEffect>("YueDeath");
            _DeathIns = _deathSFX.CreateInstance();

            _player = new Player(_spriteSheet,_jumpSFX,_jump2SFX,_dashSFX);
            _player.pos = new Vector2(600, 1600);

            _bgmSFX = Content.Load<SoundEffect>("lvl1bgm");
            _BgmIns = _bgmSFX.CreateInstance();
            _BgmIns.IsLooped = true;
            _BgmIns.Play();

            _spawnpoint = _player.pos;

            text = Game.Content.Load<SpriteFont>("text");
            _font = Content.Load<SpriteFont>("font");
            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData<Color>(new Color[] { Color.White });
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState kState = Keyboard.GetState();

            if (kState.IsKeyDown(Keys.Enter) && !resume)
            {
                resume = true;
                pauseTimer = 0f;

                if (paused)
                {
                    paused = false;
                }
                else
                {
                    paused = true;
                }
            }

            if (resume)
            {
                pauseTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (pauseTimer >= 500)
                {
                    resume = false;
                }
            }

            if (paused)
            {
                if (kState.IsKeyDown(Keys.M))
                {
                    _BgmIns.Stop();
                    Game.LoadMainMenu();
                }
                if (kState.IsKeyDown(Keys.R))
                {
                    _BgmIns.Stop();
                    Game.LoadLevel1();
                }
            }
            else
            {
                var initpos = _player.pos;

                _player.Update(gameTime);
                Score -= 1;

                if(_player.health <= 0)
                {
                    _deathSFX.Play();
                    _player.health = 1;
                    _BgmIns.Stop();
                    Game.LoadGameover();
                }

                _player.isFalling = true;
                foreach (var rect in collisionObjects)
                {
                    if (rect.Intersects(_player.playerFallRect))
                    {
                        _player.isFalling = false;
                        _player.canDoubleJump = true;
                    }
                }

                foreach (var rect in collisionObjects)
                {
                    if (rect.Intersects(_player.playerBounds))
                    {
                        _player.pos = initpos;
                    }
                }

                foreach (var rect in checkpoint)
                {
                    if (rect.Intersects(_player.playerBounds))
                    {
                        _spawnpoint = _player.pos;
                    }
                }

                foreach (var rect in death)
                {
                    if (rect.Intersects(_player.playerBounds))
                    {
                        _player.pos = _spawnpoint;
                        _player.health -= 40f;
                        Score -= 500;
                        _HurtIns.Play();
                    }
                }

                foreach (var point in heal)
                {
                    if (_player.playerBounds.Contains(point))
                    {
                        heal.Remove(point);
                        Score += 20;
                        if (_player.health < 200)
                        {
                            _player.health += 2;
                        }
                        break;
                    }
                }

                foreach (var rect in exit)
                {
                    if (rect.Intersects(_player.playerBounds))
                    {
                        _BgmIns.Stop();
                        Game.lvl2 = true;
                        Game.lvl1s = Score;
                        Game.UpdateProgress();
                        Game.LoadLevel2();
                    }
                }

                _camera.Position = _player.pos - new Vector2(400, 250);
                _camera.MaximumZoom = 1.4f;
                _camera.MinimumZoom = 0.7f;

                if (kState.IsKeyDown(Keys.E))
                {
                    _camera.ZoomIn(0.1f);
                    if (_zoomX <= 110)
                    {
                        _zoomX += 30;
                    }
                    if (_zoomY <= 70)
                    {
                        _zoomY += 20;
                    }
                }

                if (kState.IsKeyDown(Keys.Q))
                {
                    _camera.ZoomOut(0.1f);

                    if (_zoomX >= -140)
                    {
                        _zoomX -= 30;
                    }
                    if (_zoomY >= -70)
                    {
                        _zoomY -= 20;
                    }
                }
            }

        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(25, 26, 26));

            // TODO: Add your drawing code here
            Matrix transformMatrix = _camera.GetViewMatrix();

            _mapManager.Draw(transformMatrix);
            _player.Draw(_spriteBatch, transformMatrix);

            _spriteBatch.Begin(transformMatrix: transformMatrix);
            _spriteBatch.Draw(_pixel, new Rectangle((int)_camera.Position.X + _zoomX, (int)_camera.Position.Y + _zoomY, 200, 20), Color.Gray);
            _spriteBatch.Draw(_pixel, new Rectangle((int)_camera.Position.X + _zoomX, (int)_camera.Position.Y + _zoomY, (int)_player.health, 20), Color.Red);
            _spriteBatch.DrawString(_font, "Score: " + Score.ToString(), new Vector2(_camera.Position.X + _zoomX, _camera.Position.Y + _zoomY + 30), Color.Green);
            foreach (var point in heal)
            {
                _spriteBatch.Draw(_pixel, new Rectangle((int)point.X, (int)point.Y, 8, 8), Color.DarkRed);
            }

            if (paused)
            {
                _spriteBatch.Draw(_pixel, new Rectangle((int)_camera.Position.X - 200, (int)_camera.Position.Y - 200, 2000, 2000), Color.Black * 0.5f);
                _spriteBatch.DrawString(text, "Press Enter to Resume\n          R to Restart\n       M to Main Menu", new Vector2(_player.pos.X-260, _player.pos.Y-100), Color.White);

            }
            _spriteBatch.End();

        }
    }
}
