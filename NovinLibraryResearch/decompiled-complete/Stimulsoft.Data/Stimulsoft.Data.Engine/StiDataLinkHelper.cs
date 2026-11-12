using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;

namespace Stimulsoft.Data.Engine;

public static class StiDataLinkHelper
{
	public static List<StiDataLink> GetLinks(IStiAppDictionary dictionary)
	{
		return (from r in dictionary.FetchDataRelations()
			select new StiDataLink(r.GetParentDataSource()?.GetName(), r.GetChildDataSource()?.GetName(), r.FetchParentColumns().FirstOrDefault(), r.FetchChildColumns().FirstOrDefault(), r.GetActiveState(), r.GetKey())).ToList();
	}
}
