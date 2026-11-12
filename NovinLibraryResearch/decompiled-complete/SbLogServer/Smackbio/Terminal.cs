using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml;

namespace Smackbio;

internal class Terminal : IDisposable
{
	public bool m_Disposed;

	public TcpClient m_Client;

	public NetworkStream m_Stream;

	public Timer m_TimerAlive;

	public byte[] m_TmpBuffer;

	public byte[] m_RxBuffer;

	public int m_RxCount;

	private const int MaxMessageSize = 18432;

	private const int PingTimeout = 30000;

	public TimeLogCallback m_TimeLogCallBack = null;

	public AdminLogCallback m_AdminLogCallBack = null;

	public AlarmLogCallback m_AlarmLogCallBack = null;

	public PingCallback m_PingCallBack = null;

	private void CleanUp(bool disposing)
	{
		if (m_Disposed)
		{
			return;
		}
		m_Disposed = true;
		if (!disposing)
		{
			return;
		}
		m_TimerAlive.Change(-1, -1);
		try
		{
			m_Stream?.Close();
			m_Client?.Close();
		}
		catch
		{
		}
	}

	public void Dispose()
	{
		CleanUp(disposing: true);
	}

	public Terminal(TimeLogCallback timeLogCallback, AdminLogCallback adminLogCallback, AlarmLogCallback alarmLogCallback, PingCallback pingCallback)
	{
		m_Disposed = false;
		m_TimeLogCallBack = timeLogCallback;
		m_AdminLogCallBack = adminLogCallback;
		m_AlarmLogCallBack = alarmLogCallback;
		m_PingCallBack = pingCallback;
		m_TmpBuffer = new byte[18432];
		m_RxBuffer = new byte[18432];
		m_RxCount = 0;
		m_TimerAlive = new Timer(OnAliveTimerExpired);
		RestartAliveTimer();
	}

	~Terminal()
	{
		CleanUp(disposing: false);
	}

	public void OnAliveTimerExpired(object stateInfo)
	{
		Dispose();
	}

	public void RestartAliveTimer()
	{
		m_TimerAlive.Change(30000, -1);
	}

	public void EstablishConnect(TcpClient client)
	{
		try
		{
			m_Client = client;
			m_Stream = m_Client.GetStream();
			RestartAliveTimer();
			m_Stream.BeginRead(m_TmpBuffer, 0, 18432, OnReceive, this);
		}
		catch
		{
		}
	}

	public static void OnSend(IAsyncResult iar)
	{
		Terminal terminal = (Terminal)iar.AsyncState;
		try
		{
			terminal.m_Stream.EndWrite(iar);
		}
		catch
		{
		}
	}

	private static bool CheckMessageEnd(byte c)
	{
		return c == 0;
	}

	private string GetElementValue(XmlDocument doc, string elementName)
	{
		foreach (XmlElement childNode in doc.DocumentElement.ChildNodes)
		{
			if (childNode.Name == elementName)
			{
				return childNode.InnerText;
			}
		}
		return "0";
	}

	private bool SendReply(string replyType, int transId)
	{
		try
		{
			string text = "<?xml version=\"1.0\"?><Message><Request>" + replyType + "</Request><TransID>" + transId + "</TransID></Message>";
			byte[] array = new byte[text.Length + 1];
			Encoding.ASCII.GetBytes(text).CopyTo(array, 0);
			array[text.Length] = 0;
			m_Stream.Write(array, 0, array.Length);
		}
		catch
		{
			return false;
		}
		return true;
	}

