using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Server;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Dictionary;
using Stimulsoft.Report.Design.Panels;
using Stimulsoft.Report.Dictionary.Databases.Google;

namespace Stimulsoft.Report.Dictionary.Design;

public sealed class StiDataEditors : IStiDataEditors
{
	public DialogResult CsvDatabaseEdit(StiCsvDatabase database, StiDictionary dictionary, bool newDatabase)
	{
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Invalid comparison between Unknown and I4
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		StiCsvDatabaseEditForm stiCsvDatabaseEditForm = new StiCsvDatabaseEditForm(dictionary.Report, newDatabase ? null : database.Name);
		try
		{
			if (dictionary.Report.Designer != null && dictionary.Report.Designer.DesignerControl != null)
			{
				dictionary.Report.Designer.DesignerControl.AddOwnedForm((Form)(object)stiCsvDatabaseEditForm);
			}
			if (newDatabase)
			{
				((Control)stiCsvDatabaseEditForm).Text = string.Format(StiLocalization.Get("FormDatabaseEdit", "NewConnection"), "CSV");
			}
			else
			{
				((Control)stiCsvDatabaseEditForm).Text = string.Format(StiLocalization.Get("FormDatabaseEdit", "EditConnection"), "CSV");
			}
			((Control)stiCsvDatabaseEditForm.tbName).Text = database.Name;
			((Control)stiCsvDatabaseEditForm.tbAlias).Text = database.Alias;
			((Control)stiCsvDatabaseEditForm.bePathData).Text = database.PathData;
			stiCsvDatabaseEditForm.Separator = database.Separator;
			stiCsvDatabaseEditForm.CodePage = database.CodePage;
			if ((int)((Form)stiCsvDatabaseEditForm).ShowDialog() == 1)
			{
				if (stiCsvDatabaseEditForm.XmlDatabase != null)
				{
					if (dictionary.Report.Designer != null && dictionary.Report.Designer.DesignerControl is StiDesignerControl)
					{
						StiDictionaryPanelService service = StiDictionaryPanelService.GetService((StiDesignerControl)dictionary.Report.Designer.DesignerControl);
						if (service != null)
						{
							service.MoveToResourceXmlDatabase = stiCsvDatabaseEditForm.XmlDatabase;
						}
					}
				}
				else
				{
					if (database.PathData != ((Control)stiCsvDatabaseEditForm.bePathData).Text)
					{
						StiCacheCleaner.Clean(dictionary);
					}
					database.Alias = ((Control)stiCsvDatabaseEditForm.tbAlias).Text;
					database.PathData = ((Control)stiCsvDatabaseEditForm.bePathData).Text;
					database.CodePage = stiCsvDatabaseEditForm.CodePage;
					database.Separator = stiCsvDatabaseEditForm.Separator;
					dictionary.RenameDatabase(database, ((Control)stiCsvDatabaseEditForm.tbName).Text);
					if (StiRecentConnections.RecentConnections != null)
					{
						StiRecentConnections.RecentConnections.AddAndSave(database);
						StiRecentConnections.RecentConnections = null;
					}
				}
			}
			return ((Form)stiCsvDatabaseEditForm).DialogResult;
		}
		finally
		{
			((IDisposable)stiCsvDatabaseEditForm)?.Dispose();
		}
	}

	public DialogResult DBaseDatabaseEdit(StiDBaseDatabase database, StiDictionary dictionary, bool newDatabase)
	{
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Invalid comparison between Unknown and I4
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		StiDBaseDatabaseEditForm stiDBaseDatabaseEditForm = new StiDBaseDatabaseEditForm(dictionary.Report, newDatabase ? null : database.Name);
		try
		{
			if (dictionary.Report.Designer != null && dictionary.Report.Designer.DesignerControl != null)
			{
				dictionary.Report.Designer.DesignerControl.AddOwnedForm((Form)(object)stiDBaseDatabaseEditForm);
			}
			if (newDatabase)
			{
				((Control)stiDBaseDatabaseEditForm).Text = string.Format(StiLocalization.Get("FormDatabaseEdit", "NewConnection"), "DBase");
			}
			else
			{
				((Control)stiDBaseDatabaseEditForm).Text = string.Format(StiLocalization.Get("FormDatabaseEdit", "EditConnection"), "DBase");
			}
			((Control)stiDBaseDatabaseEditForm.tbName).Text = database.Name;
			((Control)stiDBaseDatabaseEditForm.tbAlias).Text = database.Alias;
			((Control)stiDBaseDatabaseEditForm.bePathData).Text = database.PathData;
			stiDBaseDatabaseEditForm.CodePage = database.CodePage;
			if ((int)((Form)stiDBaseDatabaseEditForm).ShowDialog() == 1)
			{
				if (stiDBaseDatabaseEditForm.XmlDatabase != null)
				{
					if (dictionary.Report.Designer != null && dictionary.Report.Designer.DesignerControl is StiDesignerControl)
					{
						StiDictionaryPanelService service = StiDictionaryPanelService.GetService((StiDesignerControl)dictionary.Report.Designer.DesignerControl);
						if (service != null)
						{
							service.MoveToResourceXmlDatabase = stiDBaseDatabaseEditForm.XmlDatabase;
						}
					}
				}
				else
				{
					if (database.PathData != ((Control)stiDBaseDatabaseEditForm.bePathData).Text)
					{
						StiCacheCleaner.Clean(dictionary);
					}
					database.Alias = ((Control)stiDBaseDatabaseEditForm.tbAlias).Text;
					database.PathData = ((Control)stiDBaseDatabaseEditForm.bePathData).Text;
					database.CodePage = stiDBaseDatabaseEditForm.CodePage;
					dictionary.RenameDatabase(database, ((Control)stiDBaseDatabaseEditForm.tbName).Text);
					if (StiRecentConnections.RecentConnections != null)
					{
						StiRecentConnections.RecentConnections.AddAndSave(database);
						StiRecentConnections.RecentConnections = null;
					}
				}
			}
			return ((Form)stiDBaseDatabaseEditForm).DialogResult;
		}
		finally
		{
			((IDisposable)stiDBaseDatabaseEditForm)?.Dispose();
		}
	}

	public DialogResult ExcelDatabaseEdit(StiExcelDatabase database, StiDictionary dictionary, bool newDatabase)
	{
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Invalid comparison between Unknown and I4
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		StiExcelDatabaseEditForm stiExcelDatabaseEditForm = new StiExcelDatabaseEditForm(dictionary.Report, newDatabase ? null : database.Name);
		try
		{
			if (dictionary.Report.Designer != null && dictionary.Report.Designer.DesignerControl != null)
			{
				dictionary.Report.Designer.DesignerControl.AddOwnedForm((Form)(object)stiExcelDatabaseEditForm);
			}
			if (newDatabase)
			{
				((Control)stiExcelDatabaseEditForm).Text = string.Format(StiLocalization.Get("FormDatabaseEdit", "NewConnection"), "Excel");
			}
			else
			{
				((Control)stiExcelDatabaseEditForm).Text = string.Format(StiLocalization.Get("FormDatabaseEdit", "EditConnection"), "Excel");
			}
			((Control)stiExcelDatabaseEditForm.tbName).Text = database.Name;
			((Control)stiExcelDatabaseEditForm.tbAlias).Text = database.Alias;
			((Control)stiExcelDatabaseEditForm.bePathData).Text = database.PathData;
			((CheckBox)stiExcelDatabaseEditForm.checkBoxFirstRowIsHeader).Checked = database.FirstRowIsHeader;
			if ((int)((Form)stiExcelDatabaseEditForm).ShowDialog() == 1)
			{
				if (database.PathData != ((Control)stiExcelDatabaseEditForm.bePathData).Text)
				{
					StiCacheCleaner.Clean(dictionary);
				}
				database.Alias = ((Control)stiExcelDatabaseEditForm.tbAlias).Text;
				database.PathData = ((Control)stiExcelDatabaseEditForm.bePathData).Text;
				database.FirstRowIsHeader = ((CheckBox)stiExcelDatabaseEditForm.checkBoxFirstRowIsHeader).Checked;
				dictionary.RenameDatabase(database, ((Control)stiExcelDatabaseEditForm.tbName).Text);
				if (StiRecentConnections.RecentConnections != null)
				{
					StiRecentConnections.RecentConnections.AddAndSave(database);
					StiRecentConnections.RecentConnections = null;
				}
				StiCacheCleaner.Clean(dictionary);
			}
			return ((Form)stiExcelDatabaseEditForm).DialogResult;
		}
		finally
		{
			((IDisposable)stiExcelDatabaseEditForm)?.Dispose();
		}
	}

