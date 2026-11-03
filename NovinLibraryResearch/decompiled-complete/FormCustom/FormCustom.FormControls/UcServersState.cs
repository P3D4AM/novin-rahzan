using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom.FormControls.ServerStatus;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace FormCustom.FormControls;

public class UcServersState : UserControl
{
	private bool _isDisposed;

	private CancellationTokenSource _loadDataCancellationTokenSource = new CancellationTokenSource();

	private List<UcRssFeedStatus> LstControl = new List<UcRssFeedStatus>();

	private IContainer components;

	private Panel PanelParent;

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

	public UcServersState()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	public void Start(string hostAddress = "https://rss.uptimerobot.com/u542196-325382a19d555f94057aab823ed97172", int interval = 60000)
	{
		_loadDataCancellationTokenSource?.Cancel();
		_loadDataCancellationTokenSource = new CancellationTokenSource(60000);
		CancellationToken tkn = _loadDataCancellationTokenSource.Token;
		Task.Run(() => RefreshUptimeMonitorAsync(hostAddress, interval, tkn));
	}

	public void Stop(string hostAddress = "https://rss.uptimerobot.com/u542196-325382a19d555f94057aab823ed97172", int interval = 60000)
	{
		_loadDataCancellationTokenSource?.Cancel();
	}

	private ReturnedSaveFuncInfo DisposeControl()
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			foreach (UcRssFeedStatus item in LstControl)
			{
				((Component)(object)item).Dispose();
			}
			LstControl = new List<UcRssFeedStatus>();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DisposeControl", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\FormControls\\ServerStatus\\UcServersState.cs", 48);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "DisposeControl", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\FormControls\\ServerStatus\\UcServersState.cs", 49);
		}
		return returnedSaveFuncInfo;
	}

	private ReturnedSaveFuncInfo CreateControl(CancellationToken token, List<RssServerStatus> Lstrss)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (token.IsCancellationRequested || IsDisposed)
			{
				return returnedSaveFuncInfo.AddErrorTokenIsCancelled("", "", "CreateControl", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\FormControls\\ServerStatus\\UcServersState.cs", 59);
			}
			int num = 0;
			foreach (RssServerStatus item in Lstrss)
			{
				if (token.IsCancellationRequested)
				{
					return returnedSaveFuncInfo.AddErrorTokenIsCancelled("", "", "CreateControl", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\FormControls\\ServerStatus\\UcServersState.cs", 64);
				}
				UcRssFeedStatus ucRssFeedStatus = new UcRssFeedStatus(item.IsUptime, item.link, item.description);
				((Control)ucRssFeedStatus).Dock = (DockStyle)1;
				((Control)ucRssFeedStatus).Width = ((Control)this).Width;
				((Control)ucRssFeedStatus).Name = "rss" + num;
				LstControl.Add(ucRssFeedStatus);
				((Control)PanelParent).Controls.Add((Control)(object)ucRssFeedStatus);
				num++;
			}
			return returnedSaveFuncInfo;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CreateControl", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\FormControls\\ServerStatus\\UcServersState.cs", 77);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CreateControl", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\FormControls\\ServerStatus\\UcServersState.cs", 78);
			return returnedSaveFuncInfo;
		}
	}

	private async Task RefreshUptimeMonitorAsync(string hostAddress, int interval, CancellationToken token)
	{
		_ = 1;
		try
		{
			while (!IsDisposed && !token.IsCancellationRequested)
			{
				await LoadDataAsync(hostAddress, token);
				await Task.Delay(interval);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RefreshUptimeMonitorAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\FormControls\\ServerStatus\\UcServersState.cs", 93);
		}
	}

	private async Task LoadDataAsync(string hostAddress, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(200);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			if (IsDisposed)
			{
				return;
			}
			ReturnedSaveFuncInfoWithValue<List<RssServerStatus>> retRss = await RssServerStatus.ReadRssContent(hostAddress, token);
			ret.AddReturnedValue(retRss, ReturnedState.None, "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\FormControls\\ServerStatus\\UcServersState.cs", 109);
			if (!retRss.HasError && !token.IsCancellationRequested)
			{
				((Control)this).Invoke((Delegate)(MethodInvoker)delegate
				{
					ret.AddReturnedValue(DisposeControl(), ReturnedState.None, "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\FormControls\\ServerStatus\\UcServersState.cs", 113);
					ret.AddReturnedValue(CreateControl(token, retRss.value), ReturnedState.None, "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\FormControls\\ServerStatus\\UcServersState.cs", 114);
				});
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\FormControls\\ServerStatus\\UcServersState.cs", 117);
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!IsDisposed)
			{
				_isDisposed = true;
				_loadDataCancellationTokenSource?.Cancel();
				_loadDataCancellationTokenSource = null;
				components?.Dispose();
				LstControl = null;
				((ContainerControl)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\FormControls\\ServerStatus\\UcServersState.cs", 138);
		}
	}

	private void InitializeComponent()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		components = new Container();
		PanelParent = new Panel();
		((Control)PanelParent).SuspendLayout();
		((Control)this).SuspendLayout();
		((ScrollableControl)PanelParent).AutoScroll = true;
		((Control)PanelParent).Dock = (DockStyle)5;
		((Control)PanelParent).Location = new Point(0, 0);
		((Control)PanelParent).Name = "PanelParent";
		((Control)PanelParent).Size = new Size(959, 470);
		((Control)PanelParent).TabIndex = 0;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((ScrollableControl)this).AutoScroll = true;
		((Control)this).Controls.Add((Control)(object)PanelParent);
		((Control)this).Name = "UcServersState";
		((Control)this).Size = new Size(959, 470);
		((Control)PanelParent).ResumeLayout(false);
		((Control)PanelParent).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
