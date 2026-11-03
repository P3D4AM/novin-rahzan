using Stimulsoft.Data.Engine;

namespace Stimulsoft.Report.Dashboard;

public static class StiGroupElementHelper
{
	public static string GetGroup(IStiElement element)
	{
		if (!(element is IStiGroupElement))
		{
			return null;
		}
		string group = (element as IStiGroupElement).Group;
		if (group == null)
		{
			return null;
		}
		IStiCrossFiltering val = (IStiCrossFiltering)((element is IStiCrossFiltering) ? element : null);
		if (val != null && !val.CrossFiltering)
		{
			return null;
		}
		return group.ToLowerInvariant().Trim();
	}
}
