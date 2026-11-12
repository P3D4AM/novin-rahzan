using System.Runtime.InteropServices;

namespace ADODB;

[Guid("00000521-0000-0010-8000-00AA006D2EA4")]
public enum ConnectModeEnum
{
	adModeUnknown = 0,
	adModeRead = 1,
	adModeWrite = 2,
	adModeReadWrite = 3,
	adModeShareDenyRead = 4,
	adModeShareDenyWrite = 8,
	adModeShareExclusive = 12,
	adModeShareDenyNone = 16,
	adModeRecursive = 4194304
}
