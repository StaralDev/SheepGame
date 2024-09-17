using System;
using Godot;

namespace SheepGame
{
    class Overworld
    {

/// <summary>
/// I'm the only person using this so this shouldn't be a problem but just in case
/// please don't use this often, just use it at the start of your script pls
/// </summary>
/// <returns>Sparky</returns>
        public static Sparky GetSparky(SceneTree sceneTree)
        {
            foreach(Node node in sceneTree.CurrentScene.GetChildren(true))
            {
                if (typeof(Sparky) == node.GetType())
                {
                    return node as Sparky;
                }
            }

            return null;
        }

        public static T InstantiateScene<T>(PackedScene packedScene)
        {
            var newScene = packedScene.Instantiate<Godot.Node>();
            return (T)Convert.ChangeType(newScene, typeof(T));
        }

        public static T InstantiateScene<T>(string scenePath)
        {
            return InstantiateScene<T>(GD.Load<PackedScene>(scenePath));
        }

        public static void ChangeScene()
        {
            
        }

        private static void changeSceneDefered()
        {

        }

    }
}