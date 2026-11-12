using System.Collections.Generic;
using System.Linq;

namespace Stimulsoft.Data.Engine;

public class StiDataSortVariation
{
	public static string Ident = "#Variation#";

	public static bool IsVariationSort(IEnumerable<StiDataSortRule> sort)
	{
		return sort?.Any((StiDataSortRule r) => r.Key == Ident) ?? false;
	}

	public static List<StiDataSortRule> CreateVariationSort(StiDataSortDirection direction)
	{
		return new StiDataSortRule(Ident, direction).ToList();
	}
}
