using System.Runtime.InteropServices;

namespace ADODB;

[Guid("00000530-0000-0010-8000-00AA006D2EA4")]
public enum EventStatusEnum
{
	adStatusOK = 1,
	adStatusErrorsOccurred,
	adStatusCantDeny,
	adStatusCancel,
	adStatusUnwantedEvent
}
