using System;

namespace DevComponents.Editors;

public class FreeTextEntryConversionEventArgs : EventArgs
{
	public readonly string ValueEntered;

	public object ControlValue;

	public bool IsValueConverted;

	public FreeTextEntryConversionEventArgs(string valueEntered)
	{
		ValueEntered = valueEntered;
	}
}
