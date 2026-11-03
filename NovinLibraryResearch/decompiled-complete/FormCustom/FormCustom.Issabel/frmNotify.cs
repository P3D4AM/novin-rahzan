using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientHub;
using FormCustom.Cache;
using FormCustom.Properties;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace FormCustom.Issabel;

public class frmNotify : CacheBaseForm
{
	private bool _isDisposed;

	private static Dictionary<string, frmNotify> _instance = new Dictionary<string, frmNotify>();

	private IContainer components;

	private NotifyIcon notifyIcon;

	private NotifyIcon notifyIcon1;

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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Issabel\\frmNotify.cs", 30);
		}
	}

	public static frmNotify GetInstance(string contextGuid, IssabelClient client)
	{
		frmNotify value = null;
		try
		{
			if (!_instance.TryGetValue(contextGuid, out value))
			{
				value = new frmNotify(contextGuid, client);
				_instance.Add(contextGuid, value);
			}
			else if (value == null || value.IsDisposed)
			{
				value = new frmNotify(contextGuid, client);
				_instance.Remove(contextGuid);
				_instance.Add(contextGuid, value);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInstance", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Issabel\\frmNotify.cs", 50);
		}
		return value;
	}

	private frmNotify(string contextGuid, IssabelClient client)
		: base(contextGuid)
	{
		try
		{
			InitializeComponent();
			if (client != null)
			{
				client.Changed += Client_Changed;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Issabel\\frmNotify.cs", 60);
		}
	}

	private async Task Client_Changed(object sender, ConnectionEventArgs<HubConnectionState> e)
	{
		try
		{
			if (IsDisposed)
			{
				return;
			}
			((Control)this).Invoke((Delegate)(Action)delegate
			{
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0025: Invalid comparison between Unknown and I4
				if (IsDisposed)
				{
					return;
				}
				try
				{
					HubConnectionState val = e.ConnectionData.First();
					if ((int)val != 0)
					{
						if ((int)val == 1)
						{
							notifyIcon.Icon = FormCustom.Properties.Resources.np_call_center_Connect;
						}
					}
					else
					{
						notifyIcon.Icon = FormCustom.Properties.Resources.np_call_center_Disconnect;
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "Client_Changed", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Issabel\\frmNotify.cs", 83);
				}
			});
			((Form)this).DialogResult = (DialogResult)1;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Client_Changed", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Issabel\\frmNotify.cs", 87);
		}
	}

	private void frmNotify_FormClosed(object sender, FormClosedEventArgs e)
	{
		((Component)(object)this).Dispose(disposing: true);
	}

	private void InitializeComponent()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		components = new Container();
		notifyIcon = new NotifyIcon(components);
		notifyIcon1 = new NotifyIcon(components);
		((Control)this).SuspendLayout();
		notifyIcon.Text = "سرویس ایزابل";
		notifyIcon.Visible = true;
		notifyIcon1.Text = "notifyIcon1";
		notifyIcon1.Visible = true;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(120, 57);
		((Control)this).Font = new Font("B Yekan", 9.75f);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "frmNotify";
		((Form)this).Opacity = 0.0;
		((Form)this).ShowInTaskbar = false;
		((Form)this).WindowState = (FormWindowState)1;
		((Form)this).FormClosed += new FormClosedEventHandler(frmNotify_FormClosed);
		((Control)this).ResumeLayout(false);
	}
}
