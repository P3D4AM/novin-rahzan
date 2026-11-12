using System.Runtime.InteropServices;

namespace SBXPCLib;

[ComImport]
[CoClass(typeof(SBXPCClass))]
[Guid("6C1D3CFC-712A-41B4-A53F-CC10F6412791")]
public interface SBXPC : _DSBXPC, _DSBXPCEvents_Event
{
}
