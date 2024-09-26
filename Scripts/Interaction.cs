using Godot;
using System;
using System.ComponentModel.Design;

public partial class Interaction : Area2D
{
	[Export]
	public bool EnableInteraction { get; set; } = true;

	protected bool SparkyInArea = false;
	#nullable enable
	protected Sparky? sparky;
	#nullable disable

	protected bool EnableInteractionInternal = true;

	public virtual void _OnInteraction() {}
	public virtual void _OnSparkyEntered(Sparky character) {}
	public virtual void _OnSparkyExited(Sparky character) {}

	private void updateSparkyInArea(Area2D area, bool enable)
	{
		Node parent = area.GetParent();
		if (parent.GetType() == typeof(Sparky))
		{
			if (enable) {
				sparky = parent as Sparky;
				_OnSparkyEntered(sparky);
			}
			else
			{
				if (sparky != null)
				{
					_OnSparkyExited(sparky);
					sparky = null;
				}
			}

			SparkyInArea = enable;
		}
	}

	public override void _Ready()
	{
		AreaEntered += (area) => {
			updateSparkyInArea(area, true);
		};

		AreaExited += (area) => {
			updateSparkyInArea(area, false);
		};
	}

    public override void _Process(double delta)
    {
        if (EnableInteraction && EnableInteractionInternal && SparkyInArea && sparky != null && Input.IsActionJustReleased("sg_interact"))
		{
			_OnInteraction();
		}
    }
}
