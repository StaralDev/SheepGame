using Godot;
using SheepGame;
using System;

public partial class SceneTransition : ScreenColor
{
	[Export]
	public string NewScene {get; set; }

	[Export]
	public float TransitionTime { get; set; } = 1.0f;

	private Global global;
	private float counter = 0.0f;

	private bool changedScene = false;

    public override void _Ready()
    {
        base._Ready();
		global = Overworld.GetGlobal(GetTree());
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
		counter += (float)delta;

		if (!changedScene && counter >= TransitionTime/2)
		{
			changedScene = true;
			if (NewScene != null)
			{
				global.SetScene(GD.Load<PackedScene>(NewScene));
			}
		}

		float interpolation = Mathf.Clamp(Mathsg.QuadBezier(0, 2.1f, 0, counter/TransitionTime), 0, 1);
		SetColor(Colors.Black, interpolation);

		if (counter >= TransitionTime)
		{
			QueueFree();
		}
    }
}
