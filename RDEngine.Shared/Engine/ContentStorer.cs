using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace RDEngine.Engine
{
    public static class ContentStorer
    {
        public static Dictionary<string, Texture2D> Textures;
        public static Dictionary<string, SpriteFont> Fonts;
        public static Dictionary<string, Song> Songs;
        public static Dictionary<string, SoundEffect> SFX;

        public static Texture2D WhitePixel;
        public static Texture2D WhiteSquare;

        public static void LoadContent(ContentManager content, List<string> textures, List<string> fonts, List<string> songs, List<string> sfx)
        {
            Textures = new Dictionary<string, Texture2D>();
            Fonts = new Dictionary<string, SpriteFont>();
            Songs = new Dictionary<string, Song>();
            SFX = new Dictionary<string, SoundEffect>();

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
            foreach (var name in songs)
            {
                Song song = content.Load<Song>("Songs/" + name);
                Songs.Add(name, song);
            }
            foreach (var name in sfx)
            {
                SoundEffect sf = content.Load<SoundEffect>("SoundEffects/" + name);
                SFX.Add(name, sf);
            }
            WhitePixel = content.Load<Texture2D>("Sprites/whitepixel");
            WhiteSquare = content.Load<Texture2D>("Sprites/whitesquare");
        }
    }
}