using Godot;
using System;

public partial class Species : Node
{
	private SpeciesResource _speciesResourceNode;

	public string SpeciesName => _speciesResourceNode.SpeciesName;
	public float MinSpeed => _speciesResourceNode.MinSpeed;
	public float MaxSpeed => _speciesResourceNode.MaxSpeed;
	public float TurnRate => _speciesResourceNode.TurnRate;
	public float CollisionRadius => _speciesResourceNode.CollisionRadius;
	public float VisionRadius => _speciesResourceNode.VisionRadius;
	public float SeparationWeight => _speciesResourceNode.SeparationWeight;
	public float AlignmentWeight => _speciesResourceNode.AlignmentWeight;
	public float CohesionWeight => _speciesResourceNode.CohesionWeight;
	public float SpeedWeight => _speciesResourceNode.SpeedWeight;
	public float WallWeight => _speciesResourceNode.WallWeight;
	public float LightWeight => _speciesResourceNode.LightWeight;
	public float JitterFrequency => _speciesResourceNode.JitterFrequency;
	public float JitterWeight => _speciesResourceNode.JitterWeight;
	public Color BoidColor => _speciesResourceNode.BoidColor;
	public PackedScene BoidScene => _speciesResourceNode.BoidScene;

	[Signal] public delegate void RadaiiUpdatedEventHandler();

    public override void _Ready()
    {
		if (_speciesResourceNode == null)
			GD.PrintErr("Species.cs: SpeciesData is null!");

		if (_speciesResourceNode.BoidScene == null)
			GD.PrintErr("Species.cs: BoidScene is null!");

		AddToGroup(_speciesResourceNode.SpeciesName);
    }

	public void SetSpeciesData(SpeciesResource speciesResourceNode)
	{
		_speciesResourceNode = speciesResourceNode;
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
			_speciesResourceNode.MinSpeed = Mathf.Clamp(value, 0.0f, MaxSpeed);
		else if (parameterName == "Max Speed")
			_speciesResourceNode.MaxSpeed = Mathf.Clamp(value, MinSpeed, 500.0f);
		else if (parameterName == "Turn Rate")
			_speciesResourceNode.TurnRate = value;
		else if (parameterName == "Collision Radius")//doesnt work rn
		{
			_speciesResourceNode.CollisionRadius = value;
			updatedRadius = true;
		}
		else if (parameterName == "Vision Radius")//doesnt work rn
		{
			_speciesResourceNode.VisionRadius = value;
			updatedRadius = true;
		}
		else if (parameterName == "Separation Weight")
			_speciesResourceNode.SeparationWeight = value;
		else if (parameterName == "Alignment Weight")
			_speciesResourceNode.AlignmentWeight = value;
		else if (parameterName == "Cohesion Weight")
			_speciesResourceNode.CohesionWeight = value;
		else if (parameterName == "Speed Weight")
			_speciesResourceNode.SpeedWeight = value;
		else if (parameterName == "Wall Weight")
			_speciesResourceNode.WallWeight = value;
		else if (parameterName == "Light Weight")
			_speciesResourceNode.LightWeight = value;
		else if (parameterName == "Jitter Frequency")
			_speciesResourceNode.JitterFrequency = value;
		else if (parameterName == "Jitter Weight")
			_speciesResourceNode.JitterWeight = value;

		if (updatedRadius)
			EmitSignal(SignalName.RadaiiUpdated);
	}
}
