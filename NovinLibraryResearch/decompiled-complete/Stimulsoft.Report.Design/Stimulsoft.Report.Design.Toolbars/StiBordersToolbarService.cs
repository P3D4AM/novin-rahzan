using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.Toolbars;

public class StiBordersToolbarService : StiToolbarService
{
	private bool locked;

	private ButtonItem buttonBorderAll;

	private ButtonItem buttonBorderNone;

	private ButtonItem buttonBorderTop;

	private ButtonItem buttonBorderLeft;

	private ButtonItem buttonBorderBottom;

	private ButtonItem buttonBorderRight;

	private ButtonItem buttonBorderShadow;

	private ColorPickerDropDown buttonFillBrush;

	private ColorPickerDropDown buttonBorderColor;

	private StiPenStyleItem buttonBorderStyle;

	private StiPenStyleItem buttonBorderStyleSolid;

	private StiPenStyleItem buttonBorderStyleDash;

	private StiPenStyleItem buttonBorderStyleDashDot;

	private StiPenStyleItem buttonBorderStyleDashDotDot;

	private StiPenStyleItem buttonBorderStyleDot;

	private StiPenStyleItem buttonBorderStyleDouble;

	private StiPenStyleItem buttonBorderStyleNone;

	public override string ServiceName => Loc.Get("PropertyMain", "Borders");

