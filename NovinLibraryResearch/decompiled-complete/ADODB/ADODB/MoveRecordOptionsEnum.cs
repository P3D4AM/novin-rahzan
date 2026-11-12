using System.Runtime.InteropServices;

namespace ADODB;

[Guid("00000573-0000-0010-8000-00AA006D2EA4")]
public enum MoveRecordOptionsEnum
{
	adMoveUnspecified = -1,
	adMoveOverWrite = 1,
	adMoveDontUpdateLinks = 2,
	adMoveAllowEmulation = 4
}
