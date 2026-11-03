using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json.Linq;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Scale.EventArguments;
using Serilog;

namespace PacketParser.Assistance.Net;

public class HysoonTcpClientConnection : BaseTcpClientConnection
{
	public HysoonRealTimeType Type { get; private set; }

	public override string PingCommand => "";

	public event Func<object, TimeLogEventLog, Task> TimeLog;

	public event Func<object, AdminLogEventArgs, Task> AdminLog;

	public event Func<object, AlarmEventArgs, Task> AlarmLog;

	public event Func<object, PingEventLog, Task> PingLog;

	public HysoonTcpClientConnection(int port, TcpClient connection, ILogger logger)
		: base(port, connection, logger, 1024)
	{
		base.Received += HysoonTcpClientConnection_Received;
		InitializeAsync();
	}

	private async Task WriteText(string text)
	{
		try
		{
			File.AppendAllText("c:\\log.txt", text);
			File.AppendAllText("c:\\log.txt", "\r\n");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WriteText", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\HysoonTcpClientConnection.cs", 42);
		}
	}

	private async Task HysoonTcpClientConnection_Received(object sender, ReadEventArgs e)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		int num = 0;
		try
		{
			if (base.IsDisposed)
			{
				return;
			}
			if (Type == HysoonRealTimeType.None)
			{
				if (e.Data.StartsWith("RTLOG002"))
				{
					Type = HysoonRealTimeType.Json;
				}
				else
				{
					Type = HysoonRealTimeType.Binary;
				}
			}
			TimeLogEventLog args;
			if (Type == HysoonRealTimeType.Json)
			{
				args = ParsJsonMessage(e);
			}
			else
			{
				if (Type != HysoonRealTimeType.Binary)
				{
					throw new NotImplementedException();
				}
				args = ParsBinaryMessage(e);
			}
			if (!base.IsDisposed)
			{
				Utilities.RaiseEvent(this.TimeLog, this, args, base.Token, 60, "HysoonTcpClientConnection_Received", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\HysoonTcpClientConnection.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{num}", "HysoonTcpClientConnection_Received", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\HysoonTcpClientConnection.cs", 78);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "HysoonTcpClientConnection_Received", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\HysoonTcpClientConnection.cs", 79);
		}
	}

	private TimeLogEventLog ParsBinaryMessage(ReadEventArgs e)
	{
		try
		{
			TimeLogEventLog timeLogEventLog = new TimeLogEventLog();
			if (e.Bytes[0] != 51)
			{
				throw new NotImplementedException("(e.Bytes[0] != 51)");
			}
			if (e.Bytes[1] != 153)
			{
				throw new NotImplementedException("(e.Bytes[1] != 153)");
			}
			if (e.Bytes[2] != 16)
			{
				throw new NotImplementedException("(e.Bytes[2] != 16)");
			}
			if (e.Bytes[3] != 1)
			{
				throw new NotImplementedException("(e.Bytes[3] != 1)");
			}
			if (e.Bytes[4] != 0)
			{
				throw new NotImplementedException("(e.Bytes[4] != 0)");
			}
			if (e.Bytes[5] != 0)
			{
				throw new NotImplementedException("(e.Bytes[5] != 0)");
			}
			if (e.Bytes[6] != 0)
			{
				throw new NotImplementedException("(e.Bytes[6] != 0)");
			}
			if (e.Bytes[7] != 0)
			{
				throw new NotImplementedException("(e.Bytes[7] != 0)");
			}
			if (e.Bytes[8] != 52)
			{
				throw new NotImplementedException("(e.Bytes[8] != 52)");
			}
			if (e.Bytes[9] != 0)
			{
				throw new NotImplementedException("(e.Bytes[9] != 0)");
			}
			if (e.Bytes[10] != 0)
			{
				throw new NotImplementedException("(e.Bytes[10] != 0)");
			}
			if (e.Bytes[11] != 0)
			{
				throw new NotImplementedException("(e.Bytes[11] != 0)");
			}
			if (e.Bytes[12] != 1)
			{
				throw new NotImplementedException("(e.Bytes[12] != 1)");
			}
			if (e.Bytes[13] != 0)
			{
				throw new NotImplementedException("(e.Bytes[13] != 0)");
			}
			if (e.Bytes[14] != 0)
			{
				throw new NotImplementedException("(e.Bytes[14] != 0)");
			}
			if (e.Bytes[15] != 0)
			{
				throw new NotImplementedException("(e.Bytes[15] != 0)");
			}
			if (e.Bytes[16] != 1)
			{
				throw new NotImplementedException("(e.Bytes[16] != 1)");
			}
			FillAttendStatusAndVerifyMode(e.Bytes[17], timeLogEventLog);
			if (e.Bytes[18] != 0)
			{
				throw new NotImplementedException("(e.Bytes[18] != 0)");
			}
			timeLogEventLog.second = e.Bytes[19];
			timeLogEventLog.userID = e.Bytes[20];
			if (e.Bytes[21] != 0)
			{
				throw new NotImplementedException("(e.Bytes[21] != 0)");
			}
			if (e.Bytes[22] != 0)
			{
				throw new NotImplementedException("(e.Bytes[22] != 0)");
			}
			if (e.Bytes[23] != 0)
			{
				throw new NotImplementedException("(e.Bytes[23] != 0)");
			}
			FillDayAndYear(e.Bytes[24], e.Bytes[25], timeLogEventLog);
			FillDayAndHourAndMinute(e.Bytes[26], e.Bytes[27], timeLogEventLog);
			timeLogEventLog.termId = e.Bytes[28];
			if (e.Bytes[29] != 0)
			{
				throw new NotImplementedException("(e.Bytes[29] != 0)");
			}
			timeLogEventLog.ClientPort = checked(e.Bytes[31] * 256 + e.Bytes[30]);
			timeLogEventLog.ClientIp = e.Bytes[35] + "." + e.Bytes[34] + "." + e.Bytes[33] + "." + e.Bytes[32];
			if (e.Bytes[36] != 0)
			{
				throw new NotImplementedException("(e.Bytes[36] != 0)");
			}
			timeLogEventLog.DeviceName = "290";
			return timeLogEventLog;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ParsBinaryMessage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\HysoonTcpClientConnection.cs", 134);
			throw;
		}
	}

	private void FillAttendStatusAndVerifyMode(byte byte17, TimeLogEventLog ea)
	{
		try
		{
			switch (byte17)
			{
			case 65:
				ea.attendStatus = "Enter";
				ea.verifyMode = "FP";
				break;
			case 66:
				ea.attendStatus = "Enter";
				ea.verifyMode = "PassWord";
				break;
			case 67:
				ea.attendStatus = "Enter";
				ea.verifyMode = "RFID";
				break;
			case 129:
				ea.attendStatus = "Exit";
				ea.verifyMode = "FP";
				break;
			case 130:
				ea.attendStatus = "Exit";
				ea.verifyMode = "PassWord";
				break;
			case 131:
				ea.attendStatus = "Exit";
				ea.verifyMode = "RFID";
				break;
			default:
				WebErrorLog.Instance.StartLog($"typeTaradod=>{byte17} not defined!", "", "FillAttendStatusAndVerifyMode", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\HysoonTcpClientConnection.cs", 169);
				ea.attendStatus = byte17.ToString();
				ea.verifyMode = "NONE";
				break;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FillAttendStatusAndVerifyMode", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\HysoonTcpClientConnection.cs", 176);
		}
	}

	private void FillDayAndHourAndMinute(byte byte26, byte byte27, TimeLogEventLog ea)
	{
		try
		{
			ea.minute = byte27 >> 2;
			ea.day = byte26 % 32;
			checked
			{
				ea.hour = (byte26 >> 5) + (unchecked(byte27 % 4) << 3);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FillDayAndHourAndMinute", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\HysoonTcpClientConnection.cs", 188);
			throw;
		}
	}

	private void FillDayAndYear(byte byte24, byte byte25, TimeLogEventLog ea)
	{
		try
		{
			ea.month = byte25 >> 4;
			ea.year = checked((byte25 & 0xF) * 256 + byte24);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FillDayAndYear", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\HysoonTcpClientConnection.cs", 202);
			throw;
		}
	}

	private TimeLogEventLog ParsJsonMessage(ReadEventArgs e)
	{
		try
		{
			TimeLogEventLog timeLogEventLog = new TimeLogEventLog();
			if (!e.Data.StartsWith("RTLOG002") && Type == HysoonRealTimeType.Json)
			{
				throw new NotImplementedException("data=>" + e.Data);
			}
			byte count = e.Bytes.Skip(12).Take(1).Single();
			byte[] bytes = e.Bytes.Skip(16).Take(count).ToArray();
			JObject val = JObject.Parse(Encoding.UTF8.GetString(bytes));
			timeLogEventLog.transId = long.Parse(((object)val["log_id"])?.ToString() ?? string.Empty);
			timeLogEventLog.userID = int.Parse(((object)val["user_id"])?.ToString() ?? string.Empty);
			int termId = int.Parse(((object)val["fk_device_id"])?.ToString() ?? string.Empty);
			timeLogEventLog.verifyMode = ((object)val["io_mode"])?.ToString();
			string text = ((object)val["io_time"])?.ToString();
			if (text == null)
			{
				throw new InvalidOperationException("logTimeStr == null");
			}
			DateTime dateTime = DateTime.Parse(text.Substring(0, 4) + "-" + text.Substring(4, 2) + "-" + text.Substring(6, 2) + " " + text.Substring(8, 2) + ":" + text.Substring(10, 2) + ":" + text.Substring(12, 2));
			timeLogEventLog.serialNumber = ((object)val["serial_number"])?.ToString();
			timeLogEventLog.termId = termId;
			timeLogEventLog.termType = "Hysoon";
			timeLogEventLog.year = dateTime.Year;
			timeLogEventLog.month = dateTime.Month;
			timeLogEventLog.day = dateTime.Day;
			timeLogEventLog.hour = dateTime.Hour;
			timeLogEventLog.minute = dateTime.Minute;
			timeLogEventLog.second = dateTime.Second;
			timeLogEventLog.DeviceName = "587";
			return timeLogEventLog;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ParsJsonMessage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\HysoonTcpClientConnection.cs", 251);
			throw;
		}
	}

	public async Task<bool> SendReplyAsync(bool isSuccess, long transId)
	{
		int line = 0;
		checked
		{
			try
			{
				if (base.IsDisposed || !isSuccess)
				{
					return false;
				}
				line = 1;
				JObject val = new JObject();
				val.Add("log_id", JToken.op_Implicit(transId.ToString()));
				val.Add("result", JToken.op_Implicit("OK"));
				string s = "{\r\n  \"log_id\": \"" + transId + "\",\r\n  \"result\": \"OK\"\r\n}\0";
				List<byte> list = Encoding.ASCII.GetBytes(s).ToList();
				List<byte> list2 = new List<byte>();
				list2.AddRange(Encoding.ASCII.GetBytes("RTLOG002"));
				list2.Add((byte)(list.Count() + 4));
				list2.Add(0);
				list2.Add(0);
				list2.Add(0);
				list2.Add((byte)list.Count());
				list2.Add(0);
				list2.Add(0);
				list2.Add(0);
				list2.AddRange(list);
				await SendAsync(list2.ToArray());
				return true;
			}
			catch (IOException)
			{
				Dispose();
				return false;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"#Line_{line}", "SendReplyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\HysoonTcpClientConnection.cs", 294);
				return false;
			}
		}
	}

	public async Task<bool> SendReplyAsync_290(bool isSuccess)
	{
		int line = 0;
		try
		{
			byte[] source = new byte[16]
			{
				85, 170, 12, 64, 0, 0, 0, 0, 0, 0,
				0, 0, 0, 0, 75, 1
			};
			if (base.IsDisposed || !isSuccess)
			{
				return false;
			}
			await SendAsync(source.ToArray());
			return true;
		}
		catch (IOException)
		{
			Dispose();
			return false;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, $"#Line_{line}", "SendReplyAsync_290", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\HysoonTcpClientConnection.cs", 349);
			return false;
		}
	}

	private string GetElementValue(XmlDocument doc, string elementName, string defaultValue)
	{
		try
		{
			foreach (XmlElement childNode in doc.DocumentElement.ChildNodes)
			{
				if (childNode.Name == elementName)
				{
					return childNode.InnerText;
				}
			}
			return defaultValue;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetElementValue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\HysoonTcpClientConnection.cs", 367);
			return "";
		}
	}
}
