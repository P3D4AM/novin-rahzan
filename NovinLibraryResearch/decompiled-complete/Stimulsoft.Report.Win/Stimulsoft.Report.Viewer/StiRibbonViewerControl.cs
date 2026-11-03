using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Win;
using Stimulsoft.Report.Win.Ribbon;

namespace Stimulsoft.Report.Viewer;

[ToolboxBitmap(typeof(StiProgressInformation), "Toolbox.StiRibbonViewerControl.bmp")]
public class StiRibbonViewerControl : StiViewerControl
{
	internal StiRibbonViewerUserControl userRibbon;

	public void ResetShortcuts()
	{
		if (userRibbon != null)
		{
			userRibbon.ResetShortcuts();
		}
	}

	public void InitShortcuts()
	{
		if (userRibbon != null)
		{
			userRibbon.InitShortcuts();
		}
	}

	private void Initialize()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		userRibbon = new StiRibbonViewerUserControl(this);
		RibbonControl val = new RibbonControl();
		RibbonTabItem val2 = new RibbonTabItem();
		RibbonPanel val3 = new RibbonPanel();
		((Control)this).SuspendLayout();
		val.SuspendLayout();
		((Control)val3).SuspendLayout();
		((Control)val).Controls.Add((Control)(object)val3);
		val.CaptionVisible = false;
		((Control)val).Dock = (DockStyle)1;
		val.Items.AddRange((BaseItem[])(object)new BaseItem[1] { (BaseItem)val2 });
		val.KeyTipsFont = new Font("Arial", 7f);
		((Control)val).Location = new Point(0, 0);
		((Control)val).Name = "RibbonViewerMenuControl";
		((Control)val).Padding = new Padding(0, 0, 0, 2);
		((Control)val).Size = new Size(500, 122);
		val.Style = (eDotNetBarStyle)8;
		val.TabGroupHeight = 14;
		((Control)val).TabIndex = 0;
		((Control)val).Text = "RibbonViewerMenuControl";
		((ButtonItem)val2).Checked = true;
		((ButtonItem)val2).ImageListSizeSelection = (eButtonImageListSelection)(-1);
		((ButtonItem)val2).ImagePaddingHorizontal = 8;
		((BaseItem)val2).Name = "ribbonViewerTabItem";
		val2.Panel = val3;
		((BaseItem)val2).Text = StiLocalization.Get("FormDesigner", "Preview");
		((BaseItem)val2).Name = "ribbonViewerTabItem";
		val2.Panel = val3;
		((PanelControl)val3).ColorSchemeStyle = (eDotNetBarStyle)8;
		((Control)val3).Dock = (DockStyle)5;
		val3.Location = new Point(0, 25);
		((Control)val3).Name = "ribbonPanel1";
		((Control)val3).Padding = new Padding(3, 0, 3, 3);
		val3.Size = new Size(500, 95);
		((Control)val3).TabIndex = 1;
		int count = ((ArrangedElementCollection)((Control)userRibbon).Controls).Count;
		for (int i = 0; i < count; i++)
		{
			Control obj = ((Control)userRibbon).Controls[0];
			RibbonBar val4 = (RibbonBar)(object)((obj is RibbonBar) ? obj : null);
			if (val4 != null)
			{
				((Control)val3).Controls.Add((Control)(object)val4);
			}
		}
		((Control)this).Controls.Add((Control)(object)val);
		val.ResumeLayout(false);
		((Control)val3).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}

	public StiRibbonViewerControl()
		: this(null)
	{
	}

	public StiRibbonViewerControl(StiReport report)
		: this(report, removeShortcuts: false, showRibbonMenu: true)
	{
	}

	public StiRibbonViewerControl(StiReport report, bool removeShortcuts, bool showRibbonMenu)
		: base(report, removeShortcuts)
	{
		if (showRibbonMenu)
		{
			Initialize();
		}
	}
}
