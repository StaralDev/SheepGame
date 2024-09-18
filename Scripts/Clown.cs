using System;
using Godot;
using SheepGame;

public partial class Clown : Enemy
{
	[Export]
	public SpriteFrames spriteFrames;

	public bool Pacified = false;

	private Vector2 lastDirection;
	private string animation = "WalkUp";

	private bool resting = false;
	private EnemySpawnPoint[] spawnPoints;

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
		if (Pacified)
		{
			return "Pacified";
		}

		if (lost)
		{
			return "LostAnimation";
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
		MapSize = new Vector2(4400, 3500);
		MapCenter = Vector2.Zero;

        base._Ready();
		spawnPoints = Overworld.GetEnemySpawnPoints(GetTree().CurrentScene);
		lastDirection = new Vector2(0, 1);

		sprite.SpriteFrames = spriteFrames;

		sprite.Animation = animation;

		enemyKillbox.AreaEntered += (myArea) => {
			if (Pacified) { return; }
			Node parent = myArea.GetParent();
			if (myArea.Name == "SheepHitbox" && parent.GetType() == typeof(Sparky))
			{
				globalObject.myData.Health -= 1;
				if (globalObject.myData.Health <= 0)
				{
					Overworld.ChangeScene("res://Scenes/DeathScene.tscn", GetTree());
				}
				else
				{
					searching = true;
					var newSpawnPoint = Overworld.GetRandomEnemySpawnPoint(spawnPoints);
					Position = newSpawnPoint.Position;
					navigationAgent.TargetPosition = new Vector2(
						(GD.Randi()%MapSize.X) + MapCenter.X,
						(GD.Randi()%MapSize.Y) + MapCenter.Y
					);
					/*Vector2 chosenPosition = Vector2.Zero;
					while (chosenPosition == Vector2.Zero || (chosenPosition-sparky.Position).Length() <= focusDistance)
					{
						chosenPosition = new Vector2(
							(GD.Randi()%MapSize.X) + MapCenter.X,
							(GD.Randi()%MapSize.Y) + MapCenter.Y
						);
					}

					Position = chosenPosition;
					*/
				}
			}
		};

		enemyLostTimer.Timeout += () => {
			lost = false;
		};
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

		PathfindingEnable = !Pacified;

		if (PathfindingEnable)
		{
			Vector2 nextPosition = navigationAgent.GetNextPathPosition();

			if ((nextPosition-Position).Length() > 10) //&& (Position-navigationAgent.TargetPosition).Length() < focusDistance
			{
				resting = false;
				lastDirection = lockDirection(Position.DirectionTo(nextPosition));

				enemySightboxCollider.Position = lastDirection * 213;
				enemySightboxCollider.Rotation = Mathf.DegToRad(90f - (lastDirection.Y * 90f));
			}
			else
			{
				resting = true;
				if (!lostSinceSeenLast)
				{
					lostSinceSeenLast = true;
					lost = true;
					enemyLostTimer.Start();
					sprite.SpeedScale = 1f;
					sprite.Frame = 0;
					sprite.Play();
				}
			}
		}

		animation = GetAnimationFromDirection(lastDirection);
		sprite.Animation = animation;

		bool spritePlaying = sprite.IsPlaying();

		if (!lost) {
			if (Pacified && !spritePlaying)
			{
				sprite.Play();
				sprite.SpeedScale = 0.5f;
			} else if (spritePlaying && (lastDirection == Vector2.Zero || resting))
			{
				sprite.Stop();
				sprite.SpeedScale = 1f;
				sprite.Frame = 0;
			} else if (!spritePlaying && lastDirection != Vector2.Zero) 
			{
				sprite.Play();
				sprite.SpeedScale = 1f;
			}
		}
    }
}
