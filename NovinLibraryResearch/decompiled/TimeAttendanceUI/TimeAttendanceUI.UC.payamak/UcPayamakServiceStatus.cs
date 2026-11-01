using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientHub.AccountingHubs;
using FormCustom.Cache;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PayamakUi.Classes;
using Serilog;
using TimeAttendanceDAL.Utility;
using TimeAttendanceUI.Dictionary;
using TimeAttendanceUI.Properties;

namespace TimeAttendanceUI.UC.payamak;

public class UcPayamakServiceStatus : UserControl, IHasContext
{
	private bool _isDispose;

	private string _contextGuid;

	private CancellationTokenSource _tokenSourceReconnect = new CancellationTokenSource();

	private CacheBaseForm _owner;

	private IContainer components;

	private PictureBox btnStatus;

	private Panel panel1;

	public CacheBaseForm Owner
	{
		get
		{
			return _owner;
		}
		set
		{
			try
			{
				if (value != null)
				{
					_owner = value;
					BindingSource();
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex, "");
			}
		}
	}

	public string ContextGuid
	{
		get
		{
			return _contextGuid;
		}
		set
		{
			try
			{
				if (!string.IsNullOrEmpty(value))
				{
					_contextGuid = value;
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex, "");
			}
		}
	}

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

	public UcPayamakServiceStatus()
	{
		InitializeComponent();
	}

	private void BindingSource()
	{
		try
		{
			InitService(new CancellationTokenSource().Token);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDispose)
			{
				_isDispose = true;
				_tokenSourceReconnect?.Cancel();
				_tokenSourceReconnect?.Dispose();
				_tokenSourceReconnect = null;
				components?.Dispose();
				((ContainerControl)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task InitService(CancellationToken token)
	{
		try
		{
			if (!token.IsCancellationRequested)
			{
				await PayamakServiceContainter.InitializeServiceAsync(ContextGuid, Owner);
				PayamakClientHubInstances.OnHubChanged += PayamakClientHubInstances_OnHubChanged;
				Utility.MessageBoxShowByNotif(showPopUp: false, (NotificationEnum)1, "تنظیمات جدید سرویس پیامک اعمال شد", "", 3);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task PayamakClientHubInstances_OnHubChanged(object sender, ConnectionEventArgs<HubConnectionState> e)
	{
		try
		{
			_tokenSourceReconnect?.Cancel();
			_tokenSourceReconnect = new CancellationTokenSource();
			CancellationToken token = _tokenSourceReconnect.Token;
			await Task.Delay(3000, token);
			HubConnectionState val = e.ConnectionData.First();
			switch ((int)val)
			{
			case 1:
				btnStatus.Image = (Image)(object)Resources.PayamakConnected;
				break;
			case 0:
				btnStatus.Image = (Image)(object)Resources.PayamakDisconnected;
				break;
			case 2:
				btnStatus.Image = (Image)(object)Resources.PayamakConnecting;
				break;
			case 3:
				btnStatus.Image = (Image)(object)Resources.PayamakUnknown;
				break;
			default:
				btnStatus.Image = (Image)(object)Resources.PayamakUnknown;
				break;
			}
		}
		catch (TaskCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
		}
	}

	internal async void UnInit()
	{
		try
		{
			PayamakClientHubInstances instance = PayamakClientHubInstances.GetInstance(ContextGuid);
			if (instance != null)
			{
				await instance.DisposeAsync();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		btnStatus = new PictureBox();
		panel1 = new Panel();
		((ISupportInitialize)btnStatus).BeginInit();
		((Control)panel1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)btnStatus).AccessibleRole = (AccessibleRole)43;
		btnStatus.Image = (Image)(object)ResourceUI.PayamakUnknown;
		((Control)btnStatus).Location = new Point(-1, -1);
		((Control)btnStatus).Name = "btnStatus";
		((Control)btnStatus).Size = new Size(27, 25);
		btnStatus.SizeMode = (PictureBoxSizeMode)4;
		btnStatus.TabIndex = 3;
		btnStatus.TabStop = false;
		((Control)panel1).Controls.Add((Control)(object)btnStatus);
		((Control)panel1).Location = new Point(0, 0);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(28, 27);
		((Control)panel1).TabIndex = 2;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((UserControl)this).BorderStyle = (BorderStyle)1;
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Name = "UcPayamakServiceStatus";
		((Control)this).Size = new Size(27, 26);
		((ISupportInitialize)btnStatus).EndInit();
		((Control)panel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
