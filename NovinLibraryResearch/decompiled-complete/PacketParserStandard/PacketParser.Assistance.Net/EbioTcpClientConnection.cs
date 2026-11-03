using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Scale.EventArguments;
using Serilog;

namespace PacketParser.Assistance.Net;

public class EbioTcpClientConnection : BaseTcpClientConnection
{
	public override string PingCommand => "";

	public event Func<object, TimeLogEventLog, Task> TimeLog;

	public event Func<object, AdminLogEventArgs, Task> AdminLog;

	public event Func<object, AlarmEventArgs, Task> AlarmLog;

	public event Func<object, PingEventLog, Task> PingLog;

	public EbioTcpClientConnection(int port, TcpClient connection, ILogger logger)
		: base(port, connection, logger, 1024)
	{
		base.Received += EbioTcpClientConnection_Received;
		InitializeAsync();
	}

	private async Task EbioTcpClientConnection_Received(object arg1, ReadEventArgs e)
	{
		try
		{
			if (!base.IsDisposed && !string.IsNullOrEmpty(e.Data) && !e.Data.ToLower().Contains("POST / HTTP / 1.0".ToLower()))
			{
				await ParseMessageAsync(e.Data);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "EbioTcpClientConnection_Received", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\EbioTcpClientConnection.cs", 182);
		}
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	private bool IsValidXml(string text)
	{
		try
		{
			bool errored = false;
			XmlValidatingReader xmlValidatingReader = new XmlValidatingReader(text, XmlNodeType.Element, new XmlParserContext(null, new XmlNamespaceManager(new NameTable()), null, XmlSpace.None));
			xmlValidatingReader.ValidationEventHandler += delegate(object sender, ValidationEventArgs e)
			{
				errored = e.Severity == XmlSeverityType.Error;
			};
			while (xmlValidatingReader.Read())
			{
			}
			xmlValidatingReader.Close();
			return !errored;
		}
		catch
		{
			return false;
		}
	}

	[DebuggerHidden]
	private async Task<ReturnedSaveFuncInfo> ParseMessageAsync(string message)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 0;
		try
		{
			if (string.IsNullOrEmpty(message?.FixString()?.Trim()?.KeepCharacters()))
			{
				return ret.AddError(EnSystem.None, "Message of port 5005 is null or empty!!!", "", "ParseMessageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\EbioTcpClientConnection.cs", 217);
			}
			if (!IsValidXml(message))
			{
				return ret.AddError("xml data is not valid!!!", "", "ParseMessageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\EbioTcpClientConnection.cs", 219);
			}
			line = 1;
			XmlDocument xmlDocument = new XmlDocument();
			line = 20;
			line = 21;
			try
			{
				line = 222;
				xmlDocument.Load(new StringReader(message));
			}
			catch
			{
				return ret.AddError("is not validXml", "", "ParseMessageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\EbioTcpClientConnection.cs", 235);
			}
			line = 5;
			string elementValue = GetElementValue(xmlDocument, "TerminalType", "");
			line = 6;
			int termId = int.Parse(GetElementValue(xmlDocument, "TerminalID", "-1"));
			line = 7;
			string elementValue2 = GetElementValue(xmlDocument, "DeviceSerialNo", "");
			line = 7;
			int transId = int.Parse(GetElementValue(xmlDocument, "TransID", "-1"));
			line = 8;
			string elementValue3 = GetElementValue(xmlDocument, "Event", "");
			line = 9;
			switch (elementValue3)
			{
			case "TimeLog":
			{
				line = 10;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await OnTimeLogAsync(xmlDocument, elementValue, termId, elementValue2, transId), ReturnedState.None, "ParseMessageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\EbioTcpClientConnection.cs", 323);
				break;
			}
			case "AdminLog":
				line = 15;
				await OnAdminLogAsync(xmlDocument, elementValue, termId, elementValue2, transId);
				break;
			case "Alarm":
				line = 20;
				await OnAlarmLogAsync(xmlDocument, elementValue, termId, elementValue2, transId);
				break;
			case "KeepAlive":
				line = 25;
				await OnPingAsync(xmlDocument, elementValue, termId, elementValue2, transId);
				break;
			default:
				throw new OutOfMemoryException("value(" + elementValue3 + ") is out of range!");
			}
		}
		catch (XmlException)
		{
		}
		catch (Exception ex2)
		{
			if (message != null && string.IsNullOrEmpty(message) && message.Length > 10)
			{
				WebErrorLog.Instance.StartLog(ex2, string.Format("Line:{0}//Message:{1}//messageLen:{2}//messageChars:{3}", line, message, message?.Length ?? (-1), string.Join(",", message?.ToCharArray()?.Select((char p) => p.ToString()))), "ParseMessageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\EbioTcpClientConnection.cs", 351);
			}
			ret.AddReturnedValue(ex2, "", "ParseMessageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\EbioTcpClientConnection.cs", 352);
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfo> OnTimeLogAsync(XmlDocument doc, string termType, int termId, string serialNumber, int transId)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			TimeLogEventLog timeLogEventLog = new TimeLogEventLog();
			timeLogEventLog.termId = termId;
			timeLogEventLog.serialNumber = serialNumber;
			timeLogEventLog.termType = termType;
			timeLogEventLog.transId = transId;
			timeLogEventLog.year = int.Parse(GetElementValue(doc, "Year", ""));
			timeLogEventLog.month = int.Parse(GetElementValue(doc, "Month", ""));
			timeLogEventLog.day = int.Parse(GetElementValue(doc, "Day", ""));
			timeLogEventLog.hour = int.Parse(GetElementValue(doc, "Hour", ""));
			timeLogEventLog.minute = int.Parse(GetElementValue(doc, "Minute", ""));
			timeLogEventLog.second = int.Parse(GetElementValue(doc, "Second", ""));
			timeLogEventLog.userID = long.Parse(GetElementValue(doc, "UserID", "- 1"));
			timeLogEventLog.doorID = int.Parse(GetElementValue(doc, "DoorID", "-1"));
			timeLogEventLog.attendStatus = GetElementValue(doc, "AttendStat", "");
			timeLogEventLog.verifyMode = GetElementValue(doc, "VerifMode", "");
			timeLogEventLog.jobCode = int.Parse(GetElementValue(doc, "JobCode", "-1"));
			timeLogEventLog.antipassStatus = GetElementValue(doc, "APStat", "");
			timeLogEventLog.photo = GetElementByteValue(doc, "LogImage", null);
			await Utilities.RaiseEvent(this.TimeLog, this, timeLogEventLog, base.Token, 60, "OnTimeLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\EbioTcpClientConnection.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnTimeLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\EbioTcpClientConnection.cs", 411);
			ret.AddReturnedValue(ex, "", "OnTimeLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\EbioTcpClientConnection.cs", 412);
		}
		return ret;
	}

