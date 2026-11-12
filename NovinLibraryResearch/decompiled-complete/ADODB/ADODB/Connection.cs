using System.Runtime.InteropServices;

namespace ADODB;

[ComImport]
[CoClass(typeof(ConnectionClass))]
[Guid("00000550-0000-0010-8000-00AA006D2EA4")]
public interface Connection : _Connection, ConnectionEvents_Event
{
}
