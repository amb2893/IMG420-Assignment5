using Godot;

public partial class Player : CharacterBody2D
{
	[Export] public float Speed = 200f;
	[Export] public NodePath ChainPath;
	private PhysicsChain _chain;

	public override void _Ready()
	{
		if (ChainPath != null)
			_chain = GetNode<PhysicsChain>(ChainPath);
	}

	public override void _PhysicsProcess(double delta)
	{
		// Reset velocity
		Velocity = Vector2.Zero;

		// Input
		Vector2 input = new Vector2(
			Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
			Input.GetActionStrength("move_down") - Input.GetActionStrength("move_up")
		);

		if (input.Length() > 0)
			Velocity = input.Normalized() * Speed;

		// Move player
		MoveAndSlide();

		// Push nearby chain segments
		if (_chain != null)
		{
			_chain.ApplyForceAtPoint(GlobalPosition, Velocity * 1.2f, 50f);
		}
	}
}
