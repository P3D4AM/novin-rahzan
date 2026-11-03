using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace PacketParser.EventArgs;

public class DateTimeManager : INpDisposable, IDisposable
{
	public delegate void DateTimeManagerEventHandler(object sender, DateTime lastDateTime);

	private Timer _timerDialed;

	private DateTime _serverDateTime = DateTime.Now;

	private DateTime _curDateTime;

	[XmlIgnore]
	[JsonIgnore]
	public bool IsDisposed { get; private set; }

	public event DateTimeManagerEventHandler OnDateChange;

	public event DateTimeManagerEventHandler OnMismatch;

	private async Task Instance_OnLoginResponse(object arg1, LoginResponseEventArgs arg2)
	{
		using (new JaegerMonitor("Instance_OnLoginResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\DateTimeManager.cs"))
		{
			try
			{
				_serverDateTime = arg2.UserInfo.CurrentServerTime;
				_curDateTime = arg2.UserInfo.CurrentServerTime;
				CheckDateTime();
				AutoResetEvent autoResetEvent = new AutoResetEvent(initialState: false);
				_timerDialed = new Timer(DateChange, autoResetEvent, 1000, 60000);
				autoResetEvent.WaitOne();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Instance_OnLoginResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\DateTimeManager.cs", 40);
			}
		}
	}

	private void CheckDateTime()
	{
		if (Math.Abs((_serverDateTime.ToUniversalTime() - DateTime.Now.ToUniversalTime()).Minutes) > 5)
		{
			this.OnMismatch?.Invoke(this, _curDateTime);
		}
	}

	private void DateChange(object stateInfo)
	{
		if (Math.Abs((_curDateTime.ToUniversalTime() - DateTime.Now.ToUniversalTime()).Days) > 0)
		{
			this.OnDateChange?.Invoke(this, _curDateTime);
		}
	}

	public void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				_timerDialed = null;
				if (this.OnDateChange != null)
				{
					this.OnDateChange = null;
				}
				if (this.OnMismatch != null)
				{
					this.OnMismatch = null;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\DateTimeManager.cs", 74);
		}
	}
}
