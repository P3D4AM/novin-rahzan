using System;
using Microsoft.SqlServer.Management.Assessment.Configuration;

namespace Microsoft.SqlServer.Management.Assessment.Probes;

public class ProbeFamily : PatternDictionary<ISqlObjectLocator, IProbe>
{
	public string Name { get; set; }

	public ProbeFamily()
		: base((Func<IProbe, IPattern<ISqlObjectLocator>>)((IProbe value) => value.Target))
	{
	}

	public ProbeFamily(string name)
		: base((Func<IProbe, IPattern<ISqlObjectLocator>>)((IProbe value) => value.Target))
	{
		Name = name;
	}

	protected internal override void TagItem(IProbe value)
	{
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		base.TagItem(value);
		value.FamilyName = Name;
	}
}
