using Godot;

public partial class ParticleController : GpuParticles2D
{
	private ShaderMaterial _shaderMat;

	[Export] public float WaveBase = 0.08f;
	[Export] public float WavePulseAmplitude = 0.05f;
	[Export] public float WavePulseSpeed = 1.5f;

	public override void _Ready()
	{
		// Load and assign shader
		var shader = GD.Load<Shader>("res://Shaders/custom_particle.gdshader");
		_shaderMat = new ShaderMaterial { Shader = shader };
		Material = _shaderMat;

		// Create and set up particle process material
		var pm = new ParticleProcessMaterial
		{
			EmissionShape = ParticleProcessMaterial.EmissionShapeEnum.Point,
			Direction = new Vector3(0, -1, 0),
			Spread = 0.8f,
			Gravity = new Vector3(0, 80, 0),
			InitialVelocityMin = 90f,
			InitialVelocityMax = 150f,
			ScaleMin = 0.6f,
			ScaleMax = 1.2f,
			Color = new Color(1, 1, 1, 1)
		};
		ProcessMaterial = pm;

		// These properties belong to GpuParticles2D
		Lifetime = 1.0f;
		Preprocess = 0.5f;
		SpeedScale = 1.0f;

		Amount = 20;
		Emitting = true;
	}

	public override void _Process(double delta)
	{
		if (_shaderMat == null) return;

		float t = (float)Time.GetTicksMsec() / 1000f;
		float wave = WaveBase + Mathf.Sin(t * WavePulseSpeed) * WavePulseAmplitude;
		_shaderMat.SetShaderParameter("wave_intensity", wave);
	}
}
