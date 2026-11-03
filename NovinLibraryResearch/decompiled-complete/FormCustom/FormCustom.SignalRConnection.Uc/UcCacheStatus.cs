using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EntityCache;
using FormCustom.Properties;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace FormCustom.SignalRConnection.Uc;

public class UcCacheStatus : UserControl, IHasContext
{
	private TaskCompletionSource<bool> _parentFormVisibilityChanged = new TaskCompletionSource<bool>();

	private bool _isDispose;

	private bool _isEventHandlerCreate;

	private ToolTip _tip;

	private Form parentForm;

	private string _lastMessage;

	private string _contextGuid;

	private string _errorMessage;

	private Bitmap _lastpRes;

	private Color _lastpColor;

	private IContainer components;

	private Panel panel1;

	private ContextMenuStrip contextMenuStrip1;

	private ToolStripMenuItem CmDisplayStatus;

	private ToolStripMenuItem CmReConnect;

	private PictureBox btnStatus;

	public string ContextGuid
	{
		get
		{
			return _contextGuid;
		}
		set
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			try
			{
				if (!string.IsNullOrEmpty(value))
				{
					_contextGuid = value;
					_tip = new ToolTip();
					BindingSource();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ContextGuid", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcCacheStatus.cs", 43);
			}
		}
	}

	public bool ShowStatusMenu { get; set; } = true;


	public bool IsDisposed
	{
		get
		{
			if (!_isDispose)
			{
				return ((Control)this).IsDisposed;
			}
			return true;
		}
	}

	public UcCacheStatus()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private void BindingSource()
	{
		int num = 0;
		try
		{
			WebContextHolder.GetWebContextHolder(ContextGuid);
			num = 10;
			HandledCacheEventsAsync();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{num}", "BindingSource", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcCacheStatus.cs", 57);
		}
	}

	private async Task HandledCacheEventsAsync()
	{
		while (!_isEventHandlerCreate)
		{
			try
			{
				await Task.Delay(new Random().Next(1000));
				if (IsDisposed || string.IsNullOrEmpty(_contextGuid))
				{
					break;
				}
				ClsCache cache = ClsCache.GetCache(ContextGuid);
				if (cache == null)
				{
					await Task.Delay(new Random().Next(1000));
					continue;
				}
				if (IsDisposed)
				{
					break;
				}
				_isEventHandlerCreate = true;
				cache.BufferLoggedIn += LoginExtention_OnLoginResponse;
				cache.BufferConnected += Buffer_Connected;
				cache.BufferDisconnected += Buffer_Disconnected;
				LoginResponseEventArgs loginState = ClsCache.GetCache(ContextGuid).BufferLoginState;
				if (loginState == null)
				{
					Task.Run(() => CreateMessageAsync("ارتباط با سرور برقرار نیست", FormCustom.Properties.Resources.connection_failed, Color.Red, default(CancellationToken)));
				}
				else
				{
					switch (loginState.Status)
					{
					case EnLoginResult.Success:
						Task.Run(() => CreateMessageAsync("ارتباط  با سرور برقرار است", FormCustom.Properties.Resources.connection_succsess, Color.Blue, default(CancellationToken)));
						break;
					default:
						Task.Run(() => CreateMessageAsync(loginState.Status.GetDisplay().GetTranslate() + "-" + loginState.LoginResultMessage, FormCustom.Properties.Resources.connection_failed, Color.Red, default(CancellationToken)));
						break;
					}
				}
				ViewMessage(this, ClsCache.GetCache(ContextGuid)?.BufferLoginState, default(CancellationToken));
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "HandledCacheEventsAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcCacheStatus.cs", 115);
			}
		}
	}

	private Task Buffer_Disconnected(object sender, BaseEventArgs e)
	{
		try
		{
			Task.Run(() => CreateMessageAsync("ارتباط با سرور برقرار نیست".GetTranslate(), FormCustom.Properties.Resources.connection_failed, Color.Red, default(CancellationToken)));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Buffer_Disconnected", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcCacheStatus.cs", 124);
		}
		return Task.CompletedTask;
	}

	private Task Buffer_Connected(object sender, BaseEventArgs e)
	{
		try
		{
			_errorMessage = "";
			Task.Run(() => CreateMessageAsync("شروع ارتباط با سرور".GetTranslate(), FormCustom.Properties.Resources.connection_loading, Color.Blue, e.Connection.cancellationToken));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Buffer_Connected", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcCacheStatus.cs", 135);
		}
		return Task.CompletedTask;
	}

	private Task LoginExtention_OnLoginResponse(object sender, LoginResponseEventArgs e)
	{
		try
		{
			ViewMessage(sender, e, e.connection.cancellationToken);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoginExtention_OnLoginResponse", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcCacheStatus.cs", 145);
		}
		return Task.CompletedTask;
	}

	private void ParentForm_VisibleChanged(object sender, EventArgs e)
	{
		try
		{
			_parentFormVisibilityChanged?.TrySetResult(result: true);
			_parentFormVisibilityChanged = new TaskCompletionSource<bool>();
			CreateMessageAsync(_lastMessage, _lastpRes, _lastpColor, default(CancellationToken));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ParentForm_VisibleChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcCacheStatus.cs", 158);
		}
	}

	private async Task CreateMessageAsync(string pMessage, Bitmap pRes, Color pColor, CancellationToken token_)
	{
		try
		{
			_lastMessage = pMessage;
			if (!string.IsNullOrEmpty(_errorMessage))
			{
				_lastMessage += ("(" + _errorMessage + ")").GetTranslate();
			}
			_lastpRes = pRes;
			_lastpColor = pColor;
			if (parentForm == null)
			{
				parentForm = ((Control)this).FindForm();
				((Control)parentForm).VisibleChanged += ParentForm_VisibleChanged;
			}
			if (!((Control)parentForm).Visible)
			{
				return;
			}
			CancellationTokenSource token = CancellationTokenSource.CreateLinkedTokenSource(token_, new CancellationTokenSource(60000).Token);
			if (IsDisposed || token.IsCancellationRequested || string.IsNullOrEmpty(_contextGuid))
			{
				return;
			}
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(500);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
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
						_tip.SetToolTip((Control)(object)btnStatus, (!string.IsNullOrEmpty(_lastMessage)) ? _lastMessage : "ارتباط با سرور برقرار نیست".GetTranslate());
						btnStatus.Image = (Image)(object)_lastpRes;
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "CreateMessageAsync", "CreateMessageAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcCacheStatus.cs", 193);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CreateMessageAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcCacheStatus.cs", 196);
		}
	}

	private void ViewMessage(object sender, LoginResponseEventArgs e, CancellationToken token)
	{
		try
		{
			if (e == null)
			{
				Task.Run(() => CreateMessageAsync("تنظیمات ارتباط با سرور غیر فعال است".GetTranslate(), FormCustom.Properties.Resources.connection_failed, Color.Red, token));
				_errorMessage = "";
				return;
			}
			switch (e.Status)
			{
			case EnLoginResult.Unknown:
				Task.Run(() => CreateMessageAsync("عدم برقراری ارتباط از سمت کلاینت".GetTranslate(), FormCustom.Properties.Resources.connection_failed, Color.Red, token));
				_errorMessage = "";
				break;
			case EnLoginResult.Success:
				Task.Run(() => CreateMessageAsync("ارتباط  با سرور برقرار است".GetTranslate(), FormCustom.Properties.Resources.connection_succsess, Color.Blue, token));
				_errorMessage = "";
				break;
			case EnLoginResult.Duplicate:
				Task.Run(() => CreateMessageAsync("استفاده از کلید اشتراکی".GetTranslate(), FormCustom.Properties.Resources.connection_failed, Color.Red, token));
				_errorMessage = "استفاده از کلید اشتراکی".GetTranslate();
				break;
			case EnLoginResult.UserExpired:
			case EnLoginResult.DepartmentExpired:
			case EnLoginResult.NotExists:
			case EnLoginResult.DepartmentNotVerified:
			case EnLoginResult.TimeOut:
			case EnLoginResult.RequestCountExceeded:
			case EnLoginResult.SourceMismatch:
			case EnLoginResult.LoginCanceled:
			case EnLoginResult.CustomerNotExists:
			case EnLoginResult.AccountingUserNotAssigned:
			case EnLoginResult.UserDeleted:
			case EnLoginResult.DepartmentDeleted:
			case EnLoginResult.ServerError:
			case EnLoginResult.ClientError:
			case EnLoginResult.ConnectFailed:
			case EnLoginResult.DbGuidMisMatch:
				Task.Run(() => CreateMessageAsync(e.LoginResultMessage, FormCustom.Properties.Resources.connetion_error, Color.Red, token));
				_errorMessage = e.LoginResultMessage;
				break;
			default:
				Task.Run(() => CreateMessageAsync("قطع ارتباط به دلایل نا معلوم".GetTranslate(), FormCustom.Properties.Resources.connection_failed, Color.Red, token));
				break;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ViewMessage", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcCacheStatus.cs", 248);
		}
	}

	protected override void Dispose(bool disposing)
	{
		int num = 0;
		try
		{
			if (_isDispose)
			{
				return;
			}
			_isDispose = true;
			if (!string.IsNullOrEmpty(ContextGuid))
			{
				ClsCache cache = ClsCache.GetCache(ContextGuid);
				if (cache != null)
				{
					cache.BufferLoggedIn -= LoginExtention_OnLoginResponse;
					cache.BufferDisconnected -= Buffer_Disconnected;
					cache.BufferConnected -= Buffer_Connected;
				}
			}
			if (parentForm != null)
			{
				((Control)parentForm).VisibleChanged -= ParentForm_VisibleChanged;
			}
			parentForm = null;
			components?.Dispose();
			((ContainerControl)this).Dispose(disposing);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{num}", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcCacheStatus.cs", 278);
		}
	}

	private void CmReConnect_click(object sender, EventArgs e)
	{
		try
		{
			if (!IsDisposed)
			{
				_errorMessage = "";
				Task.Run(() => CreateMessageAsync("درحال برقراری ارتباط با سرور...".GetTranslate(), FormCustom.Properties.Resources.connection_loading, Color.Red, default(CancellationToken)));
				ClsCache.GetCache(ContextGuid)?.ConnectAsync(EnConnectionType.SignalRCore);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CmReConnect_click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcCacheStatus.cs", 290);
		}
	}

	private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
	{
		try
		{
			if (ShowStatusMenu)
			{
				((ToolStripItem)CmDisplayStatus).Visible = FrmConnectionSyncStatus.GetInstance(ContextGuid, ClsCache.GetCache(ContextGuid)?.Source ?? ENSource.None) != ((Control)this).FindForm();
			}
			else
			{
				((ToolStripItem)CmDisplayStatus).Visible = false;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "contextMenuStrip1_Opening", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcCacheStatus.cs", 302);
		}
	}

	private void CmDisplayStatus_Click(object sender, EventArgs e)
	{
		try
		{
			FrmConnectionSyncStatus instance = FrmConnectionSyncStatus.GetInstance(ContextGuid, ClsCache.GetCache(ContextGuid)?.Source ?? ENSource.None);
			if (instance == ((Control)this).FindForm())
			{
				throw new Exception("FrmConnectionSyncStatus.GetInstance(ContextGuid) is parent of current!");
			}
			if (!((Control)instance).Visible)
			{
				((Form)instance).Show((IWin32Window)(object)this);
			}
			else
			{
				((Control)instance).BringToFront();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CmDisplayStatus_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SignalRConnection\\Uc\\UcCacheStatus.cs", 316);
		}
	}

	private void InitializeComponent()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Expected O, but got Unknown
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		components = new Container();
		panel1 = new Panel();
		btnStatus = new PictureBox();
		contextMenuStrip1 = new ContextMenuStrip(components);
		CmDisplayStatus = new ToolStripMenuItem();
		CmReConnect = new ToolStripMenuItem();
		((Control)panel1).SuspendLayout();
		((ISupportInitialize)btnStatus).BeginInit();
		((Control)contextMenuStrip1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panel1).Controls.Add((Control)(object)btnStatus);
		((Control)panel1).Dock = (DockStyle)5;
		((Control)panel1).Location = new Point(0, 0);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(30, 27);
		((Control)panel1).TabIndex = 2;
		((Control)btnStatus).AccessibleRole = (AccessibleRole)43;
		((Control)btnStatus).ContextMenuStrip = contextMenuStrip1;
		((Control)btnStatus).Dock = (DockStyle)5;
		btnStatus.Image = (Image)(object)FormCustom.Properties.Resources.connection_loading;
		((Control)btnStatus).Location = new Point(0, 0);
		((Control)btnStatus).Name = "btnStatus";
		((Control)btnStatus).Size = new Size(30, 27);
		btnStatus.SizeMode = (PictureBoxSizeMode)4;
		btnStatus.TabIndex = 3;
		btnStatus.TabStop = false;
		((ToolStrip)contextMenuStrip1).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[2]
		{
			(ToolStripItem)CmDisplayStatus,
			(ToolStripItem)CmReConnect
		});
		((Control)contextMenuStrip1).Name = "contextMenuStrip1";
		((Control)contextMenuStrip1).Size = new Size(186, 74);
		((ToolStripDropDown)contextMenuStrip1).Opening += contextMenuStrip1_Opening;
		((ToolStripItem)CmDisplayStatus).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((ToolStripItem)CmDisplayStatus).Name = "CmDisplayStatus";
		((ToolStripItem)CmDisplayStatus).Size = new Size(185, 24);
		((ToolStripItem)CmDisplayStatus).Text = "مشاهده وضعیت سینک";
		((ToolStripItem)CmDisplayStatus).Click += CmDisplayStatus_Click;
		((ToolStripItem)CmReConnect).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((ToolStripItem)CmReConnect).Name = "CmReConnect";
		((ToolStripItem)CmReConnect).Size = new Size(185, 24);
		((ToolStripItem)CmReConnect).Text = "تجدید ارتباط";
		((ToolStripItem)CmReConnect).Click += CmReConnect_click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((UserControl)this).BorderStyle = (BorderStyle)1;
		((Control)this).ContextMenuStrip = contextMenuStrip1;
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Name = "UcCacheStatus";
		((Control)this).Size = new Size(30, 27);
		((Control)panel1).ResumeLayout(false);
		((ISupportInitialize)btnStatus).EndInit();
		((Control)contextMenuStrip1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
