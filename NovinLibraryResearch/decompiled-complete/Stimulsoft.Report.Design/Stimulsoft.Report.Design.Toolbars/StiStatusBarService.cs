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
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Design.Toolbars;

public class StiStatusBarService : StiToolbarService
{
	private ButtonItem buttonUnit;

	private LabelItem labelComponent;

	private LabelItem labelLocation;

	private ExtendedButtonItem buttonReportChecker;

	private ButtonItem itemCentimeters;

	private ButtonItem itemHundredthsOfInch;

	private ButtonItem itemInches;

	private ButtonItem itemMillimeters;

	public override string ServiceName => StiLocalization.Get("FormViewer", "StatusBar");

	public override int Position => 9;

	public override DockStyle DefaultDock => (DockStyle)2;

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowUnits { get; set; } = true;


	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowLocation { get; set; } = true;


	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowComponentName { get; set; } = true;


	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	[Obsolete("Please use ShowReportChecker property instead ShowMessages proeprty")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowMessages
	{
		get
		{
			return ShowReportChecker;
		}
		set
		{
			ShowReportChecker = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowReportChecker { get; set; } = true;


	private void buttonUnit_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		if (Designer != null)
		{
			itemCentimeters.Checked = Designer.Report.Unit is StiCentimetersUnit;
			itemHundredthsOfInch.Checked = Designer.Report.Unit is StiHundredthsOfInchUnit;
			itemInches.Checked = Designer.Report.Unit is StiInchesUnit;
			itemMillimeters.Checked = Designer.Report.Unit is StiMillimetersUnit;
			itemCentimeters.Enabled = true;
			itemHundredthsOfInch.Enabled = true;
			itemInches.Enabled = true;
			itemMillimeters.Enabled = true;
		}
		else
		{
			itemCentimeters.Enabled = false;
			itemHundredthsOfInch.Enabled = false;
			itemInches.Enabled = false;
			itemMillimeters.Enabled = false;
		}
	}

	private void ItemCentimetersClick(object sender, EventArgs e)
	{
		try
		{
			Designer.Report.Unit = new StiCentimetersUnit();
			((Control)Designer).Refresh();
			Designer.InvokeSelectedObjectsChanged();
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ItemHundredthsOfInchClick(object sender, EventArgs e)
	{
		try
		{
			Designer.Report.Unit = new StiHundredthsOfInchUnit();
			((Control)Designer).Refresh();
			Designer.InvokeSelectedObjectsChanged();
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ItemInchesClick(object sender, EventArgs e)
	{
		try
		{
			Designer.Report.Unit = new StiInchesUnit();
			((Control)Designer).Refresh();
			Designer.InvokeSelectedObjectsChanged();
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ItemMillimetersClick(object sender, EventArgs e)
	{
		try
		{
			Designer.Report.Unit = new StiMillimetersUnit();
			((Control)Designer).Refresh();
			Designer.InvokeSelectedObjectsChanged();
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void Designer_RefreshStatusBar(object sender, EventArgs e)
	{
		if (Designer.Report == null)
		{
			if (labelComponent != null)
			{
				labelComponent.Text = string.Empty;
			}
			if (labelLocation != null)
			{
				labelLocation.Text = string.Empty;
				labelLocation.Width = StiScale.I(100);
			}
			if (buttonUnit != null)
			{
				buttonUnit.Text = string.Empty;
			}
			if (buttonUnit != null)
			{
				buttonUnit.Enabled = false;
			}
			if (buttonReportChecker != null)
			{
				buttonReportChecker.Enabled = false;
			}
			return;
		}
		if (buttonUnit != null)
		{
			buttonUnit.Enabled = true;
			if (Report.GetCurrentPage() is IStiForm)
			{
				buttonUnit.Text = StiLocalization.Get("PropertyEnum", "StiReportUnitTypePixels");
			}
			else
			{
				buttonUnit.Text = Report.GetCurrentPage().Unit.ToString();
			}
		}
		if (buttonReportChecker != null)
		{
			buttonReportChecker.Enabled = true;
		}
		if (labelComponent != null && Designer.SelectedObject != null)
		{
			object selectedObject = Designer.SelectedObject;
			if (selectedObject is StiReport)
			{
				labelComponent.Text = ((StiReport)selectedObject).ReportName;
			}
			if (selectedObject is StiComponent)
			{
				labelComponent.Text = ((StiComponent)selectedObject).Name;
			}
		}
		if (labelLocation == null)
		{
			return;
		}
		if (Designer.CurrentTool != null)
		{
			labelLocation.Text = Designer.CurrentTool.GetStatusBarInformation();
			if (string.IsNullOrEmpty(labelLocation.Text))
			{
				labelLocation.Width = StiScale.I(100);
			}
			else
			{
				labelLocation.Width = 0;
			}
		}
		else
		{
			labelLocation.Text = string.Empty;
			labelLocation.Width = StiScale.I(100);
		}
	}

	public static StiStatusBarService GetService()
	{
		return GetService(null);
	}

	public static StiStatusBarService GetService(StiDesignerControl designer)
	{
		if (designer == null)
		{
			return StiDesignerOptions.Toolbars.FirstOrDefault((StiToolbarService c) => c.GetType() == typeof(StiStatusBarService)) as StiStatusBarService;
		}
		return designer.Services.GetServices(typeof(StiToolbarService)).OfType<StiStatusBarService>().FirstOrDefault();
	}

	public override void Localize()
	{
		base.Localize();
		if (base.ToolBar != null)
		{
			((Control)base.ToolBar).Text = StiLocalization.Get("FormViewer", "StatusBar");
		}
		if (itemCentimeters != null)
		{
			itemCentimeters.Text = StiLocalization.Get("PropertyEnum", "StiReportUnitTypeCentimeters");
		}
		if (itemHundredthsOfInch != null)
		{
			itemHundredthsOfInch.Text = StiLocalization.Get("PropertyEnum", "StiReportUnitTypeHundredthsOfInch");
		}
		if (itemInches != null)
		{
			itemInches.Text = StiLocalization.Get("PropertyEnum", "StiReportUnitTypeInches");
		}
		if (itemMillimeters != null)
		{
			itemMillimeters.Text = StiLocalization.Get("PropertyEnum", "StiReportUnitTypeMillimeters");
		}
		if (buttonReportChecker != null)
		{
			buttonReportChecker.Text = StiLocalization.Get("MainMenu", "menuCheckIssues");
		}
	}

	public override void Init(StiDesignerControl designer)
	{
		base.Init(designer);
		CreateToolbar();
		((Control)base.ToolBar).Text = StiLocalization.Get("FormViewer", "StatusBar");
		base.ToolBar.RoundCorners = false;
		base.ToolBar.Stretch = true;
		base.ToolBar.BarType = eBarType.StatusBar;
		if (ShowUnits && StiOptions.Designer.Toolbars.StatusBar.ShowUnits)
		{
			buttonUnit = new ButtonItem
			{
				Name = "buttonUnit",
				PopupSide = ePopupSide.Top,
				AutoExpandOnClick = true,
				ForeColor = Color.White,
				ShowExpandArrow = false
			};
			buttonUnit.PopupOpen += buttonUnit_PopupOpen;
			itemCentimeters = new ButtonItem
			{
				Text = StiLocalization.Get("PropertyEnum", "StiReportUnitTypeCentimeters")
			};
			itemCentimeters.Click += ItemCentimetersClick;
			itemHundredthsOfInch = new ButtonItem
			{
				Text = StiLocalization.Get("PropertyEnum", "StiReportUnitTypeHundredthsOfInch")
			};
			itemHundredthsOfInch.Click += ItemHundredthsOfInchClick;
			itemInches = new ButtonItem
			{
				Text = StiLocalization.Get("PropertyEnum", "StiReportUnitTypeInches")
			};
			itemInches.Click += ItemInchesClick;
			itemMillimeters = new ButtonItem
			{
				Text = StiLocalization.Get("PropertyEnum", "StiReportUnitTypeMillimeters")
			};
			itemMillimeters.Click += ItemMillimetersClick;
			buttonUnit.SubItems.AddRange(new BaseItem[4] { itemCentimeters, itemHundredthsOfInch, itemInches, itemMillimeters });
			base.ToolBar.Items.Add(buttonUnit);
		}
		if (ShowReportChecker && StiOptions.Designer.Toolbars.StatusBar.ShowReportChecker)
		{
			buttonReportChecker = new ExtendedButtonItem
			{
				ButtonStyle = eButtonStyle.ImageAndText,
				Image = StiReportDesignImages.Actions.Run(),
				Name = "buttonReportChecker",
				BeginGroup = true,
				Text = StiLocalization.Get("MainMenu", "menuCheckIssues"),
				ForeColor = Color.White
			};
			buttonReportChecker.Click += delegate
			{
				designer.RunReportChecker(buttonReportChecker);
			};
			base.ToolBar.Items.Add(buttonReportChecker);
			designer.ShowingReportCheckerResults += delegate
			{
				designer.UpdateCheckHelperButton(buttonReportChecker);
			};
			designer.ClearingReportCheckerResults += delegate
			{
				designer.RefreshCheckHelperButton(buttonReportChecker);
			};
		}
		if (ShowComponentName && StiOptions.Designer.Toolbars.StatusBar.ShowComponentName)
		{
			labelComponent = new LabelItem
			{
				Name = "labelComponent",
				BeginGroup = true,
				Text = string.Empty,
				TextAlignment = (StringAlignment)1
			};
			base.ToolBar.Items.Add(labelComponent);
		}
		if (ShowLocation && StiOptions.Designer.Toolbars.StatusBar.ShowLocation)
		{
			labelLocation = new LabelItem
			{
				Name = "labelLocation",
				BeginGroup = true,
				Text = string.Empty,
				Width = StiScale.I(100),
				TextAlignment = (StringAlignment)1
			};
			base.ToolBar.Items.Add(labelLocation);
		}
		AddToolbarToDockSite();
		base.ToolBar.ResumeLayout();
		designer.RefreshStatusBar += Designer_RefreshStatusBar;
	}

	public override void Dispose()
	{
		base.Dispose();
		labelComponent = null;
		labelLocation = null;
		buttonUnit = null;
		itemCentimeters = null;
		itemHundredthsOfInch = null;
		itemInches = null;
		itemMillimeters = null;
	}
}
