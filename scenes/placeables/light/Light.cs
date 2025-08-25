using Godot;
using System;

public partial class Light : Placeable
{
	[Export] public bool isOn {get; private set;} = true;
	[Export] public Sprite2D Sprite;
	[Export] public Texture2D OnTexture;
	[Export] public Texture2D OffTexture;

	public override void _Ready()
	{
		base._Ready();
		AddToGroup("lights");

		if(Sprite == null)
			GD.PrintErr("Light.cs: Sprite is null!");
		
		if(OnTexture == null)
			GD.PrintErr("Light.cs: OnTexture is null!");

		if(OffTexture == null)
			GD.PrintErr("Light.cs: OffTexture is null!");

	}

	public void ToggleLight()
	{
			isOn = !isOn;

			if(isOn)
				Sprite.Texture = OnTexture;
			else
				Sprite.Texture = OffTexture;
	}
}