	public DialogResult JsonDatabaseEdit(StiJsonDatabase database, StiDictionary dictionary, bool newDatabase)
	{
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Invalid comparison between Unknown and I4
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		StiJsonDatabaseEditForm stiJsonDatabaseEditForm = new StiJsonDatabaseEditForm(dictionary.Report, newDatabase ? null : database.Name);
		try
		{
			if (dictionary.Report.Designer != null && dictionary.Report.Designer.DesignerControl != null)
			{
				dictionary.Report.Designer.DesignerControl.AddOwnedForm((Form)(object)stiJsonDatabaseEditForm);
			}
			if (newDatabase)
			{
				((Control)stiJsonDatabaseEditForm).Text = StiLocalization.Get("FormDatabaseEdit", "JsonNew");
			}
			else
			{
				((Control)stiJsonDatabaseEditForm).Text = StiLocalization.Get("FormDatabaseEdit", "JsonEdit");
			}
			((Control)stiJsonDatabaseEditForm.tbName).Text = database.Name;
			((Control)stiJsonDatabaseEditForm.tbAlias).Text = database.Alias;
			((Control)stiJsonDatabaseEditForm.bePathData).Text = database.PathData;
			stiJsonDatabaseEditForm.RelationDirection = database.RelationDirection;
			if ((int)((Form)stiJsonDatabaseEditForm).ShowDialog() == 1)
			{
				if (database.PathData != ((Control)stiJsonDatabaseEditForm.bePathData).Text)
				{
					StiCacheCleaner.Clean(dictionary);
				}
				database.Alias = ((Control)stiJsonDatabaseEditForm.tbAlias).Text;
				database.PathData = ((Control)stiJsonDatabaseEditForm.bePathData).Text;
				database.RelationDirection = stiJsonDatabaseEditForm.RelationDirection;
				dictionary.RenameDatabase(database, ((Control)stiJsonDatabaseEditForm.tbName).Text);
			}
			return ((Form)stiJsonDatabaseEditForm).DialogResult;
		}
		finally
		{
			((IDisposable)stiJsonDatabaseEditForm)?.Dispose();
		}
	}

	public DialogResult GisDatabaseEdit(StiGisDatabase database, StiDictionary dictionary, bool newDatabase)
	{
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Invalid comparison between Unknown and I4
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		StiGisDatabaseEditForm stiGisDatabaseEditForm = new StiGisDatabaseEditForm(dictionary.Report, newDatabase ? null : database.Name);
		try
		{
			if (dictionary.Report.Designer != null && dictionary.Report.Designer.DesignerControl != null)
			{
				dictionary.Report.Designer.DesignerControl.AddOwnedForm((Form)(object)stiGisDatabaseEditForm);
			}
			((Control)stiGisDatabaseEditForm).Text = (newDatabase ? StiLocalization.Get("FormDatabaseEdit", "GisNew") : StiLocalization.Get("FormDatabaseEdit", "GisEdit"));
			((Control)stiGisDatabaseEditForm.tbName).Text = database.Name;
			((Control)stiGisDatabaseEditForm.tbAlias).Text = database.Alias;
			((Control)stiGisDatabaseEditForm.bePathData).Text = database.PathData;
			stiGisDatabaseEditForm.Separator = database.Separator;
			stiGisDatabaseEditForm.DataType = database.DataType;
			if ((int)((Form)stiGisDatabaseEditForm).ShowDialog() == 1)
			{
				if (database.PathData != ((Control)stiGisDatabaseEditForm.bePathData).Text)
				{
					StiCacheCleaner.Clean(dictionary);
				}
				database.Alias = ((Control)stiGisDatabaseEditForm.tbAlias).Text;
				database.PathData = ((Control)stiGisDatabaseEditForm.bePathData).Text;
				database.Separator = stiGisDatabaseEditForm.Separator;
				database.DataType = stiGisDatabaseEditForm.DataType;
				dictionary.RenameDatabase(database, ((Control)stiGisDatabaseEditForm.tbName).Text);
			}
			return ((Form)stiGisDatabaseEditForm).DialogResult;
		}
		finally
		{
			((IDisposable)stiGisDatabaseEditForm)?.Dispose();
		}
	}

	public DialogResult SqlDatabaseEdit(StiSqlDatabase database, StiDictionary dictionary, bool newDatabase)
	{
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		StiSqlDataConnector stiSqlDataConnector = database.CreateSqlConnector(database.ConnectionString);
		StiDataConnector[] familyConnectors = stiSqlDataConnector.GetFamilyConnectors();
		if (!stiSqlDataConnector.IsAvailable && familyConnectors != null)
		{
			if (StiAccountSettings.CurrentAccount != null)
			{
				if (familyConnectors.FirstOrDefault((StiDataConnector x) => x.NuGetPackages != null) != null && StiAdapterChecker.DownloadAdapterFromNugetWindow((StiDesignerControl)dictionary.Report.Designer, stiSqlDataConnector))
				{
					stiSqlDataConnector.ResetSettings();
					stiSqlDataConnector = database.CreateSqlConnector(database.ConnectionString);
				}
			}
			else
			{
				StiNotFoundPackageProvider stiNotFoundPackageProvider = new StiNotFoundPackageProvider();
				stiNotFoundPackageProvider.Show(stiSqlDataConnector.NuGetPackages.FirstOrDefault());
			}
		}
		return ShowSqlDatabaseEditForm(database, dictionary, newDatabase, stiSqlDataConnector.Name);
	}

	public async Task<DialogResult> SqlDatabaseEditAsync(StiSqlDatabase database, StiDictionary dictionary, bool newDatabase)
	{
		StiSqlDataConnector connector = database.CreateSqlConnector(database.ConnectionString);
		StiAdapterChecker stiAdapterChecker = new StiAdapterChecker(dictionary.Report.Designer, connector);
		stiAdapterChecker.AdapterFromNugetDownloaded += delegate
		{
			connector.ResetSettings();
			connector = database.CreateSqlConnector(database.ConnectionString);
		};
		await stiAdapterChecker.Run();
		return ShowSqlDatabaseEditForm(database, dictionary, newDatabase, connector.Name);
	}