	private byte[] GetElementByteValue(XmlDocument doc, string elementName, byte[] defaultValue)
	{
		try
		{
			foreach (XmlElement childNode in doc.DocumentElement.ChildNodes)
			{
				if (childNode.Name == elementName)
				{
					return Convert.FromBase64String(childNode.InnerText);
				}
			}
			return defaultValue;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetElementByteValue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\EbioTcpClientConnection.cs", 431);
			return null;
		}
	}

	private async Task OnAdminLogAsync(XmlDocument doc, string termType, int termId, string serialNumber, int transId)
	{
		try
		{
			AdminLogEventArgs adminLogEventArgs = new AdminLogEventArgs();
			adminLogEventArgs.termId = termId;
			adminLogEventArgs.serialNumber = serialNumber;
			adminLogEventArgs.termType = termType;
			adminLogEventArgs.transId = transId;
			adminLogEventArgs.year = int.Parse(GetElementValue(doc, "Year", ""));
			adminLogEventArgs.month = int.Parse(GetElementValue(doc, "Month", ""));
			adminLogEventArgs.day = int.Parse(GetElementValue(doc, "Day", ""));
			adminLogEventArgs.hour = int.Parse(GetElementValue(doc, "Hour", ""));
			adminLogEventArgs.minute = int.Parse(GetElementValue(doc, "Minute", ""));
			adminLogEventArgs.second = int.Parse(GetElementValue(doc, "Second", ""));
			adminLogEventArgs.adminID = long.Parse(GetElementValue(doc, "AdminID", "-1"));
			adminLogEventArgs.action = GetElementValue(doc, "Action", "");
			adminLogEventArgs.result = int.Parse(GetElementValue(doc, "Stat", "-1"));
			adminLogEventArgs.userID = long.Parse(GetElementValue(doc, "UserID", "-1"));
			await Utilities.RaiseEvent(this.AdminLog, this, adminLogEventArgs, base.Token, 60, "OnAdminLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\EbioTcpClientConnection.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnAdminLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\EbioTcpClientConnection.cs", 459);
		}
	}

