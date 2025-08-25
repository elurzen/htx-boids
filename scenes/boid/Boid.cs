using Godot;
using System;

public partial class Boid : CharacterBody2D
{
	private Species _species;

	public string Species => _species.SpeciesName;
    public float MinSpeed => _species.MinSpeed;
    public float MaxSpeed => _species.MaxSpeed;
    public float TurnRate => _species.TurnRate;
	public float CollisionRadius => _species.CollisionRadius;
	public float VisionRadius => _species.VisionRadius;
	public float SeparationWeight => _species.SeparationWeight;
	public float AlignmentWeight => _species.AlignmentWeight;
	public float CohesionWeight => _species.CohesionWeight;
	public float SpeedWeight => _species.SpeedWeight;
	public float WallWeight => _species.WallWeight;
	public float LightWeight => _species.LightWeight;
	public float JitterFrequency => _species.JitterFrequency;
	public float JitterWeight => _species.JitterWeight;
	public Color BoidColor => _species.BoidColor;

    public float Speed { get; set; } = 150f;
	public Vector2 CurrentDirection {get; private set;} = Vector2.Right;
	public bool CanJitter {get; private set;} = false;
	public Vector2 CurrentJitter = Vector2.Zero;

	private Polygon2D _skin;
	public Timer JitterTimer {get; private set;}

    public override void _Ready()
    {
		_species = GetParentOrNull<Species>();
		if(_species == null)
			GD.PrintErr("Boid.cs: _speciesManager is null!");

		_skin = GetNodeOrNull<Polygon2D>("Skin");
		_skin.Color = BoidColor;

        AddToGroup("obstacles");
		RandomSpawn();
		SetDirection(CurrentDirection);
		AddJitterTimer();
    }

	private void AddJitterTimer()
	{
		JitterTimer = new Timer();
        JitterTimer.WaitTime = JitterFrequency;
        JitterTimer.OneShot = true;
        JitterTimer.Timeout += () => CanJitter = true;
        AddChild(JitterTimer);
		JitterTimer.Start();
	}

	public void ResetJitterTimer()
	{
		CanJitter = false;
        JitterTimer.WaitTime = JitterFrequency;
		JitterTimer.Start();
	}

	private void RandomSpawn()
	{
		RandomNumberGenerator rnd = new();
		rnd.Randomize();

		Speed = rnd.RandfRange(MinSpeed, MaxSpeed);
		float x = rnd.RandfRange(-1,1);
		float y = rnd.RandfRange(-1,1);
		CurrentDirection = new Vector2(x,y);
	}

	public void SetDirection(Vector2 direction)
	{
		CurrentDirection = direction.Normalized();
		if (CurrentDirection != Vector2.Zero)
		{
			Rotation = CurrentDirection.Angle();
		}
	}
}
