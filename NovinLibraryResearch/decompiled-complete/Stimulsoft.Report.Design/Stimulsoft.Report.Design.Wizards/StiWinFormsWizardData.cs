using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stimulsoft.Base.Wizards;
using Stimulsoft.Report.Design.Panels;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Design.Wizards;

public sealed class StiWinFormsWizardData : IStiWizardData
{
	public bool AllowAsync => false;

	public object CreateNewConnection(object designer, object report, object ownerWindow)
	{
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			StiReport stiReport = (StiReport)report;
			StiCreateNewConnectionResult stiCreateNewConnectionResult = StiReportNewConnection.CreateNewConnection(designer as StiDesignerControl, stiReport);
			if (stiCreateNewConnectionResult == null)
			{
				return null;
			}
			StiDatabase stiDatabase = stiCreateNewConnectionResult.Database;
			StiDictionaryPanelService service = StiDictionaryPanelService.GetService(designer as StiDesignerControl);
			if (service != null && service.MoveToResourceXmlDatabase != null)
			{
				stiDatabase = service.MoveToResourceXmlDatabase;
				service.MoveToResourceXmlDatabase = null;
			}
			StiDictionary dictionary = stiReport.Dictionary;
			if (StiNameCreation.IsConnectionNameExists(stiReport, stiDatabase.Name))
			{
				string text = StiNameCreation.CreateConnectionName(stiReport, stiDatabase.Name);
				if (stiDatabase.Name == stiDatabase.Alias)
				{
					stiDatabase.Alias = text;
				}
				stiDatabase.Name = text;
			}
			dictionary.Databases.Add(stiDatabase);
			dictionary.Connect(loadData: false);
			if (stiCreateNewConnectionResult.SkipSchemaWizard && stiDatabase is StiSqlDatabase)
			{
				StiReportNewConnection.InvokeDataStoreSourceEdit(designer as StiDesignerControl, stiReport, (StiSqlDatabase)stiDatabase);
			}
			else
			{
				StiSelectDataForm stiSelectDataForm = new StiSelectDataForm(stiReport, stiDatabase);
				((Form)stiSelectDataForm).ShowDialog();
			}
			return stiDatabase;
		}
		catch
		{
		}
		return null;
	}

	public Task<object> CreateNewConnectionAsync(object designer, object report, object ownerWindow)
	{
		throw new NotSupportedException();
	}
}
