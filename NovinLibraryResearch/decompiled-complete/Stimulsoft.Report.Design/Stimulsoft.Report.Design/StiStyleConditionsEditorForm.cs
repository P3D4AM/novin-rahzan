using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Design;

public class StiStyleConditionsEditorForm : Form
{
	private StiStyleConditionsCollection conditions;

	private IContainer components;

	private ImageList ilButtons;

	private StiZoneContainerControl zcConditions;

	private Button btCancel;

	private Button btOk;

	private StiGroupLine stiGroupLine1;

	private Button tbAddCondition;

	public StiStyleConditionsEditorForm(StiStyleConditionsCollection conditions)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			this.conditions = conditions;
			Localize();
			StiSettings.Load();
			StiFormSettings.Load((Form)(object)this);
			BuildConditions(conditions);
			((Form)this).ShowIcon = false;
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormConditions", "title");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)tbAddCondition).Text = StiLocalization.Get("FormConditions", "AddCondition");
	}

	public void FillConditions(StiStyleConditionsCollection conditions)
	{
		conditions.Clear();
		for (int num = ((ArrangedElementCollection)((Control)zcConditions).Controls).Count - 1; num >= 0; num--)
		{
			if (((Control)zcConditions).Controls[num] is StiStyleConditionZoneControl)
			{
				StiStyleConditionZoneControl stiStyleConditionZoneControl = ((Control)zcConditions).Controls[num] as StiStyleConditionZoneControl;
				StiStyleConditionType stiStyleConditionType = (StiStyleConditionType)0;
				if (stiStyleConditionZoneControl.checkBoxComponentType.Checked)
				{
					stiStyleConditionType |= StiStyleConditionType.ComponentType;
				}
				if (stiStyleConditionZoneControl.checkBoxPlacement.Checked)
				{
					stiStyleConditionType |= StiStyleConditionType.Placement;
				}
				if (stiStyleConditionZoneControl.checkBoxPlacementNestedLevel.Checked)
				{
					stiStyleConditionType |= StiStyleConditionType.PlacementNestedLevel;
				}
				if (stiStyleConditionZoneControl.checkBoxLocation.Checked)
				{
					stiStyleConditionType |= StiStyleConditionType.Location;
				}
				if (stiStyleConditionZoneControl.checkBoxName.Checked)
				{
					stiStyleConditionType |= StiStyleConditionType.ComponentName;
				}
				StiStyleCondition condition = new StiStyleCondition(stiStyleConditionType, ConvertStringToCondition(((Control)stiStyleConditionZoneControl.comboBoxOperationPlacement).Text), ConvertStringToCondition(((Control)stiStyleConditionZoneControl.comboBoxOperationPlacementNestedLevel).Text), ConvertStringToCondition(((Control)stiStyleConditionZoneControl.comboBoxOperationComponentType).Text), ConvertStringToCondition(((Control)stiStyleConditionZoneControl.comboBoxOperationLocation).Text), ConvertStringToCondition(((Control)stiStyleConditionZoneControl.comboBoxOperationName).Text), stiStyleConditionZoneControl.Placement, stiStyleConditionZoneControl.PlacementNestedLevel, stiStyleConditionZoneControl.ComponentType, stiStyleConditionZoneControl.Location, ((Control)stiStyleConditionZoneControl.textBoxName).Text);
				conditions.Add(condition);
			}
		}
	}

	public void BuildConditions(StiStyleConditionsCollection conditions)
	{
		foreach (StiStyleCondition condition in conditions)
		{
			StiStyleConditionZoneControl stiStyleConditionZoneControl = AddLevelCondition(condition.Type, condition.OperationPlacement, condition.OperationPlacementNestedLevel, condition.OperationComponentType, condition.OperationLocation, condition.OperationComponentName, condition.Placement, condition.PlacementNestedLevel, condition.ComponentType, condition.Location, condition.ComponentName);
			((Control)zcConditions).Controls.Add((Control)(object)stiStyleConditionZoneControl);
			((Control)zcConditions).Controls.SetChildIndex((Control)(object)stiStyleConditionZoneControl, 0);
		}
	}

	private string ConvertPlacementToString(StiStyleComponentPlacement placement)
	{
		return placement switch
		{
			StiStyleComponentPlacement.ReportTitle => StiLocalization.Get("Components", "StiReportTitleBand"), 
			StiStyleComponentPlacement.ReportSummary => StiLocalization.Get("Components", "StiReportSummaryBand"), 
			StiStyleComponentPlacement.PageHeader => StiLocalization.Get("Components", "StiPageHeaderBand"), 
			StiStyleComponentPlacement.PageFooter => StiLocalization.Get("Components", "StiPageFooterBand"), 
			StiStyleComponentPlacement.GroupHeader => StiLocalization.Get("Components", "StiGroupHeaderBand"), 
			StiStyleComponentPlacement.GroupFooter => StiLocalization.Get("Components", "StiGroupFooterBand"), 
			StiStyleComponentPlacement.Header => StiLocalization.Get("Components", "StiHeaderBand"), 
			StiStyleComponentPlacement.Footer => StiLocalization.Get("Components", "StiFooterBand"), 
			StiStyleComponentPlacement.ColumnHeader => StiLocalization.Get("Components", "StiColumnHeaderBand"), 
			StiStyleComponentPlacement.ColumnFooter => StiLocalization.Get("Components", "StiColumnFooterBand"), 
			StiStyleComponentPlacement.Data => StiLocalization.Get("Components", "StiDataBand"), 
			StiStyleComponentPlacement.DataEvenStyle => StiLocalization.Get("PropertyMain", "EvenStyle"), 
			StiStyleComponentPlacement.DataOddStyle => StiLocalization.Get("PropertyMain", "OddStyle"), 
			StiStyleComponentPlacement.Table => StiLocalization.Get("Components", "StiTable"), 
			StiStyleComponentPlacement.Hierarchical => StiLocalization.Get("Components", "StiHierarchicalBand"), 
			StiStyleComponentPlacement.Child => StiLocalization.Get("Components", "StiChildBand"), 
			StiStyleComponentPlacement.Empty => StiLocalization.Get("Components", "StiEmptyBand"), 
			StiStyleComponentPlacement.Overlay => StiLocalization.Get("Components", "StiOverlayBand"), 
			StiStyleComponentPlacement.Panel => StiLocalization.Get("Components", "StiPanel"), 
			StiStyleComponentPlacement.Page => StiLocalization.Get("Components", "StiPage"), 
			_ => string.Empty, 
		};
	}

	private StiStyleComponentPlacement ConvertStringToPlacement(string placement)
	{
		if (placement == StiLocalization.Get("Components", "StiReportTitleBand"))
		{
			return StiStyleComponentPlacement.ReportTitle;
		}
		if (placement == StiLocalization.Get("Components", "StiReportSummaryBand"))
		{
			return StiStyleComponentPlacement.ReportTitle;
		}
		if (placement == StiLocalization.Get("Components", "StiPageHeaderBand"))
		{
			return StiStyleComponentPlacement.PageHeader;
		}
		if (placement == StiLocalization.Get("Components", "StiPageFooterBand"))
		{
			return StiStyleComponentPlacement.PageFooter;
		}
		if (placement == StiLocalization.Get("Components", "StiGroupHeaderBand"))
		{
			return StiStyleComponentPlacement.GroupHeader;
		}
		if (placement == StiLocalization.Get("Components", "StiGroupFooterBand"))
		{
			return StiStyleComponentPlacement.GroupFooter;
		}
		if (placement == StiLocalization.Get("Components", "StiHeaderBand"))
		{
			return StiStyleComponentPlacement.Header;
		}
		if (placement == StiLocalization.Get("Components", "StiFooterBand"))
		{
			return StiStyleComponentPlacement.Footer;
		}
		if (placement == StiLocalization.Get("Components", "StiColumnHeaderBand"))
		{
			return StiStyleComponentPlacement.ColumnHeader;
		}
		if (placement == StiLocalization.Get("Components", "StiColumnFooterBand"))
		{
			return StiStyleComponentPlacement.ColumnFooter;
		}
		if (placement == StiLocalization.Get("Components", "StiDataBand"))
		{
			return StiStyleComponentPlacement.Data;
		}
		if (placement == StiLocalization.Get("PropertyMain", "EvenStyle"))
		{
			return StiStyleComponentPlacement.DataEvenStyle;
		}
		if (placement == StiLocalization.Get("PropertyMain", "OddStyle"))
		{
			return StiStyleComponentPlacement.DataOddStyle;
		}
		if (placement == StiLocalization.Get("Components", "StiTable"))
		{
			return StiStyleComponentPlacement.Table;
		}
		if (placement == StiLocalization.Get("Components", "StiHierarchicalBand"))
		{
			return StiStyleComponentPlacement.Hierarchical;
		}
		if (placement == StiLocalization.Get("Components", "StiChildBand"))
		{
			return StiStyleComponentPlacement.Child;
		}
		if (placement == StiLocalization.Get("Components", "StiEmptyBand"))
		{
			return StiStyleComponentPlacement.Empty;
		}
		if (placement == StiLocalization.Get("Components", "StiOverlayBand"))
		{
			return StiStyleComponentPlacement.Overlay;
		}
		if (placement == StiLocalization.Get("Components", "StiPanel"))
		{
			return StiStyleComponentPlacement.Panel;
		}
		if (placement == StiLocalization.Get("Components", "StiPage"))
		{
			return StiStyleComponentPlacement.Page;
		}
		return StiStyleComponentPlacement.Header;
	}

	private string ConvertLocationToString(StiStyleLocation location)
	{
		return location switch
		{
			StiStyleLocation.TopLeft => StiLocalization.Get("PropertyEnum", "ContentAlignmentTopLeft"), 
			StiStyleLocation.TopCenter => StiLocalization.Get("PropertyEnum", "ContentAlignmentTopCenter"), 
			StiStyleLocation.TopRight => StiLocalization.Get("PropertyEnum", "ContentAlignmentTopRight"), 
			StiStyleLocation.MiddleLeft => StiLocalization.Get("PropertyEnum", "ContentAlignmentMiddleLeft"), 
			StiStyleLocation.MiddleCenter => StiLocalization.Get("PropertyEnum", "ContentAlignmentMiddleCenter"), 
			StiStyleLocation.MiddleRight => StiLocalization.Get("PropertyEnum", "ContentAlignmentMiddleRight"), 
			StiStyleLocation.BottomLeft => StiLocalization.Get("PropertyEnum", "ContentAlignmentBottomLeft"), 
			StiStyleLocation.BottomCenter => StiLocalization.Get("PropertyEnum", "ContentAlignmentBottomCenter"), 
			StiStyleLocation.BottomRight => StiLocalization.Get("PropertyEnum", "ContentAlignmentBottomRight"), 
			StiStyleLocation.Left => StiLocalization.Get("PropertyEnum", "HorizontalAlignmentLeft"), 
			StiStyleLocation.CenterHorizontal => StiLocalization.Get("Toolbars", "CenterHorizontally"), 
			StiStyleLocation.Right => StiLocalization.Get("PropertyEnum", "HorizontalAlignmentRight"), 
			StiStyleLocation.Top => StiLocalization.Get("PropertyEnum", "StiVertAlignmentTop"), 
			StiStyleLocation.CenterVertical => StiLocalization.Get("Toolbars", "CenterVertically"), 
			StiStyleLocation.Bottom => StiLocalization.Get("PropertyEnum", "StiVertAlignmentBottom"), 
			_ => string.Empty, 
		};
	}

	private StiStyleLocation ConvertStringToLocation(string location)
	{
		if (location == StiLocalization.Get("PropertyEnum", "ContentAlignmentTopLeft"))
		{
			return StiStyleLocation.TopLeft;
		}
		if (location == StiLocalization.Get("PropertyEnum", "ContentAlignmentTopCenter"))
		{
			return StiStyleLocation.TopCenter;
		}
		if (location == StiLocalization.Get("PropertyEnum", "ContentAlignmentTopRight"))
		{
			return StiStyleLocation.TopRight;
		}
		if (location == StiLocalization.Get("PropertyEnum", "ContentAlignmentMiddleLeft"))
		{
			return StiStyleLocation.MiddleLeft;
		}
		if (location == StiLocalization.Get("PropertyEnum", "ContentAlignmentMiddleCenter"))
		{
			return StiStyleLocation.MiddleCenter;
		}
		if (location == StiLocalization.Get("PropertyEnum", "ContentAlignmentMiddleRight"))
		{
			return StiStyleLocation.MiddleRight;
		}
		if (location == StiLocalization.Get("PropertyEnum", "ContentAlignmentBottomLeft"))
		{
			return StiStyleLocation.BottomLeft;
		}
		if (location == StiLocalization.Get("PropertyEnum", "ContentAlignmentBottomCenter"))
		{
			return StiStyleLocation.BottomCenter;
		}
		if (location == StiLocalization.Get("PropertyEnum", "ContentAlignmentBottomRight"))
		{
			return StiStyleLocation.BottomRight;
		}
		if (location == StiLocalization.Get("PropertyEnum", "HorizontalAlignmentLeft"))
		{
			return StiStyleLocation.Left;
		}
		if (location == StiLocalization.Get("Toolbars", "CenterHorizontally"))
		{
			return StiStyleLocation.CenterHorizontal;
		}
		if (location == StiLocalization.Get("PropertyEnum", "HorizontalAlignmentRight"))
		{
			return StiStyleLocation.Right;
		}
		if (location == StiLocalization.Get("PropertyEnum", "StiVertAlignmentTop"))
		{
			return StiStyleLocation.Top;
		}
		if (location == StiLocalization.Get("Toolbars", "CenterVertically"))
		{
			return StiStyleLocation.CenterVertical;
		}
		if (location == StiLocalization.Get("PropertyEnum", "StiVertAlignmentBottom"))
		{
			return StiStyleLocation.Bottom;
		}
		return StiStyleLocation.None;
	}

	private string ConvertComponentTypeToString(StiStyleComponentType componentType)
	{
		return componentType switch
		{
			StiStyleComponentType.Image => StiLocalization.Get("PropertyEnum", "StiStyleComponentTypeImage"), 
			StiStyleComponentType.Primitive => StiLocalization.Get("PropertyEnum", "StiStyleComponentTypePrimitive"), 
			StiStyleComponentType.Text => StiLocalization.Get("PropertyEnum", "StiStyleComponentTypeText"), 
			StiStyleComponentType.CrossTab => StiLocalization.Get("PropertyEnum", "StiStyleComponentTypeCrossTab"), 
			StiStyleComponentType.Chart => StiLocalization.Get("PropertyEnum", "StiStyleComponentTypeChart"), 
			_ => string.Empty, 
		};
	}

	private StiStyleComponentType ConvertStringToComponentType(string componentType)
	{
		if (componentType == StiLocalization.Get("PropertyEnum", "StiStyleComponentTypeImage"))
		{
			return StiStyleComponentType.Image;
		}
		if (componentType == StiLocalization.Get("PropertyEnum", "StiStyleComponentTypePrimitive"))
		{
			return StiStyleComponentType.Primitive;
		}
		if (componentType == StiLocalization.Get("PropertyEnum", "StiStyleComponentTypeText"))
		{
			return StiStyleComponentType.Text;
		}
		if (componentType == StiLocalization.Get("PropertyEnum", "StiStyleComponentTypeCrossTab"))
		{
			return StiStyleComponentType.CrossTab;
		}
		if (componentType == StiLocalization.Get("PropertyEnum", "StiStyleComponentTypeChart"))
		{
			return StiStyleComponentType.Chart;
		}
		return StiStyleComponentType.Text;
	}

	private string ConvertConditionToString(StiStyleConditionOperation operation)
	{
		return operation switch
		{
			StiStyleConditionOperation.EqualTo => StiLocalization.Get("PropertyEnum", "StiFilterConditionEqualTo"), 
			StiStyleConditionOperation.NotEqualTo => StiLocalization.Get("PropertyEnum", "StiFilterConditionNotEqualTo"), 
			StiStyleConditionOperation.GreaterThan => StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThan"), 
			StiStyleConditionOperation.GreaterThanOrEqualTo => StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThanOrEqualTo"), 
			StiStyleConditionOperation.LessThan => StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThan"), 
			StiStyleConditionOperation.LessThanOrEqualTo => StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThanOrEqualTo"), 
			StiStyleConditionOperation.Containing => StiLocalization.Get("PropertyEnum", "StiFilterConditionContaining"), 
			StiStyleConditionOperation.NotContaining => StiLocalization.Get("PropertyEnum", "StiFilterConditionNotContaining"), 
			StiStyleConditionOperation.BeginningWith => StiLocalization.Get("PropertyEnum", "StiFilterConditionBeginningWith"), 
			StiStyleConditionOperation.EndingWith => StiLocalization.Get("PropertyEnum", "StiFilterConditionEndingWith"), 
			_ => string.Empty, 
		};
	}

	private StiStyleConditionOperation ConvertStringToCondition(string operation)
	{
		if (operation == StiLocalization.Get("PropertyEnum", "StiFilterConditionEqualTo"))
		{
			return StiStyleConditionOperation.EqualTo;
		}
		if (operation == StiLocalization.Get("PropertyEnum", "StiFilterConditionNotEqualTo"))
		{
			return StiStyleConditionOperation.NotEqualTo;
		}
		if (operation == StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThan"))
		{
			return StiStyleConditionOperation.GreaterThan;
		}
		if (operation == StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThanOrEqualTo"))
		{
			return StiStyleConditionOperation.GreaterThanOrEqualTo;
		}
		if (operation == StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThan"))
		{
			return StiStyleConditionOperation.LessThan;
		}
		if (operation == StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThanOrEqualTo"))
		{
			return StiStyleConditionOperation.LessThanOrEqualTo;
		}
		if (operation == StiLocalization.Get("PropertyEnum", "StiFilterConditionContaining"))
		{
			return StiStyleConditionOperation.Containing;
		}
		if (operation == StiLocalization.Get("PropertyEnum", "StiFilterConditionNotContaining"))
		{
			return StiStyleConditionOperation.NotContaining;
		}
		if (operation == StiLocalization.Get("PropertyEnum", "StiFilterConditionBeginningWith"))
		{
			return StiStyleConditionOperation.BeginningWith;
		}
		_ = operation == StiLocalization.Get("PropertyEnum", "StiFilterConditionEndingWith");
		return StiStyleConditionOperation.EndingWith;
	}

	private StiStyleConditionZoneControl AddLevelCondition(StiStyleConditionType type, StiStyleConditionOperation operationPlacement, StiStyleConditionOperation operationPlacementNestedLevel, StiStyleConditionOperation operationComponentType, StiStyleConditionOperation operationLocation, StiStyleConditionOperation operationName, StiStyleComponentPlacement placement, int placementNestedLevel, StiStyleComponentType componentType, StiStyleLocation location, string componentName)
	{
		StiStyleConditionZoneControl stiStyleConditionZoneControl = new StiStyleConditionZoneControl(placement, placementNestedLevel, componentType, location);
		stiStyleConditionZoneControl.checkBoxPlacement.Checked = (type & StiStyleConditionType.Placement) > (StiStyleConditionType)0;
		stiStyleConditionZoneControl.checkBoxPlacementNestedLevel.Checked = (type & StiStyleConditionType.PlacementNestedLevel) > (StiStyleConditionType)0;
		stiStyleConditionZoneControl.checkBoxComponentType.Checked = (type & StiStyleConditionType.ComponentType) > (StiStyleConditionType)0;
		stiStyleConditionZoneControl.checkBoxLocation.Checked = (type & StiStyleConditionType.Location) > (StiStyleConditionType)0;
		stiStyleConditionZoneControl.checkBoxName.Checked = (type & StiStyleConditionType.ComponentName) > (StiStyleConditionType)0;
		stiStyleConditionZoneControl.comboBoxOperationPlacement.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionEqualTo"));
		stiStyleConditionZoneControl.comboBoxOperationPlacement.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotEqualTo"));
		((Control)stiStyleConditionZoneControl.comboBoxOperationPlacement).Text = ConvertConditionToString(operationPlacement);
		stiStyleConditionZoneControl.comboBoxOperationPlacementNestedLevel.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionEqualTo"));
		stiStyleConditionZoneControl.comboBoxOperationPlacementNestedLevel.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotEqualTo"));
		stiStyleConditionZoneControl.comboBoxOperationPlacementNestedLevel.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThan"));
		stiStyleConditionZoneControl.comboBoxOperationPlacementNestedLevel.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThanOrEqualTo"));
		stiStyleConditionZoneControl.comboBoxOperationPlacementNestedLevel.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThan"));
		stiStyleConditionZoneControl.comboBoxOperationPlacementNestedLevel.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThanOrEqualTo"));
		((Control)stiStyleConditionZoneControl.comboBoxOperationPlacementNestedLevel).Text = ConvertConditionToString(operationPlacementNestedLevel);
		stiStyleConditionZoneControl.comboBoxOperationComponentType.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionEqualTo"));
		stiStyleConditionZoneControl.comboBoxOperationComponentType.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotEqualTo"));
		((Control)stiStyleConditionZoneControl.comboBoxOperationComponentType).Text = ConvertConditionToString(operationComponentType);
		stiStyleConditionZoneControl.comboBoxOperationLocation.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionEqualTo"));
		stiStyleConditionZoneControl.comboBoxOperationLocation.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotEqualTo"));
		((Control)stiStyleConditionZoneControl.comboBoxOperationLocation).Text = ConvertConditionToString(operationLocation);
		stiStyleConditionZoneControl.comboBoxOperationName.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionEqualTo"));
		stiStyleConditionZoneControl.comboBoxOperationName.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotEqualTo"));
		stiStyleConditionZoneControl.comboBoxOperationName.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionContaining"));
		stiStyleConditionZoneControl.comboBoxOperationName.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotContaining"));
		stiStyleConditionZoneControl.comboBoxOperationName.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionBeginningWith"));
		stiStyleConditionZoneControl.comboBoxOperationName.Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionEndingWith"));
		((Control)stiStyleConditionZoneControl.comboBoxOperationName).Text = ConvertConditionToString(operationName);
		((Control)stiStyleConditionZoneControl.textBoxName).Text = componentName;
		return stiStyleConditionZoneControl;
	}

	protected override void OnLoad(EventArgs e)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		((Form)this).OnLoad(e);
		Timer timer = new Timer
		{
			Interval = 300
		};
		timer.Tick += delegate
		{
			timer.Stop();
			if (((ArrangedElementCollection)((Control)zcConditions).Controls).Count > 0)
			{
				((Control)(((Control)zcConditions).Controls[0] as StiStyleConditionZoneControl)).Focus();
				(((Control)zcConditions).Controls[0] as StiStyleConditionZoneControl).SetBackColor();
			}
		};
	}

	private void ButtonAddLevel_Click(object sender, EventArgs e)
	{
		StiStyleConditionZoneControl stiStyleConditionZoneControl = AddLevelCondition(StiStyleConditionType.Placement, StiStyleConditionOperation.EqualTo, StiStyleConditionOperation.EqualTo, StiStyleConditionOperation.EqualTo, StiStyleConditionOperation.EqualTo, StiStyleConditionOperation.EqualTo, StiStyleComponentPlacement.None, 1, StiStyleComponentType.Text, StiStyleLocation.None, string.Empty);
		((Control)zcConditions).Controls.Add((Control)(object)stiStyleConditionZoneControl);
		((Control)zcConditions).Controls.SetChildIndex((Control)(object)stiStyleConditionZoneControl, 0);
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).Close();
		}
	}

	private void This_Closed(object sender, EventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		FillConditions(conditions);
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/report_internals_appearance_style_designer_conditions.htm");
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiStyleConditionsEditorForm));
		ilButtons = new ImageList(components);
		btCancel = new Button();
		btOk = new Button();
		stiGroupLine1 = new StiGroupLine();
		tbAddCondition = new Button();
		zcConditions = new StiZoneContainerControl();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)this).SuspendLayout();
		ilButtons.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilButtons.ImageStream");
		ilButtons.TransparentColor = Color.Magenta;
		ilButtons.Images.SetKeyName(0, "AddCondition.png");
		ilButtons.Images.SetKeyName(1, "");
		ilButtons.Images.SetKeyName(2, "");
		ilButtons.Images.SetKeyName(3, "");
		((Control)btCancel).Anchor = (AnchorStyles)10;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(553, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 63;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += This_CancelClick;
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(472, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 62;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)btCancel);
		((Control)stiGroupLine1).Controls.Add((Control)(object)btOk);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 424);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(640, 46);
		((Control)stiGroupLine1).TabIndex = 67;
		((Control)tbAddCondition).Anchor = (AnchorStyles)5;
		((Control)tbAddCondition).Location = new Point(12, 12);
		((Control)tbAddCondition).Name = "tbAddCondition";
		((Control)tbAddCondition).Size = new Size(113, 24);
		((Control)tbAddCondition).TabIndex = 75;
		((Control)tbAddCondition).Text = "Add Condition";
		((ButtonBase)tbAddCondition).UseVisualStyleBackColor = true;
		((Control)tbAddCondition).Click += ButtonAddLevel_Click;
		((Control)zcConditions).AllowDrop = true;
		((Control)zcConditions).Anchor = (AnchorStyles)15;
		((ScrollableControl)zcConditions).AutoScroll = true;
		((Control)zcConditions).BackColor = Color.White;
		((Control)zcConditions).Location = new Point(12, 49);
		((Control)zcConditions).Margin = new Padding(2, 2, 2, 2);
		((Control)zcConditions).Name = "zcConditions";
		((Control)zcConditions).Padding = new Padding(2);
		zcConditions.ShowBorder = true;
		((Control)zcConditions).Size = new Size(617, 364);
		((Control)zcConditions).TabIndex = 61;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(640, 470);
		((Control)this).Controls.Add((Control)(object)tbAddCondition);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Control)this).Controls.Add((Control)(object)zcConditions);
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(643, 489);
		((Control)this).Name = "StiStyleConditionsEditorForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Conditions";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Closed += This_Closed;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
