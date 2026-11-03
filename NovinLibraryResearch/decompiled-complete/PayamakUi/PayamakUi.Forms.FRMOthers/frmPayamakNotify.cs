using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientHub.AccountingHubs;
using FormCustom.Cache;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace PayamakUi.Forms.FRMOthers;

public class frmPayamakNotify : CacheBaseForm
{
	private static Dictionary<string, frmPayamakNotify> _intance = new Dictionary<string, frmPayamakNotify>();

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

	public static frmPayamakNotify GetInstance(string contextGuid)
	{
		frmPayamakNotify value = null;
		try
		{
			if (!_intance.TryGetValue(contextGuid, out value))
			{
				value = new frmPayamakNotify(contextGuid);
				_intance.Add(contextGuid, value);
			}
			else if (value == null || value.IsDisposed)
			{
				_intance = new Dictionary<string, frmPayamakNotify>();
				_intance.Remove(contextGuid);
				_intance.Add(contextGuid, value);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInstance", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOthers\\frmPayamakNotify.cs", 40);
		}
		return value;
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOthers\\frmPayamakNotify.cs", 52);
		}
	}

	private frmPayamakNotify(string contextGuid)
		: base(contextGuid)
	{
		try
		{
			InitializeComponent();
			PayamakClientHubInstances.OnHubChanged += PayamakClientHubInstances_OnHubChanged;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOthers\\frmPayamakNotify.cs", 62);
		}
	}

	private Task PayamakClientHubInstances_OnHubChanged(object sender, ConnectionEventArgs<HubConnectionState> e)
	{
		try
		{
			CancellationToken token = new CancellationTokenSource(60000).Token;
			if (IsDisposed || token.IsCancellationRequested)
			{
				return Task.CompletedTask;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "PayamakClientHubInstances_OnHubChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOthers\\frmPayamakNotify.cs", 81);
		}
		return Task.CompletedTask;
	}

	private async void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
	{
		try
		{
			await new FrmPayamakMain(base.ContextGuid).ShowAsync((IWin32Window)(object)this);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "notifyIcon1_MouseDoubleClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Forms\\FRMOthers\\frmPayamakNotify.cs", 91);
		}
	}

	private void InitializeComponent()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		components = new Container();
		notifyIcon1 = new NotifyIcon(components);
		ContextMenuStrip1 = new ContextMenuStrip(components);
		((Control)this).SuspendLayout();
		notifyIcon1.ContextMenuStrip = ContextMenuStrip1;
		notifyIcon1.Text = "[نرم افزار جامع ارتباط با سامانه پیامک نوین پرداز]";
		notifyIcon1.Visible = true;
		notifyIcon1.MouseDoubleClick += new MouseEventHandler(notifyIcon1_MouseDoubleClick);
		((Control)ContextMenuStrip1).Name = "ContextMenuStrip1";
		((Control)ContextMenuStrip1).RightToLeft = (RightToLeft)1;
		((Control)ContextMenuStrip1).Size = new Size(61, 4);
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(10, 10);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).Name = "frmPayamakNotify";
		((Form)this).Opacity = 0.0;
		((Form)this).ShowInTaskbar = false;
		((Control)this).Text = "FrmScaleNotify";
		((Form)this).WindowState = (FormWindowState)1;
		((Control)this).ResumeLayout(false);
	}
}