	private void OnTimeLog(XmlDocument doc, string termType, int termId, string serialNumber, int transId)
	{
		int year;
		int month;
		int day;
		int hour;
		int minute;
		int second;
		try
		{
			year = int.Parse(GetElementValue(doc, "Year"));
			month = int.Parse(GetElementValue(doc, "Month"));
			day = int.Parse(GetElementValue(doc, "Day"));
			hour = int.Parse(GetElementValue(doc, "Hour"));
			minute = int.Parse(GetElementValue(doc, "Minute"));
			second = int.Parse(GetElementValue(doc, "Second"));
		}
		catch (Exception)
		{
			year = 0;
			month = 0;
			day = 0;
			hour = 0;
			minute = 0;
			second = 0;
		}
		long userID;
		try
		{
			userID = long.Parse(GetElementValue(doc, "UserID"));
		}
		catch (Exception)
		{
			userID = -1L;
		}
		int doorID;
		try
		{
			doorID = int.Parse(GetElementValue(doc, "DoorID"));
		}
		catch (Exception)
		{
			doorID = -1;
		}
		string attendanceStatus;
		try
		{
			attendanceStatus = GetElementValue(doc, "AttendStat");
		}
		catch (Exception)
		{
			attendanceStatus = "";
		}
		string verifyMode;
		try
		{
			verifyMode = GetElementValue(doc, "VerifMode");
		}
		catch (Exception)
		{
			verifyMode = "";
		}
		int jobCode;
		try
		{
			jobCode = int.Parse(GetElementValue(doc, "JobCode"));
		}
		catch (Exception)
		{
			jobCode = -1;
		}
		string antipass;
		try
		{
			antipass = GetElementValue(doc, "APStat");
		}
		catch (Exception)
		{
			antipass = "";
		}
		byte[] photo = null;
		try
		{
			if (GetElementValue(doc, "Photo") == "Yes")
			{
				string elementValue = GetElementValue(doc, "LogImage");
				if (elementValue != null)
				{
					photo = Convert.FromBase64String(elementValue);
				}
			}
		}
		catch (Exception)
		{
			photo = null;
		}
		bool flag = false;
		try
		{
			if (SendReply("UploadedLog", transId) && m_TimeLogCallBack != null)
			{
				flag = m_TimeLogCallBack(termType, termId, serialNumber, transId, new DateTime(year, month, day, hour, minute, second), userID, doorID, attendanceStatus, verifyMode, jobCode, antipass, photo);
			}
		}
		catch (Exception)
		{
		}
	}

	private void OnAdminLog(XmlDocument doc, string termType, int termId, string serialNumber, int transId)
	{
		int year;
		int month;
		int day;
		int hour;
		int minute;
		int second;
		try
		{
			year = int.Parse(GetElementValue(doc, "Year"));
			month = int.Parse(GetElementValue(doc, "Month"));
			day = int.Parse(GetElementValue(doc, "Day"));
			hour = int.Parse(GetElementValue(doc, "Hour"));
			minute = int.Parse(GetElementValue(doc, "Minute"));
			second = int.Parse(GetElementValue(doc, "Second"));
		}
		catch (Exception)
		{
			year = 0;
			month = 0;
			day = 0;
			hour = 0;
			minute = 0;
			second = 0;
		}
		long adminID;
		try
		{
			adminID = long.Parse(GetElementValue(doc, "AdminID"));
		}
		catch (Exception)
		{
			adminID = -1L;
		}
		long userID;
		try
		{
			userID = long.Parse(GetElementValue(doc, "UserID"));
		}
		catch (Exception)
		{
			userID = -1L;
		}
		string action;
		try
		{
			action = GetElementValue(doc, "Action");
		}
		catch (Exception)
		{
			action = "";
		}
		int result;
		try
		{
			result = int.Parse(GetElementValue(doc, "Stat"));
		}
		catch (Exception)
		{
			result = -1;
		}
		bool flag = false;
		try
		{
			if (SendReply("UploadedLog", transId) && m_AdminLogCallBack != null)
			{
				flag = m_AdminLogCallBack(termType, termId, serialNumber, transId, new DateTime(year, month, day, hour, minute, second), adminID, userID, action, result);
			}
		}
		catch (Exception)
		{
		}
	}

	private void OnAlarmLog(XmlDocument doc, string termType, int termId, string serialNumber, int transId)
	{
		int year;
		int month;
		int day;
		int hour;
		int minute;
		int second;
		try
		{
			year = int.Parse(GetElementValue(doc, "Year"));
			month = int.Parse(GetElementValue(doc, "Month"));
			day = int.Parse(GetElementValue(doc, "Day"));
			hour = int.Parse(GetElementValue(doc, "Hour"));
			minute = int.Parse(GetElementValue(doc, "Minute"));
			second = int.Parse(GetElementValue(doc, "Second"));
		}
		catch (Exception)
		{
			year = 0;
			month = 0;
			day = 0;
			hour = 0;
			minute = 0;
			second = 0;
		}
		long userID;
		try
		{
			userID = long.Parse(GetElementValue(doc, "UserID"));
		}
		catch (Exception)
		{
			userID = -1L;
		}
		int doorID;
		try
		{
			doorID = int.Parse(GetElementValue(doc, "DoorID"));
		}
		catch (Exception)
		{
			doorID = -1;
		}
		string alarmType;
		try
		{
			alarmType = GetElementValue(doc, "Type");
		}
		catch (Exception)
		{
			alarmType = "";
		}
		bool flag = false;
		try
		{
			if (SendReply("UploadedLog", transId) && m_AlarmLogCallBack != null)
			{
				flag = m_AlarmLogCallBack(termType, termId, serialNumber, transId, new DateTime(year, month, day, hour, minute, second), userID, doorID, alarmType);
			}
		}
		catch (Exception)
		{
		}
	}

