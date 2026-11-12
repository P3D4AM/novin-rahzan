using System.Collections.ObjectModel;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class FacetInfoCollection : KeyedCollection<string, FacetInfo>
{
	protected override string GetKeyForItem(FacetInfo item)
	{
		return item.Name;
	}
}
