using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace RDEngine.Engine
{
    public static class ContentStorer
    {
        public static Dictionary<string, Texture2D> Textures;
        public static Dictionary<string, SpriteFont> Fonts;

        public static Texture2D WhitePixel;

        public static void LoadContent(ContentManager content, List<string> textures, List<string> fonts)
        {
            Textures = new Dictionary<string, Texture2D>();
            Fonts = new Dictionary<string, SpriteFont>();

            foreach (var name in textures)
            {
                Texture2D texture = content.Load<Texture2D>("Sprites/" + name);
                Textures.Add(name, texture);
            }
            foreach (var name in fonts)
            {
                SpriteFont font = content.Load<SpriteFont>("Fonts/" + name);
                Fonts.Add(name, font);
            }
            WhitePixel = content.Load<Texture2D>("Sprites/whitepixel");
        }
    }
}