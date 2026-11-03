using System.Runtime.InteropServices;
using SBXPCLib;

namespace AxSBXPCLib;

[ClassInterface(ClassInterfaceType.None)]
public class AxSBXPCEventMulticaster : _DSBXPCEvents
{
	private AxSBXPC parent;

	public AxSBXPCEventMulticaster(AxSBXPC parent)
	{
		this.parent = parent;
	}

	public virtual void OnReceiveEventXML(string lpszEventXML)
	{
		_DSBXPCEvents_OnReceiveEventXMLEvent e = new _DSBXPCEvents_OnReceiveEventXMLEvent(lpszEventXML);
		parent.RaiseOnOnReceiveEventXML(parent, e);
	}
}
