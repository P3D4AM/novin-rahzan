using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Server;
using Stimulsoft.Report.Design.Panels;

namespace Stimulsoft.Report.Design.Dictionary;

public class StiAdapterChecker
{
	private readonly StiDesignerControl designer;

	private readonly StiDataConnector connector;

	public event EventHandler AdapterFromNugetDownloaded;

	public void InvokeAdapterFromNugetDownloaded()
	{
		if (this.AdapterFromNugetDownloaded != null)
		{
			this.AdapterFromNugetDownloaded(null, EventArgs.Empty);
		}
	}

	public async Task Run()
	{
		StiDataConnector[] familyConnectors = connector.GetFamilyConnectors();
		if (connector.IsAvailable || familyConnectors == null)
		{
			return;
		}
		if (StiAccountSettings.CurrentAccount != null)
		{
			StiDataConnector stiDataConnector = familyConnectors.FirstOrDefault((StiDataConnector x) => x.NuGetPackages != null);
			if (stiDataConnector != null)
			{
				if (StiOptions.Designer.IsBeginnerDesigner || StiOptions.Designer.IsBICreatorDesigner || familyConnectors.Length == 1)
				{
					await DownloadAdapter();
				}
				else if (DownloadAdapterFromNugetWindow())
				{
					InvokeAdapterFromNugetDownloaded();
				}
			}
		}
		else
		{
			StiNotFoundPackageProvider stiNotFoundPackageProvider = new StiNotFoundPackageProvider();
			stiNotFoundPackageProvider.Show(connector.NuGetPackages.FirstOrDefault());
		}
	}

	public async Task DownloadAdapter()
	{
		StiDictionaryPanelHelper.ShowProgress(designer, Loc.Get("NuGet", "DownloadDataAdapter"));
		IStiNuGetDownloader nuGetDownloader = StiAccountSettings.CurrentAccount.AccountCreater.GetNuGetDownloader();
		await nuGetDownloader.RunDownloaderAsync(connector, ((Control)designer).Handle);
		StiDictionaryPanelHelper.HideProgress(designer);
	}

	public bool DownloadAdapterFromNugetWindow()
	{
		return DownloadAdapterFromNugetWindow(designer, connector);
	}

	public static bool DownloadAdapterFromNugetWindow(StiDesignerControl designer, StiDataConnector connector)
	{
		IStiNuGetWindow nuGetWindow = StiAccountSettings.CurrentAccount.AccountCreater.GetNuGetWindow(connector);
		if (designer != null)
		{
			_ = ((Control)designer).Handle;
			nuGetWindow.ShowDialog(((Control)designer).Handle);
		}
		else
		{
			nuGetWindow.ShowDialog();
		}
		return nuGetWindow.IsDownloaded;
	}

	public StiAdapterChecker(object designer, StiDataConnector connector)
	{
		if (designer is StiDesignerControl stiDesignerControl)
		{
			this.designer = stiDesignerControl;
		}
		if (designer is StiRibbonDesigner stiRibbonDesigner)
		{
			this.designer = stiRibbonDesigner.DesignerControl as StiDesignerControl;
		}
		this.connector = connector;
	}
}
