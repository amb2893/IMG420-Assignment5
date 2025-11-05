using Godot;

public partial class LaserDetector : Node2D
{
	[Export] public float LaserLength = 500f;
	[Export] public Color LaserColorNormal = Colors.Green;
	[Export] public Color LaserColorAlert = Colors.Red;
	[Export] public NodePath PlayerPath;

	private RayCast2D _rayCast;
	private Line2D _laserBeam;
	private Node2D _player;
	private bool _isAlarmActive = false;
	private Timer _alarmTimer;

	public override void _Ready()
	{
		// Get player reference
		_player = GetNode<Node2D>(PlayerPath);

		SetupRaycast();
		SetupVisuals();
		SetupAlarmTimer();
	}

	private void SetupRaycast()
	{
		_rayCast = new RayCast2D();
		AddChild(_rayCast);

		_rayCast.TargetPosition = new Vector2(LaserLength, 0); // horizontal laser
		_rayCast.CollideWithAreas = true;
		_rayCast.CollideWithBodies = true;
		_rayCast.Enabled = true;

		// Optional: set collision mask to detect only player layer
		// _rayCast.CollisionMask = 1 << 1; // example layer 1
	}

	private void SetupVisuals()
	{
		_laserBeam = new Line2D();
		AddChild(_laserBeam);

		_laserBeam.Width = 2f;
		_laserBeam.DefaultColor = LaserColorNormal;
		_laserBeam.Points = new Vector2[] { Vector2.Zero, _rayCast.TargetPosition };
	}

	private void SetupAlarmTimer()
	{
		_alarmTimer = new Timer();
		_alarmTimer.WaitTime = 1.0f; // alarm duration
		_alarmTimer.OneShot = true;
		_alarmTimer.Connect("timeout", new Callable(this, "ResetAlarm"));
		AddChild(_alarmTimer);
	}

	public override void _PhysicsProcess(double delta)
	{
		_rayCast.ForceRaycastUpdate();
		UpdateLaserBeam();

		if (_rayCast.IsColliding())
		{
			var collider = _rayCast.GetCollider();
			if (collider == _player && !_isAlarmActive)
			{
				TriggerAlarm();
			}
		}
	}

	private void UpdateLaserBeam()
	{
		Vector2 endPoint = _rayCast.TargetPosition;

		if (_rayCast.IsColliding())
		{
			endPoint = _rayCast.GetCollisionPoint() - GlobalPosition;
		}

		_laserBeam.Points = new Vector2[] { Vector2.Zero, endPoint };
	}

	private void TriggerAlarm()
	{
		_isAlarmActive = true;
		_laserBeam.DefaultColor = LaserColorAlert;

		// Visual feedback: flash line
		_alarmTimer.Start();

		// Optional: play sound here
		GD.Print("ALARM! Player detected!");
	}

	private void ResetAlarm()
	{
		_isAlarmActive = false;
		_laserBeam.DefaultColor = LaserColorNormal;
	}
}
