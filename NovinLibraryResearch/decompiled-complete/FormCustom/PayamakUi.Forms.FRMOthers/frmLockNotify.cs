using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EntityCache.LockService;
using FormCustom;
using FormCustom.Properties;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace PayamakUi.Forms.FRMOthers;

public class frmLockNotify : AccountingBaseForm
{
	private static Dictionary<string, frmLockNotify> _intance = new Dictionary<string, frmLockNotify>();

	private bool _isDisposed;

	private IContainer components;

	private NotifyIcon notifyIcon1;

	internal ContextMenuStrip ContextMenuStrip1;

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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmLockNotify.cs", 30);
		}
	}

	public static frmLockNotify GetInstance(string contextGuid)
	{
		frmLockNotify value = null;
		try
		{
			if (!_intance.TryGetValue(contextGuid, out value))
			{
				value = new frmLockNotify(contextGuid);
				_intance.Add(contextGuid, value);
			}
			else if (value == null || value.IsDisposed)
			{
				_intance = new Dictionary<string, frmLockNotify>();
				_intance.Remove(contextGuid);
				_intance.Add(contextGuid, value);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInstance", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmLockNotify.cs", 49);
		}
		return value;
	}

	private frmLockNotify(string contextGuid)
		: base(contextGuid)
	{
		try
		{
			InitializeComponent();
			NpLockHubInstances.OnHubChanged += LkEnvironmentHub_Changed;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmLockNotify.cs", 60);
		}
	}

	private async Task LkEnvironmentHub_Changed(object sender, ConnectionEventArgs<HubConnectionState> e)
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
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_002c: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Expected I4, but got Unknown
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				try
				{
					HubConnectionState val = e.ConnectionData.First();
					switch ((int)val)
					{
					case 0:
						notifyIcon1.Icon = FormCustom.Properties.Resources.LockDisconnected;
						break;
					case 2:
						notifyIcon1.Icon = FormCustom.Properties.Resources.LockConnecting;
						break;
					case 1:
						notifyIcon1.Icon = FormCustom.Properties.Resources.LockConnected;
						break;
					case 3:
						notifyIcon1.Icon = FormCustom.Properties.Resources.LockConnecting;
						break;
					default:
						throw new NotImplementedException();
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "LkEnvironmentHub_Changed", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmLockNotify.cs", 89);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LkEnvironmentHub_Changed", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Forms\\frmLockNotify.cs", 92);
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
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(frmLockNotify));
		notifyIcon1 = new NotifyIcon(components);
		ContextMenuStrip1 = new ContextMenuStrip(components);
		((Control)this).SuspendLayout();
		notifyIcon1.ContextMenuStrip = ContextMenuStrip1;
		notifyIcon1.Text = " ارتباط با سرویس قفل نوین پرداز";
		notifyIcon1.Visible = true;
		((Control)ContextMenuStrip1).Name = "ContextMenuStrip1";
		((Control)ContextMenuStrip1).RightToLeft = (RightToLeft)1;
		((Control)ContextMenuStrip1).Size = new Size(61, 4);
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(10, 10);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Control)this).Name = "frmLockNotify";
		((Form)this).Opacity = 0.0;
		((Form)this).ShowInTaskbar = false;
		((Control)this).Text = "FrmScaleNotify";
		((Form)this).WindowState = (FormWindowState)1;
		((Control)this).ResumeLayout(false);
	}
}
