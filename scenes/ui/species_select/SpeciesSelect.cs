using Godot;
using System;

public partial class SpeciesSelect : OptionButton
{
	private SpeciesManager _speciesManagerNode;
	private Settings _settingsNode; 

	[Signal] public delegate void SpeciesChangedEventHandler(Species species);

	public override void _Ready()
	{
		_settingsNode = GetParentOrNull<Settings>(); 

		if(_settingsNode == null)
		{
			GD.PrintErr("SpeciesSelect.cs: _settingsNode is null!");
		}
		else
		{
			_speciesManagerNode = _settingsNode.SpeciesManagerNode;
		}

		if(_speciesManagerNode == null)
			GD.PrintErr("SpeciesSelect.cs: _speciesManagerNode is null!");

		CallDeferred("PopulateSpeciesSelect");
	}


	private void PopulateSpeciesSelect()
	{
		if(_speciesManagerNode == null)  
			return;

		Clear();

		int index = 0;
		foreach(var kvp in _speciesManagerNode.SpeciesNodeDict)
		{
			AddItem(kvp.Value.SpeciesName);
			SetItemMetadata(index, kvp.Key);
			index++;
		}

		ItemSelected += OnSpeciesSelected;

		if (ItemCount > 0)
		{
			Select(0);
			OnSpeciesSelected(0);
		}
	}

	private void OnSpeciesSelected(long index)
	{
		if(_speciesManagerNode == null)  
			return;

		var metadata = GetItemMetadata((int)index);
		string speciesName = metadata.AsString();

		if (!_speciesManagerNode.SpeciesNodeDict.TryGetValue(speciesName, out Species species))
		{
			GD.PrintErr("Settings.cs: SpeciesSelect is null!");
			return;
		}

		_speciesManagerNode.SetActiveSpecies(speciesName);
		EmitSignal(SignalName.SpeciesChanged,species);
	}

}
