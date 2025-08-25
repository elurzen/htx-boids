using Godot;
using System;

public abstract partial class Placeable : StaticBody2D
{
	public override void _Ready()
	{
		AddToGroup("obstacles");
	}
}
