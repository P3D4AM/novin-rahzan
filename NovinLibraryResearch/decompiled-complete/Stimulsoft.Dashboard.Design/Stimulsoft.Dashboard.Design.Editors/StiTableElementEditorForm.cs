using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Dashboard.Design.Controls;
using Stimulsoft.Dashboard.Design.Editors.Helpers;
using Stimulsoft.Dashboard.Design.Editors.Menu;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Dashboard.Interactions;
using Stimulsoft.Data.Functions;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Design.Panels;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Dashboard.Design.Editors;

public class StiTableElementEditorForm : StiElementEditorForm
{
	private IContainer components;

	private StiMeterListBox listBoxColumns;

	private StiPanelUX panelUi;

	private StiOperationRadioButton buttonMeasure;

	private StiOperationRadioButton buttonDimension;

	private StiLabelUX labelType;

	private ToolTip toolTip;

	private StiOperationRadioButton buttonDataBars;

	private StiOperationRadioButton buttonSparklines;

	private StiOperationRadioButton buttonColorScale;

	private StiLabelUX labelSparklines;

	private StiPanelUX panelSparklines;

	private StiCheckBoxUX checkBoxFirstLastPoints;

	private StiCheckBoxUX checkBoxHighLowPoints;

	private StiOperationRadioButton buttonSparklinesWinLoss;

	private StiOperationRadioButton buttonSparklinesColumn;

	private StiOperationRadioButton buttonSparklinesArea;

	private StiOperationRadioButton buttonSparklinesLine;

	private StiOperationRadioButton buttonIndicator;

	private StiLabelUX labelExpression;

	private StiPanelBox panelBoxExpression;

	private HintTextBox textBoxExpression;

	private StiOperationButton buttonExpressionMenu;

	private ContextMenuBar contextMenuBar;

	private ButtonItem menuExpression;

	private StiPanelUX panelHyperlink;

	private StiPanelBox panelBoxHyperlinkPattern;

	private HintTextBox textBoxHyperlinkPattern;

	private StiOperationButton buttonEditHyperlink;

	private StiLabelUX labelHyperlinkPattern;

	private StiPanelUX panelShowHyperlink;

	private StiPanelUX panelShowTotals;

	private StiCheckBoxUX checkBoxShowHyperlink;

	private StiCheckBoxUX checkBoxShowTotal;

	private StiPanelUX panelBlockInteraction;

	private StiPanelBox panelInteraction;

	private StiLabelUX labelInteractionStatus;

	private StiOperationButton buttonEditInteraction;

	private StiLabelUX labelInteraction;

	private StiOperationButton buttonSetDefaultInteraction;

	private StiLabelUX labelHyperlinkSample;

	private StiSampleLabelUX labelExpressionSample2;

	private StiOperationButton buttonExpressionEdit;

	private StiLabelUX labelSummaryType;

	private StiComboBox comboBoxSummaryType;

	private StiPanelUX panelSummaryType;

	private StiOperationRadioButton buttonBubble;

	private StiPanelUX panelColorScaleUI;

	private StiColorBoxXF colorBoxMinimum;

	private StiColorBoxXF colorBoxMaximum;

	private StiLabelUX labelMaximum;

	private StiLabelUX labelMinimum;

	private StiLabelUX labelMinimumMaximumSeparator;

	private StiPanelUX panelColors;

	private StiColorBoxXF colorBoxNegativeColor;

	private StiColorBoxXF colorBoxPositiveColor;

	private StiPanelUX panelColorsMode;

	private StiComboBox comboBoxColorMode;

	private StiLabelUX labelColors;

	private StiLabelUX labelPositiveNegativeSeparator;

	private StiPanelUX panelColorsLabels;

	private StiLabelUX labelNegative;

	private StiLabelUX labelPositive;

	private StiPanelUX panelColorsEmpty;

	private StiComboBox comboBoxVisible;

	private StiLabelUX labelVisible;

	private StiPanelUX panelVisibility;

	private StiPanelBox panelVisibleExpression;

	private HintTextBox textBoxVisibleExpression;

	private StiOperationButton buttonEditVisibleExpression;

	internal StiTableElement TableElement => base.Element as StiTableElement;

	internal StiTableColumn TableColumn => listBoxColumns.SelectedMeterItem?.Meter as StiTableColumn;

	public override bool AllowResize => false;

	public override bool IsAnyPresent => ((ListBox)listBoxColumns).Items.Count > 0;

	private string LocColorModeFromStyle => Loc.Get("FormStyleDesigner", "FromStyle");

	private string LocColorModeCustom => Loc.Get("FormColorBoxPopup", "Custom");

	private string LocVisibleTrue => Loc.Get("FormFormatEditor", "nameTrue");

	private string LocVisibleFalse => Loc.Get("FormFormatEditor", "nameFalse");

	private string LocVisibleExpression => Loc.GetMain("Expression");

	public StiTableElementEditorForm()
		: this(null)
	{
	}

	public StiTableElementEditorForm(StiElement element)
		: base(element)
	{
		InitializeComponent();
		if (StiOptions.Appearance.ShowFormsHelp)
		{
			((Form)this).HelpButton = true;
		}
		if (!((Control)this).MinimumSize.IsEmpty)
		{
			((Form)this).Size = ((Control)this).MinimumSize;
		}
		foreach (StiMeterListBox item in (from Control c in (IEnumerable)((Control)this).Controls
			where c is StiMeterListBox
			select c).Cast<StiMeterListBox>())
		{
			item.Designer = Designer;
			item.Editor = this;
			contextMenuBar.SetContextMenuEx((Control)(object)item, menuExpression);
		}
		ReadPropertiesFromElement();
		LocalizeInternal();
		InitializeMenus();
		ActivateDictionaryPanel();
		DoScaling();
		((ListBox)listBoxColumns).SelectedIndexChanged += delegate
		{
			StiPropertiesPanelHelper.SetPropertyGrid(Designer, TableColumn);
		};
	}

	public override void Localize()
	{
		LocalizeInternal();
		((Control)listBoxColumns).Invalidate();
	}

	private void LocalizeInternal()
	{
		((Control)this).Text = Loc.Get("Components", "StiTable");
		((Control)labelType).Text = Loc.Get("PropertyMain", "Type");
		((Control)labelSparklines).Text = Loc.Get("Dashboard", "Sparklines");
		((Control)labelExpression).Text = Loc.Get("PropertyMain", "Expression");
		((Control)labelHyperlinkPattern).Text = Loc.Get("PropertyMain", "Pattern");
		((Control)labelInteraction).Text = Loc.Get("PropertyMain", "Interaction");
		((Control)labelHyperlinkSample).Text = Loc.Get("FormFormatEditor", "Sample") + ":";
		((Control)labelColors).Text = Loc.GetMain("Color");
		((Control)labelPositive).Text = Loc.GetMain("Positive");
		((Control)labelNegative).Text = Loc.GetMain("Negative");
		((Control)labelMinimum).Text = (Loc.IsCyrillic ? Loc.GetEnum("StiFilterItemValueLow") : Loc.GetMain("Minimum"));
		((Control)labelMaximum).Text = (Loc.IsCyrillic ? Loc.GetEnum("StiFilterItemValueHigh") : Loc.GetMain("Maximum"));
		if (StiLocalization.CultureName != "en")
		{
			((Control)labelSummaryType).Text = Loc.GetMain("Summary");
		}
		else
		{
			((Control)labelSummaryType).Text = Loc.GetMain("SummaryType");
		}
		((Control)checkBoxShowTotal).Text = Loc.GetMain("ShowTotal");
		((Control)labelVisible).Text = Loc.GetMain("Visible");
		((Control)checkBoxFirstLastPoints).Text = Loc.Get("Dashboard", "FirstLastPoints");
		((Control)checkBoxHighLowPoints).Text = Loc.Get("Dashboard", "HighLowPoints");
		((Control)checkBoxShowHyperlink).Text = Loc.GetEnum("StiInteractionOnHoverShowHyperlink");
		toolTip.SetToolTip((Control)(object)buttonDimension, Loc.Get("Dashboard", "Dimension"));
		toolTip.SetToolTip((Control)(object)buttonMeasure, Loc.Get("Dashboard", "Measure"));
		toolTip.SetToolTip((Control)(object)buttonDataBars, Loc.Get("Dashboard", "DataBars"));
		toolTip.SetToolTip((Control)(object)buttonColorScale, Loc.Get("Dashboard", "ColorScale"));
		toolTip.SetToolTip((Control)(object)buttonIndicator, Loc.Get("Dashboard", "Indicator"));
		toolTip.SetToolTip((Control)(object)buttonSparklines, Loc.Get("Dashboard", "Sparklines"));
		toolTip.SetToolTip((Control)(object)buttonBubble, Loc.Get("Chart", "Bubble"));
		toolTip.SetToolTip((Control)(object)buttonSparklinesLine, Loc.Get("Chart", "SparklinesLine"));
		toolTip.SetToolTip((Control)(object)buttonSparklinesArea, Loc.Get("Chart", "SparklinesArea"));
		toolTip.SetToolTip((Control)(object)buttonSparklinesColumn, Loc.Get("Chart", "SparklinesColumn"));
		toolTip.SetToolTip((Control)(object)buttonSparklinesWinLoss, Loc.Get("Chart", "SparklinesWinLoss"));
		int selectedIndex = ((ListControl)comboBoxColorMode).SelectedIndex;
		((ComboBox)comboBoxColorMode).BeginUpdate();
		((ComboBox)comboBoxColorMode).Items.Clear();
		((ComboBox)comboBoxColorMode).Items.Add((object)LocColorModeFromStyle);
		((ComboBox)comboBoxColorMode).Items.Add((object)LocColorModeCustom);
		((ListControl)comboBoxColorMode).SelectedIndex = selectedIndex;
		((ComboBox)comboBoxColorMode).EndUpdate();
		selectedIndex = ((ListControl)comboBoxVisible).SelectedIndex;
		((ComboBox)comboBoxVisible).BeginUpdate();
		((ComboBox)comboBoxVisible).Items.Clear();
		((ComboBox)comboBoxVisible).Items.Add((object)LocVisibleTrue);
		((ComboBox)comboBoxVisible).Items.Add((object)LocVisibleFalse);
		((ComboBox)comboBoxVisible).Items.Add((object)LocVisibleExpression);
		((ListControl)comboBoxVisible).SelectedIndex = selectedIndex;
		((ComboBox)comboBoxVisible).EndUpdate();
	}

	private void InitializeMenus()
	{
		menuExpression.PopupOpen += delegate
		{
			new StiTableExpressionMenu(menuExpression, (TextBox)(object)textBoxExpression, this, listBoxColumns).CreateItems();
		};
	}

	private void DoScaling()
	{
		((ButtonBase)buttonEditHyperlink).Image = StiReportDesignImages.Actions.Edit();
		((ButtonBase)buttonSetDefaultInteraction).Image = StiReportDesignImages.Actions.Remove();
		((ButtonBase)buttonEditInteraction).Image = StiReportDesignImages.Actions.Edit();
		((ButtonBase)buttonExpressionEdit).Image = StiReportDesignImages.Actions.Edit();
		((ButtonBase)buttonDimension).Image = StiReportDesignImages.Actions.Dimension();
		((ButtonBase)buttonMeasure).Image = StiReportDesignImages.Actions.Measure();
		((ButtonBase)buttonColorScale).Image = StiReportDesignImages.Table.ColorScale();
		((ButtonBase)buttonDataBars).Image = StiReportDesignImages.Table.DataBars();
		((ButtonBase)buttonIndicator).Image = StiReportDesignImages.Table.Indicator();
		((ButtonBase)buttonSparklines).Image = StiReportDesignImages.Table.Sparklines();
		((ButtonBase)buttonBubble).Image = StiReportDesignImages.Table.Bubble();
		((ButtonBase)buttonSparklinesArea).Image = StiReportDesignImages.Table.SparklinesArea();
		((ButtonBase)buttonSparklinesColumn).Image = StiReportDesignImages.Table.SparklinesColumn();
		((ButtonBase)buttonSparklinesLine).Image = StiReportDesignImages.Table.Sparklines();
		((ButtonBase)buttonSparklinesWinLoss).Image = StiReportDesignImages.Table.SparklinesWinLoss();
		StiButtonsLocator.ArrangeButtons((Control)(object)panelBoxExpression);
		StiButtonsLocator.ArrangeButtons((Control)(object)panelBoxHyperlinkPattern);
		StiButtonsLocator.ArrangeButtons((Control)(object)panelInteraction);
	}

