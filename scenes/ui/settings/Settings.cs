using Godot;
using System;

public partial class Settings : Control
{
	[Export] public SpeciesManager SpeciesManagerNode {get; private set;}
	[Export] public SpeciesSelect SpeciesSelect {get; private set;}

    public override void _Ready()
	{
		if(SpeciesManagerNode == null)
			GD.PrintErr("Settings.cs: SpeciesManagerNode is null!");

		if(SpeciesSelect == null)
			GD.PrintErr("Settings.cs: SpeciesSelect is null!");

		SetProcessUnhandledInput(true);
	}

    public override void _UnhandledInput(InputEvent @event)
    {
		if(@event.IsActionPressed("toggle_settings"))
		{
			Visible = !Visible;
			GetViewport().SetInputAsHandled();
		}
    }
} 