	private void OnPing(XmlDocument doc, string termType, int termId, string serialNumber, int transId)
	{
		if (SendReply("KeptAlive", transId) && m_PingCallBack != null)
		{
			m_PingCallBack(termType, termId, serialNumber, transId);
		}
	}

	private void ParseMessage(string message)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(new StringReader(message));
		string termType;
		try
		{
			termType = GetElementValue(xmlDocument, "TerminalType");
		}
		catch (Exception)
		{
			termType = "";
		}
		int termId;
		try
		{
			termId = int.Parse(GetElementValue(xmlDocument, "TerminalID"));
		}
		catch (Exception)
		{
			termId = -1;
		}
		string serialNumber;
		try
		{
			serialNumber = GetElementValue(xmlDocument, "DeviceSerialNo");
		}
		catch (Exception)
		{
			serialNumber = "";
		}
		int transId;
		try
		{
			transId = int.Parse(GetElementValue(xmlDocument, "TransID"));
		}
		catch (Exception)
		{
			transId = -1;
		}
		string text;
		try
		{
			text = GetElementValue(xmlDocument, "Event");
		}
		catch (Exception)
		{
			text = "";
		}
		switch (text)
		{
		case "TimeLog":
			OnTimeLog(xmlDocument, termType, termId, serialNumber, transId);
			break;
		case "AdminLog":
			OnAdminLog(xmlDocument, termType, termId, serialNumber, transId);
			break;
		case "Alarm":
			OnAlarmLog(xmlDocument, termType, termId, serialNumber, transId);
			break;
		case "KeepAlive":
			OnPing(xmlDocument, termType, termId, serialNumber, transId);
			break;
		}
	}

	public bool ParseBuffer(out int consumed)
	{
		byte[] rxBuffer = m_RxBuffer;
		int rxCount = m_RxCount;
		consumed = 0;
		if (m_RxCount == 18432)
		{
			return false;
		}
		int i = Array.FindIndex(rxBuffer, 0, m_RxCount, CheckMessageEnd);
		if (i == -1)
		{
			consumed = 0;
			return true;
		}
		ParseMessage(Encoding.ASCII.GetString(rxBuffer, 0, i));
		for (; i < m_RxCount && rxBuffer[i] == 0; i++)
		{
		}
		if (i != m_RxCount)
		{
			i++;
		}
		consumed = i;
		return true;
	}

	public static void OnReceive(IAsyncResult iar)
	{
		Terminal terminal = (Terminal)iar.AsyncState;
		if (terminal.m_Disposed)
		{
			return;
		}
		try
		{
			int num = terminal.m_Stream.EndRead(iar);
			if (num <= 0)
			{
				throw new Exception("connection closed");
			}
			if (num > 18432 - terminal.m_RxCount)
			{
				num = 18432 - terminal.m_RxCount;
			}
			Array.Copy(terminal.m_TmpBuffer, 0, terminal.m_RxBuffer, terminal.m_RxCount, num);
			terminal.m_RxCount += num;
			while (terminal.m_RxCount > 0)
			{
				if (!terminal.ParseBuffer(out var consumed))
				{
					throw new Exception("handle failed");
				}
				if (consumed > 0)
				{
					terminal.m_RxCount -= consumed;
					Array.Copy(terminal.m_RxBuffer, consumed, terminal.m_RxBuffer, 0, terminal.m_RxCount);
					continue;
				}
				break;
			}
			terminal.RestartAliveTimer();
			terminal.m_Stream.BeginRead(terminal.m_TmpBuffer, 0, 18432, OnReceive, terminal);
		}
		catch
		{
			terminal.Dispose();
		}
	}
}
