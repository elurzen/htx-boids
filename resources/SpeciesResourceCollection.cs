using Godot;
using System;
using System.Collections.Generic;

public partial class SpeciesResourceCollection : Resource
{
	private Dictionary<string, SpeciesResource> _speciesDict = new();
	public IReadOnlyDictionary<string, SpeciesResource> SpeciesDict => _speciesDict;

	public SpeciesResourceCollection(){}
	
	public SpeciesResourceCollection(string dirPath)
	{
		PopulateDict(dirPath);
	}

	public void PopulateDict(string dirPath)
	{
		// string dirPath = "res://data/species";
		var dir = DirAccess.Open(dirPath);

		if (dir == null)
		{
			GD.PrintErr($"SpeciesResourceCollection.cs: unable to open {dirPath}");
			return;
		}

		foreach(string fileName in dir.GetFiles())
		{
			var resource = ResourceLoader.Load<SpeciesResource>($"{dirPath}/{fileName}");
			if (resource != null)
			{
				string key = fileName.GetBaseName().ToLower();
				_speciesDict.TryAdd(key, resource);
			}
		}
	}

	public SpeciesResource GetSpeciesResource(string speciesName)
	{
		if(!_speciesDict.TryGetValue(speciesName, out SpeciesResource returnSpeciesResource))
		{
			GD.PrintErr($"SpeciesResourceCollection.cs: No Species found with name {speciesName}");
		}

		return returnSpeciesResource;
	}
}
