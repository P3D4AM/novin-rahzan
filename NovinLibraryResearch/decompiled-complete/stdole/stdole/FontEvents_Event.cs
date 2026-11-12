using System.Runtime.InteropServices;

namespace stdole;

[ComVisible(false)]
[ComEventInterface(typeof(FontEvents_0000), typeof(FontEvents_EventProvider_0000))]
public interface FontEvents_Event
{
	event FontEvents_FontChangedEventHandler FontChanged;
}