	private DialogResult ShowSqlDatabaseEditForm(StiSqlDatabase database, StiDictionary dictionary, bool newDatabase, string connectorName)
	{
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Invalid comparison between Unknown and I4
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		StiSqlDatabaseEditForm stiSqlDatabaseEditForm = new StiSqlDatabaseEditForm(dictionary.Report, database, newDatabase ? null : database.Name);
		try
		{
			if (dictionary.Report.Designer != null && dictionary.Report.Designer.DesignerControl != null)
			{
				dictionary.Report.Designer.DesignerControl.AddOwnedForm((Form)(object)stiSqlDatabaseEditForm);
			}
			if (newDatabase)
			{
				((Control)stiSqlDatabaseEditForm).Text = string.Format(StiLocalization.Get("FormDatabaseEdit", "NewConnection"), connectorName);
			}
			else
			{
				((Control)stiSqlDatabaseEditForm).Text = string.Format(StiLocalization.Get("FormDatabaseEdit", "EditConnection"), connectorName);
			}
			((Control)stiSqlDatabaseEditForm.tbName).Text = database.Name;
			((Control)stiSqlDatabaseEditForm.tbAlias).Text = database.Alias;
			((Control)stiSqlDatabaseEditForm.tbConnectionString).Text = database.ConnectionString;
			((CheckBox)stiSqlDatabaseEditForm.checkBoxPromptUserNameAndPassword).Checked = database.PromptUserNameAndPassword;
			if (database is StiODataDatabase)
			{
				((CheckBox)stiSqlDatabaseEditForm.checkBoxODataV3).Checked = ((StiODataDatabase)database).Version == StiODataVersion.V3;
			}
			if ((int)((Form)stiSqlDatabaseEditForm).ShowDialog() == 1)
			{
				if (database.ConnectionString != ((Control)stiSqlDatabaseEditForm.tbConnectionString).Text)
				{
					StiCacheCleaner.Clean(dictionary);
				}
				database.Alias = ((Control)stiSqlDatabaseEditForm.tbAlias).Text;
				database.ConnectionString = ((Control)stiSqlDatabaseEditForm.tbConnectionString).Text;
				database.PromptUserNameAndPassword = ((CheckBox)stiSqlDatabaseEditForm.checkBoxPromptUserNameAndPassword).Checked;
				if (database is StiODataDatabase)
				{
					((StiODataDatabase)database).Version = ((!((CheckBox)stiSqlDatabaseEditForm.checkBoxODataV3).Checked) ? StiODataVersion.V4 : StiODataVersion.V3);
				}
				dictionary.RenameDatabase(database, ((Control)stiSqlDatabaseEditForm.tbName).Text);
				if (StiRecentConnections.RecentConnections != null)
				{
					StiRecentConnections.RecentConnections.AddAndSave(database);
					StiRecentConnections.RecentConnections = null;
				}
			}
			return ((Form)stiSqlDatabaseEditForm).DialogResult;
		}
		finally
		{
			((IDisposable)stiSqlDatabaseEditForm)?.Dispose();
		}
	}

	public DialogResult GoogleSheetsDatabaseEdit(StiGoogleSheetsDatabase database, StiDictionary dictionary, bool newDatabase)
	{
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		StiDataConnector stiDataConnector = database.CreateConnector();
		StiDataConnector[] familyConnectors = stiDataConnector.GetFamilyConnectors();
		if (!stiDataConnector.IsAvailable && familyConnectors != null)
		{
			if (StiAccountSettings.CurrentAccount != null)
			{
				if (familyConnectors.FirstOrDefault((StiDataConnector x) => x.NuGetPackages != null) != null && StiAdapterChecker.DownloadAdapterFromNugetWindow((StiDesignerControl)dictionary.Report.Designer, stiDataConnector))
				{
					stiDataConnector.ResetSettings();
					stiDataConnector = database.CreateConnector();
				}
			}
			else
			{
				StiNotFoundPackageProvider stiNotFoundPackageProvider = new StiNotFoundPackageProvider();
				stiNotFoundPackageProvider.Show(stiDataConnector.NuGetPackages.FirstOrDefault());
			}
		}
		return ShowGoogleSheetsDatabseEditForm(database, dictionary, newDatabase, stiDataConnector.Name);
	}

	public async Task<DialogResult> GoogleSheetsDatabaseEditAsync(StiGoogleSheetsDatabase database, StiDictionary dictionary, bool newDatabase)
	{
		StiDataConnector connector = database.CreateConnector();
		StiAdapterChecker stiAdapterChecker = new StiAdapterChecker(dictionary.Report.Designer, connector);
		stiAdapterChecker.AdapterFromNugetDownloaded += delegate
		{
			connector.ResetSettings();
			connector = database.CreateConnector();
		};
		await stiAdapterChecker.Run();
		return ShowGoogleSheetsDatabseEditForm(database, dictionary, newDatabase, connector.Name);
	}

	private DialogResult ShowGoogleSheetsDatabseEditForm(StiGoogleSheetsDatabase database, StiDictionary dictionary, bool newDatabase, string connectorName)
	{
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Invalid comparison between Unknown and I4
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		StiGoogleSheetsDatabaseEditForm stiGoogleSheetsDatabaseEditForm = new StiGoogleSheetsDatabaseEditForm(dictionary.Report, newDatabase ? null : database.Name);
		try
		{
			if (dictionary.Report.Designer != null && dictionary.Report.Designer.DesignerControl != null)
			{
				dictionary.Report.Designer.DesignerControl.AddOwnedForm((Form)(object)stiGoogleSheetsDatabaseEditForm);
			}
			((Control)stiGoogleSheetsDatabaseEditForm).Text = (newDatabase ? string.Format(StiLocalization.Get("FormDatabaseEdit", "NewConnection"), connectorName) : string.Format(StiLocalization.Get("FormDatabaseEdit", "EditConnection"), connectorName));
			((Control)stiGoogleSheetsDatabaseEditForm.tbName).Text = database.Name;
			((Control)stiGoogleSheetsDatabaseEditForm.tbAlias).Text = database.Alias;
			((Control)stiGoogleSheetsDatabaseEditForm.textBoxClientId).Text = database.ClientId;
			((Control)stiGoogleSheetsDatabaseEditForm.textBoxClientSecret).Text = database.ClientSecret;
			((Control)stiGoogleSheetsDatabaseEditForm.textBoxSpreadsheetId).Text = database.SpreadsheetId;
			if ((int)((Form)stiGoogleSheetsDatabaseEditForm).ShowDialog() == 1)
			{
				if (database.ClientId != ((Control)stiGoogleSheetsDatabaseEditForm.textBoxClientId).Text || database.ClientSecret != ((Control)stiGoogleSheetsDatabaseEditForm.textBoxClientSecret).Text || database.SpreadsheetId != ((Control)stiGoogleSheetsDatabaseEditForm.textBoxSpreadsheetId).Text)
				{
					StiCacheCleaner.Clean(dictionary);
				}
				database.Alias = ((Control)stiGoogleSheetsDatabaseEditForm.tbAlias).Text;
				database.ClientId = ((Control)stiGoogleSheetsDatabaseEditForm.textBoxClientId).Text;
				database.ClientSecret = ((Control)stiGoogleSheetsDatabaseEditForm.textBoxClientSecret).Text;
				database.SpreadsheetId = ((Control)stiGoogleSheetsDatabaseEditForm.textBoxSpreadsheetId).Text;
				dictionary.RenameDatabase(database, ((Control)stiGoogleSheetsDatabaseEditForm.tbName).Text);
				if (StiRecentConnections.RecentConnections != null)
				{
					StiRecentConnections.RecentConnections.AddAndSave(database);
					StiRecentConnections.RecentConnections = null;
				}
			}
			return ((Form)stiGoogleSheetsDatabaseEditForm).DialogResult;
		}
		finally
		{
			((IDisposable)stiGoogleSheetsDatabaseEditForm)?.Dispose();
		}
	}

