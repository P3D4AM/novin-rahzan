using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EntityCache.Interfaces.Sync;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Interfaces;
using PacketParser.Entities;

namespace FormCustom.SignalRConnection;

public class frmMasterConnectionSyncStatus : AccountingBaseForm, IMasterSendData2Server, IReturnedSaveFuncInfo
{
	private static ConcurrentDictionary<string, frmMasterConnectionSyncStatus> _instances = new ConcurrentDictionary<string, frmMasterConnectionSyncStatus>();

	private CancellationTokenSource _descToken = new CancellationTokenSource();

	private CancellationTokenSource _retToken = new CancellationTokenSource();

	private CancellationTokenSource _startSyncToken = new CancellationTokenSource();

	private CancellationTokenSource _endSyncToken = new CancellationTokenSource();

	private ReturnedSaveFuncInfo res;

	private string _message;

	private DateTime _startSync;

	private DateTime? _endSync;

	private IContainer components;

	private SplitContainer splitContainer1;

	private DataGridView dgridDesc;

	private DataGridView dgridRet;

	private DataGridViewTextBoxColumn dgDesc;

	private DataGridViewTextBoxColumn dgRetDesc;

	private Label label1;

	private Label lblStartSync;

	private Label label2;

	private Label lblEndSync;

	public string Description
	{
		get
		{
			return _message;
		}
		set
		{
			_message = value;
			_descToken?.Cancel();
			_descToken = new CancellationTokenSource(60000);
			CancellationToken tkn = _descToken.Token;
			Task.Run(() => LoadDescriptionToGridAsync(_message, tkn));
		}
	}

	public DateTime StartSync
	{
		get
		{
			return _startSync;
		}
		set
		{
			_startSync = value;
			_startSyncToken?.Cancel();
			_startSyncToken = new CancellationTokenSource(60000);
			CancellationToken tkn = _startSyncToken.Token;
			Task.Run(() => ChangeStartSyncAsync(tkn));
		}
	}

	public string StartSyncTime => StartSync.ToShortTimeString();

	public DateTime? EndSync
	{
		get
		{
			return _endSync;
		}
		set
		{
			_endSync = value;
			_endSyncToken?.Cancel();
			_endSyncToken = new CancellationTokenSource(60000);
			CancellationToken tkn = _endSyncToken.Token;
			Task.Run(() => ChangeEndSyncAsync(tkn));
		}
	}

	public string EndSyncTime
	{
		get
		{
			if (EndSync.HasValue)
			{
				return EndSync.Value.ToShortTimeString();
			}
			return "-";
		}
	}

	public ReturnedSaveFuncInfo Result => res;

	public MasterSendData2ServerAggregator MasterSendData2ServerInstance { get; private set; }

	public void ResetResults()
	{
		res = new ReturnedSaveFuncInfo(this);
	}

	public frmMasterConnectionSyncStatus(string contextGuid)
		: base(contextGuid)
	{
		InitializeComponent();
		res = new ReturnedSaveFuncInfo(this);
		MasterSendData2ServerInstance = new MasterSendData2ServerAggregator(this);
	}

