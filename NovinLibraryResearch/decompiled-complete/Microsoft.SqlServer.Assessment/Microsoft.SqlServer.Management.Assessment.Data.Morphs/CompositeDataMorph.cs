using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Data.Morphs;

internal class CompositeDataMorph : IDataMorph
{
	public List<IDataMorph> Children { get; }

	public bool BenefitsFromCache => Children.All((IDataMorph c) => c.BenefitsFromCache);

	public CompositeDataMorph()
	{
		Children = new List<IDataMorph>();
	}

	public CompositeDataMorph(IEnumerable<IDataMorph> children)
	{
		Children = new List<IDataMorph>(children);
	}

	public IDataView ApplyTo(IDataView source, bool useCache = true)
	{
		int num = Children.FindLastIndex((IDataMorph c) => !c.BenefitsFromCache);
		IDataView dataView = source;
		for (int i = 0; i < num; i++)
		{
			dataView = Children[i].ApplyTo(dataView, useCache: false);
		}
		if (num >= 0)
		{
			dataView = Children[num].ApplyTo(dataView, useCache);
		}
		for (int j = num + 1; j < Children.Count; j++)
		{
			dataView = Children[j].ApplyTo(dataView, useCache: false);
		}
		return dataView;
	}
}
