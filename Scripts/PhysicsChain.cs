using Godot;
using System.Collections.Generic;

public partial class PhysicsChain : Node2D
{
	[Export] public int ChainSegments = 8;
	[Export] public float SegmentDistance = 35f;
	[Export] public PackedScene SegmentScene;

	private StaticBody2D _anchor;
	private readonly List<RigidBody2D> _segments = new();
	private readonly List<PinJoint2D> _joints = new();

	public override void _Ready()
	{
		if (SegmentScene == null)
		{
			GD.PrintErr("SegmentScene not assigned!");
			return;
		}

		CreateChain();
	}

	private void CreateChain()
	{
		// --- Anchor ---
		_anchor = new StaticBody2D();
		AddChild(_anchor);
		_anchor.Position = Vector2.Zero;

		var anchorShape = new CollisionShape2D();
		anchorShape.Shape = new RectangleShape2D { Size = new Vector2(30, 10) };
		_anchor.AddChild(anchorShape);

		Vector2 lastPos = _anchor.Position;

		// --- Segments ---
		for (int i = 0; i < ChainSegments; i++)
		{
			var segment = SegmentScene.Instantiate<RigidBody2D>();
			AddChild(segment);

			float spacing = SegmentDistance * 1.25f; // .95small overlap to keep tight
			segment.Position = lastPos + new Vector2(0, spacing);

			// Slight damping for smooth motion
			segment.GravityScale = 1f;
			segment.LinearDamp = 3f;
			segment.AngularDamp = 3f;

			_segments.Add(segment);

			// --- Joints ---
			var joint = new PinJoint2D();
			AddChild(joint);

			joint.NodeA = (i == 0) ? _anchor.GetPath() : _segments[i - 1].GetPath();
			joint.NodeB = segment.GetPath();

			// Pin at the connection point (between both segments)
			joint.GlobalPosition = (i == 0)
				? _anchor.GlobalPosition + new Vector2(0, spacing / 2f)
				: (_segments[i - 1].GlobalPosition + segment.GlobalPosition) / 2f;

			_joints.Add(joint);
			lastPos = segment.Position;
		}
	}

public void ApplyForceAtPoint(Vector2 point, Vector2 force, float radius = 50f)
{
	foreach (var segment in _segments)
	{
		float distance = segment.GlobalPosition.DistanceTo(point);

		if (distance <= radius)
		{
			// Scale force by proximity
			float intensity = Mathf.Lerp(1f, 0.3f, distance / radius);

			// Apply the force gently
			segment.ApplyCentralImpulse(force * intensity);

			// Also optionally nudge neighbors for smooth movement
			int index = _segments.IndexOf(segment);
			if (index > 0)
				_segments[index - 1].ApplyCentralImpulse(force * intensity * 0.5f); // previous link
			if (index < _segments.Count - 1)
				_segments[index + 1].ApplyCentralImpulse(force * intensity * 0.5f); // next link
		}
	}
}


}
