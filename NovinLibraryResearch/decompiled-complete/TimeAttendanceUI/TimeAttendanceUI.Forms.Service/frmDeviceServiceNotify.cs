using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientHub;
using FormCustom;
using PacketParser.Entities;
using PacketParser.EventArgs;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.Forms.Service;

public class frmDeviceServiceNotify : AccountingBaseForm
{
	private static Dictionary<string, frmDeviceServiceNotify> _intance = new Dictionary<string, frmDeviceServiceNotify>();

	private bool _isDisposed;

	private IContainer components;

	private NotifyIcon notifyIcon1;

	internal ContextMenuStrip ContextMenuStrip1;

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((BaseForm)this).IsDisposed;
			}
			return true;
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
				((BaseForm)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmDeviceServiceNotify.cs", 27);
		}
	}

	public static frmDeviceServiceNotify GetInstance(string contextGuid, Buffer buffer)
	{
		frmDeviceServiceNotify value = null;
		try
		{
			if (!_intance.TryGetValue(contextGuid, out value))
			{
				value = new frmDeviceServiceNotify(contextGuid, buffer);
				_intance.Add(contextGuid, value);
			}
			else if (value == null || value.IsDisposed)
			{
				_intance = new Dictionary<string, frmDeviceServiceNotify>();
				_intance.Remove(contextGuid);
				_intance.Add(contextGuid, value);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInstance", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmDeviceServiceNotify.cs", 46);
		}
		return value;
	}

	private frmDeviceServiceNotify(string contextGuid, Buffer buffer)
		: base(contextGuid)
	{
		frmDeviceServiceNotify frmDeviceServiceNotify2 = this;
		if (buffer == null)
		{
			throw new ArgumentNullException();
		}
		try
		{
			InitializeComponent();
			buffer.StateChanged += Buffer_StateChanged;
			Task.Run(() => frmDeviceServiceNotify2.Buffer_StateChanged(frmDeviceServiceNotify2, new BufferStateChangedEventArgs
			{
				Status = buffer.ConnectionState
			}));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmDeviceServiceNotify.cs", 60);
		}
	}

	private async Task Buffer_StateChanged(object sender, BufferStateChangedEventArgs e)
	{
		try
		{
			CancellationToken token = new CancellationTokenSource(60000).Token;
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
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_003e: Expected I4, but got Unknown
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				try
				{
					EnConnectionState status = e.Status;
					switch ((int)status)
					{
					case 3:
						notifyIcon1.Icon = Resources.D_red;
						break;
					case 0:
						notifyIcon1.Icon = Resources.D2_yellow;
						break;
					case 1:
						notifyIcon1.Icon = Resources.D_green;
						break;
					case 2:
						notifyIcon1.Icon = Resources.D2_yellow;
						break;
					default:
						throw new NotImplementedException();
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "Buffer_StateChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmDeviceServiceNotify.cs", 89);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Buffer_StateChanged", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\frmDeviceServiceNotify.cs", 92);
		}
	}

	private void InitializeComponent()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(frmDeviceServiceNotify));
		notifyIcon1 = new NotifyIcon(components);
		ContextMenuStrip1 = new ContextMenuStrip(components);
		((Control)this).SuspendLayout();
		notifyIcon1.ContextMenuStrip = ContextMenuStrip1;
		notifyIcon1.Text = "سرویس ارتباط با دستگاه های حضور و غیاب نوین پرداز";
		notifyIcon1.Visible = true;
		((Control)ContextMenuStrip1).Name = "ContextMenuStrip1";
		((Control)ContextMenuStrip1).RightToLeft = (RightToLeft)1;
		((Control)ContextMenuStrip1).Size = new Size(61, 4);
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(12, 10);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).Margin = new Padding(5, 3, 5, 3);
		((Control)this).Name = "frmDeviceServiceNotify";
		((Form)this).Opacity = 0.0;
		((Form)this).ShowInTaskbar = false;
		((Control)this).Text = "FrmScaleNotify";
		((Form)this).WindowState = (FormWindowState)1;
		((Control)this).ResumeLayout(false);
	}
}
