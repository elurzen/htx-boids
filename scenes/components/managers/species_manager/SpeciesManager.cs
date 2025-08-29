using Godot;
using System;
using System.Collections.Generic;

public partial class SpeciesManager : Node
{
	[Export] public string DefaultSpeciesDir = "res://data/species";

	public Dictionary<string, Species> SpeciesNodeDict {get; private set;}= new();

	private SpeciesActions _speciesActionsNode;
	private SpeciesResourceCollection _defaultSpeciesCollection;
	private string _activeSpecies;

    public override void _Ready()
    {
		_speciesActionsNode = GetNodeOrNull<SpeciesActions>("SpeciesActions");
		if(_speciesActionsNode == null)
			GD.PrintErr("SpeciesManager.cs: SpeciesActionsNode is null!");

		_defaultSpeciesCollection = new SpeciesResourceCollection(DefaultSpeciesDir);
		CreateSpeciesNodes();

		SetProcessUnhandledInput(true);
    }

	private void CreateSpeciesNodes()
	{
		foreach(SpeciesResource speciesResource in _defaultSpeciesCollection.SpeciesDict.Values)
		{
			Species speciesNode = _speciesActionsNode.AddSpeciesNode(speciesResource);
			SpeciesNodeDict.TryAdd(speciesResource.SpeciesName, speciesNode);
		}
	}

	public void SetActiveSpecies(string speciesName)
	{
		_activeSpecies = speciesName;
	}

    public override void _UnhandledInput(Godot.InputEvent @event)
    {
		if(_speciesActionsNode == null)
			return;

		if(@event.IsActionPressed("kill_all_boids"))
		{
			_speciesActionsNode.KillAllBoids(SpeciesNodeDict);
			GetViewport().SetInputAsHandled();
			return;
		}

		if(_activeSpecies == null)
			return;

		if(!SpeciesNodeDict.TryGetValue(_activeSpecies, out Species speciesNode))
		{
			GD.PrintErr($"SpeciesManager.cs: No species found with name {_activeSpecies}");
			return;
		}

		if(@event.IsActionPressed("spawn_boid"))
		{
			_speciesActionsNode.SpawnBoid(speciesNode, 1);
			GetViewport().SetInputAsHandled();
			return;
		}

		if(@event.IsActionPressed("spawn_10_boids"))
		{
			_speciesActionsNode.SpawnBoid(speciesNode, 10);
			GetViewport().SetInputAsHandled();
			return;
		}

		if(@event.IsActionPressed("spawn_20_boids"))
		{
			_speciesActionsNode.SpawnBoid(speciesNode, 20);
			GetViewport().SetInputAsHandled();
			return;
		}

		if(@event.IsActionPressed("kill_boid_species"))
		{
			_speciesActionsNode.KillBoidSpecies(speciesNode);
			GetViewport().SetInputAsHandled();
			return;
		}
    }
}
