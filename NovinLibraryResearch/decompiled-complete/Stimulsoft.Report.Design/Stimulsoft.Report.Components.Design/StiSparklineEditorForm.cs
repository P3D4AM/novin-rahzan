using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Chart.Design;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Design.Inplace;
using Stimulsoft.Report.Design.Panels;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Components.Design;

public class StiSparklineEditorForm : StiInplaceToolForm
{
	private bool isItemsInitialized;

	private StiChartDataColumnListBox selectedListBox;

	private IContainer components;

	private ButtonItem menuExpression;

	private StiChartDataColumnListBox listBoxValues;

	private ToolTip toolTip;

	private StiChartGroupLabel labelValue;

	private ContextMenuBar contextMenuBar;

	private StiButtonPanelUX panelFooter;

	private StiButtonUX buttonOk;

	private StiButtonUX buttonCancel;

	private StiPanelUX panelOptions;

	private StiPanelBox panelBoxExpression;

	private HintTextBox textBoxExpression;

	private StiOperationButton buttonExpressionMenu;

	private StiLabelUX labelField;

	private StiPanelUX panelType;

	private StiRadioButtonUX buttonSparklinesWinLoss;

	private StiRadioButtonUX buttonSparklinesColumn;

	private StiRadioButtonUX buttonSparklinesArea;

	private StiRadioButtonUX buttonSparklinesLine;

	private StiCheckBoxUX checkBoxFirstLastPoints;

	private StiCheckBoxUX checkBoxHighLowPoints;

	private StiLabelUX labelType;

	private StiPanelUX panelPointsOptions;

	private StiPanelUX panelListBoxes;

	private ColorPickerButton colorBoxPositiveColor;

	private ColorPickerButton colorBoxNegativeColor;

	private StiLabelUX labelColor;

	private StiPanelUX panelColors;

	protected bool? UseAliases => Designer?.UseAliases;

	internal virtual StiDesignerControl Designer { get; set; }

	internal StiReport Report => Sparkline.Report;

	internal StiSparkline Sparkline { get; }

	internal StiDataColumnMeter SparklineValue => listBoxValues.SelectedChartItem?.Meter;

	public override bool IsAnyPresent => ((ListBox)listBoxValues).Items.Count > 0;

	public override bool AllowResize => false;

	private static string LocNotDefined => "[" + Loc.Get("Dashboard", "DataNotDefined") + "]";

	public StiSparklineEditorForm()
		: this(null)
	{
	}

	public StiSparklineEditorForm(StiSparkline sparkline)
	{
		InitializeComponent();
		Sparkline = sparkline;
		Designer = Report?.Designer?.DesignerControl as StiDesignerControl;
		if (StiOptions.Appearance.ShowFormsHelp)
		{
			((Form)this).HelpButton = true;
		}
		listBoxValues.Designer = Designer;
		listBoxValues.AllowImage = false;
		listBoxValues.AllowMultipleElements = false;
		ReadPropertiesFromElement();
		LocalizeInternal();
		DoScaling();
	}

	public override void Localize()
	{
		LocalizeInternal();
		((Control)listBoxValues).Invalidate();
		((Control)this).Invalidate();
	}

	private void LocalizeInternal()
	{
		((Control)this).Text = Loc.Get("Components", "StiSparkline");
		((Control)labelField).Text = Loc.GetMain("Field");
		((Control)labelColor).Text = Loc.GetMain("Color");
		((Control)labelValue).Text = Loc.GetMain("Value");
		((Control)labelType).Text = Loc.GetMain("Type");
		((Control)checkBoxFirstLastPoints).Text = Loc.Get("Dashboard", "FirstLastPoints");
		((Control)checkBoxHighLowPoints).Text = Loc.Get("Dashboard", "HighLowPoints");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		toolTip.SetToolTip((Control)(object)buttonSparklinesLine, Loc.Get("Chart", "SparklinesLine"));
		toolTip.SetToolTip((Control)(object)buttonSparklinesArea, Loc.Get("Chart", "SparklinesArea"));
		toolTip.SetToolTip((Control)(object)buttonSparklinesColumn, Loc.Get("Chart", "SparklinesColumn"));
		toolTip.SetToolTip((Control)(object)buttonSparklinesWinLoss, Loc.Get("Chart", "SparklinesWinLoss"));
	}

	public void RefreshElementView()
	{
		StiDesignerControl designer = Designer;
		if (designer != null)
		{
			((Control)designer).Refresh();
		}
		Designer?.Builder.Redraw();
	}

	protected virtual void SetListHeightFromCount(StiChartDataColumnListBox list)
	{
		((Control)list).Height = ((ListBox)list).ItemHeight + StiScale.I1;
	}

	public virtual void ProcessElementChanging(StiElementChangedArgs args)
	{
		StiElementChangedProcessor.ProcessElementChanging(Sparkline, args);
	}

