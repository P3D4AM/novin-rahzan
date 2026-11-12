using System.Runtime.InteropServices;

namespace ADODB;

[Guid("00000523-0000-0010-8000-00AA006D2EA4")]
public enum IsolationLevelEnum
{
	adXactUnspecified = -1,
	adXactChaos = 16,
	adXactReadUncommitted = 256,
	adXactBrowse = 256,
	adXactCursorStability = 4096,
	adXactReadCommitted = 4096,
	adXactRepeatableRead = 65536,
	adXactSerializable = 1048576,
	adXactIsolated = 1048576
}
