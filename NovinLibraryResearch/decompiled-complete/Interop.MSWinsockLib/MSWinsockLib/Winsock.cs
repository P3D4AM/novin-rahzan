using System.Runtime.InteropServices;

namespace MSWinsockLib;

[ComImport]
[CoClass(typeof(WinsockClass))]
[Guid("248DD892-BB45-11CF-9ABC-0080C7E7B78D")]
public interface Winsock : IMSWinsockControl, DMSWinsockControlEvents_Event
{
}
