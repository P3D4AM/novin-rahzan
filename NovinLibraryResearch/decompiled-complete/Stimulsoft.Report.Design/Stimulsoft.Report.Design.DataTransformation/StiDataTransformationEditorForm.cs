using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Menu;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Controls.Win.DotNetBar.Controls;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Exceptions;
using Stimulsoft.Data.Expressions.NCalc;
using Stimulsoft.Data.Functions;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Design.DataTransformation.Controls;
using Stimulsoft.Report.Design.DataTransformation.Menu;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Design.Inplace;
using Stimulsoft.Report.Design.Panels;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Dictionary.Design;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Design.DataTransformation;

public class StiDataTransformationEditorForm : StiInplaceEditorForm
{
	private StiDataTransformation previousTransformation;

	private bool isDragOverFromDictionary;

	private StiTypeWrapper[] types;

	private string oldValue;

	private bool isFormClosing;

	private Exception exception;

	private bool isAnalysisRunning;

	private bool isAnalysisRunAgain;

	private bool textBoxExpressionChanged;

	private bool isUpdatingDataGrid;

	private int listBoxColumnsSelectedIndex;

	private Hashtable prevColumnTypes;

	private StiDataTransformationExpressionMenu expressionMenu;

	private StiDataTransformationSortFilterActionMenu sortFilterMenu;

	private StiContextMenu sortFilterContextMenu;

	private IContainer components;

	private StiDataTransformColumnListBox listBoxColumns;

	private RadioButton buttonMeasure;

	private RadioButton buttonDimension;

	private Label labelMode;

	private ToolTip toolTip;

	private Label labelExpression;

	private StiPanelBox panelBoxExpression;

	private HintTextBox textBoxExpression;

	private StiOperationButton buttonExpressionMenu;

	private ContextMenuBar contextMenuBar;

	private ButtonItem menuExpression;

	private StiGroupLine stiGroupLine1;

	private Button buttonOk;

	private Button buttonCancel;

	private Label labelName;

	public StiTextBox textBoxName;

	private Label labelAlias;

	public StiTextBox textBoxAlias;

	private StiDataGridView dataGridSample;

	public StiTextBox textBoxNameInSource;

	private Label labelNameInSource;

	private StiOffice2013GroupBox groupBoxSettings;

	private Panel panelSample;

	private CircularProgress progressPreview;

	private BackgroundWorker workerPreview;

	public StiComboBox comboBoxType;

	private Label labelType;

	private Panel panelTop;

	private Label labelErrorMessage;

	private ImageList imageListTypes;

	private bool IsElementTransformation { get; }

	internal StiDesignerControl Designer => Transformation.Dictionary.Report.Designer.DesignerControl as StiDesignerControl;

	internal StiDataGridView DataGridSample => dataGridSample;

	internal DataTable DataTableSample { get; set; }

	internal StiDataTransformation Transformation { get; set; }

	internal bool IsNewTransformation => previousTransformation == null;

	internal StiDataTransformationColumn SelectedColumn => listBoxColumns.SelectedDataColumnItem;

	private StiTypeWrapper SelectedColumnType
	{
		get
		{
			StiTypeWrapper stiTypeWrapper = ((SelectedColumn != null) ? types.FirstOrDefault((StiTypeWrapper c) => c.Type == SelectedColumn.Type) : null);
			if (stiTypeWrapper != null)
			{
				return stiTypeWrapper;
			}
			return types.FirstOrDefault((StiTypeWrapper c) => c.Type == typeof(object));
		}
		set
		{
			if (SelectedColumn != null)
			{
				SelectedColumn.Type = ((value == null) ? typeof(object) : value.Type);
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override bool AllowResize => true;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override bool IsAnyPresent => ((ListBox)listBoxColumns).Items.Count > 0;

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((Form)this).CreateParams;
			createParams.ExStyle |= 0x2000000;
			return createParams;
		}
	}

	public StiDataTransformationEditorForm()
		: this(null)
	{
	}

	public StiDataTransformationEditorForm(StiDataTransformation transformation, bool isElementTransformation = false, StiDataTransformationRestrictions restrictions = StiDataTransformationRestrictions.None, bool activateDictionary = true)
	{
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		IsElementTransformation = isElementTransformation;
		if (IsElementTransformation)
		{
			((Control)this).Name = "StiElementTransformationEditorForm";
			((Form)this).StartPosition = (FormStartPosition)4;
		}
		if (StiOptions.Appearance.ShowFormsHelp && StiLocalization.CultureName == "ru")
		{
			((Form)this).HelpButton = true;
		}
		if (!((Control)this).MinimumSize.IsEmpty)
		{
			((Form)this).Size = ((Control)this).MinimumSize;
		}
		Transformation = transformation;
		if (transformation.Dictionary.DataSources.IndexOf(transformation) >= 0)
		{
			previousTransformation = transformation;
			Transformation = CopyTransformationProperties(transformation, null);
		}
		ReadPropertiesFromTransformation();
		LocalizeInternal();
		InitializeColumnTypes();
		if (activateDictionary)
		{
			ActivateDictionaryPanel();
		}
		listBoxColumns.Editor = this;
		ObjectCollection items = ((ComboBox)comboBoxType).Items;
		object[] array = types;
		items.AddRange(array);
		expressionMenu = new StiDataTransformationExpressionMenu(menuExpression, (TextBox)(object)textBoxExpression, this, listBoxColumns, (ComboBox)(object)comboBoxType);
		contextMenuBar.SetContextMenuEx((Control)(object)listBoxColumns, menuExpression);
		sortFilterContextMenu = new StiContextMenu();
		sortFilterMenu = new StiDataTransformationSortFilterActionMenu(sortFilterContextMenu, Transformation, (Form)(object)this, restrictions);
		sortFilterMenu.SortChanged += SortFilterMenu_Changed;
		sortFilterMenu.FiltersChanged += SortFilterMenu_Changed;
		sortFilterMenu.ActionsChanged += SortFilterMenu_Changed;
		if (IsElementTransformation)
		{
			SwitchToElementTransformationMode();
		}
		DoScaling();
	}

	public override void Localize()
	{
		LocalizeInternal();
		((Control)buttonOk).Text = Loc.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = Loc.Get("Buttons", "Cancel");
		((Control)listBoxColumns).Invalidate();
	}

	private void LocalizeInternal()
	{
		if (IsElementTransformation)
		{
			((Control)this).Text = Loc.Get("PropertyMain", "DataTransformation");
		}
		else
		{
			((Control)this).Text = Loc.Get("FormDictionaryDesigner", IsNewTransformation ? "DataTransformationNew" : "DataTransformationEdit");
		}
		((Control)labelNameInSource).Text = Loc.GetCleaned("Report", "LabelNameInSource");
		((Control)labelName).Text = Loc.GetCleaned("Report", "LabelName");
		((Control)labelAlias).Text = Loc.GetCleaned("Report", "LabelAlias");
		((Control)groupBoxSettings).Text = Loc.Get("FormBand", "title");
		((Control)labelMode).Text = Loc.GetCleaned("PropertyMain", "Mode");
		((Control)labelExpression).Text = Loc.GetCleaned("Report", "LabelExpression");
		((Control)labelType).Text = Loc.GetCleaned("Report", "LabelType");
		toolTip.SetToolTip((Control)(object)buttonDimension, Loc.Get("Dashboard", "Dimension"));
		toolTip.SetToolTip((Control)(object)buttonMeasure, Loc.Get("Dashboard", "Measure"));
		((Control)listBoxColumns).Invalidate();
		((Control)dataGridSample).Invalidate();
	}

	protected override void AttachEvents()
	{
		base.AttachEvents();
		listBoxColumns.ElementsChanged += ListBoxColumns_ElementsChanged;
		listBoxColumns.ElementsDragDrop += ListBoxColumns_ElementsDragDrop;
		listBoxColumns.ElementsSelectedChanged += ListBoxColumns_ElementsSelectedChanged;
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
			((Control)textBoxExpression).Visible = SelectedColumn != null;
			((Control)panelBoxExpression).Enabled = SelectedColumn != null;
			((Control)buttonDimension).Enabled = SelectedColumn != null;
			((Control)buttonMeasure).Enabled = SelectedColumn != null;
			((Control)comboBoxType).Enabled = SelectedColumn != null;
			buttonDimension.Checked = SelectedColumn != null && SelectedColumn.Mode == StiDataTransformationMode.Dimension;
			buttonMeasure.Checked = SelectedColumn != null && SelectedColumn.Mode == StiDataTransformationMode.Measure;
			((Control)textBoxExpression).Text = ((SelectedColumn != null) ? SelectedColumn.Expression : "");
			((ComboBox)comboBoxType).SelectedItem = SelectedColumnType;
			CalculateWindowPosition();
			RefreshExpressionHint();
		}
		finally
		{
			((Control)this).ResumeLayout();
			base.LockRefresh = false;
		}
	}