	public static frmMasterConnectionSyncStatus GetInstance(string contextGuid)
	{
		try
		{
			if (_instances.TryGetValue(contextGuid, out var value))
			{
				return value;
			}
			value = new frmMasterConnectionSyncStatus(contextGuid);
			_instances.TryAdd(contextGuid, value);
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInstance", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\frmMasterConnectionSyncStatus.cs", 88);
			return null;
		}
	}

	public async Task AddErrorAsync(string errorMessage, string key = "")
	{
		try
		{
			if (!string.IsNullOrEmpty(errorMessage))
			{
				_retToken?.Cancel();
				_retToken = new CancellationTokenSource(60000);
				CancellationToken tkn = _retToken.Token;
				Task.Run(() => LoadRetuernedSaveFuncInfoToGridAsync(errorMessage, tkn));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddErrorAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\frmMasterConnectionSyncStatus.cs", 103);
		}
	}

	public async Task AddWarningAsync(string warningMessage, string key = "")
	{
		try
		{
			if (!string.IsNullOrEmpty(warningMessage))
			{
				_retToken?.Cancel();
				_retToken = new CancellationTokenSource(60000);
				CancellationToken tkn = _retToken.Token;
				Task.Run(() => LoadRetuernedSaveFuncInfoToGridAsync(warningMessage, tkn));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddWarningAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\frmMasterConnectionSyncStatus.cs", 118);
		}
	}

	public async Task AddInformationAsync(string message, string key = "")
	{
		try
		{
			if (!string.IsNullOrEmpty(message))
			{
				_retToken?.Cancel();
				_retToken = new CancellationTokenSource(60000);
				CancellationToken tkn = _retToken.Token;
				Task.Run(() => LoadRetuernedSaveFuncInfoToGridAsync(message, tkn));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddInformationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\frmMasterConnectionSyncStatus.cs", 133);
		}
	}

	private async Task LoadRetuernedSaveFuncInfoToGridAsync(string message, CancellationToken token)
	{
		try
		{
			if (base.IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			while (!((Control)this).IsHandleCreated)
			{
				if (base.IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				await Task.Delay(500);
			}
			if (base.IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).Invoke((Delegate)(Action)delegate
			{
				try
				{
					if (!base.IsDisposed && !token.IsCancellationRequested)
					{
						dgridRet.Rows.Add(new object[1] { message });
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "LoadRetuernedSaveFuncInfoToGridAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\frmMasterConnectionSyncStatus.cs", 157);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadRetuernedSaveFuncInfoToGridAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\frmMasterConnectionSyncStatus.cs", 163);
		}
	}

	private async Task LoadDescriptionToGridAsync(string message, CancellationToken token)
	{
		try
		{
			if (base.IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			while (!((Control)this).IsHandleCreated)
			{
				if (base.IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				await Task.Delay(500);
			}
			if (base.IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).Invoke((Delegate)(Action)delegate
			{
				try
				{
					if (!base.IsDisposed && !token.IsCancellationRequested)
					{
						dgridDesc.Rows.Add(new object[1] { message });
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "LoadDescriptionToGridAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\frmMasterConnectionSyncStatus.cs", 187);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDescriptionToGridAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\frmMasterConnectionSyncStatus.cs", 193);
		}
	}

	private async Task ChangeStartSyncAsync(CancellationToken token)
	{
		try
		{
			if (base.IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			while (!((Control)this).IsHandleCreated)
			{
				if (base.IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				await Task.Delay(500);
			}
			if (base.IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).Invoke((Delegate)(Action)delegate
			{
				try
				{
					if (!base.IsDisposed && !token.IsCancellationRequested)
					{
						((Control)lblStartSync).Text = StartSyncTime;
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "ChangeStartSyncAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\frmMasterConnectionSyncStatus.cs", 217);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChangeStartSyncAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\frmMasterConnectionSyncStatus.cs", 223);
		}
	}

	private async Task ChangeEndSyncAsync(CancellationToken token)
	{
		try
		{
			if (base.IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			while (!((Control)this).IsHandleCreated)
			{
				if (base.IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				await Task.Delay(500);
			}
			if (base.IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).Invoke((Delegate)(Action)delegate
			{
				try
				{
					if (!base.IsDisposed && !token.IsCancellationRequested)
					{
						((Control)lblEndSync).Text = EndSyncTime;
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "ChangeEndSyncAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\frmMasterConnectionSyncStatus.cs", 247);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChangeEndSyncAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\frmMasterConnectionSyncStatus.cs", 251);
		}
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Expected O, but got Unknown
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Expected O, but got Unknown
		//IL_0675: Unknown result type (might be due to invalid IL or missing references)
		//IL_067f: Expected O, but got Unknown
		//IL_068b: Unknown result type (might be due to invalid IL or missing references)
		DataGridViewCellStyle val = new DataGridViewCellStyle();
		DataGridViewCellStyle val2 = new DataGridViewCellStyle();
		DataGridViewCellStyle val3 = new DataGridViewCellStyle();
		DataGridViewCellStyle val4 = new DataGridViewCellStyle();
		splitContainer1 = new SplitContainer();
		dgridDesc = new DataGridView();
		dgDesc = new DataGridViewTextBoxColumn();
		dgridRet = new DataGridView();
		dgRetDesc = new DataGridViewTextBoxColumn();
		label1 = new Label();
		lblStartSync = new Label();
		label2 = new Label();
		lblEndSync = new Label();
		((ISupportInitialize)splitContainer1).BeginInit();
		((Control)splitContainer1.Panel1).SuspendLayout();
		((Control)splitContainer1.Panel2).SuspendLayout();
		((Control)splitContainer1).SuspendLayout();
		((ISupportInitialize)dgridDesc).BeginInit();
		((ISupportInitialize)dgridRet).BeginInit();
		((Control)this).SuspendLayout();
		((Control)splitContainer1).Anchor = (AnchorStyles)15;
		((Control)splitContainer1).Location = new Point(0, 35);
		((Control)splitContainer1).Name = "splitContainer1";
		((Control)splitContainer1.Panel1).Controls.Add((Control)(object)dgridDesc);
		((Control)splitContainer1.Panel1).RightToLeft = (RightToLeft)1;
		((Control)splitContainer1.Panel2).Controls.Add((Control)(object)dgridRet);
		((Control)splitContainer1.Panel2).RightToLeft = (RightToLeft)1;
		((Control)splitContainer1).Size = new Size(784, 526);
		splitContainer1.SplitterDistance = 402;
		((Control)splitContainer1).TabIndex = 0;
		dgridDesc.AllowUserToAddRows = false;
		dgridDesc.AllowUserToDeleteRows = false;
		val.BackColor = Color.White;
		dgridDesc.AlternatingRowsDefaultCellStyle = val;
		dgridDesc.ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		dgridDesc.Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[1] { (DataGridViewColumn)dgDesc });
		((Control)dgridDesc).Dock = (DockStyle)5;
		((Control)dgridDesc).Location = new Point(0, 0);
		((Control)dgridDesc).Margin = new Padding(3, 4, 3, 4);
		((Control)dgridDesc).Name = "dgridDesc";
		dgridDesc.ReadOnly = true;
		dgridDesc.RowHeadersVisible = false;
		val2.Font = new Font("B Yekan", 8.25f);
		dgridDesc.RowsDefaultCellStyle = val2;
		dgridDesc.RowTemplate.Height = 30;
		dgridDesc.SelectionMode = (DataGridViewSelectionMode)1;
		((Control)dgridDesc).Size = new Size(402, 526);
		((Control)dgridDesc).TabIndex = 3;
		((DataGridViewColumn)dgDesc).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)dgDesc).HeaderText = "[شرح سینک]";
		((DataGridViewColumn)dgDesc).Name = "dgDesc";
		((DataGridViewBand)dgDesc).ReadOnly = true;
		dgridRet.AllowUserToAddRows = false;
		dgridRet.AllowUserToDeleteRows = false;
		val3.BackColor = Color.White;
		dgridRet.AlternatingRowsDefaultCellStyle = val3;
		dgridRet.ColumnHeadersHeightSizeMode = (DataGridViewColumnHeadersHeightSizeMode)2;
		dgridRet.Columns.AddRange((DataGridViewColumn[])(object)new DataGridViewColumn[1] { (DataGridViewColumn)dgRetDesc });
		((Control)dgridRet).Dock = (DockStyle)5;
		((Control)dgridRet).Location = new Point(0, 0);
		((Control)dgridRet).Margin = new Padding(3, 4, 3, 4);
		((Control)dgridRet).Name = "dgridRet";
		dgridRet.ReadOnly = true;
		dgridRet.RowHeadersVisible = false;
		val4.Font = new Font("B Yekan", 8.25f);
		dgridRet.RowsDefaultCellStyle = val4;
		dgridRet.RowTemplate.Height = 30;
		dgridRet.SelectionMode = (DataGridViewSelectionMode)1;
		((Control)dgridRet).Size = new Size(378, 526);
		((Control)dgridRet).TabIndex = 3;
		((DataGridViewColumn)dgRetDesc).AutoSizeMode = (DataGridViewAutoSizeColumnMode)16;
		((DataGridViewColumn)dgRetDesc).HeaderText = "[پیغام سیستم]";
		((DataGridViewColumn)dgRetDesc).Name = "dgRetDesc";
		((DataGridViewBand)dgRetDesc).ReadOnly = true;
		((Control)label1).Anchor = (AnchorStyles)9;
		((Control)label1).AutoSize = true;
		((Control)label1).Location = new Point(687, 9);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(85, 20);
		((Control)label1).TabIndex = 1;
		((Control)label1).Text = "[زمان آغاز سینک]";
		((Control)lblStartSync).Anchor = (AnchorStyles)9;
		((Control)lblStartSync).Location = new Point(536, 9);
		((Control)lblStartSync).Name = "lblStartSync";
		((Control)lblStartSync).Size = new Size(145, 20);
		((Control)lblStartSync).TabIndex = 1;
		((Control)lblStartSync).Text = "-";
		((Control)label2).Anchor = (AnchorStyles)9;
		((Control)label2).AutoSize = true;
		((Control)label2).Location = new Point(406, 9);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(88, 20);
		((Control)label2).TabIndex = 1;
		((Control)label2).Text = "[زمان پایان سینک]";
		((Control)lblEndSync).Anchor = (AnchorStyles)9;
		((Control)lblEndSync).Location = new Point(255, 9);
		((Control)lblEndSync).Name = "lblEndSync";
		((Control)lblEndSync).Size = new Size(145, 20);
		((Control)lblEndSync).TabIndex = 1;
		((Control)lblEndSync).Text = "-";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(784, 561);
		((Control)this).Controls.Add((Control)(object)lblEndSync);
		((Control)this).Controls.Add((Control)(object)label2);
		((Control)this).Controls.Add((Control)(object)lblStartSync);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Controls.Add((Control)(object)splitContainer1);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "frmMasterConnectionSyncStatus";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)splitContainer1.Panel1).ResumeLayout(false);
		((Control)splitContainer1.Panel2).ResumeLayout(false);
		((ISupportInitialize)splitContainer1).EndInit();
		((Control)splitContainer1).ResumeLayout(false);
		((ISupportInitialize)dgridDesc).EndInit();
		((ISupportInitialize)dgridRet).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
