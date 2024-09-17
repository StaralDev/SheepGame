using Godot;
using SheepGame;
using System;

public partial class Enemy : CharacterBody2D
{
	public const float WalkSpeed = 3.0f;
	public const float RunSpeed = 4.5f;

	private Sparky sparky;

	private AnimatedSprite2D sprite;
	private CollisionShape2D enemyCollider;
	private Area2D enemySightbox;
	private CollisionShape2D enemySightboxCollider;
	private NavigationAgent2D navigationAgent;
	private Area2D enemyKillbox;

	private bool allowProcess;

	private float currentSpeed;

    public override void _Ready()
    {
		currentSpeed = WalkSpeed;

        sprite = GetNode<AnimatedSprite2D>("Sprite");
		enemyCollider = GetNode<CollisionShape2D>("EnemyCollider");
		enemySightbox = GetNode<Area2D>("EnemySightbox");
		enemySightboxCollider = enemySightbox.GetNode<CollisionShape2D>("EnemySightboxCollider");
		navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		enemyKillbox = GetNode<Area2D>("EnemyKillbox");

		CallDeferred(MethodName.Setup);

		enemyKillbox.AreaEntered += (myArea) => {
			Node parent = myArea.GetParent();
			if (myArea.Name == "SheepHitbox" && parent.GetType() == typeof(Sparky))
			{
				Overworld.ChangeScene("res://Scenes/DeathScene.tscn", GetTree());
			}
		};
	}

    public override void _PhysicsProcess(double delta)
    {
		if (!allowProcess) { return; }

		sparky ??= Overworld.GetSparky(GetTree());
		navigationAgent.TargetPosition = sparky.Position;

        Vector2 nextPosition = navigationAgent.GetNextPathPosition();
		
		var agentVelocity = Position.DirectionTo(nextPosition) * currentSpeed;

		MoveAndCollide(agentVelocity);
	}

    public override void _Process(double delta)
    {
        
    }

	public void Setup()
	{
		allowProcess = true;
	}
}
