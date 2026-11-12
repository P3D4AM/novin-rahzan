using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Server;
using Stimulsoft.Report.Design.Dictionary;
using Stimulsoft.Report.Design.Panels;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Dictionary.Databases.Google;

namespace Stimulsoft.Report.Design.Helpers;

public static class StiAdapterHelper
{
	public static async void CheckDownloadAdapter(StiReport report, StiDesignerControl designerControl)
	{
		foreach (object database2 in report.Dictionary.Databases)
		{
			if (!(database2 is StiDatabase database))
			{
				continue;
			}
			StiDataConnector connector = GetConnector(database);
			StiDataConnector[] array = connector?.GetFamilyConnectors();
			if (connector == null || array == null || connector.IsAvailable)
			{
				continue;
			}
			if (StiAccountSettings.CurrentAccount != null)
			{
				if (array.FirstOrDefault((StiDataConnector x) => x.NuGetPackages != null) != null)
				{
					await DownloadAdapterAsync(designerControl, connector);
				}
			}
			else
			{
				StiNotFoundPackageProvider stiNotFoundPackageProvider = new StiNotFoundPackageProvider();
				stiNotFoundPackageProvider.Show(connector.NuGetPackages.FirstOrDefault());
			}
		}
	}

	private static StiDataConnector GetConnector(StiDatabase database)
	{
		StiDataConnector result = null;
		if (database is StiSqlDatabase)
		{
			StiSqlDatabase stiSqlDatabase = database as StiSqlDatabase;
			result = stiSqlDatabase.CreateSqlConnector(stiSqlDatabase.ConnectionString);
		}
		else if (database is StiGoogleSheetsDatabase)
		{
			StiGoogleSheetsDatabase stiGoogleSheetsDatabase = database as StiGoogleSheetsDatabase;
			result = stiGoogleSheetsDatabase.CreateConnector();
		}
		else if (database is StiNoSqlDatabase)
		{
			StiNoSqlDatabase stiNoSqlDatabase = database as StiNoSqlDatabase;
			result = stiNoSqlDatabase.CreateNoSqlConnector(stiNoSqlDatabase.ConnectionString);
		}
		return result;
	}

	private static async Task DownloadAdapterAsync(StiDesignerControl designer, StiDataConnector connector)
	{
		string processGuid = StiGuidUtils.NewGuid();
		try
		{
			StiDictionaryPanelHelper.ShowProgress(designer, Loc.Get("NuGet", "DownloadDataAdapter"), useTimer: true, processGuid);
			IStiNuGetDownloader nuGetDownloader = StiAccountSettings.CurrentAccount.AccountCreater.GetNuGetDownloader();
			await nuGetDownloader.RunDownloaderAsync(connector, ((Control)designer).Handle);
		}
		finally
		{
			StiDictionaryPanelHelper.HideProgress(designer, processGuid);
		}
	}
}
