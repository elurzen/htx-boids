using Godot;
using System;
using System.Collections.Generic;

public partial class SpeciesActions : Node2D
{

	public override void _Ready()
	{
	}

	public Species AddSpeciesNode(SpeciesResource speciesResource)
	{
			Species speciesNode = new Species();
			speciesNode.SetSpeciesData(speciesResource);
			AddChild(speciesNode);

			return speciesNode;
	}

	public void SpawnBoid(Species speciesNode, int count)
	{
		var position = GetGlobalMousePosition();

		for(int i = 0; i < count; i++)
			speciesNode.SpawnBoid(position);
	}

	public void KillAllBoids(Dictionary<string, Species> spiecesNodeDict)
	{
		foreach(Species species in spiecesNodeDict.Values)
			KillBoidSpecies(species);
	}

	public void KillBoidSpecies(Species speciesNode)
	{
		speciesNode.KillAllBoids();
	}

}
