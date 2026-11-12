using System.Runtime.InteropServices;

namespace ADODB;

[Guid("00000541-0000-0010-8000-00AA006D2EA4")]
public enum ConnectOptionEnum
{
	[TypeLibVar(64)]
	adConnectUnspecified = -1,
	adAsyncConnect = 16
}
