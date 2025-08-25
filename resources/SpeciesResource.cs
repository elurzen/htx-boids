using Godot;
using System;

[GlobalClass]
public partial class SpeciesResource : Resource
{
	[Export] public string SpeciesName {get; set;} = "unknown";
	[Export] public float MinSpeed { get; set; } = 100;
	[Export] public float MaxSpeed { get; set; } = 300;
	[Export] public float TurnRate { get; set; } = 5.0f;
	[Export] public float CollisionRadius {get; set;} = 100.0f;
	[Export] public float VisionRadius {get; set;} = 200.0f;
	[Export] public float SeparationWeight {get; set;} = 1.0f;
	[Export] public float AlignmentWeight {get; set;} = 1.0f;
	[Export] public float CohesionWeight {get; set;} = 1.0f;
	[Export] public float SpeedWeight {get; set;} = 1.0f;
	[Export] public float WallWeight {get; set;} = 100.0f;
	[Export] public float LightWeight {get; set;} = 2.0f;
	[Export] public float JitterFrequency {get; set;} = 2.0f;
	[Export] public float JitterWeight {get; set;} = 0.3f;
	[Export] public Color BoidColor {get; set;} = Colors.White;
	[Export] public PackedScene BoidScene {get; set;}

	public SpeciesResource() {}

	public SpeciesResource(string name)
	{
		SpeciesName = name;
	}

}