	public override void ReadPropertiesFromElement()
	{
		if (listBoxColumns.ItemsEqualTo(TableElement.Columns))
		{
			return;
		}
		((ListBox)listBoxColumns).BeginUpdate();
		((ListBox)listBoxColumns).Items.Clear();
		foreach (StiTableColumn column in TableElement.Columns)
		{
			listBoxColumns.AddItem(column, GetNodeImageKey(column));
		}
		CheckSelectedColumns();
		if (listBoxColumns.SelectedMeterItem == null)
		{
			listBoxColumns.SelectFirstItem();
		}
		((ListBox)listBoxColumns).EndUpdate();
	}

	public override void WritePropertiesToElement()
	{
		TableElement.Columns = (from t in listBoxColumns.ToItems()
			select t.Meter).Cast<StiTableColumn>().ToList();
		RefreshElementView();
	}

	private void CheckSelectedColumns()
	{
		StiTableColumn selectedColumn = StiTableElementSelection.GetSelected(TableElement)?.FirstOrDefault();
		listBoxColumns.SelectedMeterItem = ((selectedColumn != null) ? listBoxColumns.ToItems().FirstOrDefault((StiMeterItem t) => t.Meter == selectedColumn) : null);
	}

	private StiMeterItemImage GetNodeImageKey(StiTableColumn column)
	{
		if (column is StiDimensionColumn)
		{
			return StiMeterItemImage.Dimension;
		}
		if (column is StiDataBarsColumn)
		{
			return StiMeterItemImage.DataBars;
		}
		if (column is StiColorScaleColumn)
		{
			return StiMeterItemImage.ColorScale;
		}
		if (column is StiIndicatorColumn)
		{
			return StiMeterItemImage.Indicator;
		}
		if (column is StiSparklinesColumn stiSparklinesColumn)
		{
			if (stiSparklinesColumn.Type == StiSparklinesType.Line)
			{
				return StiMeterItemImage.SparklinesLine;
			}
			if (stiSparklinesColumn.Type == StiSparklinesType.Area)
			{
				return StiMeterItemImage.SparklinesArea;
			}
			if (stiSparklinesColumn.Type == StiSparklinesType.Column)
			{
				return StiMeterItemImage.SparklinesColumn;
			}
			if (stiSparklinesColumn.Type == StiSparklinesType.WinLoss)
			{
				return StiMeterItemImage.SparklinesWinLoss;
			}
		}
		if (column is StiBubbleColumn)
		{
			return StiMeterItemImage.Bubble;
		}
		return StiMeterItemImage.Measure;
	}

	protected override void CalculateWindowHeight()
	{
		if (IsAnyPresent)
		{
			base.CalculateWindowHeight();
		}
		else
		{
			((Form)this).ClientSize = new Size(((Form)this).ClientSize.Width, base.PaddingCorrection + ((Control)listBoxColumns).Width);
		}
	}

	protected override void SetListHeightFromCount(StiMeterListBox list, bool multiple)
	{
		int val = Math.Max(((ListBox)list).Items.Count + 1, 7);
		val = Math.Min(10, val);
		((Control)list).Height = val * ((ListBox)list).ItemHeight;
	}

	protected override void AttachEvents()
	{
		base.AttachEvents();
		AttachEventsToColumns();
	}

