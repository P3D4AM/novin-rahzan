using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Controls.Win.DotNetBar.Controls;
using Stimulsoft.Editor;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Design.Dictionary;

public class StiViewDataForm : StiFormUX
{
	private string storedSqlCommand;

	private bool isDataSourceConnected;

	private bool isFormClosing;

	private StiDataSource dataSource;

	private StiBusinessObject businessObject;

	private object gridDataSource;

	private string gridDataMember;

	private StiSqlSource sqlSource;

	private IContainer components;

	private DataGridView dataGrid;

	private StiPanelUX pnSQL;

	private StiOffice2013GroupBox groupBoxSqlCommand;

	private StiToolBar toolBarSql;

	private StiToolButton tbRunScript;

	private StiCodeEditor codeEditorSqlCommand;

	private SyntaxEdit syntaxEdit1;

	private SyntaxEdit syntaxEdit2;

	private ToolTip toolTip1;

	private StiToolButton tbEditSql;

	private ImageList ilButtons;

	private StiPanelUX panel1;

	private CircularProgress progressView;

	private BackgroundWorker workerView;

	private StiLabelUX labelErrorMessage;

	private StiOffice2013GroupBox groupBoxData;

	private Timer timerView;

	private StiLabelUX labelDataNotFound;

	internal bool IsCancelSqlExpressionsForm { get; set; }

	public StiViewDataForm(StiBusinessObject businessObject)
		: this(null, businessObject)
	{
	}

	public StiViewDataForm(StiDataSource dataSource)
		: this(dataSource, null)
	{
	}

