using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;
using PacketParser.Entities;

namespace Tele;

public class NovinPhone
{
	public delegate void PlayingFinishedEventHandler(object sender);

	public delegate void ConnectEventHandler(object sender);

	public delegate void RingEventHandler(object sender, string number);

	public delegate void TerminalNewMessageEventHandler(object sender, string message);

	public delegate void EndCallEventHandler(object sender, string number);

	public delegate void ToneEventHandler(object sender, string tone);

	private readonly List<string> _reciveData = new List<string>();

	private readonly DateTime _lastReadError = DateTime.Now.AddDays(-5.0);

	public string LastTerminalMessage = "";

	private DateTime _lastDate = DateTime.Now;

	public Timer TResetConnection = new Timer();

	public Timer TReadLine = new Timer();

	private readonly SerialPort _serial = new SerialPort();

	private bool _readData = true;

	private readonly short _autoAnswerCount = 3;

	public string InputCharacter = "";

	public ENStatus Status { get; set; } = ENStatus.Dissconnectd;


	public bool StopPlaying { get; set; } = false;


	public bool SupportAtCommand { get; } = false;


	public string TerminalMessage { get; set; } = "";


	public string Model { get; } = "--";


	public string Port => _serial.PortName;

	public CurrentMode Mode { get; private set; } = CurrentMode.Command;


	public event PlayingFinishedEventHandler PlayingFinished;

	public event ConnectEventHandler ConnectCall;

	public event RingEventHandler Ring;

	public event TerminalNewMessageEventHandler TerminalNewMessage;

	public event EndCallEventHandler EndCall;

	public event ToneEventHandler Tone;

