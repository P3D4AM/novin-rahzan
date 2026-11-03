using System;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Dictionary;

public static class StiDataSourceParserHelper
{
	public static void ConnectSqlSource(StiSqlSource sqlSource)
	{
		StiText component = new StiText
		{
			Name = "**DataSourceParameter**",
			Page = sqlSource.Dictionary?.Report?.Pages[0]
		};
		foreach (StiDataParameter parameter in sqlSource.Parameters)
		{
			parameter.ParameterValue = StiParser.ParseTextValue("{" + parameter.Value + "}", component);
		}
		sqlSource.SqlCommand = Convert.ToString(StiParser.ParseTextValue(sqlSource.SqlCommand, component));
	}
}
