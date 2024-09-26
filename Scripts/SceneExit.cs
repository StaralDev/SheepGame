using Godot;
using SheepGame;
using System;

public partial class SceneExit : Interaction
{
	[Export]
	public bool RunOnEntered { get; set; } = true;

	[Export]
	public bool Enabled { get; set; } = true;

	[Export]
	public float TransitionTime { get; set; } = 2.0f;

	[Export]
	public string NewScene {get; set; }

	private Global global;
	private bool lockAfterAction = false;

    private void run()
	{
		var newTransition = Overworld.InstantiateScene(GD.Load<PackedScene>("res://Replicatables/SceneTransition.tscn")) as SceneTransition;
		newTransition.TransitionTime = TransitionTime;
		newTransition.NewScene = NewScene;
		global.AddChild(newTransition);
	}

    public override void _Ready()
    {
        base._Ready();
		
		global = Overworld.GetGlobal(GetTree());
    }

    public override void _OnSparkyEntered(Sparky character)
    {
		if (!Enabled) { return; }
		if (!RunOnEntered) { return; }
        if (lockAfterAction) { return; }
		lockAfterAction = true;
		run();
    }

    public override void _OnInteraction()
    {
		if (!Enabled) { return; }
		if (RunOnEntered) { return; }
		if (lockAfterAction) {return; }
		lockAfterAction = true;
		run();
    }
}
