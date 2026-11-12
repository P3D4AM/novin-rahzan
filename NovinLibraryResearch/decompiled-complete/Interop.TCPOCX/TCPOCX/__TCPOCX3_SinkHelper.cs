using System.Runtime.InteropServices;

namespace TCPOCX;

[TypeLibType(TypeLibTypeFlags.FHidden)]
[ClassInterface(ClassInterfaceType.None)]
public sealed class __TCPOCX3_SinkHelper : __TCPOCX3
{
	public __TCPOCX3_ReceiveSalePacketEventHandler m_ReceiveSalePacketDelegate;

	public int m_dwCookie;

	public void ReceiveSalePacket(byte P_0, string P_1, string P_2, short P_3, short P_4, double P_5, double P_6, string P_7, short P_8)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_ReceiveSalePacketDelegate != null)
		{
			m_ReceiveSalePacketDelegate(P_0, P_1, P_2, P_3, P_4, P_5, P_6, P_7, P_8);
		}
	}

	internal __TCPOCX3_SinkHelper()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		m_dwCookie = 0;
		m_ReceiveSalePacketDelegate = null;
	}
}
