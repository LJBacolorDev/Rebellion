using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace RebellionGame
{
    internal class Player
    {
        public Vector2 pos;
        public float health = 200;
        private AnimatedSprite _playerSprite;
        private SpriteSheet _spriteSheet;

        private SoundEffectInstance _JumpIns;
        private SoundEffectInstance _Jump2Ins;
        private SoundEffectInstance _DashIns;

        private float moveSpeed = 8f;
        private float jumpSpeed;
        private float dashTimer = 0f;
        private float canDashTimer = 0f;
        private float startY;

        public Rectangle playerBounds;
        public Rectangle playerFallRect;

        public bool isIdle = false;
        public bool isFacingLeft = false;
        public bool isFalling = true;
        public bool isJumping = false;
        public bool isDashing = false;
        public bool canDash = true;
        public bool canDoubleJump = true;

        public Player(SpriteSheet _sheet, SoundEffect _Jumps, SoundEffect _Jump2s, SoundEffect _Dashs)
        {
            _spriteSheet = _sheet;
            _playerSprite = new AnimatedSprite(_spriteSheet);
            playerFallRect = new Rectangle((int)pos.X, (int)pos.Y + 32, 32, 4);

            _JumpIns = _Jumps.CreateInstance();
            _Jump2Ins = _Jump2s.CreateInstance();
            _DashIns = _Dashs.CreateInstance();
        }

        public void Update(GameTime gameTime)
        {
            isIdle = true;

            var kstate = Keyboard.GetState();
            String animation = "idleR";


            if (isFalling && !isDashing)
            {
                isIdle = false;
                if (isFacingLeft)
                {
                    animation = "fallL";
                }
                else
                {
                    animation = "fallR";
                }
                pos.Y += 10f;
            }

            startY = pos.Y;
            Move(kstate);
            Jump(kstate);
            Dash(kstate);

            if (isIdle)
            {
                if (isFacingLeft)
                {
                    animation = "idleL";
                }
                else
                {
                    animation = "idleR";
                }
            }

            void Move(KeyboardState keyboard)
            {

                if (keyboard.IsKeyDown(Keys.A))
                {
                    pos.X -= moveSpeed;
                    if(!isJumping)
                        animation = "runL";
                    isIdle = false;
                    isFacingLeft = true;
                }
                if (keyboard.IsKeyDown(Keys.D))
                {
                    pos.X += moveSpeed;
                    if (!isJumping)
                        animation = "runR";
                    isIdle = false;
                    isFacingLeft = false;
                }
            }

            void Jump(KeyboardState keyboard)
            {
                if (isJumping)
                {
                    pos.Y += jumpSpeed;
                    jumpSpeed += 1;
                    Move(keyboard);
                    isIdle = false;

                    if (isFacingLeft)
                    {
                        animation = "jumpL";
                    }
                    else
                    {
                        animation = "jumpR";
                    }
                    if (pos.Y >= startY)
                    
                    {
                        pos.Y = startY;
                        isJumping = false;
                        moveSpeed = 8f;
                    }
                }
                else
                {
                    if (keyboard.IsKeyDown(Keys.W) && !isFalling)
                    {
                        moveSpeed = 4f;
                        isJumping = true;
                        isFalling = false;
                        jumpSpeed = -25;
                        _JumpIns.Play();
                    }
                    else if (keyboard.IsKeyDown(Keys.W) && canDoubleJump)
                    {
                        moveSpeed = 4f;
                        isJumping = true;
                        isFalling = false;
                        jumpSpeed = -25;
                        canDoubleJump = false;
                        _Jump2Ins.Play();
                    }
                }
            }

            void Dash(KeyboardState keyboard)
            {
                if (keyboard.IsKeyDown(Keys.Space) && canDash)
                {
                    isDashing = true;
                    canDash = false;
                    canDashTimer = 0f;
                    dashTimer = 0f;
                    _DashIns.Play();
                }

                if (isDashing)
                {
                    isIdle = false;
                    if (isFacingLeft)
                    {
                        animation = "dashL";
                        pos.X -= 40f;
                    }
                    else
                    {
                        animation = "dashR";
                        pos.X += 40f;
                    }

                    dashTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (dashTimer >= 50)
                    {
                        isDashing = false;
                    }
                }

                if (!canDash)
                {
                    canDashTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (canDashTimer >= 1000)
                    {
                        canDash = true;
                    }
                }
            }

            _playerSprite.Play(animation);

            playerBounds.Height = 58;
            playerBounds.Width = 32;
            playerBounds.X = (int)pos.X - 16;
            playerBounds.Y = (int)pos.Y - 30;

            playerFallRect.X = (int)pos.X - 16;
            playerFallRect.Y = (int)pos.Y + 34;

            _playerSprite.Update(gameTime);
            
        }

        public void Draw(SpriteBatch spriteBatch, Matrix transformMatrix)
        {
            spriteBatch.Begin(transformMatrix: transformMatrix);
            spriteBatch.Draw(_playerSprite, new Vector2(pos.X,pos.Y+4));
            spriteBatch.End();
        }
    }
}
