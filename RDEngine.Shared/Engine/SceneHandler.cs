using Microsoft.Xna.Framework.Content;

namespace RDEngine.Engine
{
    public static class SceneHandler
    {
        public static Scene ActiveScene;
        public static ContentManager Content;
        public static void LoadScene(Scene scene)
        {
            ActiveScene = null;

            ActiveScene = scene;
            ActiveScene.Initialize(Content);
        }
    }
}