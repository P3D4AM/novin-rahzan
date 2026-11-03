using Stimulsoft.Report.CodeDom;

namespace Stimulsoft.Report.Dictionary;

public static class StiDataSourceExt
{
	public static string GetFixedName(this StiDataSource dataSource)
	{
		if (dataSource == null)
		{
			return null;
		}
		return StiCodeDomSerializator.ReplaceSymbols(dataSource.Name);
	}
}
