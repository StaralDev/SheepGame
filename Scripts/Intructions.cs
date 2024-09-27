using Godot;
using SheepGame;
using System;

public partial class Intructions : Node2D
{
	Sprite2D giveballoontoclownsprite;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		giveballoontoclownsprite = GetNode<Sprite2D>("GiveBaloonToClown");

		GetNode<Timer>("Timer").Timeout += () => {
			var SceneTransition = Overworld.InstantiateScene("res://Replicatables/SceneTransition.tscn") as SceneTransition;
			SceneTransition.NewScene = "res://Scenes/MapMain.tscn";
			SceneTransition.TransitionTime = 4.0f;

			Overworld.GetGlobal(GetTree()).AddChild(SceneTransition);
		};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		string selectColor = "White";
		if (GD.Randi()%25 == 0)
		{
			selectColor = "Black";
		}
		giveballoontoclownsprite.Modulate = new Color(selectColor);

		giveballoontoclownsprite.Offset = new Vector2(
			Mathf.Clamp((GD.Randi()%20)-10, -10, 10),
			Mathf.Clamp((GD.Randi()%20)-10, -10, 10)
		)/7;
	}
}
