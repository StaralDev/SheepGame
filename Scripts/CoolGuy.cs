using Godot;
using SheepGame;
using System;

public partial class CoolGuy : Interaction
{
	private Global global;
	private AnimatedSprite2D animatedSprite2D;
	private Sprite2D textboxSprite;
	private Sprite2D interact;

    public override void _Ready()
    {
        base._Ready();

		global = Overworld.GetGlobal(GetTree());
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		textboxSprite = GetNode<Sprite2D>("TextBox");
		interact = GetNode<Sprite2D>("InteractText");

		global.CreateBilboard(this, -50);

    }

    public override void _OnInteraction()
    {
		textboxSprite.Visible = true;
        if (!animatedSprite2D.IsPlaying())
		{
			animatedSprite2D.Play();
		}
    }

    public override void _OnSparkyExited(Sparky character)
    {
        textboxSprite.Visible = false;
		if (animatedSprite2D.IsPlaying())
		{
			animatedSprite2D.Stop();
			animatedSprite2D.Frame = 0;
		}
    }

	public void InteactEnter()
	{
		interact.Visible = true;
	}
	public void InteractExit()
	{
		interact.Visible = false;
	}
}
