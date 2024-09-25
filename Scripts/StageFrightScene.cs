using Godot;
using SheepGame;
using System;

public partial class StageFrightScene : Node2D
{
	
	private Timer timer;
	private Timer moveon;
	private Sprite2D stageScare;
	private Global global;

	public override void _Ready()
	{
		stageScare = GetNode<Sprite2D>("StageScare");
		global = Overworld.GetGlobal(GetTree());

		timer = GetNode<Timer>("Timer");
		timer.Timeout += () => {
			stageScare.Visible = true;
		};
		timer.Start();
	
		moveon = GetNode<Timer>("MoveOn");
		moveon.Timeout += () => {
			var SceneTransition = Overworld.InstantiateScene("res://Replicatables/SceneTransition.tscn") as SceneTransition;
			SceneTransition.NewScene = "res://Scenes/map.tscn";
			SceneTransition.TransitionTime = 4.0f;

			global.AddChild(SceneTransition);
		};
		moveon.Start();
	}

	public override void _Process(double delta)
	{
	}
}
