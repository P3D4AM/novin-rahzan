using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using EntityCache.Interfaces;
using EntityCache.Interfaces.Notification;
using FormCustom.Cache;
using FormCustom.Properties;
using PacketParser;
using PacketParser.Entities;

namespace FormCustom.Notification;

public class FrmSimpleNotification : CacheBaseForm, IUiNotification
{
	private bool mouseIsDown;

	private bool _isDisposed;

	private Point firstPoint;

	private Action _clickAction;

	private Action _doubleClickAction;

	private const int WS_EX_TOPMOST = 8;

	private string _title;

	private string _message;

	private ReturnedState _status;

	private CancellationTokenSource _autoDisposedToken;

	private CancellationTokenSource _titleToken;

	private CancellationTokenSource _messageToken;

	private CancellationTokenSource _statusToken;

	private IContainer components;

	private GroupPanel groupPanel1;

	private Label lblDateTime;

	private Label lblTitle;

	private PictureBox picCustomer;

	private Label lblMessage;

	private Panel panel1;

	protected override bool ShowWithoutActivation => true;

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((Form)this).CreateParams;
			createParams.ExStyle |= 8;
			return createParams;
		}
	}

	public NotificationAggregator Instance { get; private set; }

	public new bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return base.IsDisposed;
			}
			return true;
		}
	}

	public string Title => _title;

	public string Message => _message;

	public ReturnedState Status => _status;

	public int Duration { get; private set; }

	public Task UpdateMessageAsync(string title, string message, ReturnedState icon, int duration = 10000, CancellationToken token = default(CancellationToken))
	{
		return UpdateMessageAsync(title, message, icon, duration, token, _clickAction, _doubleClickAction);
	}

	public async Task UpdateMessageAsync(string title, string message, ReturnedState icon, int duration, CancellationToken cancellationToken, Action clickAction, Action doubleClickAction)
	{
		try
		{
			_autoDisposedToken?.Cancel();
			_autoDisposedToken = CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken[1] { cancellationToken });
			CancellationToken token = _autoDisposedToken.Token;
			token.ThrowIfCancellationRequested();
			StartAutoDisposeAsync(token, duration);
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
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
						((Control)this).Top = (int)SystemParameters.FullPrimaryScreenHeight - ((Control)this).Height;
						((Control)this).Left = (int)SystemParameters.FullPrimaryScreenWidth - ((Control)this).Width;
						((Control)lblTitle).Text = title;
						((Control)lblMessage).Text = message;
						((Control)lblDateTime).Text = $"{DateTime.Now.Hour}:{DateTime.Now.Minute}\r\n" + DateTime.Now.MiladiToCalture().Substring(2, 8);
						switch (icon)
						{
						case ReturnedState.Error:
							((Control)lblTitle).ForeColor = Color.DarkRed;
							picCustomer.Image = (Image)(object)FormCustom.Properties.Resources.connection_failed;
							break;
						case ReturnedState.Success:
							((Control)lblTitle).ForeColor = Color.DarkGreen;
							picCustomer.Image = (Image)(object)FormCustom.Properties.Resources.connection_succsess;
							break;
						case ReturnedState.Warning:
							((Control)lblTitle).ForeColor = Color.Brown;
							picCustomer.Image = (Image)(object)FormCustom.Properties.Resources.connetion_error;
							break;
						default:
							((Control)lblTitle).ForeColor = Color.DarkBlue;
							picCustomer.Image = (Image)(object)FormCustom.Properties.Resources.Info;
							break;
						}
					}
				}
				catch (OperationCanceledException)
				{
				}
				catch (Exception ex4)
				{
					WebErrorLog.Instance.StartLog(ex4, "", "UpdateMessageAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Notification\\FrmSimpleNotification.cs", 315);
				}
			});
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "UpdateMessageAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Notification\\FrmSimpleNotification.cs", 319);
		}
	}

	private async Task StartAutoDisposeAsync(CancellationToken token, int duration)
	{
		try
		{
			await Task.Delay(duration, token);
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
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
						((Component)(object)this).Dispose(disposing: true);
					}
				}
				catch (Exception ex3)
				{
					WebErrorLog.Instance.StartLog(ex3, "", "StartAutoDisposeAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Notification\\FrmSimpleNotification.cs", 339);
				}
			});
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "StartAutoDisposeAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Notification\\FrmSimpleNotification.cs", 343);
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (_isDisposed)
			{
				return;
			}
			_isDisposed = true;
			try
			{
				if (((Control)this).Visible)
				{
					((Form)this).Close();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Notification\\FrmSimpleNotification.cs", 353);
			}
			components?.Dispose();
			base.Dispose(disposing);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Notification\\FrmSimpleNotification.cs", 358);
		}
	}

	private async Task ChangeTitleAsync(CancellationToken token)
	{
		try
		{
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			while (!((Control)this).IsHandleCreated)
			{
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				await Task.Delay(500);
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
						((Control)lblTitle).Text = Title;
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "ChangeTitleAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Notification\\FrmSimpleNotification.cs", 379);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChangeTitleAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Notification\\FrmSimpleNotification.cs", 382);
		}
	}

	private async Task ChangeMessageAsync(CancellationToken token)
	{
		try
		{
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			while (!((Control)this).IsHandleCreated)
			{
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				await Task.Delay(500);
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
						((Control)lblMessage).Text = Message;
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "ChangeMessageAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Notification\\FrmSimpleNotification.cs", 404);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChangeMessageAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Notification\\FrmSimpleNotification.cs", 407);
		}
	}

	private async Task ChangeStatusAsync(CancellationToken token)
	{
		try
		{
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			while (!((Control)this).IsHandleCreated)
			{
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				await Task.Delay(500);
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
						switch (Status)
						{
						case ReturnedState.Error:
							((Control)lblTitle).ForeColor = Color.DarkRed;
							picCustomer.Image = (Image)(object)FormCustom.Properties.Resources.connection_failed;
							break;
						case ReturnedState.Success:
							((Control)lblTitle).ForeColor = Color.DarkGreen;
							picCustomer.Image = (Image)(object)FormCustom.Properties.Resources.connection_succsess;
							break;
						case ReturnedState.Warning:
							((Control)lblTitle).ForeColor = Color.Brown;
							picCustomer.Image = (Image)(object)FormCustom.Properties.Resources.connetion_error;
							break;
						default:
							((Control)lblTitle).ForeColor = Color.DarkBlue;
							picCustomer.Image = (Image)(object)FormCustom.Properties.Resources.Info;
							break;
						}
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Notification\\FrmSimpleNotification.cs", 448);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Notification\\FrmSimpleNotification.cs", 451);
		}
	}

	public FrmSimpleNotification(string contextGuid, string title, string message, ReturnedState icon, int duration = 60000, CancellationToken token = default(CancellationToken), Action clickAction = null, Action doubleClickAction = null)
		: base(contextGuid)
	{
		try
		{
			InitializeComponent();
			picCustomer.Image = null;
			((Form)this).ShowInTaskbar = false;
			UpdateMessageAsync(title, message, icon, duration, token, clickAction, doubleClickAction);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Notification\\FrmSimpleNotification.cs", 465);
		}
	}

	internal FrmSimpleNotification(string contextGuid)
		: base(contextGuid)
	{
		try
		{
			InitializeComponent();
			picCustomer.Image = null;
			((Form)this).ShowInTaskbar = false;
			Instance = new NotificationAggregator(SimpleNotificationFactory.GetInstance(contextGuid));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Notification\\FrmSimpleNotification.cs", 478);
		}
	}

	private void control_Click(object sender, EventArgs e)
	{
		try
		{
			if (_clickAction != null)
			{
				_clickAction();
				return;
			}
			((Form)this).Close();
			((Component)(object)this).Dispose(disposing: true);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "control_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Notification\\FrmSimpleNotification.cs", 493);
		}
	}

	private void control_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (_clickAction != null)
			{
				_doubleClickAction();
				return;
			}
			((Form)this).Close();
			((Component)(object)this).Dispose(disposing: true);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "control_DoubleClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Notification\\FrmSimpleNotification.cs", 507);
		}
	}

	private void FrmSimpleNotification_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	private void FrmSimpleNotification_KeyDown(object sender, KeyEventArgs e)
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
			WebErrorLog.Instance.StartLog(ex, "", "FrmSimpleNotification_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Notification\\FrmSimpleNotification.cs", 517);
		}
	}

	private void FrmSimpleNotification_Load(object sender, EventArgs e)
	{
		try
		{
			GroupPanel groupPanel = groupPanel1;
			Label obj = lblDateTime;
			Label obj2 = lblTitle;
			PictureBox obj3 = picCustomer;
			Label obj4 = lblMessage;
			bool flag2 = (panel1.TabStop = true);
			bool flag4 = (obj4.TabStop = flag2);
			bool flag6 = (obj3.TabStop = flag4);
			bool flag8 = (obj2.TabStop = flag6);
			bool tabStop = (obj.TabStop = flag8);
			((Panel)groupPanel).TabStop = tabStop;
			((Control)this).Top = (int)SystemParameters.FullPrimaryScreenHeight - ((Control)this).Height;
			((Control)this).Left = (int)SystemParameters.FullPrimaryScreenWidth - ((Control)this).Width;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FrmSimpleNotification_Load", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Notification\\FrmSimpleNotification.cs", 529);
		}
	}

	private void Control_MouseDown(object sender, MouseEventArgs e)
	{
		try
		{
			firstPoint = e.Location;
			mouseIsDown = true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Control_MouseDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Notification\\FrmSimpleNotification.cs", 539);
		}
	}

	private void Control_MouseUp(object sender, MouseEventArgs e)
	{
		try
		{
			mouseIsDown = false;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Control_MouseUp", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Notification\\FrmSimpleNotification.cs", 544);
		}
	}

	private void FrmSimpleNotification_MouseMove(object sender, MouseEventArgs e)
	{
		try
		{
			if (mouseIsDown)
			{
				int num = firstPoint.X - e.Location.X;
				int num2 = firstPoint.Y - e.Location.Y;
				int x = ((Form)this).Location.X - num;
				int y = ((Form)this).Location.Y - num2;
				((Form)this).Location = new Point(x, y);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FrmSimpleNotification_MouseMove", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Notification\\FrmSimpleNotification.cs", 562);
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
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Expected O, but got Unknown
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d1: Expected O, but got Unknown
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Expected O, but got Unknown
		//IL_06b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06be: Expected O, but got Unknown
		//IL_06c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d0: Expected O, but got Unknown
		//IL_06d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e2: Expected O, but got Unknown
		//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f4: Expected O, but got Unknown
		groupPanel1 = new GroupPanel();
		lblDateTime = new Label();
		lblTitle = new Label();
		picCustomer = new PictureBox();
		lblMessage = new Label();
		panel1 = new Panel();
		((Control)groupPanel1).SuspendLayout();
		((ISupportInitialize)picCustomer).BeginInit();
		((Control)panel1).SuspendLayout();
		((Control)this).SuspendLayout();
		((ScrollableControl)groupPanel1).AutoScroll = true;
		((Control)groupPanel1).BackColor = Color.Transparent;
		((Control)groupPanel1).Controls.Add((Control)(object)lblTitle);
		((Control)groupPanel1).Controls.Add((Control)(object)picCustomer);
		((Control)groupPanel1).Controls.Add((Control)(object)lblMessage);
		((Control)groupPanel1).Controls.Add((Control)(object)lblDateTime);
		groupPanel1.DisabledBackColor = Color.Empty;
		((Control)groupPanel1).Dock = (DockStyle)5;
		((Control)groupPanel1).Location = new Point(0, 0);
		((Control)groupPanel1).Name = "groupPanel1";
		((Control)groupPanel1).Padding = new Padding(10);
		((Control)groupPanel1).Size = new Size(435, 88);
		groupPanel1.Style.BackColor = Color.FromArgb(148, 160, 232);
		groupPanel1.Style.BackColor2 = Color.FromArgb(148, 160, 232);
		groupPanel1.Style.BorderBottom = eStyleBorderType.Solid;
		groupPanel1.Style.BorderColor = Color.White;
		groupPanel1.Style.BorderColor2 = Color.White;
		groupPanel1.Style.BorderLeft = eStyleBorderType.Solid;
		groupPanel1.Style.BorderRight = eStyleBorderType.Solid;
		groupPanel1.Style.BorderTop = eStyleBorderType.Solid;
		groupPanel1.Style.CornerDiameter = 15;
		groupPanel1.Style.CornerType = eCornerType.Rounded;
		groupPanel1.StyleMouseDown.CornerType = eCornerType.Square;
		groupPanel1.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)groupPanel1).TabIndex = 21;
		((Control)lblDateTime).AllowDrop = true;
		((Control)lblDateTime).Anchor = (AnchorStyles)13;
		((Control)lblDateTime).BackColor = Color.Transparent;
		((Control)lblDateTime).ForeColor = Color.DarkBlue;
		((Control)lblDateTime).Location = new Point(372, 58);
		((Control)lblDateTime).Name = "lblDateTime";
		((Control)lblDateTime).Size = new Size(59, 29);
		((Control)lblDateTime).TabIndex = 9;
		((Control)lblDateTime).Text = "-";
		lblDateTime.TextAlign = (ContentAlignment)32;
		((Control)lblDateTime).Click += control_Click;
		((Control)lblDateTime).DoubleClick += control_DoubleClick;
		((Control)lblTitle).Anchor = (AnchorStyles)13;
		((Control)lblTitle).Cursor = Cursors.Hand;
		((Control)lblTitle).Font = new Font("B Yekan", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)lblTitle).ForeColor = Color.White;
		((Control)lblTitle).Location = new Point(0, 0);
		((Control)lblTitle).Name = "lblTitle";
		((Control)lblTitle).Size = new Size(366, 28);
		((Control)lblTitle).TabIndex = 7;
		lblTitle.TextAlign = (ContentAlignment)16;
		((Control)lblTitle).Click += control_Click;
		((Control)lblTitle).DoubleClick += control_DoubleClick;
		((Control)picCustomer).Cursor = Cursors.Hand;
		picCustomer.Image = (Image)(object)FormCustom.Properties.Resources.Error;
		((Control)picCustomer).Location = new Point(372, 12);
		((Control)picCustomer).Name = "picCustomer";
		((Control)picCustomer).Size = new Size(59, 49);
		picCustomer.SizeMode = (PictureBoxSizeMode)1;
		picCustomer.TabIndex = 6;
		picCustomer.TabStop = false;
		((Control)picCustomer).Click += control_Click;
		((Control)picCustomer).DoubleClick += control_DoubleClick;
		((Control)lblMessage).Anchor = (AnchorStyles)13;
		((Control)lblMessage).Cursor = Cursors.Hand;
		((Control)lblMessage).Font = new Font("B Traffic", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblMessage).ForeColor = Color.White;
		((Control)lblMessage).Location = new Point(3, 28);
		((Control)lblMessage).Name = "lblMessage";
		((Control)lblMessage).Size = new Size(364, 60);
		((Control)lblMessage).TabIndex = 8;
		lblMessage.TextAlign = (ContentAlignment)16;
		((Control)lblMessage).Click += control_Click;
		((Control)lblMessage).DoubleClick += control_DoubleClick;
		((Control)panel1).BackColor = Color.Gainsboro;
		((Control)panel1).Controls.Add((Control)(object)groupPanel1);
		((Control)panel1).Dock = (DockStyle)5;
		((Control)panel1).Location = new Point(0, 0);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(435, 88);
		((Control)panel1).TabIndex = 22;
		((Control)this).AllowDrop = true;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Black;
		((Form)this).ClientSize = new Size(435, 88);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FrmSimpleNotification";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Form)this).ShowInTaskbar = false;
		((Control)this).Text = "FrmSimpleNotification";
		((Form)this).TransparencyKey = Color.Gainsboro;
		((Form)this).FormClosed += new FormClosedEventHandler(FrmSimpleNotification_FormClosed);
		((Form)this).Load += FrmSimpleNotification_Load;
		((Control)this).DoubleClick += control_DoubleClick;
		((Control)this).KeyDown += new KeyEventHandler(FrmSimpleNotification_KeyDown);
		((Control)this).MouseDown += new MouseEventHandler(Control_MouseDown);
		((Control)this).MouseMove += new MouseEventHandler(FrmSimpleNotification_MouseMove);
		((Control)this).MouseUp += new MouseEventHandler(Control_MouseUp);
		((Control)groupPanel1).ResumeLayout(false);
		((ISupportInitialize)picCustomer).EndInit();
		((Control)panel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
