using System.Runtime.InteropServices;

namespace AXIMAGELib;

[ComEventInterface(typeof(_DAxImageEvents_0000), typeof(_DAxImageEvents_EventProvider_0000))]
[ComVisible(false)]
[TypeLibType(16)]
public interface _DAxImageEvents_Event
{
	event _DAxImageEvents_ClickEventHandler Click;
}
