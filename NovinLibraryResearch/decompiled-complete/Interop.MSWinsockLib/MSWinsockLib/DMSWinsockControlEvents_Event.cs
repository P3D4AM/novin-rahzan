using System.Runtime.InteropServices;

namespace MSWinsockLib;

[TypeLibType(16)]
[ComEventInterface(typeof(DMSWinsockControlEvents_0000), typeof(DMSWinsockControlEvents_EventProvider_0000))]
[ComVisible(false)]
public interface DMSWinsockControlEvents_Event
{
	event DMSWinsockControlEvents_ErrorEventHandler Error;

	event DMSWinsockControlEvents_DataArrivalEventHandler DataArrival;

	event DMSWinsockControlEvents_ConnectEventHandler Connect;

	event DMSWinsockControlEvents_ConnectionRequestEventHandler ConnectionRequest;

	event DMSWinsockControlEvents_CloseEventHandler Close;

	event DMSWinsockControlEvents_SendProgressEventHandler SendProgress;

	event DMSWinsockControlEvents_SendCompleteEventHandler SendComplete;
}
