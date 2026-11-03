using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom.Properties;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace FormCustom.Waiting;

public class WaiterUserControl : UserControl, IWaiterUi, INpDisposable, IDisposable
{
	private bool _isDisposed;

	private readonly Control _parent;

	private readonly string _callerMemberName = "";

	private readonly string _callerFilePath = "";

	private IContainer components;

	private Label label1;

	private PictureBox pictureBox1;

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((Control)this).IsDisposed;
			}
			return true;
		}
	}

	public event Func<object, EventArgs, Task> OnAutoClose;

	public void Show()
	{
		((Control)this).Show();
	}

	public WaiterUserControl(Control parent, string labelProgress, Task task, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
	{
		WaiterUserControl waiterUserControl = this;
		try
		{
			_parent = parent;
			_callerMemberName = memberName;
			_callerFilePath = sourceFilePath;
			InitializeComponent();
			((Control)(object)this).Transelate();
			((Control)this).BackColor = Color.Transparent;
			((Control)label1).Text = labelProgress;
			double num = 1.0;
			if (((Control)this).Width > parent.Width || ((Control)this).Height > parent.Height)
			{
				int num2 = parent.Width - ((Control)this).Width;
				num = ((parent.Height - ((Control)this).Height < num2) ? ((double)(parent.Height - 50) / (double)((Control)this).Height) : ((double)(parent.Width - 50) / (double)((Control)this).Width));
			}
			((Control)this).Width = (int)((double)((Control)this).Width * num);
			((Control)this).Height = (int)((double)((Control)this).Height * num);
			((ContainerControl)this).AutoScaleDimensions = new SizeF(0f, 0f);
			((Control)this).BackColor = Color.Transparent;
			((Control)this).Left = (parent.Width - ((Control)this).Width) / 2;
			((Control)this).Top = (parent.Height - ((Control)this).Height) / 2;
			parent.Controls.Add((Control)(object)this);
			((Control)this).BringToFront();
			if (task != null)
			{
				Task.Run(() => waiterUserControl.CheckAutoStopAsync(task));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "memberName is " + _callerMemberName + " and sourceFilePath =" + _callerFilePath, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Waiting\\WaiterUserControl.cs", 59);
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
				await PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnAutoClose, this, EventArgs.Empty, default(CancellationToken), 60, "CheckAutoStopAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Waiting\\WaiterUserControl.cs");
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "memberName is " + _callerMemberName + " and sourceFilePath =" + _callerFilePath, "CheckAutoStopAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Waiting\\WaiterUserControl.cs", 71);
		}
	}

	public async Task HideAsync()
	{
		try
		{
			CancellationToken token = new CancellationTokenSource(60000).Token;
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
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "memberName is " + _callerMemberName + " and sourceFilePath =" + _callerFilePath, "HideAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Waiting\\WaiterUserControl.cs", 92);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "memberName is " + _callerMemberName + " and sourceFilePath =" + _callerFilePath, "HideAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Waiting\\WaiterUserControl.cs", 95);
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				pictureBox1.Image = null;
				((Control)this).Hide();
				_parent.Controls.Remove((Control)(object)this);
				components?.Dispose();
				((Component)this).Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Waiting\\WaiterUserControl.cs", 114);
		}
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		label1 = new Label();
		pictureBox1 = new PictureBox();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)this).SuspendLayout();
		((Control)label1).Anchor = (AnchorStyles)15;
		((Control)label1).BackColor = Color.Transparent;
		((Control)label1).Font = new Font("Tahoma", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label1).Location = new Point(0, 0);
		((Control)label1).Name = "label1";
		((Control)label1).RightToLeft = (RightToLeft)1;
		((Control)label1).Size = new Size(246, 103);
		((Control)label1).TabIndex = 0;
		((Control)label1).Text = "[در حال پردازش دیتابیس هستید...]";
		label1.TextAlign = (ContentAlignment)32;
		((Control)pictureBox1).Anchor = (AnchorStyles)11;
		((Control)pictureBox1).BackColor = Color.Transparent;
		pictureBox1.Image = (Image)(object)FormCustom.Properties.Resources.blueWaiter;
		((Control)pictureBox1).Location = new Point(204, -25);
		((Control)pictureBox1).Margin = new Padding(0);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(197, 150);
		pictureBox1.SizeMode = (PictureBoxSizeMode)1;
		pictureBox1.TabIndex = 1;
		pictureBox1.TabStop = false;
		((Control)this).BackgroundImage = (Image)(object)FormCustom.Properties.Resources.Loading_backGround;
		((Control)this).BackgroundImageLayout = (ImageLayout)3;
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Controls.Add((Control)(object)pictureBox1);
		((Control)this).DoubleBuffered = true;
		((Control)this).Name = "WaiterUserControl";
		((Control)this).Size = new Size(369, 103);
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
