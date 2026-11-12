using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;

namespace ADODB;

internal sealed class ConnectionEvents_EventProvider : ConnectionEvents_Event, IDisposable
{
	private UCOMIConnectionPointContainer m_ConnectionPointContainer;

	private ArrayList m_aEventSinkHelpers;

	private UCOMIConnectionPoint m_ConnectionPoint;

	private void Init()
	{
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Expected O, but got Unknown
		UCOMIConnectionPoint val = null;
		Guid guid = new Guid(new byte[16]
		{
			0, 4, 0, 0, 0, 0, 16, 0, 128, 0,
			0, 170, 0, 109, 46, 164
		});
		m_ConnectionPointContainer.FindConnectionPoint(ref guid, ref val);
		m_ConnectionPoint = val;
		m_aEventSinkHelpers = new ArrayList();
	}

	public void add_Disconnect(ConnectionEvents_DisconnectEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			ConnectionEvents_SinkHelper connectionEvents_SinkHelper = new ConnectionEvents_SinkHelper();
			int dwCookie = 0;
			m_ConnectionPoint.Advise((object)connectionEvents_SinkHelper, ref dwCookie);
			connectionEvents_SinkHelper.m_dwCookie = dwCookie;
			connectionEvents_SinkHelper.m_DisconnectDelegate = A_1;
			m_aEventSinkHelpers.Add(connectionEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_Disconnect(ConnectionEvents_DisconnectEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				ConnectionEvents_SinkHelper connectionEvents_SinkHelper = (ConnectionEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (connectionEvents_SinkHelper.m_DisconnectDelegate != null && ((connectionEvents_SinkHelper.m_DisconnectDelegate.Equals(A_1) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(connectionEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void add_ConnectComplete(ConnectionEvents_ConnectCompleteEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			ConnectionEvents_SinkHelper connectionEvents_SinkHelper = new ConnectionEvents_SinkHelper();
			int dwCookie = 0;
			m_ConnectionPoint.Advise((object)connectionEvents_SinkHelper, ref dwCookie);
			connectionEvents_SinkHelper.m_dwCookie = dwCookie;
			connectionEvents_SinkHelper.m_ConnectCompleteDelegate = A_1;
			m_aEventSinkHelpers.Add(connectionEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_ConnectComplete(ConnectionEvents_ConnectCompleteEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				ConnectionEvents_SinkHelper connectionEvents_SinkHelper = (ConnectionEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (connectionEvents_SinkHelper.m_ConnectCompleteDelegate != null && ((connectionEvents_SinkHelper.m_ConnectCompleteDelegate.Equals(A_1) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(connectionEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void add_WillConnect(ConnectionEvents_WillConnectEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			ConnectionEvents_SinkHelper connectionEvents_SinkHelper = new ConnectionEvents_SinkHelper();
			int dwCookie = 0;
			m_ConnectionPoint.Advise((object)connectionEvents_SinkHelper, ref dwCookie);
			connectionEvents_SinkHelper.m_dwCookie = dwCookie;
			connectionEvents_SinkHelper.m_WillConnectDelegate = A_1;
			m_aEventSinkHelpers.Add(connectionEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_WillConnect(ConnectionEvents_WillConnectEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				ConnectionEvents_SinkHelper connectionEvents_SinkHelper = (ConnectionEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (connectionEvents_SinkHelper.m_WillConnectDelegate != null && ((connectionEvents_SinkHelper.m_WillConnectDelegate.Equals(A_1) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(connectionEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void add_ExecuteComplete(ConnectionEvents_ExecuteCompleteEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			ConnectionEvents_SinkHelper connectionEvents_SinkHelper = new ConnectionEvents_SinkHelper();
			int dwCookie = 0;
			m_ConnectionPoint.Advise((object)connectionEvents_SinkHelper, ref dwCookie);
			connectionEvents_SinkHelper.m_dwCookie = dwCookie;
			connectionEvents_SinkHelper.m_ExecuteCompleteDelegate = A_1;
			m_aEventSinkHelpers.Add(connectionEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_ExecuteComplete(ConnectionEvents_ExecuteCompleteEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				ConnectionEvents_SinkHelper connectionEvents_SinkHelper = (ConnectionEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (connectionEvents_SinkHelper.m_ExecuteCompleteDelegate != null && ((connectionEvents_SinkHelper.m_ExecuteCompleteDelegate.Equals(A_1) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(connectionEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void add_WillExecute(ConnectionEvents_WillExecuteEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			ConnectionEvents_SinkHelper connectionEvents_SinkHelper = new ConnectionEvents_SinkHelper();
			int dwCookie = 0;
			m_ConnectionPoint.Advise((object)connectionEvents_SinkHelper, ref dwCookie);
			connectionEvents_SinkHelper.m_dwCookie = dwCookie;
			connectionEvents_SinkHelper.m_WillExecuteDelegate = A_1;
			m_aEventSinkHelpers.Add(connectionEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_WillExecute(ConnectionEvents_WillExecuteEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				ConnectionEvents_SinkHelper connectionEvents_SinkHelper = (ConnectionEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (connectionEvents_SinkHelper.m_WillExecuteDelegate != null && ((connectionEvents_SinkHelper.m_WillExecuteDelegate.Equals(A_1) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(connectionEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void add_RollbackTransComplete(ConnectionEvents_RollbackTransCompleteEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			ConnectionEvents_SinkHelper connectionEvents_SinkHelper = new ConnectionEvents_SinkHelper();
			int dwCookie = 0;
			m_ConnectionPoint.Advise((object)connectionEvents_SinkHelper, ref dwCookie);
			connectionEvents_SinkHelper.m_dwCookie = dwCookie;
			connectionEvents_SinkHelper.m_RollbackTransCompleteDelegate = A_1;
			m_aEventSinkHelpers.Add(connectionEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_RollbackTransComplete(ConnectionEvents_RollbackTransCompleteEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				ConnectionEvents_SinkHelper connectionEvents_SinkHelper = (ConnectionEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (connectionEvents_SinkHelper.m_RollbackTransCompleteDelegate != null && ((connectionEvents_SinkHelper.m_RollbackTransCompleteDelegate.Equals(A_1) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(connectionEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void add_CommitTransComplete(ConnectionEvents_CommitTransCompleteEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			ConnectionEvents_SinkHelper connectionEvents_SinkHelper = new ConnectionEvents_SinkHelper();
			int dwCookie = 0;
			m_ConnectionPoint.Advise((object)connectionEvents_SinkHelper, ref dwCookie);
			connectionEvents_SinkHelper.m_dwCookie = dwCookie;
			connectionEvents_SinkHelper.m_CommitTransCompleteDelegate = A_1;
			m_aEventSinkHelpers.Add(connectionEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_CommitTransComplete(ConnectionEvents_CommitTransCompleteEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				ConnectionEvents_SinkHelper connectionEvents_SinkHelper = (ConnectionEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (connectionEvents_SinkHelper.m_CommitTransCompleteDelegate != null && ((connectionEvents_SinkHelper.m_CommitTransCompleteDelegate.Equals(A_1) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(connectionEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void add_BeginTransComplete(ConnectionEvents_BeginTransCompleteEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			ConnectionEvents_SinkHelper connectionEvents_SinkHelper = new ConnectionEvents_SinkHelper();
			int dwCookie = 0;
			m_ConnectionPoint.Advise((object)connectionEvents_SinkHelper, ref dwCookie);
			connectionEvents_SinkHelper.m_dwCookie = dwCookie;
			connectionEvents_SinkHelper.m_BeginTransCompleteDelegate = A_1;
			m_aEventSinkHelpers.Add(connectionEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_BeginTransComplete(ConnectionEvents_BeginTransCompleteEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				ConnectionEvents_SinkHelper connectionEvents_SinkHelper = (ConnectionEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (connectionEvents_SinkHelper.m_BeginTransCompleteDelegate != null && ((connectionEvents_SinkHelper.m_BeginTransCompleteDelegate.Equals(A_1) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(connectionEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void add_InfoMessage(ConnectionEvents_InfoMessageEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			ConnectionEvents_SinkHelper connectionEvents_SinkHelper = new ConnectionEvents_SinkHelper();
			int dwCookie = 0;
			m_ConnectionPoint.Advise((object)connectionEvents_SinkHelper, ref dwCookie);
			connectionEvents_SinkHelper.m_dwCookie = dwCookie;
			connectionEvents_SinkHelper.m_InfoMessageDelegate = A_1;
			m_aEventSinkHelpers.Add(connectionEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_InfoMessage(ConnectionEvents_InfoMessageEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				ConnectionEvents_SinkHelper connectionEvents_SinkHelper = (ConnectionEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (connectionEvents_SinkHelper.m_InfoMessageDelegate != null && ((connectionEvents_SinkHelper.m_InfoMessageDelegate.Equals(A_1) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(connectionEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public ConnectionEvents_EventProvider(object A_1)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		m_ConnectionPointContainer = (UCOMIConnectionPointContainer)A_1;
	}

	public void Finalize()
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 < count)
			{
				do
				{
					ConnectionEvents_SinkHelper connectionEvents_SinkHelper = (ConnectionEvents_SinkHelper)m_aEventSinkHelpers[num];
					m_ConnectionPoint.Unadvise(connectionEvents_SinkHelper.m_dwCookie);
					num++;
				}
				while (num < count);
			}
			Marshal.ReleaseComObject(m_ConnectionPoint);
		}
		catch (Exception)
		{
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void Dispose()
	{
		Finalize();
		GC.SuppressFinalize(this);
	}
}
