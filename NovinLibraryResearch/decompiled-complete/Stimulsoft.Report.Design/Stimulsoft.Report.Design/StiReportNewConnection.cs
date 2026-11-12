using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Server;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Dictionary;
using Stimulsoft.Report.Design.Panels;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Dictionary.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Design;

public static class StiReportNewConnection
{
	public static bool InvokeDataStoreSourceEdit(StiDesignerControl designer, StiReport report, StiSqlDatabase sqlDatabase)
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Invalid comparison between Unknown and I4
		StiSqlAdapterService dataAdapter = sqlDatabase.GetDataAdapter();
		StiSqlSource stiSqlSource = dataAdapter.Create(report.Dictionary) as StiSqlSource;
		stiSqlSource.NameInSource = sqlDatabase.Name;
		StiEditDataStoreAdapterEventArgs stiEditDataStoreAdapterEventArgs = new StiEditDataStoreAdapterEventArgs(designer, dataAdapter, report.Dictionary, stiSqlSource);
		StiOptions.Designer.Editors.InvokeEditDataStoreAdapter(stiEditDataStoreAdapterEventArgs);
		if (stiEditDataStoreAdapterEventArgs.DialogResult.HasValue)
		{
			return false;
		}
		string name = stiSqlSource.Name;
		StiDataStoreSourceEditForm stiDataStoreSourceEditForm = new StiDataStoreSourceEditForm(report.Dictionary, dataAdapter, stiSqlSource);
		designer?.AddOwnedForm((Form)(object)stiDataStoreSourceEditForm);
		if ((int)((Form)stiDataStoreSourceEditForm).ShowDialog() == 1)
		{
			if (stiSqlSource.Name != name)
			{
				StiComponentsCollection components = report.GetComponents();
				foreach (StiComponent item in components)
				{
					if (item is IStiDataSource stiDataSource && stiDataSource.DataSourceName == name)
					{
						stiDataSource.DataSourceName = stiSqlSource.Name;
					}
				}
			}
			report.Dictionary.Connect(loadData: false);
			if (designer != null)
			{
				designer.GetDictionaryService().BuildDictionary();
				TreeNode val = designer.GetDictionaryService().DictionaryControl.SelectNodeByTag(stiSqlSource);
				if (val != null)
				{
					val.EnsureVisible();
				}
			}
			return true;
		}
		return false;
	}

	public static StiCreateNewConnectionResult CreateNewConnection(StiDesignerControl designer, StiReport report)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Invalid comparison between Unknown and I4
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Invalid comparison between Unknown and I4
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Invalid comparison between Unknown and I4
		StiDatabase stiDatabase = null;
		StiDatabase stiDatabase2 = null;
		StiDataAdapterService stiDataAdapterService = null;
		bool flag = false;
		StiDatabaseSelectForm stiDatabaseSelectForm = new StiDatabaseSelectForm(report);
		try
		{
			if (designer != null)
			{
				designer.AddOwnedForm((Form)(object)stiDatabaseSelectForm);
			}
			else if (((ReadOnlyCollectionBase)(object)Application.OpenForms).Count > 0)
			{
				Form val = Application.OpenForms[0];
				val.AddOwnedForm((Form)(object)stiDatabaseSelectForm);
			}
			if ((int)((Form)stiDatabaseSelectForm).ShowDialog() == 1)
			{
				stiDatabase = stiDatabaseSelectForm.SelectedDatabase;
				stiDatabase2 = stiDatabaseSelectForm.CreatedDatabase;
				stiDataAdapterService = stiDatabaseSelectForm.AdapterService;
				flag = stiDatabaseSelectForm.SkipSchemaWizard;
			}
		}
		finally
		{
			((IDisposable)stiDatabaseSelectForm)?.Dispose();
		}
		if (stiDatabase2 != null)
		{
			StiSqlDatabase stiSqlDatabase = stiDatabase2 as StiSqlDatabase;
			if (stiSqlDatabase != null && flag)
			{
				InvokeDataStoreSourceEdit(designer, report, stiSqlDatabase);
				return null;
			}
			if (stiDatabase2 is StiSqlDatabase)
			{
				StiSqlDatabase stiSqlDatabase2 = (StiSqlDatabase)stiDatabase2;
				StiSqlDataConnector stiSqlDataConnector = stiSqlDatabase2.CreateSqlConnector(stiSqlDatabase2.ConnectionString);
				StiDataConnector[] familyConnectors = stiSqlDataConnector.GetFamilyConnectors();
				if (!stiSqlDataConnector.IsAvailable && familyConnectors != null && StiAccountSettings.CurrentAccount != null && familyConnectors.FirstOrDefault((StiDataConnector x) => x.NuGetPackages != null) != null)
				{
					IStiNuGetWindow nuGetWindow = StiAccountSettings.CurrentAccount.AccountCreater.GetNuGetWindow(stiSqlDataConnector);
					if (designer != null)
					{
						nuGetWindow.ShowDialog(((Control)designer).Handle);
					}
					else
					{
						nuGetWindow.ShowDialog();
					}
					if (nuGetWindow.IsDownloaded)
					{
						stiSqlDataConnector.ResetSettings();
						stiSqlDatabase2.CreateSqlConnector(stiSqlDatabase2.ConnectionString);
					}
				}
			}
			else if (stiDatabase2 is StiNoSqlDatabase)
			{
				StiNoSqlDatabase stiNoSqlDatabase = (StiNoSqlDatabase)stiDatabase2;
				StiNoSqlDataConnector stiNoSqlDataConnector = stiNoSqlDatabase.CreateNoSqlConnector(stiNoSqlDatabase.ConnectionString);
				StiDataConnector[] familyConnectors2 = stiNoSqlDataConnector.GetFamilyConnectors();
				if (!stiNoSqlDataConnector.IsAvailable && familyConnectors2 != null && StiAccountSettings.CurrentAccount != null && familyConnectors2.FirstOrDefault((StiDataConnector x) => x.NuGetPackages != null) != null)
				{
					IStiNuGetWindow nuGetWindow2 = StiAccountSettings.CurrentAccount.AccountCreater.GetNuGetWindow(stiNoSqlDataConnector);
					if (designer != null)
					{
						nuGetWindow2.ShowDialog(((Control)designer).Handle);
					}
					else
					{
						nuGetWindow2.ShowDialog();
					}
					if (nuGetWindow2.IsDownloaded)
					{
						stiNoSqlDataConnector.ResetSettings();
						stiNoSqlDatabase.CreateNoSqlConnector(stiNoSqlDatabase.ConnectionString);
					}
				}
			}
			StiSelectDataForm stiSelectDataForm = new StiSelectDataForm(report, stiDatabase2);
			designer?.AddOwnedForm((Form)(object)stiSelectDataForm);
			if ((int)((Form)stiSelectDataForm).ShowDialog() == 1)
			{
				designer?.GetDictionaryService()?.BuildDictionary();
			}
			return null;
		}
		if (report != null && stiDataAdapterService != null)
		{
			StiDictionary dictionary = report.Dictionary;
			StiDataSource stiDataSource = stiDataAdapterService.Create(dictionary);
			if (!stiDataAdapterService.New(dictionary, stiDataSource))
			{
				dictionary.DataSources.Remove(stiDataSource);
			}
			else if (designer != null)
			{
				StiDictionaryPanelService dictionaryService = designer.GetDictionaryService();
				if (stiDataSource != null)
				{
					StiDataLeader.Connect(stiDataSource, loadData: false);
					dictionaryService.BuildDictionary();
					if (((TreeView)dictionaryService.DictionaryControl.tvData).SelectedNode == null || !(((TreeView)dictionaryService.DictionaryControl.tvData).SelectedNode.Tag is StiDatabase))
					{
						dictionaryService.DictionaryControl.SelectNodeByTag("DataSources");
					}
					dictionaryService.DictionaryControl.ExpandTreeNode(((TreeView)dictionaryService.DictionaryControl.tvData).SelectedNode);
					dictionaryService.DictionaryControl.SelectNodeByTag(stiDataSource);
				}
			}
			return null;
		}
		if (stiDatabase != null)
		{
			StiDatabase stiDatabase3 = (StiDatabase)stiDatabase.Clone();
			string alias = (stiDatabase3.Name = ((report != null) ? StiNameCreation.CreateConnectionName(report, stiDatabase3.ServiceName) : stiDatabase3.ServiceName));
			stiDatabase3.Alias = alias;
			if ((int)stiDatabase3.Edit(report?.Dictionary, newDatabase: true) == 1)
			{
				return new StiCreateNewConnectionResult
				{
					Database = stiDatabase3,
					SkipSchemaWizard = flag
				};
			}
		}
		return null;
	}

	public static async Task<StiCreateNewConnectionResult> CreateNewConnectionAsync(StiDesignerControl designer, StiReport report)
	{
		StiDatabase stiDatabase = null;
		StiDatabase stiDatabase2 = null;
		StiDataAdapterService stiDataAdapterService = null;
		bool flag = false;
		bool skipSchemaWizard = false;
		StiDatabaseSelectForm stiDatabaseSelectForm = new StiDatabaseSelectForm(report);
		try
		{
			if (designer != null)
			{
				designer.AddOwnedForm((Form)(object)stiDatabaseSelectForm);
			}
			else if (((ReadOnlyCollectionBase)(object)Application.OpenForms).Count > 0)
			{
				Form val = Application.OpenForms[0];
				val.AddOwnedForm((Form)(object)stiDatabaseSelectForm);
			}
			if ((int)((Form)stiDatabaseSelectForm).ShowDialog() == 1)
			{
				stiDatabase = stiDatabaseSelectForm.SelectedDatabase;
				stiDatabase2 = stiDatabaseSelectForm.CreatedDatabase;
				stiDataAdapterService = stiDatabaseSelectForm.AdapterService;
				skipSchemaWizard = stiDatabaseSelectForm.SkipSchemaWizard;
			}
		}
		finally
		{
			((IDisposable)stiDatabaseSelectForm)?.Dispose();
		}
		if (stiDatabase2 != null)
		{
			StiSqlDatabase stiSqlDatabase = stiDatabase2 as StiSqlDatabase;
			if (stiSqlDatabase != null && skipSchemaWizard)
			{
				InvokeDataStoreSourceEdit(designer, report, stiSqlDatabase);
				return null;
			}
			if (stiDatabase2 is StiSqlDatabase)
			{
				StiSqlDatabase stiSqlDatabase2 = (StiSqlDatabase)stiDatabase2;
				StiSqlDataConnector stiSqlDataConnector = stiSqlDatabase2.CreateSqlConnector(stiSqlDatabase2.ConnectionString);
				StiDataConnector[] familyConnectors = stiSqlDataConnector.GetFamilyConnectors();
				if (!stiSqlDataConnector.IsAvailable && familyConnectors != null && StiAccountSettings.CurrentAccount != null && familyConnectors.FirstOrDefault((StiDataConnector x) => x.NuGetPackages != null) != null)
				{
					IStiNuGetWindow nuGetWindow = StiAccountSettings.CurrentAccount.AccountCreater.GetNuGetWindow(stiSqlDataConnector);
					if (designer != null)
					{
						nuGetWindow.ShowDialog(((Control)designer).Handle);
					}
					else
					{
						nuGetWindow.ShowDialog();
					}
					if (nuGetWindow.IsDownloaded)
					{
						stiSqlDataConnector.ResetSettings();
						stiSqlDatabase2.CreateSqlConnector(stiSqlDatabase2.ConnectionString);
					}
				}
			}
			else if (stiDatabase2 is StiNoSqlDatabase)
			{
				StiNoSqlDatabase stiNoSqlDatabase = (StiNoSqlDatabase)stiDatabase2;
				StiNoSqlDataConnector stiNoSqlDataConnector = stiNoSqlDatabase.CreateNoSqlConnector(stiNoSqlDatabase.ConnectionString);
				StiDataConnector[] familyConnectors2 = stiNoSqlDataConnector.GetFamilyConnectors();
				if (!stiNoSqlDataConnector.IsAvailable && familyConnectors2 != null && StiAccountSettings.CurrentAccount != null && familyConnectors2.FirstOrDefault((StiDataConnector x) => x.NuGetPackages != null) != null)
				{
					IStiNuGetWindow nuGetWindow2 = StiAccountSettings.CurrentAccount.AccountCreater.GetNuGetWindow(stiNoSqlDataConnector);
					if (designer != null)
					{
						nuGetWindow2.ShowDialog(((Control)designer).Handle);
					}
					else
					{
						nuGetWindow2.ShowDialog();
					}
					if (nuGetWindow2.IsDownloaded)
					{
						stiNoSqlDataConnector.ResetSettings();
						stiNoSqlDatabase.CreateNoSqlConnector(stiNoSqlDatabase.ConnectionString);
					}
				}
			}
			StiSelectDataForm stiSelectDataForm = new StiSelectDataForm(report, stiDatabase2);
			designer?.AddOwnedForm((Form)(object)stiSelectDataForm);
			if ((int)((Form)stiSelectDataForm).ShowDialog() == 1 && designer != null)
			{
				designer.GetDictionaryService().BuildDictionary();
				StiDictionaryPanelService service = StiDictionaryPanelService.GetService(designer);
				if (service != null)
				{
					StiDatabaseInformation informationSelected = stiSelectDataForm.InformationSelected;
					string text = informationSelected.Tables?.FirstOrDefault()?.TableName;
					text = text ?? informationSelected.Views?.FirstOrDefault()?.TableName;
					text = text ?? informationSelected.StoredProcedures?.FirstOrDefault()?.TableName;
					if (!string.IsNullOrWhiteSpace(text))
					{
						StiDataSource tag = report.Dictionary.DataSources[text];
						service.DictionaryControl.SelectNodeByTag(tag);
					}
				}
			}
			return null;
		}
		if (report != null && stiDataAdapterService != null)
		{
			StiDictionary dictionary = report.Dictionary;
			StiDataSource stiDataSource = stiDataAdapterService.Create(dictionary);
			if (!stiDataAdapterService.New(dictionary, stiDataSource))
			{
				dictionary.DataSources.Remove(stiDataSource);
			}
			else if (designer != null)
			{
				StiDictionaryPanelService dictionaryService = designer.GetDictionaryService();
				if (stiDataSource != null)
				{
					StiDataLeader.Connect(stiDataSource, loadData: false);
					dictionaryService.BuildDictionary();
					if (((TreeView)dictionaryService.DictionaryControl.tvData).SelectedNode == null || !(((TreeView)dictionaryService.DictionaryControl.tvData).SelectedNode.Tag is StiDatabase))
					{
						dictionaryService.DictionaryControl.SelectNodeByTag("DataSources");
					}
					dictionaryService.DictionaryControl.ExpandTreeNode(((TreeView)dictionaryService.DictionaryControl.tvData).SelectedNode);
					dictionaryService.DictionaryControl.SelectNodeByTag(stiDataSource);
				}
			}
			return null;
		}
		if (stiDatabase != null)
		{
			StiDatabase newDatabase = (StiDatabase)stiDatabase.Clone();
			if (!flag)
			{
				string alias = (newDatabase.Name = ((report != null) ? StiNameCreation.CreateConnectionName(report, newDatabase.ServiceName) : newDatabase.ServiceName));
				newDatabase.Alias = alias;
			}
			if ((int)(await newDatabase.EditAsync(report?.Dictionary, newDatabase: true)) == 1)
			{
				newDatabase = PrepareExcelDatabase(newDatabase);
				return new StiCreateNewConnectionResult
				{
					Database = newDatabase,
					SkipSchemaWizard = skipSchemaWizard
				};
			}
		}
		return null;
	}

	private static StiDatabase PrepareExcelDatabase(StiDatabase database)
	{
		if (database is StiExcelDatabase { PathData: var pathData } && !string.IsNullOrEmpty(pathData) && Path.GetExtension(pathData) == ".csv")
		{
			return new StiCsvDatabase(database.Name, pathData);
		}
		return database;
	}
}
