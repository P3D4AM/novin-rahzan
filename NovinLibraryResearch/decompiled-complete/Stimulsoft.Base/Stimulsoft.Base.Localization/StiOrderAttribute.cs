using System;

namespace Stimulsoft.Base.Localization;

[AttributeUsage(AttributeTargets.Property)]
public class StiOrderAttribute : Attribute
{
	public int Position { get; }

	public StiOrderAttribute(int position)
	{
		Position = position;
	}
}
