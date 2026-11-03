using System;

namespace Stimulsoft.Report.Dictionary;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class StiAliasAttribute : Attribute
{
	public string Alias { get; set; }

	public StiAliasAttribute(string alias)
	{
		Alias = alias;
	}
}
