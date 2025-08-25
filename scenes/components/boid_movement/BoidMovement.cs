using Godot;
using System;


public partial class BoidMovement : Node2D
{
	[Export] BoidDetection BoidDetectionNode;

	public Boid BoidNode {get; private set;}

	private Vector2 _screenSize;
	private Timer _wallFlipTimer;
	private Timer _jitterTimer;
    private bool _canFlip = true;
    private bool _canJitter = true;

    public override void _Ready()
    {
		_screenSize = GetViewport().GetVisibleRect().Size;

		BoidNode = GetParentOrNull<Boid>();

        if (BoidNode == null)
            GD.PrintErr("BoidMovement.cs: _boid is null!");

		AddTimers();
    }

	private void AddTimers()
	{
		_wallFlipTimer = new Timer();
        _wallFlipTimer.WaitTime = 1.0f;
        _wallFlipTimer.OneShot = true;
        _wallFlipTimer.Timeout += () => _canFlip = true;
        AddChild(_wallFlipTimer);

	}

	public void ResetJitterTimer()
	{
		// _jitterTimer.WaitTime = BoidNode.JitterTime
	}

    public override void _Process(double delta)
    {
        base._Process(delta);

		if (BoidNode == null)
			return;

		float fDelta = (float)delta;

		UpdateDirectionAndSpeed(fDelta);

		BoidNode.MoveAndSlide();

		WallBounce();
		ScreenWrap();
    }

	private void UpdateDirectionAndSpeed(float fDelta)
	{
		TargetAndSpeed targetAndSpeed = ForceCalculator.CalculateTargetDirectionAndSpeed(BoidNode, BoidDetectionNode.ObstacleHashSet, BoidDetectionNode.VisionHashSet);
		
		Vector2 targetDirection = targetAndSpeed.TargetDirection;
		float speedAdjustment = targetAndSpeed.SpeedAdjustment;

		if(targetDirection != Vector2.Zero)
		{
			float maxTurnRate = BoidNode.TurnRate *  fDelta;
			Vector2 newDirection = BoidNode.CurrentDirection.Slerp(targetDirection, maxTurnRate).Normalized();
			BoidNode.SetDirection(newDirection);
		}

		speedAdjustment *= fDelta;
		BoidNode.Speed += speedAdjustment;
		BoidNode.Speed = Mathf.Clamp(BoidNode.Speed, BoidNode.MinSpeed, BoidNode.MaxSpeed);

		BoidNode.Velocity = BoidNode.CurrentDirection * BoidNode.Speed;
	}

	private void ScreenWrap()
	{
		Vector2 boidPos = BoidNode.GlobalPosition;

		if(boidPos.X < 0)
			boidPos.X = _screenSize.X;
		else if(boidPos.X > _screenSize.X)
			boidPos.X = 0;

		if(boidPos.Y < 0)
			boidPos.Y = _screenSize.Y;
		else if (boidPos.Y > _screenSize.Y)
			boidPos.Y = 0;

		BoidNode.GlobalPosition = boidPos;
	}

	private void WallBounce()
	{
		if(_canFlip && BoidNode.GetSlideCollisionCount() > 0)
		{
			for (int i = 0; i < BoidNode.GetSlideCollisionCount(); i++)
			{
				var collision = BoidNode.GetSlideCollision(i);
				var collider = collision.GetCollider() as Node;

				if (collider.IsInGroup("walls"))
				{
					// Flip the direction based on wall normal
					Vector2 wallNormal = collision.GetNormal();
					Vector2 newDirection = BoidNode.CurrentDirection.Bounce(wallNormal);
					BoidNode.SetDirection(newDirection);
					_canFlip = false;
					_wallFlipTimer.Start(); // Start 1-second timer
					break; // Handle first wall collision only
				}
			}
		}
	}
}
