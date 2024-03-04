using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RDEngine.Engine;
using RDEngine.Engine.Physics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RDEngine.GameScripts
{
    internal class LayoutLoader : GComponent
    {
        private Texture2D _layoutTexture, _wall, _border, _rug;
        private int _tileSize;

        public override void Start()
        {
            _tileSize = 16;
            Vector2 origin = Vector2.Zero;

            _layoutTexture = ContentStorer.Textures["Layout"];
            _wall = _border = _rug = ContentStorer.WhiteSquare;

            Color[] colors = new Color[_layoutTexture.Width * _layoutTexture.Height];
            _layoutTexture.GetData(colors);
            Color[,] colors2D = new Color[_layoutTexture.Width, _layoutTexture.Height];
            for (int i = 0; i < _layoutTexture.Width; i++)
            {
                for (int j = 0; j < _layoutTexture.Height; j++)
                {
                    colors2D[i, j] = colors[i + j * _layoutTexture.Width];
                    if (colors2D[i, j] == Color.Black)
                        origin = new Vector2(i, j);
                }
            }

            TiledTexture[,] tiles = new TiledTexture[_layoutTexture.Width, _layoutTexture.Height];
            RigidBody[,] tileRbs = new RigidBody[_layoutTexture.Width, _layoutTexture.Height];
            for (int i = 0; i < _layoutTexture.Width; i++)
            {
                for (int j = 0; j < _layoutTexture.Height; j++)
                {
                    Color col = colors2D[i, j];

                    if (col.A == 0) continue;

                    TiledTexture tile = new TiledTexture($"Tile[{i},{j}]", null, Vector2.Zero, Vector2.One);
                    tiles[i, j] = tile;
                    RigidBody rb = new RigidBody(Vector2.One * _tileSize, Vector2.Zero, isStatic: true);
                    tileRbs[i, j] = rb;
                    tile.AddComponent(rb);
                    tile.WorldPosition = new Vector2(i, j) - origin;

                    if (col == Color.White)
                    {
                        tile.Texture = _border;
                        tile.Color = Color.Wheat;
                    }
                    else if (col == Color.Yellow)
                    {
                        tile.Texture = _wall;
                        tile.Color = Color.Yellow;
                    }
                    else if (col == Color.Red)
                    {
                        tile.Texture = _rug;
                        tile.Color = Color.Red;
                        rb.IsTrigger = true;
                    }
                    else
                    {
                        continue;
                    }

                    if (i > 0)
                    {
                        if (colors2D[i - 1, j] == col)
                        {
                            TiledTexture oldTile = tiles[i - 1, j];
                            RigidBody oldRb = tileRbs[i - 1, j];

                            rb.Size = new Vector2(rb.Size.X + oldRb.Size.X, rb.Size.Y);
                            //rb.Offset = new Vector2((rb.Size.X - _tileSize) / 2f, 0f);
                            oldRb.Remove();
                            tileRbs[i - 1, j] = null;

                            tile.Size = new Vector2(tile.Size.X + oldTile.Size.X, tile.Size.Y);
                            tile.Position = new Vector2(tile.Position.X - _tileSize * oldTile.Size.X / 2f, tile.Position.Y);
                            oldTile.Destroy();
                            tiles[i - 1, j] = null;
                            oldTile = null;
                        }
                    }
                }
            }

            for (int i = 0; i < _layoutTexture.Width; i++)
            {
                for (int j = 1; j < _layoutTexture.Height; j++)
                {
                    Color col = colors2D[i, j];
                    RigidBody rb = tileRbs[i, j];
                    RigidBody oldRb = tileRbs[i, j - 1];
                    TiledTexture tile = tiles[i, j];
                    TiledTexture oldTile = tiles[i, j - 1];
                    if (colors2D[i, j - 1] == col && tile != null && oldTile != null)
                    {
                        if (oldRb.Size.X == rb.Size.X)
                        {
                            rb.Size = new Vector2(rb.Size.X, rb.Size.Y + oldRb.Size.Y);
                            //rb.Offset = new Vector2(rb.Offset.X, (rb.Size.Y - _tileSize) / 2f);
                            oldRb.Remove();

                            tile.Size = new Vector2(tile.Size.X, tile.Size.Y + oldTile.Size.Y);
                            tile.Position = new Vector2(tile.Position.X, tile.Position.Y - _tileSize * oldTile.Size.Y / 2f);
                            oldTile.Destroy();
                            oldTile = null;
                        }
                    }
                    if (tile != null && tile.Texture != null)
                    {
                        Parent.Scene.AddGameObject(tile);
                        tile.SetParent(Parent);
                    }
                }
            }
        }
    }
}
