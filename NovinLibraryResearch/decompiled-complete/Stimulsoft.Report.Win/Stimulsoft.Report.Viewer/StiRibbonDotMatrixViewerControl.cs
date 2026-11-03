using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Win.Ribbon;

namespace Stimulsoft.Report.Viewer;

public class StiRibbonDotMatrixViewerControl : StiDotMatrixViewerControl
{
	[Category("Parameters")]
	[DefaultValue(true)]
	public override bool ShowToolbar
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	private void Initialize()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		StiRibbonDotMatrixViewUserControl stiRibbonDotMatrixViewUserControl = new StiRibbonDotMatrixViewUserControl(this);
		Panel val = new Panel();
		((Control)val).Size = new Size(100, 93);
		((Control)val).Dock = (DockStyle)1;
		((Control)val).Name = "RibbonDotMatrixViewerMenuPanel";
		RibbonPanel val2 = new RibbonPanel();
		((PanelControl)val2).ColorSchemeStyle = (eDotNetBarStyle)8;
		((PanelControl)val2).Style.CornerType = (eCornerType)1;
		((PanelControl)val2).StyleMouseDown.CornerType = (eCornerType)1;
		((PanelControl)val2).StyleMouseOver.CornerType = (eCornerType)1;
		((Control)val).Controls.Add((Control)(object)val2);
		((Control)val2).Dock = (DockStyle)5;
		((Control)val2).Padding = new Padding(3, 0, 3, 3);
		int count = ((ArrangedElementCollection)((Control)stiRibbonDotMatrixViewUserControl).Controls).Count;
		for (int i = 0; i < count; i++)
		{
			Control obj = ((Control)stiRibbonDotMatrixViewUserControl).Controls[0];
			RibbonBar val3 = (RibbonBar)(object)((obj is RibbonBar) ? obj : null);
			if (val3 != null)
			{
				((Control)val2).Controls.Add((Control)(object)val3);
			}
		}
		((Control)this).Controls.Add((Control)(object)val);
	}

	public StiRibbonDotMatrixViewerControl()
		: this(showRibbonMenu: true)
	{
	}

	public StiRibbonDotMatrixViewerControl(bool showRibbonMenu)
	{
		base.ShowToolbar = false;
		if (showRibbonMenu)
		{
			Initialize();
		}
	}
}
