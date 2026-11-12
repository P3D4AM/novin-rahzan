using System.Runtime.InteropServices;

namespace ADODB;

[ClassInterface(ClassInterfaceType.None)]
public sealed class ConnectionEvents_SinkHelper : ConnectionEvents
{
	public ConnectionEvents_DisconnectEventHandler m_DisconnectDelegate;

	public ConnectionEvents_ConnectCompleteEventHandler m_ConnectCompleteDelegate;

	public ConnectionEvents_WillConnectEventHandler m_WillConnectDelegate;

	public ConnectionEvents_ExecuteCompleteEventHandler m_ExecuteCompleteDelegate;

	public ConnectionEvents_WillExecuteEventHandler m_WillExecuteDelegate;

	public ConnectionEvents_RollbackTransCompleteEventHandler m_RollbackTransCompleteDelegate;

	public ConnectionEvents_CommitTransCompleteEventHandler m_CommitTransCompleteDelegate;

	public ConnectionEvents_BeginTransCompleteEventHandler m_BeginTransCompleteDelegate;

	public ConnectionEvents_InfoMessageEventHandler m_InfoMessageDelegate;

	public int m_dwCookie;

	public void Disconnect(ref EventStatusEnum A_1, Connection A_2)
	{
		if (m_DisconnectDelegate != null)
		{
			m_DisconnectDelegate(ref A_1, A_2);
		}
	}

	public void ConnectComplete(Error A_1, ref EventStatusEnum A_2, Connection A_3)
	{
		if (m_ConnectCompleteDelegate != null)
		{
			m_ConnectCompleteDelegate(A_1, ref A_2, A_3);
		}
	}

	public void WillConnect(ref string A_1, ref string A_2, ref string A_3, ref int A_4, ref EventStatusEnum A_5, Connection A_6)
	{
		if (m_WillConnectDelegate != null)
		{
			m_WillConnectDelegate(ref A_1, ref A_2, ref A_3, ref A_4, ref A_5, A_6);
		}
	}

	public void ExecuteComplete(int A_1, Error A_2, ref EventStatusEnum A_3, Command A_4, Recordset A_5, Connection A_6)
	{
		if (m_ExecuteCompleteDelegate != null)
		{
			m_ExecuteCompleteDelegate(A_1, A_2, ref A_3, A_4, A_5, A_6);
		}
	}

	public void WillExecute(ref string A_1, ref CursorTypeEnum A_2, ref LockTypeEnum A_3, ref int A_4, ref EventStatusEnum A_5, Command A_6, Recordset A_7, Connection A_8)
	{
		if (m_WillExecuteDelegate != null)
		{
			m_WillExecuteDelegate(ref A_1, ref A_2, ref A_3, ref A_4, ref A_5, A_6, A_7, A_8);
		}
	}

	public void RollbackTransComplete(Error A_1, ref EventStatusEnum A_2, Connection A_3)
	{
		if (m_RollbackTransCompleteDelegate != null)
		{
			m_RollbackTransCompleteDelegate(A_1, ref A_2, A_3);
		}
	}

	public void CommitTransComplete(Error A_1, ref EventStatusEnum A_2, Connection A_3)
	{
		if (m_CommitTransCompleteDelegate != null)
		{
			m_CommitTransCompleteDelegate(A_1, ref A_2, A_3);
		}
	}

	public void BeginTransComplete(int A_1, Error A_2, ref EventStatusEnum A_3, Connection A_4)
	{
		if (m_BeginTransCompleteDelegate != null)
		{
			m_BeginTransCompleteDelegate(A_1, A_2, ref A_3, A_4);
		}
	}

	public void InfoMessage(Error A_1, ref EventStatusEnum A_2, Connection A_3)
	{
		if (m_InfoMessageDelegate != null)
		{
			m_InfoMessageDelegate(A_1, ref A_2, A_3);
		}
	}

	internal ConnectionEvents_SinkHelper()
	{
		m_dwCookie = 0;
		m_DisconnectDelegate = null;
		m_ConnectCompleteDelegate = null;
		m_WillConnectDelegate = null;
		m_ExecuteCompleteDelegate = null;
		m_WillExecuteDelegate = null;
		m_RollbackTransCompleteDelegate = null;
		m_CommitTransCompleteDelegate = null;
		m_BeginTransCompleteDelegate = null;
		m_InfoMessageDelegate = null;
	}
}