	private void DoScaling()
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		((ButtonBase)buttonDimension).Image = StiReportDesignImages.Actions.Dimension();
		((ButtonBase)buttonMeasure).Image = StiReportDesignImages.Actions.Measure();
		((DataGridView)dataGridSample).ColumnHeadersHeight = StiScale.I(24);
		double step = StiScale.Step;
		if (step >= 1.5)
		{
			string stepName = StiScale.StepName;
			imageListTypes = new ImageList();
			imageListTypes.TransparentColor = Color.Transparent;
			Bitmap image = StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.VariableString" + stepName + ".png");
			imageListTypes.ImageSize = ((Image)image).Size;
			imageListTypes.Images.Add("String", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.VariableString" + stepName + ".png"));
			imageListTypes.Images.Add("Binary", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.VariableBinary" + stepName + ".png"));
			imageListTypes.Images.Add("Bool", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.VariableBool" + stepName + ".png"));
			imageListTypes.Images.Add("Char", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.VariableChar" + stepName + ".png"));
			imageListTypes.Images.Add("DateTime", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.VariableDateTime" + stepName + ".png"));
			imageListTypes.Images.Add("Decimal", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.VariableDecimal" + stepName + ".png"));
			imageListTypes.Images.Add("Float", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.VariableFloat" + stepName + ".png"));
			imageListTypes.Images.Add("Image", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.VariableImage" + stepName + ".png"));
			imageListTypes.Images.Add("Int", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.VariableInt" + stepName + ".png"));
		}
	}

	private StiDataTransformation CopyTransformationProperties(StiDataTransformation source, StiDataTransformation destination)
	{
		bool flag = false;
		if (destination == null)
		{
			destination = new StiDataTransformation();
			flag = true;
		}
		destination.NameInSource = source.NameInSource;
		destination.Name = source.Name;
		destination.Alias = source.Alias;
		destination.Dictionary = source.Dictionary;
		destination.Key = source.Key;
		destination.Columns.Clear();
		destination.Sorts.Clear();
		destination.Filters.Clear();
		destination.Actions.Clear();
		if (flag)
		{
			source.Columns.ToList().ForEach(delegate(StiDataColumn c)
			{
				StiDataColumn stiDataColumn = (StiDataColumn)c.Clone();
				stiDataColumn.Name = StiNameCreation.CreateColumnName(destination, stiDataColumn.Name);
				destination.Columns.Add(stiDataColumn);
			});
			source.Sorts.ForEach(delegate(StiDataSortRule i)
			{
				destination.Sorts.Add((StiDataSortRule)i.Clone());
			});
			source.Filters.ForEach(delegate(StiDataFilterRule i)
			{
				destination.Filters.Add((StiDataFilterRule)i.Clone());
			});
			source.Actions.ForEach(delegate(StiDataActionRule i)
			{
				destination.Actions.Add((StiDataActionRule)i.Clone());
			});
		}
		else
		{
			source.Columns.ToList().ForEach(delegate(StiDataColumn i)
			{
				destination.Columns.Add(i);
			});
			source.Sorts.ForEach(delegate(StiDataSortRule i)
			{
				destination.Sorts.Add(i);
			});
			source.Filters.ForEach(delegate(StiDataFilterRule i)
			{
				destination.Filters.Add(i);
			});
			source.Actions.ForEach(delegate(StiDataActionRule i)
			{
				destination.Actions.Add(i);
			});
		}
		return destination;
	}

	internal void ReadPropertiesFromTransformation()
	{
		((Control)textBoxNameInSource).Text = Transformation.NameInSource;
		((Control)textBoxName).Text = Transformation.Name;
		((Control)textBoxAlias).Text = Transformation.Alias;
		((ListBox)listBoxColumns).BeginUpdate();
		((ListBox)listBoxColumns).Items.Clear();
		Transformation.Columns.ToList().ForEach(delegate(StiDataColumn c)
		{
			c.Key = StiKeyHelper.GetOrGeneratedKey(c.Key);
			((ListBox)listBoxColumns).Items.Add((object)c);
		});
		listBoxColumns.SelectFirstItem();
		((ListBox)listBoxColumns).EndUpdate();
	}

	public void WritePropertiesToTransformation()
	{
		Transformation.NameInSource = ((Control)textBoxNameInSource).Text;
		Transformation.Name = ((Control)textBoxName).Text;
		Transformation.Alias = ((Control)textBoxAlias).Text;
		WriteParametersToTransformation();
	}

	public void WriteParametersToTransformation()
	{
		Transformation.Columns.Clear();
		listBoxColumns.ToItems().ToList().ForEach(delegate(StiDataTransformationColumn c)
		{
			Transformation.Columns.Add(c);
		});
		List<string> columnKeys = (from c in listBoxColumns.ToItems()
			select c.Key).ToList();
		Transformation.Sorts = StiDataSortRuleHelper.Validate(Transformation.Sorts, columnKeys);
		Transformation.Filters = StiDataFilterRuleHelper.Validate(Transformation.Filters, columnKeys);
		Transformation.Actions = StiDataActionRuleHelper.Validate(Transformation.Actions, columnKeys);
	}

	internal static StiDataTransformationColumn CreateTransformationColumnFromDataColumn(StiDataColumn dataColumn)
	{
		bool flag = dataColumn?.Type != null && dataColumn.Type.IsNumericType();
		return new StiDataTransformationColumn
		{
			Key = StiKeyHelper.GenerateKey(),
			Name = dataColumn.Name,
			Alias = dataColumn.Alias,
			Mode = (flag ? StiDataTransformationMode.Measure : StiDataTransformationMode.Dimension),
			Type = dataColumn?.Type,
			Expression = (flag ? ("Sum(" + Funcs.ToExpression(dataColumn.DataSource.Name, dataColumn.Name) + ")") : Funcs.ToExpression(dataColumn.DataSource.Name, dataColumn.Name))
		};
	}

	private void InitializeColumnTypes()
	{
		types = StiTypeWrapper.GetBaseTypeWrappers();
	}

	public void RefreshElementView()
	{
		if (isAnalysisRunning)
		{
			isAnalysisRunAgain = true;
			return;
		}
		isAnalysisRunning = true;
		exception = null;
		progressPreview.Value = 0;
		progressPreview.IsRunning = true;
		((Control)progressPreview).Visible = true;
		((Control)dataGridSample).Visible = false;
		workerPreview.RunWorkerAsync();
	}

	private void RefreshExpressionHint()
	{
		if (buttonDimension.Checked)
		{
			textBoxExpression.Hint = Loc.GetMain("Field");
		}
		else if (buttonMeasure.Checked)
		{
			textBoxExpression.Hint = "Sum(" + Loc.GetMain("Field") + ")";
		}
		else
		{
			textBoxExpression.Hint = "";
		}
	}

	protected void ActivateDictionaryPanel()
	{
		try
		{
			StiDictionaryPanelService dictionaryService = Designer.GetDictionaryService();
			if (dictionaryService != null && dictionaryService.DockingBar != null && dictionaryService.DockingItem != null && dictionaryService.DockingBar.Items.Contains(dictionaryService.DockingItem))
			{
				dictionaryService.DockingBar.SelectedDockContainerItem = dictionaryService.DockingItem;
			}
		}
		catch
		{
		}
	}

	private void SwitchToElementTransformationMode()
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		(from Control c in (IEnumerable)((Control)this).Controls
			where c != DataGridSample
			select c).ToList().ForEach(delegate(Control c)
		{
			c.Visible = false;
		});
		Label val = new Label
		{
			TextAlign = (ContentAlignment)16,
			ForeColor = Color.DimGray,
			Left = StiScale.XXI(12.0),
			Width = ((Control)this).ClientRectangle.Width - StiScale.XXI(24.0),
			Anchor = (AnchorStyles)14,
			UseCompatibleTextRendering = false,
			Text = Loc.Get("Dashboard", "TransformationHint")
		};
		((Control)val).Height = StiScale.YYI(((Control)val).GetPreferredSize(new Size(((Control)val).Width, ((Control)this).ClientRectangle.Height)).Height + 10);
		((Control)val).Top = ((Control)this).ClientRectangle.Height - ((Control)val).Height - StiScale.XXI(12.0);
		((Control)this).Controls.Add((Control)(object)val);
		((Control)DataGridSample).Parent.Controls.Remove((Control)(object)DataGridSample);
		((Control)DataGridSample).Dock = (DockStyle)0;
		((Control)DataGridSample).Left = StiScale.XXI(12.0);
		((Control)DataGridSample).Top = StiScale.XXI(12.0);
		((Control)DataGridSample).Width = ((Control)this).ClientRectangle.Width - StiScale.XXI(24.0);
		((Control)DataGridSample).Height = ((Control)val).Top - StiScale.YYI(12.0) - ((Control)DataGridSample).Top;
		((Control)DataGridSample).Anchor = (AnchorStyles)15;
		((Control)this).Controls.Add((Control)(object)DataGridSample);
	}

	internal void InvalidateDataGridSampleHeaders()
	{
		for (int i = 0; i < ((DataGridView)DataGridSample).ColumnCount; i++)
		{
			((DataGridView)DataGridSample).InvalidateCell(i, -1);
		}
	}

	private void RestoreDataGridSelection()
	{
		if (dataGridSample != null && ((DataGridView)dataGridSample).Rows.Count > 0 && ((BaseCollection)((DataGridView)dataGridSample).Columns).Count > 0)
		{
			DataGridViewCell currentCell = ((DataGridView)dataGridSample).CurrentCell;
			if (((currentCell != null) ? new int?(currentCell.ColumnIndex) : null) != listBoxColumnsSelectedIndex)
			{
				int dataGridColumn = Math.Min((listBoxColumnsSelectedIndex >= 0) ? listBoxColumnsSelectedIndex : 0, ((BaseCollection)((DataGridView)dataGridSample).Columns).Count - 1);
				int dataGridRow = ((((DataGridView)dataGridSample).CurrentCell != null) ? ((DataGridView)dataGridSample).CurrentCell.RowIndex : 0);
				((Control)dataGridSample).Invoke((Delegate)(Action)delegate
				{
					((DataGridView)dataGridSample).CurrentCell = ((DataGridView)dataGridSample)[dataGridColumn, dataGridRow];
				});
			}
		}
		isUpdatingDataGrid = false;
	}

	internal bool IsDragDropAllowed(StiDesignerControl designer)
	{
		StiDataSource draggedDataSource = StiDragDropColumnHelper.GetDraggedDataSource(designer);
		if (draggedDataSource != null)
		{
			return !(draggedDataSource is StiDataTransformation);
		}
		return false;
	}

	private void ShowErrorMessage(string message)
	{
		((Control)labelErrorMessage).Invoke((Delegate)(Action)delegate
		{
			((Control)labelErrorMessage).Text = message;
			((Control)labelErrorMessage).Visible = true;
			((Control)buttonOk).Enabled = false;
		});
	}

	private void HideErrorMessage()
	{
		((Control)labelErrorMessage).Invoke((Delegate)(Action)delegate
		{
			((Control)labelErrorMessage).Visible = false;
			((Control)buttonOk).Enabled = true;
		});
	}

	private void SaveDataTransformation()
	{
		Designer.UndoRedoSave(StiLocalization.Get("FormDictionaryDesigner", IsNewTransformation ? "DataTransformationNew" : "DataTransformationEdit"));
		WritePropertiesToTransformation();
		if (IsNewTransformation)
		{
			Transformation.Dictionary.DataSources.Add(Transformation);
		}
		else
		{
			CopyTransformationProperties(Transformation, previousTransformation);
		}
		Designer.BuildDictionary();
		StiDictionaryPanelService dictionaryService = Designer.GetDictionaryService();
		previousTransformation = Transformation;
		dictionaryService?.DictionaryControl?.SelectNodeByKey(previousTransformation.Key);
		StiCacheCleaner.Clean(Designer.Report);
	}

	protected override void OnGlobalElementChanged(object sender, EventArgs e)
	{
		if (sender == Transformation)
		{
			IEnumerable<StiDataTransformationColumn> second = from c in listBoxColumns.ToItems()
				where c != null
				select c;
			IEnumerable<StiDataTransformationColumn> dif = Transformation.Columns.Cast<StiDataTransformationColumn>().Except(second);
			ReadPropertiesFromTransformation();
			listBoxColumns.SelectedDataColumnItem = listBoxColumns.ToItems().FirstOrDefault((StiDataTransformationColumn t) => t == dif.FirstOrDefault());
		}
	}

	private void ComboBoxType_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			SelectedColumnType = ((ComboBox)comboBoxType).SelectedItem as StiTypeWrapper;
			RefreshElementView();
		}
	}

	private void TextBoxName_Enter(object sender, EventArgs e)
	{
		oldValue = ((Control)textBoxName).Text;
	}

	private void TextBoxName_Leave(object sender, EventArgs e)
	{
		if (oldValue == ((Control)textBoxAlias).Text)
		{
			((Control)textBoxAlias).Text = ((Control)textBoxName).Text;
		}
	}

	private void ButtonMeter_CheckedChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (base.LockRefresh || SelectedColumn == null || !((RadioButton)sender).Checked)
		{
			return;
		}
		if (prevColumnTypes == null)
		{
			prevColumnTypes = new Hashtable();
		}
		if (buttonMeasure.Checked)
		{
			prevColumnTypes[SelectedColumn] = SelectedColumn.Type;
		}
		SelectedColumn.Mode = ((!buttonDimension.Checked) ? StiDataTransformationMode.Measure : StiDataTransformationMode.Dimension);
		if (SelectedColumn.Mode == StiDataTransformationMode.Dimension)
		{
			SelectedColumn.Expression = StiExpressionHelper.RemoveFunction(SelectedColumn.Expression);
			if (prevColumnTypes.Contains(SelectedColumn))
			{
				SelectedColumn.Type = (Type)prevColumnTypes[SelectedColumn];
			}
			else
			{
				StiDataColumn dictionaryColumn = SelectedColumn.GetDictionaryColumn();
				SelectedColumn.Type = ((dictionaryColumn != null) ? dictionaryColumn.Type : typeof(object));
			}
		}
		else if (SelectedColumn.Mode == StiDataTransformationMode.Measure)
		{
			bool flag = StiDataExpressionHelper.IsNumericDataColumnInExpression(Transformation, SelectedColumn.Expression);
			SelectedColumn.Expression = StiExpressionHelper.ReplaceFunction(SelectedColumn.Expression, flag ? "Sum" : "Count");
			SelectedColumn.Type = (flag ? typeof(decimal) : typeof(int));
		}
		listBoxColumns.RefreshSelectedImage();
		RefreshElementView();
		RefreshUiPanel();
	}

	private void ComboBoxType_DrawItem(object sender, DrawItemEventArgs e)
	{
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Invalid comparison between Unknown and I4
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0426: Expected O, but got Unknown
		if (e.Index == -1)
		{
			return;
		}
		string text = null;
		StiTypeWrapper stiTypeWrapper = ((ComboBox)comboBoxType).Items[e.Index] as StiTypeWrapper;
		Type type = stiTypeWrapper.Type;
		if (Nullable.GetUnderlyingType(type) != null)
		{
			type = Nullable.GetUnderlyingType(type);
		}
		text = ((type == typeof(float)) ? "Float" : ((type == typeof(double)) ? "Float" : ((type == typeof(decimal)) ? "Decimal" : ((type == typeof(DateTime)) ? "DateTime" : ((type == typeof(TimeSpan)) ? "DateTime" : ((type == typeof(bool)) ? "Bool" : ((type == typeof(sbyte)) ? "Int" : ((type == typeof(byte)) ? "Int" : ((type == typeof(short)) ? "Int" : ((type == typeof(ushort)) ? "Int" : ((type == typeof(int)) ? "Int" : ((type == typeof(uint)) ? "Int" : ((type == typeof(long)) ? "Int" : ((type == typeof(ulong)) ? "Int" : ((type == typeof(Image)) ? "Image" : ((!(type == typeof(char))) ? "String" : "Char"))))))))))))))));
		Graphics graphics = e.Graphics;
		Brush val = SystemBrushes.Window;
		Brush val2 = SystemBrushes.WindowText;
		if ((e.State & 0x10) > 0)
		{
			val = SystemBrushes.Highlight;
			val2 = SystemBrushes.HighlightText;
		}
		graphics.FillRectangle(val, e.Bounds);
		if ((e.State & 0x10) == 0 && e.Bounds.Top > 10 && (type == typeof(float) || type == typeof(DateTime) || type == typeof(bool) || type == typeof(sbyte) || type == typeof(Image)))
		{
			graphics.DrawLine(Pens.Gray, e.Bounds.X, e.Bounds.Y, e.Bounds.Right, e.Bounds.Y);
		}
		double step = StiScale.Step;
		Image val3 = imageListTypes.Images[text];
		if (val3 != null)
		{
			Rectangle rectangle = new Rectangle(e.Bounds.Left + (int)Math.Ceiling(2.0 * step), e.Bounds.Top + (e.Bounds.Height - (int)(16.0 * step)) / 2, (int)Math.Ceiling(16.0 * step), (int)Math.Ceiling(16.0 * step));
			graphics.DrawImage(val3, rectangle);
		}
		Rectangle rectangle2 = new Rectangle(e.Bounds.Left + (int)Math.Ceiling(22.0 * step), e.Bounds.Top, e.Bounds.Width - (int)Math.Ceiling(22.0 * step), e.Bounds.Height - 2);
		StringFormat val4 = new StringFormat();
		try
		{
			val4.LineAlignment = (StringAlignment)1;
			val4.Alignment = (StringAlignment)0;
			graphics.DrawString(stiTypeWrapper.ToString(), comboBoxType.DrawingFont, val2, (RectangleF)rectangle2, val4);
		}
		finally
		{
			((IDisposable)val4)?.Dispose();
		}
	}

	private void WorkerPreview_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		if (!isFormClosing)
		{
			isAnalysisRunning = false;
			if (isAnalysisRunAgain)
			{
				isAnalysisRunAgain = false;
				RefreshElementView();
			}
			if (exception != null)
			{
				StiExceptionProvider.Show(exception);
			}
			else
			{
				isUpdatingDataGrid = true;
				((Control)dataGridSample).Visible = false;
				((DataGridView)dataGridSample).DataSource = null;
				HideErrorMessage();
				((DataGridView)dataGridSample).DataSource = DataTableSample;
				RestoreDataGridSelection();
			}
			StiTableSizer.MakeAutoSize((DataGridView)(object)dataGridSample);
			progressPreview.IsRunning = false;
			((Control)progressPreview).Visible = false;
			((Control)dataGridSample).Visible = DataTableSample != null;
		}
	}

	private void WorkerPreview_DoWork(object sender, DoWorkEventArgs e)
	{
		HideErrorMessage();
		try
		{
			DataTableSample = Transformation.RetrieveDataTable(StiDataRequestOption.All);
		}
		catch (StiDataException ex)
		{
			ShowErrorMessage(ex.Message);
		}
		catch (EvaluationException ex2)
		{
			ShowErrorMessage(ex2.Message);
		}
		catch (FormatException ex3)
		{
			ShowErrorMessage(ex3.Message);
		}
		catch (Exception ex4)
		{
			exception = ex4;
		}
	}

	private void PanelSample_Resize(object sender, EventArgs e)
	{
		((Control)panelSample).Invalidate();
	}

	private void PanelSample_Paint(object sender, PaintEventArgs e)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		Pen val = new Pen(isDragOverFromDictionary ? Color.FromArgb(200, 43, 87, 154) : Color.DarkGray, (float)((!isDragOverFromDictionary) ? 1 : 2));
		try
		{
			val.DashStyle = (DashStyle)((((ListBox)listBoxColumns).Items.Count == 0 || isDragOverFromDictionary) ? 1 : 0);
			val.Alignment = (PenAlignment)1;
			Rectangle rectangle = new Rectangle(0, 0, ((Control)panelSample).Width, ((Control)panelSample).Height);
			if (!isDragOverFromDictionary)
			{
				rectangle.Width--;
				rectangle.Height--;
			}
			graphics.DrawRectangle(Pens.White, rectangle);
			graphics.DrawRectangle(val, rectangle);
			if (((DataGridView)dataGridSample).DataSource != null)
			{
				return;
			}
			StringFormat val2 = new StringFormat();
			try
			{
				Font val3 = new Font("Arial", 8f);
				try
				{
					val2.LineAlignment = (StringAlignment)1;
					val2.Alignment = (StringAlignment)1;
					graphics.DrawString(Loc.Get("FormDesigner", "Preview"), val3, Brushes.DimGray, (RectangleF)rectangle, val2);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void SortFilterMenu_Changed(object sender, EventArgs e)
	{
		RefreshElementView();
	}

	private void DataGridSample_Resize(object sender, EventArgs e)
	{
		((Control)progressPreview).Left = (((Control)dataGridSample).Width - ((Control)progressPreview).Width) / 2;
		((Control)progressPreview).Top = (((Control)dataGridSample).Height - ((Control)progressPreview).Height) / 2;
	}

	private void DataGridSample_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Invalid comparison between Unknown and I4
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Invalid comparison between Unknown and I4
		if (e.RowIndex != -1 || e.ColumnIndex <= -1)
		{
			return;
		}
		bool flag = (Control.ModifierKeys & 0x20000) > 0;
		bool flag2 = (Control.ModifierKeys & 0x10000) > 0;
		StiDataColumn stiDataColumn = ((ListBox)listBoxColumns).Items[e.ColumnIndex] as StiDataColumn;
		if (flag || flag2)
		{
			StiDataSortDirection sortDirection = StiDataSortRuleHelper.GetSortDirection(Transformation.Sorts, stiDataColumn.Key);
			sortDirection = ((sortDirection != StiDataSortDirection.Ascending) ? StiDataSortDirection.Ascending : StiDataSortDirection.Descending);
			if (flag2)
			{
				Transformation.Sorts = new List<StiDataSortRule>();
			}
			Transformation.Sorts = StiDataSortRuleHelper.SetSortDirection(Transformation.Sorts, (from StiDataColumn c in Transformation.Columns
				select c?.Key into c
				where c != null
				select c).ToList(), stiDataColumn.Key, sortDirection);
			InvalidateDataGridSampleHeaders();
		}
		else
		{
			if (((DataGridView)dataGridSample).CurrentCell != null)
			{
				((DataGridView)dataGridSample).CurrentCell = ((DataGridView)dataGridSample).Rows[((DataGridView)dataGridSample).CurrentCell.RowIndex].Cells[e.ColumnIndex];
			}
			Rectangle cellDisplayRectangle = ((DataGridView)dataGridSample).GetCellDisplayRectangle(e.ColumnIndex, -1, false);
			sortFilterMenu.CreateItems(Transformation.GetMeters(), e.ColumnIndex, null);
			Point point = ((Control)dataGridSample).PointToScreen(new Point(cellDisplayRectangle.X, cellDisplayRectangle.Bottom + 1));
			((ToolStripDropDown)sortFilterContextMenu).Show(point);
		}
	}

	private void DataGridSample_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
	{
		StiTablePartDrawer stiTablePartDrawer = new StiTablePartDrawer(e, (DataGridView)(object)dataGridSample, ((ListBox)listBoxColumns).Items, Transformation.Sorts, Transformation.Filters, Transformation.Actions);
		stiTablePartDrawer.DrawCell();
	}

	private void DataGridSample_CurrentCellChanged(object sender, EventArgs e)
	{
		if (!isUpdatingDataGrid && ((DataGridView)dataGridSample).CurrentCell != null && ((ListBox)listBoxColumns).Items.Count == ((DataGridView)dataGridSample).ColumnCount)
		{
			((ListControl)listBoxColumns).SelectedIndex = ((DataGridView)dataGridSample).CurrentCell.ColumnIndex;
		}
	}

	private void DataGridSample_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 13)
		{
			This_OkClick(sender, (EventArgs)(object)e);
		}
	}

	private void DataGridSample_DragEnter(object sender, DragEventArgs e)
	{
		((Control)this).OnDragOver(e);
		object data = e.Data.GetData(typeof(StiDictionaryControl));
		if (data != null && IsDragDropAllowed(Designer))
		{
			isDragOverFromDictionary = true;
			((Control)panelSample).Invalidate();
			e.Effect = (DragDropEffects)2;
		}
		else
		{
			e.Effect = (DragDropEffects)0;
		}
	}

	private void DataGridSample_DragDrop(object sender, DragEventArgs e)
	{
		((Control)this).OnDragDrop(e);
		listBoxColumns.InvokeDragDrop(e);
		isDragOverFromDictionary = false;
		((Control)panelSample).Invalidate();
	}

	private void DataGridSample_DragLeave(object sender, EventArgs e)
	{
		isDragOverFromDictionary = false;
		((Control)panelSample).Invalidate();
	}

	private void DataGridSample_DataError(object sender, DataGridViewDataErrorEventArgs e)
	{
		ShowErrorMessage($"{e.Exception.Message} Column index {((DataGridViewCellCancelEventArgs)e).ColumnIndex}.");
	}

	private void ListBoxColumns_ElementsChanged(object sender, EventArgs e)
	{
		WriteParametersToTransformation();
		RefreshElementView();
		RefreshUiPanel();
	}

	private void ListBoxColumns_ElementsDragDrop(object sender, EventArgs e)
	{
		if (StiDragDropColumnHelper.IsDataColumnDragged(Designer))
		{
			StiDataColumn draggedDataColumn = StiDragDropColumnHelper.GetDraggedDataColumn(Designer);
			StiDataTransformationColumn stiDataTransformationColumn = CreateTransformationColumnFromDataColumn(draggedDataColumn);
			stiDataTransformationColumn.Name = StiNameCreation.CreateColumnName(Transformation, stiDataTransformationColumn.Name);
			StiDataTransformationColumn itemAtMouse = listBoxColumns.GetItemAtMouse();
			if (itemAtMouse != null)
			{
				int num = ((ListBox)listBoxColumns).Items.IndexOf((object)itemAtMouse);
				((ListBox)listBoxColumns).Items.Insert(num, (object)stiDataTransformationColumn);
				Transformation.Columns.Insert(num, stiDataTransformationColumn);
			}
			else
			{
				((ListBox)listBoxColumns).Items.Add((object)stiDataTransformationColumn);
				Transformation.Columns.Add(stiDataTransformationColumn);
			}
			Designer.PageView.ResetDragging();
			RefreshElementView();
			listBoxColumns.SelectedDataColumnItem = stiDataTransformationColumn;
			((Control)listBoxColumns).Focus();
		}
		else if (StiDragDropColumnHelper.IsDataBandDragged(Designer) || StiDragDropColumnHelper.IsTableElementDragged(Designer))
		{
			StiDataSource draggedDataSource = StiDragDropColumnHelper.GetDraggedDataSource(Designer);
			StiDataTransformationColumn[] source = draggedDataSource.Columns.ToList().Select(CreateTransformationColumnFromDataColumn).ToArray();
			source.ToList().ForEach(delegate(StiDataTransformationColumn column)
			{
				column.Name = StiNameCreation.CreateColumnName(Transformation, column.Name);
				((ListBox)listBoxColumns).Items.Add((object)column);
				Transformation.Columns.Add(column);
			});
			Designer.PageView.ResetDragging();
			RefreshElementView();
			listBoxColumns.SelectedDataColumnItem = source.FirstOrDefault();
			((Control)listBoxColumns).Focus();
		}
	}

	private void ListBoxColumns_ElementsSelectedChanged(object sender, EventArgs e)
	{
		listBoxColumnsSelectedIndex = ((ListControl)listBoxColumns).SelectedIndex;
		RestoreDataGridSelection();
		RefreshUiPanel();
	}

	private void TextBoxExpression_Enter(object sender, EventArgs e)
	{
		textBoxExpressionChanged = false;
		((Form)this).AcceptButton = null;
	}

	private void TextBoxExpression_Leave(object sender, EventArgs e)
	{
		if (textBoxExpressionChanged)
		{
			RefreshElementView();
		}
		textBoxExpressionChanged = false;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
	}

	private void TextBoxExpression_TextChanged(object sender, EventArgs e)
	{
		textBoxExpressionChanged = true;
		if (SelectedColumn != null)
		{
			SelectedColumn.Expression = ((Control)textBoxExpression).Text;
		}
		RefreshExpressionHint();
	}

	private void TextBoxExpression_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 13)
		{
			e.Handled = true;
			e.SuppressKeyPress = true;
			if (DataGridSample != null)
			{
				((Control)DataGridSample).Focus();
			}
			else
			{
				((Control)listBoxColumns).Focus();
			}
		}
	}

	private void ButtonExpressionMenu_Click(object sender, EventArgs e)
	{
		Point point = new Point(((Control)panelBoxExpression).Width, ((Control)panelBoxExpression).Height + StiScale.I1);
		if (StiLeftRightHandedHelper.IsLeftHanded)
		{
			point.X -= menuExpression.PopupSize.Width;
		}
		point = ((Control)panelBoxExpression).PointToScreen(point);
		menuExpression.Popup(point);
	}

	private void MenuExpression_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		expressionMenu.CreateItems();
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		DataGridSample.InitGridProperties();
		DataGridSample.ColumnHeadersDefaultCellStyle.Font = ((Control)DataGridSample).Font;
		DataGridSample.DefaultCellStyle.Font = ((Control)DataGridSample).Font;
		RefreshElementView();
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		((Control)this).OnSizeChanged(e);
		StiDataGridView stiDataGridView = dataGridSample;
		if (stiDataGridView != null)
		{
			((Control)stiDataGridView).Invalidate();
		}
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		SaveDataTransformation();
		((Form)this).Close();
	}

