using System.Runtime.InteropServices;

namespace ADODB;

[Guid("00000526-0000-0010-8000-00AA006D2EA4")]
public enum EditModeEnum
{
	adEditNone = 0,
	adEditInProgress = 1,
	adEditAdd = 2,
	adEditDelete = 4
}
