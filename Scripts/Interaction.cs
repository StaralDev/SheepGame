using Godot;
using SheepGame;
using System;

public partial class Interaction : Area2D
{
	// Define what type this is. either balloon or Game
	public string self;
	public string selfName;

	public Sparky sparky;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Visible = false;
		sparky = GetTree().CurrentScene.GetNode<Sparky>("Sparky");
		GD.Print(sparky);
	}

	public void _on_area_entered(Area2D area)
	{
		this.Visible = true;
	}

	public void _on_area_exited(Area2D area)
	{
		this.Visible = false;
	}

    public override void _Process(double delta)
    {
        if (Input.IsActionPressed("Interact") && this.Visible == true)
		{
			GD.Print(this.GetParent());
			if (self == "balloon");
			{
				//sparky.HasGreenBalloon;
				QueueFree();
			}
		}
    }
}
