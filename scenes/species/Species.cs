using Godot;
using System;

public partial class Species : Node
{
	private SpeciesResource _speciesData;

	public string SpeciesName => _speciesData.SpeciesName;
	public float MinSpeed => _speciesData.MinSpeed;
	public float MaxSpeed => _speciesData.MaxSpeed;
	public float TurnRate => _speciesData.TurnRate;
	public float CollisionRadius => _speciesData.CollisionRadius;
	public float VisionRadius => _speciesData.VisionRadius;
	public float SeparationWeight => _speciesData.SeparationWeight;
	public float AlignmentWeight => _speciesData.AlignmentWeight;
	public float CohesionWeight => _speciesData.CohesionWeight;
	public float SpeedWeight => _speciesData.SpeedWeight;
	public float WallWeight => _speciesData.WallWeight;
	public float LightWeight => _speciesData.LightWeight;
	public float JitterFrequency => _speciesData.JitterFrequency;
	public float JitterWeight => _speciesData.JitterWeight;
	public Color BoidColor => _speciesData.BoidColor;
	public PackedScene BoidScene => _speciesData.BoidScene;

	[Signal] public delegate void RadaiiUpdatedEventHandler();

    public override void _Ready()
    {
		if (_speciesData == null)
			GD.PrintErr("Species.cs: SpeciesData is null!");

		if (_speciesData.BoidScene == null)
			GD.PrintErr("Species.cs: BoidScene is null!");

		// AddToGroup("species");
		AddToGroup(_speciesData.SpeciesName);
    }

	public void SetSpeciesData(SpeciesResource speciesData)
	{
		_speciesData = speciesData;
	}

	public void SpawnBoid(Vector2 position)
	{
		Boid boid = BoidScene.Instantiate<Boid>();
		boid.GlobalPosition = position;
		AddChild(boid);
	}

	public void KillAllBoids()
	{
		var children = GetChildren();

		foreach(var child in children)
		{
			if(child is Boid boid)
				boid.QueueFree();
		}
	}

	public void UpdateSetting(string parameterName, float value)
	{
		bool updatedRadius = false;

		if (parameterName == "Min Speed")
			_speciesData.MinSpeed = Mathf.Clamp(value, 0.0f, MaxSpeed);
		else if (parameterName == "Max Speed")
			_speciesData.MaxSpeed = Mathf.Clamp(value, MinSpeed, 500.0f);
		else if (parameterName == "Turn Rate")
			_speciesData.TurnRate = value;
		else if (parameterName == "Collision Radius")//doesnt work rn
		{
			_speciesData.CollisionRadius = value;
			updatedRadius = true;
		}
		else if (parameterName == "Vision Radius")//doesnt work rn
		{
			_speciesData.VisionRadius = value;
			updatedRadius = true;
		}
		else if (parameterName == "Separation Weight")
			_speciesData.SeparationWeight = value;
		else if (parameterName == "Alignment Weight")
			_speciesData.AlignmentWeight = value;
		else if (parameterName == "Cohesion Weight")
			_speciesData.CohesionWeight = value;
		else if (parameterName == "Speed Weight")
			_speciesData.SpeedWeight = value;
		else if (parameterName == "Wall Weight")
			_speciesData.WallWeight = value;
		else if (parameterName == "Light Weight")
			_speciesData.LightWeight = value;
		else if (parameterName == "Jitter Frequency")
			_speciesData.JitterFrequency = value;
		else if (parameterName == "Jitter Weight")
			_speciesData.JitterWeight = value;

		if (updatedRadius)
			EmitSignal(SignalName.RadaiiUpdated);
	}
}
