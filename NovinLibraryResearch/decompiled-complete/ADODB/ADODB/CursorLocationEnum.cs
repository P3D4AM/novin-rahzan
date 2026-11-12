using System.Runtime.InteropServices;

namespace ADODB;

[Guid("0000052F-0000-0010-8000-00AA006D2EA4")]
public enum CursorLocationEnum
{
	[TypeLibVar(64)]
	adUseNone = 1,
	adUseServer = 2,
	adUseClient = 3,
	[TypeLibVar(64)]
	adUseClientBatch = 3
}
