using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EntityCache.Interfaces.Sync;
using FormCustom.Properties;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace FormCustom.SignalRConnection.Uc;

public class UcSyncStatus : UserControl, ISendData2Server, INpDisposable, IDisposable
{
	private bool _isDisposed;

	private CancellationTokenSource _updateUiToken;

	private string _shortMessage = "";

	private string _title = "";

	private decimal _percent;

	private int _totalCount;

	private int _count;

	private SyncPart _group = SyncPart.None;

	private TaskCompletionSource<bool> _uiHasUpdated = new TaskCompletionSource<bool>();

	private TaskCompletionSource<bool> _parentFormVisibilityChanged = new TaskCompletionSource<bool>();

	private SyncStatus status;

	private Form parentForm;

	private IContainer components;

	private Label lblTitle;

	private PictureBox pictureBox1;

	private Label lblShortMessage;

	private Label lblPercent;

	private ToolTip toolTip1;

	public bool IsDisposed
	{
		get
		{
			if (!((Control)this).IsDisposed)
			{
				return _isDisposed;
			}
			return true;
		}
	}

	public ReturnedSaveFuncInfo FinalStatus { get; set; }

	public string Title
	{
		get
		{
			return _title;
		}
		set
		{
			try
			{
				_title = value;
				((Control)lblTitle).Text = value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Title", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcSyncStatus.cs", 38);
			}
		}
	}

	public SyncStatus Status
	{
		get
		{
			return status;
		}
		set
		{
			if (!IsDisposed && status != value)
			{
				status = value;
				_uiHasUpdated.TrySetResult(result: true);
				_uiHasUpdated = new TaskCompletionSource<bool>();
			}
		}
	}

	public string ShortMessage
	{
		get
		{
			return _shortMessage;
		}
		set
		{
			try
			{
				if (!IsDisposed && !string.IsNullOrEmpty(value) && !(_shortMessage == value))
				{
					_shortMessage = value;
					_uiHasUpdated.TrySetResult(result: true);
					_uiHasUpdated = new TaskCompletionSource<bool>();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ShortMessage", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcSyncStatus.cs", 64);
			}
		}
	}

	public int TotalCount
	{
		get
		{
			return _totalCount;
		}
		set
		{
			try
			{
				if (!IsDisposed && _totalCount != value)
				{
					_totalCount = value;
					_uiHasUpdated.TrySetResult(result: true);
					_uiHasUpdated = new TaskCompletionSource<bool>();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "TotalCount", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcSyncStatus.cs", 79);
			}
		}
	}

	public int Count
	{
		get
		{
			return _count;
		}
		set
		{
			try
			{
				if (!IsDisposed && _count != value)
				{
					_count = value;
					_uiHasUpdated.TrySetResult(result: true);
					_uiHasUpdated = new TaskCompletionSource<bool>();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Count", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcSyncStatus.cs", 94);
			}
		}
	}

	public SyncPart Group
	{
		get
		{
			return _group;
		}
		set
		{
			try
			{
				if (!IsDisposed && _group != value)
				{
					_group = value;
					_uiHasUpdated.TrySetResult(result: true);
					_uiHasUpdated = new TaskCompletionSource<bool>();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Group", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcSyncStatus.cs", 109);
			}
		}
	}

	private async Task UpdateUiAsync()
	{
		_ = 3;
		try
		{
			do
			{
				await Task.Delay(500);
				if (IsDisposed)
				{
					return;
				}
			}
			while (!((Control)this).IsHandleCreated);
			parentForm = ((Control)this).FindForm();
			if (parentForm == null)
			{
				return;
			}
			((Control)parentForm).VisibleChanged += ParentForm_VisibleChanged;
			while (!IsDisposed)
			{
				try
				{
					int num;
					_ = num - 1;
					_ = 1;
					try
					{
						if (!((Control)parentForm).Visible)
						{
							await Task.WhenAny(new Task[2]
							{
								Task.Delay(60000),
								_parentFormVisibilityChanged.Task
							});
							parentForm = ((Control)this).FindForm();
							goto end_IL_00f3;
						}
						if (!((Control)this).IsHandleCreated)
						{
							goto end_IL_00f3;
						}
						await _uiHasUpdated.Task;
						_updateUiToken?.Cancel();
						_updateUiToken = new CancellationTokenSource(60000);
						CancellationToken token = _updateUiToken.Token;
						UpdateUiAsync_(token);
						goto end_IL_00e1;
						end_IL_00f3:;
					}
					catch (Exception ex)
					{
						WebErrorLog.Instance.StartLog(ex, "", "UpdateUiAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcSyncStatus.cs", 143);
						goto end_IL_00e1;
					}
					end_IL_00e1:;
				}
				finally
				{
					await Task.Delay(5000);
				}
			}
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "UpdateUiAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcSyncStatus.cs", 146);
		}
	}

	private async Task UpdateUiAsync_(CancellationToken token)
	{
		try
		{
			do
			{
				await Task.Delay(1000);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			while (!((Control)this).IsHandleCreated);
			using (new JaegerMonitor("UpdateUiAsync_", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcSyncStatus.cs"))
			{
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				((Control)this).BeginInvoke((Delegate)(Action)delegate
				{
					try
					{
						if (!IsDisposed && !token.IsCancellationRequested)
						{
							switch (Status)
							{
							case SyncStatus.SyncNotStarted:
								pictureBox1.Image = (Image)(object)FormCustom.Properties.Resources.Info;
								break;
							case SyncStatus.Syncing:
								pictureBox1.Image = (Image)(object)FormCustom.Properties.Resources.wait_Small_JPG;
								break;
							case SyncStatus.SyncedOk:
								pictureBox1.Image = (Image)(object)FormCustom.Properties.Resources.accept;
								break;
							case SyncStatus.SyncFailed:
								pictureBox1.Image = (Image)(object)FormCustom.Properties.Resources.Error;
								break;
							default:
								pictureBox1.Image = (Image)(object)FormCustom.Properties.Resources.icons8_warning_50;
								WebErrorLog.Instance.StartLog($"status({Status}) is not defined!", "", "UpdateUiAsync_", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcSyncStatus.cs", 174);
								break;
							}
							switch (Group)
							{
							case SyncPart.Customers:
								((Control)this).BackColor = Color.BurlyWood;
								break;
							case SyncPart.Hesabs:
								((Control)this).BackColor = Color.DarkSeaGreen;
								break;
							case SyncPart.Operations:
								((Control)this).BackColor = Color.LightPink;
								break;
							case SyncPart.Others:
								((Control)this).BackColor = Color.SteelBlue;
								break;
							case SyncPart.Products:
								((Control)this).BackColor = Color.DarkOrange;
								break;
							}
							if (!string.IsNullOrEmpty(_shortMessage))
							{
								((Control)lblShortMessage).Text = _shortMessage;
							}
							if (Count != 0 && TotalCount != 0)
							{
								_percent = Math.Round(100m * (decimal)Count / (decimal)TotalCount, 2);
							}
							else
							{
								_percent = 0m;
							}
							((Control)lblPercent).Text = $"{_percent:N0}%";
							if (_percent < 100m && _percent > 0m)
							{
								toolTip1.SetToolTip((Control)(object)lblPercent, $"ارسال {Count} از {TotalCount}");
								if (!((Control)lblPercent).Visible)
								{
									((Control)lblPercent).Visible = true;
								}
							}
							else
							{
								((Control)lblPercent).Visible = false;
							}
						}
					}
					catch (OperationCanceledException)
					{
					}
					catch (Exception ex4)
					{
						WebErrorLog.Instance.StartLog(ex4, "", "UpdateUiAsync_", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcSyncStatus.cs", 200);
					}
				});
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "UpdateUiAsync_", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcSyncStatus.cs", 204);
		}
	}

	public void Reset()
	{
		try
		{
			if (FinalStatus == null)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo2 = (FinalStatus = new ReturnedSaveFuncInfo());
			}
			FinalStatus?.ResetWarningList();
			FinalStatus?.ResetInformationList();
			FinalStatus?.ResetErrorList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Reset", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcSyncStatus.cs", 215);
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_updateUiToken?.Cancel();
				_updateUiToken = null;
				if (parentForm != null)
				{
					((Control)parentForm).VisibleChanged -= ParentForm_VisibleChanged;
				}
				parentForm = null;
				components?.Dispose();
				((ContainerControl)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcSyncStatus.cs", 233);
		}
	}

	public UcSyncStatus()
	{
		try
		{
			InitializeComponent();
			((Control)(object)this).Transelate();
			Task.Run((Func<Task?>)UpdateUiAsync);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcSyncStatus.cs", 246);
		}
	}

	private void ParentForm_VisibleChanged(object sender, EventArgs e)
	{
		try
		{
			_parentFormVisibilityChanged?.TrySetResult(result: true);
			_parentFormVisibilityChanged = new TaskCompletionSource<bool>();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ParentForm_VisibleChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcSyncStatus.cs", 256);
		}
	}

	private void controls_Click(object sender, EventArgs e)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			this.ShowReturnedSavedFuncInfoForUserControl(FinalStatus ?? new ReturnedSaveFuncInfo(), "نتیجه سینک " + Title, "controls_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcSyncStatus.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "controls_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcSyncStatus.cs", 264);
		}
	}

	private void UcSyncStatus_Load(object sender, EventArgs e)
	{
		try
		{
			_updateUiToken?.Cancel();
			_updateUiToken = new CancellationTokenSource(60000);
			CancellationToken token = _updateUiToken.Token;
			UpdateUiAsync_(token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UcSyncStatus_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcSyncStatus.cs", 275);
		}
	}

	private void InitializeComponent()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Expected O, but got Unknown
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Expected O, but got Unknown
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Expected O, but got Unknown
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		lblTitle = new Label();
		pictureBox1 = new PictureBox();
		lblShortMessage = new Label();
		lblPercent = new Label();
		toolTip1 = new ToolTip(components);
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)this).SuspendLayout();
		((Control)lblTitle).Anchor = (AnchorStyles)13;
		((Control)lblTitle).BackColor = Color.Transparent;
		((Control)lblTitle).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblTitle).ForeColor = Color.DarkBlue;
		((Control)lblTitle).Location = new Point(48, 0);
		((Control)lblTitle).Margin = new Padding(4, 0, 4, 0);
		((Control)lblTitle).Name = "lblTitle";
		((Control)lblTitle).Size = new Size(300, 24);
		((Control)lblTitle).TabIndex = 0;
		((Control)lblTitle).Text = "[سینک اشخاص]";
		lblTitle.TextAlign = (ContentAlignment)32;
		((Control)lblTitle).Click += controls_Click;
		((Control)pictureBox1).Anchor = (AnchorStyles)11;
		pictureBox1.Image = (Image)(object)FormCustom.Properties.Resources.accept;
		((Control)pictureBox1).Location = new Point(352, 2);
		((Control)pictureBox1).Margin = new Padding(4, 5, 4, 5);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(45, 36);
		pictureBox1.SizeMode = (PictureBoxSizeMode)4;
		pictureBox1.TabIndex = 1;
		pictureBox1.TabStop = false;
		((Control)pictureBox1).Click += controls_Click;
		((Control)lblShortMessage).Anchor = (AnchorStyles)15;
		((Control)lblShortMessage).BackColor = Color.Transparent;
		((Control)lblShortMessage).Font = new Font("B Yekan", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblShortMessage).Location = new Point(-1, 21);
		((Control)lblShortMessage).Margin = new Padding(4, 0, 4, 0);
		((Control)lblShortMessage).Name = "lblShortMessage";
		((Control)lblShortMessage).Size = new Size(350, 15);
		((Control)lblShortMessage).TabIndex = 2;
		((Control)lblShortMessage).Text = "[وضعیت سینک ...]";
		lblShortMessage.TextAlign = (ContentAlignment)16;
		((Control)lblShortMessage).Click += controls_Click;
		((Control)lblPercent).BackColor = Color.Transparent;
		((Control)lblPercent).Font = new Font("B Yekan", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblPercent).Location = new Point(-1, -3);
		((Control)lblPercent).Margin = new Padding(4, 0, 4, 0);
		((Control)lblPercent).Name = "lblPercent";
		((Control)lblPercent).Size = new Size(49, 27);
		((Control)lblPercent).TabIndex = 3;
		((Control)lblPercent).Text = "%0";
		lblPercent.TextAlign = (ContentAlignment)32;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((UserControl)this).BorderStyle = (BorderStyle)1;
		((Control)this).Controls.Add((Control)(object)lblPercent);
		((Control)this).Controls.Add((Control)(object)lblShortMessage);
		((Control)this).Controls.Add((Control)(object)pictureBox1);
		((Control)this).Controls.Add((Control)(object)lblTitle);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcSyncStatus";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(400, 40);
		((UserControl)this).Load += UcSyncStatus_Load;
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
