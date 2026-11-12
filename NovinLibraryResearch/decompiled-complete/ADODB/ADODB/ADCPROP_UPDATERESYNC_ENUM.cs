using System.Runtime.InteropServices;

namespace ADODB;

[Guid("00000553-0000-0010-8000-00AA006D2EA4")]
public enum ADCPROP_UPDATERESYNC_ENUM
{
	adResyncNone = 0,
	adResyncAutoIncrement = 1,
	adResyncConflicts = 2,
	adResyncUpdates = 4,
	adResyncInserts = 8,
	adResyncAll = 15
}
