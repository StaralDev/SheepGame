using Godot;
using SheepGame;
using System;

public partial class Cage : Interaction
{
	private Global globalObject;
    public override void _Ready()
    {
        base._Ready();

		globalObject = Overworld.GetGlobal(GetTree());
    }
    public override void _OnInteraction()
    {
        JumpscareGui jumpscare = Overworld.InstantiateScene("res://Replicatables/Gui/JumpscareGui.tscn") as JumpscareGui;
		globalObject.AddChild(jumpscare);

		jumpscare.CenterImage.Texture = GD.Load<Texture2D>("res://Sprites/Clowns/CageBadJumpScare.png");
		jumpscare.offset = false;
		jumpscare.flashColor = false;
    }
}
