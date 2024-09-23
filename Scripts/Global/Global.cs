using Godot;
using SheepGame;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Global : Godot.Node
{

    public struct Bilboard {
        public Node2D node;
        public float overlapOffset;
    }

    private List<Bilboard> Bilboards = new();
    private List<Node2D> Overlays = new();

    public struct PlayerData {
        public int Health;
        public Balloons? currentBalloon;
    }

    public PlayerData myData = new PlayerData{
        Health = 3,
        currentBalloon = null,
    };

    public void CreateBilboard(Node2D node, float overlapOffset)
    {
        Bilboards.Add(new Bilboard {
            node = node,
            overlapOffset = overlapOffset
        });
    }

    public void CreateBilboard(Bilboard bilboard)
    {
        Bilboards.Add(bilboard);
    }

    public void AddOverlay(Node2D node)
    {
        Overlays.Insert(0, node);
    }

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

    private static int CompareBilboards(Bilboard a, Bilboard b)
    {
        return (a.node.Position.Y + a.overlapOffset).CompareTo(b.node.Position.Y + b.overlapOffset);
    }

    public override void _Process(double delta)
    {
        Bilboards.Sort(CompareBilboards);

        foreach (Bilboard bilboard in Bilboards) {
            bilboard.node.MoveToFront();
        }

        foreach (Node2d node in Overlays)
        {
            node.MoveToFront();
        }
    }
}