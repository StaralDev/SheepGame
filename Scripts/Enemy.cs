using Godot;
using SheepGame;
using System;

public partial class Enemy : CharacterBody2D
{
	public const float Speed = 300.0f;

	private Sparky sparky;

	private AnimatedSprite2D sprite;
	private CollisionShape2D enemyCollider;
	private Area2D enemySightbox;
	private CollisionShape2D enemySightboxCollider;
	private NavigationAgent2D navigationAgent;

    public override void _Ready()
    {
		sparky = Overworld.GetSparky(GetTree());

        sprite = GetNode<AnimatedSprite2D>("Sprite");
		enemyCollider = GetNode<CollisionShape2D>("EnemyCollider");
		enemySightbox = GetNode<Area2D>("EnemySightbox");
		enemySightboxCollider = enemySightbox.GetNode<CollisionShape2D>("EnemySightboxCollider");
		navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
	}

    public override void _PhysicsProcess(double delta)
    {
        navigationAgent.TargetPosition = Position;
    }

    public override void _Process(double delta)
    {
        
    }
}
