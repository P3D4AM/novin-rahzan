using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom.Properties;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace FormCustom.Waiting;

public class WaiterUserControlRectangle : UserControl, IWaiterUi, INpDisposable, IDisposable
{
	private bool _isDisposed;

	private readonly Control _parent;

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

	public WaiterUserControlRectangle(Control parent, string labelProgress, Task task)
	{
		WaiterUserControlRectangle waiterUserControlRectangle = this;
		try
		{
			_parent = parent;
			InitializeComponent();
			((Control)(object)this).Transelate();
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
				Task.Run(() => waiterUserControlRectangle.CheckAutoStopAsync(task));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Waiting\\WaiterUserControlRectangle.cs", 51);
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
				await PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnAutoClose, this, EventArgs.Empty, default(CancellationToken), 60, "CheckAutoStopAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Waiting\\WaiterUserControlRectangle.cs");
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "CheckAutoStopAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Waiting\\WaiterUserControlRectangle.cs", 63);
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
					((Component)this).Dispose();
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "HideAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Waiting\\WaiterUserControlRectangle.cs", 83);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "HideAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Waiting\\WaiterUserControlRectangle.cs", 86);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Waiting\\WaiterUserControlRectangle.cs", 105);
		}
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		label1 = new Label();
		pictureBox1 = new PictureBox();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)this).SuspendLayout();
		((Control)label1).BackColor = Color.Transparent;
		((Control)label1).Font = new Font("Tahoma", 12f, (FontStyle)1, (GraphicsUnit)2);
		((Control)label1).ForeColor = Color.Maroon;
		((Control)label1).Location = new Point(0, 0);
		((Control)label1).Name = "label1";
		((Control)label1).RightToLeft = (RightToLeft)1;
		((Control)label1).Size = new Size(220, 66);
		((Control)label1).TabIndex = 0;
		((Control)label1).Text = "[در حال پردازش]";
		label1.TextAlign = (ContentAlignment)32;
		((Control)pictureBox1).Anchor = (AnchorStyles)9;
		((Control)pictureBox1).BackColor = Color.Transparent;
		pictureBox1.Image = (Image)(object)FormCustom.Properties.Resources.DoubleRing;
		((Control)pictureBox1).Location = new Point(223, 0);
		((Control)pictureBox1).Margin = new Padding(0);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(72, 66);
		pictureBox1.SizeMode = (PictureBoxSizeMode)4;
		pictureBox1.TabIndex = 1;
		pictureBox1.TabStop = false;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).BackgroundImageLayout = (ImageLayout)3;
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Controls.Add((Control)(object)pictureBox1);
		((Control)this).DoubleBuffered = true;
		((Control)this).Name = "WaiterUserControlRectangle";
		((Control)this).Size = new Size(295, 66);
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
