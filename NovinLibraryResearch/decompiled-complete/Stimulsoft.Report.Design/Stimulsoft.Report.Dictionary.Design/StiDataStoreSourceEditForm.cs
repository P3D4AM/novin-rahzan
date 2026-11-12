using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Editor;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Dictionary;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary.DataSources.Google;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helper;
using Stimulsoft.Report.Images;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiDataStoreSourceEditForm : StiFormUX
{
	private StiReport originalReportClone;

	private string originalName;

	private StiDataStoreSource dataSource;

	private string oldValue;

	private StiDictionary dictionary;

	private StiDataAdapterService adapter;

	private IContainer components;

	private StiButtonUX btOk;

	private StiButtonUX btCancel;

	private StiLabelUX lbAlias;

	public StiTextBox tbAlias;

	private StiLabelUX lbName;

	private ImageList ilButtons;

	private ErrorProvider errorProvider;

	private StiOffice2013GroupBox gbQuery;

	private StiPanelUX pnQuery;

	private StiToolButton tbRunScript;

	private StiToolButton tbEditSql;

	private StiPanelUX pnMain;

	private StiButtonEdit buttonEditNameInSource;

	private ToolTip toolTip1;

	private StiToolBar tbSQL;

	private StiOffice2013GroupBox gbColumns;

	private Splitter splMain;

	private SyntaxEdit syntaxEdit1;

	private StiCodeEditor tbSqlCommand;

	private StiToolButton tbViewData;

	private StiComboBox comboBoxCodePage;

	private StiLabelUX labelNameInSource;

	public StiTextBox tbName;

	private StiToolButton tbRunQueryBuilder;

	private StiLabelUX labelType;

	private StiButtonUX btSaveCopy;

	public StiTextBox textBoxCustomDelimeter;

	private StiComboBox comboBoxDelimiter;

	private StiLabelUX lbDelimiter;

	private StiLabelUX lbEncoding;

	private StiButtonPanelUX stiGroupLine1;

	private StiDataColumnsCollection Columns;

	private StiPanelUX panelDelimiter;

	private StiPanelUX panelEncoding;

	private StiPanelUX panelName;

	private StiPanelUX pnColumns;

	private StiReportPropertyGrid pgDataSource;

	private Splitter splitterColumns;

	private StiTreeViewUX tvDataSource;

	private StiToolBar tbMain;

	private StiToolButton tbGetColumnsFromAssembly;

	private StiToolButton tbRetrieveColumns;

	private StiToolButton toolButtonSeparator;

	private StiToolButton toolButtonViewData;

	private StiToolButton stiToolButton1;

	private StiToolButton tbDelete;

	private StiToolButton tbParameterNew;

	private StiToolButton tbCalcColumnNew;

	private StiToolButton tbColumnNew;

	private StiNumericUpDown numericUpDownCommandTimeout;

	private StiLabelUX labelCommandTimeout;

	private StiPanelUX panel2;

	private StiPanelUX panel1;

	private StiPanelUX panel3;

	private StiComboBox comboBoxSqlSourceType;

	public DotNetBarManager DotNetBarManager;

	public ContextMenuBar ContextMenuBar;

	private ButtonItem menuColumns;

	private ButtonItem ciColumnNew;

	private ButtonItem ciCalcColumnNew;

	private ButtonItem ciParameterNew;

	private ButtonItem ciDelete;

	private ButtonItem menuRetrieving;

	private ButtonItem ciRetrieveColumnsAndParameters;

	private ButtonItem ciRetrieveParameters;

	private ButtonItem ciRetrieveColumnsAllowRun;

	public bool CopyModeActivated { get; private set; }

	public StiDataStoreSourceEditForm(StiDictionary dictionary, StiDataAdapterService adapter, StiDataStoreSource dataSource)
		: this(dictionary, adapter, dataSource, newForm: false)
	{
	}

	public StiDataStoreSourceEditForm(StiDictionary dictionary, StiDataAdapterService adapter, StiDataStoreSource dataSource, bool newForm)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (((Component)(object)this).DesignMode)
		{
			return;
		}
		if (StiOptions.Appearance.ShowFormsHelp)
		{
			((Form)this).HelpButton = true;
		}
		DoScaling();
		originalReportClone = dictionary.Report.Clone() as StiReport;
		this.dictionary = dictionary;
		this.adapter = adapter;
		this.dataSource = dataSource;
		originalName = dataSource.Name;
		tbSqlCommand.Init(dictionary.Report, StiCodeEditorMode.EditorSQL);
		((Control)tbName).Text = dataSource.Name;
		((Control)tbAlias).Text = dataSource.Alias;
		((Control)buttonEditNameInSource).Text = dataSource.NameInSource;
		if (((dataSource.GetConnection() is StiDatabase stiDatabase) ? stiDatabase.ServiceName : null) != "MS SQL")
		{
			((Control)tbRunQueryBuilder).Visible = false;
		}
		if (adapter is StiODataAdapterService || adapter is StiQuickBooksAdapterService)
		{
			((Control)labelType).Visible = false;
			((Control)comboBoxSqlSourceType).Visible = false;
			((Control)tbParameterNew).Visible = false;
			((Control)tbSQL).Visible = false;
			((Control)tbSqlCommand).Text = ((StiSqlSource)dataSource).SqlCommand;
			StiCodeEditor stiCodeEditor = tbSqlCommand;
			((Control)stiCodeEditor).Height = ((Control)stiCodeEditor).Height + StiScale.YYI(28.0);
		}
		else if (dataSource is StiSqlSource)
		{
			((Control)tbSqlCommand).Text = ((StiSqlSource)dataSource).SqlCommand;
			((Control)gbQuery).Visible = true;
			((Control)tbSQL).Visible = true;
			((Control)splMain).Visible = true;
			((Control)tbParameterNew).Visible = true;
			((Control)gbColumns).Dock = (DockStyle)2;
			ciParameterNew.Visible = true;
		}
		else
		{
			((Control)splMain).Visible = false;
			((Control)tbSQL).Visible = false;
			((Control)gbQuery).Visible = false;
			((Control)tbParameterNew).Visible = false;
			((Control)gbColumns).Dock = (DockStyle)5;
			ciParameterNew.Visible = false;
			((Control)toolButtonViewData).Visible = true;
			((Control)toolButtonSeparator).Visible = true;
		}
		if (dataSource is StiDBaseSource)
		{
			StiButtonEdit stiButtonEdit = buttonEditNameInSource;
			((Control)stiButtonEdit).Width = ((Control)stiButtonEdit).Width - (((Control)comboBoxCodePage).Width + StiScale.I8);
			((Control)panelEncoding).Visible = true;
			ObjectCollection items = ((ComboBox)comboBoxCodePage).Items;
			object[] codePageNames = StiDBaseHelper.CodePageNames;
			items.AddRange(codePageNames);
			int codePage = ((StiDBaseSource)dataSource).CodePage;
			for (int i = 0; i < StiDBaseHelper.CodePageCodes.Length; i++)
			{
				if (StiDBaseHelper.CodePageCodes[i, 1] == codePage)
				{
					((ListControl)comboBoxCodePage).SelectedIndex = i;
					break;
				}
			}
			if (((ListControl)comboBoxCodePage).SelectedIndex == -1)
			{
				((ListControl)comboBoxCodePage).SelectedIndex = 0;
			}
		}
		if (dataSource is StiCsvSource)
		{
			((Control)panelEncoding).Visible = true;
			StiButtonEdit stiButtonEdit2 = buttonEditNameInSource;
			((Control)stiButtonEdit2).Width = ((Control)stiButtonEdit2).Width - (((Control)comboBoxCodePage).Width + StiScale.I8);
			ObjectCollection items2 = ((ComboBox)comboBoxCodePage).Items;
			object[] codePageNames = StiCsvHelper.CodePageNames;
			items2.AddRange(codePageNames);
			int codePage2 = ((StiCsvSource)dataSource).CodePage;
			for (int j = 0; j < StiCsvHelper.CodePageCodes.Length; j++)
			{
				if (StiCsvHelper.CodePageCodes[j] == codePage2)
				{
					((ListControl)comboBoxCodePage).SelectedIndex = j;
					break;
				}
			}
			if (((ListControl)comboBoxCodePage).SelectedIndex == -1)
			{
				((ListControl)comboBoxCodePage).SelectedIndex = 0;
			}
			((Control)panelDelimiter).Visible = true;
			((ComboBox)comboBoxDelimiter).Items.Add((object)StiLocalization.Get("FormDictionaryDesigner", "CsvSeparatorSystem"));
			((ComboBox)comboBoxDelimiter).Items.Add((object)StiLocalization.Get("FormDictionaryDesigner", "CsvSeparatorTab"));
			((ComboBox)comboBoxDelimiter).Items.Add((object)StiLocalization.Get("FormDictionaryDesigner", "CsvSeparatorSemicolon"));
			((ComboBox)comboBoxDelimiter).Items.Add((object)StiLocalization.Get("FormDictionaryDesigner", "CsvSeparatorComma"));
			((ComboBox)comboBoxDelimiter).Items.Add((object)StiLocalization.Get("FormDictionaryDesigner", "CsvSeparatorSpace"));
			((ComboBox)comboBoxDelimiter).Items.Add((object)StiLocalization.Get("FormDictionaryDesigner", "CsvSeparatorOther"));
			((ListControl)comboBoxDelimiter).SelectedIndex = 0;
			string separator = ((StiCsvSource)dataSource).Separator;
			if (!string.IsNullOrEmpty(separator))
			{
				switch (separator)
				{
				case "\t":
					((ListControl)comboBoxDelimiter).SelectedIndex = 1;
					break;
				case ";":
					((ListControl)comboBoxDelimiter).SelectedIndex = 2;
					break;
				case ",":
					((ListControl)comboBoxDelimiter).SelectedIndex = 3;
					break;
				case " ":
					((ListControl)comboBoxDelimiter).SelectedIndex = 4;
					break;
				default:
					((ListControl)comboBoxDelimiter).SelectedIndex = 5;
					((Control)textBoxCustomDelimeter).Text = separator;
					break;
				}
			}
			if (((ListControl)comboBoxDelimiter).SelectedIndex == -1)
			{
				((ListControl)comboBoxDelimiter).SelectedIndex = 0;
			}
		}
		if (dataSource is StiMongoDbSource)
		{
			((Control)tbSQL).Visible = false;
			((Control)tbCalcColumnNew).Visible = false;
			((Control)tbParameterNew).Visible = false;
			((Control)tbGetColumnsFromAssembly).Visible = false;
			((Control)labelType).Visible = false;
			((Control)comboBoxSqlSourceType).Visible = false;
		}
		((Control)tbGetColumnsFromAssembly).Visible = dataSource is StiBusinessObjectSource;
		Columns = new StiDataColumnsCollection();
		foreach (StiDataColumn column in dataSource.Columns)
		{
			Columns.Add(column.Clone() as StiDataColumn);
		}
		if (!((Component)(object)this).DesignMode)
		{
			Localize();
		}
		dictionary.Connect(loadData: false);
		StiDataAdapterService dataAdapter = dataSource.GetDataAdapter();
		if (dataAdapter != null && string.IsNullOrEmpty(dataSource.NameInSource) && newForm)
		{
			TreeNode dataAdapterCategories = StiDataAdapterService.GetDataAdapterCategories(dataAdapter, dataSource.Dictionary.DataStore);
			if (dataAdapterCategories != null && dataAdapterCategories.Nodes.Count == 1 && dataAdapterCategories.Nodes[0].Nodes.Count == 1)
			{
				((Control)buttonEditNameInSource).Text = dataAdapterCategories.Nodes[0].Nodes[0].Text;
			}
		}
		if (dataSource is StiSqlSource && !(dataSource is StiODataSource))
		{
			numericUpDownCommandTimeout.Value = ((StiSqlSource)dataSource).CommandTimeout;
			((ListControl)comboBoxSqlSourceType).SelectedIndex = ((((StiSqlSource)dataSource).Type != 0) ? 1 : 0);
		}
		((Form)this).ShowIcon = false;
		((Control)btSaveCopy).Visible = !newForm;
		Build();
		tbRetrieveColumns.IsDrawDropDownArrow = dataSource is StiSqlSource && !(dataSource is StiODataSource);
	}

	private void Localize()
	{
		((Control)btSaveCopy).Text = StiLocalization.Get("Buttons", "SaveCopy");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)labelCommandTimeout).Text = Loc.GetCleaned("Report", "LabelQueryTimeout");
		((Control)gbQuery).Text = Loc.GetCleaned("QueryBuilder", "Query");
		((Control)labelNameInSource).Text = Loc.GetCleaned("Report", "LabelNameInSource");
		((Control)lbName).Text = Loc.GetCleaned("Report", "LabelName");
		((Control)lbAlias).Text = Loc.GetCleaned("Report", "LabelAlias");
		((Control)lbEncoding).Text = Loc.GetCleaned("Export", "Encoding");
		((Control)lbDelimiter).Text = Loc.GetCleaned("FormDictionaryDesigner", "LabelSeparator");
		if (dataSource is StiSqlSource && !(dataSource is StiMongoDbSource) && !(dataSource is StiODataSource))
		{
			((Control)gbColumns).Text = string.Format("{0} && {1}", StiLocalization.Get("PropertyMain", "Columns"), StiLocalization.Get("PropertyMain", "Parameters"));
		}
		else
		{
			((Control)gbColumns).Text = StiLocalization.Get("PropertyMain", "Columns");
		}
		ciColumnNew.Text = StiLocalization.Get("FormDictionaryDesigner", "ColumnNew");
		ciCalcColumnNew.Text = StiLocalization.Get("FormDictionaryDesigner", "CalcColumnNew");
		ciParameterNew.Text = StiLocalization.Get("FormDictionaryDesigner", "DataParameterNew");
		ciDelete.Text = StiLocalization.Get("FormDictionaryDesigner", "Delete");
		((Control)tbRetrieveColumns).Text = StiLocalization.Get("FormDictionaryDesigner", "RetrieveColumns");
		ciRetrieveColumnsAndParameters.Text = StiLocalization.Get("FormDictionaryDesigner", "RetrieveColumnsAndParameters");
		ciRetrieveParameters.Text = StiLocalization.Get("FormDictionaryDesigner", "RetrieveParameters");
		ciRetrieveColumnsAllowRun.Text = StiLocalization.Get("FormDictionaryDesigner", "RetrieveColumnsAllowRun");
		((Control)tbGetColumnsFromAssembly).Text = StiLocalization.Get("FormDictionaryDesigner", "GetColumnsFromAssembly");
		((Control)tbRunQueryBuilder).Text = StiLocalization.Get("QueryBuilder", "RunQueryBuilder");
		toolTip1.SetToolTip((Control)(object)tbRunQueryBuilder, StiHint.CreateHint(StiLocalization.Get("QueryBuilder", "RunQueryBuilder")));
		toolTip1.SetToolTip((Control)(object)tbColumnNew, StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "ColumnNew")));
		toolTip1.SetToolTip((Control)(object)tbCalcColumnNew, StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "CalcColumnNew")));
		toolTip1.SetToolTip((Control)(object)tbParameterNew, StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "DataParameterNew")));
		toolTip1.SetToolTip((Control)(object)tbDelete, StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "Delete")));
		toolTip1.SetToolTip((Control)(object)tbRunScript, StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "Run")));
		toolTip1.SetToolTip((Control)(object)tbEditSql, StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "EditQuery")));
		toolTip1.SetToolTip((Control)(object)tbViewData, StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "ViewQuery")));
		toolTip1.SetToolTip((Control)(object)toolButtonViewData, StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "ViewQuery")));
		((Control)labelType).Text = Loc.GetCleaned("Report", "LabelType");
		((ComboBox)comboBoxSqlSourceType).Items.Clear();
		((ComboBox)comboBoxSqlSourceType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiSqlSourceTypeTable"));
		((ComboBox)comboBoxSqlSourceType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiSqlSourceTypeStoredProcedure"));
		StiButtonSizeHelper.CheckSizeOfButton((Button)(object)btSaveCopy);
	}

	private void LoadSettings()
	{
		string text = ((dataSource is StiSqlSource) ? "StiDataStoreSourceEditForm" : "StiDataStoreSourceEditForm2");
		StiFormSettings.Load(text, (Form)(object)this);
		StiSettings.Load();
		if (dataSource is StiSqlSource)
		{
			((Control)gbColumns).Height = StiScale.I(StiSettings.GetInt(text, "Spl1", 300));
		}
		((Control)tvDataSource).Width = StiScale.I(StiSettings.GetInt(text, "Spl2", 300));
	}

	private void DoScaling()
	{
		buttonEditNameInSource.Image = StiReportDesignImages.Actions.EditSmall();
		tbSQL.ControlStyle = StiControlStyle.Office2013Blue;
		tbMain.ControlStyle = StiControlStyle.Office2013Blue;
		((TreeView)tvDataSource).BorderStyle = (BorderStyle)0;
		ilButtons.Images.Clear();
		ilButtons.ImageSize = new Size(StiScaleUI.I16, StiScaleUI.I16);
		ilButtons.TransparentColor = Color.Magenta;
		ilButtons.Images.Add("ColumnNew", StiReportDesignImages.Actions.NewDataColumn());
		ilButtons.Images.Add("CalcColumnNew", StiReportDesignImages.Actions.NewCalculatedColumn());
		ilButtons.Images.Add("ParameterNew", StiReportDesignImages.Actions.NewDataParameter());
		ilButtons.Images.Add("ViewData", StiReportDesignImages.Actions.ViewData());
		ilButtons.Images.Add("Delete", StiReportDesignImages.Actions.Remove());
		ilButtons.Images.Add("RetrieveColumns", StiReportDesignImages.Actions.RetrieveColumnsDown());
		ilButtons.Images.Add("Column", StiReportImages.Dictionary.DataColumn());
		ilButtons.Images.Add("CalcColumn", StiReportImages.Dictionary.CalcColumn());
		ilButtons.Images.Add("Parameter", StiReportImages.Dictionary.DataParameter());
		ilButtons.Images.Add("Variable", StiReportImages.Dictionary.Variable());
		ilButtons.Images.Add("CalcColumnBool", StiReportImages.Dictionary.CalcColumnBool());
		ilButtons.Images.Add("ColumnBool", StiReportImages.Dictionary.DataColumnBool());
		ilButtons.Images.Add("CalcColumnChar", StiReportImages.Dictionary.CalcColumnChar());
		ilButtons.Images.Add("ColumnChar", StiReportImages.Dictionary.DataColumnChar());
		ilButtons.Images.Add("CalcColumnDateTime", StiReportImages.Dictionary.CalcColumnDateTime());
		ilButtons.Images.Add("ColumnDateTime", StiReportImages.Dictionary.DataColumnDateTime());
		ilButtons.Images.Add("CalcColumnDecimal", StiReportImages.Dictionary.CalcColumnDateTime());
		ilButtons.Images.Add("ColumnDecimal", StiReportImages.Dictionary.DataColumnDecimal());
		ilButtons.Images.Add("CalcColumnInt", StiReportImages.Dictionary.CalcColumnInt());
		ilButtons.Images.Add("ColumnInt", StiReportImages.Dictionary.DataColumnInt());
		ilButtons.Images.Add("CalcColumnFloat", StiReportImages.Dictionary.CalcColumnFloat());
		ilButtons.Images.Add("ColumnFloat", StiReportImages.Dictionary.DataColumnFloat());
		ilButtons.Images.Add("CalcColumnImage", StiReportImages.Dictionary.CalcColumnImage());
		ilButtons.Images.Add("ColumnImage", StiReportImages.Dictionary.DataColumnImage());
		ilButtons.Images.Add("CalcColumnBinary", StiReportImages.Dictionary.CalcColumnBinary());
		ilButtons.Images.Add("ColumnBinary", StiReportImages.Dictionary.DataColumnBinary());
		ciColumnNew.Image = StiReportDesignImages.Actions.NewDataColumn();
		ciCalcColumnNew.Image = StiReportDesignImages.Actions.NewCalculatedColumn();
		ciParameterNew.Image = StiReportDesignImages.Actions.NewDataParameter();
		ciDelete.Image = StiReportDesignImages.Actions.Remove();
		((ButtonBase)tbDelete).Image = StiReportDesignImages.Actions.Remove();
		((ButtonBase)tbColumnNew).Image = StiReportDesignImages.Actions.NewDataColumn();
		((ButtonBase)tbCalcColumnNew).Image = StiReportDesignImages.Actions.NewCalculatedColumn();
		((ButtonBase)tbParameterNew).Image = StiReportDesignImages.Actions.NewDataParameter();
		((ButtonBase)tbRetrieveColumns).Image = StiReportDesignImages.Actions.RetrieveColumnsDown();
		((ButtonBase)tbRetrieveColumns).Image = StiReportDesignImages.Actions.RetrieveColumnsPlus();
		((ButtonBase)tbRunScript).Image = StiReportDesignImages.Actions.RunQuery();
		((ButtonBase)tbEditSql).Image = StiReportDesignImages.Actions.EditTable();
		((ButtonBase)tbViewData).Image = StiReportDesignImages.Actions.ViewData();
		((ButtonBase)toolButtonViewData).Image = StiReportDesignImages.Actions.ViewData();
		errorProvider.Icon = StiIconUtils.ResizeIcon(errorProvider.Icon, StiScale.I16, StiScale.I16);
		((Control)comboBoxSqlSourceType).Width = ((Control)numericUpDownCommandTimeout).Width;
	}

	private void Build()
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Expected O, but got Unknown
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Expected O, but got Unknown
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Expected O, but got Unknown
		object obj = null;
		if (((TreeView)tvDataSource).SelectedNode != null)
		{
			obj = ((TreeView)tvDataSource).SelectedNode.Tag;
		}
		((TreeView)tvDataSource).Nodes.Clear();
		TreeNode val = new TreeNode(StiLocalization.Get("PropertyMain", "Columns"))
		{
			ImageIndex = ilButtons.Images.IndexOfKey("Column"),
			SelectedImageIndex = ilButtons.Images.IndexOfKey("Column")
		};
		((TreeView)tvDataSource).Nodes.Add(val);
		foreach (StiDataColumn column in dataSource.Columns)
		{
			TreeNode val2 = new TreeNode(column.Name)
			{
				Tag = column
			};
			if (column is StiCalcDataColumn)
			{
				int imageIndex = (val2.SelectedImageIndex = ilButtons.Images.IndexOfKey(GetImageColumnName("CalcColumn", column.Type)));
				val2.ImageIndex = imageIndex;
			}
			else
			{
				int imageIndex = (val2.SelectedImageIndex = ilButtons.Images.IndexOfKey(GetImageColumnName("Column", column.Type)));
				val2.ImageIndex = imageIndex;
			}
			if (obj == column)
			{
				((TreeView)tvDataSource).SelectedNode = val2;
			}
			val.Nodes.Add(val2);
		}
		if (((TreeView)tvDataSource).SelectedNode == null)
		{
			((TreeView)tvDataSource).SelectedNode = val;
		}
		if (!(adapter is StiODataAdapterService) && !(adapter is StiQuickBooksAdapterService) && dataSource is StiSqlSource stiSqlSource)
		{
			TreeNode val3 = new TreeNode(StiLocalization.Get("PropertyMain", "Parameters"))
			{
				ImageIndex = ilButtons.Images.IndexOfKey("Parameter"),
				SelectedImageIndex = ilButtons.Images.IndexOfKey("Parameter")
			};
			((TreeView)tvDataSource).Nodes.Add(val3);
			foreach (StiDataParameter parameter in stiSqlSource.Parameters)
			{
				TreeNode val4 = new TreeNode(parameter.Name)
				{
					Tag = parameter,
					ImageIndex = ilButtons.Images.IndexOfKey("Parameter"),
					SelectedImageIndex = ilButtons.Images.IndexOfKey("Parameter")
				};
				if (obj == parameter)
				{
					((TreeView)tvDataSource).SelectedNode = val4;
				}
				val3.Nodes.Add(val4);
			}
			List<StiVariable> list = StiVariableAsParameterHelper.FetchAll(((Control)tbSqlCommand).Text, stiSqlSource.Parameters.ToList(), stiSqlSource.Dictionary.Report);
			foreach (StiVariable item in list)
			{
				TreeNode val5 = new TreeNode(item.Name)
				{
					Tag = item,
					ImageIndex = ilButtons.Images.IndexOfKey("Variable"),
					SelectedImageIndex = ilButtons.Images.IndexOfKey("Variable")
				};
				if (obj == item)
				{
					((TreeView)tvDataSource).SelectedNode = val5;
				}
				val3.Nodes.Add(val5);
			}
			if (((TreeView)tvDataSource).SelectedNode == null)
			{
				((TreeView)tvDataSource).SelectedNode = val;
			}
		}
		((TreeView)tvDataSource).ExpandAll();
	}

	private string GetImageColumnName(string name, Type type)
	{
		if (type == typeof(bool) || type == typeof(bool?))
		{
			return name + "Bool";
		}
		if (type == typeof(char) || type == typeof(char?))
		{
			return name + "Char";
		}
		if (type == typeof(DateTime) || type == typeof(TimeSpan) || type == typeof(DateTime?) || type == typeof(TimeSpan?))
		{
			return name + "DateTime";
		}
		if (type == typeof(decimal) || type == typeof(decimal?))
		{
			return name + "Decimal";
		}
		if (type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong) || type == typeof(byte) || type == typeof(sbyte) || type == typeof(short) || type == typeof(ushort) || type == typeof(int?) || type == typeof(uint?) || type == typeof(long?) || type == typeof(ulong?) || type == typeof(byte?) || type == typeof(sbyte?) || type == typeof(short?) || type == typeof(ushort?))
		{
			return name + "Int";
		}
		if (type == typeof(float) || type == typeof(double) || type == typeof(float?) || type == typeof(double?))
		{
			return name + "Float";
		}
		if (type == typeof(Image) || type == typeof(Bitmap))
		{
			return name + "Image";
		}
		if (type != null && type.IsArray)
		{
			return name + "Binary";
		}
		return name;
	}

	private IDbConnection GetConnection()
	{
		IDbConnection result = null;
		if (dataSource is StiSqlSource)
		{
			foreach (StiData item in dataSource.Dictionary.DataStore)
			{
				if (item.Name.ToLowerInvariant() == ((Control)buttonEditNameInSource).Text.ToLowerInvariant() && item.Data is IDbConnection)
				{
					result = item.Data as IDbConnection;
				}
			}
		}
		return result;
	}

	private static object GetNextNodeTag(TreeNode node)
	{
		if (node.Parent != null)
		{
			int num = node.Parent.Nodes.IndexOf(node);
			if (num < node.Parent.Nodes.Count - 1)
			{
				node = node.Parent.Nodes[num + 1];
			}
			else if (num > 0)
			{
				node = node.Parent.Nodes[num - 1];
			}
		}
		return node.Tag;
	}

	private static TreeNode GetNodeByTag(TreeNodeCollection nodes, object tag)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		foreach (TreeNode node in nodes)
		{
			TreeNode val = node;
			if (val.Tag == tag)
			{
				return val;
			}
			TreeNode nodeByTag = GetNodeByTag(val.Nodes, tag);
			if (nodeByTag != null)
			{
				return nodeByTag;
			}
		}
		return null;
	}

	public TreeNode GetNodeByTag(object tag)
	{
		return GetNodeByTag(((TreeView)tvDataSource).Nodes, tag);
	}

	public void SelectNodeByTag(object tag)
	{
		((TreeView)tvDataSource).SelectedNode = GetNodeByTag(tag);
		StiTreeViewUX stiTreeViewUX = tvDataSource;
		if (stiTreeViewUX != null)
		{
			TreeNode selectedNode = ((TreeView)stiTreeViewUX).SelectedNode;
			if (selectedNode != null)
			{
				selectedNode.EnsureVisible();
			}
		}
	}

	private void SaveParam(ref StiDataStoreSource dataSourceWithParam)
	{
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Expected O, but got Unknown
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Expected O, but got Unknown
		string text = ((Control)tbName).Text;
		if (CopyModeActivated)
		{
			dataSourceWithParam = StiActivator.CreateObject(dataSource.GetType()) as StiDataStoreSource;
			dictionary.DataSources.Add(dataSourceWithParam);
			text = ((Control)tbName).Text;
			if (dictionary.DataSources.Contains(text))
			{
				int num = 1;
				while (true)
				{
					text = ((num == 1) ? (((Control)tbName).Text + Loc.Get("Report", "CopyOf")) : string.Format("{0}{1}{2}", ((Control)tbName).Text, Loc.Get("Report", "CopyOf"), num));
					if (!dictionary.DataSources.Contains(text))
					{
						break;
					}
					num++;
				}
			}
		}
		if (dataSourceWithParam == null)
		{
			dataSourceWithParam = StiActivator.CreateObject(dataSource.GetType()) as StiDataStoreSource;
		}
		StiDataColumnsCollection stiDataColumnsCollection = new StiDataColumnsCollection();
		stiDataColumnsCollection.AddRange(dataSourceWithParam.Columns);
		dataSourceWithParam.Columns.Clear();
		foreach (TreeNode node in ((TreeView)tvDataSource).Nodes[0].Nodes)
		{
			TreeNode val = node;
			StiDataColumn stiDataColumn = (val.Tag as StiDataColumn).Clone() as StiDataColumn;
			StiDataColumn stiDataColumn2 = dataSourceWithParam.Columns[stiDataColumn.Name];
			dataSourceWithParam.Columns.Add(stiDataColumn2 ?? stiDataColumn);
		}
		dataSourceWithParam.NameInSource = ((Control)buttonEditNameInSource).Text;
		dataSourceWithParam.Name = ((Control)tbName).Text;
		dataSourceWithParam.Alias = ((Control)tbAlias).Text;
		if (dataSourceWithParam is StiSqlSource stiSqlSource)
		{
			StiDataParametersCollection stiDataParametersCollection = new StiDataParametersCollection();
			stiDataParametersCollection.AddRange(dataSourceWithParam.Parameters);
			dataSourceWithParam.Parameters.Clear();
			if (((TreeView)tvDataSource).Nodes.Count > 1)
			{
				foreach (TreeNode node2 in ((TreeView)tvDataSource).Nodes[1].Nodes)
				{
					TreeNode val2 = node2;
					if (val2.Tag is StiDataParameter stiDataParameter)
					{
						StiDataParameter stiDataParameter2 = dataSourceWithParam.Parameters[stiDataParameter.Name];
						dataSourceWithParam.Parameters.Add(stiDataParameter2 ?? stiDataParameter);
					}
				}
			}
			((StiSqlSource)dataSource).CommandTimeout = (int)numericUpDownCommandTimeout.Value;
			stiSqlSource.Type = ((((ListControl)comboBoxSqlSourceType).SelectedIndex != 0) ? StiSqlSourceType.StoredProcedure : StiSqlSourceType.Table);
			stiSqlSource.SqlCommand = ((Control)tbSqlCommand).Text;
			stiSqlSource.AllowExpressions = ((StiSqlSource)dataSource).AllowExpressions;
		}
		if (dataSourceWithParam is StiDBaseSource stiDBaseSource)
		{
			stiDBaseSource.CodePage = StiDBaseHelper.CodePageCodes[((ListControl)comboBoxCodePage).SelectedIndex, 1];
		}
		if (dataSourceWithParam is StiCsvSource stiCsvSource)
		{
			stiCsvSource.CodePage = StiCsvHelper.CodePageCodes[((ListControl)comboBoxCodePage).SelectedIndex];
			stiCsvSource.Separator = GetSeparatorFromIndex();
		}
		if (dataSourceWithParam.Name == dataSourceWithParam.Alias)
		{
			dataSourceWithParam.Alias = text;
		}
		dataSourceWithParam.Name = text;
	}

	private string GetSeparatorFromIndex()
	{
		switch (((ListControl)comboBoxDelimiter).SelectedIndex)
		{
		case 1:
			return "\t";
		case 2:
			return ";";
		case 3:
			return ",";
		case 4:
			return " ";
		case 5:
			if (string.IsNullOrEmpty(((Control)textBoxCustomDelimeter).Text))
			{
				return string.Empty;
			}
			return ((Control)textBoxCustomDelimeter).Text;
		default:
			return string.Empty;
		}
	}

	private static void NodeSetCheck(TreeNode node, bool check)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		foreach (TreeNode node2 in node.Nodes)
		{
			TreeNode val = node2;
			val.Checked = check;
			NodeSetCheck(val, check);
		}
	}

	private string GetColumnName(StiDataSource dataSource, string baseName)
	{
		string text = baseName;
		int num = 1;
		while (dataSource.Columns.Contains(text))
		{
			text = $"{baseName}{num++}";
		}
		return text;
	}

	private void TreeViewDataSource_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 46 && ((Control)tbDelete).Enabled)
		{
			e.Handled = true;
			ButtonDelete_Click(sender, (EventArgs)(object)e);
		}
	}

	private void ComboBoxDelimiter_SelectedValueChanged(object sender, EventArgs e)
	{
		((Control)textBoxCustomDelimeter).Visible = ((ComboBox)comboBoxDelimiter).SelectedItem.ToString().Equals(StiLocalization.Get("FormDictionaryDesigner", "CsvSeparatorOther"));
	}

	private void ButtonRunScript_Click(object sender, EventArgs e)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Invalid comparison between Unknown and I4
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			StiDataStoreSource dataSourceWithParam = null;
			SaveParam(ref dataSourceWithParam);
			if (string.IsNullOrWhiteSpace(dataSourceWithParam?.NameInSource))
			{
				MessageBox.Show(string.Format(StiLocalization.Get("Errors", "FieldRequire"), StiLocalization.Get("Report", "LabelNameInSource")));
				return;
			}
			StiSqlSource stiSqlSource = dataSourceWithParam as StiSqlSource;
			List<string> lexem = StiCodeDomExpressionHelper.GetLexem(stiSqlSource.SqlCommand);
			DialogResult val = (DialogResult)1;
			StiSqlExpressionsForm stiSqlExpressionsForm = new StiSqlExpressionsForm(stiSqlSource, lexem, dataSource.Dictionary.Report);
			try
			{
				if (stiSqlExpressionsForm.CheckResult)
				{
					val = ((Form)stiSqlExpressionsForm).ShowDialog((IWin32Window)(object)this);
					stiSqlSource.SqlCommand = stiSqlExpressionsForm.Value;
				}
			}
			finally
			{
				((IDisposable)stiSqlExpressionsForm)?.Dispose();
			}
			if ((int)val != 1)
			{
				return;
			}
			try
			{
				dataSourceWithParam.Dictionary = dataSource.Dictionary;
				dataSourceWithParam.Dictionary.ConnectToDatabases();
				StiDataLeader.Connect(dataSourceWithParam);
				Build();
				MessageBox.Show(StiLocalization.Get("FormDictionaryDesigner", "ExecutedSQLStatementSuccessfully"));
			}
			finally
			{
				StiDataLeader.Disconnect(dataSourceWithParam);
				dataSource.Dictionary.Disconnect();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ButtonEditSql_Click(object sender, EventArgs e)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Invalid comparison between Unknown and I4
		string text = ((Control)tbSqlCommand).Text;
		DialogResult val = StiTextEditor.EditText(dictionary.Report, ref text, StiCodeEditorMode.EditorSQL);
		if ((int)val == 1)
		{
			((Control)tbSqlCommand).Text = text;
		}
	}

	private void ButtonEditNameInSource_ButtonClick(object sender, EventArgs e)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Invalid comparison between Unknown and I4
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Invalid comparison between Unknown and I4
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Invalid comparison between Unknown and I4
		if (dataSource is StiDBaseSource || dataSource is StiCsvSource)
		{
			if (dataSource is StiDBaseSource)
			{
				OpenFileDialog val = new OpenFileDialog();
				try
				{
					((FileDialog)val).DefaultExt = "dbf";
					((FileDialog)val).Filter = "dBase files|*.dbf";
					if ((int)((CommonDialog)val).ShowDialog() == 1)
					{
						((Control)buttonEditNameInSource).Text = ((FileDialog)val).FileName;
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			if (!(dataSource is StiCsvSource))
			{
				return;
			}
			OpenFileDialog val2 = new OpenFileDialog();
			try
			{
				((FileDialog)val2).DefaultExt = "csv";
				((FileDialog)val2).Filter = "Csv files|*.csv";
				if ((int)((CommonDialog)val2).ShowDialog() == 1)
				{
					((Control)buttonEditNameInSource).Text = ((FileDialog)val2).FileName;
				}
				return;
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		dataSource.Dictionary.Connect(loadData: false);
		StiDataControlForm stiDataControlForm = new StiDataControlForm(dictionary, adapter, ((Control)buttonEditNameInSource).Text);
		try
		{
			if ((int)((Form)stiDataControlForm).ShowDialog((IWin32Window)(object)this) == 1)
			{
				((Control)buttonEditNameInSource).Text = stiDataControlForm.NameInSource;
			}
		}
		finally
		{
			((IDisposable)stiDataControlForm)?.Dispose();
		}
		dataSource.Dictionary.Disconnect();
	}

	private void TreeViewDataSource_AfterSelect(object sender, TreeViewEventArgs e)
	{
		((PropertyGrid)pgDataSource).SelectedObject = ((TreeView)tvDataSource).SelectedNode.Tag;
		((Control)tbDelete).Enabled = !(((TreeView)tvDataSource).SelectedNode.Tag is StiVariable);
	}

	private void ButtonColumnNew_Click(object sender, EventArgs e)
	{
		StiDataColumn stiDataColumn = new StiDataColumn();
		string name = StiNameCreation.CreateColumnName(dataSource, stiDataColumn.Name);
		stiDataColumn.Name = name;
		dataSource.Columns.Add(stiDataColumn);
		Build();
		SelectNodeByTag(stiDataColumn);
	}

	private void ButtonCalcColumnNew_Click(object sender, EventArgs e)
	{
		StiCalcDataColumn stiCalcDataColumn = new StiCalcDataColumn();
		string name = StiNameCreation.CreateColumnName(dataSource, stiCalcDataColumn.Name);
		stiCalcDataColumn.Name = name;
		dataSource.Columns.Add(stiCalcDataColumn);
		Build();
		SelectNodeByTag(stiCalcDataColumn);
	}

	private void ButtonParameterNew_Click(object sender, EventArgs e)
	{
		StiSqlSource stiSqlSource = dataSource as StiSqlSource;
		string name = StiNameCreation.CreateParameterName(stiSqlSource, Loc.Get("PropertyMain", "Parameter"));
		StiDataParameter stiDataParameter = stiSqlSource.AddParameter();
		stiDataParameter.Name = name;
		Build();
		SelectNodeByTag(stiDataParameter);
	}

	private void ButtonDelete_Click(object sender, EventArgs e)
	{
		if (((TreeView)tvDataSource).SelectedNode != null)
		{
			if (((TreeView)tvDataSource).SelectedNode.Tag == null && ((TreeView)tvDataSource).SelectedNode.Text == StiLocalization.Get("PropertyMain", "Columns"))
			{
				dataSource.Columns.Clear();
				Build();
			}
			else if (((TreeView)tvDataSource).SelectedNode.Tag == null && ((TreeView)tvDataSource).SelectedNode.Text == StiLocalization.Get("PropertyMain", "Parameters"))
			{
				((StiSqlSource)dataSource).Parameters.Clear();
				Build();
			}
			else if (((TreeView)tvDataSource).SelectedNode.Tag is StiDataColumn)
			{
				dataSource.Columns.Remove(((TreeView)tvDataSource).SelectedNode.Tag as StiDataColumn);
				object nextNodeTag = GetNextNodeTag(((TreeView)tvDataSource).SelectedNode);
				Build();
				SelectNodeByTag(nextNodeTag);
			}
			else if (((TreeView)tvDataSource).SelectedNode.Tag is StiDataParameter)
			{
				((StiSqlSource)dataSource).Parameters.Remove(((TreeView)tvDataSource).SelectedNode.Tag as StiDataParameter);
				object nextNodeTag2 = GetNextNodeTag(((TreeView)tvDataSource).SelectedNode);
				Build();
				SelectNodeByTag(nextNodeTag2);
			}
		}
	}

	private void ButtonRetrieveColumns_Click(object sender, EventArgs e)
	{
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Invalid comparison between Unknown and I4
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Expected O, but got Unknown
		try
		{
			dictionary.Connect(loadData: false);
			SaveParam(ref dataSource);
			if (!(dataSource is StiSqlSource) && !(dataSource is StiGoogleSheetsSource))
			{
				StiDataStoreSource stiDataStoreSource = dataSource;
				if (string.IsNullOrWhiteSpace(stiDataStoreSource?.NameInSource))
				{
					return;
				}
				StiDataAdapterService dataAdapter = StiDataAdapterService.GetDataAdapter(dataSource);
				if (dataAdapter == null)
				{
					return;
				}
				StiData data = null;
				foreach (StiData item in dataSource.Dictionary.DataStore)
				{
					if (item.Name.ToLowerInvariant() == stiDataStoreSource.NameInSource.ToLowerInvariant())
					{
						data = item;
						break;
					}
				}
				try
				{
					dataSource.Dictionary.SynchronizeColumns(data, dataSource);
					return;
				}
				catch (Exception exception)
				{
					StiExceptionProvider.Show(exception);
					return;
				}
			}
			DialogResult val = (DialogResult)1;
			string sqlCommand = string.Empty;
			StiSqlSource stiSqlSource = dataSource as StiSqlSource;
			if (stiSqlSource != null)
			{
				sqlCommand = stiSqlSource.SqlCommand;
				if (stiSqlSource.NameInSource == null || stiSqlSource.NameInSource.Trim().Length == 0)
				{
					MessageBox.Show(string.Format(StiLocalization.Get("Errors", "FieldRequire"), StiLocalization.Get("Report", "LabelNameInSource")));
					return;
				}
				List<string> lexem = StiCodeDomExpressionHelper.GetLexem(stiSqlSource.SqlCommand);
				StiSqlExpressionsForm stiSqlExpressionsForm = new StiSqlExpressionsForm(stiSqlSource, lexem, dataSource.Dictionary.Report);
				try
				{
					if (stiSqlExpressionsForm.CheckResult)
					{
						val = ((Form)stiSqlExpressionsForm).ShowDialog((IWin32Window)(object)this);
						stiSqlSource.SqlCommand = stiSqlExpressionsForm.Value;
					}
				}
				finally
				{
					((IDisposable)stiSqlExpressionsForm)?.Dispose();
				}
			}
			if ((int)val == 1)
			{
				StiDataColumnsCollection stiDataColumnsCollection = null;
				foreach (StiData item2 in dictionary.DataStore)
				{
					if (item2.Name.ToLowerInvariant() == ((Control)buttonEditNameInSource).Text.ToLowerInvariant())
					{
						stiDataColumnsCollection = StiDataLeader.GetColumnsFromData(adapter, item2, dataSource, ciRetrieveColumnsAllowRun.Checked ? CommandBehavior.KeyInfo : CommandBehavior.SchemaOnly);
						break;
					}
				}
				foreach (StiDataStoreSource dataSource in dictionary.DataSources)
				{
					if (!(dataSource.NameInSource.ToLowerInvariant() == ((Control)buttonEditNameInSource).Text.ToLowerInvariant()) || !(((Control)tbName).Text.ToLowerInvariant() == dataSource.Name.ToLowerInvariant()))
					{
						continue;
					}
					if (!(dataSource.GetConnection() is StiODataDatabase stiODataDatabase))
					{
						break;
					}
					stiDataColumnsCollection = new StiDataColumnsCollection();
					StiODataConnector stiODataConnector = stiODataDatabase.CreateConnector(stiODataDatabase.ConnectionString) as StiODataConnector;
					foreach (StiDataColumnSchema column in stiODataConnector.GetColumns(dataSource.Name))
					{
						stiDataColumnsCollection.Add(new StiDataColumn(column.Name, column.Name, column.Type));
					}
					break;
				}
				try
				{
					if (stiDataColumnsCollection != null)
					{
						if (stiDataColumnsCollection.ToList().Any((StiDataColumn c) => string.IsNullOrWhiteSpace(c.NameInSource)) && this.dataSource is StiSqlSource)
						{
							try
							{
								this.dataSource.Dictionary.Connect(loadData: false, new List<StiDataSource> { this.dataSource });
								StiDataLeader.Connect(this.dataSource);
								StiDataColumnsCollection stiDataColumnsCollection2 = new StiDataColumnsCollection((from DataColumn c in this.dataSource.DataTable.Columns
									select new StiDataColumn(c.ColumnName, c.Caption, c.DataType)).Cast<StiDataColumn>().ToList());
								if (stiDataColumnsCollection2.Count > 0)
								{
									stiDataColumnsCollection = stiDataColumnsCollection2;
								}
							}
							finally
							{
								this.dataSource.Dictionary.Disconnect();
							}
						}
						foreach (StiDataColumn item3 in stiDataColumnsCollection)
						{
							int num = 0;
							foreach (TreeNode node in ((TreeView)tvDataSource).Nodes[0].Nodes)
							{
								TreeNode val2 = node;
								StiDataColumn stiDataColumn2 = val2.Tag as StiDataColumn;
								if (!(item3.Name == stiDataColumn2.Name))
								{
									num++;
									continue;
								}
								break;
							}
							if (num == ((TreeView)tvDataSource).Nodes[0].Nodes.Count)
							{
								string text = item3.Name;
								if (string.IsNullOrWhiteSpace(text))
								{
									text = Loc.GetMain("Column");
								}
								text = GetColumnName(this.dataSource, text);
								if (string.IsNullOrWhiteSpace(item3.NameInSource))
								{
									item3.NameInSource = text;
								}
								this.dataSource.Columns.Add(new StiDataColumn(item3.NameInSource, text, text, item3.Type));
							}
						}
					}
				}
				catch (Exception exception2)
				{
					StiExceptionProvider.Show(exception2);
				}
			}
			if (stiSqlSource != null)
			{
				stiSqlSource.SqlCommand = sqlCommand;
			}
			dictionary.Disconnect();
		}
		finally
		{
			Build();
		}
	}

	private void ButtonRetrieveParameters_Click(object sender, EventArgs e)
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		dictionary.Connect(loadData: false);
		SaveParam(ref dataSource);
		StiSqlSource stiSqlSource = dataSource as StiSqlSource;
		string sqlCommand = stiSqlSource.SqlCommand;
		if (stiSqlSource.NameInSource == null || stiSqlSource.NameInSource.Trim().Length == 0)
		{
			MessageBox.Show(string.Format(StiLocalization.Get("Errors", "FieldRequire"), StiLocalization.Get("Report", "LabelNameInSource")));
			return;
		}
		StiData stiData = null;
		foreach (StiData item in dictionary.DataStore)
		{
			if (item.Name.ToLower(CultureInfo.InvariantCulture) == ((Control)buttonEditNameInSource).Text.ToLower(CultureInfo.InvariantCulture))
			{
				stiData = item;
				break;
			}
		}
		try
		{
			if (stiData != null)
			{
				StiDataParametersCollection parametersFromData = adapter.GetParametersFromData(stiData, dataSource);
				foreach (StiDataParameter item2 in parametersFromData)
				{
					int num = 0;
					foreach (TreeNode node in ((TreeView)tvDataSource).Nodes[1].Nodes)
					{
						TreeNode val = node;
						StiDataParameter stiDataParameter2 = val.Tag as StiDataParameter;
						if (!(item2.Name == stiDataParameter2.Name))
						{
							num++;
							continue;
						}
						break;
					}
					if (num == ((TreeView)tvDataSource).Nodes[1].Nodes.Count)
					{
						dataSource.Parameters.Add(new StiDataParameter(item2.Name, item2.Type, item2.Size));
					}
				}
				Build();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
		stiSqlSource.SqlCommand = sqlCommand;
		dictionary.Disconnect();
	}

	private void TreeViewDataSource_BeforeCheck(object sender, TreeViewCancelEventArgs e)
	{
		NodeSetCheck(e.Node, !e.Node.Checked);
	}

	private void TextBoxName_Enter(object sender, EventArgs e)
	{
		oldValue = ((Control)tbName).Text;
	}

	private void TextBoxName_Leave(object sender, EventArgs e)
	{
		if (oldValue == ((Control)tbAlias).Text)
		{
			((Control)tbAlias).Text = ((Control)tbName).Text;
		}
	}

	private void PropertyGridDataSource_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
	{
		Build();
	}

	private void ButtonViewData_Click(object sender, EventArgs e)
	{
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		dataSource.Dictionary.Connect(loadData: false);
		StiDataStoreSource dataSourceWithParam = null;
		try
		{
			SaveParam(ref dataSourceWithParam);
			dataSourceWithParam.Dictionary = dataSource.Dictionary;
			StiDataLeader.Connect(dataSourceWithParam, loadData: false);
			Build();
			StiViewDataForm stiViewDataForm = new StiViewDataForm(dataSourceWithParam);
			try
			{
				dataSourceWithParam?.Dictionary?.Report?.Designer?.AddOwnedForm((Form)(object)stiViewDataForm);
				((Control)stiViewDataForm).Text = StiLocalization.Get("FormDictionaryDesigner", "ViewQuery");
				((Form)stiViewDataForm).ShowDialog();
			}
			finally
			{
				((IDisposable)stiViewDataForm)?.Dispose();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
		finally
		{
			StiDataLeader.Disconnect(dataSourceWithParam);
			dataSource.Dictionary.Disconnect();
		}
	}

	private void ButtonGetColumnsFromAssembly_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Invalid comparison between Unknown and I4
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Invalid comparison between Unknown and I4
		OpenFileDialog val = new OpenFileDialog();
		try
		{
			((FileDialog)val).Filter = "Assembly files (*.exe;*.dll)|*.exe;*.dll";
			((FileDialog)val).Title = StiLocalization.Get("FormDictionaryDesigner", "OpenAssembly");
			((FileDialog)val).RestoreDirectory = true;
			if ((int)((CommonDialog)val).ShowDialog() != 1 || !(((FileDialog)val).FileName != ""))
			{
				return;
			}
			Assembly assembly = null;
			try
			{
				assembly = Assembly.LoadFrom(((FileDialog)val).FileName);
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
			StiSelectPropertiesInAssemblyForm stiSelectPropertiesInAssemblyForm = new StiSelectPropertiesInAssemblyForm(assembly);
			try
			{
				if ((int)((Form)stiSelectPropertiesInAssemblyForm).ShowDialog((IWin32Window)(object)this) != 1)
				{
					return;
				}
				List<TreeNode> selectColumns = stiSelectPropertiesInAssemblyForm.SelectColumns;
				foreach (TreeNode item in selectColumns)
				{
					dataSource.Columns.Add(new StiDataColumn(item.Text, item.Text, item.Tag as Type));
				}
				Build();
			}
			finally
			{
				((IDisposable)stiSelectPropertiesInAssemblyForm)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void QueryBuilder_SaveQuery(object sender, EventArgs e)
	{
		((Control)tbSqlCommand).Text = sender as string;
	}

	private void ButtonRunQueryBuilder_Click(object sender, EventArgs e)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (((Control)buttonEditNameInSource).Text.Trim().Length == 0)
			{
				MessageBox.Show(string.Format(StiLocalization.Get("Errors", "FieldRequire"), StiLocalization.Get("Report", "LabelNameInSource")));
				return;
			}
			Assembly assembly = StiDatabaseAssembly.GetAssembly();
			if (assembly == null)
			{
				MessageBox.Show("Assembly \"Stimulsoft.Database.dll\" not founded.");
				return;
			}
			Type type = StiTypeFinder.GetType("Stimulsoft.Database.StiQueryBuilderActivator");
			if (type == null)
			{
				MessageBox.Show("Type \"Stimulsoft.Database.StiQueryBuilderActivator\" not founded in assembly \"Stimulsoft.Database.dll\"");
				return;
			}
			try
			{
				dataSource.Dictionary.Connect(loadData: false);
				IDbConnection connection = GetConnection();
				if (connection == null)
				{
					MessageBox.Show("Error: Unknown connection");
					return;
				}
				object syntaxProvider = null;
				object metadataProvider = null;
				StiDataAdapterService dataAdapter = StiDataAdapterService.GetDataAdapter(dataSource);
				if (dataAdapter != null)
				{
					syntaxProvider = dataAdapter.GetSyntaxProvider();
					metadataProvider = dataAdapter.GetMetadataProvider(connection);
				}
				IStiQueryBuilderActivator stiQueryBuilderActivator = StiActivator.CreateObject(type) as IStiQueryBuilderActivator;
				string text = stiQueryBuilderActivator.RunQueryBuilder(connection, ((Control)tbSqlCommand).Text, QueryBuilder_SaveQuery, null, syntaxProvider, metadataProvider);
				if (text != null)
				{
					((Control)tbSqlCommand).Text = text;
				}
			}
			finally
			{
				dataSource.Dictionary.Disconnect();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ComboBoxSqlSourceType_SelectedIndexChanged(object sender, EventArgs e)
	{
		ciRetrieveParameters.Enabled = dataSource.GetType() == typeof(StiSqlSource) && ((ListControl)comboBoxSqlSourceType).SelectedIndex == 1;
	}

	private void TextBoxSqlCommand_Leave(object sender, EventArgs e)
	{
		Build();
	}

	private void ButtonShowRetrieveParametersMenu_Click(object sender, EventArgs e)
	{
		if (dataSource is StiSqlSource && !(dataSource is StiODataSource))
		{
			Point p = ((Control)tbRetrieveColumns).PointToScreen(((Control)tbRetrieveColumns).ClientRectangle.Location);
			p.Y += ((Control)tbRetrieveColumns).Height;
			menuRetrieving.Popup(p);
		}
		else
		{
			ButtonRetrieveColumns_Click(sender, e);
		}
	}

	private void This_Load(object sender, EventArgs e)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		LoadSettings();
		pgDataSource.SetStandardPropertyTabs();
		if (((Control)tbSqlCommand).Height < StiScale.YYI(10.0))
		{
			StiOffice2013GroupBox stiOffice2013GroupBox = gbColumns;
			((Control)stiOffice2013GroupBox).Height = ((Control)stiOffice2013GroupBox).Height - StiScale.YYI(40.0);
		}
		Timer timer = new Timer
		{
			Interval = 150
		};
		timer.Tick += delegate
		{
			timer.Stop();
			((Control)buttonEditNameInSource).Focus();
			((Control)buttonEditNameInSource).Select();
			((Component)(object)timer).Dispose();
		};
		timer.Start();
	}

	private void This_Closed(object sender, EventArgs e)
	{
		string text = ((dataSource is StiSqlSource) ? "StiDataStoreSourceEditForm" : "StiDataStoreSourceEditForm2");
		StiSettings.Load();
		if (dataSource is StiSqlSource)
		{
			StiSettings.Set(text, "Spl1", (int)((double)((Control)gbColumns).Height / StiScale.Factor));
		}
		StiSettings.Set(text, "Spl2", (int)((double)((Control)tvDataSource).Width / StiScale.Factor));
		StiSettings.Save();
		StiFormSettings.Save(text, (Form)(object)this);
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		dataSource.Columns.Clear();
		foreach (StiDataColumn column in Columns)
		{
			dataSource.Columns.Add(column);
		}
		((Form)this).Close();
	}

	private void This_Closing(object sender, CancelEventArgs e)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Invalid comparison between Unknown and I4
		if (((Control)tbSqlCommand.Editor).Focused)
		{
			e.Cancel = (int)MessageBox.Show(StiLocalization.Get("DesignerFx", "CloseDataSourceEditor"), ((Control)this).Text, (MessageBoxButtons)4) == 7;
		}
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/data_data_dictionary_datasources.htm");
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		bool flag = true;
		if (((Control)tbAlias).Text.Trim().Length == 0)
		{
			errorProvider.SetError((Control)(object)tbAlias, StiFieldChecker.AliasRequire);
			flag = false;
			((Control)tbAlias).Focus();
		}
		else
		{
			errorProvider.SetError((Control)(object)tbAlias, string.Empty);
		}
		if (((Control)tbName).Text.Trim().Length == 0)
		{
			errorProvider.SetError((Control)(object)tbName, StiFieldChecker.NameRequire);
			flag = false;
			((Control)tbName).Focus();
		}
		else
		{
			if (((Control)tbName).Text != originalName && StiNameCreation.Exists(originalReportClone, ((Control)tbName).Text))
			{
				errorProvider.SetError((Control)(object)tbName, StiFieldChecker.NameExists(((Control)tbName).Text));
				((Control)tbName).Focus();
				return;
			}
			errorProvider.SetError((Control)(object)tbName, string.Empty);
		}
		if (flag)
		{
			((Form)this).DialogResult = (DialogResult)1;
			SaveParam(ref dataSource);
			((Form)this).Close();
		}
	}

	private void This_SaveCopyClick(object sender, EventArgs e)
	{
		CopyModeActivated = true;
		This_OkClick(sender, e);
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
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Expected O, but got Unknown
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Expected O, but got Unknown
		//IL_05e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f0: Expected O, but got Unknown
		//IL_08af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a45: Expected O, but got Unknown
		//IL_0da2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eac: Expected O, but got Unknown
		//IL_1326: Unknown result type (might be due to invalid IL or missing references)
		//IL_1330: Expected O, but got Unknown
		//IL_1759: Unknown result type (might be due to invalid IL or missing references)
		//IL_1763: Expected O, but got Unknown
		//IL_1770: Unknown result type (might be due to invalid IL or missing references)
		//IL_177a: Expected O, but got Unknown
		//IL_1787: Unknown result type (might be due to invalid IL or missing references)
		//IL_1791: Expected O, but got Unknown
		//IL_1858: Unknown result type (might be due to invalid IL or missing references)
		//IL_1862: Expected O, but got Unknown
		//IL_1ac3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bef: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bf9: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiDataStoreSourceEditForm));
		lbAlias = new StiLabelUX();
		tbAlias = new StiTextBox();
		lbName = new StiLabelUX();
		tbName = new StiTextBox();
		labelNameInSource = new StiLabelUX();
		errorProvider = new ErrorProvider(components);
		pnMain = new StiPanelUX();
		gbQuery = new StiOffice2013GroupBox();
		pnQuery = new StiPanelUX();
		tbSQL = new StiToolBar();
		tbRunQueryBuilder = new StiToolButton();
		tbViewData = new StiToolButton();
		ilButtons = new ImageList(components);
		tbEditSql = new StiToolButton();
		tbRunScript = new StiToolButton();
		tbSqlCommand = new StiCodeEditor();
		syntaxEdit1 = new SyntaxEdit(components);
		labelType = new StiLabelUX();
		comboBoxSqlSourceType = new StiComboBox();
		labelCommandTimeout = new StiLabelUX();
		numericUpDownCommandTimeout = new StiNumericUpDown();
		splMain = new Splitter();
		gbColumns = new StiOffice2013GroupBox();
		pnColumns = new StiPanelUX();
		ContextMenuBar = new ContextMenuBar();
		menuColumns = new ButtonItem();
		ciColumnNew = new ButtonItem();
		ciCalcColumnNew = new ButtonItem();
		ciParameterNew = new ButtonItem();
		ciDelete = new ButtonItem();
		menuRetrieving = new ButtonItem();
		ciRetrieveColumnsAndParameters = new ButtonItem();
		ciRetrieveParameters = new ButtonItem();
		ciRetrieveColumnsAllowRun = new ButtonItem();
		tvDataSource = new StiTreeViewUX();
		pgDataSource = new StiReportPropertyGrid();
		panel2 = new StiPanelUX();
		splitterColumns = new Splitter();
		panel1 = new StiPanelUX();
		panel3 = new StiPanelUX();
		tbMain = new StiToolBar();
		tbGetColumnsFromAssembly = new StiToolButton();
		tbRetrieveColumns = new StiToolButton();
		toolButtonSeparator = new StiToolButton();
		toolButtonViewData = new StiToolButton();
		stiToolButton1 = new StiToolButton();
		tbDelete = new StiToolButton();
		tbParameterNew = new StiToolButton();
		tbCalcColumnNew = new StiToolButton();
		tbColumnNew = new StiToolButton();
		toolTip1 = new ToolTip(components);
		comboBoxCodePage = new StiComboBox();
		lbEncoding = new StiLabelUX();
		lbDelimiter = new StiLabelUX();
		comboBoxDelimiter = new StiComboBox();
		textBoxCustomDelimeter = new StiTextBox();
		panelName = new StiPanelUX();
		buttonEditNameInSource = new StiButtonEdit();
		panelEncoding = new StiPanelUX();
		panelDelimiter = new StiPanelUX();
		btOk = new StiButtonUX();
		btCancel = new StiButtonUX();
		stiGroupLine1 = new StiButtonPanelUX();
		btSaveCopy = new StiButtonUX();
		DotNetBarManager = new DotNetBarManager(components);
		((ISupportInitialize)errorProvider).BeginInit();
		((Control)pnMain).SuspendLayout();
		((Control)gbQuery).SuspendLayout();
		((Control)pnQuery).SuspendLayout();
		((Control)tbSQL).SuspendLayout();
		((Control)tbSqlCommand).SuspendLayout();
		((ISupportInitialize)numericUpDownCommandTimeout).BeginInit();
		((Control)gbColumns).SuspendLayout();
		((Control)pnColumns).SuspendLayout();
		((ISupportInitialize)ContextMenuBar).BeginInit();
		((Control)tbMain).SuspendLayout();
		((Control)panelName).SuspendLayout();
		((Control)panelEncoding).SuspendLayout();
		((Control)panelDelimiter).SuspendLayout();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)lbAlias).Location = new Point(12, 78);
		((Control)lbAlias).Name = "lbAlias";
		((Control)lbAlias).Size = new Size(152, 20);
		((Control)lbAlias).TabIndex = 10;
		((Control)lbAlias).Text = "Alias";
		((Label)lbAlias).TextAlign = (ContentAlignment)16;
		((Control)tbAlias).Anchor = (AnchorStyles)13;
		((Control)tbAlias).Location = new Point(169, 78);
		((Control)tbAlias).Name = "tbAlias";
		((Control)tbAlias).Size = new Size(158, 21);
		((Control)tbAlias).TabIndex = 3;
		((Control)lbName).Location = new Point(12, 45);
		((Control)lbName).Name = "lbName";
		((Control)lbName).Size = new Size(150, 20);
		((Control)lbName).TabIndex = 9;
		((Control)lbName).Text = "Name";
		((Label)lbName).TextAlign = (ContentAlignment)16;
		((Control)tbName).Anchor = (AnchorStyles)13;
		((Control)tbName).Location = new Point(169, 45);
		((Control)tbName).Name = "tbName";
		((Control)tbName).Size = new Size(158, 21);
		((Control)tbName).TabIndex = 2;
		((Control)tbName).Enter += TextBoxName_Enter;
		((Control)tbName).Leave += TextBoxName_Leave;
		((Control)labelNameInSource).Location = new Point(12, 14);
		((Control)labelNameInSource).Name = "labelNameInSource";
		((Control)labelNameInSource).Size = new Size(154, 21);
		((Control)labelNameInSource).TabIndex = 8;
		((Control)labelNameInSource).Text = "NameInSource";
		((Label)labelNameInSource).TextAlign = (ContentAlignment)16;
		errorProvider.ContainerControl = (ContainerControl)(object)this;
		errorProvider.DataMember = "";
		errorProvider.Icon = (Icon)componentResourceManager.GetObject("errorProvider.Icon");
		((Control)pnMain).Controls.Add((Control)(object)gbQuery);
		((Control)pnMain).Controls.Add((Control)(object)splMain);
		((Control)pnMain).Controls.Add((Control)(object)gbColumns);
		((Control)pnMain).Dock = (DockStyle)5;
		((Control)pnMain).Location = new Point(0, 179);
		((Control)pnMain).Name = "pnMain";
		((Control)pnMain).Size = new Size(496, 479);
		((Control)pnMain).TabIndex = 3;
		((Control)gbQuery).Controls.Add((Control)(object)pnQuery);
		((Control)gbQuery).Dock = (DockStyle)5;
		gbQuery.HeaderHeight = 24;
		((Control)gbQuery).Location = new Point(0, 0);
		((Control)gbQuery).Name = "gbQuery";
		((Control)gbQuery).Size = new Size(496, 262);
		((Control)gbQuery).TabIndex = 1;
		((Control)gbQuery).TabStop = false;
		((Control)gbQuery).Text = "Query";
		((Control)pnQuery).Anchor = (AnchorStyles)15;
		((Control)pnQuery).Controls.Add((Control)(object)tbSQL);
		((Control)pnQuery).Controls.Add((Control)(object)tbSqlCommand);
		((Control)pnQuery).Controls.Add((Control)(object)labelType);
		((Control)pnQuery).Controls.Add((Control)(object)comboBoxSqlSourceType);
		((Control)pnQuery).Controls.Add((Control)(object)labelCommandTimeout);
		((Control)pnQuery).Controls.Add((Control)(object)numericUpDownCommandTimeout);
		((Control)pnQuery).Location = new Point(0, 26);
		((Control)pnQuery).Name = "pnQuery";
		((Control)pnQuery).Size = new Size(493, 236);
		((Control)pnQuery).TabIndex = 1;
		((Control)pnQuery).Text = "Query";
		((Control)tbSQL).Controls.Add((Control)(object)tbRunQueryBuilder);
		((Control)tbSQL).Controls.Add((Control)(object)tbViewData);
		((Control)tbSQL).Controls.Add((Control)(object)tbEditSql);
		((Control)tbSQL).Controls.Add((Control)(object)tbRunScript);
		tbSQL.ControlStyle = StiControlStyle.Office2013Blue;
		((Control)tbSQL).Dock = (DockStyle)1;
		tbSQL.LineStyle = StiToolBar.StiToolBarLineStyle.Bottom;
		((Control)tbSQL).Location = new Point(0, 0);
		((Control)tbSQL).Name = "tbSQL";
		((Control)tbSQL).Padding = new Padding(6, 3, 3, 3);
		((Control)tbSQL).Size = new Size(493, 32);
		((Control)tbSQL).TabIndex = 0;
		((Control)tbRunQueryBuilder).AutoSize = true;
		((Control)tbRunQueryBuilder).Dock = (DockStyle)3;
		tbRunQueryBuilder.Draw3DButton = false;
		tbRunQueryBuilder.HotDragDrop = false;
		((Control)tbRunQueryBuilder).Location = new Point(78, 3);
		((Control)tbRunQueryBuilder).Name = "tbRunQueryBuilder";
		((Control)tbRunQueryBuilder).Size = new Size(107, 26);
		((Control)tbRunQueryBuilder).TabIndex = 1;
		((Control)tbRunQueryBuilder).Text = "Run Query Builder";
		((ButtonBase)tbRunQueryBuilder).UseVisualStyleBackColor = false;
		((Control)tbRunQueryBuilder).Click += ButtonRunQueryBuilder_Click;
		((Control)tbViewData).Dock = (DockStyle)3;
		tbViewData.Draw3DButton = false;
		tbViewData.HotDragDrop = false;
		((ButtonBase)tbViewData).ImageIndex = 12;
		((ButtonBase)tbViewData).ImageList = ilButtons;
		((Control)tbViewData).Location = new Point(54, 3);
		((Control)tbViewData).Name = "tbViewData";
		((Control)tbViewData).Size = new Size(24, 26);
		((Control)tbViewData).TabIndex = 3;
		((ButtonBase)tbViewData).UseVisualStyleBackColor = false;
		((Control)tbViewData).Click += ButtonViewData_Click;
		ilButtons.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilButtons.ImageStream");
		ilButtons.TransparentColor = Color.Magenta;
		ilButtons.Images.SetKeyName(0, "");
		ilButtons.Images.SetKeyName(1, "");
		ilButtons.Images.SetKeyName(2, "");
		ilButtons.Images.SetKeyName(3, "");
		ilButtons.Images.SetKeyName(4, "");
		ilButtons.Images.SetKeyName(5, "");
		ilButtons.Images.SetKeyName(6, "Column.png");
		ilButtons.Images.SetKeyName(7, "ColumnNew.png");
		ilButtons.Images.SetKeyName(8, "Delete.png");
		ilButtons.Images.SetKeyName(9, "");
		ilButtons.Images.SetKeyName(10, "CalcColumnNew.png");
		ilButtons.Images.SetKeyName(11, "CalcColumnDecimal.png");
		ilButtons.Images.SetKeyName(12, "ViewData.png");
		ilButtons.Images.SetKeyName(13, "ParameterNew.png");
		ilButtons.Images.SetKeyName(14, "Parameter.png");
		ilButtons.Images.SetKeyName(15, "Rotate.png");
		ilButtons.Images.SetKeyName(16, "Variable.png");
		((Control)tbEditSql).Dock = (DockStyle)3;
		tbEditSql.Draw3DButton = false;
		tbEditSql.HotDragDrop = false;
		((ButtonBase)tbEditSql).ImageIndex = 5;
		((ButtonBase)tbEditSql).ImageList = ilButtons;
		((Control)tbEditSql).Location = new Point(30, 3);
		((Control)tbEditSql).Name = "tbEditSql";
		((Control)tbEditSql).Size = new Size(24, 26);
		((Control)tbEditSql).TabIndex = 2;
		((ButtonBase)tbEditSql).UseVisualStyleBackColor = false;
		((Control)tbEditSql).Click += ButtonEditSql_Click;
		((Control)tbRunScript).Dock = (DockStyle)3;
		tbRunScript.Draw3DButton = false;
		tbRunScript.HotDragDrop = false;
		((ButtonBase)tbRunScript).ImageIndex = 4;
		((ButtonBase)tbRunScript).ImageList = ilButtons;
		((Control)tbRunScript).Location = new Point(6, 3);
		((Control)tbRunScript).Name = "tbRunScript";
		((Control)tbRunScript).Size = new Size(24, 26);
		((Control)tbRunScript).TabIndex = 1;
		((ButtonBase)tbRunScript).UseVisualStyleBackColor = false;
		((Control)tbRunScript).Click += ButtonRunScript_Click;
		tbSqlCommand.AllowOutlining = false;
		tbSqlCommand.AllowSQLFormatting = false;
		((Control)tbSqlCommand).Anchor = (AnchorStyles)15;
		((Control)tbSqlCommand).Controls.Add((Control)(object)syntaxEdit1);
		tbSqlCommand.ForceShowAutoGeneratedCode = false;
		((Control)tbSqlCommand).ForeColor = SystemColors.ControlText;
		((Control)tbSqlCommand).Location = new Point(12, 43);
		((Control)tbSqlCommand).Name = "tbSqlCommand";
		((Control)tbSqlCommand).Padding = new Padding(1);
		tbSqlCommand.ReplaceMode = false;
		tbSqlCommand.ShowAutoGeneratedCode = false;
		tbSqlCommand.ShowInsertSymbol = false;
		tbSqlCommand.ShowLineNumbersInEditor = false;
		((Control)tbSqlCommand).Size = new Size(472, 120);
		((Control)tbSqlCommand).TabIndex = 1;
		tbSqlCommand.TextFont = null;
		((Control)tbSqlCommand).Leave += TextBoxSqlCommand_Leave;
		syntaxEdit1.AcceptReturns = false;
		syntaxEdit1.AcceptTabs = false;
		((Control)syntaxEdit1).BackColor = SystemColors.Window;
		syntaxEdit1.BorderStyle = EditBorderStyle.None;
		((Control)syntaxEdit1).Cursor = Cursors.IBeam;
		syntaxEdit1.DisabledBackColor = SystemColors.Control;
		syntaxEdit1.DisabledForeColor = SystemColors.GrayText;
		((Control)syntaxEdit1).Dock = (DockStyle)5;
		((Control)syntaxEdit1).Font = new Font("Courier New", 10f);
		syntaxEdit1.Gutter.Visible = false;
		((Control)syntaxEdit1).Location = new Point(1, 1);
		((Control)syntaxEdit1).Name = "syntaxEdit1";
		((Control)syntaxEdit1).Size = new Size(470, 118);
		((Control)syntaxEdit1).TabIndex = 0;
		((Control)syntaxEdit1).Text = "";
		syntaxEdit1.UseDefaultMenu = false;
		((Control)labelType).Anchor = (AnchorStyles)6;
		((Control)labelType).Location = new Point(12, 177);
		((Control)labelType).Name = "labelType";
		((Control)labelType).Size = new Size(152, 20);
		((Control)labelType).TabIndex = 8;
		((Control)labelType).Text = "Type";
		((Label)labelType).TextAlign = (ContentAlignment)16;
		((Control)comboBoxSqlSourceType).Anchor = (AnchorStyles)6;
		comboBoxSqlSourceType.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxSqlSourceType).Location = new Point(169, 177);
		((Control)comboBoxSqlSourceType).Name = "comboBoxSqlSourceType";
		((Control)comboBoxSqlSourceType).Size = new Size(194, 21);
		((Control)comboBoxSqlSourceType).TabIndex = 2;
		((ComboBox)comboBoxSqlSourceType).SelectedIndexChanged += ComboBoxSqlSourceType_SelectedIndexChanged;
		((Control)labelCommandTimeout).Anchor = (AnchorStyles)6;
		((Control)labelCommandTimeout).Location = new Point(12, 212);
		((Control)labelCommandTimeout).Name = "labelCommandTimeout";
		((Control)labelCommandTimeout).Size = new Size(152, 20);
		((Control)labelCommandTimeout).TabIndex = 9;
		((Control)labelCommandTimeout).Text = "Timeout";
		((Label)labelCommandTimeout).TextAlign = (ContentAlignment)16;
		((Control)numericUpDownCommandTimeout).Anchor = (AnchorStyles)6;
		numericUpDownCommandTimeout.DecimalPlaces = 0;
		((Control)numericUpDownCommandTimeout).Location = new Point(169, 212);
		((Control)numericUpDownCommandTimeout).Name = "numericUpDownCommandTimeout";
		((Control)numericUpDownCommandTimeout).Size = new Size(194, 21);
		((Control)numericUpDownCommandTimeout).TabIndex = 10;
		((Control)numericUpDownCommandTimeout).Text = "30";
		((Control)splMain).Dock = (DockStyle)2;
		((Control)splMain).Location = new Point(0, 262);
		splMain.MinSize = 130;
		((Control)splMain).Name = "splMain";
		((Control)splMain).Size = new Size(496, 12);
		((Control)splMain).TabIndex = 2;
		splMain.TabStop = false;
		((Control)gbColumns).Controls.Add((Control)(object)pnColumns);
		((Control)gbColumns).Dock = (DockStyle)2;
		gbColumns.HeaderHeight = 24;
		((Control)gbColumns).Location = new Point(0, 274);
		((Control)gbColumns).Name = "gbColumns";
		((Control)gbColumns).Size = new Size(496, 205);
		((Control)gbColumns).TabIndex = 5;
		((Control)gbColumns).Text = "Columns";
		((Control)pnColumns).Anchor = (AnchorStyles)15;
		((Control)pnColumns).Controls.Add((Control)(object)ContextMenuBar);
		((Control)pnColumns).Controls.Add((Control)(object)pgDataSource);
		((Control)pnColumns).Controls.Add((Control)(object)panel2);
		((Control)pnColumns).Controls.Add((Control)(object)splitterColumns);
		((Control)pnColumns).Controls.Add((Control)(object)tvDataSource);
		((Control)pnColumns).Controls.Add((Control)(object)panel1);
		((Control)pnColumns).Controls.Add((Control)(object)panel3);
		((Control)pnColumns).Controls.Add((Control)(object)tbMain);
		((Control)pnColumns).Location = new Point(0, 28);
		((Control)pnColumns).Name = "pnColumns";
		((Control)pnColumns).Size = new Size(493, 163);
		((Control)pnColumns).TabIndex = 1;
		((Control)pnColumns).Text = "Columns";
		ContextMenuBar.DockTabStripHeight = 50;
		((Control)ContextMenuBar).Font = new Font("Segoe UI", 9f);
		ContextMenuBar.Items.AddRange(new BaseItem[2] { menuColumns, menuRetrieving });
		ContextMenuBar.Location = new Point(56, 61);
		ContextMenuBar.Name = "ContextMenuBar";
		ContextMenuBar.Size = new Size(145, 59);
		ContextMenuBar.Stretch = true;
		ContextMenuBar.Style = eDotNetBarStyle.Office2007;
		ContextMenuBar.TabIndex = 15;
		ContextMenuBar.TabStop = false;
		((Control)ContextMenuBar).Text = "ContextMenuBar";
		ContextMenuBar.WrapItemsDock = true;
		menuColumns.AutoExpandOnClick = true;
		menuColumns.ImagePaddingHorizontal = 16;
		menuColumns.ImagePaddingVertical = 12;
		menuColumns.Name = "menuColumns";
		menuColumns.Shortcuts.Add(eShortcut.CtrlTab);
		menuColumns.SubItems.AddRange(new BaseItem[4] { ciColumnNew, ciCalcColumnNew, ciParameterNew, ciDelete });
		menuColumns.Text = "menuColumns";
		ciColumnNew.ImagePaddingHorizontal = 16;
		ciColumnNew.ImagePaddingVertical = 12;
		ciColumnNew.Name = "ciColumnNew";
		ciColumnNew.Text = "Column New";
		ciColumnNew.Click += ButtonColumnNew_Click;
		ciCalcColumnNew.ImagePaddingHorizontal = 16;
		ciCalcColumnNew.ImagePaddingVertical = 12;
		ciCalcColumnNew.Name = "ciCalcColumnNew";
		ciCalcColumnNew.Text = "Calc Column New";
		ciCalcColumnNew.Click += ButtonCalcColumnNew_Click;
		ciParameterNew.Name = "ciParameterNew";
		ciParameterNew.Text = "Parameter New";
		ciParameterNew.Click += ButtonParameterNew_Click;
		ciDelete.BeginGroup = true;
		ciDelete.Name = "ciDelete";
		ciDelete.Text = "Delete";
		ciDelete.Click += ButtonDelete_Click;
		menuRetrieving.AutoExpandOnClick = true;
		menuRetrieving.Name = "menuRetrieving";
		menuRetrieving.SubItems.AddRange(new BaseItem[3] { ciRetrieveColumnsAndParameters, ciRetrieveParameters, ciRetrieveColumnsAllowRun });
		menuRetrieving.Text = "menuRetrieving";
		ciRetrieveColumnsAndParameters.Name = "ciRetrieveColumnsAndParameters";
		ciRetrieveColumnsAndParameters.Text = "RetrieveColumnsAndParameters";
		ciRetrieveColumnsAndParameters.Click += ButtonRetrieveColumns_Click;
		ciRetrieveParameters.Name = "ciRetrieveParameters";
		ciRetrieveParameters.Text = "RetrieveParameters";
		ciRetrieveParameters.Click += ButtonRetrieveParameters_Click;
		ciRetrieveColumnsAllowRun.AutoCheckOnClick = true;
		ciRetrieveColumnsAllowRun.AutoCollapseOnClick = false;
		ciRetrieveColumnsAllowRun.BeginGroup = true;
		ciRetrieveColumnsAllowRun.Name = "ciRetrieveColumnsAllowRun";
		ciRetrieveColumnsAllowRun.Text = "RetrieveColumnsAllowRun";
		ContextMenuBar.SetContextMenuEx((Control)(object)tvDataSource, menuColumns);
		((Control)tvDataSource).Dock = (DockStyle)3;
		((TreeView)tvDataSource).HideSelection = false;
		((TreeView)tvDataSource).ImageIndex = 0;
		((TreeView)tvDataSource).ImageList = ilButtons;
		((Control)tvDataSource).Location = new Point(12, 44);
		((Control)tvDataSource).Name = "tvDataSource";
		((TreeView)tvDataSource).SelectedImageIndex = 0;
		((TreeView)tvDataSource).ShowRootLines = false;
		((Control)tvDataSource).Size = new Size(229, 119);
		((Control)tvDataSource).TabIndex = 0;
		((TreeView)tvDataSource).BeforeCheck += new TreeViewCancelEventHandler(TreeViewDataSource_BeforeCheck);
		((TreeView)tvDataSource).AfterSelect += new TreeViewEventHandler(TreeViewDataSource_AfterSelect);
		((Control)tvDataSource).KeyDown += new KeyEventHandler(TreeViewDataSource_KeyDown);
		((PropertyGrid)pgDataSource).CategoryForeColor = SystemColors.InactiveCaptionText;
		((PropertyGrid)pgDataSource).CommandsForeColor = SystemColors.ActiveCaption;
		((Control)pgDataSource).Dock = (DockStyle)5;
		((PropertyGrid)pgDataSource).HelpBackColor = Color.White;
		((PropertyGrid)pgDataSource).LineColor = SystemColors.ScrollBar;
		((Control)pgDataSource).Location = new Point(253, 44);
		((Control)pgDataSource).Name = "pgDataSource";
		pgDataSource.ScrollOffset = 0;
		pgDataSource.ShowEventsTag = true;
		((Control)pgDataSource).Size = new Size(230, 119);
		((Control)pgDataSource).TabIndex = 1;
		((PropertyGrid)pgDataSource).ToolbarVisible = false;
		((PropertyGrid)pgDataSource).PropertyValueChanged += new PropertyValueChangedEventHandler(PropertyGridDataSource_PropertyValueChanged);
		((Control)panel2).Dock = (DockStyle)4;
		((Control)panel2).Location = new Point(483, 44);
		((Control)panel2).Name = "panel2";
		((Control)panel2).Size = new Size(10, 119);
		((Control)panel2).TabIndex = 13;
		((Control)splitterColumns).Location = new Point(241, 44);
		((Control)splitterColumns).Name = "splitterColumns";
		((Control)splitterColumns).Size = new Size(12, 119);
		((Control)splitterColumns).TabIndex = 10;
		splitterColumns.TabStop = false;
		((Control)panel1).Dock = (DockStyle)3;
		((Control)panel1).Location = new Point(0, 44);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(12, 119);
		((Control)panel1).TabIndex = 12;
		((Control)panel3).Dock = (DockStyle)1;
		((Control)panel3).Location = new Point(0, 32);
		((Control)panel3).Name = "panel3";
		((Control)panel3).Size = new Size(493, 12);
		((Control)panel3).TabIndex = 14;
		((Control)tbMain).Controls.Add((Control)(object)tbGetColumnsFromAssembly);
		((Control)tbMain).Controls.Add((Control)(object)tbRetrieveColumns);
		((Control)tbMain).Controls.Add((Control)(object)toolButtonSeparator);
		((Control)tbMain).Controls.Add((Control)(object)toolButtonViewData);
		((Control)tbMain).Controls.Add((Control)(object)stiToolButton1);
		((Control)tbMain).Controls.Add((Control)(object)tbDelete);
		((Control)tbMain).Controls.Add((Control)(object)tbParameterNew);
		((Control)tbMain).Controls.Add((Control)(object)tbCalcColumnNew);
		((Control)tbMain).Controls.Add((Control)(object)tbColumnNew);
		tbMain.ControlStyle = StiControlStyle.Office2013Blue;
		((Control)tbMain).Dock = (DockStyle)1;
		tbMain.LineStyle = StiToolBar.StiToolBarLineStyle.Bottom;
		((Control)tbMain).Location = new Point(0, 0);
		((Control)tbMain).Name = "tbMain";
		((Control)tbMain).Padding = new Padding(6, 3, 3, 3);
		((Control)tbMain).Size = new Size(493, 32);
		((Control)tbMain).TabIndex = 0;
		((Control)tbGetColumnsFromAssembly).AutoSize = true;
		((Control)tbGetColumnsFromAssembly).Dock = (DockStyle)3;
		tbGetColumnsFromAssembly.Draw3DButton = false;
		tbGetColumnsFromAssembly.HotDragDrop = false;
		((ButtonBase)tbGetColumnsFromAssembly).ImageList = ilButtons;
		((Control)tbGetColumnsFromAssembly).Location = new Point(267, 3);
		((Control)tbGetColumnsFromAssembly).Name = "tbGetColumnsFromAssembly";
		((Control)tbGetColumnsFromAssembly).Size = new Size(151, 26);
		((Control)tbGetColumnsFromAssembly).TabIndex = 16;
		((Control)tbGetColumnsFromAssembly).Text = "Get Columns from Assembly";
		((ButtonBase)tbGetColumnsFromAssembly).UseVisualStyleBackColor = false;
		((Control)tbGetColumnsFromAssembly).Click += ButtonGetColumnsFromAssembly_Click;
		((Control)tbRetrieveColumns).AutoSize = true;
		((Control)tbRetrieveColumns).Dock = (DockStyle)3;
		tbRetrieveColumns.Draw3DButton = false;
		tbRetrieveColumns.HotDragDrop = false;
		((ButtonBase)tbRetrieveColumns).Image = (Image)componentResourceManager.GetObject("tbRetrieveColumns.Image");
		((Control)tbRetrieveColumns).Location = new Point(147, 3);
		((Control)tbRetrieveColumns).Name = "tbRetrieveColumns";
		((Control)tbRetrieveColumns).Size = new Size(120, 26);
		((Control)tbRetrieveColumns).TabIndex = 15;
		((Control)tbRetrieveColumns).Text = "Retrieve Columns";
		((ButtonBase)tbRetrieveColumns).UseVisualStyleBackColor = false;
		((Control)tbRetrieveColumns).Click += ButtonShowRetrieveParametersMenu_Click;
		((Control)toolButtonSeparator).Dock = (DockStyle)3;
		toolButtonSeparator.Draw3DButton = false;
		toolButtonSeparator.HotDragDrop = false;
		((Control)toolButtonSeparator).Location = new Point(139, 3);
		((Control)toolButtonSeparator).Name = "toolButtonSeparator";
		((Control)toolButtonSeparator).Size = new Size(8, 26);
		toolButtonSeparator.Style = StiToolBarButtonStyle.Separator;
		((Control)toolButtonSeparator).TabIndex = 14;
		((ButtonBase)toolButtonSeparator).UseVisualStyleBackColor = false;
		((Control)toolButtonSeparator).Visible = false;
		((Control)toolButtonViewData).Dock = (DockStyle)3;
		toolButtonViewData.Draw3DButton = false;
		toolButtonViewData.HotDragDrop = false;
		((ButtonBase)toolButtonViewData).ImageIndex = 12;
		((ButtonBase)toolButtonViewData).ImageList = ilButtons;
		((Control)toolButtonViewData).Location = new Point(114, 3);
		((Control)toolButtonViewData).Name = "toolButtonViewData";
		((Control)toolButtonViewData).Size = new Size(25, 26);
		((Control)toolButtonViewData).TabIndex = 13;
		((ButtonBase)toolButtonViewData).UseVisualStyleBackColor = false;
		((Control)toolButtonViewData).Visible = false;
		((Control)toolButtonViewData).Click += ButtonViewData_Click;
		((Control)stiToolButton1).Dock = (DockStyle)3;
		stiToolButton1.Draw3DButton = false;
		stiToolButton1.HotDragDrop = false;
		((Control)stiToolButton1).Location = new Point(106, 3);
		((Control)stiToolButton1).Name = "stiToolButton1";
		((Control)stiToolButton1).Size = new Size(8, 26);
		stiToolButton1.Style = StiToolBarButtonStyle.Separator;
		((Control)stiToolButton1).TabIndex = 9;
		((ButtonBase)stiToolButton1).UseVisualStyleBackColor = false;
		((Control)tbDelete).Dock = (DockStyle)3;
		tbDelete.Draw3DButton = false;
		tbDelete.HotDragDrop = false;
		((ButtonBase)tbDelete).ImageIndex = 8;
		((ButtonBase)tbDelete).ImageList = ilButtons;
		((Control)tbDelete).Location = new Point(81, 3);
		((Control)tbDelete).Name = "tbDelete";
		((Control)tbDelete).Size = new Size(25, 26);
		((Control)tbDelete).TabIndex = 5;
		((ButtonBase)tbDelete).UseVisualStyleBackColor = false;
		((Control)tbDelete).Click += ButtonDelete_Click;
		((Control)tbParameterNew).Dock = (DockStyle)3;
		tbParameterNew.Draw3DButton = false;
		tbParameterNew.HotDragDrop = false;
		((ButtonBase)tbParameterNew).ImageIndex = 13;
		((ButtonBase)tbParameterNew).ImageList = ilButtons;
		((Control)tbParameterNew).Location = new Point(56, 3);
		((Control)tbParameterNew).Name = "tbParameterNew";
		((Control)tbParameterNew).Size = new Size(25, 26);
		((Control)tbParameterNew).TabIndex = 11;
		((ButtonBase)tbParameterNew).UseVisualStyleBackColor = false;
		((Control)tbParameterNew).Click += ButtonParameterNew_Click;
		((Control)tbCalcColumnNew).Dock = (DockStyle)3;
		tbCalcColumnNew.Draw3DButton = false;
		tbCalcColumnNew.HotDragDrop = false;
		((ButtonBase)tbCalcColumnNew).ImageIndex = 10;
		((ButtonBase)tbCalcColumnNew).ImageList = ilButtons;
		((Control)tbCalcColumnNew).Location = new Point(31, 3);
		((Control)tbCalcColumnNew).Name = "tbCalcColumnNew";
		((Control)tbCalcColumnNew).Size = new Size(25, 26);
		((Control)tbCalcColumnNew).TabIndex = 10;
		((ButtonBase)tbCalcColumnNew).UseVisualStyleBackColor = false;
		((Control)tbCalcColumnNew).Click += ButtonCalcColumnNew_Click;
		((Control)tbColumnNew).Dock = (DockStyle)3;
		tbColumnNew.Draw3DButton = false;
		tbColumnNew.HotDragDrop = false;
		((ButtonBase)tbColumnNew).ImageIndex = 7;
		((ButtonBase)tbColumnNew).ImageList = ilButtons;
		((Control)tbColumnNew).Location = new Point(6, 3);
		((Control)tbColumnNew).Name = "tbColumnNew";
		((Control)tbColumnNew).Size = new Size(25, 26);
		((Control)tbColumnNew).TabIndex = 4;
		((ButtonBase)tbColumnNew).UseVisualStyleBackColor = false;
		((Control)tbColumnNew).Click += ButtonColumnNew_Click;
		comboBoxCodePage.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)comboBoxCodePage).DropDownWidth = 250;
		((Control)comboBoxCodePage).Location = new Point(169, 0);
		((ComboBox)comboBoxCodePage).MaxDropDownItems = 20;
		((Control)comboBoxCodePage).Name = "comboBoxCodePage";
		((Control)comboBoxCodePage).Size = new Size(194, 21);
		((Control)comboBoxCodePage).TabIndex = 1;
		((Control)lbEncoding).Location = new Point(12, 0);
		((Control)lbEncoding).Name = "lbEncoding";
		((Control)lbEncoding).Size = new Size(152, 20);
		((Control)lbEncoding).TabIndex = 11;
		((Control)lbEncoding).Text = "Encoding";
		((Label)lbEncoding).TextAlign = (ContentAlignment)16;
		((Control)lbDelimiter).Location = new Point(12, 0);
		((Control)lbDelimiter).Name = "lbDelimiter";
		((Control)lbDelimiter).Size = new Size(152, 20);
		((Control)lbDelimiter).TabIndex = 12;
		((Control)lbDelimiter).Text = "Delimiter";
		((Label)lbDelimiter).TextAlign = (ContentAlignment)16;
		comboBoxDelimiter.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)comboBoxDelimiter).DropDownWidth = 250;
		((Control)comboBoxDelimiter).Location = new Point(169, 0);
		((ComboBox)comboBoxDelimiter).MaxDropDownItems = 20;
		((Control)comboBoxDelimiter).Name = "comboBoxDelimiter";
		((Control)comboBoxDelimiter).Size = new Size(194, 21);
		((Control)comboBoxDelimiter).TabIndex = 13;
		((ListControl)comboBoxDelimiter).SelectedValueChanged += ComboBoxDelimiter_SelectedValueChanged;
		((Control)textBoxCustomDelimeter).Location = new Point(369, 0);
		((Control)textBoxCustomDelimeter).Name = "textBoxCustomDelimeter";
		((Control)textBoxCustomDelimeter).Size = new Size(89, 21);
		((Control)textBoxCustomDelimeter).TabIndex = 14;
		((Control)textBoxCustomDelimeter).Visible = false;
		((Control)panelName).Controls.Add((Control)(object)buttonEditNameInSource);
		((Control)panelName).Controls.Add((Control)(object)labelNameInSource);
		((Control)panelName).Controls.Add((Control)(object)lbName);
		((Control)panelName).Controls.Add((Control)(object)lbAlias);
		((Control)panelName).Controls.Add((Control)(object)tbAlias);
		((Control)panelName).Controls.Add((Control)(object)tbName);
		((Control)panelName).Dock = (DockStyle)1;
		((Control)panelName).Location = new Point(0, 0);
		((Control)panelName).Name = "panelName";
		((Control)panelName).Size = new Size(496, 111);
		((Control)panelName).TabIndex = 16;
		buttonEditNameInSource.AllowEscape = false;
		((Control)buttonEditNameInSource).Anchor = (AnchorStyles)13;
		buttonEditNameInSource.ButtonIconKind = StiButtonIconKind.Dots;
		buttonEditNameInSource.Flat = false;
		((Control)buttonEditNameInSource).Location = new Point(169, 12);
		((Control)buttonEditNameInSource).Name = "buttonEditNameInSource";
		((Control)buttonEditNameInSource).Size = new Size(315, 21);
		((Control)buttonEditNameInSource).TabIndex = 0;
		buttonEditNameInSource.ButtonClick += ButtonEditNameInSource_ButtonClick;
		((Control)panelEncoding).Controls.Add((Control)(object)comboBoxCodePage);
		((Control)panelEncoding).Controls.Add((Control)(object)lbEncoding);
		((Control)panelEncoding).Dock = (DockStyle)1;
		((Control)panelEncoding).Location = new Point(0, 111);
		((Control)panelEncoding).Name = "panelEncoding";
		((Control)panelEncoding).Size = new Size(496, 34);
		((Control)panelEncoding).TabIndex = 17;
		((Control)panelEncoding).Visible = false;
		((Control)panelDelimiter).Controls.Add((Control)(object)lbDelimiter);
		((Control)panelDelimiter).Controls.Add((Control)(object)comboBoxDelimiter);
		((Control)panelDelimiter).Controls.Add((Control)(object)textBoxCustomDelimeter);
		((Control)panelDelimiter).Dock = (DockStyle)1;
		((Control)panelDelimiter).Location = new Point(0, 145);
		((Control)panelDelimiter).Name = "panelDelimiter";
		((Control)panelDelimiter).Size = new Size(496, 34);
		((Control)panelDelimiter).TabIndex = 18;
		((Control)panelDelimiter).Visible = false;
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(328, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 23);
		((Control)btOk).TabIndex = 5;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(409, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 23);
		((Control)btCancel).TabIndex = 6;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += This_CancelClick;
		((Control)stiGroupLine1).Controls.Add((Control)(object)btSaveCopy);
		((Control)stiGroupLine1).Controls.Add((Control)(object)btCancel);
		((Control)stiGroupLine1).Controls.Add((Control)(object)btOk);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 658);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(496, 46);
		((Control)stiGroupLine1).TabIndex = 15;
		((Control)btSaveCopy).Anchor = (AnchorStyles)6;
		((Control)btSaveCopy).Location = new Point(12, 12);
		((Control)btSaveCopy).Name = "btSaveCopy";
		((Control)btSaveCopy).Size = new Size(93, 23);
		((Control)btSaveCopy).TabIndex = 4;
		((Control)btSaveCopy).Text = "Save a Copy";
		((ButtonBase)btSaveCopy).UseVisualStyleBackColor = true;
		((Control)btSaveCopy).Visible = false;
		((Control)btSaveCopy).Click += This_SaveCopyClick;
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.F1);
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.CtrlC);
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.CtrlA);
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.CtrlV);
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.CtrlX);
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.CtrlZ);
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.CtrlY);
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.Del);
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.Ins);
		DotNetBarManager.EnableFullSizeDock = false;
		DotNetBarManager.ParentForm = null;
		DotNetBarManager.Style = eDotNetBarStyle.StyleManagerControlled;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(496, 704);
		((Control)this).Controls.Add((Control)(object)pnMain);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Control)this).Controls.Add((Control)(object)panelDelimiter);
		((Control)this).Controls.Add((Control)(object)panelEncoding);
		((Control)this).Controls.Add((Control)(object)panelName);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(512, 650);
		((Control)this).Name = "StiDataStoreSourceEditForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Edit Data Source";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Closing += This_Closing;
		((Form)this).Closed += This_Closed;
		((Form)this).Load += This_Load;
		((ISupportInitialize)errorProvider).EndInit();
		((Control)pnMain).ResumeLayout(false);
		((Control)gbQuery).ResumeLayout(false);
		((Control)pnQuery).ResumeLayout(false);
		((Control)tbSQL).ResumeLayout(false);
		((Control)tbSQL).PerformLayout();
		((Control)tbSqlCommand).ResumeLayout(false);
		((Control)tbSqlCommand).PerformLayout();
		((ISupportInitialize)numericUpDownCommandTimeout).EndInit();
		((Control)gbColumns).ResumeLayout(false);
		((Control)pnColumns).ResumeLayout(false);
		((ISupportInitialize)ContextMenuBar).EndInit();
		((Control)tbMain).ResumeLayout(false);
		((Control)tbMain).PerformLayout();
		((Control)panelName).ResumeLayout(false);
		((Control)panelEncoding).ResumeLayout(false);
		((Control)panelDelimiter).ResumeLayout(false);
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
