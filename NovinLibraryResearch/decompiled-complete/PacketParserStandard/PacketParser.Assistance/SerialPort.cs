using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;

namespace PacketParser.Assistance;

public class SerialPort : INpDisposable, IDisposable
{
	private SerialPort _port;

	private bool _Busy;

	private DateTime _LastBusyTime = DateTime.Now;

	private DateTime _LastRecivedDate = DateTime.Now;

	private bool _isDisposed;

	public string PortName { get; private set; }

	public int BaudRate { get; private set; }

	public bool LastPortOpenStatusResult { get; private set; }

	public string LastPortOpenStatusMessage { get; private set; }

	public bool PortOpened
	{
		get
		{
			SerialPort port = _port;
			if (port == null)
			{
				return false;
			}
			return port.IsOpen;
		}
	}

	public bool IsDisposed => _isDisposed;

	public event Func<object, Tuple<DateTime, string>, Task> DataRecived;

	public SerialPort(string portName, int baudRate, bool usePortSemaphoreForKeepIt = true)
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		try
		{
			PortName = portName.ToLower();
			BaudRate = baudRate;
			if ((from p in SerialPort.GetPortNames()
				select p.ToLower()).ToList().ToList().Contains(PortName))
			{
				_LastRecivedDate = DateTime.Now;
				_port = new SerialPort();
				_port.DataReceived += new SerialDataReceivedEventHandler(_port_DataReceived);
				_port.PortName = PortName;
				_port.BaudRate = BaudRate;
				_port.Encoding = Encoding.ASCII;
				_port.Parity = (Parity)0;
				_port.StopBits = (StopBits)1;
				_port.DataBits = 8;
				_port.ReadTimeout = 5000;
				_port.WriteTimeout = 5000;
				OpenPort();
				if (usePortSemaphoreForKeepIt)
				{
					Task.Run((Func<Task?>)PortSemaphoreAsync);
				}
			}
		}
		catch (Exception ex)
		{
			if (ex.Message.ToLower().Contains("is denied".ToLower()))
			{
				Log.Warning(ex, "");
			}
			else
			{
				WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\SerialPort.cs", 57);
			}
		}
	}

	private bool OpenPort()
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException("SerialPort");
		}
		if (_port == null)
		{
			throw new NullReferenceException("_port");
		}
		try
		{
			if (PortOpened)
			{
				return true;
			}
			_port.Open();
			_LastRecivedDate = DateTime.Now;
			LastPortOpenStatusMessage = "success";
			LastPortOpenStatusResult = true;
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OpenPort", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\SerialPort.cs", 77);
			LastPortOpenStatusMessage = ex.Message;
			LastPortOpenStatusResult = false;
			return false;
		}
	}

	private async Task PortSemaphoreAsync()
	{
		while (!IsDisposed)
		{
			try
			{
				try
				{
					if (Math.Abs(_LastRecivedDate.Subtract(DateTime.Now).TotalSeconds) > 30.0)
					{
						_port.Close();
					}
					if (PortOpened)
					{
						goto end_IL_0045;
					}
					if (!OpenPort())
					{
						await Task.Delay(30000);
					}
					goto end_IL_0037;
					end_IL_0045:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "PortSemaphoreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\SerialPort.cs", 102);
					await Task.Delay(30000);
					goto end_IL_0037;
				}
				end_IL_0037:;
			}
			finally
			{
				await Task.Delay(1000);
			}
		}
	}

	public virtual void Dispose()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			if (_isDisposed)
			{
				return;
			}
			_isDisposed = true;
			if (_port != null)
			{
				_port.DataReceived -= new SerialDataReceivedEventHandler(_port_DataReceived);
				if (_port.IsOpen)
				{
					_port.Close();
				}
				((Component)(object)_port)?.Dispose();
				_port = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\SerialPort.cs", 200);
		}
	}

	private void _port_DataReceived(object sender, SerialDataReceivedEventArgs e)
	{
		try
		{
			if (_Busy)
			{
				return;
			}
			_Busy = true;
			string text = "";
			if (_port != null && _port.IsOpen)
			{
				_Busy = true;
				string text2 = _port.ReadExisting();
				if (!string.IsNullOrEmpty(text2?.Trim()))
				{
					text += text2;
				}
				_Busy = false;
				text = text.Replace("?", "").FixString();
				_LastRecivedDate = DateTime.Now;
				Log.Verbose("");
				Utilities.RaiseEvent(this.DataRecived, this, new Tuple<DateTime, string>(DateTime.Now, text), default(CancellationToken), 60, "_port_DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\SerialPort.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "_port_DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\SerialPort.cs", 233);
		}
	}

	private void CheckBusyReset()
	{
		try
		{
			if (Math.Abs(_LastBusyTime.Subtract(DateTime.Now).Seconds) > 10)
			{
				_Busy = false;
				_LastBusyTime = DateTime.Now;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckBusyReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\SerialPort.cs", 247);
		}
	}

	public Task SendDataAsync(string data, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (string.IsNullOrEmpty(data))
			{
				return Task.CompletedTask;
			}
			_port.Write(data);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\SerialPort.cs", 257);
		}
		return Task.CompletedTask;
	}
}
