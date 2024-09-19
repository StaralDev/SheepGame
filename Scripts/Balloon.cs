using Godot;
using SheepGame;
using System;

public partial class Balloon : Interaction
{

    [Export]
    public string BalloonColor { get; set; } = "Red";

    [Export]
    public float BalloonFloatSpeed { get; set; } = 0.4f;

    [Export]
    public float BalloonSwaySpeed { get; set; } = 0.13f;

    [Export]
    public float BalloonWindSpeed { get; set; } = 3f;

    protected Sprite2D sprite;

	private Global myGlobalObject;
    

    public override void _Ready()
    {
        base._Ready();

		myGlobalObject = Overworld.GetGlobal(GetTree());

        sprite = GetNode<Sprite2D>("Sprite2D");
    }

    private Balloons getEnumFromBalloonColor(string colorString)
    {
        if (colorString == "Red")
        {
            return Balloons.Red;
        }
        else if (colorString == "Yellow")
        {
            return Balloons.Yellow;
        }
        else if (colorString == "Green")
        {
            return Balloons.Green;
        }
        else if (colorString == "Blue")
        {
            return Balloons.Blue;
        }

        return Balloons.Red;
    }

    public override void _OnInteraction()
    {
        EnableInteraction = false;

		if (myGlobalObject.myData.currentBalloon == null)
		{
			myGlobalObject.myData.currentBalloon = getEnumFromBalloonColor(BalloonColor);
            sprite.Visible = false;
		}
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        var time = (float)Time.GetTicksMsec()/1000;

        sprite.Position = new Vector2(
            (Mathf.Sin(time * BalloonSwaySpeed) * 24) + (Mathf.Sin(time * BalloonWindSpeed) * 1f),
            Mathf.Round(Mathf.Sin(time * BalloonFloatSpeed) * 16 / 4) * 4
        );
    }
}
