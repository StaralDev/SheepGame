using Godot;
using SheepGame;
using System;

public partial class TitleScreen : Node2D
{
	public PackedScene simultaneousScene;
	public int selected = 1;

    public override void _Ready()
    {
        var global = Overworld.GetGlobal(GetTree());
		simultaneousScene = ResourceLoader.Load<PackedScene>("res://Scenes/map.tscn");
    }

    public void play()
	{
		if (selected == 1)
		{
			var global = Overworld.GetGlobal(GetTree());
			simultaneousScene = ResourceLoader.Load<PackedScene>("res://Scenes/map.tscn");
			global.SetScene(simultaneousScene);
		}
		else if (selected == 2);
		{
			GetTree().Quit();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("sg_interact"))
		{
			GD.Print("Helo");
			play();
		}
		if (Input.IsActionPressed("sg_up"))
		{
			selected = 1;
			GD.Print(selected);
		}
		else if (Input.IsActionPressed("sg_down"))
		{
			selected = 2;
			GD.Print(selected);
		}
	}
}
