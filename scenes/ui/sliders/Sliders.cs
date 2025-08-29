using Godot;
using System;
using System.Collections.Generic;

public record SliderDefinition(string Name, float MinValue, float MaxValue, float InitialValue, float StepValue);

public partial class Sliders : VBoxContainer
{
	[Export] public PackedScene SliderGroupScene;

	private Settings _settingsNode;
	private SpeciesSelect _speciesSelectNode;

	private Dictionary<string, SliderGroup> _sliderDict = new();
	private bool _updatingSettings = false;
	private Species _speciesNode;

	//min, max, default, step size
	private readonly List<SliderDefinition> _slidersList = new (){
		new SliderDefinition("Min Speed",0.0f, 500.0f, 110.0f, 1.0f),
		new SliderDefinition("Max Speed",0.0f, 500.0f, 239.0f, 1.0f),
		new SliderDefinition("Turn Rate", 0.1f, 10.0f, 4.0f, 0.1f),
		new SliderDefinition("Collision Radius", 0.0f, 75.0f, 15.0f, 1.0f),
		new SliderDefinition("Vision Radius", 0.0f, 75.0f, 30.0f, 1.0f),
		new SliderDefinition("Separation Weight", 0.0f, 5.0f, 1.85f, 0.01f),
		new SliderDefinition("Alignment Weight", 0.0f, 5.0f, 0.02f, 0.01f),
		new SliderDefinition("Cohesion Weight", 0.0f, 5.0f, 1.9f, 0.01f),
		new SliderDefinition("Speed Weight", 0.0f, 5.0f, 1.7f, 0.01f),
		new SliderDefinition("Wall Weight", 0.0f, 100.0f, 5f, 0.01f),
		new SliderDefinition("Light Weight", 0.0f, 5.0f, 0.3f, 0.01f),
		new SliderDefinition("Jitter Frequency", 0.0f, 10.0f, 0.3f, 0.1f),
		new SliderDefinition("Jitter Weight", 0.0f, 5.0f, 0.3f, 0.01f),
	};

    public override void _Ready()
	{
		_settingsNode = GetParentOrNull<Settings>(); 

		if(_settingsNode == null)
			GD.PrintErr("Sliders.cs: _settingsNode is null!");
		else
			_speciesSelectNode = _settingsNode.SpeciesSelect;

		if(_speciesSelectNode == null)
			GD.PrintErr("SpeciesSelect.cs: _speciesManagerNode is null!");
		else
			_speciesSelectNode.SpeciesChanged += UpdateSpecies;

		if(SliderGroupScene == null)
			GD.PrintErr("Sliders.cs: SliderGroupScene is null!");

		CreateSliders();
	}

	public void CreateSliders()
	{
		foreach(SliderDefinition s in _slidersList)
		{
			SliderGroup newSlider = SliderGroupScene.Instantiate<SliderGroup>();
			newSlider.ValueChanged += OnSliderValueChanged;

			if(!_sliderDict.TryAdd(s.Name, newSlider))
					GD.PrintErr($"Sliders.cs: failed to add {s.Name} slider!");

			AddChild(newSlider);
			newSlider.Init(s);
		}
	}

	private void UpdateSpecies(Species species)
	{
		_speciesNode = species;
		UpdateSlidersFromSpecies();
	}

    private void UpdateSlidersFromSpecies()
    {
        if (_speciesNode == null) 
			return;

		_updatingSettings = true;

        SetSliderNoSignal("Min Speed", _speciesNode.MinSpeed);
        SetSliderNoSignal("Max Speed", _speciesNode.MaxSpeed);
        SetSliderNoSignal("Turn Rate", _speciesNode.TurnRate);
        SetSliderNoSignal("Collision Radius", _speciesNode.CollisionRadius);
        SetSliderNoSignal("Vision Radius", _speciesNode.VisionRadius);
        SetSliderNoSignal("Separation Weight", _speciesNode.SeparationWeight);
        SetSliderNoSignal("Alignment Weight", _speciesNode.AlignmentWeight);
        SetSliderNoSignal("Cohesion Weight", _speciesNode.CohesionWeight);
        SetSliderNoSignal("Speed Weight", _speciesNode.SpeedWeight);
        SetSliderNoSignal("Wall Weight", _speciesNode.WallWeight);
        SetSliderNoSignal("Light Weight", _speciesNode.LightWeight);
        SetSliderNoSignal("Jitter Frequency", _speciesNode.JitterFrequency);
        SetSliderNoSignal("Jitter Weight", _speciesNode.JitterWeight);

		_updatingSettings = false;
    }

    private void SetSliderNoSignal(string name, float value)
    {
        if (_sliderDict.TryGetValue(name, out SliderGroup sliderGroup))
            sliderGroup.SetValueNoSignal(value);
    }

	private void OnSliderValueChanged(string parameterName, float value)
	{
		if(_updatingSettings || _speciesNode == null)
			return;

		if(parameterName == "Min Speed" || parameterName == "Max Speed")
			ValidateSpeeds(parameterName, value);

		_speciesNode.UpdateSetting(parameterName, value);
	}

	private void ValidateSpeeds(string parameterName, float value)
	{
		if(_updatingSettings)
			return;

		if(parameterName == "Min Speed")
		{
			if(!_sliderDict.TryGetValue("Max Speed", out SliderGroup maxSpeed))
				return;

			if(maxSpeed.Value < value)
				maxSpeed.SetValue(value);

			return;
		}

		if(parameterName == "Max Speed")
		{
			if(!_sliderDict.TryGetValue("Min Speed", out SliderGroup minSpeed))
				return;

			if(minSpeed.Value > value)
				minSpeed.SetValue(value);

			return;
		}

	}
}  
