using System.Collections;
using System.Linq;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Dashboard.Viewer;

internal static class StiDashboardVariables
{
	private static bool IsParametersExist(StiReport report)
	{
		return report.Dictionary.Variables.ToList().Any((StiVariable v) => v.RequestFromUser);
	}

	internal static void FillParameters(StiReport report)
	{
		if (!IsParametersExist(report))
		{
			return;
		}
		Hashtable dataSourcesUsedInRequestFromUsersVariables = StiDataSourceHelper.GetDataSourcesUsedInRequestFromUsersVariables(report);
		Hashtable hashtable = new Hashtable();
		foreach (StiDataSource dataSource in report.Dictionary.DataSources)
		{
			if (!dataSourcesUsedInRequestFromUsersVariables.ContainsKey(dataSource.Name))
			{
				hashtable[dataSource.Name] = dataSource.ConnectOnStart;
				dataSource.ConnectOnStart = false;
			}
		}
		StiRenderProviderV2.ConnectToData(report);
		foreach (StiDataSource dataSource2 in report.Dictionary.DataSources)
		{
			if (hashtable.ContainsKey(dataSource2.Name))
			{
				dataSource2.ConnectOnStart = (bool)hashtable[dataSource2.Name];
			}
		}
		StiVariableHelper.FillItemsOfVariables(report.CompiledReport ?? report);
	}
}
