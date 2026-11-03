using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AccSqlServerPersistence.AssistanceClasses;
using DevComponents.DotNetBar.Controls;
using PacketParser.Attendance;
using PacketParser.Entities;

namespace FormCustom.BackUp.Forms;

public class frmBackUpLog : AccountingBaseForm
{
	private bool _isDisposed;

	private string _connectionString;

	private IContainer components;

	internal DataGridViewX DGrid;

	internal Label LBLSearch;

	internal TextBox TXTSearch;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

	private BindingSource backUpLogBindingSource;

	private DataGridViewTextBoxColumn Radif;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;

	private DataGridViewTextBoxColumn timeDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;

	private DataGridViewTextBoxColumn dgGuid;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;

	private DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;

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

	public frmBackUpLog(string connectionString, IBackUpLog_GetUserName userInterface, string contextGuid)
		: base(contextGuid)
	{
		InitializeComponent();
		_connectionString = connectionString;
		SqlBackUpLogMaster.Init(userInterface);
	}

	private void frmBackUpLog_Load(object sender, EventArgs e)
	{
		Task.Run(() => LoadDataAsync(base.ContextGuid));
	}

	private void TXTSearch_TextChanged(object sender, EventArgs e)
	{
		Task.Run(() => LoadDataAsync(((Control)TXTSearch).Text));
	}

