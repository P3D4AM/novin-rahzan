using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AccSqlServerPersistence;
using AccSqlServerPersistence.AssistanceClasses;
using DevComponents.DotNetBar.Controls;
using FormCustom.Properties;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace FormCustom.BackUp.Forms.SqlQuery;

public class frmShowQueries : AccountingBaseForm
{
	private string _connectionString = "";

	private ENSource _source;

	private bool _isDisposed;

	private IContainer components;

	private BindingSource sqlQueryBindingSource;

	internal DataGridViewX DGrid;

	internal Label LBLSearch;

	internal TextBox TXTSearch;

	internal Button BTNUpdate;

	internal Button BTNInsert;

	internal Button BTNDelete;

	internal Button btnExecuteSelected;

	internal Button btnExecuteAll;

	private ContextMenuStrip contextMenuStrip1;

	private ToolStripMenuItem انتخابToolStripMenuItem;

	private ToolStripMenuItem mnuSelectAll;

	private ToolStripMenuItem mnuSelectNone;

	private ToolStripMenuItem mnuReverse;

	private DataGridViewCheckBoxColumn dgChecked;

	private DataGridViewTextBoxColumn Radif;

	private DataGridViewTextBoxColumn dgTitle;

	private DataGridViewTextBoxColumn dgGuid;

	private DataGridViewTextBoxColumn dgQuery;

	public new bool IsDisposed
	{
		get
		{
			if (!base.IsDisposed)
			{
				return _isDisposed;
			}
			return true;
		}
	}

	private async Task LoadDataAsync(string search = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			List<AccSqlServerPersistence.AssistanceClasses.SqlQuery> list = await AccSqlServerPersistence.AssistanceClasses.SqlQuery.GetAllAsync(_connectionString, _source, search);
			foreach (string item in search.SplitString())
			{
				list = list.Where((AccSqlServerPersistence.AssistanceClasses.SqlQuery q) => q.Title.Contains(item)).ToList();
			}
			sqlQueryBindingSource.DataSource = list.ToSortableBindingList(token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmShowQueries.cs", 34);
		}
	}

	public frmShowQueries(string contextGuid, string connectionString, ENSource source)
		: base(contextGuid)
	{
		InitializeComponent();
		_connectionString = connectionString;
		_source = source;
	}

	private async void frmShowQueries_Load(object sender, EventArgs e)
	{
		await LoadDataAsync();
	}

