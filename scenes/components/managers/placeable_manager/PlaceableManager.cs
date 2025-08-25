using Godot;
using System;

public partial class PlaceableManager : Node2D
{
	private PlaceableActions _placeableActionsNode;

	private readonly Vector2 TILE_SIZE = new Vector2(32.0f, 32.0f);

	private delegate void PaintbrushFunctionDelegate(Vector2 tile);
	private PaintbrushFunctionDelegate _paintbrush;
	private Vector2 tile => GetGlobalMousePosition().Snapped(TILE_SIZE);


    public override void _Ready()
    {
		_placeableActionsNode = GetNodeOrNull<PlaceableActions>("PlaceableActions");
		if(_placeableActionsNode == null)
			GD.PrintErr("PlaceableManager.cs: PlaceableActionsNode is null!");

		SetProcessUnhandledInput(true);
    }

	public override void _Process(double delta)
	{
		if(_paintbrush == null)
			return;
		
		_paintbrush(tile);
	}

    public override void _UnhandledInput(Godot.InputEvent @event)
    {
		if(_placeableActionsNode == null)
			return;

		if(@event.IsActionPressed("delete_all_placeables"))
		{
			_placeableActionsNode.DeleteAllPlaceables();
			GetViewport().SetInputAsHandled();
			return;
		}

		if(@event.IsActionPressed("delete_all_walls"))
		{
			_placeableActionsNode.DeleteAllWalls();
			GetViewport().SetInputAsHandled();
			return;
		}

		if(@event.IsActionPressed("delete_all_lights"))
		{
			_placeableActionsNode.DeleteAllLights();
			GetViewport().SetInputAsHandled();
			return;
		}

		if(@event.IsActionPressed("spawn_wall"))
		{
			_paintbrush = _placeableActionsNode.SpawnWall;
			GetViewport().SetInputAsHandled();
			return;
		}
		
		if(@event.IsActionPressed("spawn_light"))
		{
			_paintbrush = _placeableActionsNode.SpawnLight;
			GetViewport().SetInputAsHandled();
			return;
		}

		if(@event.IsActionPressed("toggle_all_lights"))
		{
			_placeableActionsNode.ToggleAllLights();
			GetViewport().SetInputAsHandled();
			return;
		}

		if(@event.IsActionPressed("toggle_light"))
		{
			_placeableActionsNode.ToggleLight(tile);
			GetViewport().SetInputAsHandled();
			return;
		}

		if(@event.IsActionPressed("delete_placeable"))
		{
			_paintbrush = _placeableActionsNode.DeletePlaceable;
			GetViewport().SetInputAsHandled();
			return;
		}

		if(@event.IsActionReleased("spawn_wall") && _paintbrush == _placeableActionsNode.SpawnWall)
		{
			_paintbrush = null;
			GetViewport().SetInputAsHandled();
			return;
		}

		if(@event.IsActionReleased("spawn_light") && _paintbrush == _placeableActionsNode.SpawnLight)
		{
			_paintbrush = null;
			GetViewport().SetInputAsHandled();
			return;
		}

		if(@event.IsActionReleased("delete_placeable") && _paintbrush == _placeableActionsNode.DeletePlaceable)
		{
			_paintbrush = null;
			GetViewport().SetInputAsHandled();
			return;
		}
    }
}
