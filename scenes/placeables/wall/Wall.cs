using Godot;
using System;

public partial class Wall : Placeable
{
	public override void _Ready()
	{
		base._Ready();
		AddToGroup("walls");
	}

}