	private void AttachEventsToColumns()
	{
		listBoxColumns.ElementsConverting += delegate(object sender, EventArgs e)
		{
			StiMeterItem stiMeterItem2 = sender as StiMeterItem;
			stiMeterItem2.Meter = StiMeterHelper.Table.GetColumn(stiMeterItem2.Meter);
			RefreshNodeImage();
		};
		listBoxColumns.ElementsChanged += delegate(object sender, StiElementChangedArgs changedArgs)
		{
			ProcessElementChanging(changedArgs);
			WritePropertiesToElement();
			RefreshUiPanel();
		};
		listBoxColumns.ElementsDragEnter += delegate
		{
			ResetComponentHighlights();
		};
		listBoxColumns.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				Type type = StiMeterHelper.ToDataType(draggedDataColumnOrVariable);
				StiTableColumn stiTableColumn = ((type != null && type.IsNumericType()) ? ((StiTableColumn)StiMeterHelper.Table.GetMeasure(draggedDataColumnOrVariable)) : ((StiTableColumn)StiMeterHelper.Table.GetDimension(draggedDataColumnOrVariable)));
				if (stiTableColumn is StiMeasureColumn)
				{
					stiTableColumn.ShowTotalSummary = true;
					stiTableColumn.SummaryType = StiSummaryColumnType.Sum;
				}
				string label = StiLabelHelper.GetLabel(stiTableColumn);
				StiMeterItem stiMeterItem = new StiMeterItem(label, stiTableColumn, GetNodeImageKey(stiTableColumn));
				StiMeterItem itemAtMouse = listBoxColumns.GetItemAtMouse();
				if (itemAtMouse != null)
				{
					int index = ((ListBox)listBoxColumns).Items.IndexOf((object)itemAtMouse);
					listBoxColumns.InsertItem(index, stiMeterItem);
				}
				else
				{
					listBoxColumns.AddItem(stiMeterItem);
				}
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxColumns.SelectedMeterItem = stiMeterItem;
				((Control)listBoxColumns).Focus();
			}
			else if (StiDragDropColumnHelper.IsTableElementDragged(Designer))
			{
				if (!(StiDragDropColumnHelper.GetDraggedTableElement(Designer) is StiTableElement stiTableElement))
				{
					return;
				}
				int count = ((ListBox)listBoxColumns).Items.Count;
				foreach (StiTableColumn column in stiTableElement.Columns)
				{
					TableElement.Columns.Add(column);
				}
				RefreshElementView();
				ReadPropertiesFromElement();
				Designer.PageView.ResetDragging();
				((ListControl)listBoxColumns).SelectedIndex = count;
			}
			ResetComponentHighlights();
			RefreshUiPanel();
		};
		listBoxColumns.ElementsSelectedChanged += delegate
		{
			RefreshUiPanel();
			StiTableElementSelection.ResetSelection(TableElement);
			StiTableElementSelection.Select(TableElement, TableColumn);
			TableElement.Select();
			RefreshElementView();
		};
	}

	public void RefreshNodeImage()
	{
		listBoxColumns.SetSelectedImage(GetNodeImageKey(TableColumn));
	}

	public override void RefreshUiPanel()
	{
		if (base.LockRefresh)
		{
			return;
		}
		base.LockRefresh = true;
		((Control)this).SuspendLayout();
		try
		{
			StiPanelUX stiPanelUX = panelUi;
			StiPanelUX stiPanelUX2 = panelBlockInteraction;
			StiPanelUX stiPanelUX3 = panelShowTotals;
			StiLabelUX stiLabelUX = labelExpression;
			bool flag = (((Control)panelBoxExpression).Visible = IsAnyPresent);
			bool flag3 = (((Control)stiLabelUX).Visible = flag);
			bool flag5 = (((Control)stiPanelUX3).Visible = flag3);
			bool visible = (((Control)stiPanelUX2).Visible = flag5);
			((Control)stiPanelUX).Visible = visible;
			((Control)listBoxColumns).Dock = (DockStyle)((((ListBox)listBoxColumns).Items.Count != 0) ? 1 : 5);
			((RadioButton)buttonDimension).Checked = TableColumn is StiDimensionColumn;
			((RadioButton)buttonMeasure).Checked = TableColumn is StiMeasureColumn;
			((RadioButton)buttonDataBars).Checked = TableColumn is StiDataBarsColumn;
			((RadioButton)buttonColorScale).Checked = TableColumn is StiColorScaleColumn;
			((RadioButton)buttonSparklines).Checked = TableColumn is StiSparklinesColumn;
			((RadioButton)buttonIndicator).Checked = TableColumn is StiIndicatorColumn;
			((RadioButton)buttonBubble).Checked = TableColumn is StiBubbleColumn;
			((Control)textBoxExpression).Text = ((TableColumn != null) ? TableColumn.Expression : "");
			((CheckBox)checkBoxShowTotal).Checked = TableColumn == null || TableColumn.ShowTotalSummary;
			if (TableColumn != null)
			{
				if (TableColumn.Visibility == StiTableColumnVisibility.True)
				{
					((ComboBox)comboBoxVisible).SelectedItem = LocVisibleTrue;
				}
				else if (TableColumn.Visibility == StiTableColumnVisibility.False)
				{
					((ComboBox)comboBoxVisible).SelectedItem = LocVisibleFalse;
				}
				else if (TableColumn.Visibility == StiTableColumnVisibility.Expression)
				{
					((ComboBox)comboBoxVisible).SelectedItem = LocVisibleExpression;
				}
				((Control)textBoxVisibleExpression).Text = TableColumn.VisibilityExpression;
			}
			((Control)panelVisibility).Visible = IsAnyPresent && TableColumn != null && TableColumn.Visibility == StiTableColumnVisibility.Expression;
			((Control)panelSparklines).Visible = ((Control)panelUi).Visible && ((RadioButton)buttonSparklines).Checked;
			((Control)panelColorScaleUI).Visible = ((Control)panelUi).Visible && ((RadioButton)buttonColorScale).Checked;
			((Control)panelSummaryType).Visible = IsAnyPresent && ((CheckBox)checkBoxShowTotal).Checked;
			if (TableColumn == null)
			{
				((ListControl)comboBoxSummaryType).SelectedIndex = 0;
			}
			else
			{
				switch (TableColumn.SummaryType)
				{
				case StiSummaryColumnType.Average:
					((ComboBox)comboBoxSummaryType).SelectedItem = "Average";
					break;
				case StiSummaryColumnType.Count:
					((ComboBox)comboBoxSummaryType).SelectedItem = "Count";
					break;
				case StiSummaryColumnType.Max:
					((ComboBox)comboBoxSummaryType).SelectedItem = "Max";
					break;
				case StiSummaryColumnType.Min:
					((ComboBox)comboBoxSummaryType).SelectedItem = "Min";
					break;
				case StiSummaryColumnType.Sum:
					((ComboBox)comboBoxSummaryType).SelectedItem = "Sum";
					break;
				}
			}
			StiSparklinesColumn stiSparklinesColumn = TableColumn as StiSparklinesColumn;
			((RadioButton)buttonSparklinesLine).Checked = stiSparklinesColumn != null && stiSparklinesColumn.Type == StiSparklinesType.Line;
			((RadioButton)buttonSparklinesArea).Checked = stiSparklinesColumn != null && stiSparklinesColumn.Type == StiSparklinesType.Area;
			((RadioButton)buttonSparklinesColumn).Checked = stiSparklinesColumn != null && stiSparklinesColumn.Type == StiSparklinesType.Column;
			((RadioButton)buttonSparklinesWinLoss).Checked = stiSparklinesColumn != null && stiSparklinesColumn.Type == StiSparklinesType.WinLoss;
			((CheckBox)checkBoxFirstLastPoints).Checked = stiSparklinesColumn?.ShowFirstLastPoints ?? false;
			((CheckBox)checkBoxHighLowPoints).Checked = stiSparklinesColumn?.ShowHighLowPoints ?? false;
			bool flag7 = stiSparklinesColumn != null && (stiSparklinesColumn.Type == StiSparklinesType.Line || stiSparklinesColumn.Type == StiSparklinesType.Area);
			StiCheckBoxUX stiCheckBoxUX = checkBoxFirstLastPoints;
			visible = (((Control)checkBoxHighLowPoints).Visible = flag7);
			((Control)stiCheckBoxUX).Visible = visible;
			((Control)panelSparklines).Height = (flag7 ? ((Control)checkBoxFirstLastPoints).Bottom : ((Control)buttonDimension).Bottom);
			StiDimensionColumn stiDimensionColumn = TableColumn as StiDimensionColumn;
			((CheckBox)checkBoxShowHyperlink).Checked = stiDimensionColumn?.ShowHyperlink ?? false;
			((Control)textBoxHyperlinkPattern).Text = ((stiDimensionColumn != null) ? stiDimensionColumn.HyperlinkPattern : "");
			((Control)labelInteractionStatus).Text = ((TableColumn?.DashboardInteraction != null && TableColumn.DashboardInteraction.IsDefault) ? ("(" + Loc.GetMain("Default") + ")") : ("(" + Loc.Get("FormFormatEditor", "Custom") + ")"));
			((Control)buttonSetDefaultInteraction).Enabled = TableColumn?.DashboardInteraction == null || !TableColumn.DashboardInteraction.IsDefault;
			((Control)panelShowHyperlink).Visible = ((RadioButton)buttonDimension).Checked;
			((Control)panelHyperlink).Visible = ((RadioButton)buttonDimension).Checked && ((CheckBox)checkBoxShowHyperlink).Checked;
			if (TableColumn is StiColorScaleColumn stiColorScaleColumn)
			{
				colorBoxMinimum.SelectedColor = stiColorScaleColumn.MinimumColor;
				colorBoxMaximum.SelectedColor = stiColorScaleColumn.MaximumColor;
			}
			StiSparklinesColumn stiSparklinesColumn2 = TableColumn as StiSparklinesColumn;
			if (stiSparklinesColumn2 != null)
			{
				colorBoxPositiveColor.SelectedColor = stiSparklinesColumn2.PositiveColor;
				colorBoxNegativeColor.SelectedColor = stiSparklinesColumn2.NegativeColor;
			}
			StiBubbleColumn stiBubbleColumn = TableColumn as StiBubbleColumn;
			if (stiBubbleColumn != null)
			{
				colorBoxPositiveColor.SelectedColor = stiBubbleColumn.PositiveColor;
				colorBoxNegativeColor.SelectedColor = stiBubbleColumn.NegativeColor;
			}
			StiLabelUX stiLabelUX2 = labelPositiveNegativeSeparator;
			StiPanelUX stiPanelUX4 = panelColorsLabels;
			flag5 = (((Control)colorBoxNegativeColor).Visible = ((RadioButton)buttonSparklinesColumn).Checked || ((RadioButton)buttonSparklinesWinLoss).Checked || ((RadioButton)buttonBubble).Checked);
			visible = (((Control)stiPanelUX4).Visible = flag5);
			((Control)stiLabelUX2).Visible = visible;
			((Control)labelColors).Text = ((((RadioButton)buttonSparklinesColumn).Checked || ((RadioButton)buttonSparklinesWinLoss).Checked) ? Loc.GetMain("Colors") : Loc.GetMain("Color"));
			((ComboBox)comboBoxColorMode).SelectedItem = (((stiSparklinesColumn2 != null && stiSparklinesColumn2.AllowCustomColors) || (stiBubbleColumn != null && stiBubbleColumn.AllowCustomColors)) ? LocColorModeCustom : LocColorModeFromStyle);
			((Control)panelColorsMode).Visible = IsAnyPresent && (((RadioButton)buttonSparklines).Checked || ((RadioButton)buttonBubble).Checked);
			((Control)panelColors).Visible = IsAnyPresent && ((((RadioButton)buttonSparklines).Checked && stiSparklinesColumn2 != null && stiSparklinesColumn2.AllowCustomColors) || (((RadioButton)buttonBubble).Checked && stiBubbleColumn != null && stiBubbleColumn.AllowCustomColors));
			((Control)panelColorsLabels).Visible = ((Control)panelColors).Visible && (((RadioButton)buttonSparklinesColumn).Checked || ((RadioButton)buttonSparklinesWinLoss).Checked || ((RadioButton)buttonBubble).Checked);
			((Control)panelColorsEmpty).Visible = !((Control)panelColorsLabels).Visible && stiSparklinesColumn2 != null && stiSparklinesColumn2.AllowCustomColors;
			((Control)colorBoxPositiveColor).Width = (((Control)panelColorsEmpty).Visible ? ((Control)comboBoxColorMode).Width : ((Control)colorBoxNegativeColor).Width);
			SetListHeightFromCount(listBoxColumns, multiple: true);
			CalculateWindowHeight();
			CalculateWindowPosition();
			RefreshExpressionHint();
		}
		finally
		{
			((Control)this).ResumeLayout();
			base.LockRefresh = false;
		}
	}

	private void RefreshExpressionHint()
	{
		if (((RadioButton)buttonDimension).Checked || ((RadioButton)buttonSparklines).Checked)
		{
			textBoxExpression.Hint = Loc.GetMain("Field");
		}
		else if (((RadioButton)buttonMeasure).Checked || ((RadioButton)buttonDataBars).Checked || ((RadioButton)buttonColorScale).Checked || ((RadioButton)buttonBubble).Checked)
		{
			textBoxExpression.Hint = "Sum(" + Loc.GetMain("Field") + ")";
		}
		else if (((RadioButton)buttonIndicator).Checked)
		{
			textBoxExpression.Hint = "Sum(" + Loc.GetMain("Field") + ") / Sum(" + Loc.GetMain("Target") + ")";
		}
		else
		{
			textBoxExpression.Hint = "";
		}
	}

	internal void SwitchCurrentMeterToDimension()
	{
		((RadioButton)buttonDimension).Checked = true;
		((Control)textBoxExpression).Focus();
	}

	internal void SwitchCurrentMeterToMeasure()
	{
		((RadioButton)buttonMeasure).Checked = true;
		((Control)textBoxExpression).Focus();
		if (TableColumn?.TextFormat != null && !(TableColumn.TextFormat is StiNumberFormatService))
		{
			TableColumn.TextFormat = new StiNumberFormatService();
			Designer.InvokeRefreshDesigner();
		}
	}

	internal void SwitchToPercentOfGrandTotal()
	{
		((RadioButton)buttonMeasure).Checked = true;
		((Control)textBoxExpression).Focus();
		if (TableColumn?.TextFormat != null && !(TableColumn.TextFormat is StiPercentageFormatService))
		{
			TableColumn.TextFormat = new StiPercentageFormatService();
			Designer.InvokeRefreshDesigner();
		}
	}

	private static void CheckMeasureMeterTextFormat(StiTableColumn meter)
	{
		if (!(meter.TextFormat is StiGeneralFormatService) && !(meter.TextFormat is StiCurrencyFormatService) && !(meter.TextFormat is StiNumberFormatService) && !(meter.TextFormat is StiPercentageFormatService))
		{
			string text = Funcs.ToLowerCase(StiExpressionHelper.GetFunction(meter.Expression));
			StiNumberFormatService stiNumberFormatService = new StiNumberFormatService();
			if (text == "count" || text == "countdiscount")
			{
				stiNumberFormatService.State |= Stimulsoft.Report.Components.StiTextFormatState.DecimalDigits;
				stiNumberFormatService.DecimalDigits = 0;
			}
			meter.TextFormat = stiNumberFormatService;
		}
	}

	protected override void OnGlobalElementChanged(object sender, EventArgs e)
	{
		if (sender == base.Element)
		{
			IEnumerable<StiTableColumn> second = from t in listBoxColumns.ToItems()
				select t.Meter as StiTableColumn into c
				where c != null
				select c;
			IEnumerable<StiTableColumn> dif = TableElement.Columns.Except(second);
			ReadPropertiesFromElement();
			listBoxColumns.SelectedMeterItem = listBoxColumns.ToItems().FirstOrDefault((StiMeterItem t) => t.Meter == dif.FirstOrDefault());
		}
	}

	private void ButtonExpressionEdit_Click(object sender, EventArgs e)
	{
		if (StiExpressionEditorHelper.RunExpressionEditor(base.Element, ref textBoxExpression))
		{
			RefreshElementView();
		}
	}

	private void ButtonMeter_CheckedChanged(object sender, EventArgs e)
	{
		if (base.LockRefresh || TableColumn == null)
		{
			return;
		}
		try
		{
			if (((RadioButton)buttonDimension).Checked)
			{
				SwitchTextFormatFromPercentFromGrandTotal();
				listBoxColumns.SelectedMeterItem.Meter = StiMeterHelper.Table.GetDimension(TableColumn);
			}
			else if (((RadioButton)buttonMeasure).Checked)
			{
				listBoxColumns.SelectedMeterItem.Meter = StiMeterHelper.Table.GetMeasure(TableColumn, TableElement.Dashboard);
				StiTableColumn stiTableColumn = listBoxColumns.SelectedMeterItem.Meter as StiTableColumn;
				stiTableColumn.HorAlignment = StiHorAlignment.Right;
				CheckMeasureMeterTextFormat(stiTableColumn);
			}
			else if (((RadioButton)buttonDataBars).Checked)
			{
				listBoxColumns.SelectedMeterItem.Meter = StiMeterHelper.Table.GetDataBars(TableColumn, TableElement.Dashboard);
			}
			else if (((RadioButton)buttonColorScale).Checked)
			{
				((StiTableColumn)listBoxColumns.SelectedMeterItem.Meter).HorAlignment = StiHorAlignment.Right;
				listBoxColumns.SelectedMeterItem.Meter = StiMeterHelper.Table.GetColorScale(TableColumn, TableElement.Dashboard);
			}
			else if (((RadioButton)buttonIndicator).Checked)
			{
				listBoxColumns.SelectedMeterItem.Meter = StiMeterHelper.Table.GetIndicator(TableColumn, TableElement.Dashboard);
			}
			else if (((RadioButton)buttonSparklines).Checked)
			{
				SwitchTextFormatFromPercentFromGrandTotal();
				listBoxColumns.SelectedMeterItem.Meter = StiMeterHelper.Table.GetSparklines(TableColumn);
			}
			else if (((RadioButton)buttonBubble).Checked)
			{
				listBoxColumns.SelectedMeterItem.Meter = StiMeterHelper.Table.GetBubble(TableColumn, TableElement.Dashboard);
			}
		}
		catch
		{
		}
		StiTableElementSelection.Select(TableElement, TableColumn);
		RefreshNodeImage();
		RefreshElementView();
		WritePropertiesToElement();
		RefreshUiPanel();
	}

	private void SwitchTextFormatFromPercentFromGrandTotal()
	{
		if (StiExpressionHelper.GetFunction(TableColumn.Expression)?.ToLowerInvariant() == "percentofgrandtotal" && TableColumn.TextFormat is StiPercentageFormatService)
		{
			TableColumn.TextFormat = new StiNumberFormatService();
		}
	}

	private void ButtonSparklines_CheckedChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && TableColumn is StiSparklinesColumn stiSparklinesColumn)
		{
			if (((RadioButton)buttonSparklinesLine).Checked)
			{
				stiSparklinesColumn.Type = StiSparklinesType.Line;
			}
			else if (((RadioButton)buttonSparklinesArea).Checked)
			{
				stiSparklinesColumn.Type = StiSparklinesType.Area;
			}
			else if (((RadioButton)buttonSparklinesColumn).Checked)
			{
				stiSparklinesColumn.Type = StiSparklinesType.Column;
			}
			else if (((RadioButton)buttonSparklinesWinLoss).Checked)
			{
				stiSparklinesColumn.Type = StiSparklinesType.WinLoss;
			}
			RefreshElementView();
			RefreshNodeImage();
			RefreshUiPanel();
		}
	}

	private void CheckBoxHighLowPoints_CheckedChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && TableColumn is StiSparklinesColumn stiSparklinesColumn)
		{
			stiSparklinesColumn.ShowHighLowPoints = ((CheckBox)checkBoxHighLowPoints).Checked;
			RefreshElementView();
		}
	}

	private void CheckBoxFirstLastPoints_CheckedChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && TableColumn is StiSparklinesColumn stiSparklinesColumn)
		{
			stiSparklinesColumn.ShowFirstLastPoints = ((CheckBox)checkBoxFirstLastPoints).Checked;
			RefreshElementView();
		}
	}

	private void CheckBoxShowTotals_CheckedChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && TableColumn != null)
		{
			TableColumn.ShowTotalSummary = ((CheckBox)checkBoxShowTotal).Checked;
			((Control)panelSummaryType).Visible = ((CheckBox)checkBoxShowTotal).Checked;
			RefreshUiPanel();
			RefreshElementView();
		}
	}

	private void TextBoxHyperlinkPattern_TextChanged(object sender, EventArgs e)
	{
		if (TableColumn is StiDimensionColumn stiDimensionColumn)
		{
			stiDimensionColumn.HyperlinkPattern = ((Control)textBoxHyperlinkPattern).Text;
		}
	}

	private void ButtonEditHyperlinkPattern_Click(object sender, EventArgs e)
	{
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Invalid comparison between Unknown and I4
		StiExpressionEditorForm stiExpressionEditorForm = new StiExpressionEditorForm(((Control)textBoxHyperlinkPattern).Text, base.Element?.Report);
		try
		{
			stiExpressionEditorForm.Idents = new List<string> { "Value" };
			StiTableElement stiTableElement = base.Element as StiTableElement;
			foreach (StiTableColumn column in stiTableElement.Columns)
			{
				string label = StiLabelHelper.GetLabel(column);
				string item = "Row." + StiCodeDomSerializator.ReplaceSymbols(label);
				stiExpressionEditorForm.Idents.Add(item);
			}
			if ((int)((Form)stiExpressionEditorForm).ShowDialog() == 1)
			{
				((Control)textBoxHyperlinkPattern).Text = stiExpressionEditorForm.CodeEditorText;
				((Control)textBoxHyperlinkPattern).Focus();
				((TextBoxBase)textBoxHyperlinkPattern).Select(0, 0);
			}
		}
		finally
		{
			((IDisposable)stiExpressionEditorForm)?.Dispose();
		}
	}

	private void CheckBoxShowHyperlink_CheckedChanged(object sender, EventArgs e)
	{
		if (TableColumn is StiDimensionColumn stiDimensionColumn)
		{
			stiDimensionColumn.ShowHyperlink = ((CheckBox)checkBoxShowHyperlink).Checked;
		}
		RefreshUiPanel();
		if (((CheckBox)checkBoxShowHyperlink).Checked)
		{
			((Control)textBoxHyperlinkPattern).Focus();
		}
		RefreshElementView();
	}

	private void ButtonEditInteraction_Click(object sender, EventArgs e)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Invalid comparison between Unknown and I4
		if (TableColumn.DashboardInteraction == null)
		{
			TableColumn.DashboardInteraction = new StiTableColumnDashboardInteraction();
		}
		StiElementInteractionEditorForm stiElementInteractionEditorForm = new StiElementInteractionEditorForm(TableElement, TableColumn.DashboardInteraction as StiDashboardInteraction);
		try
		{
			stiElementInteractionEditorForm.LockInplaceEditorClosing = true;
			if ((int)((Form)stiElementInteractionEditorForm).ShowDialog() == 1)
			{
				TableColumn.DashboardInteraction = stiElementInteractionEditorForm.Interaction;
				RefreshUiPanel();
			}
		}
		finally
		{
			((IDisposable)stiElementInteractionEditorForm)?.Dispose();
		}
	}

	private void ButtonSetDefaultInteraction_Click(object sender, EventArgs e)
	{
		TableColumn.DashboardInteraction = new StiTableColumnDashboardInteraction();
		RefreshUiPanel();
	}

	private void ComboBoxSummaryType_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			switch (((ComboBox)comboBoxSummaryType).SelectedItem as string)
			{
			case "Sum":
				TableColumn.SummaryType = StiSummaryColumnType.Sum;
				break;
			case "Min":
				TableColumn.SummaryType = StiSummaryColumnType.Min;
				break;
			case "Max":
				TableColumn.SummaryType = StiSummaryColumnType.Max;
				break;
			case "Count":
				TableColumn.SummaryType = StiSummaryColumnType.Count;
				break;
			case "Average":
				TableColumn.SummaryType = StiSummaryColumnType.Average;
				break;
			}
			RefreshElementView();
			RefreshUiPanel();
		}
	}

	private void ColorBoxMinimum_SelectedColorChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && ((RadioButton)buttonColorScale).Checked)
		{
			StiColorScaleColumn stiColorScaleColumn = TableColumn as StiColorScaleColumn;
			stiColorScaleColumn.MinimumColor = colorBoxMinimum.SelectedColor;
			RefreshElementView();
			RefreshUiPanel();
		}
	}

	private void ColorBoxMaximum_SelectedColorChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && ((RadioButton)buttonColorScale).Checked)
		{
			StiColorScaleColumn stiColorScaleColumn = TableColumn as StiColorScaleColumn;
			stiColorScaleColumn.MaximumColor = colorBoxMaximum.SelectedColor;
			RefreshElementView();
			RefreshUiPanel();
		}
	}

	private void ColorBoxColors_SelectedColorChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			if (TableColumn is StiSparklinesColumn stiSparklinesColumn)
			{
				stiSparklinesColumn.PositiveColor = colorBoxPositiveColor.SelectedColor;
				stiSparklinesColumn.NegativeColor = colorBoxNegativeColor.SelectedColor;
			}
			if (TableColumn is StiBubbleColumn stiBubbleColumn)
			{
				stiBubbleColumn.PositiveColor = colorBoxPositiveColor.SelectedColor;
				stiBubbleColumn.NegativeColor = colorBoxNegativeColor.SelectedColor;
			}
			RefreshElementView();
			RefreshUiPanel();
		}
	}

	private void ComboBoxColorMode_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			if (TableColumn is StiSparklinesColumn stiSparklinesColumn)
			{
				stiSparklinesColumn.AllowCustomColors = ((ComboBox)comboBoxColorMode).SelectedItem?.ToString() == LocColorModeCustom;
				RefreshUiPanel();
				RefreshElementView();
			}
			if (TableColumn is StiBubbleColumn stiBubbleColumn)
			{
				stiBubbleColumn.AllowCustomColors = ((ComboBox)comboBoxColorMode).SelectedItem?.ToString() == LocColorModeCustom;
				RefreshUiPanel();
				RefreshElementView();
			}
		}
	}

	private void ButtonEditVisibleExpression_Click(object sender, EventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Invalid comparison between Unknown and I4
		StiExpressionEditorForm stiExpressionEditorForm = new StiExpressionEditorForm(((Control)textBoxVisibleExpression).Text, base.Element?.Report);
		try
		{
			if ((int)((Form)stiExpressionEditorForm).ShowDialog() == 1)
			{
				((Control)textBoxVisibleExpression).Text = stiExpressionEditorForm.CodeEditorText;
				((Control)textBoxVisibleExpression).Focus();
				((TextBoxBase)textBoxVisibleExpression).Select(0, 0);
			}
		}
		finally
		{
			((IDisposable)stiExpressionEditorForm)?.Dispose();
		}
	}

	private void ComboBoxVisible_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && TableColumn != null)
		{
			if (((ComboBox)comboBoxVisible).SelectedItem?.ToString() == LocVisibleTrue)
			{
				TableColumn.Visibility = StiTableColumnVisibility.True;
			}
			else if (((ComboBox)comboBoxVisible).SelectedItem?.ToString() == LocVisibleFalse)
			{
				TableColumn.Visibility = StiTableColumnVisibility.False;
			}
			else
			{
				TableColumn.Visibility = StiTableColumnVisibility.Expression;
			}
			RefreshUiPanel();
			RefreshElementView();
		}
	}

	private void TextBoxVisibleExpression_TextChanged(object sender, EventArgs e)
	{
		if (TableColumn != null)
		{
			TableColumn.VisibilityExpression = ((Control)textBoxVisibleExpression).Text;
		}
	}

	private void TextBoxVisibleExpression_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 13)
		{
			RefreshElementView();
		}
	}

	private void LabelExpressionSample2_MouseDown(object sender, MouseEventArgs e)
	{
		((Control)textBoxHyperlinkPattern).Text = ((Control)labelExpressionSample2).Text;
	}

	private void TextBoxExpression_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 13)
		{
			RefreshElementView();
		}
	}

	private void TextBoxExpression_Leave(object sender, EventArgs e)
	{
		RefreshElementView();
	}

	private void TextBoxExpression_TextChanged(object sender, EventArgs e)
	{
		if (TableColumn != null)
		{
			TableColumn.Expression = ((Control)textBoxExpression).Text;
		}
		listBoxColumns.SetSelectedText(StiLabelHelper.GetLabel(TableColumn));
		RefreshExpressionHint();
	}

	private void ButtonExpressionMenu_Click(object sender, EventArgs e)
	{
		new StiTableExpressionMenu(menuExpression, (TextBox)(object)textBoxExpression, this, listBoxColumns).CreateItems();
		Point point = new Point(((Control)panelBoxExpression).Width, ((Control)panelBoxExpression).Height + StiScale.I1);
		if (StiLeftRightHandedHelper.IsLeftHanded)
		{
			point.X -= menuExpression.PopupSize.Width;
		}
		point = ((Control)panelBoxExpression).PointToScreen(point);
		menuExpression.Popup(point);
	}

	private void StiTableElementClickHelper_TableHeaderClick(object sender, EventArgs e)
	{
		if (((sender is StiTableElement stiTableElement) ? stiTableElement.Key : null) == TableElement.Key)
		{
			CheckSelectedColumns();
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		StiTableElementClickHelper.TableHeaderClick += StiTableElementClickHelper_TableHeaderClick;
	}

	protected override void OnClosed(EventArgs e)
	{
		((Form)this).OnClosed(e);
		StiTableElementClickHelper.TableHeaderClick -= StiTableElementClickHelper_TableHeaderClick;
		StiTableElementSelection.ResetSelection(TableElement);
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/dashboards_table.htm");
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		((Control)this).OnKeyDown(e);
		if ((int)e.KeyCode == 115)
		{
			StiPropertiesPanelHelper.ActivatePanel(Designer);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Unknown result type (might be due to invalid IL or missing references)
		//IL_061a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0624: Expected O, but got Unknown
		//IL_0642: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d7: Expected O, but got Unknown
		//IL_06f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_076f: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0868: Unknown result type (might be due to invalid IL or missing references)
		//IL_0872: Expected O, but got Unknown
		//IL_0890: Unknown result type (might be due to invalid IL or missing references)
		//IL_091b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0925: Expected O, but got Unknown
		//IL_0943: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d8: Expected O, but got Unknown
		//IL_09f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8b: Expected O, but got Unknown
		//IL_0aa6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b47: Expected O, but got Unknown
		//IL_0b62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fef: Unknown result type (might be due to invalid IL or missing references)
		//IL_1060: Unknown result type (might be due to invalid IL or missing references)
		//IL_1152: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b0: Expected O, but got Unknown
		//IL_1242: Unknown result type (might be due to invalid IL or missing references)
		//IL_124c: Expected O, but got Unknown
		//IL_12c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_12cb: Expected O, but got Unknown
		//IL_1405: Unknown result type (might be due to invalid IL or missing references)
		//IL_1478: Unknown result type (might be due to invalid IL or missing references)
		//IL_1482: Expected O, but got Unknown
		//IL_14bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_15b7: Expected O, but got Unknown
		//IL_15d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1660: Unknown result type (might be due to invalid IL or missing references)
		//IL_166a: Expected O, but got Unknown
		//IL_1688: Unknown result type (might be due to invalid IL or missing references)
		//IL_1713: Unknown result type (might be due to invalid IL or missing references)
		//IL_171d: Expected O, but got Unknown
		//IL_1738: Unknown result type (might be due to invalid IL or missing references)
		//IL_17cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_17d9: Expected O, but got Unknown
		//IL_17f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1884: Unknown result type (might be due to invalid IL or missing references)
		//IL_1928: Unknown result type (might be due to invalid IL or missing references)
		//IL_19b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aa4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1af9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b03: Expected O, but got Unknown
		//IL_1b24: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c10: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d68: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e86: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ee4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f58: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2082: Unknown result type (might be due to invalid IL or missing references)
		//IL_20d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_20e1: Expected O, but got Unknown
		//IL_2101: Unknown result type (might be due to invalid IL or missing references)
		//IL_21af: Unknown result type (might be due to invalid IL or missing references)
		//IL_22a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_231c: Unknown result type (might be due to invalid IL or missing references)
		//IL_237d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2387: Expected O, but got Unknown
		//IL_23bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_23c6: Expected O, but got Unknown
		//IL_23e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_24b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2513: Unknown result type (might be due to invalid IL or missing references)
		//IL_251d: Expected O, but got Unknown
		//IL_253a: Unknown result type (might be due to invalid IL or missing references)
		//IL_25d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2636: Unknown result type (might be due to invalid IL or missing references)
		//IL_2640: Expected O, but got Unknown
		//IL_2681: Unknown result type (might be due to invalid IL or missing references)
		//IL_271d: Unknown result type (might be due to invalid IL or missing references)
		//IL_27b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_282c: Unknown result type (might be due to invalid IL or missing references)
		//IL_289e: Unknown result type (might be due to invalid IL or missing references)
		//IL_28a8: Expected O, but got Unknown
		//IL_28c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2959: Unknown result type (might be due to invalid IL or missing references)
		//IL_29ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a85: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b19: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bf0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c55: Expected O, but got Unknown
		//IL_2c72: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d58: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d62: Expected O, but got Unknown
		//IL_2d7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f4c: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiTableElementEditorForm));
		toolTip = new ToolTip(components);
		panelUi = new StiPanelUX();
		comboBoxVisible = new StiComboBox();
		buttonBubble = new StiOperationRadioButton();
		buttonIndicator = new StiOperationRadioButton();
		labelVisible = new StiLabelUX();
		labelType = new StiLabelUX();
		buttonColorScale = new StiOperationRadioButton();
		buttonSparklines = new StiOperationRadioButton();
		buttonDataBars = new StiOperationRadioButton();
		buttonMeasure = new StiOperationRadioButton();
		buttonDimension = new StiOperationRadioButton();
		panelVisibility = new StiPanelUX();
		panelShowTotals = new StiPanelUX();
		checkBoxShowTotal = new StiCheckBoxUX();
		panelSummaryType = new StiPanelUX();
		comboBoxSummaryType = new StiComboBox();
		labelSummaryType = new StiLabelUX();
		labelExpression = new StiLabelUX();
		panelShowHyperlink = new StiPanelUX();
		checkBoxShowHyperlink = new StiCheckBoxUX();
		panelHyperlink = new StiPanelUX();
		labelHyperlinkSample = new StiLabelUX();
		labelExpressionSample2 = new StiSampleLabelUX();
		labelHyperlinkPattern = new StiLabelUX();
		panelSparklines = new StiPanelUX();
		contextMenuBar = new ContextMenuBar();
		menuExpression = new ButtonItem();
		buttonSparklinesWinLoss = new StiOperationRadioButton();
		buttonSparklinesColumn = new StiOperationRadioButton();
		buttonSparklinesArea = new StiOperationRadioButton();
		buttonSparklinesLine = new StiOperationRadioButton();
		checkBoxFirstLastPoints = new StiCheckBoxUX();
		checkBoxHighLowPoints = new StiCheckBoxUX();
		labelSparklines = new StiLabelUX();
		panelColorScaleUI = new StiPanelUX();
		labelMinimumMaximumSeparator = new StiLabelUX();
		labelMaximum = new StiLabelUX();
		labelMinimum = new StiLabelUX();
		panelColorsMode = new StiPanelUX();
		comboBoxColorMode = new StiComboBox();
		labelColors = new StiLabelUX();
		panelColorsLabels = new StiPanelUX();
		labelNegative = new StiLabelUX();
		labelPositive = new StiLabelUX();
		panelColorsEmpty = new StiPanelUX();
		panelColors = new StiPanelUX();
		labelPositiveNegativeSeparator = new StiLabelUX();
		panelBlockInteraction = new StiPanelUX();
		labelInteraction = new StiLabelUX();
		panelVisibleExpression = new StiPanelBox();
		textBoxVisibleExpression = new HintTextBox();
		buttonEditVisibleExpression = new StiOperationButton();
		panelBoxExpression = new StiPanelBox();
		buttonExpressionEdit = new StiOperationButton();
		textBoxExpression = new HintTextBox();
		buttonExpressionMenu = new StiOperationButton();
		listBoxColumns = new StiMeterListBox();
		panelBoxHyperlinkPattern = new StiPanelBox();
		textBoxHyperlinkPattern = new HintTextBox();
		buttonEditHyperlink = new StiOperationButton();
		colorBoxMaximum = new StiColorBoxXF();
		colorBoxMinimum = new StiColorBoxXF();
		colorBoxNegativeColor = new StiColorBoxXF();
		colorBoxPositiveColor = new StiColorBoxXF();
		panelInteraction = new StiPanelBox();
		buttonSetDefaultInteraction = new StiOperationButton();
		labelInteractionStatus = new StiLabelUX();
		buttonEditInteraction = new StiOperationButton();
		((Control)panelUi).SuspendLayout();
		((Control)panelVisibility).SuspendLayout();
		((Control)panelShowTotals).SuspendLayout();
		((Control)panelSummaryType).SuspendLayout();
		((Control)panelShowHyperlink).SuspendLayout();
		((Control)panelHyperlink).SuspendLayout();
		((Control)panelSparklines).SuspendLayout();
		((ISupportInitialize)contextMenuBar).BeginInit();
		((Control)panelColorScaleUI).SuspendLayout();
		((Control)panelColorsMode).SuspendLayout();
		((Control)panelColorsLabels).SuspendLayout();
		((Control)panelColors).SuspendLayout();
		((Control)panelBlockInteraction).SuspendLayout();
		((Control)panelVisibleExpression).SuspendLayout();
		((Control)panelBoxExpression).SuspendLayout();
		((Control)panelBoxHyperlinkPattern).SuspendLayout();
		((Control)panelInteraction).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panelUi).Controls.Add((Control)(object)comboBoxVisible);
		((Control)panelUi).Controls.Add((Control)(object)buttonBubble);
		((Control)panelUi).Controls.Add((Control)(object)buttonIndicator);
		((Control)panelUi).Controls.Add((Control)(object)labelVisible);
		((Control)panelUi).Controls.Add((Control)(object)labelType);
		((Control)panelUi).Controls.Add((Control)(object)buttonColorScale);
		((Control)panelUi).Controls.Add((Control)(object)buttonSparklines);
		((Control)panelUi).Controls.Add((Control)(object)buttonDataBars);
		((Control)panelUi).Controls.Add((Control)(object)buttonMeasure);
		((Control)panelUi).Controls.Add((Control)(object)buttonDimension);
		((Control)panelUi).Dock = (DockStyle)2;
		((Control)panelUi).Location = new Point(12, -24);
		((Control)panelUi).Margin = new Padding(2);
		((Control)panelUi).Name = "panelUi";
		((Control)panelUi).Size = new Size(295, 76);
		((Control)panelUi).TabIndex = 3;
		((Control)comboBoxVisible).Anchor = (AnchorStyles)13;
		comboBoxVisible.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxVisible).FormattingEnabled = true;
		((ComboBox)comboBoxVisible).Items.AddRange(new object[5] { "Sum", "Min", "Max", "Count", "Average" });
		((Control)comboBoxVisible).Location = new Point(90, 50);
		((Control)comboBoxVisible).Margin = new Padding(4);
		((Control)comboBoxVisible).Name = "comboBoxVisible";
		((Control)comboBoxVisible).Size = new Size(133, 21);
		((Control)comboBoxVisible).TabIndex = 9;
		((ComboBox)comboBoxVisible).SelectedIndexChanged += ComboBoxVisible_SelectedIndexChanged;
		((RadioButton)buttonBubble).Appearance = (Appearance)1;
		((ButtonBase)buttonBubble).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonBubble).Image = (Image)componentResourceManager.GetObject("buttonBubble.Image");
		((Control)buttonBubble).Location = new Point(269, 12);
		((Control)buttonBubble).Margin = new Padding(2);
		((Control)buttonBubble).Name = "buttonBubble";
		((Control)buttonBubble).Size = new Size(26, 26);
		((Control)buttonBubble).TabIndex = 6;
		((ButtonBase)buttonBubble).UseVisualStyleBackColor = true;
		((RadioButton)buttonBubble).CheckedChanged += ButtonMeter_CheckedChanged;
		((RadioButton)buttonIndicator).Appearance = (Appearance)1;
		((ButtonBase)buttonIndicator).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonIndicator).Image = (Image)componentResourceManager.GetObject("buttonIndicator.Image");
		((Control)buttonIndicator).Location = new Point(209, 12);
		((Control)buttonIndicator).Margin = new Padding(2);
		((Control)buttonIndicator).Name = "buttonIndicator";
		((Control)buttonIndicator).Size = new Size(26, 26);
		((Control)buttonIndicator).TabIndex = 4;
		((ButtonBase)buttonIndicator).UseVisualStyleBackColor = true;
		((RadioButton)buttonIndicator).CheckedChanged += ButtonMeter_CheckedChanged;
		((Control)labelVisible).Location = new Point(0, 49);
		((Control)labelVisible).Margin = new Padding(2, 0, 2, 0);
		((Control)labelVisible).Name = "labelVisible";
		((Control)labelVisible).Size = new Size(82, 26);
		((Control)labelVisible).TabIndex = 1;
		((Control)labelVisible).Text = "Visible";
		((Label)labelVisible).TextAlign = (ContentAlignment)16;
		((Control)labelType).Location = new Point(0, 12);
		((Control)labelType).Margin = new Padding(2, 0, 2, 0);
		((Control)labelType).Name = "labelType";
		((Control)labelType).Size = new Size(82, 26);
		((Control)labelType).TabIndex = 1;
		((Control)labelType).Text = "Type";
		((Label)labelType).TextAlign = (ContentAlignment)16;
		((RadioButton)buttonColorScale).Appearance = (Appearance)1;
		((ButtonBase)buttonColorScale).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonColorScale).Image = (Image)componentResourceManager.GetObject("buttonColorScale.Image");
		((Control)buttonColorScale).Location = new Point(179, 12);
		((Control)buttonColorScale).Margin = new Padding(2);
		((Control)buttonColorScale).Name = "buttonColorScale";
		((Control)buttonColorScale).Size = new Size(26, 26);
		((Control)buttonColorScale).TabIndex = 3;
		((ButtonBase)buttonColorScale).UseVisualStyleBackColor = true;
		((RadioButton)buttonColorScale).CheckedChanged += ButtonMeter_CheckedChanged;
		((RadioButton)buttonSparklines).Appearance = (Appearance)1;
		((ButtonBase)buttonSparklines).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonSparklines).Image = (Image)componentResourceManager.GetObject("buttonSparklines.Image");
		((Control)buttonSparklines).Location = new Point(239, 12);
		((Control)buttonSparklines).Margin = new Padding(2);
		((Control)buttonSparklines).Name = "buttonSparklines";
		((Control)buttonSparklines).Size = new Size(26, 26);
		((Control)buttonSparklines).TabIndex = 5;
		((ButtonBase)buttonSparklines).UseVisualStyleBackColor = true;
		((RadioButton)buttonSparklines).CheckedChanged += ButtonMeter_CheckedChanged;
		((RadioButton)buttonDataBars).Appearance = (Appearance)1;
		((ButtonBase)buttonDataBars).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonDataBars).Image = (Image)componentResourceManager.GetObject("buttonDataBars.Image");
		((Control)buttonDataBars).Location = new Point(149, 12);
		((Control)buttonDataBars).Margin = new Padding(2);
		((Control)buttonDataBars).Name = "buttonDataBars";
		((Control)buttonDataBars).Size = new Size(26, 26);
		((Control)buttonDataBars).TabIndex = 2;
		((ButtonBase)buttonDataBars).UseVisualStyleBackColor = true;
		((RadioButton)buttonDataBars).CheckedChanged += ButtonMeter_CheckedChanged;
		((RadioButton)buttonMeasure).Appearance = (Appearance)1;
		((ButtonBase)buttonMeasure).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonMeasure).Image = (Image)componentResourceManager.GetObject("buttonMeasure.Image");
		((Control)buttonMeasure).Location = new Point(119, 12);
		((Control)buttonMeasure).Margin = new Padding(2);
		((Control)buttonMeasure).Name = "buttonMeasure";
		((Control)buttonMeasure).Size = new Size(26, 26);
		((Control)buttonMeasure).TabIndex = 1;
		((ButtonBase)buttonMeasure).UseVisualStyleBackColor = true;
		((RadioButton)buttonMeasure).CheckedChanged += ButtonMeter_CheckedChanged;
		((RadioButton)buttonDimension).Appearance = (Appearance)1;
		((RadioButton)buttonDimension).Checked = true;
		((ButtonBase)buttonDimension).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonDimension).Image = (Image)componentResourceManager.GetObject("buttonDimension.Image");
		((Control)buttonDimension).Location = new Point(89, 12);
		((Control)buttonDimension).Margin = new Padding(2);
		((Control)buttonDimension).Name = "buttonDimension";
		((Control)buttonDimension).Size = new Size(26, 26);
		((Control)buttonDimension).TabIndex = 0;
		((RadioButton)buttonDimension).TabStop = true;
		((ButtonBase)buttonDimension).UseVisualStyleBackColor = true;
		((RadioButton)buttonDimension).CheckedChanged += ButtonMeter_CheckedChanged;
		((Control)panelVisibility).Controls.Add((Control)(object)panelVisibleExpression);
		((Control)panelVisibility).Dock = (DockStyle)2;
		((Control)panelVisibility).Location = new Point(12, 52);
		((Control)panelVisibility).Margin = new Padding(2);
		((Control)panelVisibility).Name = "panelVisibility";
		((Control)panelVisibility).Size = new Size(295, 40);
		((Control)panelVisibility).TabIndex = 4;
		((Control)panelShowTotals).Controls.Add((Control)(object)checkBoxShowTotal);
		((Control)panelShowTotals).Dock = (DockStyle)2;
		((Control)panelShowTotals).Location = new Point(12, 92);
		((Control)panelShowTotals).Margin = new Padding(2);
		((Control)panelShowTotals).Name = "panelShowTotals";
		((Control)panelShowTotals).Size = new Size(295, 30);
		((Control)panelShowTotals).TabIndex = 5;
		((Control)checkBoxShowTotal).Anchor = (AnchorStyles)13;
		((ButtonBase)checkBoxShowTotal).AutoEllipsis = true;
		((Control)checkBoxShowTotal).Location = new Point(90, 12);
		((Control)checkBoxShowTotal).Margin = new Padding(2);
		((Control)checkBoxShowTotal).Name = "checkBoxShowTotal";
		((Control)checkBoxShowTotal).Size = new Size(204, 18);
		((Control)checkBoxShowTotal).TabIndex = 0;
		((Control)checkBoxShowTotal).Text = "Show Total";
		((ButtonBase)checkBoxShowTotal).UseVisualStyleBackColor = true;
		((CheckBox)checkBoxShowTotal).CheckedChanged += CheckBoxShowTotals_CheckedChanged;
		((Control)panelSummaryType).Controls.Add((Control)(object)comboBoxSummaryType);
		((Control)panelSummaryType).Controls.Add((Control)(object)labelSummaryType);
		((Control)panelSummaryType).Dock = (DockStyle)2;
		((Control)panelSummaryType).Location = new Point(12, 122);
		((Control)panelSummaryType).Name = "panelSummaryType";
		((Control)panelSummaryType).Size = new Size(295, 38);
		((Control)panelSummaryType).TabIndex = 6;
		((Control)comboBoxSummaryType).Anchor = (AnchorStyles)13;
		comboBoxSummaryType.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxSummaryType).FormattingEnabled = true;
		((ComboBox)comboBoxSummaryType).Items.AddRange(new object[5] { "Sum", "Min", "Max", "Count", "Average" });
		((Control)comboBoxSummaryType).Location = new Point(90, 12);
		((Control)comboBoxSummaryType).Margin = new Padding(4);
		((Control)comboBoxSummaryType).Name = "comboBoxSummaryType";
		((Control)comboBoxSummaryType).Size = new Size(133, 21);
		((Control)comboBoxSummaryType).TabIndex = 1;
		((ComboBox)comboBoxSummaryType).SelectedIndexChanged += ComboBoxSummaryType_SelectedIndexChanged;
		((Control)labelSummaryType).Location = new Point(0, 12);
		((Control)labelSummaryType).Margin = new Padding(2, 0, 2, 0);
		((Control)labelSummaryType).Name = "labelSummaryType";
		((Control)labelSummaryType).Size = new Size(91, 21);
		((Control)labelSummaryType).TabIndex = 0;
		((Control)labelSummaryType).Text = "Summary Type";
		((Label)labelSummaryType).TextAlign = (ContentAlignment)16;
		((Control)labelExpression).Dock = (DockStyle)1;
		((Control)labelExpression).Location = new Point(12, 155);
		((Control)labelExpression).Margin = new Padding(2, 0, 2, 0);
		((Control)labelExpression).Name = "labelExpression";
		((Control)labelExpression).Size = new Size(295, 32);
		((Control)labelExpression).TabIndex = 1;
		((Control)labelExpression).Text = "Expression";
		((Label)labelExpression).TextAlign = (ContentAlignment)16;
		((Control)panelShowHyperlink).Controls.Add((Control)(object)checkBoxShowHyperlink);
		((Control)panelShowHyperlink).Dock = (DockStyle)2;
		((Control)panelShowHyperlink).Location = new Point(12, 160);
		((Control)panelShowHyperlink).Margin = new Padding(2);
		((Control)panelShowHyperlink).Name = "panelShowHyperlink";
		((Control)panelShowHyperlink).Size = new Size(295, 30);
		((Control)panelShowHyperlink).TabIndex = 7;
		((Control)checkBoxShowHyperlink).Anchor = (AnchorStyles)13;
		((ButtonBase)checkBoxShowHyperlink).AutoEllipsis = true;
		((Control)checkBoxShowHyperlink).Location = new Point(90, 12);
		((Control)checkBoxShowHyperlink).Margin = new Padding(2);
		((Control)checkBoxShowHyperlink).Name = "checkBoxShowHyperlink";
		((Control)checkBoxShowHyperlink).Size = new Size(204, 18);
		((Control)checkBoxShowHyperlink).TabIndex = 0;
		((Control)checkBoxShowHyperlink).Text = "Show Hyperlink";
		((ButtonBase)checkBoxShowHyperlink).UseVisualStyleBackColor = true;
		((CheckBox)checkBoxShowHyperlink).CheckedChanged += CheckBoxShowHyperlink_CheckedChanged;
		((Control)panelHyperlink).Controls.Add((Control)(object)labelHyperlinkSample);
		((Control)panelHyperlink).Controls.Add((Control)(object)labelExpressionSample2);
		((Control)panelHyperlink).Controls.Add((Control)(object)panelBoxHyperlinkPattern);
		((Control)panelHyperlink).Controls.Add((Control)(object)labelHyperlinkPattern);
		((Control)panelHyperlink).Dock = (DockStyle)2;
		((Control)panelHyperlink).Location = new Point(12, 190);
		((Control)panelHyperlink).Margin = new Padding(2);
		((Control)panelHyperlink).Name = "panelHyperlink";
		((Control)panelHyperlink).Size = new Size(295, 56);
		((Control)panelHyperlink).TabIndex = 8;
		((Control)labelHyperlinkSample).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelHyperlinkSample).Location = new Point(92, 41);
		((Control)labelHyperlinkSample).Name = "labelHyperlinkSample";
		((Control)labelHyperlinkSample).Size = new Size(46, 13);
		((Control)labelHyperlinkSample).TabIndex = 19;
		((Control)labelHyperlinkSample).Text = "Sample:";
		((Label)labelHyperlinkSample).TextAlign = (ContentAlignment)16;
		((Control)labelExpressionSample2).Anchor = (AnchorStyles)13;
		((Label)labelExpressionSample2).AutoEllipsis = true;
		((Control)labelExpressionSample2).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)labelExpressionSample2).Location = new Point(142, 41);
		((Control)labelExpressionSample2).Name = "labelExpressionSample2";
		((Control)labelExpressionSample2).Size = new Size(150, 13);
		((Control)labelExpressionSample2).TabIndex = 18;
		((Control)labelExpressionSample2).Text = "www.{value}.com";
		((Label)labelExpressionSample2).TextAlign = (ContentAlignment)16;
		((Control)labelExpressionSample2).MouseDown += new MouseEventHandler(LabelExpressionSample2_MouseDown);
		((Control)labelHyperlinkPattern).Location = new Point(0, 15);
		((Control)labelHyperlinkPattern).Name = "labelHyperlinkPattern";
		((Control)labelHyperlinkPattern).Size = new Size(89, 21);
		((Control)labelHyperlinkPattern).TabIndex = 4;
		((Control)labelHyperlinkPattern).Text = "Hyperlink";
		((Label)labelHyperlinkPattern).TextAlign = (ContentAlignment)16;
		((Control)panelSparklines).Controls.Add((Control)(object)contextMenuBar);
		((Control)panelSparklines).Controls.Add((Control)(object)buttonSparklinesWinLoss);
		((Control)panelSparklines).Controls.Add((Control)(object)buttonSparklinesColumn);
		((Control)panelSparklines).Controls.Add((Control)(object)buttonSparklinesArea);
		((Control)panelSparklines).Controls.Add((Control)(object)buttonSparklinesLine);
		((Control)panelSparklines).Controls.Add((Control)(object)checkBoxFirstLastPoints);
		((Control)panelSparklines).Controls.Add((Control)(object)checkBoxHighLowPoints);
		((Control)panelSparklines).Controls.Add((Control)(object)labelSparklines);
		((Control)panelSparklines).Dock = (DockStyle)2;
		((Control)panelSparklines).Location = new Point(12, 246);
		((Control)panelSparklines).Margin = new Padding(2);
		((Control)panelSparklines).Name = "panelSparklines";
		((Control)panelSparklines).Size = new Size(295, 97);
		((Control)panelSparklines).TabIndex = 9;
		contextMenuBar.AntiAlias = true;
		contextMenuBar.DockSide = eDockSide.Bottom;
		contextMenuBar.DockTabStripHeight = 50;
		((Control)contextMenuBar).Font = new Font("Segoe UI", 9f);
		contextMenuBar.Items.AddRange(new BaseItem[1] { menuExpression });
		contextMenuBar.Location = new Point(200, 64);
		((Control)contextMenuBar).Margin = new Padding(2);
		contextMenuBar.Name = "contextMenuBar";
		contextMenuBar.Size = new Size(79, 31);
		contextMenuBar.Stretch = true;
		contextMenuBar.Style = eDotNetBarStyle.StyleManagerControlled;
		contextMenuBar.TabIndex = 7;
		contextMenuBar.TabStop = false;
		((Control)contextMenuBar).Text = "contextMenuBar";
		contextMenuBar.WrapItemsDock = true;
		menuExpression.AutoExpandOnClick = true;
		menuExpression.ImagePaddingHorizontal = 16;
		menuExpression.ImagePaddingVertical = 12;
		menuExpression.Name = "menuExpression";
		menuExpression.Text = "menuExpression";
		((RadioButton)buttonSparklinesWinLoss).Appearance = (Appearance)1;
		((ButtonBase)buttonSparklinesWinLoss).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonSparklinesWinLoss).Image = (Image)componentResourceManager.GetObject("buttonSparklinesWinLoss.Image");
		((Control)buttonSparklinesWinLoss).Location = new Point(179, 12);
		((Control)buttonSparklinesWinLoss).Margin = new Padding(2);
		((Control)buttonSparklinesWinLoss).Name = "buttonSparklinesWinLoss";
		((Control)buttonSparklinesWinLoss).Size = new Size(26, 26);
		((Control)buttonSparklinesWinLoss).TabIndex = 3;
		((ButtonBase)buttonSparklinesWinLoss).UseVisualStyleBackColor = true;
		((RadioButton)buttonSparklinesWinLoss).CheckedChanged += ButtonSparklines_CheckedChanged;
		((RadioButton)buttonSparklinesColumn).Appearance = (Appearance)1;
		((ButtonBase)buttonSparklinesColumn).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonSparklinesColumn).Image = (Image)componentResourceManager.GetObject("buttonSparklinesColumn.Image");
		((Control)buttonSparklinesColumn).Location = new Point(149, 12);
		((Control)buttonSparklinesColumn).Margin = new Padding(2);
		((Control)buttonSparklinesColumn).Name = "buttonSparklinesColumn";
		((Control)buttonSparklinesColumn).Size = new Size(26, 26);
		((Control)buttonSparklinesColumn).TabIndex = 2;
		((ButtonBase)buttonSparklinesColumn).UseVisualStyleBackColor = true;
		((RadioButton)buttonSparklinesColumn).CheckedChanged += ButtonSparklines_CheckedChanged;
		((RadioButton)buttonSparklinesArea).Appearance = (Appearance)1;
		((ButtonBase)buttonSparklinesArea).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonSparklinesArea).Image = (Image)componentResourceManager.GetObject("buttonSparklinesArea.Image");
		((Control)buttonSparklinesArea).Location = new Point(119, 12);
		((Control)buttonSparklinesArea).Margin = new Padding(2);
		((Control)buttonSparklinesArea).Name = "buttonSparklinesArea";
		((Control)buttonSparklinesArea).Size = new Size(26, 26);
		((Control)buttonSparklinesArea).TabIndex = 1;
		((ButtonBase)buttonSparklinesArea).UseVisualStyleBackColor = true;
		((RadioButton)buttonSparklinesArea).CheckedChanged += ButtonSparklines_CheckedChanged;
		((RadioButton)buttonSparklinesLine).Appearance = (Appearance)1;
		((RadioButton)buttonSparklinesLine).Checked = true;
		((ButtonBase)buttonSparklinesLine).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonSparklinesLine).Image = (Image)componentResourceManager.GetObject("buttonSparklinesLine.Image");
		((Control)buttonSparklinesLine).Location = new Point(89, 12);
		((Control)buttonSparklinesLine).Margin = new Padding(2);
		((Control)buttonSparklinesLine).Name = "buttonSparklinesLine";
		((Control)buttonSparklinesLine).Size = new Size(26, 26);
		((Control)buttonSparklinesLine).TabIndex = 0;
		((RadioButton)buttonSparklinesLine).TabStop = true;
		((ButtonBase)buttonSparklinesLine).UseVisualStyleBackColor = true;
		((RadioButton)buttonSparklinesLine).CheckedChanged += ButtonSparklines_CheckedChanged;
		((ButtonBase)checkBoxFirstLastPoints).AutoEllipsis = true;
		((Control)checkBoxFirstLastPoints).Location = new Point(90, 77);
		((Control)checkBoxFirstLastPoints).Margin = new Padding(2);
		((Control)checkBoxFirstLastPoints).Name = "checkBoxFirstLastPoints";
		((Control)checkBoxFirstLastPoints).Size = new Size(202, 18);
		((Control)checkBoxFirstLastPoints).TabIndex = 5;
		((Control)checkBoxFirstLastPoints).Text = "First / Last Points";
		((ButtonBase)checkBoxFirstLastPoints).UseVisualStyleBackColor = true;
		((CheckBox)checkBoxFirstLastPoints).CheckedChanged += CheckBoxFirstLastPoints_CheckedChanged;
		((Control)checkBoxHighLowPoints).Anchor = (AnchorStyles)13;
		((ButtonBase)checkBoxHighLowPoints).AutoEllipsis = true;
		((Control)checkBoxHighLowPoints).Location = new Point(90, 49);
		((Control)checkBoxHighLowPoints).Margin = new Padding(2);
		((Control)checkBoxHighLowPoints).Name = "checkBoxHighLowPoints";
		((Control)checkBoxHighLowPoints).Size = new Size(204, 18);
		((Control)checkBoxHighLowPoints).TabIndex = 4;
		((Control)checkBoxHighLowPoints).Text = "High / Low Points";
		((ButtonBase)checkBoxHighLowPoints).UseVisualStyleBackColor = true;
		((CheckBox)checkBoxHighLowPoints).CheckedChanged += CheckBoxHighLowPoints_CheckedChanged;
		((Control)labelSparklines).Location = new Point(0, 12);
		((Control)labelSparklines).Margin = new Padding(2, 0, 2, 0);
		((Control)labelSparklines).Name = "labelSparklines";
		((Control)labelSparklines).Size = new Size(82, 26);
		((Control)labelSparklines).TabIndex = 1;
		((Control)labelSparklines).Text = "Sparklines";
		((Label)labelSparklines).TextAlign = (ContentAlignment)16;
		((Control)panelColorScaleUI).Controls.Add((Control)(object)labelMinimumMaximumSeparator);
		((Control)panelColorScaleUI).Controls.Add((Control)(object)labelMaximum);
		((Control)panelColorScaleUI).Controls.Add((Control)(object)labelMinimum);
		((Control)panelColorScaleUI).Controls.Add((Control)(object)colorBoxMaximum);
		((Control)panelColorScaleUI).Controls.Add((Control)(object)colorBoxMinimum);
		((Control)panelColorScaleUI).Dock = (DockStyle)2;
		((Control)panelColorScaleUI).Location = new Point(12, 343);
		((Control)panelColorScaleUI).Margin = new Padding(2);
		((Control)panelColorScaleUI).Name = "panelColorScaleUI";
		((Control)panelColorScaleUI).Size = new Size(295, 58);
		((Control)panelColorScaleUI).TabIndex = 10;
		((Control)labelMinimumMaximumSeparator).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelMinimumMaximumSeparator).Location = new Point(186, 32);
		((Control)labelMinimumMaximumSeparator).Margin = new Padding(2, 0, 2, 0);
		((Control)labelMinimumMaximumSeparator).Name = "labelMinimumMaximumSeparator";
		((Control)labelMinimumMaximumSeparator).Size = new Size(13, 20);
		((Control)labelMinimumMaximumSeparator).TabIndex = 28;
		((Control)labelMinimumMaximumSeparator).Text = "-";
		((Label)labelMinimumMaximumSeparator).TextAlign = (ContentAlignment)32;
		((Control)labelMaximum).Location = new Point(205, 8);
		((Control)labelMaximum).Margin = new Padding(2, 0, 2, 0);
		((Control)labelMaximum).Name = "labelMaximum";
		((Control)labelMaximum).Size = new Size(83, 17);
		((Control)labelMaximum).TabIndex = 2;
		((Control)labelMaximum).Text = "Maximum";
		((Label)labelMaximum).TextAlign = (ContentAlignment)16;
		((Control)labelMinimum).Location = new Point(90, 8);
		((Control)labelMinimum).Margin = new Padding(2, 0, 2, 0);
		((Control)labelMinimum).Name = "labelMinimum";
		((Control)labelMinimum).Size = new Size(83, 17);
		((Control)labelMinimum).TabIndex = 0;
		((Control)labelMinimum).Text = "Minimum";
		((Label)labelMinimum).TextAlign = (ContentAlignment)16;
		((Control)panelColorsMode).Controls.Add((Control)(object)comboBoxColorMode);
		((Control)panelColorsMode).Controls.Add((Control)(object)labelColors);
		((Control)panelColorsMode).Dock = (DockStyle)2;
		((Control)panelColorsMode).Location = new Point(12, 401);
		((Control)panelColorsMode).Name = "panelColorsMode";
		((Control)panelColorsMode).Size = new Size(295, 38);
		((Control)panelColorsMode).TabIndex = 11;
		((Control)comboBoxColorMode).Anchor = (AnchorStyles)13;
		comboBoxColorMode.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxColorMode).FormattingEnabled = true;
		((ComboBox)comboBoxColorMode).Items.AddRange(new object[5] { "Sum", "Min", "Max", "Count", "Average" });
		((Control)comboBoxColorMode).Location = new Point(90, 12);
		((Control)comboBoxColorMode).Margin = new Padding(4);
		((Control)comboBoxColorMode).Name = "comboBoxColorMode";
		((Control)comboBoxColorMode).Size = new Size(133, 21);
		((Control)comboBoxColorMode).TabIndex = 1;
		((ComboBox)comboBoxColorMode).SelectedIndexChanged += ComboBoxColorMode_SelectedIndexChanged;
		((Control)labelColors).Location = new Point(0, 14);
		((Control)labelColors).Margin = new Padding(2, 0, 2, 0);
		((Control)labelColors).Name = "labelColors";
		((Control)labelColors).Size = new Size(84, 21);
		((Control)labelColors).TabIndex = 0;
		((Control)labelColors).Text = "Colors";
		((Label)labelColors).TextAlign = (ContentAlignment)16;
		((Control)panelColorsLabels).Controls.Add((Control)(object)labelNegative);
		((Control)panelColorsLabels).Controls.Add((Control)(object)labelPositive);
		((Control)panelColorsLabels).Dock = (DockStyle)2;
		((Control)panelColorsLabels).Location = new Point(12, 439);
		((Control)panelColorsLabels).Margin = new Padding(2);
		((Control)panelColorsLabels).Name = "panelColorsLabels";
		((Control)panelColorsLabels).Size = new Size(295, 26);
		((Control)panelColorsLabels).TabIndex = 12;
		((Control)labelNegative).Location = new Point(205, 7);
		((Control)labelNegative).Margin = new Padding(2, 0, 2, 0);
		((Control)labelNegative).Name = "labelNegative";
		((Control)labelNegative).Size = new Size(84, 17);
		((Control)labelNegative).TabIndex = 4;
		((Control)labelNegative).Text = "Negative";
		((Label)labelNegative).TextAlign = (ContentAlignment)16;
		((Control)labelPositive).Location = new Point(90, 7);
		((Control)labelPositive).Margin = new Padding(2, 0, 2, 0);
		((Control)labelPositive).Name = "labelPositive";
		((Control)labelPositive).Size = new Size(83, 17);
		((Control)labelPositive).TabIndex = 3;
		((Control)labelPositive).Text = "Positive";
		((Label)labelPositive).TextAlign = (ContentAlignment)16;
		((Control)panelColorsEmpty).Dock = (DockStyle)2;
		((Control)panelColorsEmpty).Location = new Point(12, 465);
		((Control)panelColorsEmpty).Margin = new Padding(2);
		((Control)panelColorsEmpty).Name = "panelColorsEmpty";
		((Control)panelColorsEmpty).Size = new Size(295, 8);
		((Control)panelColorsEmpty).TabIndex = 15;
		((Control)panelColors).Controls.Add((Control)(object)labelPositiveNegativeSeparator);
		((Control)panelColors).Controls.Add((Control)(object)colorBoxNegativeColor);
		((Control)panelColors).Controls.Add((Control)(object)colorBoxPositiveColor);
		((Control)panelColors).Dock = (DockStyle)2;
		((Control)panelColors).Location = new Point(12, 473);
		((Control)panelColors).Margin = new Padding(2);
		((Control)panelColors).Name = "panelColors";
		((Control)panelColors).Size = new Size(295, 29);
		((Control)panelColors).TabIndex = 13;
		((Control)labelPositiveNegativeSeparator).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelPositiveNegativeSeparator).Location = new Point(186, 4);
		((Control)labelPositiveNegativeSeparator).Margin = new Padding(2, 0, 2, 0);
		((Control)labelPositiveNegativeSeparator).Name = "labelPositiveNegativeSeparator";
		((Control)labelPositiveNegativeSeparator).Size = new Size(13, 20);
		((Control)labelPositiveNegativeSeparator).TabIndex = 29;
		((Control)labelPositiveNegativeSeparator).Text = "-";
		((Label)labelPositiveNegativeSeparator).TextAlign = (ContentAlignment)32;
		((Control)panelBlockInteraction).Controls.Add((Control)(object)panelInteraction);
		((Control)panelBlockInteraction).Controls.Add((Control)(object)labelInteraction);
		((Control)panelBlockInteraction).Dock = (DockStyle)2;
		((Control)panelBlockInteraction).Location = new Point(12, 502);
		((Control)panelBlockInteraction).Margin = new Padding(2);
		((Control)panelBlockInteraction).Name = "panelBlockInteraction";
		((Control)panelBlockInteraction).Size = new Size(295, 38);
		((Control)panelBlockInteraction).TabIndex = 14;
		((Control)labelInteraction).Location = new Point(0, 14);
		((Control)labelInteraction).Name = "labelInteraction";
		((Control)labelInteraction).Size = new Size(102, 23);
		((Control)labelInteraction).TabIndex = 4;
		((Control)labelInteraction).Text = "Interaction";
		((Label)labelInteraction).TextAlign = (ContentAlignment)16;
		((Control)panelVisibleExpression).Anchor = (AnchorStyles)13;
		((Control)panelVisibleExpression).Controls.Add((Control)(object)textBoxVisibleExpression);
		((Control)panelVisibleExpression).Controls.Add((Control)(object)buttonEditVisibleExpression);
		((Control)panelVisibleExpression).Location = new Point(90, 12);
		((Control)panelVisibleExpression).Margin = new Padding(2);
		((Control)panelVisibleExpression).Name = "panelVisibleExpression";
		((Control)panelVisibleExpression).Size = new Size(205, 25);
		((Control)panelVisibleExpression).TabIndex = 0;
		((Control)textBoxVisibleExpression).AllowDrop = true;
		((Control)textBoxVisibleExpression).Anchor = (AnchorStyles)13;
		((TextBoxBase)textBoxVisibleExpression).BorderStyle = (BorderStyle)0;
		((Control)textBoxVisibleExpression).Location = new Point(4, 6);
		((Control)textBoxVisibleExpression).Margin = new Padding(2);
		((Control)textBoxVisibleExpression).Name = "textBoxVisibleExpression";
		((Control)textBoxVisibleExpression).Size = new Size(174, 13);
		((Control)textBoxVisibleExpression).TabIndex = 0;
		((Control)textBoxVisibleExpression).TextChanged += TextBoxVisibleExpression_TextChanged;
		((Control)textBoxVisibleExpression).KeyDown += new KeyEventHandler(TextBoxVisibleExpression_KeyDown);
		((Control)textBoxVisibleExpression).Leave += TextBoxExpression_Leave;
		((Control)buttonEditVisibleExpression).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonEditVisibleExpression).Image = (Image)componentResourceManager.GetObject("buttonEditVisibleExpression.Image");
		((Control)buttonEditVisibleExpression).Location = new Point(181, 1);
		((Control)buttonEditVisibleExpression).Margin = new Padding(2);
		((Control)buttonEditVisibleExpression).Name = "buttonEditVisibleExpression";
		((Control)buttonEditVisibleExpression).Size = new Size(23, 23);
		((Control)buttonEditVisibleExpression).TabIndex = 1;
		((Control)buttonEditVisibleExpression).TabStop = false;
		((ButtonBase)buttonEditVisibleExpression).UseVisualStyleBackColor = true;
		((Control)buttonEditVisibleExpression).Click += ButtonEditVisibleExpression_Click;
		((Control)panelBoxExpression).Controls.Add((Control)(object)buttonExpressionEdit);
		((Control)panelBoxExpression).Controls.Add((Control)(object)textBoxExpression);
		((Control)panelBoxExpression).Controls.Add((Control)(object)buttonExpressionMenu);
		((Control)panelBoxExpression).Dock = (DockStyle)1;
		((Control)panelBoxExpression).Location = new Point(12, 187);
		((Control)panelBoxExpression).Margin = new Padding(2);
		((Control)panelBoxExpression).Name = "panelBoxExpression";
		((Control)panelBoxExpression).Size = new Size(295, 25);
		((Control)panelBoxExpression).TabIndex = 2;
		((Control)buttonExpressionEdit).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonExpressionEdit).Image = (Image)componentResourceManager.GetObject("buttonExpressionEdit.Image");
		((Control)buttonExpressionEdit).Location = new Point(249, 1);
		((Control)buttonExpressionEdit).Margin = new Padding(2);
		((Control)buttonExpressionEdit).Name = "buttonExpressionEdit";
		((Control)buttonExpressionEdit).Size = new Size(23, 23);
		((Control)buttonExpressionEdit).TabIndex = 1;
		((Control)buttonExpressionEdit).TabStop = false;
		((ButtonBase)buttonExpressionEdit).UseVisualStyleBackColor = true;
		((Control)buttonExpressionEdit).Click += ButtonExpressionEdit_Click;
		((Control)textBoxExpression).Anchor = (AnchorStyles)13;
		((TextBoxBase)textBoxExpression).BorderStyle = (BorderStyle)0;
		((Control)textBoxExpression).Location = new Point(4, 6);
		((Control)textBoxExpression).Margin = new Padding(2);
		((Control)textBoxExpression).Name = "textBoxExpression";
		((Control)textBoxExpression).Size = new Size(240, 13);
		((Control)textBoxExpression).TabIndex = 0;
		((Control)textBoxExpression).TextChanged += TextBoxExpression_TextChanged;
		((Control)textBoxExpression).KeyDown += new KeyEventHandler(TextBoxExpression_KeyDown);
		((Control)textBoxExpression).Leave += TextBoxExpression_Leave;
		((Control)buttonExpressionMenu).Anchor = (AnchorStyles)9;
		((Control)buttonExpressionMenu).Location = new Point(271, 1);
		((Control)buttonExpressionMenu).Margin = new Padding(2);
		((Control)buttonExpressionMenu).Name = "buttonExpressionMenu";
		((Control)buttonExpressionMenu).Size = new Size(23, 23);
		((Control)buttonExpressionMenu).TabIndex = 2;
		((Control)buttonExpressionMenu).TabStop = false;
		((ButtonBase)buttonExpressionMenu).UseVisualStyleBackColor = true;
		((Control)buttonExpressionMenu).Click += ButtonExpressionMenu_Click;
		((Control)listBoxColumns).AllowDrop = true;
		((Control)listBoxColumns).Dock = (DockStyle)1;
		((Control)listBoxColumns).Location = new Point(12, 12);
		((Control)listBoxColumns).Margin = new Padding(2);
		((Control)listBoxColumns).Name = "listBoxColumns";
		((Control)listBoxColumns).Size = new Size(295, 143);
		((Control)listBoxColumns).TabIndex = 0;
		((Control)panelBoxHyperlinkPattern).Anchor = (AnchorStyles)13;
		((Control)panelBoxHyperlinkPattern).Controls.Add((Control)(object)textBoxHyperlinkPattern);
		((Control)panelBoxHyperlinkPattern).Controls.Add((Control)(object)buttonEditHyperlink);
		((Control)panelBoxHyperlinkPattern).Location = new Point(90, 12);
		((Control)panelBoxHyperlinkPattern).Margin = new Padding(2);
		((Control)panelBoxHyperlinkPattern).Name = "panelBoxHyperlinkPattern";
		((Control)panelBoxHyperlinkPattern).Size = new Size(205, 25);
		((Control)panelBoxHyperlinkPattern).TabIndex = 0;
		((Control)textBoxHyperlinkPattern).AllowDrop = true;
		((Control)textBoxHyperlinkPattern).Anchor = (AnchorStyles)13;
		((TextBoxBase)textBoxHyperlinkPattern).BorderStyle = (BorderStyle)0;
		((Control)textBoxHyperlinkPattern).Location = new Point(4, 6);
		((Control)textBoxHyperlinkPattern).Margin = new Padding(2);
		((Control)textBoxHyperlinkPattern).Name = "textBoxHyperlinkPattern";
		((Control)textBoxHyperlinkPattern).Size = new Size(174, 13);
		((Control)textBoxHyperlinkPattern).TabIndex = 0;
		((Control)textBoxHyperlinkPattern).TextChanged += TextBoxHyperlinkPattern_TextChanged;
		((Control)buttonEditHyperlink).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonEditHyperlink).Image = (Image)componentResourceManager.GetObject("buttonEditHyperlink.Image");
		((Control)buttonEditHyperlink).Location = new Point(181, 1);
		((Control)buttonEditHyperlink).Margin = new Padding(2);
		((Control)buttonEditHyperlink).Name = "buttonEditHyperlink";
		((Control)buttonEditHyperlink).Size = new Size(23, 23);
		((Control)buttonEditHyperlink).TabIndex = 1;
		((Control)buttonEditHyperlink).TabStop = false;
		((ButtonBase)buttonEditHyperlink).UseVisualStyleBackColor = true;
		((Control)buttonEditHyperlink).Click += ButtonEditHyperlinkPattern_Click;
		((Control)colorBoxMaximum).AccessibleRole = (AccessibleRole)43;
		((Control)colorBoxMaximum).Location = new Point(205, 31);
		((Control)colorBoxMaximum).Margin = new Padding(2);
		((Control)colorBoxMaximum).Name = "colorBoxMaximum";
		colorBoxMaximum.SelectedColor = Color.Transparent;
		colorBoxMaximum.ShowColorName = false;
		((Control)colorBoxMaximum).Size = new Size(90, 25);
		((Control)colorBoxMaximum).TabIndex = 3;
		colorBoxMaximum.SelectedColorChanged += ColorBoxMaximum_SelectedColorChanged;
		((Control)colorBoxMinimum).AccessibleRole = (AccessibleRole)43;
		((Control)colorBoxMinimum).Location = new Point(90, 31);
		((Control)colorBoxMinimum).Margin = new Padding(2);
		((Control)colorBoxMinimum).Name = "colorBoxMinimum";
		colorBoxMinimum.SelectedColor = Color.Transparent;
		colorBoxMinimum.ShowColorName = false;
		((Control)colorBoxMinimum).Size = new Size(90, 25);
		((Control)colorBoxMinimum).TabIndex = 1;
		colorBoxMinimum.SelectedColorChanged += ColorBoxMinimum_SelectedColorChanged;
		((Control)colorBoxNegativeColor).AccessibleRole = (AccessibleRole)43;
		((Control)colorBoxNegativeColor).Location = new Point(205, 2);
		((Control)colorBoxNegativeColor).Margin = new Padding(2);
		((Control)colorBoxNegativeColor).Name = "colorBoxNegativeColor";
		colorBoxNegativeColor.SelectedColor = Color.Transparent;
		colorBoxNegativeColor.ShowColorName = false;
		((Control)colorBoxNegativeColor).Size = new Size(89, 25);
		((Control)colorBoxNegativeColor).TabIndex = 3;
		colorBoxNegativeColor.SelectedColorChanged += ColorBoxColors_SelectedColorChanged;
		((Control)colorBoxPositiveColor).AccessibleRole = (AccessibleRole)43;
		((Control)colorBoxPositiveColor).Location = new Point(90, 2);
		((Control)colorBoxPositiveColor).Margin = new Padding(2);
		((Control)colorBoxPositiveColor).Name = "colorBoxPositiveColor";
		colorBoxPositiveColor.SelectedColor = Color.Transparent;
		colorBoxPositiveColor.ShowColorName = false;
		((Control)colorBoxPositiveColor).Size = new Size(90, 25);
		((Control)colorBoxPositiveColor).TabIndex = 1;
		colorBoxPositiveColor.SelectedColorChanged += ColorBoxColors_SelectedColorChanged;
		((Control)panelInteraction).Anchor = (AnchorStyles)13;
		((Control)panelInteraction).Controls.Add((Control)(object)buttonSetDefaultInteraction);
		((Control)panelInteraction).Controls.Add((Control)(object)labelInteractionStatus);
		((Control)panelInteraction).Controls.Add((Control)(object)buttonEditInteraction);
		((Control)panelInteraction).Location = new Point(90, 12);
		((Control)panelInteraction).Margin = new Padding(2);
		((Control)panelInteraction).Name = "panelInteraction";
		((Control)panelInteraction).Size = new Size(205, 25);
		((Control)panelInteraction).TabIndex = 0;
		((Control)buttonSetDefaultInteraction).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonSetDefaultInteraction).Image = (Image)componentResourceManager.GetObject("buttonSetDefaultInteraction.Image");
		((Control)buttonSetDefaultInteraction).Location = new Point(180, 1);
		((Control)buttonSetDefaultInteraction).Margin = new Padding(2);
		((Control)buttonSetDefaultInteraction).Name = "buttonSetDefaultInteraction";
		((Control)buttonSetDefaultInteraction).Size = new Size(24, 23);
		((Control)buttonSetDefaultInteraction).TabIndex = 2;
		((Control)buttonSetDefaultInteraction).TabStop = false;
		((ButtonBase)buttonSetDefaultInteraction).UseVisualStyleBackColor = true;
		((Control)buttonSetDefaultInteraction).Click += ButtonSetDefaultInteraction_Click;
		((Control)labelInteractionStatus).Anchor = (AnchorStyles)15;
		((Control)labelInteractionStatus).Location = new Point(1, 1);
		((Control)labelInteractionStatus).Name = "labelInteractionStatus";
		((Control)labelInteractionStatus).Size = new Size(154, 23);
		((Control)labelInteractionStatus).TabIndex = 0;
		((Label)labelInteractionStatus).TextAlign = (ContentAlignment)16;
		((Control)buttonEditInteraction).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonEditInteraction).Image = (Image)componentResourceManager.GetObject("buttonEditInteraction.Image");
		((Control)buttonEditInteraction).Location = new Point(158, 1);
		((Control)buttonEditInteraction).Margin = new Padding(2);
		((Control)buttonEditInteraction).Name = "buttonEditInteraction";
		((Control)buttonEditInteraction).Size = new Size(23, 23);
		((Control)buttonEditInteraction).TabIndex = 1;
		((Control)buttonEditInteraction).TabStop = false;
		((ButtonBase)buttonEditInteraction).UseVisualStyleBackColor = true;
		((Control)buttonEditInteraction).Click += ButtonEditInteraction_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(319, 552);
		((Control)this).Controls.Add((Control)(object)panelUi);
		((Control)this).Controls.Add((Control)(object)panelVisibility);
		((Control)this).Controls.Add((Control)(object)panelShowTotals);
		((Control)this).Controls.Add((Control)(object)panelBoxExpression);
		((Control)this).Controls.Add((Control)(object)panelSummaryType);
		((Control)this).Controls.Add((Control)(object)labelExpression);
		((Control)this).Controls.Add((Control)(object)panelShowHyperlink);
		((Control)this).Controls.Add((Control)(object)listBoxColumns);
		((Control)this).Controls.Add((Control)(object)panelHyperlink);
		((Control)this).Controls.Add((Control)(object)panelSparklines);
		((Control)this).Controls.Add((Control)(object)panelColorScaleUI);
		((Control)this).Controls.Add((Control)(object)panelColorsMode);
		((Control)this).Controls.Add((Control)(object)panelColorsLabels);
		((Control)this).Controls.Add((Control)(object)panelColorsEmpty);
		((Control)this).Controls.Add((Control)(object)panelColors);
		((Control)this).Controls.Add((Control)(object)panelBlockInteraction);
		((Form)this).Margin = new Padding(2, 2, 2, 2);
		((Control)this).Name = "StiTableElementEditorForm";
		((Control)this).Padding = new Padding(12, 12, 12, 12);
		((Control)this).Text = "Table";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Control)panelUi).ResumeLayout(false);
		((Control)panelVisibility).ResumeLayout(false);
		((Control)panelShowTotals).ResumeLayout(false);
		((Control)panelSummaryType).ResumeLayout(false);
		((Control)panelShowHyperlink).ResumeLayout(false);
		((Control)panelHyperlink).ResumeLayout(false);
		((Control)panelSparklines).ResumeLayout(false);
		((ISupportInitialize)contextMenuBar).EndInit();
		((Control)panelColorScaleUI).ResumeLayout(false);
		((Control)panelColorsMode).ResumeLayout(false);
		((Control)panelColorsLabels).ResumeLayout(false);
		((Control)panelColors).ResumeLayout(false);
		((Control)panelBlockInteraction).ResumeLayout(false);
		((Control)panelVisibleExpression).ResumeLayout(false);
		((Control)panelVisibleExpression).PerformLayout();
		((Control)panelBoxExpression).ResumeLayout(false);
		((Control)panelBoxExpression).PerformLayout();
		((Control)panelBoxHyperlinkPattern).ResumeLayout(false);
		((Control)panelBoxHyperlinkPattern).PerformLayout();
		((Control)panelInteraction).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