	private void frmBackUpLog_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode != 13)
			{
				if ((int)keyCode == 27)
				{
					((Form)this).Close();
				}
			}
			else if (((DataGridView)DGrid).RowCount > 0 && ((DataGridView)DGrid).CurrentRow != null)
			{
				frmBackUpLogSlaves obj = new frmBackUpLogSlaves((Guid)((DataGridView)DGrid)[((DataGridViewBand)dgGuid).Index, ((DataGridViewBand)((DataGridView)DGrid).CurrentRow).Index].Value, _connectionString, base.ContextGuid);
				((Form)(object)this).CheckVisibility("frmBackUpLog_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmBackUpLog.cs");
				((Form)obj).ShowDialog((IWin32Window)(object)this);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "frmBackUpLog_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmBackUpLog.cs", 42);
		}
	}

	private void DGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
	{
		((DataGridView)DGrid).Rows[e.RowIndex].Cells["Radif"].Value = e.RowIndex + 1;
	}

	private async Task LoadDataAsync(string search, CancellationToken token = default(CancellationToken))
	{
		try
		{
			List<SqlBackUpLogMaster> list = await SqlBackUpLogMaster.GetAllAsync(base.ContextGuid, _connectionString, search);
			((Control)this).Invoke((Delegate)(MethodInvoker)delegate
			{
				backUpLogBindingSource.DataSource = list.ToSortableBindingList(token);
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmBackUpLog.cs", 54);
		}
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmBackUpLog.cs", 71);
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
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Expected O, but got Unknown
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Expected O, but got Unknown
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Expected O, but got Unknown
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Expected O, but got Unknown
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Expected O, but got Unknown
		//IL_09fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a07: Expected O, but got Unknown
		//IL_0a5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a67: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DGrid = new DataGridViewX();
		Radif = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
		timeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn9 = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn12 = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn10 = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn17 = new DataGridViewTextBoxColumn();
		dgGuid = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn8 = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn11 = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn13 = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn14 = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn15 = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn16 = new DataGridViewTextBoxColumn();
		backUpLogBindingSource = new BindingSource(components);
		LBLSearch = new Label();
		TXTSearch = new TextBox();
		dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
		dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
		((ISupportInitialize)DGrid).BeginInit();
		((ISupportInitialize)backUpLogBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToOrderColumns = true;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		((Control)DGrid).Anchor = (AnchorStyles)15;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[15]
		{
			(DataGridViewColumn)Radif,
			(DataGridViewColumn)dataGridViewTextBoxColumn7,
			(DataGridViewColumn)timeDataGridViewTextBoxColumn,
			(DataGridViewColumn)dataGridViewTextBoxColumn9,
			(DataGridViewColumn)dataGridViewTextBoxColumn12,
			(DataGridViewColumn)dataGridViewTextBoxColumn10,
			(DataGridViewColumn)dataGridViewTextBoxColumn17,
			(DataGridViewColumn)dgGuid,
			(DataGridViewColumn)dataGridViewTextBoxColumn6,
			(DataGridViewColumn)dataGridViewTextBoxColumn8,
			(DataGridViewColumn)dataGridViewTextBoxColumn11,
			(DataGridViewColumn)dataGridViewTextBoxColumn13,
			(DataGridViewColumn)dataGridViewTextBoxColumn14,
			(DataGridViewColumn)dataGridViewTextBoxColumn15,
			(DataGridViewColumn)dataGridViewTextBoxColumn16
		});
		((DataGridView)DGrid).DataSource = backUpLogBindingSource;
		val.Alignment = (DataGridViewContentAlignment)16;
		val.BackColor = SystemColors.Window;
		val.Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		val.ForeColor = SystemColors.ControlText;
		val.SelectionBackColor = SystemColors.Highlight;
		val.SelectionForeColor = SystemColors.ControlText;
		val.WrapMode = (DataGridViewTriState)2;
		((DataGridView)DGrid).DefaultCellStyle = val;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(3, 35);
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(779, 525);
		((Control)DGrid).TabIndex = 41;
		((DataGridView)DGrid).CellFormatting += new DataGridViewCellFormattingEventHandler(DGrid_CellFormatting);
		((DataGridViewColumn)Radif).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		val2.NullValue = null;
		((DataGridViewBand)Radif).DefaultCellStyle = val2;
		((DataGridViewColumn)Radif).HeaderText = "[ردیف]";
		((DataGridViewColumn)Radif).Name = "Radif";
		((DataGridViewBand)Radif).ReadOnly = true;
		((DataGridViewColumn)Radif).Width = 54;
		((DataGridViewColumn)dataGridViewTextBoxColumn7).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)dataGridViewTextBoxColumn7).DataPropertyName = "DateSh";
		((DataGridViewColumn)dataGridViewTextBoxColumn7).HeaderText = "[تاریخ]";
		((DataGridViewColumn)dataGridViewTextBoxColumn7).Name = "dataGridViewTextBoxColumn7";
		((DataGridViewBand)dataGridViewTextBoxColumn7).ReadOnly = true;
		((DataGridViewColumn)dataGridViewTextBoxColumn7).Width = 53;
		((DataGridViewColumn)timeDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)timeDataGridViewTextBoxColumn).DataPropertyName = "Time";
		((DataGridViewColumn)timeDataGridViewTextBoxColumn).HeaderText = "[زمان]";
		((DataGridViewColumn)timeDataGridViewTextBoxColumn).Name = "timeDataGridViewTextBoxColumn";
		((DataGridViewBand)timeDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)timeDataGridViewTextBoxColumn).Width = 52;
		((DataGridViewColumn)dataGridViewTextBoxColumn9).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)dataGridViewTextBoxColumn9).DataPropertyName = "UserName";
		((DataGridViewColumn)dataGridViewTextBoxColumn9).HeaderText = "[کاربر]";
		((DataGridViewColumn)dataGridViewTextBoxColumn9).Name = "dataGridViewTextBoxColumn9";
		((DataGridViewBand)dataGridViewTextBoxColumn9).ReadOnly = true;
		((DataGridViewColumn)dataGridViewTextBoxColumn9).Width = 52;
		((DataGridViewColumn)dataGridViewTextBoxColumn12).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)dataGridViewTextBoxColumn12).DataPropertyName = "StatusName";
		((DataGridViewColumn)dataGridViewTextBoxColumn12).HeaderText = "[وضعیت]";
		((DataGridViewColumn)dataGridViewTextBoxColumn12).Name = "dataGridViewTextBoxColumn12";
		((DataGridViewBand)dataGridViewTextBoxColumn12).ReadOnly = true;
		((DataGridViewColumn)dataGridViewTextBoxColumn12).Width = 66;
		((DataGridViewColumn)dataGridViewTextBoxColumn10).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)dataGridViewTextBoxColumn10).DataPropertyName = "Path";
		((DataGridViewColumn)dataGridViewTextBoxColumn10).HeaderText = "[آدرس فایل]";
		((DataGridViewColumn)dataGridViewTextBoxColumn10).Name = "dataGridViewTextBoxColumn10";
		((DataGridViewBand)dataGridViewTextBoxColumn10).ReadOnly = true;
		((DataGridViewColumn)dataGridViewTextBoxColumn10).Width = 79;
		((DataGridViewColumn)dataGridViewTextBoxColumn17).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)dataGridViewTextBoxColumn17).DataPropertyName = "Description";
		((DataGridViewColumn)dataGridViewTextBoxColumn17).HeaderText = "[توضیحات]";
		((DataGridViewColumn)dataGridViewTextBoxColumn17).Name = "dataGridViewTextBoxColumn17";
		((DataGridViewBand)dataGridViewTextBoxColumn17).ReadOnly = true;
		((DataGridViewColumn)dgGuid).DataPropertyName = "Guid";
		((DataGridViewColumn)dgGuid).HeaderText = "Guid";
		((DataGridViewColumn)dgGuid).Name = "dgGuid";
		((DataGridViewBand)dgGuid).ReadOnly = true;
		((DataGridViewBand)dgGuid).Visible = false;
		((DataGridViewColumn)dataGridViewTextBoxColumn6).DataPropertyName = "DateM";
		((DataGridViewColumn)dataGridViewTextBoxColumn6).HeaderText = "DateM";
		((DataGridViewColumn)dataGridViewTextBoxColumn6).Name = "dataGridViewTextBoxColumn6";
		((DataGridViewBand)dataGridViewTextBoxColumn6).ReadOnly = true;
		((DataGridViewBand)dataGridViewTextBoxColumn6).Visible = false;
		((DataGridViewColumn)dataGridViewTextBoxColumn8).DataPropertyName = "UserAccGuid";
		((DataGridViewColumn)dataGridViewTextBoxColumn8).HeaderText = "UserAccGuid";
		((DataGridViewColumn)dataGridViewTextBoxColumn8).Name = "dataGridViewTextBoxColumn8";
		((DataGridViewBand)dataGridViewTextBoxColumn8).ReadOnly = true;
		((DataGridViewBand)dataGridViewTextBoxColumn8).Visible = false;
		((DataGridViewColumn)dataGridViewTextBoxColumn11).DataPropertyName = "Status";
		((DataGridViewColumn)dataGridViewTextBoxColumn11).HeaderText = "Status";
		((DataGridViewColumn)dataGridViewTextBoxColumn11).Name = "dataGridViewTextBoxColumn11";
		((DataGridViewBand)dataGridViewTextBoxColumn11).ReadOnly = true;
		((DataGridViewBand)dataGridViewTextBoxColumn11).Visible = false;
		((DataGridViewColumn)dataGridViewTextBoxColumn13).DataPropertyName = "Source";
		((DataGridViewColumn)dataGridViewTextBoxColumn13).HeaderText = "Source";
		((DataGridViewColumn)dataGridViewTextBoxColumn13).Name = "dataGridViewTextBoxColumn13";
		((DataGridViewBand)dataGridViewTextBoxColumn13).ReadOnly = true;
		((DataGridViewBand)dataGridViewTextBoxColumn13).Visible = false;
		((DataGridViewColumn)dataGridViewTextBoxColumn14).DataPropertyName = "SourceName";
		((DataGridViewColumn)dataGridViewTextBoxColumn14).HeaderText = "SourceName";
		((DataGridViewColumn)dataGridViewTextBoxColumn14).Name = "dataGridViewTextBoxColumn14";
		((DataGridViewBand)dataGridViewTextBoxColumn14).ReadOnly = true;
		((DataGridViewBand)dataGridViewTextBoxColumn14).Visible = false;
		((DataGridViewColumn)dataGridViewTextBoxColumn15).DataPropertyName = "Type";
		((DataGridViewColumn)dataGridViewTextBoxColumn15).HeaderText = "Type";
		((DataGridViewColumn)dataGridViewTextBoxColumn15).Name = "dataGridViewTextBoxColumn15";
		((DataGridViewBand)dataGridViewTextBoxColumn15).ReadOnly = true;
		((DataGridViewBand)dataGridViewTextBoxColumn15).Visible = false;
		((DataGridViewColumn)dataGridViewTextBoxColumn16).DataPropertyName = "TypeName";
		((DataGridViewColumn)dataGridViewTextBoxColumn16).HeaderText = "TypeName";
		((DataGridViewColumn)dataGridViewTextBoxColumn16).Name = "dataGridViewTextBoxColumn16";
		((DataGridViewBand)dataGridViewTextBoxColumn16).ReadOnly = true;
		((DataGridViewBand)dataGridViewTextBoxColumn16).Visible = false;
		backUpLogBindingSource.DataSource = typeof(SqlBackUpLogMaster);
		((Control)LBLSearch).Anchor = (AnchorStyles)9;
		((Control)LBLSearch).AutoSize = true;
		((Control)LBLSearch).Location = new Point(736, 12);
		((Control)LBLSearch).Name = "LBLSearch";
		((Control)LBLSearch).Size = new Size(49, 13);
		((Control)LBLSearch).TabIndex = 43;
		((Control)LBLSearch).Text = "[جستجو]:";
		((Control)TXTSearch).Anchor = (AnchorStyles)13;
		((Control)TXTSearch).Location = new Point(3, 9);
		((Control)TXTSearch).Name = "TXTSearch";
		((Control)TXTSearch).Size = new Size(727, 21);
		((Control)TXTSearch).TabIndex = 42;
		((Control)TXTSearch).TextChanged += TXTSearch_TextChanged;
		((DataGridViewColumn)dataGridViewTextBoxColumn1).DataPropertyName = "Status";
		((DataGridViewColumn)dataGridViewTextBoxColumn1).HeaderText = "Status";
		((DataGridViewColumn)dataGridViewTextBoxColumn1).Name = "dataGridViewTextBoxColumn1";
		((DataGridViewBand)dataGridViewTextBoxColumn1).ReadOnly = true;
		((DataGridViewBand)dataGridViewTextBoxColumn1).Visible = false;
		((DataGridViewColumn)dataGridViewTextBoxColumn2).DataPropertyName = "Source";
		((DataGridViewColumn)dataGridViewTextBoxColumn2).HeaderText = "Source";
		((DataGridViewColumn)dataGridViewTextBoxColumn2).Name = "dataGridViewTextBoxColumn2";
		((DataGridViewBand)dataGridViewTextBoxColumn2).ReadOnly = true;
		((DataGridViewBand)dataGridViewTextBoxColumn2).Visible = false;
		((DataGridViewColumn)dataGridViewTextBoxColumn3).DataPropertyName = "Type";
		((DataGridViewColumn)dataGridViewTextBoxColumn3).HeaderText = "Type";
		((DataGridViewColumn)dataGridViewTextBoxColumn3).Name = "dataGridViewTextBoxColumn3";
		((DataGridViewBand)dataGridViewTextBoxColumn3).ReadOnly = true;
		((DataGridViewBand)dataGridViewTextBoxColumn3).Visible = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(784, 561);
		((Control)this).Controls.Add((Control)(object)DGrid);
		((Control)this).Controls.Add((Control)(object)LBLSearch);
		((Control)this).Controls.Add((Control)(object)TXTSearch);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Form)this).KeyPreview = true;
		((Control)this).MinimumSize = new Size(800, 600);
		((Control)this).Name = "frmBackUpLog";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Form)this).WindowState = (FormWindowState)2;
		((Form)this).Load += frmBackUpLog_Load;
		((Control)this).KeyDown += new KeyEventHandler(frmBackUpLog_KeyDown);
		((ISupportInitialize)DGrid).EndInit();
		((ISupportInitialize)backUpLogBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
