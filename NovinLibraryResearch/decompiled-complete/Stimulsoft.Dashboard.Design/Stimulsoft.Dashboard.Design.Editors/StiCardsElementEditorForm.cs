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
using Stimulsoft.Dashboard.Components.Cards;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Dashboard.Design.Controls;
using Stimulsoft.Dashboard.Design.Editors.Helpers;
using Stimulsoft.Dashboard.Design.Editors.Menu;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Data.Functions;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Design.Panels;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Dashboard.Design.Editors;

public class StiCardsElementEditorForm : StiElementEditorForm
{
	private IContainer components;

	private StiMeterListBox listBoxColumns;

	private StiPanelUX panelUi;

	private StiComboBox comboBoxVisible;

	private StiOperationRadioButton buttonBubble;

	private StiOperationRadioButton buttonIndicator;

	private StiLabelUX labelVisible;

	private StiLabelUX labelType;

	private StiOperationRadioButton buttonColorScale;

	private StiOperationRadioButton buttonSparklines;

	private StiOperationRadioButton buttonDataBars;

	private StiOperationRadioButton buttonMeasure;

	private StiOperationRadioButton buttonDimension;

	private StiLabelUX labelExpression;

	private StiPanelBox panelBoxExpression;

	private StiOperationButton buttonExpressionEdit;

	private HintTextBox textBoxExpression;

	private StiOperationButton buttonExpressionMenu;

	private StiPanelUX panelVisibility;

	private StiPanelBox panelVisibleExpression;

	private HintTextBox textBoxVisibleExpression;

	private StiOperationButton buttonEditVisibleExpression;

	private StiPanelUX panelSparklines;

	private ContextMenuBar contextMenuBar;

	private ButtonItem menuExpression;

	private StiOperationRadioButton buttonSparklinesWinLoss;

	private StiOperationRadioButton buttonSparklinesColumn;

	private StiOperationRadioButton buttonSparklinesArea;

	private StiOperationRadioButton buttonSparklinesLine;

	private StiCheckBoxUX checkBoxFirstLastPoints;

	private StiCheckBoxUX checkBoxHighLowPoints;

	private StiLabelUX labelSparklines;

	private StiPanelUX panelColorScaleUI;

	private StiLabelUX labelMinimumMaximumSeparator;

	private StiLabelUX labelMaximum;

	private StiLabelUX labelMinimum;

	private StiColorBoxXF colorBoxMaximum;

	private StiColorBoxXF colorBoxMinimum;

	private StiPanelUX panelColorsMode;

	private StiComboBox comboBoxColorMode;

	private StiLabelUX labelColors;

	private StiPanelUX panelColorsLabels;

	private StiLabelUX labelNegative;

	private StiLabelUX labelPositive;

	private StiPanelUX panelColorsEmpty;

	private StiPanelUX panelColors;

	private StiLabelUX labelPositiveNegativeSeparator;

	private StiColorBoxXF colorBoxNegativeColor;

	private StiColorBoxXF colorBoxPositiveColor;

	private ToolTip toolTip;

	internal StiCardsElement CardsElement => base.Element as StiCardsElement;

	internal StiCardsColumn CardsColumn => listBoxColumns.SelectedMeterItem?.Meter as StiCardsColumn;

	public override bool AllowResize => false;

	public override bool IsAnyPresent => ((ListBox)listBoxColumns).Items.Count > 0;

	private string LocColorModeFromStyle => Loc.Get("FormStyleDesigner", "FromStyle");

	private string LocColorModeCustom => Loc.Get("FormColorBoxPopup", "Custom");

	private string LocVisibleTrue => Loc.Get("FormFormatEditor", "nameTrue");

	private string LocVisibleFalse => Loc.Get("FormFormatEditor", "nameFalse");

	private string LocVisibleExpression => Loc.GetMain("Expression");

	public StiCardsElementEditorForm()
		: this(null)
	{
	}

