using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AccSqlServerPersistence.AssistanceClasses;
using DevComponents.DotNetBar.Controls;
using FormCustom.Cache;
using PacketParser.Entities;

namespace FormCustom.BackUp.Forms;

public class frmBackUpLogSlaves : CacheBaseForm
{
	private Guid _masterGuid;

	private string _connectionString;

	private bool _isDisposed;

	private IContainer components;

	private BindingSource backUpLogBindingSource;

	internal DataGridViewX DGrid;

	private DataGridViewTextBoxColumn Radif;

	private DataGridViewTextBoxColumn statusNameDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn statusDescriptionDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn guidDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn masterGuidDataGridViewTextBoxColumn;

	private DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;

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

	public frmBackUpLogSlaves(Guid masterGuid, string connectionString, string contextGuid)
		: base(contextGuid)
	{
		InitializeComponent();
		_masterGuid = masterGuid;
		_connectionString = connectionString;
	}

	private void frmBackUpLogSlaves_Load(object sender, EventArgs e)
	{
		Task.Run(() => LoadDateAsync(_masterGuid));
	}

	private async Task LoadDateAsync(Guid masterGuid, CancellationToken token = default(CancellationToken))
	{
		try
		{
			List<SqlBackUpLogSlaves> list = await SqlBackUpLogSlaves.GetAllAsync(masterGuid, _connectionString);
			((Control)this).Invoke((Delegate)(MethodInvoker)delegate
			{
				backUpLogBindingSource.DataSource = list.OrderBy((SqlBackUpLogSlaves q) => q.DateM).ToSortableBindingList(token);
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmBackUpLogSlaves.cs", 36);
		}
	}

	private void DGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
	{
		((DataGridView)DGrid).Rows[e.RowIndex].Cells["Radif"].Value = e.RowIndex + 1;
	}

	private void frmBackUpLogSlaves_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 27)
			{
				((Form)this).Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "frmBackUpLogSlaves_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmBackUpLogSlaves.cs", 54);
		}
	}

	private void frmBackUpLogSlaves_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\frmBackUpLogSlaves.cs", 76);
		}
	}

	private void InitializeComponent()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Expected O, but got Unknown
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Expected O, but got Unknown
		//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ea: Expected O, but got Unknown
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Expected O, but got Unknown
		//IL_054b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Expected O, but got Unknown
		components = new Container();
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		backUpLogBindingSource = new BindingSource(components);
		DGrid = new DataGridViewX();
		Radif = new DataGridViewTextBoxColumn();
		statusNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		statusDescriptionDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		descriptionDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		guidDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		masterGuidDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		statusDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
		((ISupportInitialize)backUpLogBindingSource).BeginInit();
		((ISupportInitialize)DGrid).BeginInit();
		((Control)this).SuspendLayout();
		backUpLogBindingSource.DataSource = typeof(SqlBackUpLogSlaves);
		((DataGridView)DGrid).AllowUserToAddRows = false;
		((DataGridView)DGrid).AllowUserToDeleteRows = false;
		((DataGridView)DGrid).AllowUserToOrderColumns = true;
		((DataGridView)DGrid).AllowUserToResizeRows = false;
		((DataGridView)DGrid).AutoGenerateColumns = false;
		((DataGridView)DGrid).ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		((DataGridView)DGrid).Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[7]
		{
			(DataGridViewColumn)Radif,
			(DataGridViewColumn)statusNameDataGridViewTextBoxColumn,
			(DataGridViewColumn)statusDescriptionDataGridViewTextBoxColumn,
			(DataGridViewColumn)descriptionDataGridViewTextBoxColumn,
			(DataGridViewColumn)guidDataGridViewTextBoxColumn,
			(DataGridViewColumn)masterGuidDataGridViewTextBoxColumn,
			(DataGridViewColumn)statusDataGridViewTextBoxColumn
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
		((Control)DGrid).Dock = (DockStyle)5;
		((DataGridView)DGrid).GridColor = Color.FromArgb(208, 215, 229);
		((Control)DGrid).Location = new Point(0, 0);
		((Control)DGrid).Name = "DGrid";
		((DataGridView)DGrid).ReadOnly = true;
		((DataGridView)DGrid).RowHeadersVisible = false;
		((DataGridView)DGrid).SelectionMode = (DataGridViewSelectionMode)1;
		((Control)DGrid).Size = new Size(784, 561);
		((Control)DGrid).TabIndex = 42;
		((DataGridView)DGrid).CellFormatting += new DataGridViewCellFormattingEventHandler(DGrid_CellFormatting);
		((DataGridViewColumn)Radif).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		val2.NullValue = null;
		((DataGridViewBand)Radif).DefaultCellStyle = val2;
		((DataGridViewColumn)Radif).HeaderText = "[ردیف]";
		((DataGridViewColumn)Radif).Name = "Radif";
		((DataGridViewBand)Radif).ReadOnly = true;
		((DataGridViewColumn)Radif).Width = 54;
		((DataGridViewColumn)statusNameDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)statusNameDataGridViewTextBoxColumn).DataPropertyName = "StatusName";
		((DataGridViewColumn)statusNameDataGridViewTextBoxColumn).HeaderText = "[وضعیت]";
		((DataGridViewColumn)statusNameDataGridViewTextBoxColumn).Name = "statusNameDataGridViewTextBoxColumn";
		((DataGridViewBand)statusNameDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)statusNameDataGridViewTextBoxColumn).Width = 66;
		((DataGridViewColumn)statusDescriptionDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)6;
		((DataGridViewColumn)statusDescriptionDataGridViewTextBoxColumn).DataPropertyName = "StatusDescription";
		((DataGridViewColumn)statusDescriptionDataGridViewTextBoxColumn).HeaderText = "[شرح مختصر]";
		((DataGridViewColumn)statusDescriptionDataGridViewTextBoxColumn).Name = "statusDescriptionDataGridViewTextBoxColumn";
		((DataGridViewBand)statusDescriptionDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)statusDescriptionDataGridViewTextBoxColumn).Width = 89;
		((DataGridViewColumn)descriptionDataGridViewTextBoxColumn).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)descriptionDataGridViewTextBoxColumn).DataPropertyName = "Description";
		((DataGridViewColumn)descriptionDataGridViewTextBoxColumn).HeaderText = "[توضیحات]";
		((DataGridViewColumn)descriptionDataGridViewTextBoxColumn).Name = "descriptionDataGridViewTextBoxColumn";
		((DataGridViewBand)descriptionDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewColumn)guidDataGridViewTextBoxColumn).DataPropertyName = "Guid";
		((DataGridViewColumn)guidDataGridViewTextBoxColumn).HeaderText = "Guid";
		((DataGridViewColumn)guidDataGridViewTextBoxColumn).Name = "guidDataGridViewTextBoxColumn";
		((DataGridViewBand)guidDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)guidDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)masterGuidDataGridViewTextBoxColumn).DataPropertyName = "MasterGuid";
		((DataGridViewColumn)masterGuidDataGridViewTextBoxColumn).HeaderText = "MasterGuid";
		((DataGridViewColumn)masterGuidDataGridViewTextBoxColumn).Name = "masterGuidDataGridViewTextBoxColumn";
		((DataGridViewBand)masterGuidDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)masterGuidDataGridViewTextBoxColumn).Visible = false;
		((DataGridViewColumn)statusDataGridViewTextBoxColumn).DataPropertyName = "Status";
		((DataGridViewColumn)statusDataGridViewTextBoxColumn).HeaderText = "Status";
		((DataGridViewColumn)statusDataGridViewTextBoxColumn).Name = "statusDataGridViewTextBoxColumn";
		((DataGridViewBand)statusDataGridViewTextBoxColumn).ReadOnly = true;
		((DataGridViewBand)statusDataGridViewTextBoxColumn).Visible = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(784, 561);
		((Control)this).Controls.Add((Control)(object)DGrid);
		((Control)this).Font = new Font("Tahoma", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Form)this).KeyPreview = true;
		((Control)this).MinimumSize = new Size(800, 600);
		((Control)this).Name = "frmBackUpLogSlaves";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Form)this).FormClosed += new FormClosedEventHandler(frmBackUpLogSlaves_FormClosed);
		((Form)this).Load += frmBackUpLogSlaves_Load;
		((Control)this).KeyDown += new KeyEventHandler(frmBackUpLogSlaves_KeyDown);
		((ISupportInitialize)backUpLogBindingSource).EndInit();
		((ISupportInitialize)DGrid).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
