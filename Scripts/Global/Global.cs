using Godot;
using SheepGame;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Global : Node2D
{

    public struct Bilboard {
        public Node2D node;
        public float overlapOffset;
    }

    private List<Bilboard> Bilboards = new();
    private List<Node2D> Overlays = new();

    public struct PasifiedData {
        public bool Red;
        public bool Yellow;
        public bool Green;
        public bool Blue;
    }

    public struct PlayerData {
        public int Health;
        public Balloons? currentBalloon;
        public PasifiedData pasifiedData;
    }

    public PlayerData myData = new PlayerData{
        Health = 3,
        currentBalloon = null,
        pasifiedData = new PasifiedData{
            Red = false,
            Green = false,
            Yellow = false,
            Blue = false
        }
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

    public void ClearBilboards()
    {
        Bilboards.Clear();
    }

    public void AddOverlay(Node2D node)
    {
        Overlays.Insert(0, node);
    }

    private void setSceneDefered(PackedScene packedScene)
    {
        SceneTree tree = GetTree();
        tree.CurrentScene.QueueFree();

        ClearBilboards();

        var newScene = Overworld.InstantiateScene(packedScene);
        tree.Root.AddChild(newScene);
        tree.CurrentScene = newScene;
    }

    public void SetScene(PackedScene packedScene)
    {
        CallDeferred(MethodName.setSceneDefered, packedScene);
    }

    public override void _Ready()
    {
        GD.Randomize();
    }

    private static int CompareBilboards(Bilboard a, Bilboard b)
    {
        if (a.node == null && b.node != null)
        {
            return 0.CompareTo(b.node.Position.Y + b.overlapOffset);
        }

        if (b.node == null && a.node != null)
        {
            return (a.node.Position.Y + a.overlapOffset).CompareTo(0);
        }

        if (a.node == null && b.node == null)
        {
            return 0;
        }

        try
        {
            return (a.node.Position.Y + a.overlapOffset).CompareTo(b.node.Position.Y + b.overlapOffset);
        } 
        catch
        {
            return 0;
        }
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustReleased("sg_quit"))
        {
            GetTree().Quit();
        }


        Bilboards.Sort(CompareBilboards);

        foreach (Bilboard bilboard in Bilboards) {
            if (bilboard.node == null) { continue; }
            bilboard.node.MoveToFront();
        }

        foreach (Node2d node in Overlays)
        {
            node.MoveToFront();
        }
    }
}