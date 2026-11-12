using System;

namespace Stimulsoft.Base.Localization;

[AttributeUsage(AttributeTargets.Property)]
public sealed class StiLocalizationKeyAttribute : Attribute
{
	public string Category { get; set; }

	public string Key { get; set; }

	public StiLocalizationKeyAttribute(string category, string key)
	{
		Category = category;
		Key = key;
	}
}
