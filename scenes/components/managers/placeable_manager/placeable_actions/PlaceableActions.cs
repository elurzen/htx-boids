using Godot;
using System;
using System.Collections.Generic;

public partial class PlaceableActions : Node2D
{
	[Export] public PackedScene WallScene {get; set;}
	[Export] public PackedScene LightScene {get; set;}

	private Dictionary<Vector2, Placeable> _tileDict = new();

	public override void _Ready()
	{
        if (WallScene == null)
            GD.PrintErr("PlaceableActions.cs: WallScene is null!");

        if (LightScene == null)
            GD.PrintErr("PlaceableActions.cs: LightScene is null!");
	}

	public void SpawnWall(Vector2 tile)
	{
		if (WallScene == null || _tileDict.ContainsKey(tile))
			return;

		Wall newWall = WallScene.Instantiate<Wall>();
		newWall.GlobalPosition = tile;

		if(!_tileDict.TryAdd(tile, newWall))
			GD.PrintErr($"PlaceableManager.cs: couldn't add Wall {tile.ToString()} to dict");

		AddChild(newWall);
	}

	public void SpawnLight(Vector2 tile)
	{
		if (LightScene == null || _tileDict.ContainsKey(tile))
			return;

		Light newLight = LightScene.Instantiate<Light>();
		newLight.GlobalPosition = tile;

		if(!_tileDict.TryAdd(tile, newLight))
			GD.PrintErr($"PlaceableManager.cs: couldn't add Light {tile.ToString()} to dict");

		AddChild(newLight);
		
	}

	public void ToggleLight(Vector2 tile)
	{
		if (_tileDict.TryGetValue(tile, out Placeable placeable))
		{
			if(placeable is Light light)
				light.ToggleLight();
		}
	}

	public void ToggleAllLights()
	{
		foreach(var kvp in _tileDict)
		{
			if(kvp.Value is Light light)	
			{
				light.ToggleLight();
			}
		}
	}

	public void DeletePlaceable(Vector2 tile)
	{
		if (_tileDict.TryGetValue(tile, out Placeable placeable))
		{
			_tileDict.Remove(tile);
			placeable.QueueFree();
		}
	}

	public void DeleteAllPlaceables()
	{
		foreach(var placeable in _tileDict.Values)
			placeable.QueueFree();

		_tileDict.Clear();
	}

	public void DeleteAllWalls()
	{
		List<Vector2> tilesToFree = new();

		foreach(var kvp in _tileDict)
		{
			if(kvp.Value is Wall wall)	
			{
				tilesToFree.Add(kvp.Key);
				wall.QueueFree();
			}
		}

		foreach(var tile in tilesToFree)
		{
			if(!_tileDict.Remove(tile))
				GD.PrintErr($"PlaceableActions.cs: Couldn't delete Wall tile {tile.ToString()}");
		}
	}

	public void DeleteAllLights()
	{
		List<Vector2> tilesToFree = new();

		foreach(var kvp in _tileDict)
		{
			if(kvp.Value is Light light)	
			{
				tilesToFree.Add(kvp.Key);
				light.QueueFree();
			}
		}

		foreach(var tile in tilesToFree)
		{
			if(!_tileDict.Remove(tile))
				GD.PrintErr($"PlaceableActions.cs: Couldn't delete Light tile {tile.ToString()}");
		}
	}
}