	public DialogResult DataWorldDatabaseEdit(StiDataWorldDatabase database, StiDictionary dictionary, bool newDatabase)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		StiDataConnector stiDataConnector = database.CreateConnector();
		return ShowDataWorldDatabaseEdit(database, dictionary, newDatabase, stiDataConnector.Name);
	}

	public async Task<DialogResult> DataWorldDatabaseEditAsync(StiDataWorldDatabase database, StiDictionary dictionary, bool newDatabase)
	{
		StiDataConnector connector = database.CreateConnector();
		await Task.Delay(100);
		return ShowDataWorldDatabaseEdit(database, dictionary, newDatabase, connector.Name);
	}

	private DialogResult ShowDataWorldDatabaseEdit(StiDataWorldDatabase database, StiDictionary dictionary, bool newDatabase, string connectorName)
	{
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Invalid comparison between Unknown and I4
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Invalid comparison between Unknown and I4
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		StiDataWorldDatabaseEditForm stiDataWorldDatabaseEditForm = new StiDataWorldDatabaseEditForm(dictionary.Report, newDatabase ? null : database.Name);
		try
		{
			if (dictionary.Report.Designer != null && dictionary.Report.Designer.DesignerControl != null)
			{
				dictionary.Report.Designer.DesignerControl.AddOwnedForm((Form)(object)stiDataWorldDatabaseEditForm);
			}
			if (newDatabase)
			{
				((Control)stiDataWorldDatabaseEditForm).Text = string.Format(StiLocalization.Get("FormDatabaseEdit", "NewConnection"), connectorName);
			}
			else
			{
				((Control)stiDataWorldDatabaseEditForm).Text = string.Format(StiLocalization.Get("FormDatabaseEdit", "EditConnection"), connectorName);
			}
			((Control)stiDataWorldDatabaseEditForm.tbName).Text = database.Name;
			((Control)stiDataWorldDatabaseEditForm.tbAlias).Text = database.Alias;
			((Control)stiDataWorldDatabaseEditForm.textBoxOwner).Text = database.Owner;
			((Control)stiDataWorldDatabaseEditForm.textBoxDatabase).Text = database.Database;
			((Control)stiDataWorldDatabaseEditForm.textBoxToken).Text = database.Token;
			if ((int)((Form)stiDataWorldDatabaseEditForm).ShowDialog() == 1)
			{
				if (((CheckBox)stiDataWorldDatabaseEditForm.checkBoxOAuthv2).Checked)
				{
					StiDataWorldOAuthForm stiDataWorldOAuthForm = new StiDataWorldOAuthForm();
					if ((int)((Form)stiDataWorldOAuthForm).ShowDialog() == 1)
					{
						((Control)stiDataWorldDatabaseEditForm.textBoxToken).Text = stiDataWorldOAuthForm.Token;
					}
					else if (!string.IsNullOrEmpty(stiDataWorldOAuthForm.Error) && !string.IsNullOrEmpty(stiDataWorldOAuthForm.Message))
					{
						MessageBox.Show(stiDataWorldOAuthForm.Message, stiDataWorldOAuthForm.Error, (MessageBoxButtons)0, (MessageBoxIcon)48);
					}
				}
				if (database.Owner != ((Control)stiDataWorldDatabaseEditForm.textBoxOwner).Text || database.Database != ((Control)stiDataWorldDatabaseEditForm.textBoxDatabase).Text || database.Token != ((Control)stiDataWorldDatabaseEditForm.textBoxToken).Text)
				{
					StiCacheCleaner.Clean(dictionary);
				}
				string connectionString = "Owner=" + ((Control)stiDataWorldDatabaseEditForm.textBoxOwner).Text + ";Database=" + ((Control)stiDataWorldDatabaseEditForm.textBoxDatabase).Text + ";Token=" + ((Control)stiDataWorldDatabaseEditForm.textBoxToken).Text;
				database.ConnectionString = connectionString;
				database.Alias = ((Control)stiDataWorldDatabaseEditForm.tbAlias).Text;
				dictionary.RenameDatabase(database, ((Control)stiDataWorldDatabaseEditForm.tbName).Text);
				if (StiRecentConnections.RecentConnections != null)
				{
					StiRecentConnections.RecentConnections.AddAndSave(database);
					StiRecentConnections.RecentConnections = null;
				}
			}
			return ((Form)stiDataWorldDatabaseEditForm).DialogResult;
		}
		finally
		{
			((IDisposable)stiDataWorldDatabaseEditForm)?.Dispose();
		}
	}

	public DialogResult FirebaseDatabaseEdit(StiFirebaseDatabase database, StiDictionary dictionary, bool newDatabase)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		StiDataConnector stiDataConnector = database.CreateConnector();
		return ShowFirebaseDatabaseEdit(database, dictionary, newDatabase, stiDataConnector.Name);
	}

	public async Task<DialogResult> FirebaseDatabaseEditAsync(StiFirebaseDatabase database, StiDictionary dictionary, bool newDatabase)
	{
		StiDataConnector connector = database.CreateConnector();
		StiAdapterChecker stiAdapterChecker = new StiAdapterChecker(dictionary.Report.Designer, connector);
		stiAdapterChecker.AdapterFromNugetDownloaded += delegate
		{
			connector.ResetSettings();
			connector = database.CreateConnector(database.ConnectionString);
		};
		await stiAdapterChecker.Run();
		return ShowFirebaseDatabaseEdit(database, dictionary, newDatabase, connector.Name);
	}

	private DialogResult ShowFirebaseDatabaseEdit(StiFirebaseDatabase database, StiDictionary dictionary, bool newDatabase, string connectorName)
	{
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Invalid comparison between Unknown and I4
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		StiFirebaseDatabaseEditForm stiFirebaseDatabaseEditForm = new StiFirebaseDatabaseEditForm(dictionary.Report, newDatabase ? null : database.Name);
		try
		{
			if (dictionary.Report.Designer != null && dictionary.Report.Designer.DesignerControl != null)
			{
				dictionary.Report.Designer.DesignerControl.AddOwnedForm((Form)(object)stiFirebaseDatabaseEditForm);
			}
			if (newDatabase)
			{
				((Control)stiFirebaseDatabaseEditForm).Text = string.Format(StiLocalization.Get("FormDatabaseEdit", "NewConnection"), connectorName);
			}
			else
			{
				((Control)stiFirebaseDatabaseEditForm).Text = string.Format(StiLocalization.Get("FormDatabaseEdit", "EditConnection"), connectorName);
			}
			((Control)stiFirebaseDatabaseEditForm.tbName).Text = database.Name;
			((Control)stiFirebaseDatabaseEditForm.tbAlias).Text = database.Alias;
			((Control)stiFirebaseDatabaseEditForm.textBoxAuthSecret).Text = database.AuthSecret;
			((Control)stiFirebaseDatabaseEditForm.textBoxBasePath).Text = database.BasePath;
			if ((int)((Form)stiFirebaseDatabaseEditForm).ShowDialog() == 1)
			{
				if (database.AuthSecret != ((Control)stiFirebaseDatabaseEditForm.textBoxAuthSecret).Text || database.BasePath != ((Control)stiFirebaseDatabaseEditForm.textBoxBasePath).Text)
				{
					StiCacheCleaner.Clean(dictionary);
				}
				string connectionString = "AuthSecret=" + ((Control)stiFirebaseDatabaseEditForm.textBoxAuthSecret).Text + ";BasePath=" + ((Control)stiFirebaseDatabaseEditForm.textBoxBasePath).Text;
				database.ConnectionString = connectionString;
				database.Alias = ((Control)stiFirebaseDatabaseEditForm.tbAlias).Text;
				dictionary.RenameDatabase(database, ((Control)stiFirebaseDatabaseEditForm.tbName).Text);
				if (StiRecentConnections.RecentConnections != null)
				{
					StiRecentConnections.RecentConnections.AddAndSave(database);
					StiRecentConnections.RecentConnections = null;
				}
			}
			return ((Form)stiFirebaseDatabaseEditForm).DialogResult;
		}
		finally
		{
			((IDisposable)stiFirebaseDatabaseEditForm)?.Dispose();
		}
	}

	public DialogResult BigQueryDatabaseEdit(StiBigQueryDatabase database, StiDictionary dictionary, bool newDatabase)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		StiDataConnector stiDataConnector = database.CreateConnector();
		return ShowBigQueryDatabaseEdit(database, dictionary, newDatabase, stiDataConnector.Name);
	}

	public async Task<DialogResult> BigQueryDatabaseEditAsync(StiBigQueryDatabase database, StiDictionary dictionary, bool newDatabase)
	{
		StiDataConnector connector = database.CreateConnector();
		StiAdapterChecker stiAdapterChecker = new StiAdapterChecker(dictionary.Report.Designer, connector);
		stiAdapterChecker.AdapterFromNugetDownloaded += delegate
		{
			connector.ResetSettings();
			connector = database.CreateConnector(database.ConnectionString);
		};
		await stiAdapterChecker.Run();
		return ShowBigQueryDatabaseEdit(database, dictionary, newDatabase, connector.Name);
	}

	private DialogResult ShowBigQueryDatabaseEdit(StiBigQueryDatabase database, StiDictionary dictionary, bool newDatabase, string connectorName)
	{
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Invalid comparison between Unknown and I4
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		StiBigQueryDatabaseEditForm stiBigQueryDatabaseEditForm = new StiBigQueryDatabaseEditForm(dictionary.Report, newDatabase ? null : database.Name);
		try
		{
			if (dictionary.Report.Designer != null && dictionary.Report.Designer.DesignerControl != null)
			{
				dictionary.Report.Designer.DesignerControl.AddOwnedForm((Form)(object)stiBigQueryDatabaseEditForm);
			}
			if (newDatabase)
			{
				((Control)stiBigQueryDatabaseEditForm).Text = string.Format(StiLocalization.Get("FormDatabaseEdit", "NewConnection"), connectorName);
			}
			else
			{
				((Control)stiBigQueryDatabaseEditForm).Text = string.Format(StiLocalization.Get("FormDatabaseEdit", "EditConnection"), connectorName);
			}
			((Control)stiBigQueryDatabaseEditForm.tbName).Text = database.Name;
			((Control)stiBigQueryDatabaseEditForm.tbAlias).Text = database.Alias;
			stiBigQueryDatabaseEditForm.Base64EncodedAuthSecret = database.Base64EncodedAuthSecret;
			((Control)stiBigQueryDatabaseEditForm.tbProjectId).Text = database.ProjectId;
			((Control)stiBigQueryDatabaseEditForm.tbDatasetId).Text = database.DatasetId;
			if ((int)((Form)stiBigQueryDatabaseEditForm).ShowDialog() == 1)
			{
				if (database.Base64EncodedAuthSecret != stiBigQueryDatabaseEditForm.Base64EncodedAuthSecret || database.ProjectId != ((Control)stiBigQueryDatabaseEditForm.tbProjectId).Text || database.DatasetId != ((Control)stiBigQueryDatabaseEditForm.tbDatasetId).Text)
				{
					StiCacheCleaner.Clean(dictionary);
				}
				string connectionString = "Base64EncodedAuthSecret=" + stiBigQueryDatabaseEditForm.Base64EncodedAuthSecret + ";ProjectId=" + ((Control)stiBigQueryDatabaseEditForm.tbProjectId).Text + ";DatasetId=" + ((Control)stiBigQueryDatabaseEditForm.tbDatasetId).Text;
				database.ConnectionString = connectionString;
				database.Alias = ((Control)stiBigQueryDatabaseEditForm.tbAlias).Text;
				dictionary.RenameDatabase(database, ((Control)stiBigQueryDatabaseEditForm.tbName).Text);
				if (StiRecentConnections.RecentConnections != null)
				{
					StiRecentConnections.RecentConnections.AddAndSave(database);
					StiRecentConnections.RecentConnections = null;
				}
			}
			return ((Form)stiBigQueryDatabaseEditForm).DialogResult;
		}
		finally
		{
			((IDisposable)stiBigQueryDatabaseEditForm)?.Dispose();
		}
	}

	public DialogResult GoogleAnalyticsDatabaseEdit(StiGoogleAnalyticsDatabase database, StiDictionary dictionary, bool newDatabase)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		StiDataConnector stiDataConnector = database.CreateConnector();
		return ShowGoogleAnalyticsDatabaseEdit(database, dictionary, newDatabase, stiDataConnector.Name);
	}

	public async Task<DialogResult> GoogleAnalyticsDatabaseEditAsync(StiGoogleAnalyticsDatabase database, StiDictionary dictionary, bool newDatabase)
	{
		StiDataConnector connector = database.CreateConnector();
		StiAdapterChecker stiAdapterChecker = new StiAdapterChecker(dictionary.Report.Designer, connector);
		stiAdapterChecker.AdapterFromNugetDownloaded += delegate
		{
			connector.ResetSettings();
			connector = database.CreateConnector(database.ConnectionString);
		};
		await stiAdapterChecker.Run();
		return ShowGoogleAnalyticsDatabaseEdit(database, dictionary, newDatabase, connector.Name);
	}

	private DialogResult ShowGoogleAnalyticsDatabaseEdit(StiGoogleAnalyticsDatabase database, StiDictionary dictionary, bool newDatabase, string connectorName)
	{
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Invalid comparison between Unknown and I4
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		StiGoogleAnalyticsDatabaseEditForm stiGoogleAnalyticsDatabaseEditForm = new StiGoogleAnalyticsDatabaseEditForm(dictionary.Report, newDatabase ? null : database.Name);
		try
		{
			if (dictionary.Report.Designer != null && dictionary.Report.Designer.DesignerControl != null)
			{
				dictionary.Report.Designer.DesignerControl.AddOwnedForm((Form)(object)stiGoogleAnalyticsDatabaseEditForm);
			}
			if (newDatabase)
			{
				((Control)stiGoogleAnalyticsDatabaseEditForm).Text = string.Format(StiLocalization.Get("FormDatabaseEdit", "NewConnection"), connectorName);
			}
			else
			{
				((Control)stiGoogleAnalyticsDatabaseEditForm).Text = string.Format(StiLocalization.Get("FormDatabaseEdit", "EditConnection"), connectorName);
			}
			((Control)stiGoogleAnalyticsDatabaseEditForm.tbName).Text = database.Name;
			((Control)stiGoogleAnalyticsDatabaseEditForm.tbAlias).Text = database.Alias;
			stiGoogleAnalyticsDatabaseEditForm.Base64EncodedAuthSecret = database.Base64EncodedAuthSecret;
			stiGoogleAnalyticsDatabaseEditForm.AccountId = database.AccountId;
			stiGoogleAnalyticsDatabaseEditForm.PropertyId = database.PropertyId;
			stiGoogleAnalyticsDatabaseEditForm.ViewId = database.ViewId;
			stiGoogleAnalyticsDatabaseEditForm.Metrics = database.Metrics;
			stiGoogleAnalyticsDatabaseEditForm.Dimensions = database.Dimensions;
			stiGoogleAnalyticsDatabaseEditForm.StartDate = database.StartDate;
			stiGoogleAnalyticsDatabaseEditForm.EndDate = database.EndDate;
			if ((int)((Form)stiGoogleAnalyticsDatabaseEditForm).ShowDialog() == 1)
			{
				if (database.Base64EncodedAuthSecret != stiGoogleAnalyticsDatabaseEditForm.Base64EncodedAuthSecret || database.AccountId != stiGoogleAnalyticsDatabaseEditForm.AccountId || database.PropertyId != stiGoogleAnalyticsDatabaseEditForm.PropertyId || database.ViewId != stiGoogleAnalyticsDatabaseEditForm.ViewId || database.Metrics != stiGoogleAnalyticsDatabaseEditForm.Metrics || database.Dimensions != stiGoogleAnalyticsDatabaseEditForm.Dimensions || database.StartDate != stiGoogleAnalyticsDatabaseEditForm.StartDate || database.EndDate != stiGoogleAnalyticsDatabaseEditForm.EndDate)
				{
					StiCacheCleaner.Clean(dictionary);
				}
				database.ConnectionString = stiGoogleAnalyticsDatabaseEditForm.ConnectionString;
				database.Alias = ((Control)stiGoogleAnalyticsDatabaseEditForm.tbAlias).Text;
				dictionary.RenameDatabase(database, ((Control)stiGoogleAnalyticsDatabaseEditForm.tbName).Text);
				if (StiRecentConnections.RecentConnections != null)
				{
					StiRecentConnections.RecentConnections.AddAndSave(database);
					StiRecentConnections.RecentConnections = null;
				}
			}
			return ((Form)stiGoogleAnalyticsDatabaseEditForm).DialogResult;
		}
		finally
		{
			((IDisposable)stiGoogleAnalyticsDatabaseEditForm)?.Dispose();
		}
	}

	public DialogResult AzureBlobStorageDatabaseEdit(StiAzureBlobStorageDatabase database, StiDictionary dictionary, bool newDatabase)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		StiDataConnector stiDataConnector = database.CreateConnector();
		return ShowAzureBlobStorageDatabaseEdit(database, dictionary, newDatabase, stiDataConnector.Name);
	}

	public async Task<DialogResult> AzureBlobStorageDatabaseEditAsync(StiAzureBlobStorageDatabase database, StiDictionary dictionary, bool newDatabase)
	{
		StiDataConnector connector = database.CreateConnector();
		StiAdapterChecker stiAdapterChecker = new StiAdapterChecker(dictionary.Report.Designer, connector);
		stiAdapterChecker.AdapterFromNugetDownloaded += delegate
		{
			connector.ResetSettings();
			connector = database.CreateConnector(database.ConnectionString);
		};
		await stiAdapterChecker.Run();
		return ShowAzureBlobStorageDatabaseEdit(database, dictionary, newDatabase, connector.Name);
	}

	private DialogResult ShowAzureBlobStorageDatabaseEdit(StiAzureBlobStorageDatabase database, StiDictionary dictionary, bool newDatabase, string connectorName)
	{
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Invalid comparison between Unknown and I4
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		StiAzureBlobStorageDatabaseEditForm stiAzureBlobStorageDatabaseEditForm = new StiAzureBlobStorageDatabaseEditForm(dictionary.Report, newDatabase ? null : database.Name);
		try
		{
			if (dictionary.Report.Designer != null && dictionary.Report.Designer.DesignerControl != null)
			{
				dictionary.Report.Designer.DesignerControl.AddOwnedForm((Form)(object)stiAzureBlobStorageDatabaseEditForm);
			}
			if (newDatabase)
			{
				((Control)stiAzureBlobStorageDatabaseEditForm).Text = string.Format(StiLocalization.Get("FormDatabaseEdit", "NewConnection"), connectorName);
			}
			else
			{
				((Control)stiAzureBlobStorageDatabaseEditForm).Text = string.Format(StiLocalization.Get("FormDatabaseEdit", "EditConnection"), connectorName);
			}
			((Control)stiAzureBlobStorageDatabaseEditForm.tbName).Text = database.Name;
			((Control)stiAzureBlobStorageDatabaseEditForm.tbAlias).Text = database.Alias;
			stiAzureBlobStorageDatabaseEditForm.AccountKey = database.AccountKey;
			stiAzureBlobStorageDatabaseEditForm.AccountName = database.AccountName;
			stiAzureBlobStorageDatabaseEditForm.ContainerName = database.ContainerName;
			stiAzureBlobStorageDatabaseEditForm.BlobName = database.BlobName;
			stiAzureBlobStorageDatabaseEditForm.BlobContentType = database.BlobContentType;
			stiAzureBlobStorageDatabaseEditForm.CodePage = database.CodePage;
			stiAzureBlobStorageDatabaseEditForm.Delimiter = database.Delimiter;
			stiAzureBlobStorageDatabaseEditForm.FirstRowIsHeader = database.FirstRowIsHeader;
			if ((int)((Form)stiAzureBlobStorageDatabaseEditForm).ShowDialog() == 1)
			{
				if (database.AccountKey != stiAzureBlobStorageDatabaseEditForm.AccountKey || database.AccountName != stiAzureBlobStorageDatabaseEditForm.AccountName || database.ContainerName != stiAzureBlobStorageDatabaseEditForm.ContainerName || database.BlobName != stiAzureBlobStorageDatabaseEditForm.BlobName || database.BlobContentType != stiAzureBlobStorageDatabaseEditForm.BlobContentType || stiAzureBlobStorageDatabaseEditForm.CodePage != database.CodePage || stiAzureBlobStorageDatabaseEditForm.Delimiter != database.Delimiter || stiAzureBlobStorageDatabaseEditForm.FirstRowIsHeader != database.FirstRowIsHeader)
				{
					StiCacheCleaner.Clean(dictionary);
				}
				database.ConnectionString = stiAzureBlobStorageDatabaseEditForm.ConnectionString;
				database.Alias = ((Control)stiAzureBlobStorageDatabaseEditForm.tbAlias).Text;
				dictionary.RenameDatabase(database, ((Control)stiAzureBlobStorageDatabaseEditForm.tbName).Text);
				if (StiRecentConnections.RecentConnections != null)
				{
					StiRecentConnections.RecentConnections.AddAndSave(database);
					StiRecentConnections.RecentConnections = null;
				}
			}
			return ((Form)stiAzureBlobStorageDatabaseEditForm).DialogResult;
		}
		finally
		{
			((IDisposable)stiAzureBlobStorageDatabaseEditForm)?.Dispose();
		}
	}

	public DialogResult QuickBooksDatabaseEdit(StiQuickBooksDatabase database, StiDictionary dictionary, bool newDatabase)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		StiQuickBooksConnector connector = database.CreateConnector() as StiQuickBooksConnector;
		return ShowQuickBooksDatabaseEdit(database, dictionary, newDatabase, connector);
	}

	public async Task<DialogResult> QuickBooksDatabaseEditAsync(StiQuickBooksDatabase database, StiDictionary dictionary, bool newDatabase)
	{
		StiQuickBooksConnector connector = database.CreateConnector() as StiQuickBooksConnector;
		await Task.Delay(100);
		return ShowQuickBooksDatabaseEdit(database, dictionary, newDatabase, connector);
	}

	private DialogResult ShowQuickBooksDatabaseEdit(StiQuickBooksDatabase database, StiDictionary dictionary, bool newDatabase, StiQuickBooksConnector connector)
	{
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Invalid comparison between Unknown and I4
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		StiQuickBooksDatabaseEditForm stiQuickBooksDatabaseEditForm = new StiQuickBooksDatabaseEditForm(dictionary.Report, newDatabase ? null : database.Name, connector);
		try
		{
			if (dictionary.Report.Designer != null && dictionary.Report.Designer.DesignerControl != null)
			{
				dictionary.Report.Designer.DesignerControl.AddOwnedForm((Form)(object)stiQuickBooksDatabaseEditForm);
			}
			if (newDatabase)
			{
				((Control)stiQuickBooksDatabaseEditForm).Text = string.Format(StiLocalization.Get("FormDatabaseEdit", "NewConnection"), connector.Name);
			}
			else
			{
				((Control)stiQuickBooksDatabaseEditForm).Text = string.Format(StiLocalization.Get("FormDatabaseEdit", "EditConnection"), connector.Name);
			}
			((Control)stiQuickBooksDatabaseEditForm.tbName).Text = database.Name;
			((Control)stiQuickBooksDatabaseEditForm.tbAlias).Text = database.Alias;
			if ((int)((Form)stiQuickBooksDatabaseEditForm).ShowDialog() == 1)
			{
				database.ConnectionString = connector.ConnectionString;
				database.Alias = ((Control)stiQuickBooksDatabaseEditForm.tbAlias).Text;
				dictionary.RenameDatabase(database, ((Control)stiQuickBooksDatabaseEditForm.tbName).Text);
				if (StiRecentConnections.RecentConnections != null)
				{
					StiRecentConnections.RecentConnections.AddAndSave(database);
					StiRecentConnections.RecentConnections = null;
				}
			}
			return ((Form)stiQuickBooksDatabaseEditForm).DialogResult;
		}
		finally
		{
			((IDisposable)stiQuickBooksDatabaseEditForm)?.Dispose();
		}
	}

	public DialogResult NoSqlDatabaseEdit(StiNoSqlDatabase database, StiDictionary dictionary, bool newDatabase)
	{
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		StiNoSqlDataConnector stiNoSqlDataConnector = database.CreateNoSqlConnector(database.ConnectionString);
		StiDataConnector[] familyConnectors = stiNoSqlDataConnector.GetFamilyConnectors();
		if (!stiNoSqlDataConnector.IsAvailable && familyConnectors != null)
		{
			if (StiAccountSettings.CurrentAccount != null)
			{
				if (familyConnectors.FirstOrDefault((StiDataConnector x) => x.NuGetPackages != null) != null && StiAdapterChecker.DownloadAdapterFromNugetWindow((StiDesignerControl)dictionary.Report.Designer.DesignerControl, stiNoSqlDataConnector))
				{
					stiNoSqlDataConnector.ResetSettings();
					stiNoSqlDataConnector = database.CreateNoSqlConnector(database.ConnectionString);
				}
			}
			else
			{
				StiNotFoundPackageProvider stiNotFoundPackageProvider = new StiNotFoundPackageProvider();
				stiNotFoundPackageProvider.Show(stiNoSqlDataConnector.NuGetPackages.FirstOrDefault());
			}
		}
		return ShowNoSqlDatabaseEditFor(database, dictionary, newDatabase, stiNoSqlDataConnector.Name);
	}

	public async Task<DialogResult> NoSqlDatabaseEditAsync(StiNoSqlDatabase database, StiDictionary dictionary, bool newDatabase)
	{
		StiNoSqlDataConnector connector = database.CreateNoSqlConnector(database.ConnectionString);
		StiAdapterChecker stiAdapterChecker = new StiAdapterChecker(dictionary.Report.Designer, connector);
		stiAdapterChecker.AdapterFromNugetDownloaded += delegate
		{
			connector.ResetSettings();
			connector = database.CreateNoSqlConnector(database.ConnectionString);
		};
		await stiAdapterChecker.Run();
		return ShowNoSqlDatabaseEditFor(database, dictionary, newDatabase, connector.Name);
	}

	private DialogResult ShowNoSqlDatabaseEditFor(StiNoSqlDatabase database, StiDictionary dictionary, bool newDatabase, string connectorName)
	{
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Invalid comparison between Unknown and I4
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		StiNoSqlDatabaseEditForm stiNoSqlDatabaseEditForm = new StiNoSqlDatabaseEditForm(database);
		try
		{
			if (dictionary != null && dictionary.Report.Designer != null && dictionary.Report.Designer.DesignerControl != null)
			{
				dictionary.Report.Designer.DesignerControl.AddOwnedForm((Form)(object)stiNoSqlDatabaseEditForm);
			}
			if (newDatabase)
			{
				((Control)stiNoSqlDatabaseEditForm).Text = string.Format(StiLocalization.Get("FormDatabaseEdit", "NewConnection"), connectorName);
			}
			else
			{
				((Control)stiNoSqlDatabaseEditForm).Text = string.Format(StiLocalization.Get("FormDatabaseEdit", "EditConnection"), connectorName);
			}
			((Control)stiNoSqlDatabaseEditForm.tbName).Text = database.Name;
			((Control)stiNoSqlDatabaseEditForm.tbAlias).Text = database.Alias;
			stiNoSqlDatabaseEditForm.ConnectionString = database.ConnectionString;
			if ((int)((Form)stiNoSqlDatabaseEditForm).ShowDialog() == 1)
			{
				if (database.ConnectionString != stiNoSqlDatabaseEditForm.ConnectionString)
				{
					StiCacheCleaner.Clean(dictionary);
				}
				database.Alias = ((Control)stiNoSqlDatabaseEditForm.tbAlias).Text;
				database.ConnectionString = stiNoSqlDatabaseEditForm.ConnectionString;
				database.PromptUserNameAndPassword = ((CheckBox)stiNoSqlDatabaseEditForm.checkBoxPromptUserNameAndPassword).Checked;
				if (dictionary == null)
				{
					database.Name = ((Control)stiNoSqlDatabaseEditForm.tbName).Text;
				}
				else
				{
					dictionary.RenameDatabase(database, ((Control)stiNoSqlDatabaseEditForm.tbName).Text);
				}
				if (StiRecentConnections.RecentConnections != null)
				{
					StiRecentConnections.RecentConnections.AddAndSave(database);
					StiRecentConnections.RecentConnections = null;
				}
			}
			return ((Form)stiNoSqlDatabaseEditForm).DialogResult;
		}
		finally
		{
			((IDisposable)stiNoSqlDatabaseEditForm)?.Dispose();
		}
	}

	public DialogResult XmlDatabaseEdit(StiXmlDatabase database, StiDictionary dictionary, bool newDatabase)
	{
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Invalid comparison between Unknown and I4
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		StiXmlDatabaseEditForm stiXmlDatabaseEditForm = new StiXmlDatabaseEditForm(dictionary.Report, newDatabase ? null : database.Name);
		try
		{
			if (dictionary.Report.Designer != null && dictionary.Report.Designer.DesignerControl != null)
			{
				dictionary.Report.Designer.DesignerControl.AddOwnedForm((Form)(object)stiXmlDatabaseEditForm);
			}
			if (newDatabase)
			{
				((Control)stiXmlDatabaseEditForm).Text = StiLocalization.Get("FormDatabaseEdit", "XmlNew");
			}
			else
			{
				((Control)stiXmlDatabaseEditForm).Text = StiLocalization.Get("FormDatabaseEdit", "XmlEdit");
			}
			stiXmlDatabaseEditForm.DataName = database.Name;
			stiXmlDatabaseEditForm.DataAlias = database.Alias;
			stiXmlDatabaseEditForm.FilePathSchema = database.PathSchema;
			stiXmlDatabaseEditForm.FilePathData = database.PathData;
			stiXmlDatabaseEditForm.DataXmlType = database.XmlType;
			stiXmlDatabaseEditForm.RelationDirection = database.RelationDirection;
			if ((int)((Form)stiXmlDatabaseEditForm).ShowDialog() == 1)
			{
				if (database.PathSchema != stiXmlDatabaseEditForm.FilePathSchema || database.PathData != stiXmlDatabaseEditForm.FilePathData)
				{
					StiCacheCleaner.Clean(dictionary);
				}
				database.Alias = stiXmlDatabaseEditForm.DataAlias;
				database.PathSchema = stiXmlDatabaseEditForm.FilePathSchema;
				database.PathData = stiXmlDatabaseEditForm.FilePathData;
				database.XmlType = stiXmlDatabaseEditForm.DataXmlType;
				database.RelationDirection = stiXmlDatabaseEditForm.RelationDirection;
				dictionary.RenameDatabase(database, stiXmlDatabaseEditForm.DataName);
				if (StiRecentConnections.RecentConnections != null)
				{
					StiRecentConnections.RecentConnections.AddAndSave(database);
					StiRecentConnections.RecentConnections = null;
				}
			}
			return ((Form)stiXmlDatabaseEditForm).DialogResult;
		}
		finally
		{
			((IDisposable)stiXmlDatabaseEditForm)?.Dispose();
		}
	}

	public StiUserNameAndPassword PromptUserNameAndPassword()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		StiPromptForm stiPromptForm = new StiPromptForm();
		try
		{
			if ((int)((Form)stiPromptForm).ShowDialog() == 1)
			{
				return new StiUserNameAndPassword(stiPromptForm.UserName, stiPromptForm.Password, stiPromptForm.CacheData);
			}
		}
		finally
		{
			((IDisposable)stiPromptForm)?.Dispose();
		}
		return null;
	}

	public bool DataStoreAdapterEdit(StiDataAdapterService adapter, StiDictionary dictionary, StiDataSource dataSource)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Invalid comparison between Unknown and I4
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Invalid comparison between Unknown and I4
		StiDataStoreSourceEditForm stiDataStoreSourceEditForm = new StiDataStoreSourceEditForm(dictionary, adapter, dataSource as StiDataStoreSource, newForm: false);
		try
		{
			if (dictionary != null && dictionary.Report != null && dictionary.Report.Designer != null)
			{
				dictionary.Report.Designer.AddOwnedForm((Form)(object)stiDataStoreSourceEditForm);
			}
			((Control)stiDataStoreSourceEditForm).Text = StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "DataSourceEdit"));
			if ((int)((Form)stiDataStoreSourceEditForm).ShowDialog() == 1)
			{
				StiCacheCleaner.Clean(dictionary);
			}
			return (int)((Form)stiDataStoreSourceEditForm).DialogResult == 1;
		}
		finally
		{
			((IDisposable)stiDataStoreSourceEditForm)?.Dispose();
		}
	}

	public bool DataStoreAdapterNew(StiDataAdapterService adapter, StiDictionary dictionary, StiDataSource dataSource)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Invalid comparison between Unknown and I4
		StiDataStoreSourceEditForm stiDataStoreSourceEditForm = new StiDataStoreSourceEditForm(dictionary, adapter, dataSource as StiDataStoreSource, newForm: true);
		try
		{
			if (dictionary != null && dictionary.Report != null && dictionary.Report.Designer != null)
			{
				dictionary.Report.Designer.AddOwnedForm((Form)(object)stiDataStoreSourceEditForm);
			}
			((Control)stiDataStoreSourceEditForm).Text = StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "DataSourceNew"));
			((Form)stiDataStoreSourceEditForm).ShowDialog();
			return (int)((Form)stiDataStoreSourceEditForm).DialogResult == 1;
		}
		finally
		{
			((IDisposable)stiDataStoreSourceEditForm)?.Dispose();
		}
	}

	public bool VirtualAdapterEdit(StiVirtualAdapterService adapter, StiDictionary dictionary, StiDataSource dataSource)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Invalid comparison between Unknown and I4
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Invalid comparison between Unknown and I4
		StiVirtualSourceEditForm stiVirtualSourceEditForm = new StiVirtualSourceEditForm(dictionary, dataSource as StiVirtualSource, dataSource.Name);
		try
		{
			if (dictionary != null && dictionary.Report != null && dictionary.Report.Designer != null)
			{
				dictionary.Report.Designer.AddOwnedForm((Form)(object)stiVirtualSourceEditForm);
			}
			((Control)stiVirtualSourceEditForm).Text = StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "DataSourceEdit"));
			if ((int)((Form)stiVirtualSourceEditForm).ShowDialog() == 1)
			{
				StiCacheCleaner.Clean(dictionary);
			}
			return (int)((Form)stiVirtualSourceEditForm).DialogResult == 1;
		}
		finally
		{
			((IDisposable)stiVirtualSourceEditForm)?.Dispose();
		}
	}

	public bool VirtualAdapterNew(StiVirtualAdapterService adapter, StiDictionary dictionary, StiDataSource dataSource)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Invalid comparison between Unknown and I4
		StiVirtualSourceEditForm stiVirtualSourceEditForm = new StiVirtualSourceEditForm(dictionary, dataSource as StiVirtualSource, null);
		try
		{
			if (dictionary != null && dictionary.Report != null && dictionary.Report.Designer != null)
			{
				dictionary.Report.Designer.AddOwnedForm((Form)(object)stiVirtualSourceEditForm);
			}
			((Control)stiVirtualSourceEditForm).Text = StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "DataSourceNew"));
			((Form)stiVirtualSourceEditForm).ShowDialog();
			return (int)((Form)stiVirtualSourceEditForm).DialogResult == 1;
		}
		finally
		{
			((IDisposable)stiVirtualSourceEditForm)?.Dispose();
		}
	}

	public bool CrossTabAdapterNew(StiCrossTabAdapterService adapter, StiDictionary dictionary, StiDataSource dataSource)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Invalid comparison between Unknown and I4
		StiCrossTabDataSourceEditForm stiCrossTabDataSourceEditForm = new StiCrossTabDataSourceEditForm(adapter, dictionary, dataSource as StiCrossTabDataSource, newForm: true);
		try
		{
			if (dictionary != null && dictionary.Report != null && dictionary.Report.Designer != null)
			{
				dictionary.Report.Designer.AddOwnedForm((Form)(object)stiCrossTabDataSourceEditForm);
			}
			((Control)stiCrossTabDataSourceEditForm).Text = StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "DataSourceNew"));
			((Form)stiCrossTabDataSourceEditForm).ShowDialog();
			return (int)((Form)stiCrossTabDataSourceEditForm).DialogResult == 1;
		}
		finally
		{
			((IDisposable)stiCrossTabDataSourceEditForm)?.Dispose();
		}
	}

	public bool CrossTabAdapterEdit(StiCrossTabAdapterService adapter, StiDictionary dictionary, StiDataSource dataSource)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Invalid comparison between Unknown and I4
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Invalid comparison between Unknown and I4
		StiCrossTabDataSourceEditForm stiCrossTabDataSourceEditForm = new StiCrossTabDataSourceEditForm(adapter, dictionary, dataSource as StiCrossTabDataSource, newForm: false);
		try
		{
			if (dictionary != null && dictionary.Report != null && dictionary.Report.Designer != null)
			{
				dictionary.Report.Designer.AddOwnedForm((Form)(object)stiCrossTabDataSourceEditForm);
			}
			((Control)stiCrossTabDataSourceEditForm).Text = StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "DataSourceEdit"));
			if ((int)((Form)stiCrossTabDataSourceEditForm).ShowDialog() == 1)
			{
				StiCacheCleaner.Clean(dictionary);
			}
			return (int)((Form)stiCrossTabDataSourceEditForm).DialogResult == 1;
		}
		finally
		{
			((IDisposable)stiCrossTabDataSourceEditForm)?.Dispose();
		}
	}
}
