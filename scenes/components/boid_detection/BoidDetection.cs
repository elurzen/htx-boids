using Godot;
using System;
using System.Collections.Generic;

public partial class BoidDetection : Node2D
{
	[Export] Area2D CollisionArea;
	[Export] Area2D VisionArea;
	
	private Boid _boid;
	private CircleShape2D _circleCollisionShape;
	private CircleShape2D _circleVisionShape;

	public HashSet<Node2D> ObstacleHashSet {get; private set;} = new();
	public HashSet<Node2D> VisionHashSet {get; private set;} = new();


	public override void _Ready()
	{

		CallDeferred("InitDetection");
		CallDeferred("InitSignals");
	}

	public void OnRadaiiUpdated()
	{
		if(_circleCollisionShape != null)
			_circleCollisionShape.Radius = _boid.CollisionRadius;
		if(_circleVisionShape != null)
			_circleVisionShape.Radius = _boid.VisionRadius;
	}

    private void InitDetection()
    {
		BoidMovement boidMovementNode = GetParentOrNull<BoidMovement>();

		if(boidMovementNode != null)
			_boid = boidMovementNode.BoidNode;

		if(_boid == null)
            GD.PrintErr("BoidDetection.cs: _boid is null!");

        if (CollisionArea == null)
        {
            GD.PrintErr("BoidDetection.cs: _collisionArea is null!");
        }
		else
		{
			_circleCollisionShape = CollisionArea.GetNode<CollisionShape2D>("CollisionShape2D").Shape as CircleShape2D;
		}

        if (VisionArea == null)
        {
            GD.PrintErr("BoidDetection.cs: _visionArea is null!");
        }
		else
		{
			_circleVisionShape = VisionArea.GetNode<CollisionShape2D>("CollisionShape2D").Shape as CircleShape2D;
		}

        if (_circleCollisionShape == null)
        {
            GD.PrintErr("BoidDetection.cs: _circleCollisionShape is null!");
        }

        if (_circleVisionShape == null)
        {
            GD.PrintErr("BoidDetection.cs: _circleVisionShape is null!");
        }

		// DebugCollisionSetup();
    }

	private void InitSignals()
	{
		if (CollisionArea != null)
		{
			CollisionArea.BodyEntered += OnCollisionAreaEntered;
			CollisionArea.BodyExited += OnCollisionAreaExited;
		}
		
		if (VisionArea != null)
		{
			VisionArea.BodyEntered += OnVisionAreaEntered;
			VisionArea.BodyExited += OnVisionAreaExited;
		}

		if (_circleVisionShape != null)
			_circleVisionShape.Radius = _boid.VisionRadius;

		if (_circleCollisionShape != null)
			_circleCollisionShape.Radius = _boid.CollisionRadius;

		Species species = _boid.GetParentOrNull<Species>();
		if (species != null)
			species.RadaiiUpdated += OnRadaiiUpdated;
	}

	private void OnCollisionAreaEntered(Node2D body)
	{
		if(body.IsInGroup("obstacles") && body != _boid)
			ObstacleHashSet.Add(body);
	}

	private void OnCollisionAreaExited(Node2D body)
	{
		ObstacleHashSet.Remove(body);
	}

	private void OnVisionAreaEntered(Node2D body)
	{
		if(body.IsInGroup("lights"))
		{
			VisionHashSet.Add(body);
			return;
		}

		var boidBody = body as Boid;

		if (boidBody != null && 
			boidBody != _boid && 
			boidBody.Species == _boid.Species
		)
		{
			VisionHashSet.Add(body);
		}
	}

	private void OnVisionAreaExited(Node2D body)
	{
		VisionHashSet.Remove(body);
	}




	// private void DebugCollisionSetup()
	// {
	// 	if (CollisionArea != null)
	// 	{
	// 		GD.Print($"CollisionArea - Layer: {CollisionArea.CollisionLayer}, Mask: {CollisionArea.CollisionMask}");
	// 	}
	//
	// 	if (VisionArea != null)
	// 	{
	// 		GD.Print($"VisionArea - Layer: {VisionArea.CollisionLayer}, Mask: {VisionArea.CollisionMask}");
	// 	}
	//
	// 	// Check what's already in the scene that should be detected
	// 	var allBodies = GetTree().GetNodesInGroup("obstacles");
	// 	GD.Print($"Found {allBodies.Count} obstacles in scene:");
	// 	foreach (Node body in allBodies)
	// 	{
	// 		if (body is StaticBody2D staticBody)
	// 		{
	// 			GD.Print($"  - {body.Name}: Layer {staticBody.CollisionLayer}, Mask {staticBody.CollisionMask}, Groups: [{string.Join(", ", body.GetGroups())}]");
	// 		}
	// 		else if (body is CharacterBody2D charBody)
	// 		{
	// 			GD.Print($"  - {body.Name}: Layer {charBody.CollisionLayer}, Mask {charBody.CollisionMask}, Groups: [{string.Join(", ", body.GetGroups())}]");
	// 		}
	// 	}
	//
	// 	var allLights = GetTree().GetNodesInGroup("lights");
	// 	GD.Print($"Found {allLights.Count} lights in scene:");
	// 	foreach (Node light in allLights)
	// 	{
	// 		if (light is StaticBody2D staticBody)
	// 		{
	// 			GD.Print($"  - {light.Name}: Layer {staticBody.CollisionLayer}, Mask {staticBody.CollisionMask}, Groups: [{string.Join(", ", light.GetGroups())}]");
	// 		}
	// 	}
	// }
}
