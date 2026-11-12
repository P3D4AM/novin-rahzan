using System.Runtime.InteropServices;

namespace MSWinsockLib;

[TypeLibType(TypeLibTypeFlags.FHidden)]
[ClassInterface(ClassInterfaceType.None)]
public sealed class DMSWinsockControlEvents_SinkHelper : DMSWinsockControlEvents
{
	public DMSWinsockControlEvents_ErrorEventHandler m_ErrorDelegate;

	public DMSWinsockControlEvents_DataArrivalEventHandler m_DataArrivalDelegate;

	public DMSWinsockControlEvents_ConnectEventHandler m_ConnectDelegate;

	public DMSWinsockControlEvents_ConnectionRequestEventHandler m_ConnectionRequestDelegate;

	public DMSWinsockControlEvents_CloseEventHandler m_CloseDelegate;

	public DMSWinsockControlEvents_SendProgressEventHandler m_SendProgressDelegate;

	public DMSWinsockControlEvents_SendCompleteEventHandler m_SendCompleteDelegate;

	public int m_dwCookie;

	public void Error(short P_0, ref string P_1, int P_2, string P_3, string P_4, int P_5, ref bool P_6)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_ErrorDelegate != null)
		{
			m_ErrorDelegate(P_0, ref P_1, P_2, P_3, P_4, P_5, ref P_6);
		}
	}

	public void DataArrival(int P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_DataArrivalDelegate != null)
		{
			m_DataArrivalDelegate(P_0);
		}
	}

	public void Connect()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_ConnectDelegate != null)
		{
			m_ConnectDelegate();
		}
	}

	public void ConnectionRequest(int P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_ConnectionRequestDelegate != null)
		{
			m_ConnectionRequestDelegate(P_0);
		}
	}

	public void Close()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_CloseDelegate != null)
		{
			m_CloseDelegate();
		}
	}

	public void SendProgress(int P_0, int P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_SendProgressDelegate != null)
		{
			m_SendProgressDelegate(P_0, P_1);
		}
	}

	public void SendComplete()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_SendCompleteDelegate != null)
		{
			m_SendCompleteDelegate();
		}
	}

	internal DMSWinsockControlEvents_SinkHelper()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		m_dwCookie = 0;
		m_ErrorDelegate = null;
		m_DataArrivalDelegate = null;
		m_ConnectDelegate = null;
		m_ConnectionRequestDelegate = null;
		m_CloseDelegate = null;
		m_SendProgressDelegate = null;
		m_SendCompleteDelegate = null;
	}
}