	public override int Position => 2;

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowBorderAll
	{
		get
		{
			return StiOptions.Designer.Toolbars.BordersToolbar.ShowBorderAll;
		}
		set
		{
			StiOptions.Designer.Toolbars.BordersToolbar.ShowBorderAll = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowBorderNone
	{
		get
		{
			return StiOptions.Designer.Toolbars.BordersToolbar.ShowBorderNone;
		}
		set
		{
			StiOptions.Designer.Toolbars.BordersToolbar.ShowBorderNone = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowBorderTop
	{
		get
		{
			return StiOptions.Designer.Toolbars.BordersToolbar.ShowBorderTop;
		}
		set
		{
			StiOptions.Designer.Toolbars.BordersToolbar.ShowBorderTop = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowBorderLeft
	{
		get
		{
			return StiOptions.Designer.Toolbars.BordersToolbar.ShowBorderLeft;
		}
		set
		{
			StiOptions.Designer.Toolbars.BordersToolbar.ShowBorderLeft = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowBorderBottom
	{
		get
		{
			return StiOptions.Designer.Toolbars.BordersToolbar.ShowBorderBottom;
		}
		set
		{
			StiOptions.Designer.Toolbars.BordersToolbar.ShowBorderBottom = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowBorderRight
	{
		get
		{
			return StiOptions.Designer.Toolbars.BordersToolbar.ShowBorderRight;
		}
		set
		{
			StiOptions.Designer.Toolbars.BordersToolbar.ShowBorderRight = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowBorderShadow
	{
		get
		{
			return StiOptions.Designer.Toolbars.BordersToolbar.ShowBorderShadow;
		}
		set
		{
			StiOptions.Designer.Toolbars.BordersToolbar.ShowBorderShadow = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowFillBrush
	{
		get
		{
			return StiOptions.Designer.Toolbars.BordersToolbar.ShowFillBrush;
		}
		set
		{
			StiOptions.Designer.Toolbars.BordersToolbar.ShowFillBrush = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowBorderColor
	{
		get
		{
			return StiOptions.Designer.Toolbars.BordersToolbar.ShowBorderColor;
		}
		set
		{
			StiOptions.Designer.Toolbars.BordersToolbar.ShowBorderColor = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowBorderStyle
	{
		get
		{
			return StiOptions.Designer.Toolbars.BordersToolbar.ShowBorderStyle;
		}
		set
		{
			StiOptions.Designer.Toolbars.BordersToolbar.ShowBorderStyle = value;
		}
	}

	private void Designer_DesignerRefresh(object sender, EventArgs e)
	{
		if (!base.ToolBar.Visible)
		{
			return;
		}
		locked = true;
		object obj = ((Report != null) ? Report.GetCurrentPage().GetBorder() : null);
		if (obj != null && !CheckPropertyEnabled(Report, "Border"))
		{
			obj = null;
		}
		if (obj == null)
		{
			base.ToolBar.SuspendLayout();
			if (buttonBorderAll != null && buttonBorderAll.Enabled)
			{
				buttonBorderAll.Enabled = false;
			}
			if (buttonBorderNone != null && buttonBorderNone.Enabled)
			{
				buttonBorderNone.Enabled = false;
			}
			if (buttonBorderBottom != null && buttonBorderBottom.Enabled)
			{
				buttonBorderBottom.Enabled = false;
			}
			if (buttonBorderLeft != null && buttonBorderLeft.Enabled)
			{
				buttonBorderLeft.Enabled = false;
			}
			if (buttonBorderRight != null && buttonBorderRight.Enabled)
			{
				buttonBorderRight.Enabled = false;
			}
			if (buttonBorderTop != null && buttonBorderTop.Enabled)
			{
				buttonBorderTop.Enabled = false;
			}
			if (buttonBorderShadow != null && buttonBorderShadow.Enabled)
			{
				buttonBorderShadow.Enabled = false;
			}
			if (buttonBorderStyle != null && buttonBorderStyle.Enabled)
			{
				buttonBorderStyle.Enabled = false;
			}
			if (buttonBorderColor != null && buttonBorderColor.Enabled)
			{
				buttonBorderColor.Enabled = false;
			}
			base.ToolBar.ResumeLayout();
		}
		else
		{
			base.ToolBar.SuspendLayout();
			if (buttonBorderAll != null && !buttonBorderAll.Enabled)
			{
				buttonBorderAll.Enabled = true;
			}
			if (buttonBorderNone != null && !buttonBorderNone.Enabled)
			{
				buttonBorderNone.Enabled = true;
			}
			if (buttonBorderBottom != null && !buttonBorderBottom.Enabled)
			{
				buttonBorderBottom.Enabled = true;
			}
			if (buttonBorderLeft != null && !buttonBorderLeft.Enabled)
			{
				buttonBorderLeft.Enabled = true;
			}
			if (buttonBorderRight != null && !buttonBorderRight.Enabled)
			{
				buttonBorderRight.Enabled = true;
			}
			if (buttonBorderTop != null && !buttonBorderTop.Enabled)
			{
				buttonBorderTop.Enabled = true;
			}
			if (buttonBorderShadow != null && !buttonBorderShadow.Enabled)
			{
				buttonBorderShadow.Enabled = true;
			}
			if (buttonBorderStyle != null && !buttonBorderStyle.Enabled)
			{
				buttonBorderStyle.Enabled = true;
			}
			if (buttonBorderColor != null && !buttonBorderColor.Enabled)
			{
				buttonBorderColor.Enabled = true;
			}
			if (obj is StiBorder)
			{
				StiBorder stiBorder = obj as StiBorder;
				if (buttonBorderAll != null)
				{
					buttonBorderAll.Checked = stiBorder.Side == StiBorderSides.All;
				}
				if (buttonBorderNone != null)
				{
					buttonBorderNone.Checked = stiBorder.Side == StiBorderSides.None;
				}
				if (buttonBorderBottom != null)
				{
					buttonBorderBottom.Checked = (stiBorder.Side & StiBorderSides.Bottom) > StiBorderSides.None;
				}
				if (buttonBorderLeft != null)
				{
					buttonBorderLeft.Checked = (stiBorder.Side & StiBorderSides.Left) > StiBorderSides.None;
				}
				if (buttonBorderRight != null)
				{
					buttonBorderRight.Checked = (stiBorder.Side & StiBorderSides.Right) > StiBorderSides.None;
				}
				if (buttonBorderTop != null)
				{
					buttonBorderTop.Checked = (stiBorder.Side & StiBorderSides.Top) > StiBorderSides.None;
				}
				if (buttonBorderShadow != null)
				{
					buttonBorderShadow.Checked = stiBorder.DropShadow;
				}
				if (buttonBorderColor != null)
				{
					buttonBorderColor.SelectedColor = stiBorder.Color;
				}
				if (buttonBorderStyle != null)
				{
					buttonBorderStyle.SelectedStyle = stiBorder.Style;
				}
			}
			else if (obj is StiSimpleBorder)
			{
				StiSimpleBorder stiSimpleBorder = obj as StiSimpleBorder;
				if (buttonBorderAll != null)
				{
					buttonBorderAll.Checked = stiSimpleBorder.Side == StiBorderSides.All;
				}
				if (buttonBorderNone != null)
				{
					buttonBorderNone.Checked = stiSimpleBorder.Side == StiBorderSides.None;
				}
				if (buttonBorderBottom != null)
				{
					buttonBorderBottom.Checked = (stiSimpleBorder.Side & StiBorderSides.Bottom) > StiBorderSides.None;
				}
				if (buttonBorderLeft != null)
				{
					buttonBorderLeft.Checked = (stiSimpleBorder.Side & StiBorderSides.Left) > StiBorderSides.None;
				}
				if (buttonBorderRight != null)
				{
					buttonBorderRight.Checked = (stiSimpleBorder.Side & StiBorderSides.Right) > StiBorderSides.None;
				}
				if (buttonBorderTop != null)
				{
					buttonBorderTop.Checked = (stiSimpleBorder.Side & StiBorderSides.Top) > StiBorderSides.None;
				}
				if (buttonBorderColor != null)
				{
					buttonBorderColor.SelectedColor = stiSimpleBorder.Color;
				}
				if (buttonBorderStyle != null)
				{
					buttonBorderStyle.SelectedStyle = stiSimpleBorder.Style;
				}
			}
			base.ToolBar.ResumeLayout();
		}
		StiBrush stiBrush = ((Report != null) ? (Report.GetCurrentPage().GetBrush() as StiBrush) : null);
		if (stiBrush != null && !CheckPropertyEnabled(Report, "Brush"))
		{
			stiBrush = null;
		}
		if (stiBrush != null)
		{
			if (buttonFillBrush != null)
			{
				buttonFillBrush.SelectedColor = StiBrush.ToColor(stiBrush);
			}
			if (buttonFillBrush != null && !buttonFillBrush.Enabled)
			{
				buttonFillBrush.Enabled = true;
			}
		}
		else if (buttonFillBrush != null && buttonFillBrush.Enabled)
		{
			buttonFillBrush.Enabled = false;
		}
		locked = false;
	}

	private void ButtonBorderAll_Click(object sender, EventArgs e)
	{
		buttonBorderTop.Checked = true;
		buttonBorderLeft.Checked = true;
		buttonBorderBottom.Checked = true;
		buttonBorderRight.Checked = true;
		StiActions.Borders.InvokeBorderAll(Designer);
	}

	private void ButtonBorderNone_Click(object sender, EventArgs e)
	{
		buttonBorderTop.Checked = false;
		buttonBorderLeft.Checked = false;
		buttonBorderBottom.Checked = false;
		buttonBorderRight.Checked = false;
		StiActions.Borders.InvokeBorderNone(Designer);
	}

	private void ButtonBorderTop_Click(object sender, EventArgs e)
	{
		StiActions.Borders.InvokeBorderTop(Designer, buttonBorderTop.Checked);
	}

	private void ButtonBorderLeft_Click(object sender, EventArgs e)
	{
		StiActions.Borders.InvokeBorderLeft(Designer, buttonBorderLeft.Checked);
	}

	private void ButtonBorderBottom_Click(object sender, EventArgs e)
	{
		StiActions.Borders.InvokeBorderBottom(Designer, buttonBorderBottom.Checked);
	}

	private void ButtonBorderRight_Click(object sender, EventArgs e)
	{
		StiActions.Borders.InvokeBorderRight(Designer, buttonBorderRight.Checked);
	}

	private void ButtonBorderShadow_Click(object sender, EventArgs e)
	{
		StiActions.Borders.InvokeBorderShadow(Designer, buttonBorderShadow.Checked);
	}

	private void ButtonFillBrush_SelectedColorChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			StiActions.Borders.InvokeFillBrush(Designer, buttonFillBrush.SelectedColor);
		}
	}

	private void ButtonBorderColor_SelectedColorChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			StiActions.Borders.InvokeBorderColor(Designer, buttonBorderColor.SelectedColor);
		}
	}

	private void ButtonPenStyle_Click(object sender, EventArgs e)
	{
		StiPenStyleItem stiPenStyleItem = sender as StiPenStyleItem;
		StiActions.Borders.InvokeBorderStyle(Designer, stiPenStyleItem.SelectedStyle);
	}

	private void ButtonBorderStyle_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		buttonBorderStyleSolid.Checked = buttonBorderStyle.SelectedStyle == StiPenStyle.Solid;
		buttonBorderStyleDash.Checked = buttonBorderStyle.SelectedStyle == StiPenStyle.Dash;
		buttonBorderStyleDashDot.Checked = buttonBorderStyle.SelectedStyle == StiPenStyle.DashDot;
		buttonBorderStyleDashDotDot.Checked = buttonBorderStyle.SelectedStyle == StiPenStyle.DashDotDot;
		buttonBorderStyleDot.Checked = buttonBorderStyle.SelectedStyle == StiPenStyle.Dot;
		buttonBorderStyleDouble.Checked = buttonBorderStyle.SelectedStyle == StiPenStyle.Double;
		buttonBorderStyleNone.Checked = buttonBorderStyle.SelectedStyle == StiPenStyle.None;
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
			if (buttonBorderAll != null)
			{
				SetSuperToolTip(buttonBorderAll, StiHint.CreateHint(Loc.Get("PropertyEnum", "StiBorderSidesAll")), "BorderSidesAll", globalMeasureGraphics);
			}
			if (buttonBorderNone != null)
			{
				SetSuperToolTip(buttonBorderNone, StiHint.CreateHint(Loc.Get("PropertyEnum", "StiBorderSidesNone")), "BorderSidesNone", globalMeasureGraphics);
			}
			if (buttonBorderTop != null)
			{
				SetSuperToolTip(buttonBorderTop, StiHint.CreateHint(Loc.Get("PropertyEnum", "StiBorderSidesTop")), "BorderSidesTop", globalMeasureGraphics);
			}
			if (buttonBorderLeft != null)
			{
				SetSuperToolTip(buttonBorderLeft, StiHint.CreateHint(Loc.Get("PropertyEnum", "StiBorderSidesLeft")), "BorderSidesLeft", globalMeasureGraphics);
			}
			if (buttonBorderBottom != null)
			{
				SetSuperToolTip(buttonBorderBottom, StiHint.CreateHint(Loc.Get("PropertyEnum", "StiBorderSidesBottom")), "BorderSidesBottom", globalMeasureGraphics);
			}
			if (buttonBorderRight != null)
			{
				SetSuperToolTip(buttonBorderRight, StiHint.CreateHint(Loc.Get("PropertyEnum", "StiBorderSidesRight")), "BorderSidesRight", globalMeasureGraphics);
			}
			if (buttonBorderShadow != null)
			{
				SetSuperToolTip(buttonBorderShadow, StiHint.CreateHint(Loc.Get("PropertyMain", "Shadow")), "Shadow", globalMeasureGraphics);
			}
			if (buttonFillBrush != null)
			{
				SetSuperToolTip(buttonFillBrush, StiHint.CreateHint(Loc.Get("PropertyMain", "Background")), "Background", globalMeasureGraphics);
			}
			if (buttonBorderColor != null)
			{
				SetSuperToolTip(buttonBorderColor, StiHint.CreateHint(Loc.Get("PropertyMain", "BorderColor")), "BorderColor", globalMeasureGraphics);
			}
			if (buttonBorderStyle != null)
			{
				SetSuperToolTip(buttonBorderStyle, StiHint.CreateHint(Loc.Get("PropertyMain", "BorderStyle")), "BorderStyle", globalMeasureGraphics);
			}
		}
		else
		{
			if (buttonBorderAll != null)
			{
				buttonBorderAll.Tooltip = StiHint.CreateHint(Loc.Get("PropertyEnum", "StiBorderSidesAll"));
			}
			if (buttonBorderNone != null)
			{
				buttonBorderNone.Tooltip = StiHint.CreateHint(Loc.Get("PropertyEnum", "StiBorderSidesNone"));
			}
			if (buttonBorderTop != null)
			{
				buttonBorderTop.Tooltip = StiHint.CreateHint(Loc.Get("PropertyEnum", "StiBorderSidesTop"));
			}
			if (buttonBorderLeft != null)
			{
				buttonBorderLeft.Tooltip = StiHint.CreateHint(Loc.Get("PropertyEnum", "StiBorderSidesLeft"));
			}
			if (buttonBorderBottom != null)
			{
				buttonBorderBottom.Tooltip = StiHint.CreateHint(Loc.Get("PropertyEnum", "StiBorderSidesBottom"));
			}
			if (buttonBorderRight != null)
			{
				buttonBorderRight.Tooltip = StiHint.CreateHint(Loc.Get("PropertyEnum", "StiBorderSidesRight"));
			}
			if (buttonBorderShadow != null)
			{
				buttonBorderShadow.Tooltip = StiHint.CreateHint(Loc.Get("PropertyMain", "Shadow"));
			}
			if (buttonFillBrush != null)
			{
				buttonFillBrush.Tooltip = StiHint.CreateHint(Loc.Get("PropertyMain", "Background"));
			}
			if (buttonBorderColor != null)
			{
				buttonBorderColor.Tooltip = StiHint.CreateHint(Loc.Get("PropertyMain", "BorderColor"));
			}
			if (buttonBorderStyle != null)
			{
				buttonBorderStyle.Tooltip = StiHint.CreateHint(Loc.Get("PropertyMain", "BorderStyle"));
			}
		}
	}

	public void InvokeBorderAll()
	{
		ButtonBorderAll_Click(null, EventArgs.Empty);
	}

	public void InvokeBorderNone()
	{
		ButtonBorderNone_Click(null, EventArgs.Empty);
	}

	public void InvokeBorderTop()
	{
		ButtonBorderTop_Click(null, EventArgs.Empty);
	}

	public void InvokeBorderLeft()
	{
		ButtonBorderLeft_Click(null, EventArgs.Empty);
	}

	public void InvokeBorderBottom()
	{
		ButtonBorderBottom_Click(null, EventArgs.Empty);
	}

	public void InvokeBorderRight()
	{
		ButtonBorderRight_Click(null, EventArgs.Empty);
	}

	public void InvokeBorderShadow()
	{
		ButtonBorderShadow_Click(null, EventArgs.Empty);
	}

	public static StiBordersToolbarService GetService()
	{
		return GetService(null);
	}

	public static StiBordersToolbarService GetService(StiDesignerControl designer)
	{
		if (designer == null)
		{
			return StiDesignerOptions.Toolbars.FirstOrDefault((StiToolbarService c) => c.GetType() == typeof(StiBordersToolbarService)) as StiBordersToolbarService;
		}
		return designer.Services.GetServices(typeof(StiToolbarService)).OfType<StiBordersToolbarService>().FirstOrDefault();
	}

	public override void Localize()
	{
		base.Localize();
		if (base.ToolBar != null)
		{
			((Control)base.ToolBar).Text = Loc.Get("PropertyMain", "Borders");
		}
		if (buttonBorderAll != null)
		{
			buttonBorderAll.Text = StiHint.CreateHint(Loc.Get("PropertyEnum", "StiBorderSidesAll"));
		}
		if (buttonBorderNone != null)
		{
			buttonBorderNone.Text = StiHint.CreateHint(Loc.Get("PropertyEnum", "StiBorderSidesNone"));
		}
		if (buttonBorderTop != null)
		{
			buttonBorderTop.Text = StiHint.CreateHint(Loc.Get("PropertyEnum", "StiBorderSidesTop"));
		}
		if (buttonBorderLeft != null)
		{
			buttonBorderLeft.Text = StiHint.CreateHint(Loc.Get("PropertyEnum", "StiBorderSidesLeft"));
		}
		if (buttonBorderBottom != null)
		{
			buttonBorderBottom.Text = StiHint.CreateHint(Loc.Get("PropertyEnum", "StiBorderSidesBottom"));
		}
		if (buttonBorderRight != null)
		{
			buttonBorderRight.Text = StiHint.CreateHint(Loc.Get("PropertyEnum", "StiBorderSidesRight"));
		}
		if (buttonBorderShadow != null)
		{
			buttonBorderShadow.Text = StiHint.CreateHint(Loc.Get("PropertyMain", "Shadow"));
		}
		if (buttonFillBrush != null)
		{
			buttonFillBrush.Text = StiHint.CreateHint(Loc.Get("PropertyMain", "Background"));
		}
		if (buttonBorderColor != null)
		{
			buttonBorderColor.Text = StiHint.CreateHint(Loc.Get("PropertyMain", "BorderColor"));
		}
		if (buttonBorderStyle != null)
		{
			buttonBorderStyle.Text = "";
		}
		buttonFillBrush.Localize();
		buttonBorderColor.Localize();
		LocalizeSuperToolTip();
	}

	public override void Init(StiDesignerControl designer)
	{
		base.Init(designer);
		CreateToolbar();
		base.ToolBar.SuspendLayout();
		((Control)base.ToolBar).Text = Loc.Get("PropertyMain", "Borders");
		if (ShowBorderAll)
		{
			Add(ref buttonBorderAll, StiReportDesignImages.Editor.BorderAll(), Loc.Get("PropertyEnum", "StiBorderSidesAll"), ButtonBorderAll_Click);
			buttonBorderAll.Name = "buttonBorderAll";
		}
		if (ShowBorderNone)
		{
			Add(ref buttonBorderNone, StiReportDesignImages.Editor.BorderNone(), Loc.Get("PropertyEnum", "StiBorderSidesNone"), ButtonBorderNone_Click);
			buttonBorderNone.Name = "buttonBorderNone";
		}
		AddToolButtonSeparator();
		if (ShowBorderTop)
		{
			Add(ref buttonBorderTop, StiReportDesignImages.Editor.BorderTop(), Loc.Get("PropertyEnum", "StiBorderSidesTop"), ButtonBorderTop_Click);
			buttonBorderTop.AutoCheckOnClick = true;
			buttonBorderTop.Name = "buttonBorderTop";
		}
		if (ShowBorderLeft)
		{
			Add(ref buttonBorderLeft, StiReportDesignImages.Editor.BorderLeft(), Loc.Get("PropertyEnum", "StiBorderSidesLeft"), ButtonBorderLeft_Click);
			buttonBorderLeft.AutoCheckOnClick = true;
			buttonBorderLeft.Name = "buttonBorderLeft";
		}
		if (ShowBorderBottom)
		{
			Add(ref buttonBorderBottom, StiReportDesignImages.Editor.BorderBottom(), Loc.Get("PropertyEnum", "StiBorderSidesBottom"), ButtonBorderBottom_Click);
			buttonBorderBottom.AutoCheckOnClick = true;
			buttonBorderBottom.Name = "buttonBorderBottom";
		}
		if (ShowBorderRight)
		{
			Add(ref buttonBorderRight, StiReportDesignImages.Editor.BorderRight(), Loc.Get("PropertyEnum", "StiBorderSidesRight"), ButtonBorderRight_Click);
			buttonBorderRight.AutoCheckOnClick = true;
			buttonBorderRight.Name = "buttonBorderRight";
		}
		AddToolButtonSeparator();
		if (ShowBorderShadow)
		{
			Add(ref buttonBorderShadow, StiReportDesignImages.Editor.BorderShadow(), Loc.Get("PropertyMain", "Shadow"), ButtonBorderShadow_Click);
			buttonBorderShadow.AutoCheckOnClick = true;
			buttonBorderShadow.Name = "buttonBorderShadow";
		}
		AddToolButtonSeparator();
		if (ShowFillBrush)
		{
			AddColorItem(ref buttonFillBrush, StiReportDesignImages.Editor.Brush(), StiHint.CreateHint(Loc.Get("PropertyMain", "Background")), ButtonFillBrush_SelectedColorChanged);
			buttonFillBrush.Name = "buttonFillBrush";
		}
		if (ShowBorderColor)
		{
			AddColorItem(ref buttonBorderColor, StiReportDesignImages.Editor.Fill(), StiHint.CreateHint(Loc.Get("PropertyMain", "BorderColor")), ButtonBorderColor_SelectedColorChanged);
			buttonBorderColor.Name = "buttonBorderColor";
		}
		if (ShowBorderStyle)
		{
			buttonBorderStyle = new StiPenStyleItem();
			buttonBorderStyle.PopupOpen += ButtonBorderStyle_PopupOpen;
			buttonBorderStyle.AutoExpandOnClick = true;
			buttonBorderStyle.FixedSize = StiScale.I(new Size(80, 22));
			buttonBorderStyle.Text = "";
			buttonBorderStyle.Name = "buttonBorderStyle";
			base.ToolBar.Items.Add(buttonBorderStyle);
			buttonBorderStyleSolid = new StiPenStyleItem
			{
				SelectedStyle = StiPenStyle.Solid,
				FixedSize = StiScale.I(new Size(150, 22)),
				Text = Loc.Get("PropertyEnum", "StiPenStyleSolid")
			};
			buttonBorderStyleSolid.Click += ButtonPenStyle_Click;
			buttonBorderStyle.SubItems.Add(buttonBorderStyleSolid);
			buttonBorderStyleDash = new StiPenStyleItem
			{
				SelectedStyle = StiPenStyle.Dash,
				FixedSize = StiScale.I(new Size(150, 22)),
				Text = Loc.Get("PropertyEnum", "StiPenStyleDash")
			};
			buttonBorderStyleDash.Click += ButtonPenStyle_Click;
			buttonBorderStyle.SubItems.Add(buttonBorderStyleDash);
			buttonBorderStyleDashDot = new StiPenStyleItem
			{
				SelectedStyle = StiPenStyle.DashDot,
				FixedSize = StiScale.I(new Size(150, 22)),
				Text = Loc.Get("PropertyEnum", "StiPenStyleDashDot")
			};
			buttonBorderStyleDashDot.Click += ButtonPenStyle_Click;
			buttonBorderStyle.SubItems.Add(buttonBorderStyleDashDot);
			buttonBorderStyleDashDotDot = new StiPenStyleItem
			{
				SelectedStyle = StiPenStyle.DashDotDot,
				FixedSize = StiScale.I(new Size(150, 22)),
				Text = Loc.Get("PropertyEnum", "StiPenStyleDashDotDot")
			};
			buttonBorderStyleDashDotDot.Click += ButtonPenStyle_Click;
			buttonBorderStyle.SubItems.Add(buttonBorderStyleDashDotDot);
			buttonBorderStyleDot = new StiPenStyleItem
			{
				SelectedStyle = StiPenStyle.Dot,
				FixedSize = StiScale.I(new Size(150, 22)),
				Text = Loc.Get("PropertyEnum", "StiPenStyleDot")
			};
			buttonBorderStyleDot.Click += ButtonPenStyle_Click;
			buttonBorderStyle.SubItems.Add(buttonBorderStyleDot);
			buttonBorderStyleDouble = new StiPenStyleItem
			{
				SelectedStyle = StiPenStyle.Double,
				FixedSize = StiScale.I(new Size(150, 22)),
				Text = Loc.Get("PropertyEnum", "StiPenStyleDouble")
			};
			buttonBorderStyleDouble.Click += ButtonPenStyle_Click;
			buttonBorderStyle.SubItems.Add(buttonBorderStyleDouble);
			buttonBorderStyleNone = new StiPenStyleItem
			{
				SelectedStyle = StiPenStyle.None,
				FixedSize = StiScale.I(new Size(150, 22)),
				Text = Loc.Get("PropertyEnum", "StiPenStyleNone")
			};
			buttonBorderStyleNone.Click += ButtonPenStyle_Click;
			buttonBorderStyle.SubItems.Add(buttonBorderStyleNone);
		}
		AddToolbarToDockSite();
		base.ToolBar.ResumeLayout();
		LocalizeSuperToolTip();
		designer.RefreshDesigner += Designer_DesignerRefresh;
	}

	public override void Dispose()
	{
		base.Dispose();
		buttonBorderAll = null;
		buttonBorderNone = null;
		buttonBorderLeft = null;
		buttonBorderTop = null;
		buttonBorderRight = null;
		buttonBorderBottom = null;
		buttonBorderShadow = null;
		buttonFillBrush = null;
		buttonBorderColor = null;
		buttonBorderStyle = null;
		buttonBorderStyleSolid = null;
		buttonBorderStyleDash = null;
		buttonBorderStyleDashDot = null;
		buttonBorderStyleDashDotDot = null;
		buttonBorderStyleDot = null;
		buttonBorderStyleDouble = null;
		buttonBorderStyleNone = null;
	}
}
