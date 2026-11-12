using System.Runtime.InteropServices;

namespace ADODB;

[TypeLibType(16)]
[Guid("00000547-0000-0010-8000-00AA006D2EA4")]
public enum SearchDirection
{
	adSearchForward = 1,
	adSearchBackward = -1
}
