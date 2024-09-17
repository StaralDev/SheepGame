using Godot;
using System;
using System.ComponentModel;

public partial class Clown : Enemy
{
	[Export]
	public SpriteFrames spriteFrames;

	public bool Pasified = true;

	private Vector2 lastDirection;
	private string animation = "WalkUp";

	private Vector2 lockDirection(Vector2 dir)
	{
		if (dir.X != 0 || dir.Y != 0)
		{
			if (Mathf.Abs(dir.X) > Mathf.Abs(dir.Y))
			{
				return new Vector2(Mathf.Sign(dir.X), 0);
			}
			else
			{
				return new Vector2(0, Mathf.Sign(dir.Y));
			}
		}

		return lastDirection;
	}

	private string GetAnimationFromDirection(Vector2 dir)
	{
		if (Pasified)
		{
			return "Pacified";
		}

		if (dir.X == 0 && dir.Y == 0) { return animation; }
		
		if (Mathf.Abs(dir.X) > Mathf.Abs(dir.Y)) {
			
			if (dir.X > 0) 
			{
				return "WalkRight";
			}
			else
			{
				return "WalkLeft";
			}

		}
		else
		{

			if (dir.Y > 0) 
			{
				return "WalkDown";
			}
			else
			{
				return "WalkUp";
			}
		}
	}

    public override void _Ready()
    {
        base._Ready();
		lastDirection = new Vector2(0, 1);

		sprite.SpriteFrames = spriteFrames;

		sprite.Animation = animation;
		GD.Print(sprite.Animation);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
		Vector2 nextPosition = navigationAgent.GetNextPathPosition();

		lastDirection = lockDirection(Position.DirectionTo(nextPosition));

		enemySightboxCollider.Position = lastDirection * 213;
		enemySightboxCollider.Rotation = Mathf.DegToRad(90f - (lastDirection.Y * 90f));

		animation = GetAnimationFromDirection(lastDirection);
		sprite.Animation = animation;

		bool spritePlaying = sprite.IsPlaying();

		if (Pasified && !spritePlaying) 
		{
			sprite.Play();
		} else if (spritePlaying && lastDirection == Vector2.Zero)
		{
			sprite.Stop();
		} else if (!spritePlaying && lastDirection != Vector2.Zero) 
		{
			sprite.Play();
		}
    }
}
