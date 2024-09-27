using Godot;
using SheepGame;
using System;

public partial class WinScene : Node2D
{
	public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("sg_interact"))
		{
			Overworld.ChangeScene(GD.Load<PackedScene>("res://Scenes/title_screen.tscn"), GetTree());
		}
	}
}
