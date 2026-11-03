using System;
using System.ComponentModel;
using System.Resources;

namespace DevComponents.DotNetBar;

[EditorBrowsable(EditorBrowsableState.Never)]
public class LocalizationManager : IDisposable
{
	private ResourceManager _0DF7;

	private IOwnerLocalize _0DF8;

	public LocalizationManager()
		: this(null)
	{
	}

	public LocalizationManager(IOwnerLocalize manager)
	{
		_0DF8 = manager;
	}

	public void Dispose()
	{
		_0DF7 = null;
		_0DF8 = null;
	}

	public string GetDefaultLocalizedString(string key)
	{
		string text = GetLocalizedString(key);
		if (text == "" || text == null)
		{
			ResourceManager resourceManager = BarFunctions._०(_१: true);
			text = resourceManager.GetString(key);
		}
		if (text == null)
		{
			text = "";
		}
		return text;
	}

	public static string GetLocalizedString(string key, string defaultValue)
	{
		LocalizeEventArgs localizeEventArgs = new LocalizeEventArgs();
		localizeEventArgs.Key = key;
		localizeEventArgs.LocalizedValue = defaultValue;
		LocalizationKeys._0DF9(localizeEventArgs);
		if (localizeEventArgs.Handled)
		{
			return localizeEventArgs.LocalizedValue;
		}
		return defaultValue;
	}

	public string GetLocalizedString(string key)
	{
		string text = "";
		if (_0DF7 == null)
		{
			_0DF7 = BarFunctions._०();
		}
		if (_0DF7 != null)
		{
			text = _0DF7.GetString(key);
			if (text == null)
			{
				text = "";
			}
		}
		LocalizeEventArgs localizeEventArgs = new LocalizeEventArgs();
		localizeEventArgs.Key = key;
		localizeEventArgs.LocalizedValue = text;
		LocalizationKeys._0DF9(localizeEventArgs);
		if (localizeEventArgs.Handled)
		{
			return localizeEventArgs.LocalizedValue;
		}
		if (_0DF8 != null)
		{
			_0DF8.InvokeLocalizeString(localizeEventArgs);
			if (localizeEventArgs.Handled)
			{
				return localizeEventArgs.LocalizedValue;
			}
		}
		return text;
	}
}