	public StiCardsElementEditorForm(StiElement element)
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
		}
		ReadPropertiesFromElement();
		LocalizeInternal();
		menuExpression.PopupOpen += delegate
		{
			new StiCardsExpressionMenu(menuExpression, (TextBox)(object)textBoxExpression, this, listBoxColumns).CreateItems();
		};
		contextMenuBar.SetContextMenuEx((Control)(object)listBoxColumns, menuExpression);
		ActivateDictionaryPanel();
		DoScaling();
		((ListBox)listBoxColumns).SelectedIndexChanged += delegate
		{
			StiPropertiesPanelHelper.SetPropertyGrid(Designer, CardsColumn);
		};
	}

	public override void Localize()
	{
		LocalizeInternal();
		((Control)listBoxColumns).Invalidate();
	}

	private void LocalizeInternal()
	{
		((Control)this).Text = Loc.Get("Components", "StiCards");
		((Control)labelType).Text = Loc.Get("PropertyMain", "Type");
		((Control)labelSparklines).Text = Loc.Get("Dashboard", "Sparklines");
		((Control)labelExpression).Text = Loc.Get("PropertyMain", "Expression");
		((Control)labelColors).Text = Loc.GetMain("Color");
		((Control)labelPositive).Text = Loc.GetMain("Positive");
		((Control)labelNegative).Text = Loc.GetMain("Negative");
		((Control)labelMinimum).Text = (Loc.IsCyrillic ? Loc.GetEnum("StiFilterItemValueLow") : Loc.GetMain("Minimum"));
		((Control)labelMaximum).Text = (Loc.IsCyrillic ? Loc.GetEnum("StiFilterItemValueHigh") : Loc.GetMain("Maximum"));
		((Control)labelVisible).Text = Loc.GetMain("Visible");
		((Control)checkBoxFirstLastPoints).Text = Loc.Get("Dashboard", "FirstLastPoints");
		((Control)checkBoxHighLowPoints).Text = Loc.Get("Dashboard", "HighLowPoints");
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

	private void DoScaling()
	{
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
	}

	public override void ReadPropertiesFromElement()
	{
		if (listBoxColumns.ItemsEqualTo(CardsElement.Columns))
		{
			return;
		}
		((ListBox)listBoxColumns).BeginUpdate();
		((ListBox)listBoxColumns).Items.Clear();
		foreach (StiCardsColumn column in CardsElement.Columns)
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
		CardsElement.Columns = (from t in listBoxColumns.ToItems()
			select t.Meter).Cast<StiCardsColumn>().ToList();
		RefreshElementView();
	}

	private void CheckSelectedColumns()
	{
		StiCardsColumn selectedColumn = StiCardsElementSelection.GetSelected(CardsElement)?.FirstOrDefault();
		listBoxColumns.SelectedMeterItem = ((selectedColumn != null) ? listBoxColumns.ToItems().FirstOrDefault((StiMeterItem t) => t.Meter == selectedColumn) : null);
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
			stiMeterItem2.Meter = StiMeterHelper.Cards.GetColumn(stiMeterItem2.Meter);
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
				StiCardsColumn stiCardsColumn = ((type != null && type.IsNumericType()) ? ((StiCardsColumn)StiMeterHelper.Cards.GetMeasure(draggedDataColumnOrVariable)) : ((StiCardsColumn)StiMeterHelper.Cards.GetDimension(draggedDataColumnOrVariable)));
				string label = StiLabelHelper.GetLabel(stiCardsColumn);
				StiMeterItem stiMeterItem = new StiMeterItem(label, stiCardsColumn, GetNodeImageKey(stiCardsColumn));
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
				if (!(StiDragDropColumnHelper.GetDraggedTableElement(Designer) is StiTableElement element))
				{
					return;
				}
				StiCardsElement stiCardsElement = new StiCardsElement();
				stiCardsElement.ConvertFrom(element);
				int count = ((ListBox)listBoxColumns).Items.Count;
				foreach (StiCardsColumn column in stiCardsElement.Columns)
				{
					CardsElement.Columns.Add(column);
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
			RefreshElementView();
		};
	}

	public void RefreshNodeImage()
	{
		listBoxColumns.SetSelectedImage(GetNodeImageKey(CardsColumn));
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
			StiLabelUX stiLabelUX = labelExpression;
			bool flag = (((Control)panelBoxExpression).Visible = IsAnyPresent);
			bool visible = (((Control)stiLabelUX).Visible = flag);
			((Control)stiPanelUX).Visible = visible;
			((Control)listBoxColumns).Dock = (DockStyle)((((ListBox)listBoxColumns).Items.Count != 0) ? 1 : 5);
			((RadioButton)buttonDimension).Checked = CardsColumn is StiDimensionCardsColumn;
			((RadioButton)buttonMeasure).Checked = CardsColumn is StiMeasureCardsColumn;
			((RadioButton)buttonDataBars).Checked = CardsColumn is StiDataBarsCardsColumn;
			((RadioButton)buttonColorScale).Checked = CardsColumn is StiColorScaleCardsColumn;
			((RadioButton)buttonSparklines).Checked = CardsColumn is StiSparklinesCardsColumn;
			((RadioButton)buttonIndicator).Checked = CardsColumn is StiIndicatorCardsColumn;
			((RadioButton)buttonBubble).Checked = CardsColumn is StiBubbleCardsColumn;
			((Control)textBoxExpression).Text = ((CardsColumn != null) ? CardsColumn.Expression : "");
			if (CardsColumn != null)
			{
				if (CardsColumn.Visibility == StiCardsColumnVisibility.True)
				{
					((ComboBox)comboBoxVisible).SelectedItem = LocVisibleTrue;
				}
				else if (CardsColumn.Visibility == StiCardsColumnVisibility.False)
				{
					((ComboBox)comboBoxVisible).SelectedItem = LocVisibleFalse;
				}
				else if (CardsColumn.Visibility == StiCardsColumnVisibility.Expression)
				{
					((ComboBox)comboBoxVisible).SelectedItem = LocVisibleExpression;
				}
				((Control)textBoxVisibleExpression).Text = CardsColumn.VisibilityExpression;
			}
			((Control)panelVisibility).Visible = IsAnyPresent && CardsColumn != null && CardsColumn.Visibility == StiCardsColumnVisibility.Expression;
			((Control)panelSparklines).Visible = ((Control)panelUi).Visible && ((RadioButton)buttonSparklines).Checked;
			((Control)panelColorScaleUI).Visible = ((Control)panelUi).Visible && ((RadioButton)buttonColorScale).Checked;
			StiSparklinesCardsColumn stiSparklinesCardsColumn = CardsColumn as StiSparklinesCardsColumn;
			((RadioButton)buttonSparklinesLine).Checked = stiSparklinesCardsColumn != null && stiSparklinesCardsColumn.Type == StiSparklinesType.Line;
			((RadioButton)buttonSparklinesArea).Checked = stiSparklinesCardsColumn != null && stiSparklinesCardsColumn.Type == StiSparklinesType.Area;
			((RadioButton)buttonSparklinesColumn).Checked = stiSparklinesCardsColumn != null && stiSparklinesCardsColumn.Type == StiSparklinesType.Column;
			((RadioButton)buttonSparklinesWinLoss).Checked = stiSparklinesCardsColumn != null && stiSparklinesCardsColumn.Type == StiSparklinesType.WinLoss;
			((CheckBox)checkBoxFirstLastPoints).Checked = stiSparklinesCardsColumn?.ShowFirstLastPoints ?? false;
			((CheckBox)checkBoxHighLowPoints).Checked = stiSparklinesCardsColumn?.ShowHighLowPoints ?? false;
			bool flag3 = stiSparklinesCardsColumn != null && (stiSparklinesCardsColumn.Type == StiSparklinesType.Line || stiSparklinesCardsColumn.Type == StiSparklinesType.Area);
			StiCheckBoxUX stiCheckBoxUX = checkBoxFirstLastPoints;
			visible = (((Control)checkBoxHighLowPoints).Visible = flag3);
			((Control)stiCheckBoxUX).Visible = visible;
			((Control)panelSparklines).Height = (flag3 ? ((Control)checkBoxFirstLastPoints).Bottom : ((Control)buttonDimension).Bottom);
			StiDimensionCardsColumn stiDimensionCardsColumn = CardsColumn as StiDimensionCardsColumn;
			if (CardsColumn is StiColorScaleCardsColumn stiColorScaleCardsColumn)
			{
				colorBoxMinimum.SelectedColor = stiColorScaleCardsColumn.MinimumColor;
				colorBoxMaximum.SelectedColor = stiColorScaleCardsColumn.MaximumColor;
			}
			StiSparklinesCardsColumn stiSparklinesCardsColumn2 = CardsColumn as StiSparklinesCardsColumn;
			if (stiSparklinesCardsColumn2 != null)
			{
				colorBoxPositiveColor.SelectedColor = stiSparklinesCardsColumn2.PositiveColor;
				colorBoxNegativeColor.SelectedColor = stiSparklinesCardsColumn2.NegativeColor;
			}
			StiBubbleCardsColumn stiBubbleCardsColumn = CardsColumn as StiBubbleCardsColumn;
			if (stiBubbleCardsColumn != null)
			{
				colorBoxPositiveColor.SelectedColor = stiBubbleCardsColumn.PositiveColor;
				colorBoxNegativeColor.SelectedColor = stiBubbleCardsColumn.NegativeColor;
			}
			StiLabelUX stiLabelUX2 = labelPositiveNegativeSeparator;
			StiPanelUX stiPanelUX2 = panelColorsLabels;
			flag = (((Control)colorBoxNegativeColor).Visible = ((RadioButton)buttonSparklinesColumn).Checked || ((RadioButton)buttonSparklinesWinLoss).Checked || ((RadioButton)buttonBubble).Checked);
			visible = (((Control)stiPanelUX2).Visible = flag);
			((Control)stiLabelUX2).Visible = visible;
			((Control)labelColors).Text = ((((RadioButton)buttonSparklinesColumn).Checked || ((RadioButton)buttonSparklinesWinLoss).Checked) ? Loc.GetMain("Colors") : Loc.GetMain("Color"));
			((ComboBox)comboBoxColorMode).SelectedItem = (((stiSparklinesCardsColumn2 != null && stiSparklinesCardsColumn2.AllowCustomColors) || (stiBubbleCardsColumn != null && stiBubbleCardsColumn.AllowCustomColors)) ? LocColorModeCustom : LocColorModeFromStyle);
			((Control)panelColorsMode).Visible = IsAnyPresent && (((RadioButton)buttonSparklines).Checked || ((RadioButton)buttonBubble).Checked);
			((Control)panelColors).Visible = IsAnyPresent && ((((RadioButton)buttonSparklines).Checked && stiSparklinesCardsColumn2 != null && stiSparklinesCardsColumn2.AllowCustomColors) || (((RadioButton)buttonBubble).Checked && stiBubbleCardsColumn != null && stiBubbleCardsColumn.AllowCustomColors));
			((Control)panelColorsLabels).Visible = ((Control)panelColors).Visible && (((RadioButton)buttonSparklinesColumn).Checked || ((RadioButton)buttonSparklinesWinLoss).Checked || ((RadioButton)buttonBubble).Checked);
			((Control)panelColorsEmpty).Visible = !((Control)panelColorsLabels).Visible && stiSparklinesCardsColumn2 != null && stiSparklinesCardsColumn2.AllowCustomColors;
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

	private StiMeterItemImage GetNodeImageKey(StiCardsColumn column)
	{
		if (column is StiDimensionCardsColumn)
		{
			return StiMeterItemImage.Dimension;
		}
		if (column is StiDataBarsCardsColumn)
		{
			return StiMeterItemImage.DataBars;
		}
		if (column is StiColorScaleCardsColumn)
		{
			return StiMeterItemImage.ColorScale;
		}
		if (column is StiIndicatorCardsColumn)
		{
			return StiMeterItemImage.Indicator;
		}
		if (column is StiSparklinesCardsColumn stiSparklinesCardsColumn)
		{
			if (stiSparklinesCardsColumn.Type == StiSparklinesType.Line)
			{
				return StiMeterItemImage.SparklinesLine;
			}
			if (stiSparklinesCardsColumn.Type == StiSparklinesType.Area)
			{
				return StiMeterItemImage.SparklinesArea;
			}
			if (stiSparklinesCardsColumn.Type == StiSparklinesType.Column)
			{
				return StiMeterItemImage.SparklinesColumn;
			}
			if (stiSparklinesCardsColumn.Type == StiSparklinesType.WinLoss)
			{
				return StiMeterItemImage.SparklinesWinLoss;
			}
		}
		if (column is StiBubbleCardsColumn)
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

	private static void CheckMeasureMeterTextFormat(StiCardsColumn meter)
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
			IEnumerable<StiCardsColumn> second = from t in listBoxColumns.ToItems()
				select t.Meter as StiCardsColumn into c
				where c != null
				select c;
			IEnumerable<StiCardsColumn> dif = CardsElement.Columns.Except(second);
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
		if (base.LockRefresh || CardsColumn == null)
		{
			return;
		}
		try
		{
			if (((RadioButton)buttonDimension).Checked)
			{
				SwitchTextFormatFromPercentFromGrandTotal();
				listBoxColumns.SelectedMeterItem.Meter = StiMeterHelper.Cards.GetDimension(CardsColumn);
			}
			else if (((RadioButton)buttonMeasure).Checked)
			{
				listBoxColumns.SelectedMeterItem.Meter = StiMeterHelper.Cards.GetMeasure(CardsColumn, CardsElement.Dashboard);
				StiCardsColumn stiCardsColumn = listBoxColumns.SelectedMeterItem.Meter as StiCardsColumn;
				stiCardsColumn.HorAlignment = StiHorAlignment.Right;
				CheckMeasureMeterTextFormat(stiCardsColumn);
			}
			else if (((RadioButton)buttonDataBars).Checked)
			{
				listBoxColumns.SelectedMeterItem.Meter = StiMeterHelper.Cards.GetDataBars(CardsColumn, CardsElement.Dashboard);
			}
			else if (((RadioButton)buttonColorScale).Checked)
			{
				((StiCardsColumn)listBoxColumns.SelectedMeterItem.Meter).HorAlignment = StiHorAlignment.Right;
				listBoxColumns.SelectedMeterItem.Meter = StiMeterHelper.Cards.GetColorScale(CardsColumn, CardsElement.Dashboard);
			}
			else if (((RadioButton)buttonIndicator).Checked)
			{
				listBoxColumns.SelectedMeterItem.Meter = StiMeterHelper.Cards.GetIndicator(CardsColumn, CardsElement.Dashboard);
			}
			else if (((RadioButton)buttonSparklines).Checked)
			{
				SwitchTextFormatFromPercentFromGrandTotal();
				listBoxColumns.SelectedMeterItem.Meter = StiMeterHelper.Cards.GetSparklines(CardsColumn);
			}
			else if (((RadioButton)buttonBubble).Checked)
			{
				listBoxColumns.SelectedMeterItem.Meter = StiMeterHelper.Cards.GetBubble(CardsColumn, CardsElement.Dashboard);
			}
		}
		catch
		{
		}
		StiCardsElementSelection.Select(CardsElement, CardsColumn);
		RefreshNodeImage();
		RefreshElementView();
		WritePropertiesToElement();
		RefreshUiPanel();
	}

	private void SwitchTextFormatFromPercentFromGrandTotal()
	{
		if (StiExpressionHelper.GetFunction(CardsColumn.Expression)?.ToLowerInvariant() == "percentofgrandtotal" && CardsColumn.TextFormat is StiPercentageFormatService)
		{
			CardsColumn.TextFormat = new StiNumberFormatService();
		}
	}

	private void ButtonSparklines_CheckedChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && CardsColumn is StiSparklinesCardsColumn stiSparklinesCardsColumn)
		{
			if (((RadioButton)buttonSparklinesLine).Checked)
			{
				stiSparklinesCardsColumn.Type = StiSparklinesType.Line;
			}
			else if (((RadioButton)buttonSparklinesArea).Checked)
			{
				stiSparklinesCardsColumn.Type = StiSparklinesType.Area;
			}
			else if (((RadioButton)buttonSparklinesColumn).Checked)
			{
				stiSparklinesCardsColumn.Type = StiSparklinesType.Column;
			}
			else if (((RadioButton)buttonSparklinesWinLoss).Checked)
			{
				stiSparklinesCardsColumn.Type = StiSparklinesType.WinLoss;
			}
			RefreshElementView();
			RefreshNodeImage();
			RefreshUiPanel();
		}
	}

	private void CheckBoxHighLowPoints_CheckedChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && CardsColumn is StiSparklinesCardsColumn stiSparklinesCardsColumn)
		{
			stiSparklinesCardsColumn.ShowHighLowPoints = ((CheckBox)checkBoxHighLowPoints).Checked;
			RefreshElementView();
		}
	}

	private void CheckBoxFirstLastPoints_CheckedChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && CardsColumn is StiSparklinesCardsColumn stiSparklinesCardsColumn)
		{
			stiSparklinesCardsColumn.ShowFirstLastPoints = ((CheckBox)checkBoxFirstLastPoints).Checked;
			RefreshElementView();
		}
	}

	private void ColorBoxMinimum_SelectedColorChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && ((RadioButton)buttonColorScale).Checked)
		{
			StiColorScaleCardsColumn stiColorScaleCardsColumn = CardsColumn as StiColorScaleCardsColumn;
			stiColorScaleCardsColumn.MinimumColor = colorBoxMinimum.SelectedColor;
			RefreshElementView();
			RefreshUiPanel();
		}
	}

	private void ColorBoxMaximum_SelectedColorChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh && ((RadioButton)buttonColorScale).Checked)
		{
			StiColorScaleCardsColumn stiColorScaleCardsColumn = CardsColumn as StiColorScaleCardsColumn;
			stiColorScaleCardsColumn.MaximumColor = colorBoxMaximum.SelectedColor;
			RefreshElementView();
			RefreshUiPanel();
		}
	}

	private void ColorBoxColors_SelectedColorChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			if (CardsColumn is StiSparklinesCardsColumn stiSparklinesCardsColumn)
			{
				stiSparklinesCardsColumn.PositiveColor = colorBoxPositiveColor.SelectedColor;
				stiSparklinesCardsColumn.NegativeColor = colorBoxNegativeColor.SelectedColor;
			}
			if (CardsColumn is StiBubbleCardsColumn stiBubbleCardsColumn)
			{
				stiBubbleCardsColumn.PositiveColor = colorBoxPositiveColor.SelectedColor;
				stiBubbleCardsColumn.NegativeColor = colorBoxNegativeColor.SelectedColor;
			}
			RefreshElementView();
			RefreshUiPanel();
		}
	}

	private void ComboBoxColorMode_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			if (CardsColumn is StiSparklinesCardsColumn stiSparklinesCardsColumn)
			{
				stiSparklinesCardsColumn.AllowCustomColors = ((ComboBox)comboBoxColorMode).SelectedItem?.ToString() == LocColorModeCustom;
				RefreshUiPanel();
				RefreshElementView();
			}
			if (CardsColumn is StiBubbleCardsColumn stiBubbleCardsColumn)
			{
				stiBubbleCardsColumn.AllowCustomColors = ((ComboBox)comboBoxColorMode).SelectedItem?.ToString() == LocColorModeCustom;
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
		if (!base.LockRefresh && CardsColumn != null)
		{
			if (((ComboBox)comboBoxVisible).SelectedItem?.ToString() == LocVisibleTrue)
			{
				CardsColumn.Visibility = StiCardsColumnVisibility.True;
			}
			else if (((ComboBox)comboBoxVisible).SelectedItem?.ToString() == LocVisibleFalse)
			{
				CardsColumn.Visibility = StiCardsColumnVisibility.False;
			}
			else
			{
				CardsColumn.Visibility = StiCardsColumnVisibility.Expression;
			}
			RefreshUiPanel();
			RefreshElementView();
		}
	}

	private void TextBoxVisibleExpression_TextChanged(object sender, EventArgs e)
	{
		if (CardsColumn != null)
		{
			CardsColumn.VisibilityExpression = ((Control)textBoxVisibleExpression).Text;
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
		if (CardsColumn != null)
		{
			CardsColumn.Expression = ((Control)textBoxExpression).Text;
		}
		listBoxColumns.SetSelectedText(StiLabelHelper.GetLabel(CardsColumn));
		RefreshExpressionHint();
	}

	private void ButtonExpressionMenu_Click(object sender, EventArgs e)
	{
		new StiCardsExpressionMenu(menuExpression, (TextBox)(object)textBoxExpression, this, listBoxColumns).CreateItems();
		Point point = new Point(((Control)panelBoxExpression).Width, ((Control)panelBoxExpression).Height + StiScale.I1);
		if (StiLeftRightHandedHelper.IsLeftHanded)
		{
			point.X -= menuExpression.PopupSize.Width;
		}
		point = ((Control)panelBoxExpression).PointToScreen(point);
		menuExpression.Popup(point);
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
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Expected O, but got Unknown
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c4: Expected O, but got Unknown
		//IL_0505: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_071b: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0859: Unknown result type (might be due to invalid IL or missing references)
		//IL_0863: Expected O, but got Unknown
		//IL_0881: Unknown result type (might be due to invalid IL or missing references)
		//IL_090c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0916: Expected O, but got Unknown
		//IL_0934: Unknown result type (might be due to invalid IL or missing references)
		//IL_09be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad1: Expected O, but got Unknown
		//IL_0aef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b84: Expected O, but got Unknown
		//IL_0ba2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c37: Expected O, but got Unknown
		//IL_0c55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cea: Expected O, but got Unknown
		//IL_0d05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da6: Expected O, but got Unknown
		//IL_0dc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1019: Unknown result type (might be due to invalid IL or missing references)
		//IL_107a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1084: Expected O, but got Unknown
		//IL_10b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c3: Expected O, but got Unknown
		//IL_10e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1233: Unknown result type (might be due to invalid IL or missing references)
		//IL_12a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_12b0: Expected O, but got Unknown
		//IL_12ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_13db: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e5: Expected O, but got Unknown
		//IL_1403: Unknown result type (might be due to invalid IL or missing references)
		//IL_148e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1498: Expected O, but got Unknown
		//IL_14b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1541: Unknown result type (might be due to invalid IL or missing references)
		//IL_154b: Expected O, but got Unknown
		//IL_1566: Unknown result type (might be due to invalid IL or missing references)
		//IL_15fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1607: Expected O, but got Unknown
		//IL_1622: Unknown result type (might be due to invalid IL or missing references)
		//IL_16b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1756: Unknown result type (might be due to invalid IL or missing references)
		//IL_17f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_18f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1947: Unknown result type (might be due to invalid IL or missing references)
		//IL_1951: Expected O, but got Unknown
		//IL_1982: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ba1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d03: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d84: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e41: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eaf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f33: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_207d: Unknown result type (might be due to invalid IL or missing references)
		//IL_20d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_20dc: Expected O, but got Unknown
		//IL_210c: Unknown result type (might be due to invalid IL or missing references)
		//IL_219a: Unknown result type (might be due to invalid IL or missing references)
		//IL_221e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2369: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiCardsElementEditorForm));
		toolTip = new ToolTip(components);
		panelBoxExpression = new StiPanelBox();
		buttonExpressionEdit = new StiOperationButton();
		textBoxExpression = new HintTextBox();
		buttonExpressionMenu = new StiOperationButton();
		labelExpression = new StiLabelUX();
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
		listBoxColumns = new StiMeterListBox();
		panelVisibility = new StiPanelUX();
		panelVisibleExpression = new StiPanelBox();
		textBoxVisibleExpression = new HintTextBox();
		buttonEditVisibleExpression = new StiOperationButton();
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
		colorBoxMaximum = new StiColorBoxXF();
		colorBoxMinimum = new StiColorBoxXF();
		panelColorsMode = new StiPanelUX();
		comboBoxColorMode = new StiComboBox();
		labelColors = new StiLabelUX();
		panelColorsLabels = new StiPanelUX();
		labelNegative = new StiLabelUX();
		labelPositive = new StiLabelUX();
		panelColorsEmpty = new StiPanelUX();
		panelColors = new StiPanelUX();
		labelPositiveNegativeSeparator = new StiLabelUX();
		colorBoxNegativeColor = new StiColorBoxXF();
		colorBoxPositiveColor = new StiColorBoxXF();
		((Control)panelBoxExpression).SuspendLayout();
		((Control)panelUi).SuspendLayout();
		((Control)panelVisibility).SuspendLayout();
		((Control)panelVisibleExpression).SuspendLayout();
		((Control)panelSparklines).SuspendLayout();
		((ISupportInitialize)contextMenuBar).BeginInit();
		((Control)panelColorScaleUI).SuspendLayout();
		((Control)panelColorsMode).SuspendLayout();
		((Control)panelColorsLabels).SuspendLayout();
		((Control)panelColors).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panelBoxExpression).BackColor = Color.White;
		((Control)panelBoxExpression).Controls.Add((Control)(object)buttonExpressionEdit);
		((Control)panelBoxExpression).Controls.Add((Control)(object)textBoxExpression);
		((Control)panelBoxExpression).Controls.Add((Control)(object)buttonExpressionMenu);
		((Control)panelBoxExpression).Dock = (DockStyle)1;
		((Control)panelBoxExpression).Location = new Point(12, 187);
		((Control)panelBoxExpression).Margin = new Padding(2);
		((Control)panelBoxExpression).Name = "panelBoxExpression";
		((Control)panelBoxExpression).Size = new Size(295, 25);
		((Control)panelBoxExpression).TabIndex = 6;
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
		((Control)textBoxExpression).BackColor = SystemColors.Window;
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
		((Control)labelExpression).Dock = (DockStyle)1;
		((Control)labelExpression).ForeColor = Color.DimGray;
		((Control)labelExpression).Location = new Point(12, 155);
		((Control)labelExpression).Margin = new Padding(2, 0, 2, 0);
		((Control)labelExpression).Name = "labelExpression";
		((Control)labelExpression).Size = new Size(295, 32);
		((Control)labelExpression).TabIndex = 5;
		((Control)labelExpression).Text = "Expression";
		((Label)labelExpression).TextAlign = (ContentAlignment)16;
		((Control)panelUi).BackColor = Color.White;
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
		((Control)panelUi).Location = new Point(12, 406);
		((Control)panelUi).Margin = new Padding(2);
		((Control)panelUi).Name = "panelUi";
		((Control)panelUi).Size = new Size(295, 76);
		((Control)panelUi).TabIndex = 4;
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
		((Control)labelVisible).ForeColor = Color.DimGray;
		((Control)labelVisible).Location = new Point(0, 49);
		((Control)labelVisible).Margin = new Padding(2, 0, 2, 0);
		((Control)labelVisible).Name = "labelVisible";
		((Control)labelVisible).Size = new Size(82, 26);
		((Control)labelVisible).TabIndex = 1;
		((Control)labelVisible).Text = "Visible";
		((Label)labelVisible).TextAlign = (ContentAlignment)16;
		((Control)labelType).ForeColor = Color.DimGray;
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
		((Control)listBoxColumns).AllowDrop = true;
		((Control)listBoxColumns).Dock = (DockStyle)1;
		((Control)listBoxColumns).Location = new Point(12, 12);
		((Control)listBoxColumns).Margin = new Padding(2);
		((Control)listBoxColumns).Name = "listBoxColumns";
		((Control)listBoxColumns).Size = new Size(295, 143);
		((Control)listBoxColumns).TabIndex = 1;
		((Control)panelVisibility).BackColor = Color.White;
		((Control)panelVisibility).Controls.Add((Control)(object)panelVisibleExpression);
		((Control)panelVisibility).Dock = (DockStyle)2;
		((Control)panelVisibility).Location = new Point(12, 482);
		((Control)panelVisibility).Margin = new Padding(2);
		((Control)panelVisibility).Name = "panelVisibility";
		((Control)panelVisibility).Size = new Size(295, 40);
		((Control)panelVisibility).TabIndex = 7;
		((Control)panelVisibleExpression).Anchor = (AnchorStyles)13;
		((Control)panelVisibleExpression).BackColor = Color.White;
		((Control)panelVisibleExpression).Controls.Add((Control)(object)textBoxVisibleExpression);
		((Control)panelVisibleExpression).Controls.Add((Control)(object)buttonEditVisibleExpression);
		((Control)panelVisibleExpression).Location = new Point(90, 12);
		((Control)panelVisibleExpression).Margin = new Padding(2);
		((Control)panelVisibleExpression).Name = "panelVisibleExpression";
		((Control)panelVisibleExpression).Size = new Size(205, 25);
		((Control)panelVisibleExpression).TabIndex = 0;
		((Control)textBoxVisibleExpression).AllowDrop = true;
		((Control)textBoxVisibleExpression).Anchor = (AnchorStyles)13;
		((Control)textBoxVisibleExpression).BackColor = SystemColors.Window;
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
		((Control)panelSparklines).BackColor = Color.White;
		((Control)panelSparklines).Controls.Add((Control)(object)contextMenuBar);
		((Control)panelSparklines).Controls.Add((Control)(object)buttonSparklinesWinLoss);
		((Control)panelSparklines).Controls.Add((Control)(object)buttonSparklinesColumn);
		((Control)panelSparklines).Controls.Add((Control)(object)buttonSparklinesArea);
		((Control)panelSparklines).Controls.Add((Control)(object)buttonSparklinesLine);
		((Control)panelSparklines).Controls.Add((Control)(object)checkBoxFirstLastPoints);
		((Control)panelSparklines).Controls.Add((Control)(object)checkBoxHighLowPoints);
		((Control)panelSparklines).Controls.Add((Control)(object)labelSparklines);
		((Control)panelSparklines).Dock = (DockStyle)2;
		((Control)panelSparklines).Location = new Point(12, 522);
		((Control)panelSparklines).Margin = new Padding(2);
		((Control)panelSparklines).Name = "panelSparklines";
		((Control)panelSparklines).Size = new Size(295, 97);
		((Control)panelSparklines).TabIndex = 10;
		contextMenuBar.AntiAlias = true;
		contextMenuBar.DockSide = eDockSide.Bottom;
		contextMenuBar.DockTabStripHeight = 50;
		((Control)contextMenuBar).Font = new Font("Segoe UI", 9f);
		contextMenuBar.Items.AddRange(new BaseItem[1] { menuExpression });
		contextMenuBar.Location = new Point(200, 64);
		((Control)contextMenuBar).Margin = new Padding(2);
		contextMenuBar.Name = "contextMenuBar";
		contextMenuBar.Size = new Size(79, 25);
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
		((Control)labelSparklines).ForeColor = Color.DimGray;
		((Control)labelSparklines).Location = new Point(0, 12);
		((Control)labelSparklines).Margin = new Padding(2, 0, 2, 0);
		((Control)labelSparklines).Name = "labelSparklines";
		((Control)labelSparklines).Size = new Size(82, 26);
		((Control)labelSparklines).TabIndex = 1;
		((Control)labelSparklines).Text = "Sparklines";
		((Label)labelSparklines).TextAlign = (ContentAlignment)16;
		((Control)panelColorScaleUI).BackColor = Color.White;
		((Control)panelColorScaleUI).Controls.Add((Control)(object)labelMinimumMaximumSeparator);
		((Control)panelColorScaleUI).Controls.Add((Control)(object)labelMaximum);
		((Control)panelColorScaleUI).Controls.Add((Control)(object)labelMinimum);
		((Control)panelColorScaleUI).Controls.Add((Control)(object)colorBoxMaximum);
		((Control)panelColorScaleUI).Controls.Add((Control)(object)colorBoxMinimum);
		((Control)panelColorScaleUI).Dock = (DockStyle)2;
		((Control)panelColorScaleUI).Location = new Point(12, 619);
		((Control)panelColorScaleUI).Margin = new Padding(2);
		((Control)panelColorScaleUI).Name = "panelColorScaleUI";
		((Control)panelColorScaleUI).Size = new Size(295, 58);
		((Control)panelColorScaleUI).TabIndex = 11;
		((Control)labelMinimumMaximumSeparator).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelMinimumMaximumSeparator).ForeColor = Color.DimGray;
		((Control)labelMinimumMaximumSeparator).Location = new Point(186, 32);
		((Control)labelMinimumMaximumSeparator).Margin = new Padding(2, 0, 2, 0);
		((Control)labelMinimumMaximumSeparator).Name = "labelMinimumMaximumSeparator";
		((Control)labelMinimumMaximumSeparator).Size = new Size(13, 20);
		((Control)labelMinimumMaximumSeparator).TabIndex = 28;
		((Control)labelMinimumMaximumSeparator).Text = "-";
		((Label)labelMinimumMaximumSeparator).TextAlign = (ContentAlignment)32;
		((Control)labelMaximum).ForeColor = Color.DimGray;
		((Control)labelMaximum).Location = new Point(205, 8);
		((Control)labelMaximum).Margin = new Padding(2, 0, 2, 0);
		((Control)labelMaximum).Name = "labelMaximum";
		((Control)labelMaximum).Size = new Size(83, 17);
		((Control)labelMaximum).TabIndex = 2;
		((Control)labelMaximum).Text = "Maximum";
		((Label)labelMaximum).TextAlign = (ContentAlignment)16;
		((Control)labelMinimum).ForeColor = Color.DimGray;
		((Control)labelMinimum).Location = new Point(90, 8);
		((Control)labelMinimum).Margin = new Padding(2, 0, 2, 0);
		((Control)labelMinimum).Name = "labelMinimum";
		((Control)labelMinimum).Size = new Size(83, 17);
		((Control)labelMinimum).TabIndex = 0;
		((Control)labelMinimum).Text = "Minimum";
		((Label)labelMinimum).TextAlign = (ContentAlignment)16;
		((Control)colorBoxMaximum).AccessibleRole = (AccessibleRole)43;
		colorBoxMaximum.ShowColorName = false;
		((Control)colorBoxMaximum).Location = new Point(205, 31);
		((Control)colorBoxMaximum).Margin = new Padding(2);
		((Control)colorBoxMaximum).Name = "colorBoxMaximum";
		((Control)colorBoxMaximum).Size = new Size(90, 25);
		((Control)colorBoxMaximum).TabIndex = 3;
		colorBoxMaximum.SelectedColorChanged += ColorBoxMaximum_SelectedColorChanged;
		((Control)colorBoxMinimum).AccessibleRole = (AccessibleRole)43;
		colorBoxMinimum.ShowColorName = false;
		((Control)colorBoxMinimum).Location = new Point(90, 31);
		((Control)colorBoxMinimum).Margin = new Padding(2);
		((Control)colorBoxMinimum).Name = "colorBoxMinimum";
		((Control)colorBoxMinimum).Size = new Size(90, 25);
		((Control)colorBoxMinimum).TabIndex = 1;
		colorBoxMinimum.SelectedColorChanged += ColorBoxMinimum_SelectedColorChanged;
		((Control)panelColorsMode).BackColor = Color.White;
		((Control)panelColorsMode).Controls.Add((Control)(object)comboBoxColorMode);
		((Control)panelColorsMode).Controls.Add((Control)(object)labelColors);
		((Control)panelColorsMode).Dock = (DockStyle)2;
		((Control)panelColorsMode).Location = new Point(12, 677);
		((Control)panelColorsMode).Name = "panelColorsMode";
		((Control)panelColorsMode).Size = new Size(295, 38);
		((Control)panelColorsMode).TabIndex = 16;
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
		((Control)labelColors).ForeColor = Color.DimGray;
		((Control)labelColors).Location = new Point(0, 14);
		((Control)labelColors).Margin = new Padding(2, 0, 2, 0);
		((Control)labelColors).Name = "labelColors";
		((Control)labelColors).Size = new Size(84, 21);
		((Control)labelColors).TabIndex = 0;
		((Control)labelColors).Text = "Colors";
		((Label)labelColors).TextAlign = (ContentAlignment)16;
		((Control)panelColorsLabels).BackColor = Color.White;
		((Control)panelColorsLabels).Controls.Add((Control)(object)labelNegative);
		((Control)panelColorsLabels).Controls.Add((Control)(object)labelPositive);
		((Control)panelColorsLabels).Dock = (DockStyle)2;
		((Control)panelColorsLabels).Location = new Point(12, 715);
		((Control)panelColorsLabels).Margin = new Padding(2);
		((Control)panelColorsLabels).Name = "panelColorsLabels";
		((Control)panelColorsLabels).Size = new Size(295, 26);
		((Control)panelColorsLabels).TabIndex = 17;
		((Control)labelNegative).ForeColor = Color.DimGray;
		((Control)labelNegative).Location = new Point(205, 7);
		((Control)labelNegative).Margin = new Padding(2, 0, 2, 0);
		((Control)labelNegative).Name = "labelNegative";
		((Control)labelNegative).Size = new Size(84, 17);
		((Control)labelNegative).TabIndex = 4;
		((Control)labelNegative).Text = "Negative";
		((Label)labelNegative).TextAlign = (ContentAlignment)16;
		((Control)labelPositive).ForeColor = Color.DimGray;
		((Control)labelPositive).Location = new Point(90, 7);
		((Control)labelPositive).Margin = new Padding(2, 0, 2, 0);
		((Control)labelPositive).Name = "labelPositive";
		((Control)labelPositive).Size = new Size(83, 17);
		((Control)labelPositive).TabIndex = 3;
		((Control)labelPositive).Text = "Positive";
		((Label)labelPositive).TextAlign = (ContentAlignment)16;
		((Control)panelColorsEmpty).BackColor = Color.White;
		((Control)panelColorsEmpty).Dock = (DockStyle)2;
		((Control)panelColorsEmpty).Location = new Point(12, 741);
		((Control)panelColorsEmpty).Margin = new Padding(2);
		((Control)panelColorsEmpty).Name = "panelColorsEmpty";
		((Control)panelColorsEmpty).Size = new Size(295, 8);
		((Control)panelColorsEmpty).TabIndex = 19;
		((Control)panelColors).BackColor = Color.White;
		((Control)panelColors).Controls.Add((Control)(object)labelPositiveNegativeSeparator);
		((Control)panelColors).Controls.Add((Control)(object)colorBoxNegativeColor);
		((Control)panelColors).Controls.Add((Control)(object)colorBoxPositiveColor);
		((Control)panelColors).Dock = (DockStyle)2;
		((Control)panelColors).Location = new Point(12, 749);
		((Control)panelColors).Margin = new Padding(2);
		((Control)panelColors).Name = "panelColors";
		((Control)panelColors).Size = new Size(295, 29);
		((Control)panelColors).TabIndex = 18;
		((Control)labelPositiveNegativeSeparator).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelPositiveNegativeSeparator).ForeColor = Color.DimGray;
		((Control)labelPositiveNegativeSeparator).Location = new Point(186, 4);
		((Control)labelPositiveNegativeSeparator).Margin = new Padding(2, 0, 2, 0);
		((Control)labelPositiveNegativeSeparator).Name = "labelPositiveNegativeSeparator";
		((Control)labelPositiveNegativeSeparator).Size = new Size(13, 20);
		((Control)labelPositiveNegativeSeparator).TabIndex = 29;
		((Control)labelPositiveNegativeSeparator).Text = "-";
		((Label)labelPositiveNegativeSeparator).TextAlign = (ContentAlignment)32;
		((Control)colorBoxNegativeColor).AccessibleRole = (AccessibleRole)43;
		colorBoxNegativeColor.ShowColorName = false;
		((Control)colorBoxNegativeColor).Location = new Point(205, 2);
		((Control)colorBoxNegativeColor).Margin = new Padding(2);
		((Control)colorBoxNegativeColor).Name = "colorBoxNegativeColor";
		((Control)colorBoxNegativeColor).Size = new Size(89, 25);
		((Control)colorBoxNegativeColor).TabIndex = 3;
		colorBoxNegativeColor.SelectedColorChanged += ColorBoxColors_SelectedColorChanged;
		((Control)colorBoxPositiveColor).AccessibleRole = (AccessibleRole)43;
		colorBoxPositiveColor.ShowColorName = false;
		((Control)colorBoxPositiveColor).Location = new Point(90, 2);
		((Control)colorBoxPositiveColor).Margin = new Padding(2);
		((Control)colorBoxPositiveColor).Name = "colorBoxPositiveColor";
		((Control)colorBoxPositiveColor).Size = new Size(90, 25);
		((Control)colorBoxPositiveColor).TabIndex = 1;
		colorBoxPositiveColor.SelectedColorChanged += ColorBoxColors_SelectedColorChanged;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(319, 790);
		((Control)this).Controls.Add((Control)(object)panelBoxExpression);
		((Control)this).Controls.Add((Control)(object)labelExpression);
		((Control)this).Controls.Add((Control)(object)panelUi);
		((Control)this).Controls.Add((Control)(object)listBoxColumns);
		((Control)this).Controls.Add((Control)(object)panelVisibility);
		((Control)this).Controls.Add((Control)(object)panelSparklines);
		((Control)this).Controls.Add((Control)(object)panelColorScaleUI);
		((Control)this).Controls.Add((Control)(object)panelColorsMode);
		((Control)this).Controls.Add((Control)(object)panelColorsLabels);
		((Control)this).Controls.Add((Control)(object)panelColorsEmpty);
		((Control)this).Controls.Add((Control)(object)panelColors);
		((Control)this).Name = "StiCardsElementEditorForm";
		((Control)this).Padding = new Padding(12);
		((Control)this).Text = "StiCardsElementEditorForm";
		((Control)panelBoxExpression).ResumeLayout(false);
		((Control)panelBoxExpression).PerformLayout();
		((Control)panelUi).ResumeLayout(false);
		((Control)panelVisibility).ResumeLayout(false);
		((Control)panelVisibleExpression).ResumeLayout(false);
		((Control)panelVisibleExpression).PerformLayout();
		((Control)panelSparklines).ResumeLayout(false);
		((ISupportInitialize)contextMenuBar).EndInit();
		((Control)panelColorScaleUI).ResumeLayout(false);
		((Control)panelColorsMode).ResumeLayout(false);
		((Control)panelColorsLabels).ResumeLayout(false);
		((Control)panelColors).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