	public NovinPhone(string port, long speed, short dataBits, StopBits stopBits, Parity parity, Handshake handshake, short autoAnswerCount)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_serial.DataReceived += new SerialDataReceivedEventHandler(_Serial_DataReceived);
			_serial.BaudRate = (int)speed;
			_serial.PortName = port;
			_serial.DataBits = dataBits;
			_serial.StopBits = stopBits;
			_serial.Parity = parity;
			_serial.Handshake = handshake;
			_autoAnswerCount = autoAnswerCount;
			_serial.DtrEnable = true;
			_serial.WriteBufferSize = 1024;
			TResetConnection.Tick += t_Tick;
			TResetConnection.Interval = 1000;
			TResetConnection.Enabled = true;
			TResetConnection.Start();
			TReadLine.Tick += t_ReadLine_Tick;
			TReadLine.Interval = 100;
			TReadLine.Enabled = true;
			TReadLine.Start();
			Connect();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\Tele\\Tele\\NovinPhone.cs", 97);
		}
	}

	protected virtual void OnRing(string number)
	{
		try
		{
			_lastDate = DateTime.Now;
			this.Ring?.Invoke(this, "");
			Status = ENStatus.Ringing;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnRing", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\Tele\\Tele\\NovinPhone.cs", 113);
		}
	}

	protected virtual void OnPlayingFinished()
	{
		try
		{
			Status = ENStatus.PlayingVoiceFinished;
			_lastDate = DateTime.Now;
			this.PlayingFinished?.Invoke(this);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnPlayingFinished", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\Tele\\Tele\\NovinPhone.cs", 127);
		}
	}

	protected virtual void OnConnect()
	{
		try
		{
			Status = ENStatus.AnsweringCall;
			_lastDate = DateTime.Now;
			this.ConnectCall?.Invoke(this);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnConnect", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\Tele\\Tele\\NovinPhone.cs", 141);
		}
	}

	protected virtual void OnCallerID(string number)
	{
		try
		{
			Status = ENStatus.Ringing;
			_lastDate = DateTime.Now;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnCallerID", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\Tele\\Tele\\NovinPhone.cs", 158);
		}
	}

	protected virtual void OnEndCall(string number)
	{
		try
		{
			Disconnect();
			Connect();
			this.EndCall?.Invoke(this, "");
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnEndCall", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\Tele\\Tele\\NovinPhone.cs", 173);
		}
	}

	public void HookOff()
	{
		try
		{
			Disconnect();
			Connect();
			ActivateCallerID();
			AutoAnswerConfiguration();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "HookOff", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\Tele\\Tele\\NovinPhone.cs", 188);
		}
	}

	public void HookOn()
	{
		ExecCommnad("ATD");
	}

	protected virtual void OnTerminalNewMessage(string message)
	{
		try
		{
			_lastDate = DateTime.Now;
			this.TerminalNewMessage?.Invoke(this, message);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnTerminalNewMessage", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\Tele\\Tele\\NovinPhone.cs", 206);
		}
	}

	protected virtual void OnTone(string number)
	{
		try
		{
			InputCharacter = number;
			Application.DoEvents();
			Thread.Sleep(100);
			Application.DoEvents();
			_lastDate = DateTime.Now;
			this.Tone?.Invoke(this, number);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "OnTone", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\Tele\\Tele\\NovinPhone.cs", 225);
		}
	}

	private void _Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		SerialPort val = (SerialPort)sender;
		string aTRecivedCommand = val.ReadExisting().ToLower();
		ATReciveDataSpliter aTReciveDataSpliter = new ATReciveDataSpliter(aTRecivedCommand);
		foreach (string command in aTReciveDataSpliter.CommandList)
		{
			_reciveData.Add(command);
		}
		Application.DoEvents();
	}

	public bool TestAT()
	{
		return string2bool(ExecCommnad("at"));
	}

	private void SetTerminal_Message(string terminalMessage)
	{
		try
		{
			terminalMessage = terminalMessage.Replace("\r", "");
			terminalMessage = terminalMessage.Replace("\n", "");
			TerminalMessage = terminalMessage;
			if (!_readData)
			{
				return;
			}
			string text = terminalMessage.TrimStart(Array.Empty<char>()).TrimEnd(Array.Empty<char>());
			if (text == "*" || text == "\u0010*\u0010~" || text == "\u0010/\u0010*\u0010~")
			{
				OnTone("*");
				return;
			}
			if (text == "#" || text == "\u0010#\u0010~" || text == "\u0010/\u0010#\u0010~")
			{
				OnTone("#");
				return;
			}
			if (text == "0" || text == "\u00100\u0010~" || text == "\u0010/\u00100\u0010~")
			{
				OnTone("0");
				return;
			}
			if (text == "1" || text == "\u00101\u0010~" || text == "\u0010/\u00101\u0010~")
			{
				OnTone("1");
				return;
			}
			if (text == "2" || text == "\u00102\u0010~" || text == "\u0010/\u00102\u0010~")
			{
				OnTone("2");
				return;
			}
			if (text == "3" || text == "\u00103\u0010~" || text == "\u0010/\u00103\u0010~")
			{
				OnTone("3");
				return;
			}
			if (text == "4" || text == "\u00104\u0010~" || text == "\u0010/\u00104\u0010~")
			{
				OnTone("4");
				return;
			}
			if (text == "5" || text == "\u00105\u0010~" || text == "\u0010/\u00105\u0010~")
			{
				OnTone("5");
				return;
			}
			if (text == "6" || text == "\u00106\u0010~" || text == "\u0010/\u00106\u0010~")
			{
				OnTone("6");
				return;
			}
			if (text == "7" || text == "\u00107\u0010~" || text == "\u0010/\u00107\u0010~")
			{
				OnTone("7");
				return;
			}
			if (text == "8" || text == "\u00108\u0010~" || text == "\u0010/\u00108\u0010~")
			{
				OnTone("8");
				return;
			}
			if (text == "9" || text == "\u00109\u0010~" || text == "\u0010/\u00109\u0010~")
			{
				OnTone("9");
				return;
			}
			string inputNumber = ATCommandManager.GetInputNumber(text);
			if (inputNumber != "")
			{
				OnTone(inputNumber);
				if (text.Contains("*"))
				{
					OnTone("*");
				}
				if (text.Contains("#"))
				{
					OnTone("#");
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetTerminal_Message", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\Tele\\Tele\\NovinPhone.cs", 370);
		}
	}

	public void Call(string number)
	{
		try
		{
			ExecCommnad("ATZ");
			ActivateCallerID();
			Thread.Sleep(1000);
			Application.DoEvents();
			ExecCommnad("AT+FCLASS=8");
			Thread.Sleep(1000);
			Application.DoEvents();
			ExecCommnad("ATD" + number);
			Thread.Sleep(1000);
			Application.DoEvents();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Call", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\Tele\\Tele\\NovinPhone.cs", 391);
		}
	}

	public void Call_Flash(string number)
	{
		if (!string2bool(ExecCommnad("AT")))
		{
			ExecCommnad("AT+VTX");
		}
		else
		{
			ExecCommnad("ATD!" + number + ";H");
		}
	}

	public void HangUp()
	{
		try
		{
			Disconnect();
			Connect();
			ActivateCallerID();
			AutoAnswerConfiguration();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "HangUp", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\Tele\\Tele\\NovinPhone.cs", 415);
		}
	}

	public void ActivateCallerID()
	{
		ExecCommnad("ATZ");
		ExecCommnad("AT+VCID=1");
	}

	public void Play_Wave(string path)
	{
		try
		{
			if (Status == ENStatus.PlayingVoice || Status == ENStatus.Dissconnectd)
			{
				return;
			}
			StopPlaying = false;
			Status = ENStatus.PlayingVoice;
			_lastDate = DateTime.Now;
			VoiceMode();
			ExecCommnad("AT+FCLASS=8");
			ExecCommnad("AT+VSM9,8000");
			ExecCommnad("AT+VSD\u00128,0");
			ExecCommnad("AT+VTX");
			byte[] array = File.ReadAllBytes(path);
			_serial.DtrEnable = true;
			int num = 44;
			int num2 = 1024;
			while (true)
			{
				_lastDate = DateTime.Now;
				Thread.Sleep(80);
				Status = ENStatus.PlayingVoice;
				if (StopPlaying)
				{
					Status = ENStatus.PlayingVoiceFinished;
					StopPlaying = false;
					OnPlayingFinished();
					return;
				}
				try
				{
					_serial.Write(array, num, num2);
				}
				catch (Exception)
				{
				}
				if (num2 <= 0)
				{
					break;
				}
				num += num2;
				if (array.Length - num < 800)
				{
					num2 = array.Length - num;
				}
				Application.DoEvents();
			}
			ExecCommnad("AT+FCLASS=0");
			OnPlayingFinished();
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "Play_Wave", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\Tele\\Tele\\NovinPhone.cs", 493);
		}
	}

	private static bool string2bool(string input)
	{
		try
		{
			input = input.ToUpper().Replace("OK", "TRUE").Replace("ERROR", "FALSE");
			bool.TryParse(input, out var result);
			return result;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "string2bool", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\Tele\\Tele\\NovinPhone.cs", 507);
			return false;
		}
	}

	public string ReadLine()
	{
		try
		{
			if (_reciveData.Count == 0)
			{
				return "";
			}
			List<string> list = new List<string>();
			foreach (string reciveDatum in _reciveData)
			{
				list.Add(reciveDatum);
			}
			_reciveData.Clear();
			string text = "";
			foreach (string item in list)
			{
				string text2 = item.ToUpper();
				text = ((!(text.Trim() == "")) ? (text + "\r\n" + text2) : text2);
				if (text2.Contains("RING") || text2 == "R")
				{
					OnRing("");
				}
				if (text2.Contains("CONNECT"))
				{
					OnConnect();
				}
				switch (text2)
				{
				default:
					if (!(text2 == "BBBBB"))
					{
						continue;
					}
					break;
				case "B":
				case "BB":
				case "BBB":
				case "BBBB":
					break;
				}
				OnEndCall("");
			}
			if (LastTerminalMessage.Trim() == "")
			{
				LastTerminalMessage = text;
			}
			else
			{
				LastTerminalMessage = LastTerminalMessage + "\r\n" + text;
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ReadLine", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\Tele\\Tele\\NovinPhone.cs", 570);
			return null;
		}
	}

	public string ExecCommnad(string command)
	{
		try
		{
			if (LastTerminalMessage.Trim() == "")
			{
				LastTerminalMessage = command;
			}
			else
			{
				LastTerminalMessage = LastTerminalMessage + "\r\n" + command;
			}
			FileManager.Write(command);
			_lastDate = DateTime.Now;
			_serial.Write(command + "\r\n");
			Thread.Sleep(200);
			string result = "";
			_serial.ReadTimeout = 2000;
			if (!ATCommandManager.IsNoResponseCommand(command))
			{
				result = ReadLine();
			}
			OnTerminalNewMessage(command + "\r\n");
			return result;
		}
		catch (Exception ex)
		{
			if (ex.Message.Contains("port") && ex.Message.Contains("closed"))
			{
				Disconnect();
				Connect();
				ActivateCallerID();
				AutoAnswerConfiguration();
			}
			return "";
		}
	}

	public bool Connect()
	{
		try
		{
			Status = ENStatus.Ready;
			_lastDate = DateTime.Now;
			if (_serial.IsOpen)
			{
				return TestAT();
			}
			try
			{
				_serial.Open();
				ActivateCallerID();
				AutoAnswerConfiguration();
				return TestAT();
			}
			catch (Exception)
			{
				Status = ENStatus.Dissconnectd;
				return false;
			}
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "Connect", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\Tele\\Tele\\NovinPhone.cs", 648);
			return false;
		}
	}

	public bool AutoAnswerConfiguration()
	{
		try
		{
			bool flag = false;
			bool flag2 = false;
			if (_autoAnswerCount <= 0 || _autoAnswerCount >= 20)
			{
				return false;
			}
			for (int i = 0; i < 20; i++)
			{
				if (flag)
				{
					break;
				}
				string input = ExecCommnad("AT+FCLASS=8;");
				if (string2bool(input))
				{
					flag = true;
				}
				Thread.Sleep(1000);
			}
			for (int j = 0; j < 20; j++)
			{
				if (flag2)
				{
					break;
				}
				short autoAnswerCount = _autoAnswerCount;
				string input2 = ExecCommnad("AT S0=" + autoAnswerCount);
				if (string2bool(input2))
				{
					flag2 = true;
				}
				Thread.Sleep(1000);
			}
			return flag && flag2;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "AutoAnswerConfiguration", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\Tele\\Tele\\NovinPhone.cs", 693);
			return false;
		}
	}

	public bool Disconnect()
	{
		try
		{
			Status = ENStatus.Dissconnectd;
			if (_serial.IsOpen)
			{
				_serial.Close();
			}
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Disconnect", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\Tele\\Tele\\NovinPhone.cs", 713);
			return false;
		}
	}

	public void CommandMode()
	{
		try
		{
			if (Mode != 0)
			{
				Mode = CurrentMode.Command;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CommandMode", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\Tele\\Tele\\NovinPhone.cs", 735);
		}
	}

	public void VoiceMode()
	{
	}

	private void t_Tick(object sender, EventArgs e)
	{
		try
		{
			if (Math.Abs(_lastDate.Subtract(DateTime.Now).Seconds) > 120)
			{
				Disconnect();
				Connect();
				_lastDate = DateTime.Now;
				_readData = false;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "t_Tick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\Tele\\Tele\\NovinPhone.cs", 785);
		}
	}

	private void t_ReadLine_Tick(object sender, EventArgs e)
	{
		try
		{
			TReadLine.Interval = 2000;
			ReadLine();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "t_ReadLine_Tick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\Tele\\Tele\\NovinPhone.cs", 798);
		}
	}
}
