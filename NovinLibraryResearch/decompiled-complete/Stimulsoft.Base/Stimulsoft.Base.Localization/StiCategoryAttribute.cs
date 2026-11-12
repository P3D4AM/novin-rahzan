using System;

namespace Stimulsoft.Base.Localization;

[AttributeUsage(AttributeTargets.Property)]
public sealed class StiCategoryAttribute : Attribute
{
	public const string Action = "Action";

	public const string Appearance = "Appearance";

	public const string Behavior = "Behavior";

	public const string Button = "Button";

	public const string CheckBox = "CheckBox";

	public const string Layout = "Layout";

	public string Category { get; set; }

	public StiCategorySource Source { get; set; }

	public StiCategoryAttribute(string category, StiCategorySource source = StiCategorySource.PropertyCategory)
	{
		Category = category;
		Source = source;
	}
}
