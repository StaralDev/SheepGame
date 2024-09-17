using Godot;
using SheepGame;
using System;

public partial class Global : Godot.Node
{

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
        
    }

    public override void _Process(double delta)
    {
        
    }
}