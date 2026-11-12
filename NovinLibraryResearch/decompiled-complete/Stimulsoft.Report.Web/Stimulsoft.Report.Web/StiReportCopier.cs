using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Stimulsoft.Base;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Web;

internal static class StiReportCopier
{
	public static StiReport CloneReport(StiReport report, bool withResources = false)
	{
		if (report == null)
		{
			return null;
		}
		StiResourcesCollection stiResourcesCollection = null;
		string reportFile = report.ReportFile;
		if (!withResources)
		{
			stiResourcesCollection = CloneResource(report);
			report.Dictionary.Resources.Clear();
		}
		StiReport stiReport;
		if (!StiOptions.Engine.FullTrust || report.CalculationMode == StiCalculationMode.Interpretation || report.NeedsCompiling)
		{
			stiReport = new StiReport();
			MemoryStream memoryStream = new MemoryStream();
			report.Save(memoryStream);
			memoryStream.Seek(0L, SeekOrigin.Begin);
			stiReport.Load(memoryStream);
			memoryStream.Close();
		}
		else
		{
			stiReport = StiActivator.CreateObject(report.GetType()) as StiReport;
		}
		foreach (StiVariable variable in report.Dictionary.Variables)
		{
			StiDialogInfo dialogInfo = stiReport.Dictionary.Variables[variable.Name].DialogInfo;
			dialogInfo.Keys = variable.DialogInfo.Keys?.Clone() as string[];
			dialogInfo.Values = variable.DialogInfo.Values?.Clone() as string[];
			dialogInfo.CheckedStates = variable.DialogInfo.CheckedStates?.Clone() as bool[];
			FieldInfo field = report.GetType().GetField(variable.Name);
			if (field != null)
			{
				stiReport[variable.Name] = field.GetValue(report);
			}
		}
		if (report.Variables != null && report.Variables.Count > 0)
		{
			foreach (string key in report.Variables.Keys)
			{
				stiReport[key] = report[key];
			}
		}
		stiReport.RegData(report.DataStore);
		stiReport.RegBusinessObject(report.BusinessObjectsStore);
		if (stiResourcesCollection != null)
		{
			report.Dictionary.Resources.Clear();
			StiReportResourceHelper.LoadResourcesToReport(report, stiResourcesCollection);
			StiReportResourceHelper.LoadResourcesToReport(stiReport, stiResourcesCollection);
		}
		if (report.GlobalizationManager != null)
		{
			stiReport.GlobalizationManager = report.GlobalizationManager;
		}
		report.ReportFile = reportFile;
		stiReport.ReportFile = reportFile;
		stiReport.CookieContainer = report.CookieContainer;
		CopyFilterElementsUserFilters(report, stiReport);
		return stiReport;
	}

	private static StiResourcesCollection CloneResource(StiReport report)
	{
		StiResourcesCollection stiResourcesCollection = new StiResourcesCollection();
		foreach (StiResource resource in report.Dictionary.Resources)
		{
			stiResourcesCollection.Add(resource);
		}
		return stiResourcesCollection;
	}

	public static void CopyReportDictionary(StiReport reportFrom, StiReport reportTo)
	{
		if (reportTo == null)
		{
			return;
		}
		reportTo.Dictionary.Clear();
		if (reportFrom != null)
		{
			if (StiOptions.Designer.NewReport.AllowRegisterDataStoreFromOldReportInNewReport)
			{
				reportTo.Dictionary.DataStore.RegData(reportFrom.Dictionary.DataStore);
			}
			if (StiOptions.Designer.NewReport.AllowRegisterDatabasesFromOldReportInNewReport)
			{
				reportTo.Dictionary.Databases.AddRange(reportFrom.Dictionary.Databases);
			}
			if (StiOptions.Designer.NewReport.AllowRegisterDataSourcesFromOldReportInNewReport)
			{
				reportTo.Dictionary.DataSources.AddRange(reportFrom.Dictionary.DataSources);
			}
			if (StiOptions.Designer.NewReport.AllowRegisterRelationsFromOldReportInNewReport)
			{
				reportTo.Dictionary.Relations.AddRange(reportFrom.Dictionary.Relations);
			}
			if (StiOptions.Designer.NewReport.AllowRegisterVariablesFromOldReportInNewReport)
			{
				reportTo.Dictionary.Variables.AddRange(reportFrom.Dictionary.Variables);
			}
			if (StiOptions.Designer.NewReport.AllowRegisterResourcesFromOldReportInNewReport)
			{
				reportTo.Dictionary.Resources.AddRange(reportFrom.Dictionary.Resources);
			}
			if (StiOptions.Designer.NewReport.AllowRegisterRestrictionsFromOldReportInNewReport)
			{
				reportTo.Dictionary.Restrictions = reportFrom.Dictionary.Restrictions;
			}
			reportTo.Dictionary.BusinessObjects.AddRange(reportFrom.Dictionary.BusinessObjects);
			reportTo.Dictionary.SynchronizeBusinessObjects();
			reportTo.Tag = reportFrom.Tag;
			reportTo.Dictionary.Synchronize();
		}
	}

	public static void CopyElementsDrillDown(StiReport reportFrom, StiReport reportTo)
	{
		reportTo.GetComponents().ToList().ForEach(delegate(StiComponent c)
		{
			if (c is IStiDrillDownElement && reportFrom.Pages.GetComponentByName(c.Name) is IStiDrillDownElement stiDrillDownElement)
			{
				IStiDrillDownElement stiDrillDownElement2 = c as IStiDrillDownElement;
				stiDrillDownElement2.DrillDownCurrentLevel = stiDrillDownElement.DrillDownCurrentLevel;
				stiDrillDownElement2.DrillDownFilters = stiDrillDownElement.DrillDownFilters;
				stiDrillDownElement2.DrillDownFiltersList = stiDrillDownElement.DrillDownFiltersList;
			}
		});
	}

	public static void CopyFilterElementsUserFilters(StiReport reportFrom, StiReport reportTo)
	{
		reportFrom.GetComponents().ToList().ForEach(delegate(StiComponent comp)
		{
			if (comp is IStiFilterElement && comp is IStiUserFilters stiUserFilters)
			{
				List<StiDataFilterRule> userFilters = stiUserFilters.UserFilters;
				if (userFilters != null && userFilters.Count > 0)
				{
					StiComponent componentByName = reportTo.GetComponentByName(comp.Name);
					if (componentByName is IStiUserFilters)
					{
						((IStiUserFilters)componentByName).UserFilters = ((IStiUserFilters)comp).UserFilters;
					}
				}
			}
		});
	}
}
