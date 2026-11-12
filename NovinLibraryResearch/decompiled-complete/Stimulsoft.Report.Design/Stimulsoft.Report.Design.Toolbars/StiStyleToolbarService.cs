using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.Toolbars;

public class StiStyleToolbarService : StiToolbarService
{
	private ButtonItem tbStyleDesigner;

	private StiStyleItem cbStyle;

	public override string ServiceName => StiLocalization.Get("Toolbars", "ToolbarStyle");

	public override int Position => 3;

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowStyleDesigner
	{
		get
		{
			return StiOptions.Designer.Toolbars.StyleToolbar.ShowStyleDesigner;
		}
		set
		{
			StiOptions.Designer.Toolbars.StyleToolbar.ShowStyleDesigner = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowStyles
	{
		get
		{
			return StiOptions.Designer.Toolbars.StyleToolbar.ShowStyles;
		}
		set
		{
			StiOptions.Designer.Toolbars.StyleToolbar.ShowStyles = value;
		}
	}

	private void Refresh(object sender, EventArgs e)
	{
		if (base.ToolBar.Visible && Report != null)
		{
			if (cbStyle != null)
			{
				string text = null;
				if (Designer.SelectedComponentsOnPage != null && Designer.SelectedComponentsOnPage.Count > 0)
				{
					text = Designer.SelectedComponentsOnPage[0].ComponentStyle;
				}
				foreach (StiBaseStyle style in Report.Styles)
				{
					if (style.Name == text)
					{
						cbStyle.SelectedStyle = style;
						return;
					}
				}
			}
			if (cbStyle != null)
			{
				cbStyle.SelectedStyle = null;
			}
		}
		if (tbStyleDesigner != null)
		{
			tbStyleDesigner.Enabled = Report != null;
		}
		if (cbStyle != null)
		{
			cbStyle.Enabled = Report != null;
			if (cbStyle.Enabled && !CheckPropertyEnabled(Report, "ComponentStyle"))
			{
				cbStyle.Enabled = false;
			}
		}
	}

	private void tbStyleDesigner_Click(object sender, EventArgs e)
	{
		StiActions.Style.InvokeStyleDesigner(Designer);
	}

	private void cbStyle_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		cbStyle.SubItems.Clear();
		string text = null;
		foreach (StiComponent item in Designer.SelectedComponentsOnPage)
		{
			if (text == null)
			{
				text = item.ComponentStyle.Trim();
			}
			else if (text != item.ComponentStyle)
			{
				text = null;
				break;
			}
		}
		StiStyleItem stiStyleItem = new StiStyleItem();
		stiStyleItem.Click += cbStyle_Click;
		stiStyleItem.Checked = text != null && text.Trim().Length == 0;
		cbStyle.SubItems.Add(stiStyleItem);
		foreach (StiBaseStyle style in Designer.Report.Styles)
		{
			if (style is StiStyle)
			{
				StiStyleItem stiStyleItem2 = new StiStyleItem();
				stiStyleItem2.Checked = text != null && text == style.Name;
				stiStyleItem2.Click += cbStyle_Click;
				stiStyleItem2.SelectedStyle = style;
				stiStyleItem2.FixedSize = StiScale.I(new Size(120, 26));
				cbStyle.SubItems.Add(stiStyleItem2);
			}
		}
	}

	private void cbStyle_Click(object sender, EventArgs e)
	{
		StiStyleItem stiStyleItem = sender as StiStyleItem;
		StiActions.Style.InvokeSelectStyle(Designer, stiStyleItem.SelectedStyle);
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
			if (tbStyleDesigner != null)
			{
				SetSuperToolTip(tbStyleDesigner, StiHint.CreateHint(StiLocalization.Get("Toolbars", "StyleDesigner")), "StyleDesigner", globalMeasureGraphics);
			}
			if (cbStyle != null)
			{
				SetSuperToolTip(cbStyle, StiHint.CreateHint(StiLocalization.Get("Toolbars", "Styles")), "ToolbarStyle", globalMeasureGraphics);
			}
		}
		else
		{
			if (tbStyleDesigner != null)
			{
				tbStyleDesigner.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "StyleDesigner"));
			}
			if (cbStyle != null)
			{
				cbStyle.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "Styles"));
			}
		}
	}

	public void InvokeStyleDesigner()
	{
		tbStyleDesigner_Click(null, EventArgs.Empty);
	}

	public static StiStyleToolbarService GetService()
	{
		return GetService(null);
	}

	public static StiStyleToolbarService GetService(StiDesignerControl designer)
	{
		if (designer == null)
		{
			return StiDesignerOptions.Toolbars.FirstOrDefault((StiToolbarService c) => c.GetType() == typeof(StiStyleToolbarService)) as StiStyleToolbarService;
		}
		return designer.Services.GetServices(typeof(StiToolbarService)).OfType<StiStyleToolbarService>().FirstOrDefault();
	}

	public override void Localize()
	{
		base.Localize();
		LocalizeSuperToolTip();
		if (base.ToolBar != null)
		{
			((Control)base.ToolBar).Text = StiLocalization.Get("Toolbars", "ToolbarStyle");
		}
		if (tbStyleDesigner != null)
		{
			tbStyleDesigner.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "StyleDesigner"));
		}
	}

	public override void Init(StiDesignerControl designer)
	{
		if (!StiOptions.Configuration.IsWeb && StiOptions.Engine.FullTrust)
		{
			base.Init(designer);
			CreateToolbar();
			base.ToolBar.SuspendLayout();
			((Control)base.ToolBar).Text = StiLocalization.Get("Toolbars", "ToolbarStyle");
			if (ShowStyleDesigner)
			{
				Add(ref tbStyleDesigner, StiReportDesignImages.Actions.RunStyleDesigner(), StiLocalization.Get("Toolbars", "StyleDesigner"), tbStyleDesigner_Click);
				tbStyleDesigner.Name = "tbStyleDesigner";
			}
			AddToolButtonSeparator();
			if (ShowStyles)
			{
				cbStyle = new StiStyleItem
				{
					Name = "cbStyle",
					AutoExpandOnClick = true,
					FixedSize = StiScale.I(new Size(80, 22))
				};
				cbStyle.PopupOpen += cbStyle_PopupOpen;
				base.ToolBar.Items.Add(cbStyle);
				StiStyleItem item = new StiStyleItem();
				cbStyle.SubItems.Add(item);
			}
			AddToolbarToDockSite();
			base.ToolBar.ResumeLayout();
			LocalizeSuperToolTip();
			designer.RefreshDesigner += Refresh;
		}
	}

	public override void Dispose()
	{
		base.Dispose();
		tbStyleDesigner = null;
		cbStyle = null;
	}
}