	private async void BTNInsert_Click(object sender, EventArgs e)
	{
		try
		{
			frmSqlQuery obj = new frmSqlQuery(base.ContextGuid, _source, _connectionString);
			((Form)(object)this).CheckVisibility("BTNInsert_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmShowQueries.cs");
			if ((int)((Form)obj).ShowDialog((IWin32Window)(object)this) == 1)
			{
				await LoadDataAsync();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTNInsert_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmShowQueries.cs", 54);
		}
	}

	private async void BTNUpdate_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if (((DataGridView)DGrid).RowCount > 0 && ((DataGridView)DGrid).CurrentRow != null)
			{
				Guid guid = (Guid)((DataGridView)DGrid)[((DataGridViewBand)dgGuid).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value;
				AccSqlServerPersistence.AssistanceClasses.SqlQuery query = await AccSqlServerPersistence.AssistanceClasses.SqlQuery.GetAsync(BackupSettingsRepo.GetInstance(base.ContextGuid), guid, _connectionString, _source);
				frmSqlQuery obj = new frmSqlQuery(base.ContextGuid, query, _source, _connectionString);
				((Form)(object)this).CheckVisibility("BTNUpdate_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmShowQueries.cs");
				if ((int)((Form)obj).ShowDialog((IWin32Window)(object)this) == 1)
				{
					await LoadDataAsync();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTNUpdate_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmShowQueries.cs", 69);
		}
	}

	private void frmShowQueries_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode <= 45)
			{
				if ((int)keyCode != 27)
				{
					if ((int)keyCode == 45)
					{
						BTNInsert.PerformClick();
					}
				}
				else
				{
					((Form)this).Close();
				}
			}
			else if ((int)keyCode != 46)
			{
				if ((int)keyCode == 118)
				{
					BTNUpdate.PerformClick();
				}
			}
			else
			{
				BTNDelete.PerformClick();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "frmShowQueries_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmShowQueries.cs", 86);
		}
	}

	private async void DGrid_CellClick(object sender, DataGridViewCellEventArgs e)
	{
		try
		{
			if (((DataGridView)DGrid).RowCount < 0)
			{
				return;
			}
			if (e.ColumnIndex == ((DataGridViewBand)dgChecked).Index)
			{
				if (((DataGridView)DGrid).CurrentRow == null)
				{
					return;
				}
				((DataGridView)DGrid)[((DataGridViewBand)dgChecked).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value = !(bool)((DataGridView)DGrid)[((DataGridViewBand)dgChecked).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value;
				(await AccSqlServerPersistence.AssistanceClasses.SqlQuery.GetAsync(BackupSettingsRepo.GetInstance(base.ContextGuid), (Guid)((DataGridView)DGrid)[((DataGridViewBand)dgGuid).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value, _connectionString, _source)).IsChecked = (bool)((DataGridView)DGrid)[((DataGridViewBand)dgChecked).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value;
			}
			HighLightGrid();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DGrid_CellClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmShowQueries.cs", 109);
		}
	}

	private void HighLightGrid()
	{
		try
		{
			for (int i = 0; i < ((DataGridView)DGrid).RowCount; i++)
			{
				if (((bool?)((DataGridView)DGrid)[((DataGridViewBand)dgChecked).Index, i].Value).GetValueOrDefault())
				{
					((DataGridViewBand)((DataGridView)DGrid).Rows[i]).DefaultCellStyle.BackColor = Color.Plum;
				}
				else if (((DataGridViewBand)((DataGridView)DGrid).Rows[i]).DefaultCellStyle.BackColor != Color.White)
				{
					((DataGridViewBand)((DataGridView)DGrid).Rows[i]).DefaultCellStyle.BackColor = Color.White;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "HighLightGrid", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmShowQueries.cs", 129);
		}
	}

	private void mnuSelectAll_Click(object sender, EventArgs e)
	{
		try
		{
			if (((DataGridView)DGrid).RowCount != 0)
			{
				for (int i = 0; i <= ((DataGridView)DGrid).RowCount - 1; i++)
				{
					((DataGridView)DGrid)[((DataGridViewBand)dgChecked).Index, i].Value = true;
					((AccSqlServerPersistence.AssistanceClasses.SqlQuery)sqlQueryBindingSource.Current).IsChecked = (bool)((DataGridView)DGrid)[((DataGridViewBand)dgChecked).Index, i].Value;
				}
				HighLightGrid();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "mnuSelectAll_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmShowQueries.cs", 148);
		}
	}

	private void mnuSelectNone_Click(object sender, EventArgs e)
	{
		try
		{
			if (((DataGridView)DGrid).RowCount != 0)
			{
				for (int i = 0; i <= ((DataGridView)DGrid).RowCount - 1; i++)
				{
					((DataGridView)DGrid)[((DataGridViewBand)dgChecked).Index, i].Value = false;
					((AccSqlServerPersistence.AssistanceClasses.SqlQuery)sqlQueryBindingSource.Current).IsChecked = (bool)((DataGridView)DGrid)[((DataGridViewBand)dgChecked).Index, i].Value;
				}
				HighLightGrid();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "mnuSelectNone_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmShowQueries.cs", 166);
		}
	}

	private void mnuReverse_Click(object sender, EventArgs e)
	{
		try
		{
			if (((DataGridView)DGrid).RowCount != 0)
			{
				for (int i = 0; i <= ((DataGridView)DGrid).RowCount - 1; i++)
				{
					((DataGridView)DGrid)[((DataGridViewBand)dgChecked).Index, i].Value = !(bool)((DataGridView)DGrid)[((DataGridViewBand)dgChecked).Index, i].Value;
					((AccSqlServerPersistence.AssistanceClasses.SqlQuery)sqlQueryBindingSource.Current).IsChecked = (bool)((DataGridView)DGrid)[((DataGridViewBand)dgChecked).Index, i].Value;
				}
				HighLightGrid();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "mnuReverse_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmShowQueries.cs", 184);
		}
	}

	private void DGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
	{
		((DataGridView)DGrid).Rows[e.RowIndex].Cells["Radif"].Value = e.RowIndex + 1;
	}

	private async void BTNDelete_Click(object sender, EventArgs e)
	{
		_ = 3;
		try
		{
			if (((DataGridView)DGrid).RowCount <= 0 || ((DataGridView)DGrid).CurrentRow == null || (int)MessageBox.Show($"آیا از حذف دستور {((DataGridView)DGrid)[((DataGridViewBand)dgTitle).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value} اطمینان دارید؟", "حذف دستور SQL", (MessageBoxButtons)4, (MessageBoxIcon)32) == 7)
			{
				return;
			}
			Guid guid = (Guid)((DataGridView)DGrid)[((DataGridViewBand)dgGuid).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value;
			AccSqlServerPersistence.AssistanceClasses.SqlQuery sqlQuery = await AccSqlServerPersistence.AssistanceClasses.SqlQuery.GetAsync(BackupSettingsRepo.GetInstance(base.ContextGuid), guid, _connectionString, _source);
			if (sqlQuery != null)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = await sqlQuery.DeleteAsync(_connectionString);
				if (returnedSaveFuncInfo.HasError)
				{
					await this.ShowReturnedSavedFuncInfoAsync(returnedSaveFuncInfo, "خطا در حذف SQLQuery", "BTNDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmShowQueries.cs");
				}
				await LoadDataAsync();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTNDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmShowQueries.cs", 214);
		}
	}

	private async void btnExecuteSelected_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if (((DataGridView)DGrid).RowCount == 0)
			{
				return;
			}
			List<string> list = new List<string>();
			for (int i = 0; i <= ((DataGridView)DGrid).RowCount - 1; i++)
			{
				if ((bool)((DataGridView)DGrid)[((DataGridViewBand)dgChecked).Index, i].Value)
				{
					list.Add(((DataGridView)DGrid)[((DataGridViewBand)dgQuery).Index, i].Value.ToString());
				}
			}
			if (list.Count <= 0 && ((DataGridView)DGrid).CurrentRow != null)
			{
				list.Add(((DataGridView)DGrid)[((DataGridViewBand)dgQuery).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value.ToString());
			}
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			foreach (string item in list)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await RunScript.Instance.RunAsync(BackupSettingsRepo.GetInstance(base.ContextGuid), "customQuery_frmShowQuery", checkCrcBeforeExecution: false, _source, item, _connectionString), ReturnedState.None, "btnExecuteSelected_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmShowQueries.cs", 234);
			}
			if (res.HasError)
			{
				await this.ShowReturnedSavedFuncInfoAsync(res, "پیغام سیستم", "btnExecuteSelected_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmShowQueries.cs");
			}
			else
			{
				MessageBox.Show($"{list.Count}" + " " + "دستور با موفقیت اجرا شد".GetTranslate());
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnExecuteSelected_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmShowQueries.cs", 246);
		}
	}

	private async void btnExecuteAll_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			if (((DataGridView)DGrid).RowCount != 0)
			{
				ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
				for (int i = 0; i <= ((DataGridView)DGrid).RowCount - 1; i++)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await RunScript.Instance.RunAsync(BackupSettingsRepo.GetInstance(base.ContextGuid), "customQuery_frmShowQuery", checkCrcBeforeExecution: false, _source, ((DataGridView)DGrid)[((DataGridViewBand)dgQuery).Index, i].Value.ToString(), _connectionString), ReturnedState.None, "btnExecuteAll_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmShowQueries.cs", 256);
				}
				if (res.HasError)
				{
					await this.ShowReturnedSavedFuncInfoAsync(res, "پیغام سیستم", "btnExecuteAll_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmShowQueries.cs");
				}
				MessageBox.Show((IWin32Window)(object)this, $"{((DataGridView)DGrid).RowCount}" + " " + " دستور با موفقیت اجرا شد".GetTranslate());
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnExecuteAll_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmShowQueries.cs", 264);
		}
	}

	private async void TXTSearch_TextChanged(object sender, EventArgs e)
	{
		try
		{
			await LoadDataAsync(((Control)TXTSearch).Text);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TXTSearch_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmShowQueries.cs", 274);
		}
	}

	private void frmShowQueries_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose();
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				components?.Dispose();
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\frmShowQueries.cs", 300);
		}
	}

	private void InitializeComponent()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected O, but got Unknown
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Expected O, but got Unknown
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Expected O, but got Unknown
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Expected O, but got Unknown
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Expected O, but got Unknown
		//IL_0ae2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aec: Expected O, but got Unknown
		//IL_0b06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b10: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DGrid = new DataGridViewX();
		dgChecked = new DataGridViewCheckBoxColumn();
		Radif = new DataGridViewTextBoxColumn();
		dgTitle = new DataGridViewTextBoxColumn();
		dgGuid = new DataGridViewTextBoxColumn();
		dgQuery = new DataGridViewTextBoxColumn();
		contextMenuStrip1 = new ContextMenuStrip(components);
		انتخابToolStripMenuItem = new ToolStripMenuItem();
		mnuSelectAll = new ToolStripMenuItem();
		mnuSelectNone = new ToolStripMenuItem();
		mnuReverse = new ToolStripMenuItem();
		sqlQueryBindingSource = new BindingSource(components);
		LBLSearch = new Label();
		TXTSearch = new TextBox();
		BTNUpdate = new Button();
		BTNInsert = new Button();
		BTNDelete = new Button();
		btnExecuteSelected = new Button();
		btnExecuteAll = new Button();
		((ISupportInitialize)DGrid).BeginInit();
		((Control)contextMenuStrip1).SuspendLayout();
		((ISupportInitialize)sqlQueryBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToOrderColumns = true;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[5]
		{
			(DataGridViewColumn)dgChecked,
			(DataGridViewColumn)Radif,
			(DataGridViewColumn)dgTitle,
			(DataGridViewColumn)dgGuid,
			(DataGridViewColumn)dgQuery
		});
		((Control)DGrid).ContextMenuStrip = contextMenuStrip1;
		((DataGridView)DGrid).DataSource = sqlQueryBindingSource;
		val.Alignment = (DataGridViewContentAlignment)16;
		val.BackColor = SystemColors.Window;
		val.Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		val.ForeColor = SystemColors.ControlText;
		val.SelectionBackColor = SystemColors.Highlight;
		val.SelectionForeColor = SystemColors.ControlText;
		val.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(110, 32);
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(662, 525);
		((Control)DGrid).TabIndex = 31;
		((DataGridView)DGrid).CellClick += new DataGridViewCellEventHandler(DGrid_CellClick);
		((DataGridView)DGrid).CellFormatting += new DataGridViewCellFormattingEventHandler(DGrid_CellFormatting);
		((DataGridViewColumn)dgChecked).DataPropertyName = "IsChecked";
		((DataGridViewColumn)dgChecked).HeaderText = "";
		((DataGridViewColumn)dgChecked).Name = "dgChecked";
		((DataGridViewBand)dgChecked).ReadOnly = true;
		((DataGridViewColumn)dgChecked).Width = 30;
		val2.NullValue = null;
		((DataGridViewBand)Radif).DefaultCellStyle = val2;
		((DataGridViewColumn)Radif).HeaderText = "[ردیف]";
		((DataGridViewColumn)Radif).Name = "Radif";
		((DataGridViewBand)Radif).ReadOnly = true;
		((DataGridViewColumn)Radif).Width = 50;
		((DataGridViewColumn)dgTitle).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)dgTitle).DataPropertyName = "Title";
		((DataGridViewColumn)dgTitle).HeaderText = "[عنوان]";
		((DataGridViewColumn)dgTitle).Name = "dgTitle";
		((DataGridViewBand)dgTitle).ReadOnly = true;
		((DataGridViewColumn)dgGuid).DataPropertyName = "Guid";
		((DataGridViewColumn)dgGuid).HeaderText = "Guid";
		((DataGridViewColumn)dgGuid).Name = "dgGuid";
		((DataGridViewBand)dgGuid).ReadOnly = true;
		((DataGridViewBand)dgGuid).Visible = false;
		((DataGridViewColumn)dgQuery).DataPropertyName = "Query";
		((DataGridViewColumn)dgQuery).HeaderText = "Query";
		((DataGridViewColumn)dgQuery).Name = "dgQuery";
		((DataGridViewBand)dgQuery).ReadOnly = true;
		((DataGridViewBand)dgQuery).Visible = false;
		((ToolStrip)contextMenuStrip1).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[1] { (ToolStripItem)انتخابToolStripMenuItem });
		((Control)contextMenuStrip1).Name = "contextMenuStrip1";
		((Control)contextMenuStrip1).Size = new Size(107, 26);
		((ToolStripDropDownItem)انتخابToolStripMenuItem).DropDownItems.AddRange((ToolStripItem[])(object)new ToolStripItem[3]
		{
			(ToolStripItem)mnuSelectAll,
			(ToolStripItem)mnuSelectNone,
			(ToolStripItem)mnuReverse
		});
		((ToolStripItem)انتخابToolStripMenuItem).Name = "انتخابToolStripMenuItem";
		((ToolStripItem)انتخابToolStripMenuItem).Size = new Size(106, 22);
		((ToolStripItem)انتخابToolStripMenuItem).Text = "[انتخاب]";
		((ToolStripItem)mnuSelectAll).Name = "mnuSelectAll";
		((ToolStripItem)mnuSelectAll).Size = new Size(176, 22);
		((ToolStripItem)mnuSelectAll).Text = "[انتخاب همه]";
		((ToolStripItem)mnuSelectAll).Click += mnuSelectAll_Click;
		((ToolStripItem)mnuSelectNone).Name = "mnuSelectNone";
		((ToolStripItem)mnuSelectNone).Size = new Size(176, 22);
		((ToolStripItem)mnuSelectNone).Text = "[انتخاب هیچکدام]";
		((ToolStripItem)mnuSelectNone).Click += mnuSelectNone_Click;
		((ToolStripItem)mnuReverse).Name = "mnuReverse";
		((ToolStripItem)mnuReverse).Size = new Size(176, 22);
		((ToolStripItem)mnuReverse).Text = "[معکوس کردن انتخاب]";
		((ToolStripItem)mnuReverse).Click += mnuReverse_Click;
		sqlQueryBindingSource.DataSource = typeof(AccSqlServerPersistence.AssistanceClasses.SqlQuery);
		((Control)LBLSearch).Anchor = (AnchorStyles)9;
		((Control)LBLSearch).AutoSize = true;
		((Control)LBLSearch).Location = new Point(726, 9);
		((Control)LBLSearch).Name = "LBLSearch";
		((Control)LBLSearch).Size = new Size(46, 13);
		((Control)LBLSearch).TabIndex = 40;
		((Control)LBLSearch).Text = "[جستجو]:";
		((Control)TXTSearch).Anchor = (AnchorStyles)13;
		((Control)TXTSearch).Location = new Point(110, 6);
		((Control)TXTSearch).Name = "TXTSearch";
		((Control)TXTSearch).Size = new Size(610, 20);
		((Control)TXTSearch).TabIndex = 39;
		((Control)TXTSearch).TextChanged += TXTSearch_TextChanged;
		((ButtonBase)BTNUpdate).Image = (Image)(object)FormCustom.Properties.Resources.edit2;
		((ButtonBase)BTNUpdate).ImageAlign = (ContentAlignment)64;
		((Control)BTNUpdate).Location = new Point(10, 76);
		((Control)BTNUpdate).Name = "BTNUpdate";
		((Control)BTNUpdate).Size = new Size(92, 38);
		((Control)BTNUpdate).TabIndex = 34;
		((Control)BTNUpdate).TabStop = false;
		((Control)BTNUpdate).Text = "[ویرایش]";
		((ButtonBase)BTNUpdate).UseVisualStyleBackColor = true;
		((Control)BTNUpdate).Click += BTNUpdate_Click;
		((ButtonBase)BTNInsert).Image = (Image)(object)FormCustom.Properties.Resources.edit_add1;
		((ButtonBase)BTNInsert).ImageAlign = (ContentAlignment)64;
		((Control)BTNInsert).Location = new Point(10, 32);
		((Control)BTNInsert).Name = "BTNInsert";
		((Control)BTNInsert).Size = new Size(92, 38);
		((Control)BTNInsert).TabIndex = 33;
		((Control)BTNInsert).TabStop = false;
		((Control)BTNInsert).Text = "[اضافه]";
		((ButtonBase)BTNInsert).UseVisualStyleBackColor = true;
		((Control)BTNInsert).Click += BTNInsert_Click;
		((ButtonBase)BTNDelete).Image = (Image)(object)FormCustom.Properties.Resources.edit_delete;
		((ButtonBase)BTNDelete).ImageAlign = (ContentAlignment)64;
		((Control)BTNDelete).Location = new Point(10, 120);
		((Control)BTNDelete).Name = "BTNDelete";
		((Control)BTNDelete).Size = new Size(92, 38);
		((Control)BTNDelete).TabIndex = 32;
		((Control)BTNDelete).TabStop = false;
		((Control)BTNDelete).Text = "[حذف]";
		((ButtonBase)BTNDelete).UseVisualStyleBackColor = true;
		((Control)BTNDelete).Click += BTNDelete_Click;
		((ButtonBase)btnExecuteSelected).ImageAlign = (ContentAlignment)64;
		((Control)btnExecuteSelected).Location = new Point(10, 164);
		((Control)btnExecuteSelected).Name = "btnExecuteSelected";
		((Control)btnExecuteSelected).Size = new Size(92, 72);
		((Control)btnExecuteSelected).TabIndex = 32;
		((Control)btnExecuteSelected).TabStop = false;
		((Control)btnExecuteSelected).Text = "[اجرای دستور(های) انتخاب شده]";
		((ButtonBase)btnExecuteSelected).UseVisualStyleBackColor = true;
		((Control)btnExecuteSelected).Click += btnExecuteSelected_Click;
		((ButtonBase)btnExecuteAll).ImageAlign = (ContentAlignment)64;
		((Control)btnExecuteAll).Location = new Point(10, 242);
		((Control)btnExecuteAll).Name = "btnExecuteAll";
		((Control)btnExecuteAll).Size = new Size(92, 48);
		((Control)btnExecuteAll).TabIndex = 32;
		((Control)btnExecuteAll).TabStop = false;
		((Control)btnExecuteAll).Text = "[اجرای همه دستورات]";
		((ButtonBase)btnExecuteAll).UseVisualStyleBackColor = true;
		((Control)btnExecuteAll).Click += btnExecuteAll_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(784, 561);
		((Control)this).Controls.Add((Control)(object)DGrid);
		((Control)this).Controls.Add((Control)(object)LBLSearch);
		((Control)this).Controls.Add((Control)(object)TXTSearch);
		((Control)this).Controls.Add((Control)(object)BTNUpdate);
		((Control)this).Controls.Add((Control)(object)BTNInsert);
		((Control)this).Controls.Add((Control)(object)btnExecuteAll);
		((Control)this).Controls.Add((Control)(object)btnExecuteSelected);
		((Control)this).Controls.Add((Control)(object)BTNDelete);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "frmShowQueries";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Form)this).FormClosed += new FormClosedEventHandler(frmShowQueries_FormClosed);
		((Form)this).Load += frmShowQueries_Load;
		((Control)this).KeyDown += new KeyEventHandler(frmShowQueries_KeyDown);
		((ISupportInitialize)DGrid).EndInit();
		((Control)contextMenuStrip1).ResumeLayout(false);
		((ISupportInitialize)sqlQueryBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