	private void This_FormClosing(object sender, FormClosingEventArgs e)
	{
		if (IsElementTransformation)
		{
			SaveDataTransformation();
		}
		isFormClosing = true;
		StiFormSettings.Save((Form)(object)this);
		try
		{
			progressPreview.IsRunning = false;
			workerPreview.CancelAsync();
		}
		catch
		{
		}
	}

	private void This_Load(object sender, EventArgs e)
	{
		StiSettings.Load();
		StiFormSettings.Load((Form)(object)this);
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/index.html?data_data_dictionary_data_transformation.htm");
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
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Expected O, but got Unknown
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Expected O, but got Unknown
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0559: Unknown result type (might be due to invalid IL or missing references)
		//IL_0563: Expected O, but got Unknown
		//IL_0584: Unknown result type (might be due to invalid IL or missing references)
		//IL_0627: Unknown result type (might be due to invalid IL or missing references)
		//IL_0631: Expected O, but got Unknown
		//IL_0652: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_081f: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_095e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a00: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd1: Expected O, but got Unknown
		//IL_0c64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6e: Expected O, but got Unknown
		//IL_0d1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d29: Expected O, but got Unknown
		//IL_0d36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d40: Expected O, but got Unknown
		//IL_0d64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6e: Expected O, but got Unknown
		//IL_0d7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d85: Expected O, but got Unknown
		//IL_0da9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db3: Expected O, but got Unknown
		//IL_0dd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de1: Expected O, but got Unknown
		//IL_0e1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1098: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f9: Expected O, but got Unknown
		//IL_114d: Unknown result type (might be due to invalid IL or missing references)
		//IL_120b: Unknown result type (might be due to invalid IL or missing references)
		//IL_122c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1269: Unknown result type (might be due to invalid IL or missing references)
		//IL_1273: Expected O, but got Unknown
		//IL_1280: Unknown result type (might be due to invalid IL or missing references)
		//IL_128a: Expected O, but got Unknown
		//IL_12ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_12b8: Expected O, but got Unknown
		//IL_13e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f0: Expected O, but got Unknown
		//IL_142d: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1643: Unknown result type (might be due to invalid IL or missing references)
		//IL_164d: Expected O, but got Unknown
		//IL_167d: Unknown result type (might be due to invalid IL or missing references)
		//IL_16a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_170b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1715: Expected O, but got Unknown
		//IL_18cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_18d9: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiDataTransformationEditorForm));
		toolTip = new ToolTip(components);
		panelBoxExpression = new StiPanelBox();
		textBoxExpression = new HintTextBox();
		buttonExpressionMenu = new StiOperationButton();
		labelExpression = new Label();
		labelMode = new Label();
		buttonMeasure = new RadioButton();
		buttonDimension = new RadioButton();
		listBoxColumns = new StiDataTransformColumnListBox();
		stiGroupLine1 = new StiGroupLine();
		buttonOk = new Button();
		buttonCancel = new Button();
		labelName = new Label();
		textBoxName = new StiTextBox();
		labelAlias = new Label();
		textBoxAlias = new StiTextBox();
		dataGridSample = new StiDataGridView();
		textBoxNameInSource = new StiTextBox();
		labelNameInSource = new Label();
		groupBoxSettings = new StiOffice2013GroupBox();
		comboBoxType = new StiComboBox();
		labelType = new Label();
		panelSample = new Panel();
		progressPreview = new CircularProgress();
		workerPreview = new BackgroundWorker();
		contextMenuBar = new ContextMenuBar();
		menuExpression = new ButtonItem();
		panelTop = new Panel();
		labelErrorMessage = new Label();
		imageListTypes = new ImageList(components);
		((Control)panelBoxExpression).SuspendLayout();
		((Control)stiGroupLine1).SuspendLayout();
		((ISupportInitialize)dataGridSample).BeginInit();
		((Control)groupBoxSettings).SuspendLayout();
		((Control)panelSample).SuspendLayout();
		((ISupportInitialize)contextMenuBar).BeginInit();
		((Control)panelTop).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panelBoxExpression).Anchor = (AnchorStyles)14;
		((Control)panelBoxExpression).BackColor = Color.White;
		((Control)panelBoxExpression).Controls.Add((Control)(object)textBoxExpression);
		((Control)panelBoxExpression).Controls.Add((Control)(object)buttonExpressionMenu);
		((Control)panelBoxExpression).Location = new Point(190, 260);
		((Control)panelBoxExpression).Margin = new Padding(2);
		((Control)panelBoxExpression).Name = "panelBoxExpression";
		((Control)panelBoxExpression).Size = new Size(399, 25);
		((Control)panelBoxExpression).TabIndex = 4;
		((Control)textBoxExpression).Anchor = (AnchorStyles)13;
		((Control)textBoxExpression).BackColor = SystemColors.Window;
		((TextBoxBase)textBoxExpression).BorderStyle = (BorderStyle)0;
		((Control)textBoxExpression).Location = new Point(4, 6);
		((Control)textBoxExpression).Margin = new Padding(2);
		((Control)textBoxExpression).Name = "textBoxExpression";
		((Control)textBoxExpression).Size = new Size(366, 13);
		((Control)textBoxExpression).TabIndex = 0;
		((Control)textBoxExpression).TextChanged += TextBoxExpression_TextChanged;
		((Control)textBoxExpression).Enter += TextBoxExpression_Enter;
		((Control)textBoxExpression).KeyDown += new KeyEventHandler(TextBoxExpression_KeyDown);
		((Control)textBoxExpression).Leave += TextBoxExpression_Leave;
		((Control)buttonExpressionMenu).Anchor = (AnchorStyles)9;
		((Control)buttonExpressionMenu).Location = new Point(375, 1);
		((Control)buttonExpressionMenu).Margin = new Padding(2);
		((Control)buttonExpressionMenu).Name = "buttonExpressionMenu";
		((Control)buttonExpressionMenu).Size = new Size(23, 23);
		((Control)buttonExpressionMenu).TabIndex = 1;
		((Control)buttonExpressionMenu).TabStop = false;
		((ButtonBase)buttonExpressionMenu).UseVisualStyleBackColor = true;
		((Control)buttonExpressionMenu).Click += ButtonExpressionMenu_Click;
		((Control)labelExpression).Anchor = (AnchorStyles)6;
		((Control)labelExpression).ForeColor = Color.DimGray;
		((Control)labelExpression).Location = new Point(12, 262);
		((Control)labelExpression).Margin = new Padding(2, 0, 2, 0);
		((Control)labelExpression).Name = "labelExpression";
		((Control)labelExpression).Size = new Size(167, 22);
		((Control)labelExpression).TabIndex = 1;
		((Control)labelExpression).Text = "Expression";
		labelExpression.TextAlign = (ContentAlignment)16;
		labelExpression.UseCompatibleTextRendering = true;
		((Control)labelMode).Anchor = (AnchorStyles)6;
		((Control)labelMode).ForeColor = Color.DimGray;
		((Control)labelMode).Location = new Point(12, 224);
		((Control)labelMode).Margin = new Padding(2, 0, 2, 0);
		((Control)labelMode).Name = "labelMode";
		((Control)labelMode).Size = new Size(166, 26);
		((Control)labelMode).TabIndex = 1;
		((Control)labelMode).Text = "Mode";
		labelMode.TextAlign = (ContentAlignment)16;
		labelMode.UseCompatibleTextRendering = true;
		((Control)buttonMeasure).Anchor = (AnchorStyles)6;
		buttonMeasure.Appearance = (Appearance)1;
		((ButtonBase)buttonMeasure).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonMeasure).Image = (Image)componentResourceManager.GetObject("buttonMeasure.Image");
		((Control)buttonMeasure).Location = new Point(218, 224);
		((Control)buttonMeasure).Margin = new Padding(2);
		((Control)buttonMeasure).Name = "buttonMeasure";
		((Control)buttonMeasure).Size = new Size(25, 26);
		((Control)buttonMeasure).TabIndex = 3;
		((ButtonBase)buttonMeasure).UseVisualStyleBackColor = true;
		buttonMeasure.CheckedChanged += ButtonMeter_CheckedChanged;
		((Control)buttonDimension).Anchor = (AnchorStyles)6;
		buttonDimension.Appearance = (Appearance)1;
		buttonDimension.Checked = true;
		((ButtonBase)buttonDimension).FlatAppearance.BorderSize = 0;
		((ButtonBase)buttonDimension).Image = (Image)componentResourceManager.GetObject("buttonDimension.Image");
		((Control)buttonDimension).Location = new Point(189, 224);
		((Control)buttonDimension).Margin = new Padding(2);
		((Control)buttonDimension).Name = "buttonDimension";
		((Control)buttonDimension).Size = new Size(25, 26);
		((Control)buttonDimension).TabIndex = 2;
		buttonDimension.TabStop = true;
		((ButtonBase)buttonDimension).UseVisualStyleBackColor = true;
		buttonDimension.CheckedChanged += ButtonMeter_CheckedChanged;
		((Control)listBoxColumns).AllowDrop = true;
		((Control)listBoxColumns).Anchor = (AnchorStyles)7;
		((Control)listBoxColumns).Location = new Point(12, 36);
		((Control)listBoxColumns).Margin = new Padding(2);
		((Control)listBoxColumns).Name = "listBoxColumns";
		((Control)listBoxColumns).Size = new Size(166, 176);
		((Control)listBoxColumns).TabIndex = 0;
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonOk);
		((Control)stiGroupLine1).Controls.Add((Control)(object)buttonCancel);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 476);
		((Control)stiGroupLine1).Margin = new Padding(2);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(601, 46);
		((Control)stiGroupLine1).TabIndex = 4;
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		buttonOk.DialogResult = (DialogResult)1;
		((Control)buttonOk).Location = new Point(431, 12);
		((Control)buttonOk).Margin = new Padding(2);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 0;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(514, 12);
		((Control)buttonCancel).Margin = new Padding(2);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 1;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonCancel).Click += This_CancelClick;
		((Control)labelName).ForeColor = Color.DimGray;
		((Control)labelName).Location = new Point(11, 45);
		((Control)labelName).Margin = new Padding(2, 0, 2, 0);
		((Control)labelName).Name = "labelName";
		((Control)labelName).Size = new Size(166, 16);
		((Control)labelName).TabIndex = 18;
		((Control)labelName).Text = "Name";
		labelName.TextAlign = (ContentAlignment)16;
		labelName.UseCompatibleTextRendering = true;
		((Control)textBoxName).Anchor = (AnchorStyles)13;
		((Control)textBoxName).BackColor = SystemColors.Window;
		((Control)textBoxName).Location = new Point(190, 44);
		((Control)textBoxName).Margin = new Padding(2);
		((Control)textBoxName).Name = "textBoxName";
		((Control)textBoxName).Size = new Size(314, 21);
		((Control)textBoxName).TabIndex = 1;
		((Control)textBoxName).Enter += TextBoxName_Enter;
		((Control)textBoxName).Leave += TextBoxName_Leave;
		((Control)labelAlias).ForeColor = Color.DimGray;
		((Control)labelAlias).Location = new Point(11, 77);
		((Control)labelAlias).Margin = new Padding(2, 0, 2, 0);
		((Control)labelAlias).Name = "labelAlias";
		((Control)labelAlias).Size = new Size(166, 16);
		((Control)labelAlias).TabIndex = 19;
		((Control)labelAlias).Text = "Alias";
		labelAlias.TextAlign = (ContentAlignment)16;
		labelAlias.UseCompatibleTextRendering = true;
		((Control)textBoxAlias).Anchor = (AnchorStyles)13;
		((Control)textBoxAlias).BackColor = SystemColors.Window;
		((Control)textBoxAlias).Location = new Point(190, 76);
		((Control)textBoxAlias).Margin = new Padding(2);
		((Control)textBoxAlias).Name = "textBoxAlias";
		((Control)textBoxAlias).Size = new Size(314, 21);
		((Control)textBoxAlias).TabIndex = 2;
		((Control)dataGridSample).AllowDrop = true;
		dataGridSample.ColumnHeadersDefaultCellStyle.Alignment = (DataGridViewContentAlignment)16;
		dataGridSample.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Control;
		dataGridSample.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		dataGridSample.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.WindowText;
		dataGridSample.ColumnHeadersDefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
		dataGridSample.ColumnHeadersDefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
		dataGridSample.ColumnHeadersDefaultCellStyle.WrapMode = (DataGridViewTriState)1;
		dataGridSample.DefaultCellStyle.Alignment = (DataGridViewContentAlignment)16;
		dataGridSample.DefaultCellStyle.BackColor = SystemColors.Window;
		dataGridSample.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		dataGridSample.DefaultCellStyle.ForeColor = SystemColors.ControlText;
		dataGridSample.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
		dataGridSample.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
		dataGridSample.DefaultCellStyle.WrapMode = (DataGridViewTriState)2;
		((Control)dataGridSample).Dock = (DockStyle)5;
		((Control)dataGridSample).Location = new Point(2, 2);
		((Control)dataGridSample).Name = "dataGridSample";
		((Control)dataGridSample).Size = new Size(395, 172);
		((Control)dataGridSample).TabIndex = 0;
		((DataGridView)dataGridSample).CellMouseClick += new DataGridViewCellMouseEventHandler(DataGridSample_CellMouseClick);
		((DataGridView)dataGridSample).CellPainting += new DataGridViewCellPaintingEventHandler(DataGridSample_CellPainting);
		((DataGridView)dataGridSample).CurrentCellChanged += DataGridSample_CurrentCellChanged;
		((Control)dataGridSample).DragDrop += new DragEventHandler(DataGridSample_DragDrop);
		((Control)dataGridSample).DragEnter += new DragEventHandler(DataGridSample_DragEnter);
		((Control)dataGridSample).DragLeave += DataGridSample_DragLeave;
		((Control)dataGridSample).KeyDown += new KeyEventHandler(DataGridSample_KeyDown);
		((Control)dataGridSample).Resize += DataGridSample_Resize;
		((DataGridView)dataGridSample).DataError += new DataGridViewDataErrorEventHandler(DataGridSample_DataError);
		((Control)textBoxNameInSource).Anchor = (AnchorStyles)13;
		((Control)textBoxNameInSource).BackColor = SystemColors.Window;
		((Control)textBoxNameInSource).Location = new Point(190, 12);
		((Control)textBoxNameInSource).Margin = new Padding(2);
		((Control)textBoxNameInSource).Name = "textBoxNameInSource";
		((Control)textBoxNameInSource).Size = new Size(314, 21);
		((Control)textBoxNameInSource).TabIndex = 0;
		((Control)labelNameInSource).ForeColor = Color.DimGray;
		((Control)labelNameInSource).Location = new Point(11, 13);
		((Control)labelNameInSource).Margin = new Padding(2, 0, 2, 0);
		((Control)labelNameInSource).Name = "labelNameInSource";
		((Control)labelNameInSource).Size = new Size(166, 16);
		((Control)labelNameInSource).TabIndex = 18;
		((Control)labelNameInSource).Text = "Name in Source";
		labelNameInSource.TextAlign = (ContentAlignment)16;
		labelNameInSource.UseCompatibleTextRendering = true;
		((Control)groupBoxSettings).Controls.Add((Control)(object)comboBoxType);
		((Control)groupBoxSettings).Controls.Add((Control)(object)labelType);
		((Control)groupBoxSettings).Controls.Add((Control)(object)panelSample);
		((Control)groupBoxSettings).Controls.Add((Control)(object)panelBoxExpression);
		((Control)groupBoxSettings).Controls.Add((Control)(object)buttonDimension);
		((Control)groupBoxSettings).Controls.Add((Control)(object)buttonMeasure);
		((Control)groupBoxSettings).Controls.Add((Control)(object)labelMode);
		((Control)groupBoxSettings).Controls.Add((Control)(object)labelExpression);
		((Control)groupBoxSettings).Controls.Add((Control)(object)listBoxColumns);
		((Control)groupBoxSettings).Dock = (DockStyle)5;
		groupBoxSettings.HeaderHeight = 24;
		((Control)groupBoxSettings).Location = new Point(0, 107);
		((Control)groupBoxSettings).Margin = new Padding(2);
		((Control)groupBoxSettings).Name = "groupBoxSettings";
		((Control)groupBoxSettings).Size = new Size(601, 330);
		((Control)groupBoxSettings).TabIndex = 3;
		((Control)groupBoxSettings).TabStop = false;
		((Control)groupBoxSettings).Text = "Settings";
		((Control)comboBoxType).Anchor = (AnchorStyles)6;
		((Control)comboBoxType).BackColor = SystemColors.Window;
		comboBoxType.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)comboBoxType).IntegralHeight = false;
		((Control)comboBoxType).Location = new Point(190, 296);
		((Control)comboBoxType).Margin = new Padding(2);
		((ComboBox)comboBoxType).MaxDropDownItems = 30;
		((Control)comboBoxType).Name = "comboBoxType";
		((Control)comboBoxType).Size = new Size(130, 21);
		((Control)comboBoxType).TabIndex = 5;
		((ComboBox)comboBoxType).DrawItem += new DrawItemEventHandler(ComboBoxType_DrawItem);
		((ComboBox)comboBoxType).SelectedIndexChanged += ComboBoxType_SelectedIndexChanged;
		((Control)labelType).Anchor = (AnchorStyles)6;
		((Control)labelType).ForeColor = Color.DimGray;
		((Control)labelType).Location = new Point(12, 298);
		((Control)labelType).Margin = new Padding(2, 0, 2, 0);
		((Control)labelType).Name = "labelType";
		((Control)labelType).Size = new Size(167, 19);
		((Control)labelType).TabIndex = 10;
		((Control)labelType).Text = "Type";
		labelType.TextAlign = (ContentAlignment)16;
		((Control)panelSample).AllowDrop = true;
		((Control)panelSample).Anchor = (AnchorStyles)15;
		((Control)panelSample).Controls.Add((Control)(object)progressPreview);
		((Control)panelSample).Controls.Add((Control)(object)dataGridSample);
		((Control)panelSample).Location = new Point(190, 36);
		((Control)panelSample).Margin = new Padding(2);
		((Control)panelSample).Name = "panelSample";
		((Control)panelSample).Padding = new Padding(2);
		((Control)panelSample).Size = new Size(399, 176);
		((Control)panelSample).TabIndex = 1;
		((Control)panelSample).DragDrop += new DragEventHandler(DataGridSample_DragDrop);
		((Control)panelSample).DragEnter += new DragEventHandler(DataGridSample_DragEnter);
		((Control)panelSample).DragLeave += DataGridSample_DragLeave;
		((Control)panelSample).Paint += new PaintEventHandler(PanelSample_Paint);
		((Control)panelSample).Resize += PanelSample_Resize;
		((Control)progressPreview).BackColor = Color.White;
		progressPreview.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)progressPreview).Location = new Point(172, 98);
		((Control)progressPreview).Name = "progressPreview";
		progressPreview.ProgressBarType = eCircularProgressType.Donut;
		progressPreview.ProgressColor = Color.SteelBlue;
		((Control)progressPreview).Size = new Size(52, 54);
		progressPreview.Style = eDotNetBarStyle.OfficeXP;
		((Control)progressPreview).TabIndex = 6;
		((Control)progressPreview).TabStop = false;
		((Control)progressPreview).Visible = false;
		workerPreview.WorkerSupportsCancellation = true;
		workerPreview.DoWork += WorkerPreview_DoWork;
		workerPreview.RunWorkerCompleted += WorkerPreview_RunWorkerCompleted;
		contextMenuBar.AntiAlias = true;
		contextMenuBar.DockSide = eDockSide.Top;
		contextMenuBar.DockTabStripHeight = 50;
		((Control)contextMenuBar).Font = new Font("Segoe UI", 9f);
		contextMenuBar.Items.AddRange(new BaseItem[1] { menuExpression });
		contextMenuBar.Location = new Point(507, 12);
		((Control)contextMenuBar).Margin = new Padding(2);
		contextMenuBar.Name = "contextMenuBar";
		contextMenuBar.Size = new Size(75, 31);
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
		menuExpression.PopupOpen += MenuExpression_PopupOpen;
		((Control)panelTop).Controls.Add((Control)(object)labelNameInSource);
		((Control)panelTop).Controls.Add((Control)(object)contextMenuBar);
		((Control)panelTop).Controls.Add((Control)(object)labelName);
		((Control)panelTop).Controls.Add((Control)(object)textBoxNameInSource);
		((Control)panelTop).Controls.Add((Control)(object)textBoxName);
		((Control)panelTop).Controls.Add((Control)(object)labelAlias);
		((Control)panelTop).Controls.Add((Control)(object)textBoxAlias);
		((Control)panelTop).Dock = (DockStyle)1;
		((Control)panelTop).Location = new Point(0, 0);
		((Control)panelTop).Margin = new Padding(2);
		((Control)panelTop).Name = "panelTop";
		((Control)panelTop).Size = new Size(601, 107);
		((Control)panelTop).TabIndex = 20;
		((Control)labelErrorMessage).BackColor = Color.FromArgb(252, 220, 220);
		((Control)labelErrorMessage).Dock = (DockStyle)2;
		((Control)labelErrorMessage).Font = new Font("Microsoft Sans Serif", 7.875f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)labelErrorMessage).ForeColor = Color.Red;
		((Control)labelErrorMessage).Location = new Point(0, 437);
		((Control)labelErrorMessage).Margin = new Padding(2, 0, 2, 0);
		((Control)labelErrorMessage).Name = "labelErrorMessage";
		((Control)labelErrorMessage).Padding = new Padding(12, 2, 12, 2);
		((Control)labelErrorMessage).Size = new Size(601, 39);
		((Control)labelErrorMessage).TabIndex = 22;
		((Control)labelErrorMessage).Text = "Error message";
		labelErrorMessage.TextAlign = (ContentAlignment)16;
		((Control)labelErrorMessage).Visible = false;
		imageListTypes.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageListTypes.ImageStream");
		imageListTypes.TransparentColor = Color.Magenta;
		imageListTypes.Images.SetKeyName(0, "String");
		imageListTypes.Images.SetKeyName(1, "Binary");
		imageListTypes.Images.SetKeyName(2, "Bool");
		imageListTypes.Images.SetKeyName(3, "Char");
		imageListTypes.Images.SetKeyName(4, "DateTime");
		imageListTypes.Images.SetKeyName(5, "Decimal");
		imageListTypes.Images.SetKeyName(6, "Float");
		imageListTypes.Images.SetKeyName(7, "Image");
		imageListTypes.Images.SetKeyName(8, "Int");
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(601, 522);
		((Control)this).Controls.Add((Control)(object)groupBoxSettings);
		((Control)this).Controls.Add((Control)(object)labelErrorMessage);
		((Control)this).Controls.Add((Control)(object)panelTop);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Control)this).MinimumSize = new Size(617, 558);
		((Control)this).Name = "StiDataTransformationEditorForm";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Data Transformation";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).FormClosing += new FormClosingEventHandler(This_FormClosing);
		((Form)this).Load += This_Load;
		((Control)panelBoxExpression).ResumeLayout(false);
		((Control)panelBoxExpression).PerformLayout();
		((Control)stiGroupLine1).ResumeLayout(false);
		((ISupportInitialize)dataGridSample).EndInit();
		((Control)groupBoxSettings).ResumeLayout(false);
		((Control)panelSample).ResumeLayout(false);
		((ISupportInitialize)contextMenuBar).EndInit();
		((Control)panelTop).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
