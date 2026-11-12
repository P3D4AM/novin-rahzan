using System.Runtime.InteropServices;

namespace ADODB;

[Guid("00000532-0000-0010-8000-00AA006D2EA4")]
public enum ObjectStateEnum
{
	adStateClosed = 0,
	adStateOpen = 1,
	adStateConnecting = 2,
	adStateExecuting = 4,
	adStateFetching = 8
}