	private async Task OnAlarmLogAsync(XmlDocument doc, string termType, int termId, string serialNumber, int transId)
	{
		try
		{
			AlarmEventArgs alarmEventArgs = new AlarmEventArgs();
			alarmEventArgs.termId = termId;
			alarmEventArgs.serialNumber = serialNumber;
			alarmEventArgs.termType = termType;
			alarmEventArgs.transId = transId;
			alarmEventArgs.year = int.Parse(GetElementValue(doc, "Year", ""));
			alarmEventArgs.month = int.Parse(GetElementValue(doc, "Month", ""));
			alarmEventArgs.day = int.Parse(GetElementValue(doc, "Day", ""));
			alarmEventArgs.hour = int.Parse(GetElementValue(doc, "Hour", ""));
			alarmEventArgs.minute = int.Parse(GetElementValue(doc, "Minute", ""));
			alarmEventArgs.second = int.Parse(GetElementValue(doc, "Second", ""));
			alarmEventArgs.userID = long.Parse(GetElementValue(doc, "UserID", "-1"));
			alarmEventArgs.doorID = int.Parse(GetElementValue(doc, "DoorID", "-1"));
			alarmEventArgs.alarmType = GetElementValue(doc, "Type", "");
			await Utilities.RaiseEvent(this.AlarmLog, this, alarmEventArgs, base.Token, 60, "OnAlarmLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\EbioTcpClientConnection.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnAlarmLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\EbioTcpClientConnection.cs", 484);
		}
	}

	private async Task OnPingAsync(XmlDocument doc, string termType, int termId, string serialNumber, int transId)
	{
		try
		{
			PingEventLog pingEventLog = new PingEventLog();
			pingEventLog.termId = termId;
			pingEventLog.serialNumber = serialNumber;
			pingEventLog.termType = termType;
			pingEventLog.transId = transId;
			await Utilities.RaiseEvent(this.PingLog, this, pingEventLog, base.Token, 60, "OnPingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\EbioTcpClientConnection.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnPingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\EbioTcpClientConnection.cs", 499);
		}
	}

	public async Task<bool> SendReplyAsync(string replyType, long transId)
	{
		int line = 0;
		try
		{
			if (base.IsDisposed)
			{
				return false;
			}
			line = 1;
			string text = "<?xml version=\"1.0\"?><Message><Request>" + replyType + "</Request><TransID>" + transId + "</TransID></Message>";
			line = 2;
			byte[] array = new byte[checked(text.Length + 1)];
			line = 3;
			Encoding.ASCII.GetBytes(text).CopyTo(array, 0);
			line = 4;
			array[text.Length] = 0;
			line = 5;
			if (base.IsDisposed)
			{
				return false;
			}
			await SendAsync(array);
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "SendReplyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\EbioTcpClientConnection.cs", 537);
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
			WebErrorLog.Instance.StartLog(ex, "", "GetElementValue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\EbioTcpClientConnection.cs", 555);
			return "";
		}
	}
}
