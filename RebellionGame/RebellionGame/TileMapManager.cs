using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace RebellionGame
{
    internal class TileMapManager
    {
        private SpriteBatch spriteBatch;
        TmxMap map;
        Texture2D tileset;
        int tilesetTilesWide;
        int tileWidth;
        int tileHeight;

        public TileMapManager(SpriteBatch _spriteSheet,TmxMap _map, Texture2D _tileset, int _tilesetTilesWide,int _tileWidth,int _tileHeight)
        {
            spriteBatch = _spriteSheet;
            map = _map;
            tileset = _tileset;
            tilesetTilesWide = _tilesetTilesWide;
            tileWidth = _tileWidth;
            tileHeight = _tileHeight;
        }

        public void Draw(Matrix transformMatrix)
        {
            spriteBatch.Begin(transformMatrix: transformMatrix);
            for(var i = 0; i < map.Layers.Count; i++)
            {
                for(var j = 0; j < map.Layers[i].Tiles.Count;j++)
                {
                    int gid = map.Layers[i].Tiles[j].Gid;
                    if(gid != 0)
                    {
                        int tileFrame = gid - 1;
                        int column = tileFrame % tilesetTilesWide;
                        int row = (int)Math.Floor((double)tileFrame / (double)tilesetTilesWide);
                        float x = (j % map.Width) * map.TileWidth;
                        float y = (float)Math.Floor(j / (double)map.Width) * map.TileHeight;
                        Rectangle tilesetRec = new Rectangle((tileWidth) * column, (tileHeight) * row, tileWidth, tileHeight);
                        spriteBatch.Draw(tileset,new Rectangle((int)x,(int)y,tileHeight,tileWidth),tilesetRec,Color.White);
                    }
                }
            }
            spriteBatch.End();
        }
    }
}
