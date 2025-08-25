using Godot;
using System;

public partial class SliderGroup : Control
{
	[Export] public string ParameterName {get; set;} = "PARAMETER NAME";
	[Export] public float MinValue {get; set;} = 0.0f;
	[Export] public float MaxValue {get; set;} = 10.0f;
	[Export] public float DefaultValue {get; set;} = 1.0f;
	[Export] public float StepValue {get; set;} = 0.1f;

	[Signal] public delegate void ValueChangedEventHandler(string parameterName, float value);

	public float Value => (float)_slider.Value;
	public void SetValue (float value) => _slider.Value = value;

	private Label _label;
	private HSlider _slider;
	private bool _updatingSliders = false;

    public override void _Ready()
    {
		_label = GetNodeOrNull<Label>("Label");
		if(_label == null)
			GD.PrintErr("SliderGroup.cs: _label is null!");

		_slider = GetNodeOrNull<HSlider>("HSlider");
		if(_slider == null)
			GD.PrintErr("SliderGroup.cs: _label is null!");

		_slider.ValueChanged += OnSliderValueChanged;
		UpdateLabel();
    }

	public void Init(SliderDefinition sliderDefinition)
	{
		ParameterName = sliderDefinition.Name;
		MinValue = sliderDefinition.MinValue;
		MaxValue = sliderDefinition.MaxValue;
		DefaultValue = sliderDefinition.InitialValue;
		StepValue = sliderDefinition.StepValue;
		
		_slider.MinValue = MinValue;
		_slider.MaxValue = MaxValue;
		_slider.Step = StepValue;
		_slider.Value = DefaultValue;
		UpdateLabel();
	}

	private void UpdateLabel()
	{
		_label.Text = $"{ParameterName}: {_slider.Value:F2}";
	}

	public void SetValueNoSignal(float value)
	{
		_updatingSliders = true;
		_slider.Value = value;
		_updatingSliders = false;
	}

	private void OnSliderValueChanged(double value)
	{
		UpdateLabel();

		if (!_updatingSliders)
			EmitSignal(SignalName.ValueChanged, ParameterName, (float)value);
	}


}
