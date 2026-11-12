using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Smackbio;

public class LogServer : IDisposable
{
	public bool m_Disposed;

	public ushort m_PortNo;

	public TcpListener m_Listner;

	private static LinkedList<Terminal> m_TerminalList = new LinkedList<Terminal>();

	public TimeLogCallback m_TimeLogCallBack = null;

	public AdminLogCallback m_AdminLogCallBack = null;

	public AlarmLogCallback m_AlarmLogCallBack = null;

	public PingCallback m_PingCallBack = null;

	public LogServer(ushort portNo, TimeLogCallback timeLogCallback, AdminLogCallback adminLogCallback, AlarmLogCallback alarmLogCallback, PingCallback pingCallback)
	{
		m_Disposed = false;
		m_PortNo = portNo;
		m_TimeLogCallBack = timeLogCallback;
		m_AdminLogCallBack = adminLogCallback;
		m_AlarmLogCallBack = alarmLogCallback;
		m_PingCallBack = pingCallback;
		m_Listner = new TcpListener(IPAddress.Any, m_PortNo);
		m_Listner.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, optionValue: true);
		m_Listner.Start();
		m_Listner.BeginAcceptTcpClient(OnAccept, this);
	}

	~LogServer()
	{
		CleanUp(dispose: false);
	}

	private void CleanUp(bool dispose)
	{
		if (m_Disposed)
		{
			return;
		}
		m_Disposed = true;
		if (!dispose)
		{
			return;
		}
		try
		{
			m_Listner.Stop();
			foreach (Terminal terminal in m_TerminalList)
			{
				terminal?.Dispose();
			}
		}
		catch
		{
		}
	}

	public void Dispose()
	{
		CleanUp(dispose: true);
	}

	public static void OnAccept(IAsyncResult iar)
	{
		LogServer logServer = (LogServer)iar.AsyncState;
		Terminal terminal = new Terminal(logServer.m_TimeLogCallBack, logServer.m_AdminLogCallBack, logServer.m_AlarmLogCallBack, logServer.m_PingCallBack);
		try
		{
			if (!terminal.m_Disposed && !logServer.m_Disposed)
			{
				terminal.EstablishConnect(logServer.m_Listner.EndAcceptTcpClient(iar));
				m_TerminalList.AddLast(terminal);
				logServer.m_Listner.BeginAcceptTcpClient(OnAccept, logServer);
			}
		}
		catch
		{
			terminal.Dispose();
		}
		try
		{
		}
		catch
		{
		}
	}
}
