using Godot;
using System;
using System.Collections.Generic;

public record AttractionForces(
		Vector2 AlignmentForce, 
		Vector2 CohesionForce, 
		Vector2 LightForce, 
		float SpeedAdjustment);

public record RepulsionForces(Vector2 SeparationForce, Vector2  WallForce);
public record TargetAndSpeed(Vector2 TargetDirection, float SpeedAdjustment);

public static class ForceCalculator
{
	private static readonly RandomNumberGenerator rng = new RandomNumberGenerator();

	static ForceCalculator()
	{
		rng.Randomize();
	}

	public static TargetAndSpeed CalculateTargetDirectionAndSpeed(Boid _boid, IReadOnlyCollection<Node2D> obstacles, IReadOnlyCollection<Node2D> alliesInVision)
	{
		RepulsionForces repulsionForces = ForceCalculator.CalculateRepulsionForces(_boid, obstacles);
		AttractionForces attractionForces = ForceCalculator.CalculateAttractionForces(_boid, alliesInVision);
		Vector2 jitterForce = ForceCalculator.CalculateJitterForce(_boid);

		Vector2 totalForce = (repulsionForces.SeparationForce * _boid.SeparationWeight) +
							(attractionForces.AlignmentForce * _boid.AlignmentWeight) +
							(attractionForces.CohesionForce * _boid.CohesionWeight) + 
							(repulsionForces.WallForce * _boid.WallWeight) +
							(attractionForces.LightForce * _boid.LightWeight) +
							(jitterForce * _boid.JitterWeight);

		float speed = attractionForces.SpeedAdjustment * _boid.SpeedWeight;
		return new TargetAndSpeed(totalForce.Normalized(), speed);
	}

	private static RepulsionForces CalculateRepulsionForces(Boid boid, IReadOnlyCollection<Node2D> obstacles)
	{
		if (obstacles.Count == 0)
			return new RepulsionForces(Vector2.Zero, Vector2.Zero);
		
		//wall
		Vector2 wallForce = Vector2.Zero;

		//separation
		Vector2 separationForce = Vector2.Zero;

		foreach(Node2D obstacle in obstacles)
		{
			//Opposite direction of obstacle
			Vector2 positionDiff = boid.GlobalPosition - obstacle.GlobalPosition;

			//How far obstacle is away
			float distance = positionDiff.Length();
			if (distance == 0)
				continue;

			// positionDiff = positionDiff.Normalized();

			//Calculated force - closer objects apply more force
			Vector2 adjustedForce = positionDiff.Normalized() / distance;

			//Wall
			if(obstacle.IsInGroup("walls"))
			{
				wallForce += adjustedForce;
				continue;
			}

			//separation
			separationForce += adjustedForce;
		}

		wallForce = wallForce.Normalized();
		separationForce = separationForce.Normalized();

		return new RepulsionForces(separationForce, wallForce);
	}

	private static AttractionForces CalculateAttractionForces (Boid boid, IReadOnlyCollection<Node2D> attractorsInVision)
	{
		//Little trickier to read, but saves 2 Loops through alliesInVision
		if (attractorsInVision.Count == 0)
			return new AttractionForces(Vector2.Zero, Vector2.Zero, Vector2.Zero, 0.0f);

		//Light
		Vector2 lightForce = Vector2.Zero;
		Vector2 lightPositions = Vector2.Zero;
		int lightCount = 0;

		//Alignment
		Vector2 averageDirection = Vector2.Zero;
		Vector2 alignmentForce = Vector2.Zero;

		//Cohesion
		Vector2 centerOfSwarm = Vector2.Zero;
		Vector2 cohesionForce = Vector2.Zero;
		int allyCount = 0;

		//Speed
		float averageSpeed = 0.0f;
		
		foreach(Node2D attractor in attractorsInVision)
		{
			//light
			if(attractor is Light light)
			{
				if(!light.isOn)
					continue;

				lightPositions += attractor.GlobalPosition;
				lightCount++;
				continue;
			}

			Boid ally = attractor as Boid;

			if(ally == null)
				continue;

			allyCount++;

			//Alignment
			averageDirection += ally.CurrentDirection;

			//Cohesion
			centerOfSwarm += ally.GlobalPosition;	

			//Speed
			averageSpeed += ally.Speed;
		}

		//Light
		if (lightCount > 0)
		{
			lightPositions /= lightCount;
			lightForce = lightPositions - boid.GlobalPosition;
			lightForce = lightForce.Normalized();
		}

		if (allyCount == 0)
			return new AttractionForces(Vector2.Zero, Vector2.Zero, lightForce, 0.0f);
		

		//Alignment
		averageDirection /= allyCount;
		alignmentForce = averageDirection.Normalized();

		//Cohesion
		centerOfSwarm /= allyCount; 
		cohesionForce = centerOfSwarm - boid.GlobalPosition;
		cohesionForce = cohesionForce.Normalized();

		//Speed
		averageSpeed /= attractorsInVision.Count;
		float speedAdjustment = averageSpeed - boid.Speed;

		return new AttractionForces(alignmentForce, cohesionForce, lightForce, speedAdjustment);
	}

	private static Vector2 CalculateJitterForce(Boid _boid)
	{
		Vector2 jitterForce = Vector2.Zero;
		
		if(_boid.CanJitter)
		{
			float x = rng.RandfRange(-1,1);
			float y = rng.RandfRange(-1,1);
			_boid.CurrentJitter = new Vector2(x,y).Normalized();
			_boid.ResetJitterTimer();
		}

		if(_boid.JitterTimer.WaitTime > 0)
		{
			float timeDegredation = (float)(_boid.JitterTimer.TimeLeft / _boid.JitterTimer.WaitTime);
			jitterForce = _boid.CurrentJitter * timeDegredation;
		}

		return jitterForce;
	}
}
