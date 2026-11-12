using System.Runtime.InteropServices;

namespace SBXPCLib;

[ComEventInterface(typeof(_DSBXPCEvents_0000), typeof(_DSBXPCEvents_EventProvider_0000))]
[ComVisible(false)]
[TypeLibType(16)]
public interface _DSBXPCEvents_Event
{
	event _DSBXPCEvents_OnReceiveEventXMLEventHandler OnReceiveEventXML;
}
