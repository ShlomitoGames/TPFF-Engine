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
    public class LayoutLoader : GComponent
    {
        private Texture2D _layoutTexture, _wall, _border, _rug, _floor, _floor2;
        private int _tileSize;
        private string _layoutString;

        public LayoutLoader(string layoutString)
        {
            _layoutString = layoutString;
        }

        public override void Start()
        {
            _tileSize = 16;
            Vector2 origin = Vector2.Zero;

            _layoutTexture = ContentStorer.Textures[_layoutString];
            _wall = ContentStorer.Textures["Wall"];
            _rug = ContentStorer.Textures["Rug"];
            _border = ContentStorer.Textures["Border"];
            _floor = ContentStorer.Textures["Floor"];
            _floor2 = ContentStorer.Textures["Floor2"];

            //Parse de image into a 2-dimensional array of colors
            Color[] colors = new Color[_layoutTexture.Width * _layoutTexture.Height];
            _layoutTexture.GetData(colors);
            Color[,] colors2D = new Color[_layoutTexture.Width, _layoutTexture.Height];
            for (int i = 0; i < _layoutTexture.Width; i++)
            {
                for (int j = 0; j < _layoutTexture.Height; j++)
                {
                    colors2D[i, j] = colors[i + j * _layoutTexture.Width];
                    //If the color is black sets the center from which all other tiles will be added
                    if (colors2D[i, j] == Color.Black)
                        origin = new Vector2(i, j);
                }
            }

            TiledTexture[,] tiles = new TiledTexture[_layoutTexture.Width, _layoutTexture.Height];
            RigidBody[,] tileRbs = new RigidBody[_layoutTexture.Width, _layoutTexture.Height];

            // Tuple<Vector2, Vector2, Color>[,] tileTs = new Tuple<Vector2, Vector2, Color>[_layoutTexture.Width, _layoutTexture.Height];

            Random rnd = new Random();
            for (int i = 0; i < _layoutTexture.Width; i++)
            {
                for (int j = 0; j < _layoutTexture.Height; j++)
                {
                    Color col = colors2D[i, j];

                    //Ignores empty colors
                    if (col.A == 0) continue;
                    
                    //Makes the tile and its RigidBody
                    TiledTexture tile = new TiledTexture($"[{i},{j}]", null, Vector2.Zero, Vector2.One);
                    tiles[i, j] = tile;
                    RigidBody rb = new RigidBody(Vector2.One * _tileSize, Vector2.Zero, isStatic: true);
                    tileRbs[i, j] = rb;
                    tile.AddComponent(rb);
                    tile.WorldPosition = new Vector2(i, j) - origin;
                    tile.LayerDepth = 0f;

                    //Assigns to each color on the layout a type of tile
                    if (col == Color.White)
                    {
                        tile.Texture = _border;
                        tile.Tag = "Border " + tile.Tag;
                    }
                    else if (col == Color.Yellow)
                    {
                        tile.Texture = _wall;
                        tile.Tag = "Wall " + tile.Tag;
                    }
                    else if (col == Color.Red)
                    {
                        tile.Texture = _rug;
                        rb.IsTrigger = true;
                        tile.Tag = "Rug " + tile.Tag;
                    }
                    else if (col == Color.Cyan)
                    {
                        tile.Texture = null;
                        rb.IsTrigger = true;
                        tile.Tag = "OOB " + tile.Tag;
                    }
                    else if (col == new Color(0, 255, 0, 255)) //Green
                    {
                        tile.Texture = _floor;
                        rb.Remove();
                        tile.Tag = "Floor " + tile.Tag;
                        //Randomizes some with more texture and a random orientation
                        int randNum = rnd.Next(50);
                        if (randNum == 1)
                        {
                            tile.Texture = _floor2;
                            tile.Effects = SpriteEffects.FlipHorizontally;
                            colors2D[i, j] = Color.AliceBlue;
                            col = Color.AliceBlue;
                        }
                        else if (randNum == 2)
                        {
                            tile.Texture = _floor2;
                            tile.Effects = SpriteEffects.FlipVertically;
                            colors2D[i, j] = Color.AntiqueWhite;
                            col = Color.AntiqueWhite;
                        }
                        else if (randNum == 3)
                        {
                            tile.Texture = _floor2;
                            colors2D[i, j] = Color.Aqua;
                            col = Color.Aqua;
                        }
                    }
                    else if (col == Color.Blue)
                    {
                        tile.Texture = null;
                        tile.Tag = "OutBorder " + tile.Tag;
                    }
                    else
                    {
                        //If the color is not on the list, remove the tile
                        tile.Destroy();
                        tiles[i, j] = null;
                        tile = null;
                        //...I'm really trying to get the garbage collector's attention here
                        continue;
                    }

                    //This merges adjacent tiles of the same type into a single object with a single TiledTexture and Rigidbody, horizontally
                    if (i > 0)
                    {
                        if (colors2D[i - 1, j] == col)
                        {
                            TiledTexture oldTile = tiles[i - 1, j];
                            RigidBody oldRb = tileRbs[i - 1, j];

                            if (rb != null)
                            {
                                rb.Size = new Vector2(rb.Size.X + oldRb.Size.X, rb.Size.Y);
                                oldRb.Remove();
                                tileRbs[i - 1, j] = null;
                                oldRb = null;
                            }
                            tileRbs[i - 1, j] = null;

                            tile.Size = new Vector2(tile.Size.X + oldTile.Size.X, tile.Size.Y);
                            tile.Position = new Vector2(tile.Position.X - _tileSize * oldTile.Size.X / 2f, tile.Position.Y);
                            oldTile.Destroy();
                            oldTile = null;
                            tiles[i - 1, j] = null;
                        }
                    }
                }
            }

            //After they were all created and merged horizontally, this merges them vertically if they're of the same width
            //I *think* this optimizes the number of GameObjects for a given shape
            for (int i = 0; i < _layoutTexture.Width; i++)
            {
                for (int j = 1; j < _layoutTexture.Height; j++)
                {
                    Color col = colors2D[i, j];
                    RigidBody rb = tileRbs[i, j];
                    RigidBody oldRb = tileRbs[i, j - 1];
                    TiledTexture tile = tiles[i, j];
                    TiledTexture oldTile = tiles[i, j - 1];

                    //This skips already removed tiles and the floor tiles which, because of the random tiles, don't get merged
                    if (colors2D[i, j - 1] == col && tile != null && oldTile != null)
                    {
                        if (oldTile.Size.X == tile.Size.X)
                        {
                            if (rb != null)
                            {
                                rb.Size = new Vector2(rb.Size.X, rb.Size.Y + oldRb.Size.Y);
                                oldRb.Remove();
                                tileRbs[i, j - 1] = null;
                                oldRb = null;
                            }

                            tile.Size = new Vector2(tile.Size.X, tile.Size.Y + oldTile.Size.Y);
                            tile.Position = new Vector2(tile.Position.X, tile.Position.Y - _tileSize * oldTile.Size.Y / 2f);
                            oldTile.Destroy();
                            tiles[i, j - 1] = null;
                            oldTile = null;
                        }
                    }
                    //Finally, it adds the tile to the Scene (and if it gets merged by a latter tile it gets destroyed)
                    //If I put this in another for loop it ends up with way more GameObjects, I'm probably not deleting them properly when they're merged
                    if (tile != null)
                    {
                        Parent.Scene.AddGameObject(tile);
                        tile.SetParent(Parent);
                    }
                }
            }
        }
    }
}
