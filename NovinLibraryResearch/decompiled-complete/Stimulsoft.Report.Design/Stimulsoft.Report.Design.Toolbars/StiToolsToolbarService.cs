using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Report.Design.Toolbars;

public class StiToolsToolbarService : StiToolbarService
{
	private ButtonItem buttonDataStore;

	private ButtonItem buttonPagesManager;

	private ButtonItem buttonServicesConfigurator;

	public override string ServiceName => StiLocalization.Get("Toolbars", "ToolbarTools");

	public override int Position => 8;

	public override int DefaultDockLine => 3;

	public override bool DefaultVisible => false;

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public virtual bool ShowModifiers
	{
		get
		{
			return StiOptions.Designer.Toolbars.ToolsToolbar.ShowModifiers;
		}
		set
		{
			StiOptions.Designer.Toolbars.ToolsToolbar.ShowModifiers = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public virtual bool ShowPagesManager
	{
		get
		{
			return StiOptions.Designer.Toolbars.ToolsToolbar.ShowPagesManager;
		}
		set
		{
			StiOptions.Designer.Toolbars.ToolsToolbar.ShowPagesManager = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public virtual bool ShowDataStore
	{
		get
		{
			return StiOptions.Designer.Toolbars.ToolsToolbar.ShowDataStore;
		}
		set
		{
			StiOptions.Designer.Toolbars.ToolsToolbar.ShowDataStore = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public virtual bool ShowConfigurator
	{
		get
		{
			return StiOptions.Designer.Toolbars.ToolsToolbar.ShowConfigurator;
		}
		set
		{
			StiOptions.Designer.Toolbars.ToolsToolbar.ShowConfigurator = value;
		}
	}

	private void DesignerRefresh_Refresh(object sender, EventArgs e)
	{
		if (base.ToolBar.Visible)
		{
			base.ToolBar.SuspendLayout();
			if (buttonDataStore != null)
			{
				buttonDataStore.Enabled = Report != null;
			}
			if (buttonPagesManager != null)
			{
				buttonPagesManager.Enabled = Report != null;
			}
			if (buttonServicesConfigurator != null)
			{
				buttonServicesConfigurator.Enabled = Report != null;
			}
			base.ToolBar.ResumeLayout();
		}
	}

	private void ButtonDataStore_Click(object sender, EventArgs e)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		StiMainMenuService service = StiMainMenuService.GetService(Designer);
		if (service == null)
		{
			string text = string.Format(StiLocalization.Get("Errors", "ServiceNotFound"), "StiMainMenuService");
			StiLogService.Write(GetType(), text);
			MessageBox.Show(text);
		}
		else
		{
			service.InvokeDataStore();
		}
	}

	private void ButtonPagesManager_Click(object sender, EventArgs e)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		StiMainMenuService service = StiMainMenuService.GetService(Designer);
		if (service == null)
		{
			string text = string.Format(StiLocalization.Get("Errors", "ServiceNotFound"), "StiMainMenuService");
			StiLogService.Write(GetType(), text);
			MessageBox.Show(text);
		}
		else
		{
			service.InvokePagesManager();
		}
	}

	private void ButtonServicesConfigurator_Click(object sender, EventArgs e)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		StiMainMenuService service = StiMainMenuService.GetService(Designer);
		if (service == null)
		{
			string text = string.Format(StiLocalization.Get("Errors", "ServiceNotFound"), "StiMainMenuService");
			StiLogService.Write(GetType(), text);
			MessageBox.Show(text);
		}
		else
		{
			service.InvokeServicesConfigurator();
		}
	}

	private void LocalizeSuperToolTip()
	{
		if (Designer == null)
		{
			return;
		}
		if (StiOptions.Designer.ShowSuperToolTip)
		{
			Graphics globalMeasureGraphics = StiReport.GlobalMeasureGraphics;
			if (buttonDataStore != null)
			{
				SetSuperToolTip(buttonDataStore, StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuToolsDataStore")), "DataStore", globalMeasureGraphics);
			}
			if (buttonPagesManager != null)
			{
				SetSuperToolTip(buttonPagesManager, StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuToolsPagesManager")), "PagesManager", globalMeasureGraphics);
			}
			if (buttonServicesConfigurator != null)
			{
				SetSuperToolTip(buttonServicesConfigurator, StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuToolsServicesConfigurator")), "ServicesConfigurator", globalMeasureGraphics);
			}
		}
		else
		{
			if (buttonDataStore != null)
			{
				buttonDataStore.Tooltip = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuToolsDataStore"));
			}
			if (buttonPagesManager != null)
			{
				buttonPagesManager.Tooltip = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuToolsPagesManager"));
			}
			if (buttonServicesConfigurator != null)
			{
				buttonServicesConfigurator.Tooltip = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuToolsServicesConfigurator"));
			}
		}
	}

	public static StiToolsToolbarService GetService()
	{
		return GetService(null);
	}

	public static StiToolsToolbarService GetService(StiDesignerControl designer)
	{
		if (designer == null)
		{
			return StiDesignerOptions.Toolbars.FirstOrDefault((StiToolbarService c) => c.GetType() == typeof(StiToolsToolbarService)) as StiToolsToolbarService;
		}
		return designer.Services.GetServices(typeof(StiToolbarService)).OfType<StiToolsToolbarService>().FirstOrDefault();
	}

	public override void Localize()
	{
		base.Localize();
		LocalizeSuperToolTip();
		if (base.ToolBar != null)
		{
			((Control)base.ToolBar).Text = StiLocalization.Get("Toolbars", "ToolbarTools");
		}
		if (buttonDataStore != null)
		{
			buttonDataStore.Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuToolsDataStore"));
		}
		if (buttonPagesManager != null)
		{
			buttonPagesManager.Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuToolsPagesManager"));
		}
		if (buttonServicesConfigurator != null)
		{
			buttonServicesConfigurator.Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuToolsServicesConfigurator"));
		}
	}

	public override void Init(StiDesignerControl designer)
	{
		if (!StiOptions.Configuration.IsWeb && StiOptions.Engine.FullTrust)
		{
			base.Init(designer);
			CreateToolbar();
			base.ToolBar.SuspendLayout();
			((Control)base.ToolBar).Text = StiLocalization.Get("Toolbars", "ToolbarTools");
			if (ShowDataStore)
			{
				Add(ref buttonDataStore, StiLocalization.Get("MainMenu", "menuToolsDataStore"), ButtonDataStore_Click);
				buttonDataStore.Name = "buttonDataStore";
			}
			if (ShowPagesManager)
			{
				Add(ref buttonPagesManager, StiLocalization.Get("MainMenu", "menuToolsPagesManager"), ButtonPagesManager_Click);
				buttonPagesManager.Name = "buttonPagesManager";
			}
			if (ShowConfigurator)
			{
				Add(ref buttonServicesConfigurator, StiLocalization.Get("MainMenu", "menuToolsServicesConfigurator"), ButtonServicesConfigurator_Click);
				buttonServicesConfigurator.Name = "buttonServicesConfigurator";
			}
			AddToolbarToDockSite();
			LocalizeSuperToolTip();
			base.ToolBar.ResumeLayout();
			designer.RefreshDesigner += DesignerRefresh_Refresh;
		}
	}

	public override void Dispose()
	{
		base.Dispose();
		buttonDataStore = null;
		buttonPagesManager = null;
		buttonServicesConfigurator = null;
	}
}
