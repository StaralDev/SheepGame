using System;
using Godot;
using SheepGame;

public partial class Clown : Enemy
{
	[Export]
	public SpriteFrames spriteFrames { get; set; }

	[Export]
	public bool Pacified { get; set; } = false;

	[Export]
	public bool FlipX = false;
	[Export]
	public bool DecidedToChangeDirectionToFlipOnOneOfTheFourClownsButThisScriptIsInheritedByTheClownSoYouNeedANewPropertyForOneSingleThing = false;

	[Export]
	public Texture2D JumpscareImage;

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

	private void disableCollision()
	{
		enemyCollider.Disabled = true;
	}

    public override void _Ready()
    {
		transparencyDirection = 1;

		MapSize = new Vector2(4400, 3500);
		MapCenter = Vector2.Zero;

        base._Ready();

		sawSparkyTimer.Timeout += () => {
			ExclamationPoint.Visible = false;
		};

		spawnPoints = Overworld.GetEnemySpawnPoints(GetTree().CurrentScene);
		lastDirection = new Vector2(0, 1);

		sprite.SpriteFrames = spriteFrames;

		sprite.Animation = animation;

		globalObject.CreateBilboard(this, 8);

		enemyKillbox.AreaEntered += (myArea) => {
			if (Pacified) { return; }
			Node parent = myArea.GetParent();
			if (myArea.Name == "SheepHitbox" && parent.GetType() == typeof(Sparky))
			{
				if (globalObject.myData.currentBalloon.ToString() == ColorName)
				{
					Pacified = true;
					globalObject.myData.currentBalloon = null;
					sparky.UpdateBalloon();
					
					sparky.Persue(ColorName, false);

					CallDeferred(MethodName.disableCollision);					
				}
				else
				{
					JumpscareGui jumpscareGui = Overworld.InstantiateScene("res://Replicatables/Gui/JumpscareGui.tscn") as JumpscareGui;
					jumpscareGui.Speed = 0.2f;
					globalObject.AddChild(jumpscareGui);
					jumpscareGui.CenterImage.Texture = JumpscareImage;

					globalObject.myData.Health -= 1;
					sparky.UpdateHealth();
					if (globalObject.myData.Health <= 0)
					{
						Overworld.ChangeScene("res://Scenes/DeathScene.tscn", GetTree());
					}
					else
					{
						searching = true;
						transparencyDirection = 1;

						currentSpeed = SearchSpeed;
						var newSpawnPoint = Overworld.GetRandomEnemySpawnPoint(spawnPoints);
						Position = newSpawnPoint.Position;
						navigationAgent.TargetPosition = new Vector2(
							(GD.Randi()%(MapSize.X*2)) + MapCenter.X - MapSize.X,
							(GD.Randi()%(MapSize.Y*2)) + MapCenter.Y - MapSize.Y
						);
					}
				}
			}
		};

		enemyLostTimer.Timeout += () => {
			navigationAgent.TargetPosition = new Vector2(
				(GD.Randi()%(MapSize.X*2)) + MapCenter.X - MapSize.X,
				(GD.Randi()%(MapSize.Y*2)) + MapCenter.Y - MapSize.Y
			);
			searching = lost;

			if (searching)
			{
				transparencyDirection = -1;
			}

			lost = false;
		};
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

		PathfindingEnable = !Pacified;

		if (PathfindingEnable && navigationAgent != null)
		{
			Vector2 nextPosition = navigationAgent.GetNextPathPosition();

			if ((nextPosition-Position).Length() > 10) //&& (Position-navigationAgent.TargetPosition).Length() < focusDistance
			{
				resting = false;
				lastDirection = lockDirection(Position.DirectionTo(nextPosition));

				enemySightboxCollider.Position = lastDirection * 213;
				if (lastDirection.X == 1)
				{
					enemySightboxCollider.Rotation = Mathf.DegToRad(-90f);
					enemySightbox.MoveToFront();
				}
				else if (lastDirection.X == -1)
				{
					enemySightboxCollider.Rotation = Mathf.DegToRad(90f);
					enemySightbox.MoveToFront();
				}
				else if (lastDirection.Y == 1) 
				{
					enemySightboxCollider.Rotation = Mathf.DegToRad(0f);
					enemySightbox.MoveToFront();
				}
				else if (lastDirection.Y == -1)
				{
					enemySightboxCollider.Rotation = Mathf.DegToRad(180f);
					sprite.MoveToFront();
				}
			}
			else
			{
				resting = true;
				if (!lostSinceSeenLast)
				{
					lostSinceSeenLast = true;
					lost = true;
					navigationAgent.TargetPosition = Position;
					searching = true;
					currentSpeed = SearchSpeed;
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

		// Like and share if you hate magnus
		if (FlipX) {
			if (DecidedToChangeDirectionToFlipOnOneOfTheFourClownsButThisScriptIsInheritedByTheClownSoYouNeedANewPropertyForOneSingleThing)
			{
				if (lastDirection.X == -1)
				{
					sprite.Scale = new Vector2(-4, 4);
				}
				else
				{
					sprite.Scale = new Vector2(4, 4);
				}
			}
			else
			{
				if (lastDirection.X == 1)
				{
					sprite.Scale = new Vector2(-4, 4);
				}
				else
				{
					sprite.Scale = new Vector2(4, 4);
				}
			}
		}

		if (Pacified)
		{
			sprite.SpeedScale = 0.5f;
			if (!spritePlaying)
			{
				sprite.Play();
			}
		} else if (!lost) {
			if (spritePlaying && (lastDirection == Vector2.Zero || resting))
			{
				sprite.Stop();
				sprite.SpeedScale = 1f;
				sprite.Frame = 0;
			} 
			else if (!spritePlaying && lastDirection != Vector2.Zero) 
			{
				sprite.Play();
				sprite.SpeedScale = 1f;
			}
		}
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

		if (Pacified)
		{
			transparencyDirection = -1;
		}

		transparancyAmount = Mathf.Clamp(transparancyAmount + (transparencyDirection * (float)delta * 4), 0, 1);
		lightCone.Modulate = new Color(Colors.LightYellow, transparancyAmount/2);
    }
}
