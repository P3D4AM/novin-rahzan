using System;

namespace Stimulsoft.Controls.Win.Editors;

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
