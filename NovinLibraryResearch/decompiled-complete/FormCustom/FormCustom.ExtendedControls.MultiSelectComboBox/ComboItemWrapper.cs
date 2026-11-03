using System;

namespace FormCustom.ExtendedControls.MultiSelectComboBox;

public class ComboItemWrapper
{
	public Guid Value { get; }

	public string Display { get; }

	public ComboItemWrapper(Guid value, string display)
	{
		Value = value;
		Display = display;
	}

	public override string ToString()
	{
		return Display;
	}
}
