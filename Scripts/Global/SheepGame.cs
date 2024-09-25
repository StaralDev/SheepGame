using System;
using System.Collections.Generic;
using Godot;

namespace SheepGame
{
    public enum Balloons {
        Red,
        Yellow,
        Green,
        Blue
    }

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

        public static Node InstantiateScene(PackedScene packedScene)
        {
            var newScene = packedScene.Instantiate<Node>();
            return newScene;
        }

        public static Node InstantiateScene(string scenePath)
        {
            return InstantiateScene(GD.Load<PackedScene>(scenePath));
        }

        public static Global GetGlobal(SceneTree sceneTree)
        {
            return sceneTree.Root.GetNode<Global>("/root/Global");
        }

        public static void ChangeScene(PackedScene newScene, SceneTree sceneTree)
        {
            var myGlobal = GetGlobal(sceneTree);
            myGlobal.SetScene(newScene);
        }

        public static void ChangeScene(string scenePath, SceneTree sceneTree)
        {
            var myGlobal = GetGlobal(sceneTree);
            myGlobal.SetScene(GD.Load<PackedScene>(scenePath));
        }

        public static EnemySpawnPoint[] GetEnemySpawnPoints(Node scene)
        {
            List<EnemySpawnPoint> list = new();
            foreach (Node node in scene.GetChildren(true))
            {
                if (node.GetType() == typeof(EnemySpawnPoint))
                {
                    list.Add(node as EnemySpawnPoint);
                }
            }

            return list.ToArray();
        }

        public static EnemySpawnPoint GetRandomEnemySpawnPoint(EnemySpawnPoint[] spawnPoints)
        {
            return spawnPoints[GD.Randi()%(spawnPoints.Length-1)];
        }

    }

    class Mathsg
    {
        public static float QuadBezier(float a, float b, float c, float t)
        {
            return Mathf.Lerp(Mathf.Lerp(a, b, t), Mathf.Lerp(b, c, t), t);
        }
    }
}