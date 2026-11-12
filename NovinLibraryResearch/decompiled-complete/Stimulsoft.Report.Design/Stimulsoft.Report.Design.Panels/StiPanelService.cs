using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Report.Design.Panels;

[StiServiceCategoryBitmap(typeof(StiPanelService), "Stimulsoft.Report.Design.Images.Designer.PanelService.png")]
[StiServiceBitmap(typeof(StiPanelService), "Stimulsoft.Report.Design.Images.Designer.PanelService.png")]
public abstract class StiPanelService : StiDesignerInitService
{
	public sealed override string ServiceCategory => StiLocalization.Get("Services", "categoryPanels");

	public sealed override Type ServiceType => typeof(StiPanelService);

	public abstract DockStyle DockStyle { get; }

	public PanelDockContainer DockingContainer { get; set; }

	public DockContainerItem DockingItem { get; set; }

	public Bar DockingBar { get; set; }

	public virtual eShortcut Shortcut => eShortcut.None;

	protected virtual bool AutoHide => false;

	protected virtual Size DefaultFloatingSize => new Size(StiScale2.XXI(300.0), StiScale2.YYI(500.0));

	public static StiService[] GetPanelServices(StiDesignerControl designer)
	{
		return designer.Services?.GetServices(typeof(StiPanelService)).ToArray();
	}

	public override void Localize()
	{
		base.Localize();
		DockingItem.Text = ServiceName;
		((Control)DockingContainer).Text = ServiceName;
	}

	public override void Init(StiDesignerControl designer)
	{
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected I4, but got Unknown
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Expected I4, but got Unknown
		base.Init(designer);
		PanelDockContainer panelDockContainer = new PanelDockContainer();
		((Control)panelDockContainer).Text = ServiceName;
		DockingContainer = panelDockContainer;
		DockingItem = new DockContainerItem
		{
			Control = (Control)(object)DockingContainer,
			DefaultFloatingSize = DefaultFloatingSize,
			Width = StiScale2.XXI(300.0),
			Height = StiScale2.YYI(300.0),
			Text = ServiceName,
			Name = GetType().ToString()
		};
		DockSite dockSite = null;
		DockStyle dockStyle = DockStyle;
		switch (dockStyle - 1)
		{
		case 2:
			dockSite = Designer.DotNetBarManager.LeftDockSite;
			DockingBar = Designer.DockBarLeft;
			break;
		case 3:
			dockSite = Designer.DotNetBarManager.RightDockSite;
			DockingBar = Designer.DockBarRight;
			break;
		case 0:
			dockSite = Designer.DotNetBarManager.TopDockSite;
			DockingBar = Designer.DockBarTop;
			break;
		case 1:
			dockSite = Designer.DotNetBarManager.BottomDockSite;
			DockingBar = Designer.DockBarBottom;
			break;
		}
		if (DockingBar == null)
		{
			Bar bar = new Bar();
			((Control)bar).Width = StiScale2.XXI(300.0);
			((Control)bar).Height = StiScale2.YYI(300.0);
			DockStyle dockStyle2 = DockStyle;
			bar.Name = "DockBar" + ((object)(DockStyle)(ref dockStyle2)).ToString();
			bar.LayoutType = eLayoutType.DockContainer;
			bar.Stretch = true;
			bar.CanHide = true;
			bar.CanCustomize = false;
			bar.CloseSingleTab = true;
			bar.AutoHideAnimationTime = 0;
			bar.AutoSyncBarCaption = true;
			bar.GrabHandleStyle = eGrabHandleStyle.Caption;
			DockingBar = bar;
			Designer.DotNetBarManager.Bars.Add(DockingBar);
			dockSite.GetDocumentUIManager().Dock(DockingBar);
			DockStyle dockStyle3 = DockStyle;
			switch (dockStyle3 - 1)
			{
			case 2:
				Designer.DockBarLeft = DockingBar;
				break;
			case 3:
				Designer.DockBarRight = DockingBar;
				break;
			case 0:
				Designer.DockBarTop = DockingBar;
				break;
			case 1:
				Designer.DockBarBottom = DockingBar;
				break;
			}
		}
		DockingBar.Items.Add(DockingItem);
		dockSite.GetDocumentUIManager().Dock(DockingBar);
		DockingBar.RecalcLayout();
		DockingBar.AutoHide = AutoHide;
	}

	protected void DisposeComponent(Component component)
	{
		try
		{
			component?.Dispose();
		}
		catch
		{
		}
	}

	public override void Dispose()
	{
		base.Dispose();
		DockingContainer = null;
		DockingBar = null;
		DockingItem = null;
	}
}