	public StiViewDataForm(StiDataSource dataSource, StiBusinessObject businessObject)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			DoTheming();
			DoScaling();
			Localize();
			((Control)this).Text = ((dataSource != null) ? (dataSource.Name + " - " + ((Control)this).Text) : (businessObject.Name + " - " + ((Control)this).Text));
			StiSettings.Load();
			StiFormSettings.Load((Form)(object)this);
			if (dataSource != null)
			{
				codeEditorSqlCommand.Init(dataSource.Dictionary.Report, StiCodeEditorMode.EditorSQL);
			}
			this.dataSource = dataSource;
			this.businessObject = businessObject;
			sqlSource = dataSource as StiSqlSource;
			if (sqlSource != null)
			{
				((Control)codeEditorSqlCommand).Text = sqlSource.SqlCommand;
			}
			else
			{
				((Control)pnSQL).Visible = false;
			}
			dataGrid.ReadOnly = true;
			((Form)this).ShowIcon = true;
			if (StiOptions.Designer.DesignerIcon != null)
			{
				((Form)this).Icon = StiOptions.Designer.DesignerIcon;
			}
			else if (!((Component)(object)this).DesignMode)
			{
				((Form)this).Icon = StiAppIcons.GetIcon();
			}
			((Control)dataGrid).Visible = false;
			((Control)toolBarSql).Visible = false;
			((Control)groupBoxSqlCommand).Visible = false;
			((Control)groupBoxData).Visible = false;
			((Control)labelDataNotFound).Visible = false;
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormTitles", "ViewDataForm");
		((Control)groupBoxSqlCommand).Text = StiLocalization.Get("FormDictionaryDesigner", "QueryText");
		((Control)groupBoxData).Text = StiLocalization.Get("PropertyMain", "Data");
		((Control)labelDataNotFound).Text = Loc.Get("Errors", "DataNotFound");
		toolTip1.SetToolTip((Control)(object)tbRunScript, StiLocalization.Get("FormDictionaryDesigner", "Run"));
		toolTip1.SetToolTip((Control)(object)tbEditSql, StiLocalization.Get("FormDictionaryDesigner", "EditQuery"));
	}

	private void DoTheming()
	{
		if (StiUX.IsDark)
		{
			((Control)labelErrorMessage).BackColor = StiUX.ErrorBackground;
			((Control)labelErrorMessage).ForeColor = StiUX.ErrorForeground;
			dataGrid.BackgroundColor = StiUX.ItemBackground;
			((Control)dataGrid).ForeColor = StiUX.ItemForeground;
		}
	}

	private void DoScaling()
	{
		toolBarSql.ControlStyle = StiControlStyle.Office2013Blue;
		((ButtonBase)tbRunScript).Image = StiReportDesignImages.Actions.RunQuery();
		((ButtonBase)tbEditSql).Image = StiReportDesignImages.Actions.ViewData();
		dataGrid.ColumnHeadersHeight = StiScale.I(26);
	}

	private void FillLevel(StiBusinessObject businessObject, DataTable tempTable)
	{
		businessObject.Connect();
		businessObject.First();
		while (!businessObject.IsEof)
		{
			DataRow dataRow = tempTable.NewRow();
			foreach (StiDataColumn column in businessObject.Columns)
			{
				try
				{
					dataRow[column.Name] = businessObject[column.Name] ?? DBNull.Value;
				}
				catch
				{
				}
			}
			tempTable.Rows.Add(dataRow);
			businessObject.Next();
		}
	}

	private void FillLevel(int index, List<StiBusinessObject> list, StiBusinessObject businessObject, DataTable tempTable)
	{
		if (index >= list.Count)
		{
			FillLevel(businessObject, tempTable);
			return;
		}
		StiBusinessObject stiBusinessObject = list[index];
		stiBusinessObject.Connect();
		stiBusinessObject.First();
		while (!stiBusinessObject.IsEof)
		{
			FillLevel(index + 1, list, businessObject, tempTable);
			stiBusinessObject.Next();
		}
	}

	private void ConnectDataSourceFirstTime()
	{
		if (dataSource != null && !isDataSourceConnected)
		{
			dataSource.Dictionary.Connect(loadData: false);
			isDataSourceConnected = true;
		}
	}

	private void Build()
	{
		ConnectDataSourceFirstTime();
		if (businessObject != null)
		{
			BuildBusinessObject();
		}
		else
		{
			BuildDataSource();
		}
	}

	private void BuildBusinessObject()
	{
		DataTable dataTable = new DataTable
		{
			TableName = businessObject.Name
		};
		foreach (StiDataColumn column2 in businessObject.Columns)
		{
			DataColumn column = ((Nullable.GetUnderlyingType(column2.Type) != null) ? new DataColumn(column2.Name, Nullable.GetUnderlyingType(column2.Type)) : new DataColumn(column2.Name, column2.Type));
			dataTable.Columns.Add(column);
		}
		StiBusinessObject parentBusinessObject = businessObject.ParentBusinessObject;
		List<StiBusinessObject> list = new List<StiBusinessObject>();
		while (parentBusinessObject != null)
		{
			list.Insert(0, parentBusinessObject);
			parentBusinessObject = parentBusinessObject.ParentBusinessObject;
		}
		FillLevel(0, list, businessObject, dataTable);
		gridDataSource = dataTable;
	}

	private void BuildDataSource()
	{
		if (dataSource is StiDataTableSource)
		{
			foreach (StiDatabase database in dataSource.Dictionary.Databases)
			{
				if ((database is StiXmlDatabase || database is StiCsvDatabase) && ((StiDataTableSource)dataSource).NameInSource != null && ((StiDataTableSource)dataSource).NameInSource.IndexOfInvariant(database.Name) != -1)
				{
					StiDataLeader.RegData(database, dataSource.Dictionary, loadData: true);
					StiDataLeader.Connect(dataSource);
				}
			}
		}
		if (dataSource is StiDataTableSource stiDataTableSource && !(stiDataTableSource is StiSqlSource))
		{
			if (dataSource.IsConnected)
			{
				if (stiDataTableSource.DataTable.TableName.IndexOf(".", StringComparison.Ordinal) != -1)
				{
					gridDataSource = stiDataTableSource.DataTable.DataSet.Tables[stiDataTableSource.DataTable.TableName];
					return;
				}
				gridDataSource = stiDataTableSource.DataTable.DataSet;
				gridDataMember = stiDataTableSource.DataTable.TableName;
			}
			return;
		}
		try
		{
			if (dataSource is StiVirtualSource)
			{
				dataSource.Dictionary.ConnectVirtualDataSources();
			}
			else if (dataSource is StiDataTransformation)
			{
				dataSource.Dictionary.ConnectDataTransformations();
			}
			else
			{
				StiDataLeader.Connect(dataSource);
			}
			DataTable dataTable = dataSource.GetDataTable();
			DataSet dataSet = new DataSet();
			dataSet.Tables.Add(dataTable);
			if (dataTable.TableName.IndexOf(".", StringComparison.Ordinal) != -1)
			{
				gridDataSource = dataSet.Tables[dataTable.TableName];
			}
			else
			{
				gridDataSource = dataSet;
				gridDataMember = dataTable.TableName;
			}
			StiDataLeader.Connect(dataSource, loadData: false);
		}
		catch (Exception ex)
		{
			ShowErrorMessage(ex.Message);
		}
	}

	private void RestoreSqlCommand()
	{
		if (sqlSource != null)
		{
			sqlSource.SqlCommand = storedSqlCommand;
		}
	}

	private void SaveSqlCommand()
	{
		if (sqlSource != null)
		{
			storedSqlCommand = sqlSource.SqlCommand;
			sqlSource.SqlCommand = ((Control)codeEditorSqlCommand).Text;
		}
	}

	private DialogResult RequestParameters(StiSqlSource source)
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			List<string> exps = (source.AllowExpressions ? StiCodeDomExpressionHelper.GetLexem(source.SqlCommand) : new List<string>());
			DialogResult result = (DialogResult)1;
			StiSqlExpressionsForm stiSqlExpressionsForm = new StiSqlExpressionsForm(source, exps, source.Dictionary.Report);
			try
			{
				if (stiSqlExpressionsForm.CheckResult)
				{
					result = ((Form)stiSqlExpressionsForm).ShowDialog((IWin32Window)(object)this);
					source.SqlCommand = stiSqlExpressionsForm.Value;
				}
			}
			finally
			{
				((IDisposable)stiSqlExpressionsForm)?.Dispose();
			}
			return result;
		}
		catch
		{
			return (DialogResult)1;
		}
	}

	private void ShowErrorMessage(string message)
	{
		((Control)labelErrorMessage).Invoke((Delegate)(Action)delegate
		{
			((Control)labelErrorMessage).Text = message;
			((Control)labelErrorMessage).Visible = true;
			RecalculateViewLocation();
		});
	}

	private void HideErrorMessage()
	{
		((Control)labelErrorMessage).Invoke((Delegate)(Action)delegate
		{
			((Control)labelErrorMessage).Visible = false;
			RecalculateViewLocation();
		});
	}

	private void RecalculateViewLocation()
	{
		if (((Control)labelErrorMessage).Visible)
		{
			((Control)dataGrid).Height = ((Control)panel1).Height - StiScale.YYI(24.0) - ((Control)labelErrorMessage).Height - ((Control)groupBoxData).Bottom;
		}
		else
		{
			((Control)dataGrid).Height = ((Control)panel1).Height - StiScale.YYI(24.0) - ((Control)groupBoxData).Bottom;
		}
	}

	private void RefreshView()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		SaveSqlCommand();
		DialogResult val = (DialogResult)((sqlSource == null) ? 1 : ((int)RequestParameters(sqlSource)));
		if ((int)val == 1)
		{
			This_SizeChanged(null, null);
			progressView.Value = 0;
			progressView.IsRunning = true;
			((Control)progressView).Visible = true;
			((Control)dataGrid).Visible = false;
			((Control)toolBarSql).Visible = false;
			((Control)groupBoxSqlCommand).Visible = false;
			((Control)groupBoxData).Visible = false;
			workerView.RunWorkerAsync();
		}
		else
		{
			RestoreSqlCommand();
			IsCancelSqlExpressionsForm = true;
		}
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

	private void This_FormClosing(object sender, FormClosingEventArgs e)
	{
		isFormClosing = true;
	}

	private void This_FormClosed(object sender, FormClosedEventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
	}

	private void This_Load(object sender, EventArgs e)
	{
		RecalculateViewLocation();
		((Control)progressView).Parent.Controls.Remove((Control)(object)progressView);
		((Control)this).Controls.Add((Control)(object)progressView);
		((Control)progressView).BringToFront();
		((Control)labelDataNotFound).Parent.Controls.Remove((Control)(object)labelDataNotFound);
		((Control)this).Controls.Add((Control)(object)labelDataNotFound);
		((Control)labelDataNotFound).BringToFront();
		if (sqlSource != null)
		{
			timerView.Start();
		}
		else
		{
			RefreshView();
		}
	}

	private void This_SizeChanged(object sender, EventArgs e)
	{
		((Control)progressView).Left = (((Form)this).ClientSize.Width - ((Control)progressView).Width) / 2;
		((Control)progressView).Top = (((Form)this).ClientSize.Height - ((Control)progressView).Height) / 2;
		((Control)labelDataNotFound).Left = ((Control)groupBoxData).Left + (((Control)groupBoxData).Width - ((Control)labelDataNotFound).Width) / 2;
		((Control)labelDataNotFound).Top = ((Control)panel1).Top + (((Control)panel1).Height - ((Control)labelDataNotFound).Height) / 2;
		((Control)dataGrid).Invalidate();
		RecalculateViewLocation();
	}

	private void ButtonRunScript_Click(object sender, EventArgs e)
	{
		RefreshView();
	}

	private void ButtonEditSql_Click(object sender, EventArgs e)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Invalid comparison between Unknown and I4
		string text = ((Control)codeEditorSqlCommand).Text;
		DialogResult val = StiTextEditor.EditText(dataSource.Dictionary.Report, ref text, StiCodeEditorMode.EditorSQL);
		if ((int)val == 1)
		{
			((Control)codeEditorSqlCommand).Text = text;
		}
	}

	private void DataGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
	{
		((CancelEventArgs)(object)e).Cancel = true;
	}

	private void WorkerView_DoWork(object sender, DoWorkEventArgs e)
	{
		HideErrorMessage();
		try
		{
			Build();
		}
		catch (Exception ex)
		{
			ShowErrorMessage(ex.Message);
		}
	}

	private void WorkerView_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		if (isFormClosing)
		{
			return;
		}
		try
		{
			RestoreSqlCommand();
			dataGrid.DataSource = null;
			dataGrid.DataMember = null;
			progressView.IsRunning = false;
			((Control)progressView).Visible = false;
			dataGrid.DataSource = gridDataSource;
			dataGrid.DataMember = gridDataMember;
			foreach (DataGridViewColumn item in (BaseCollection)dataGrid.Columns)
			{
				DataGridViewColumn val = item;
				val.Width = StiScale.I(val.Width);
			}
			((Control)dataGrid).Visible = gridDataSource != null;
			((Control)dataGrid).Visible = true;
			((Control)toolBarSql).Visible = true;
			((Control)groupBoxSqlCommand).Visible = true;
			((Control)labelDataNotFound).Visible = gridDataSource == null;
			((Control)groupBoxData).Visible = true;
		}
		catch
		{
		}
	}

	private void TimerView_Tick(object sender, EventArgs e)
	{
		timerView.Stop();
		RefreshView();
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
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected O, but got Unknown
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Expected O, but got Unknown
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0529: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05de: Unknown result type (might be due to invalid IL or missing references)
		//IL_064c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0656: Expected O, but got Unknown
		//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0785: Unknown result type (might be due to invalid IL or missing references)
		//IL_078f: Expected O, but got Unknown
		//IL_081d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0827: Expected O, but got Unknown
		//IL_091c: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3b: Expected O, but got Unknown
		//IL_0a6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a91: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d28: Expected O, but got Unknown
		//IL_0d30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3a: Expected O, but got Unknown
		//IL_0d66: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d70: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiViewDataForm));
		dataGrid = new DataGridView();
		pnSQL = new StiPanelUX();
		groupBoxSqlCommand = new StiOffice2013GroupBox();
		codeEditorSqlCommand = new StiCodeEditor();
		toolBarSql = new StiToolBar();
		tbEditSql = new StiToolButton();
		ilButtons = new ImageList(components);
		tbRunScript = new StiToolButton();
		syntaxEdit1 = new SyntaxEdit(components);
		syntaxEdit2 = new SyntaxEdit(components);
		toolTip1 = new ToolTip(components);
		panel1 = new StiPanelUX();
		groupBoxData = new StiOffice2013GroupBox();
		labelErrorMessage = new StiLabelUX();
		progressView = new CircularProgress();
		workerView = new BackgroundWorker();
		timerView = new Timer(components);
		labelDataNotFound = new StiLabelUX();
		((ISupportInitialize)dataGrid).BeginInit();
		((Control)pnSQL).SuspendLayout();
		((Control)groupBoxSqlCommand).SuspendLayout();
		((Control)toolBarSql).SuspendLayout();
		((Control)panel1).SuspendLayout();
		((Control)this).SuspendLayout();
		dataGrid.AllowUserToAddRows = false;
		dataGrid.AllowUserToDeleteRows = false;
		((Control)dataGrid).Anchor = (AnchorStyles)15;
		dataGrid.BackgroundColor = Color.White;
		dataGrid.BorderStyle = (BorderStyle)0;
		val.Alignment = (DataGridViewContentAlignment)16;
		val.BackColor = SystemColors.Control;
		val.Font = new Font("Microsoft Sans Serif", 7.875f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		val.ForeColor = SystemColors.WindowText;
		val.SelectionBackColor = SystemColors.Highlight;
		val.SelectionForeColor = SystemColors.HighlightText;
		val.WrapMode = (DataGridViewTriState)1;
		dataGrid.ColumnHeadersDefaultCellStyle = val;
		val2.Alignment = (DataGridViewContentAlignment)16;
		val2.BackColor = SystemColors.Window;
		val2.Font = new Font("Microsoft Sans Serif", 7.875f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		val2.ForeColor = SystemColors.ControlText;
		val2.SelectionBackColor = SystemColors.Highlight;
		val2.SelectionForeColor = SystemColors.HighlightText;
		val2.WrapMode = (DataGridViewTriState)2;
		dataGrid.DefaultCellStyle = val2;
		((Control)dataGrid).Location = new Point(24, 96);
		((Control)dataGrid).Margin = new Padding(6);
		((Control)dataGrid).Name = "dataGrid";
		((Control)dataGrid).Size = new Size(928, 477);
		((Control)dataGrid).TabIndex = 0;
		dataGrid.DataError += new DataGridViewDataErrorEventHandler(DataGrid_DataError);
		((Control)pnSQL).Controls.Add((Control)(object)groupBoxSqlCommand);
		((Control)pnSQL).Controls.Add((Control)(object)toolBarSql);
		((Control)pnSQL).Dock = (DockStyle)1;
		((Control)pnSQL).Location = new Point(0, 0);
		((Control)pnSQL).Margin = new Padding(6);
		((Control)pnSQL).Name = "pnSQL";
		((Control)pnSQL).Size = new Size(976, 298);
		((Control)pnSQL).TabIndex = 3;
		((Control)groupBoxSqlCommand).Anchor = (AnchorStyles)15;
		((Control)groupBoxSqlCommand).Controls.Add((Control)(object)codeEditorSqlCommand);
		groupBoxSqlCommand.HeaderHeight = 24;
		((Control)groupBoxSqlCommand).Location = new Point(24, 85);
		((Control)groupBoxSqlCommand).Name = "groupBoxSqlCommand";
		((Control)groupBoxSqlCommand).Padding = new Padding(0, 48, 0, 0);
		((Control)groupBoxSqlCommand).Size = new Size(928, 212);
		((Control)groupBoxSqlCommand).TabIndex = 0;
		((Control)groupBoxSqlCommand).TabStop = false;
		((Control)groupBoxSqlCommand).Text = "SQL";
		codeEditorSqlCommand.AllowOutlining = false;
		((Control)codeEditorSqlCommand).Anchor = (AnchorStyles)15;
		((Control)codeEditorSqlCommand).BackColor = Color.White;
		codeEditorSqlCommand.ForceShowAutoGeneratedCode = false;
		((Control)codeEditorSqlCommand).ForeColor = SystemColors.ControlText;
		((Control)codeEditorSqlCommand).Location = new Point(0, 71);
		((Control)codeEditorSqlCommand).Name = "codeEditorSqlCommand";
		((Control)codeEditorSqlCommand).Padding = new Padding(1);
		codeEditorSqlCommand.ReplaceMode = false;
		codeEditorSqlCommand.ShowAutoGeneratedCode = false;
		codeEditorSqlCommand.ShowLineNumbersInEditor = false;
		((Control)codeEditorSqlCommand).Size = new Size(928, 140);
		((Control)codeEditorSqlCommand).TabIndex = 0;
		((Control)toolBarSql).Controls.Add((Control)(object)tbEditSql);
		((Control)toolBarSql).Controls.Add((Control)(object)tbRunScript);
		toolBarSql.ControlStyle = StiControlStyle.Office2013Blue;
		((Control)toolBarSql).Dock = (DockStyle)1;
		toolBarSql.LineStyle = StiToolBar.StiToolBarLineStyle.Bottom;
		((Control)toolBarSql).Location = new Point(0, 0);
		((Control)toolBarSql).Margin = new Padding(6);
		((Control)toolBarSql).Name = "toolBarSql";
		((Control)toolBarSql).Padding = new Padding(8);
		((Control)toolBarSql).Size = new Size(976, 62);
		((Control)toolBarSql).TabIndex = 1;
		((Control)tbEditSql).Dock = (DockStyle)3;
		tbEditSql.Draw3DButton = false;
		tbEditSql.DropDownMenu = null;
		tbEditSql.HotDragDrop = false;
		((ButtonBase)tbEditSql).ImageIndex = 1;
		((ButtonBase)tbEditSql).ImageList = ilButtons;
		((Control)tbEditSql).Location = new Point(56, 8);
		((Control)tbEditSql).Margin = new Padding(6);
		((Control)tbEditSql).Name = "tbEditSql";
		((Control)tbEditSql).Size = new Size(48, 46);
		((Control)tbEditSql).TabIndex = 1;
		((ButtonBase)tbEditSql).UseVisualStyleBackColor = false;
		((Control)tbEditSql).Click += ButtonEditSql_Click;
		ilButtons.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilButtons.ImageStream");
		ilButtons.TransparentColor = Color.Magenta;
		ilButtons.Images.SetKeyName(0, "");
		ilButtons.Images.SetKeyName(1, "");
		((Control)tbRunScript).Dock = (DockStyle)3;
		tbRunScript.Draw3DButton = false;
		tbRunScript.DropDownMenu = null;
		tbRunScript.HotDragDrop = false;
		((ButtonBase)tbRunScript).ImageIndex = 0;
		((ButtonBase)tbRunScript).ImageList = ilButtons;
		((Control)tbRunScript).Location = new Point(8, 8);
		((Control)tbRunScript).Margin = new Padding(6);
		((Control)tbRunScript).Name = "tbRunScript";
		((Control)tbRunScript).Size = new Size(48, 46);
		((Control)tbRunScript).TabIndex = 0;
		((ButtonBase)tbRunScript).UseVisualStyleBackColor = false;
		((Control)tbRunScript).Click += ButtonRunScript_Click;
		((Control)syntaxEdit1).BackColor = SystemColors.Window;
		((Control)syntaxEdit1).Cursor = Cursors.IBeam;
		((Control)syntaxEdit1).Font = new Font("Courier New", 10f);
		((Control)syntaxEdit1).Location = new Point(0, 0);
		((Control)syntaxEdit1).Name = "syntaxEdit1";
		((Control)syntaxEdit1).Size = new Size(100, 96);
		((Control)syntaxEdit1).TabIndex = 0;
		((Control)syntaxEdit1).Text = "";
		syntaxEdit1.UseDefaultMenu = false;
		((Control)syntaxEdit2).BackColor = SystemColors.Window;
		((Control)syntaxEdit2).Cursor = Cursors.IBeam;
		((Control)syntaxEdit2).Font = new Font("Courier New", 10f);
		((Control)syntaxEdit2).Location = new Point(0, 0);
		((Control)syntaxEdit2).Name = "syntaxEdit2";
		((Control)syntaxEdit2).Size = new Size(100, 96);
		((Control)syntaxEdit2).TabIndex = 0;
		((Control)syntaxEdit2).Text = "";
		syntaxEdit2.UseDefaultMenu = false;
		((Control)panel1).Controls.Add((Control)(object)labelDataNotFound);
		((Control)panel1).Controls.Add((Control)(object)groupBoxData);
		((Control)panel1).Controls.Add((Control)(object)labelErrorMessage);
		((Control)panel1).Controls.Add((Control)(object)progressView);
		((Control)panel1).Controls.Add((Control)(object)dataGrid);
		((Control)panel1).Dock = (DockStyle)5;
		((Control)panel1).Location = new Point(0, 298);
		((Control)panel1).Margin = new Padding(6);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(976, 604);
		((Control)panel1).TabIndex = 6;
		((Control)groupBoxData).Anchor = (AnchorStyles)13;
		groupBoxData.HeaderHeight = 24;
		((Control)groupBoxData).Location = new Point(24, 24);
		((Control)groupBoxData).Name = "groupBoxData";
		((Control)groupBoxData).Padding = new Padding(0, 48, 0, 0);
		((Control)groupBoxData).Size = new Size(928, 48);
		((Control)groupBoxData).TabIndex = 24;
		((Control)groupBoxData).TabStop = false;
		((Control)groupBoxData).Text = "Data";
		((Control)labelErrorMessage).BackColor = Color.FromArgb(252, 220, 220);
		((Control)labelErrorMessage).Dock = (DockStyle)2;
		((Control)labelErrorMessage).Font = new Font("Microsoft Sans Serif", 7.875f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)labelErrorMessage).ForeColor = Color.Red;
		((Control)labelErrorMessage).Location = new Point(0, 487);
		((Control)labelErrorMessage).Margin = new Padding(4, 0, 4, 0);
		((Control)labelErrorMessage).Name = "labelErrorMessage";
		((Control)labelErrorMessage).Padding = new Padding(24, 4, 24, 4);
		((Control)labelErrorMessage).Size = new Size(976, 117);
		((Control)labelErrorMessage).TabIndex = 23;
		((Control)labelErrorMessage).Text = "Error message";
		((Label)labelErrorMessage).TextAlign = (ContentAlignment)16;
		((Control)labelErrorMessage).Visible = false;
		((Control)progressView).BackColor = Color.White;
		progressView.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)progressView).Location = new Point(448, 221);
		((Control)progressView).Margin = new Padding(6);
		((Control)progressView).Name = "progressView";
		progressView.ProgressBarType = eCircularProgressType.Donut;
		progressView.ProgressColor = Color.SteelBlue;
		((Control)progressView).Size = new Size(104, 104);
		progressView.Style = eDotNetBarStyle.OfficeXP;
		((Control)progressView).TabIndex = 7;
		((Control)progressView).TabStop = false;
		((Control)progressView).Visible = false;
		workerView.WorkerSupportsCancellation = true;
		workerView.DoWork += WorkerView_DoWork;
		workerView.RunWorkerCompleted += WorkerView_RunWorkerCompleted;
		timerView.Interval = 500;
		timerView.Tick += TimerView_Tick;
		((Control)labelDataNotFound).AutoSize = true;
		((Control)labelDataNotFound).Location = new Point(430, 261);
		((Control)labelDataNotFound).Name = "labelDataNotFound";
		((Control)labelDataNotFound).Size = new Size(151, 25);
		((Control)labelDataNotFound).TabIndex = 25;
		((Control)labelDataNotFound).Text = "DataNotFound";
		((Label)labelDataNotFound).TextAlign = (ContentAlignment)32;
		((Control)labelDataNotFound).Visible = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(976, 902);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Controls.Add((Control)(object)pnSQL);
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(6);
		((Control)this).Name = "StiViewDataForm";
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiViewDataForm";
		((Form)this).FormClosing += new FormClosingEventHandler(This_FormClosing);
		((Form)this).FormClosed += new FormClosedEventHandler(This_FormClosed);
		((Form)this).Load += This_Load;
		((Control)this).SizeChanged += This_SizeChanged;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((ISupportInitialize)dataGrid).EndInit();
		((Control)pnSQL).ResumeLayout(false);
		((Control)groupBoxSqlCommand).ResumeLayout(false);
		((Control)toolBarSql).ResumeLayout(false);
		((Control)panel1).ResumeLayout(false);
		((Control)panel1).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