	private void DoScaling()
	{
		((ButtonBase)buttonSparklinesArea).Image = StiReportDesignImages.Table.SparklinesArea();
		((ButtonBase)buttonSparklinesColumn).Image = StiReportDesignImages.Table.SparklinesColumn();
		((ButtonBase)buttonSparklinesLine).Image = StiReportDesignImages.Table.Sparklines();
		((ButtonBase)buttonSparklinesWinLoss).Image = StiReportDesignImages.Table.SparklinesWinLoss();
	}

	public void ReadPropertiesFromElement()
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		((ListBox)listBoxValues).BeginUpdate();
		((ListBox)listBoxValues).Items.Clear();
		if (!string.IsNullOrWhiteSpace(Sparkline.ValueDataColumn))
		{
			StiDataColumnMeter meter = new StiDataColumnMeter(Sparkline.ValueDataColumn);
			listBoxValues.AddItem(meter);
		}
		listBoxValues.SelectFirstItem();
		((ListBox)listBoxValues).EndUpdate();
		Timer timer = new Timer();
		timer.Interval = 100;
		timer.Tick += delegate
		{
			timer.Stop();
			((Control)listBoxValues).Focus();
			StiDictionaryPanelHelper.ActivatePanel(Designer);
		};
		timer.Start();
	}

	public void WritePropertiesToElement()
	{
		StiDataColumnMeter stiDataColumnMeter = (from t in listBoxValues.ToItems()
			select t.Meter).Cast<StiDataColumnMeter>().FirstOrDefault();
		Sparkline.ValueDataColumn = ((stiDataColumnMeter == null) ? "" : stiDataColumnMeter.DataColumn);
		RefreshElementView();
	}

	protected override void AttachEvents()
	{
		base.AttachEvents();
		AttachEventsToValues();
	}

	private void AttachEventsToValues()
	{
		listBoxValues.ElementsChanged += ListBox_ElementsChanged;
		listBoxValues.ElementsSelectedChanged += ListBox_ElementsAfterSelect;
		listBoxValues.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				StiDataColumnMeter stiDataColumnMeter = new StiDataColumnMeter(draggedDataColumnOrVariable);
				StiChartItem stiChartItem = new StiChartItem(stiDataColumnMeter.DataColumn, stiDataColumnMeter, StiMeterItemImage.Measure);
				((ListBox)listBoxValues).Items.Clear();
				listBoxValues.AddItem(stiChartItem);
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxValues.SelectedChartItem = stiChartItem;
				((Control)listBoxValues).Focus();
				StiDesignerDragDrop.IgnoreAddSelectedToProperties = true;
			}
			RefreshUiPanel();
		};
		listBoxValues.ElementsSelectedChanged += delegate
		{
			Sparkline.Select();
			RefreshElementView();
		};
	}

	public override void RefreshUiPanel()
	{
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		if (base.LockRefresh)
		{
			return;
		}
		base.LockRefresh = true;
		((Control)this).SuspendLayout();
		try
		{
			StiPanelUX stiPanelUX = panelOptions;
			bool enabled = (((Control)textBoxExpression).Visible = IsAnyPresent && listBoxValues.IsCurrentlySelected);
			((Control)stiPanelUX).Enabled = enabled;
			StiPanelUX stiPanelUX2 = panelOptions;
			StiChartGroupLabel stiChartGroupLabel = labelValue;
			bool flag2 = (((Control)panelType).Visible = IsAnyPresent);
			enabled = (((Control)stiChartGroupLabel).Visible = flag2);
			((Control)stiPanelUX2).Visible = enabled;
			((Control)panelColors).Visible = IsAnyPresent;
			if (IsAnyPresent)
			{
				((Control)listBoxValues).Dock = (DockStyle)1;
				SetListHeightFromCount(listBoxValues);
			}
			else
			{
				((Control)listBoxValues).Dock = (DockStyle)5;
			}
			((Control)labelColor).Text = ((((RadioButton)buttonSparklinesColumn).Checked || ((RadioButton)buttonSparklinesWinLoss).Checked) ? Loc.GetMain("Colors") : Loc.GetMain("Color"));
			((RadioButton)buttonSparklinesLine).Checked = Sparkline != null && Sparkline.Type == StiSparklineType.Line;
			((RadioButton)buttonSparklinesArea).Checked = Sparkline != null && Sparkline.Type == StiSparklineType.Area;
			((RadioButton)buttonSparklinesColumn).Checked = Sparkline != null && Sparkline.Type == StiSparklineType.Column;
			((RadioButton)buttonSparklinesWinLoss).Checked = Sparkline != null && Sparkline.Type == StiSparklineType.WinLoss;
			((Control)panelPointsOptions).Visible = IsAnyPresent && (((RadioButton)buttonSparklinesArea).Checked || ((RadioButton)buttonSparklinesLine).Checked);
			colorBoxPositiveColor.SelectedColor = Sparkline.PositiveColor;
			colorBoxNegativeColor.SelectedColor = Sparkline.NegativeColor;
			((Control)colorBoxNegativeColor).Visible = ((RadioButton)buttonSparklinesColumn).Checked || ((RadioButton)buttonSparklinesWinLoss).Checked;
			((CheckBox)checkBoxFirstLastPoints).Checked = Sparkline != null && Sparkline.ShowFirstLastPoints;
			((CheckBox)checkBoxHighLowPoints).Checked = Sparkline != null && Sparkline.ShowHighLowPoints;
			int num;
			if (!IsAnyPresent)
			{
				num = ((Control)panelListBoxes).ClientSize.Width;
			}
			else
			{
				Padding padding = ((Control)panelListBoxes).Padding;
				int top = ((Padding)(ref padding)).Top;
				padding = ((Control)panelListBoxes).Padding;
				num = top + ((Padding)(ref padding)).Bottom + (from Control c in (IEnumerable)((Control)panelListBoxes).Controls
					where c.Visible
					select c.Height).Sum();
			}
			int height = num;
			((Control)panelListBoxes).ClientSize = new Size(((Control)panelListBoxes).ClientSize.Width, height);
			CalculateWindowHeight();
			CalculateWindowPosition();
		}
		finally
		{
			((Control)this).ResumeLayout();
			base.LockRefresh = false;
		}
	}

	protected override void CalculateWindowHeight()
	{
		((Form)this).ClientSize = new Size(((Form)this).ClientSize.Width, base.PaddingCorrection + (from Control c in (IEnumerable)((Control)this).Controls
			where c.Visible
			select c.Height).Sum());
	}

	internal void RefreshExpression()
	{
		base.LockRefresh = true;
		if (selectedListBox == listBoxValues && SparklineValue != null)
		{
			((Control)textBoxExpression).Text = GetExpression(SparklineValue.DataColumn);
		}
		else
		{
			((Control)textBoxExpression).Text = string.Empty;
		}
		base.LockRefresh = false;
	}

	private string GetExpression(string dataColumn)
	{
		if (!string.IsNullOrWhiteSpace(dataColumn))
		{
			return dataColumn;
		}
		return LocNotDefined;
	}

	private void CreateMenuItems()
	{
		if (isItemsInitialized)
		{
			return;
		}
		menuExpression.SubItems.Clear();
		StiDataColumn stiDataColumn = ((((Control)textBoxExpression).Text == LocNotDefined) ? null : StiDataColumn.GetDataColumnFromColumnName(Sparkline.Report.Dictionary, ((Control)textBoxExpression).Text));
		foreach (StiDataSource dataSource in Sparkline.Report.Dictionary.DataSources)
		{
			bool flag = stiDataColumn != null && dataSource.Columns.Contains(stiDataColumn);
			ButtonItem buttonItem = new ButtonItem
			{
				Text = dataSource.ToString(StiOptions.Dictionary.ShowOnlyAliasForDataSource && UseAliases.GetValueOrDefault(false)),
				Image = StiDataBuilder.Images.Images["DataSource"],
				FontBold = flag,
				HotFontBold = flag,
				Tag = dataSource
			};
			menuExpression.SubItems.Add(buttonItem);
			buttonItem.SubItems.Add(new ButtonItem());
			buttonItem.PopupOpen += MenuItemDataSource_PopupOpen;
		}
		isItemsInitialized = true;
	}

	private void ListBox_ElementsChanged(object sender, StiElementChangedArgs changedArgs)
	{
		ProcessElementChanging(changedArgs);
		WritePropertiesToElement();
		RefreshUiPanel();
		if (sender != null)
		{
			StiChartDataColumnListBox stiChartDataColumnListBox = sender as StiChartDataColumnListBox;
			StiSeries stiSeries = stiChartDataColumnListBox.SelectedChartItem?.Meter?.Series;
			if (stiSeries != null)
			{
				StiPropertiesPanelHelper.ActivatePanel(Designer, stiSeries);
			}
			else
			{
				StiDictionaryPanelHelper.ActivatePanel(Designer);
			}
		}
	}

	private void ListBox_ElementsAfterSelect(object sender, EventArgs e)
	{
		selectedListBox = sender as StiChartDataColumnListBox;
		RefreshExpression();
		WritePropertiesToElement();
		RefreshUiPanel();
	}

	private void ListBox_Enter(object sender, EventArgs e)
	{
		selectedListBox = sender as StiChartDataColumnListBox;
		RefreshExpression();
	}

	private void ListBox_AfterSelect(object sender, EventArgs e)
	{
		selectedListBox = sender as StiChartDataColumnListBox;
		RefreshExpression();
	}

	private void ListBox_MouseDown(object sender, MouseEventArgs e)
	{
		selectedListBox = sender as StiChartDataColumnListBox;
		RefreshExpression();
	}

	private void ButtonExpressionMenu_Click(object sender, EventArgs e)
	{
		CreateMenuItems();
		Point point = new Point(((Control)panelBoxExpression).Width, ((Control)panelBoxExpression).Height + StiScale.I1);
		if (StiLeftRightHandedHelper.IsLeftHanded)
		{
			point.X -= menuExpression.PopupSize.Width;
		}
		point = ((Control)panelBoxExpression).PointToScreen(point);
		menuExpression.Popup(point);
	}

	protected void MenuItemDataSource_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		StiDataSource stiDataSource = buttonItem.Tag as StiDataSource;
		StiDataColumn stiDataColumn = ((((Control)textBoxExpression).Text == LocNotDefined) ? null : StiDataColumn.GetDataColumnFromColumnName(Sparkline.Report.Dictionary, ((Control)textBoxExpression).Text));
		buttonItem.SubItems.Clear();
		foreach (StiDataColumn column in stiDataSource.Columns)
		{
			bool flag = stiDataColumn == column;
			ButtonItem buttonItem2 = new ButtonItem
			{
				Checked = flag,
				Image = StiDataBuilder.Images.Images[StiDataBuilder.GetImageKeyFromColumn(column)],
				FontBold = flag,
				HotFontBold = flag,
				Tag = column,
				Text = column.ToString(StiOptions.Dictionary.ShowOnlyAliasForDataColumn && UseAliases.GetValueOrDefault(false))
			};
			buttonItem2.Click += MenuItemDataColumn_Click;
			buttonItem.SubItems.Add(buttonItem2);
		}
	}

	private void MenuItemDataColumn_Click(object sender, EventArgs e)
	{
		ButtonItem item = (ButtonItem)sender;
		(from ButtonItem b in item.Parent.SubItems
			where b != item
			select b).ToList().ForEach(delegate(ButtonItem b)
		{
			bool flag3 = (b.Checked = false);
			bool fontBold2 = (b.HotFontBold = flag3);
			b.FontBold = fontBold2;
		});
		ButtonItem buttonItem = item.Parent as ButtonItem;
		item.Checked = true;
		ButtonItem buttonItem2 = item;
		bool fontBold = (item.HotFontBold = true);
		buttonItem2.FontBold = fontBold;
		StiDataSource stiDataSource = buttonItem.Tag as StiDataSource;
		StiDataColumn stiDataColumn = item.Tag as StiDataColumn;
		string text = stiDataSource.Name + "." + stiDataColumn.NameInSource;
		string text2 = StiCodeDomSerializator.ReplaceSymbols(stiDataSource.Name) + "." + StiCodeDomSerializator.ReplaceSymbols(stiDataColumn.NameInSource);
		((Control)textBoxExpression).Text = text;
		((TextBoxBase)textBoxExpression).Select(0, 0);
		selectedListBox?.SetSelectedItem(text, stiDataColumn);
		RefreshElementView();
		RefreshUiPanel();
	}

	private void ButtonSparklines_CheckedChanged(object sender, EventArgs e)
	{
		if (Sparkline != null && !base.LockRefresh)
		{
			if (((RadioButton)buttonSparklinesLine).Checked)
			{
				Sparkline.Type = StiSparklineType.Line;
			}
			else if (((RadioButton)buttonSparklinesArea).Checked)
			{
				Sparkline.Type = StiSparklineType.Area;
			}
			else if (((RadioButton)buttonSparklinesColumn).Checked)
			{
				Sparkline.Type = StiSparklineType.Column;
			}
			else if (((RadioButton)buttonSparklinesWinLoss).Checked)
			{
				Sparkline.Type = StiSparklineType.WinLoss;
			}
			RefreshElementView();
			RefreshUiPanel();
		}
	}

	private void CheckBoxHighLowPoints_CheckedChanged(object sender, EventArgs e)
	{
		if (Sparkline != null && !base.LockRefresh)
		{
			base.LockRefresh = true;
			Sparkline.ShowHighLowPoints = ((CheckBox)checkBoxHighLowPoints).Checked;
			RefreshElementView();
			base.LockRefresh = false;
		}
	}

	private void CheckBoxFirstLastPoints_CheckedChanged(object sender, EventArgs e)
	{
		if (Sparkline != null && !base.LockRefresh)
		{
			base.LockRefresh = true;
			Sparkline.ShowFirstLastPoints = ((CheckBox)checkBoxFirstLastPoints).Checked;
			RefreshElementView();
			base.LockRefresh = false;
		}
	}

	private void Color_Changed(object sender, EventArgs e)
	{
		if (Sparkline != null && !base.LockRefresh)
		{
			base.LockRefresh = true;
			Sparkline.PositiveColor = colorBoxPositiveColor.SelectedColor;
			Sparkline.NegativeColor = colorBoxNegativeColor.SelectedColor;
			RefreshElementView();
			RefreshUiPanel();
			base.LockRefresh = false;
		}
	}

	protected override void OnFormClosed(FormClosedEventArgs e)
	{
		base.OnFormClosed(e);
		((PropertyGrid)StiPropertiesPanelHelper.GetPropertyGrid(Designer)).SelectedObject = Sparkline;
	}

	protected override void OnClosed(EventArgs e)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Invalid comparison between Unknown and I4
		((Form)this).OnClosed(e);
		lock (StiInplaceEditorForm.FormClosingObject)
		{
			if (!base.IsFormClosed)
			{
				base.IsFormClosed = true;
				if ((int)((Form)this).DialogResult != 1)
				{
					StiActions.Clipboard.InvokeUndo(Designer, closeAllInplaceDesigners: false);
					Designer.UndoRedo.RedoStack.Clear();
					((Control)Designer).Refresh();
				}
			}
		}
	}

	protected override void OnHelpButtonClicked(CancelEventArgs e)
	{
		((Form)this).OnHelpButtonClicked(e);
		StiHelpProvider.ShowHelpDefaultBrowser("user-manual/index.html?report_internals_sparkline.htm#editor");
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		WritePropertiesToElement();
		((Form)this).Close();
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).Close();
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
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Expected O, but got Unknown
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Expected O, but got Unknown
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_062e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_074e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0930: Unknown result type (might be due to invalid IL or missing references)
		//IL_099b: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a5: Expected O, but got Unknown
		//IL_09c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a58: Expected O, but got Unknown
		//IL_0a76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0b: Expected O, but got Unknown
		//IL_0b26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc7: Expected O, but got Unknown
		//IL_0be2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1147: Unknown result type (might be due to invalid IL or missing references)
		//IL_1169: Unknown result type (might be due to invalid IL or missing references)
		//IL_1205: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c0: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiSparklineEditorForm));
		toolTip = new ToolTip(components);
		contextMenuBar = new ContextMenuBar();
		menuExpression = new ButtonItem();
		listBoxValues = new StiChartDataColumnListBox();
		labelValue = new StiChartGroupLabel();
		panelFooter = new StiButtonPanelUX();
		buttonOk = new StiButtonUX();
		buttonCancel = new StiButtonUX();
		panelOptions = new StiPanelUX();
		panelBoxExpression = new StiPanelBox();
		textBoxExpression = new HintTextBox();
		buttonExpressionMenu = new StiOperationButton();
		labelField = new StiLabelUX();
		panelType = new StiPanelUX();
		buttonSparklinesWinLoss = new StiRadioButtonUX();
		buttonSparklinesColumn = new StiRadioButtonUX();
		buttonSparklinesArea = new StiRadioButtonUX();
		buttonSparklinesLine = new StiRadioButtonUX();
		labelType = new StiLabelUX();
		panelPointsOptions = new StiPanelUX();
		checkBoxHighLowPoints = new StiCheckBoxUX();
		checkBoxFirstLastPoints = new StiCheckBoxUX();
		labelColor = new StiLabelUX();
		colorBoxNegativeColor = new ColorPickerButton();
		colorBoxPositiveColor = new ColorPickerButton();
		panelListBoxes = new StiPanelUX();
		panelColors = new StiPanelUX();
		((ISupportInitialize)contextMenuBar).BeginInit();
		((Control)panelFooter).SuspendLayout();
		((Control)panelOptions).SuspendLayout();
		((Control)panelBoxExpression).SuspendLayout();
		((Control)panelType).SuspendLayout();
		((Control)panelPointsOptions).SuspendLayout();
		((Control)panelListBoxes).SuspendLayout();
		((Control)panelColors).SuspendLayout();
		((Control)this).SuspendLayout();
		contextMenuBar.AntiAlias = true;
		contextMenuBar.DockTabStripHeight = 50;
		((Control)contextMenuBar).Font = new Font("Segoe UI", 9f);
		contextMenuBar.Items.AddRange(new BaseItem[1] { menuExpression });
		contextMenuBar.Location = new Point(208, 163);
		((Control)contextMenuBar).Margin = new Padding(2);
		contextMenuBar.Name = "contextMenuBar";
		contextMenuBar.Size = new Size(80, 31);
		contextMenuBar.Stretch = true;
		contextMenuBar.Style = eDotNetBarStyle.StyleManagerControlled;
		contextMenuBar.TabIndex = 11;
		contextMenuBar.TabStop = false;
		((Control)contextMenuBar).Text = "contextMenuBar";
		menuExpression.AutoExpandOnClick = true;
		menuExpression.ImagePaddingHorizontal = 16;
		menuExpression.ImagePaddingVertical = 12;
		menuExpression.Name = "menuExpression";
		menuExpression.Text = "menuExpression";
		((Control)listBoxValues).AllowDrop = true;
		((Control)listBoxValues).Dock = (DockStyle)1;
		((Control)listBoxValues).Location = new Point(12, 44);
		((Control)listBoxValues).Margin = new Padding(2);
		((Control)listBoxValues).Name = "listBoxValues";
		((Control)listBoxValues).Size = new Size(320, 32);
		((Control)listBoxValues).TabIndex = 1;
		((ListBox)listBoxValues).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxValues).Enter += ListBox_Enter;
		((Control)listBoxValues).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelValue).Dock = (DockStyle)1;
		((Control)labelValue).Location = new Point(12, 12);
		((Control)labelValue).Margin = new Padding(2, 0, 2, 0);
		((Control)labelValue).Name = "labelValue";
		((Control)labelValue).Size = new Size(320, 32);
		((Control)labelValue).TabIndex = 0;
		((Control)labelValue).Text = "Values";
		((Control)panelFooter).BackColor = Color.White;
		((Control)panelFooter).Controls.Add((Control)(object)buttonOk);
		((Control)panelFooter).Controls.Add((Control)(object)buttonCancel);
		((Control)panelFooter).Dock = (DockStyle)2;
		panelFooter.LinePosition = StiLinePosition.Top;
		panelFooter.LineStyle = StiLineStyle.Dot;
		((Control)panelFooter).Location = new Point(0, 391);
		((Control)panelFooter).Name = "panelFooter";
		((Control)panelFooter).Size = new Size(344, 46);
		((Control)panelFooter).TabIndex = 1;
		((Control)buttonOk).Anchor = (AnchorStyles)9;
		((Button)buttonOk).DialogResult = (DialogResult)1;
		((Control)buttonOk).Location = new Point(176, 12);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 0;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)buttonCancel).Anchor = (AnchorStyles)9;
		((Button)buttonCancel).DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(256, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 1;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonCancel).Click += This_CancelClick;
		((Control)panelOptions).BackColor = Color.White;
		((Control)panelOptions).Controls.Add((Control)(object)panelBoxExpression);
		((Control)panelOptions).Controls.Add((Control)(object)labelField);
		((Control)panelOptions).Dock = (DockStyle)1;
		((Control)panelOptions).Location = new Point(12, 76);
		((Control)panelOptions).Margin = new Padding(2);
		((Control)panelOptions).Name = "panelOptions";
		((Control)panelOptions).Size = new Size(320, 40);
		((Control)panelOptions).TabIndex = 2;
		((Control)panelBoxExpression).Anchor = (AnchorStyles)13;
		((Control)panelBoxExpression).BackColor = Color.WhiteSmoke;
		((Control)panelBoxExpression).Controls.Add((Control)(object)textBoxExpression);
		((Control)panelBoxExpression).Controls.Add((Control)(object)buttonExpressionMenu);
		((Control)panelBoxExpression).Location = new Point(94, 13);
		((Control)panelBoxExpression).Margin = new Padding(2);
		((Control)panelBoxExpression).Name = "panelBoxExpression";
		((Control)panelBoxExpression).Size = new Size(225, 25);
		((Control)panelBoxExpression).TabIndex = 0;
		((Control)textBoxExpression).Anchor = (AnchorStyles)13;
		((Control)textBoxExpression).BackColor = Color.WhiteSmoke;
		((TextBoxBase)textBoxExpression).BorderStyle = (BorderStyle)0;
		((Control)textBoxExpression).Location = new Point(4, 6);
		((Control)textBoxExpression).Margin = new Padding(2);
		((Control)textBoxExpression).Name = "textBoxExpression";
		((TextBoxBase)textBoxExpression).ReadOnly = true;
		((Control)textBoxExpression).Size = new Size(196, 13);
		((Control)textBoxExpression).TabIndex = 0;
		((Control)buttonExpressionMenu).Anchor = (AnchorStyles)9;
		((Control)buttonExpressionMenu).Location = new Point(201, 1);
		((Control)buttonExpressionMenu).Margin = new Padding(2);
		((Control)buttonExpressionMenu).Name = "buttonExpressionMenu";
		((Control)buttonExpressionMenu).Size = new Size(23, 23);
		((Control)buttonExpressionMenu).TabIndex = 2;
		((Control)buttonExpressionMenu).TabStop = false;
		((ButtonBase)buttonExpressionMenu).UseVisualStyleBackColor = true;
		((Control)buttonExpressionMenu).Click += ButtonExpressionMenu_Click;
		((Control)labelField).ForeColor = Color.DimGray;
		((Control)labelField).Location = new Point(0, 14);
		((Control)labelField).Name = "labelField";
		((Control)labelField).Size = new Size(94, 23);
		((Control)labelField).TabIndex = 9;
		((Control)labelField).Text = "Field";
		((Label)labelField).TextAlign = (ContentAlignment)16;
		((Control)panelType).Controls.Add((Control)(object)buttonSparklinesWinLoss);
		((Control)panelType).Controls.Add((Control)(object)buttonSparklinesColumn);
		((Control)panelType).Controls.Add((Control)(object)buttonSparklinesArea);
		((Control)panelType).Controls.Add((Control)(object)buttonSparklinesLine);
		((Control)panelType).Controls.Add((Control)(object)labelType);
		((Control)panelType).Dock = (DockStyle)1;
		((Control)panelType).Location = new Point(12, 116);
		((Control)panelType).Margin = new Padding(1);
		((Control)panelType).Name = "panelType";
		((Control)panelType).Size = new Size(320, 38);
		((Control)panelType).TabIndex = 3;
		((RadioButton)buttonSparklinesWinLoss).Appearance = (Appearance)1;
		((ButtonBase)buttonSparklinesWinLoss).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonSparklinesWinLoss).Image = (Image)componentResourceManager.GetObject("buttonSparklinesWinLoss.Image");
		((Control)buttonSparklinesWinLoss).Location = new Point(184, 10);
		((Control)buttonSparklinesWinLoss).Margin = new Padding(2);
		((Control)buttonSparklinesWinLoss).Name = "buttonSparklinesWinLoss";
		((Control)buttonSparklinesWinLoss).Size = new Size(26, 26);
		((Control)buttonSparklinesWinLoss).TabIndex = 3;
		((ButtonBase)buttonSparklinesWinLoss).UseVisualStyleBackColor = true;
		((RadioButton)buttonSparklinesWinLoss).CheckedChanged += ButtonSparklines_CheckedChanged;
		((RadioButton)buttonSparklinesColumn).Appearance = (Appearance)1;
		((ButtonBase)buttonSparklinesColumn).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonSparklinesColumn).Image = (Image)componentResourceManager.GetObject("buttonSparklinesColumn.Image");
		((Control)buttonSparklinesColumn).Location = new Point(154, 10);
		((Control)buttonSparklinesColumn).Margin = new Padding(2);
		((Control)buttonSparklinesColumn).Name = "buttonSparklinesColumn";
		((Control)buttonSparklinesColumn).Size = new Size(26, 26);
		((Control)buttonSparklinesColumn).TabIndex = 2;
		((ButtonBase)buttonSparklinesColumn).UseVisualStyleBackColor = true;
		((RadioButton)buttonSparklinesColumn).CheckedChanged += ButtonSparklines_CheckedChanged;
		((RadioButton)buttonSparklinesArea).Appearance = (Appearance)1;
		((ButtonBase)buttonSparklinesArea).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonSparklinesArea).Image = (Image)componentResourceManager.GetObject("buttonSparklinesArea.Image");
		((Control)buttonSparklinesArea).Location = new Point(124, 10);
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
		((Control)buttonSparklinesLine).Location = new Point(94, 10);
		((Control)buttonSparklinesLine).Margin = new Padding(2);
		((Control)buttonSparklinesLine).Name = "buttonSparklinesLine";
		((Control)buttonSparklinesLine).Size = new Size(26, 26);
		((Control)buttonSparklinesLine).TabIndex = 0;
		((RadioButton)buttonSparklinesLine).TabStop = true;
		((ButtonBase)buttonSparklinesLine).UseVisualStyleBackColor = true;
		((RadioButton)buttonSparklinesLine).CheckedChanged += ButtonSparklines_CheckedChanged;
		((Control)labelType).ForeColor = Color.DimGray;
		((Control)labelType).Location = new Point(0, 10);
		((Control)labelType).Margin = new Padding(2, 0, 2, 0);
		((Control)labelType).Name = "labelType";
		((Control)labelType).Size = new Size(90, 26);
		((Control)labelType).TabIndex = 8;
		((Control)labelType).Text = "Type";
		((Label)labelType).TextAlign = (ContentAlignment)16;
		((Control)panelPointsOptions).Controls.Add((Control)(object)checkBoxHighLowPoints);
		((Control)panelPointsOptions).Controls.Add((Control)(object)checkBoxFirstLastPoints);
		((Control)panelPointsOptions).Dock = (DockStyle)1;
		((Control)panelPointsOptions).Location = new Point(12, 154);
		((Control)panelPointsOptions).Margin = new Padding(1);
		((Control)panelPointsOptions).Name = "panelPointsOptions";
		((Control)panelPointsOptions).Size = new Size(320, 57);
		((Control)panelPointsOptions).TabIndex = 4;
		((Control)checkBoxHighLowPoints).Anchor = (AnchorStyles)13;
		((ButtonBase)checkBoxHighLowPoints).AutoEllipsis = true;
		((Control)checkBoxHighLowPoints).Location = new Point(94, 11);
		((Control)checkBoxHighLowPoints).Margin = new Padding(2);
		((Control)checkBoxHighLowPoints).Name = "checkBoxHighLowPoints";
		((Control)checkBoxHighLowPoints).Size = new Size(222, 18);
		((Control)checkBoxHighLowPoints).TabIndex = 0;
		((Control)checkBoxHighLowPoints).Text = "High / Low Points";
		((ButtonBase)checkBoxHighLowPoints).UseVisualStyleBackColor = true;
		((CheckBox)checkBoxHighLowPoints).CheckedChanged += CheckBoxHighLowPoints_CheckedChanged;
		((Control)checkBoxFirstLastPoints).Anchor = (AnchorStyles)13;
		((ButtonBase)checkBoxFirstLastPoints).AutoEllipsis = true;
		((Control)checkBoxFirstLastPoints).Location = new Point(94, 39);
		((Control)checkBoxFirstLastPoints).Margin = new Padding(2);
		((Control)checkBoxFirstLastPoints).Name = "checkBoxFirstLastPoints";
		((Control)checkBoxFirstLastPoints).Size = new Size(222, 18);
		((Control)checkBoxFirstLastPoints).TabIndex = 1;
		((Control)checkBoxFirstLastPoints).Text = "First / Last Points";
		((ButtonBase)checkBoxFirstLastPoints).UseVisualStyleBackColor = true;
		((CheckBox)checkBoxFirstLastPoints).CheckedChanged += CheckBoxFirstLastPoints_CheckedChanged;
		((Control)labelColor).ForeColor = Color.DimGray;
		((Control)labelColor).Location = new Point(0, 10);
		((Control)labelColor).Margin = new Padding(2, 0, 2, 0);
		((Control)labelColor).Name = "labelColor";
		((Control)labelColor).Size = new Size(90, 24);
		((Control)labelColor).TabIndex = 9;
		((Control)labelColor).Text = "Color";
		((Label)labelColor).TextAlign = (ContentAlignment)16;
		((Control)colorBoxNegativeColor).AccessibleRole = (AccessibleRole)43;
		colorBoxNegativeColor.AutoExpandOnClick = true;
		colorBoxNegativeColor.ColorTable = eButtonColor.OrangeWithBackground;
		colorBoxNegativeColor.EnableMarkup = false;
		colorBoxNegativeColor.Is20Button = false;
		((Control)colorBoxNegativeColor).Location = new Point(192, 12);
		((Control)colorBoxNegativeColor).Name = "colorBoxNegativeColor";
		((Control)colorBoxNegativeColor).Size = new Size(93, 24);
		((Control)colorBoxNegativeColor).TabIndex = 3;
		((Control)colorBoxNegativeColor).TabStop = false;
		colorBoxNegativeColor.TextAlignment = eButtonTextAlignment.Right;
		colorBoxNegativeColor.SelectedColorChanged += Color_Changed;
		((Control)colorBoxPositiveColor).AccessibleRole = (AccessibleRole)43;
		colorBoxPositiveColor.AutoExpandOnClick = true;
		colorBoxPositiveColor.ColorTable = eButtonColor.OrangeWithBackground;
		colorBoxPositiveColor.EnableMarkup = false;
		colorBoxPositiveColor.Is20Button = false;
		((Control)colorBoxPositiveColor).Location = new Point(94, 12);
		((Control)colorBoxPositiveColor).Name = "colorBoxPositiveColor";
		((Control)colorBoxPositiveColor).Size = new Size(93, 24);
		((Control)colorBoxPositiveColor).TabIndex = 1;
		((Control)colorBoxPositiveColor).TabStop = false;
		colorBoxPositiveColor.TextAlignment = eButtonTextAlignment.Right;
		colorBoxPositiveColor.SelectedColorChanged += Color_Changed;
		((Panel)panelListBoxes).AutoSizeMode = (AutoSizeMode)0;
		((Control)panelListBoxes).Controls.Add((Control)(object)panelColors);
		((Control)panelListBoxes).Controls.Add((Control)(object)panelPointsOptions);
		((Control)panelListBoxes).Controls.Add((Control)(object)panelType);
		((Control)panelListBoxes).Controls.Add((Control)(object)panelOptions);
		((Control)panelListBoxes).Controls.Add((Control)(object)listBoxValues);
		((Control)panelListBoxes).Controls.Add((Control)(object)labelValue);
		((Control)panelListBoxes).Dock = (DockStyle)1;
		((Control)panelListBoxes).Location = new Point(0, 0);
		((Control)panelListBoxes).Margin = new Padding(2);
		((Control)panelListBoxes).Name = "panelListBoxes";
		((Control)panelListBoxes).Padding = new Padding(12);
		((Control)panelListBoxes).Size = new Size(344, 336);
		((Control)panelListBoxes).TabIndex = 0;
		((Control)panelColors).Controls.Add((Control)(object)labelColor);
		((Control)panelColors).Controls.Add((Control)(object)colorBoxPositiveColor);
		((Control)panelColors).Controls.Add((Control)(object)colorBoxNegativeColor);
		((Control)panelColors).Dock = (DockStyle)1;
		((Control)panelColors).Location = new Point(12, 211);
		((Control)panelColors).Margin = new Padding(1);
		((Control)panelColors).Name = "panelColors";
		((Control)panelColors).Size = new Size(320, 38);
		((Control)panelColors).TabIndex = 5;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(344, 437);
		((Control)this).Controls.Add((Control)(object)panelListBoxes);
		((Control)this).Controls.Add((Control)(object)contextMenuBar);
		((Control)this).Controls.Add((Control)(object)panelFooter);
		((Form)this).Margin = new Padding(0);
		((Control)this).MinimumSize = new Size(319, 60);
		((Control)this).Name = "StiSparklineEditorForm";
		((Control)this).Text = " ";
		((ISupportInitialize)contextMenuBar).EndInit();
		((Control)panelFooter).ResumeLayout(false);
		((Control)panelOptions).ResumeLayout(false);
		((Control)panelBoxExpression).ResumeLayout(false);
		((Control)panelBoxExpression).PerformLayout();
		((Control)panelType).ResumeLayout(false);
		((Control)panelPointsOptions).ResumeLayout(false);
		((Control)panelListBoxes).ResumeLayout(false);
		((Control)panelColors).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
