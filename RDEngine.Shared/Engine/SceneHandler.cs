using Microsoft.Xna.Framework.Media;

namespace RDEngine.Engine
{
    public static class SceneHandler
    {
        public static Scene ActiveScene;

        private static Song _activeSong;
        public static Song ActiveSong
        {
            get
            {
                if (MediaPlayer.State == MediaState.Playing)
                    return _activeSong;
                else return null;
            }
        }
        public static void ReloadScene(bool stopMusic = false)
        {
            ActiveScene.Initialize();
        }
        public static void LoadScene(Scene scene)
        {
            /*if (ActiveScene != null)
                ActiveScene.OnDelete();*/
            
            ActiveScene = null;

            ActiveScene = scene;
            ActiveScene.Initialize();
        }
        public static void PlaySong(Song song, bool loop)
        {
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = loop;
            _activeSong = song;
        }
    }
}