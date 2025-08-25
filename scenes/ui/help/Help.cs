using Godot;
using System;
using System.Collections.Generic;

public partial class Help : Panel
{
	[Export] VBoxContainer VBox = new();
	
	public override void _Ready()
	{
		PopulateKeyBinds();
		SetProcessUnhandledInput(true);
		AddCloseButton();
	}
	
    public override void _UnhandledInput(Godot.InputEvent @event)
    {
		if(@event.IsActionPressed("toggle_help"))
		{
			Visible = !Visible;
			SetProcessUnhandledInput(true);
		}
	}

	private void PopulateKeyBinds()
	{
		var actions = InputMap.GetActions();

		foreach(StringName action in actions)
		{
			if(action.ToString().StartsWith("ui_"))
				continue;

			AddKeybind(action);
		}
	}

	private void AddKeybind(string action)
	{
		HBoxContainer hBox = new HBoxContainer();
		Label actionLabel = new Label();
		Label eventsLabel = new Label();
		
		actionLabel.Text = action.ToString();
		hBox.AddChild(actionLabel);
		hBox.AddSpacer(false);
		hBox.SetAnchorsPreset(LayoutPreset.Center, false);
		hBox.CustomMinimumSize = new Vector2(500, 0);

		var inputEvents = InputMap.ActionGetEvents(action);
		List<string> keybindList = new();
		
		foreach(var inputEvent in inputEvents)
		{
			var keybind = inputEvent.AsText().Replace("(Physical)","");
			keybindList.Add(keybind);
		}

		eventsLabel.Text = string.Join(",", keybindList);
		hBox.AddChild(eventsLabel);

		VBox.AddChild(hBox);
	}

	private void AddCloseButton()
	{
		Button button = new();
		button.Text = "Close";
		button.CustomMinimumSize = new Vector2(0, 50);
		button.Pressed += () => Visible = false;
		VBox.AddChild(button);
	}
}
