using System.Runtime.InteropServices;

namespace SBXPCLib;

[TypeLibType(TypeLibTypeFlags.FHidden)]
[ClassInterface(ClassInterfaceType.None)]
public sealed class _DSBXPCEvents_SinkHelper : _DSBXPCEvents
{
	public _DSBXPCEvents_OnReceiveEventXMLEventHandler m_OnReceiveEventXMLDelegate;

	public int m_dwCookie;

	public void OnReceiveEventXML(string P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OnReceiveEventXMLDelegate != null)
		{
			m_OnReceiveEventXMLDelegate(P_0);
		}
	}

	internal _DSBXPCEvents_SinkHelper()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		m_dwCookie = 0;
		m_OnReceiveEventXMLDelegate = null;
	}
}
