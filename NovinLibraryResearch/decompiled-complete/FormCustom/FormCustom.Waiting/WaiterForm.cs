using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom.Cache;
using FormCustom.Properties;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace FormCustom.Waiting;

public class WaiterForm : CacheBaseForm, IWaiterUi, INpDisposable, IDisposable
{
	private bool _isDisposed;

	private bool _isClosed;

	private readonly Form _parent;

	private IContainer components;

	private Label label1;

	private PictureBox pictureBox1;

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

	public event Func<object, EventArgs, Task> OnAutoClose;

	public WaiterForm(Form parent = null, string labelProgress = "در حال پردازش", Task task = null)
		: base(Guid.NewGuid().ToString())
	{
		WaiterForm waiterForm = this;
		_parent = parent;
		InitializeComponent();
		((Control)label1).Text = labelProgress;
		((Control)this).Text = labelProgress + " / " + ((parent != null) ? ((Control)parent).Text : null);
		((Control)this).BringToFront();
		((Form)this).StartPosition = (FormStartPosition)1;
		if (task != null)
		{
			Task.Run(() => waiterForm.CheckAutoStopAsync(task));
		}
	}

	private async Task CheckAutoStopAsync(Task task)
	{
		_ = 1;
		try
		{
			await task;
			if (this.OnAutoClose != null)
			{
				await PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnAutoClose, this, EventArgs.Empty, default(CancellationToken), 60, "CheckAutoStopAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Waiting\\WaiterForm.cs");
			}
			else
			{
				if (IsDisposed || !((Control)this).IsHandleCreated)
				{
					return;
				}
				((Control)this).BeginInvoke((Delegate)(Action)delegate
				{
					try
					{
						if (!IsDisposed)
						{
							((Component)(object)this).Dispose(disposing: true);
						}
					}
					catch (Exception ex3)
					{
						WebErrorLog.Instance.StartLog(ex3, "", "CheckAutoStopAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Waiting\\WaiterForm.cs", 48);
					}
				});
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "CheckAutoStopAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Waiting\\WaiterForm.cs", 52);
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				if (!_isClosed)
				{
					((Form)this).DialogResult = (DialogResult)2;
					((Form)this).Close();
				}
				components?.Dispose();
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Waiting\\WaiterForm.cs", 72);
		}
	}

	public async Task HideAsync()
	{
		try
		{
			CancellationToken token = new CancellationTokenSource(60000).Token;
			while (!((Control)this).IsHandleCreated)
			{
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				await Task.Delay(100);
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
						CloseLoadingForm();
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "HideAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Waiting\\WaiterForm.cs", 93);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "HideAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Waiting\\WaiterForm.cs", 96);
		}
	}

	public void Show()
	{
		((Form)this).Show((IWin32Window)(object)_parent);
	}

	private void CloseLoadingForm()
	{
		((Form)this).DialogResult = (DialogResult)1;
		((Form)this).Close();
	}

	private void WaiterForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		_isClosed = true;
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Expected O, but got Unknown
		label1 = new Label();
		pictureBox1 = new PictureBox();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)this).SuspendLayout();
		((Control)label1).Anchor = (AnchorStyles)13;
		((Control)label1).BackColor = Color.Transparent;
		((Control)label1).Font = new Font("Tahoma", 7f, (FontStyle)1);
		((Control)label1).ForeColor = Color.DarkRed;
		((Control)label1).Location = new Point(37, 35);
		((Control)label1).Name = "label1";
		((Control)label1).RightToLeft = (RightToLeft)1;
		((Control)label1).Size = new Size(47, 45);
		((Control)label1).TabIndex = 0;
		((Control)label1).Text = "[در حال پردازش]";
		label1.TextAlign = (ContentAlignment)32;
		((Control)pictureBox1).BackColor = Color.Transparent;
		((Control)pictureBox1).BackgroundImageLayout = (ImageLayout)4;
		((Control)pictureBox1).Dock = (DockStyle)5;
		pictureBox1.Image = (Image)(object)FormCustom.Properties.Resources.DoubleRing;
		((Control)pictureBox1).Location = new Point(0, 0);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(121, 114);
		pictureBox1.SizeMode = (PictureBoxSizeMode)4;
		pictureBox1.TabIndex = 1;
		pictureBox1.TabStop = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).BackgroundImage = (Image)(object)FormCustom.Properties.Resources.circle;
		((Control)this).BackgroundImageLayout = (ImageLayout)4;
		((Form)this).ClientSize = new Size(121, 114);
		((Form)this).ControlBox = false;
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Controls.Add((Control)(object)pictureBox1);
		((Control)this).Cursor = Cursors.No;
		((Control)this).DoubleBuffered = true;
		((Control)this).ForeColor = Color.Black;
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).Name = "WaiterForm";
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "WaitForm";
		((Form)this).TransparencyKey = Color.White;
		((Form)this).FormClosing += new FormClosingEventHandler(WaiterForm_FormClosing);
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
