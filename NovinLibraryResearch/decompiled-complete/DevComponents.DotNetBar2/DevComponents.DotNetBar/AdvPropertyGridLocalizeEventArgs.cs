using System;

namespace DevComponents.DotNetBar;

public class AdvPropertyGridLocalizeEventArgs : EventArgs
{
	public readonly string Key;

	public readonly ePropertyGridLocalizationType LocalizationType;

	public string LocalizedValue;

	public AdvPropertyGridLocalizeEventArgs(string key, ePropertyGridLocalizationType localizationType)
	{
		Key = key;
		LocalizationType = localizationType;
	}
}
