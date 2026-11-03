namespace Stimulsoft.Report.Helpers;

public class StiDictionaryInitializer
{
	public static void Initialize(StiReport oldReport, StiReport newReport)
	{
		if (newReport == null)
		{
			return;
		}
		newReport.Dictionary.Clear();
		if (oldReport != null && StiOptions.Designer.NewReport.DictionaryBehavior == StiNewReportDictionaryBehavior.MergeDictionary)
		{
			if (StiOptions.Designer.NewReport.AllowRegisterDataStoreFromOldReportInNewReport)
			{
				newReport.Dictionary.DataStore.RegData(oldReport.Dictionary.DataStore);
			}
			if (StiOptions.Designer.NewReport.AllowRegisterDatabasesFromOldReportInNewReport)
			{
				newReport.Dictionary.Databases.AddRange(oldReport.Dictionary.Databases);
			}
			if (StiOptions.Designer.NewReport.AllowRegisterDataSourcesFromOldReportInNewReport)
			{
				newReport.Dictionary.DataSources.AddRange(oldReport.Dictionary.DataSources);
			}
			if (StiOptions.Designer.NewReport.AllowRegisterRelationsFromOldReportInNewReport)
			{
				newReport.Dictionary.Relations.AddRange(oldReport.Dictionary.Relations);
			}
			if (StiOptions.Designer.NewReport.AllowRegisterVariablesFromOldReportInNewReport)
			{
				newReport.Dictionary.Variables.AddRange(oldReport.Dictionary.Variables);
			}
			if (StiOptions.Designer.NewReport.AllowRegisterResourcesFromOldReportInNewReport)
			{
				newReport.Dictionary.Resources.AddRange(oldReport.Dictionary.Resources);
			}
			if (StiOptions.Designer.NewReport.AllowRegisterRestrictionsFromOldReportInNewReport)
			{
				newReport.Dictionary.Restrictions = oldReport.Dictionary.Restrictions;
			}
		}
	}
}
