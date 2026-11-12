using System.Runtime.InteropServices;

namespace ADODB;

[Guid("0000054B-0000-0010-8000-00AA006D2EA4")]
public enum ADCPROP_ASYNCTHREADPRIORITY_ENUM
{
	adPriorityLowest = 1,
	adPriorityBelowNormal,
	adPriorityNormal,
	adPriorityAboveNormal,
	adPriorityHighest
}
