using Godot;
using SheepGame;
using System;

public partial class Global : Godot.Node
{

    public struct PlayerData {
        public int Health;
        public Balloons? currentBalloon;
    }

    public PlayerData myData = new PlayerData{
        Health = 3,
        currentBalloon = null,
    };

    public void SetScene(PackedScene packedScene)
    {
        CallDeferred(MethodName.setSceneDefered, packedScene);
    }

    private void setSceneDefered(PackedScene packedScene)
    {
        SceneTree tree = GetTree();
        tree.CurrentScene.QueueFree();

        var newScene = Overworld.InstantiateScene(packedScene);
        tree.Root.AddChild(newScene);
        tree.CurrentScene = newScene;
    }

    public override void _Ready()
    {
        GD.Randomize();
    }

    public override void _Process(double delta)
    {
        
    }
}