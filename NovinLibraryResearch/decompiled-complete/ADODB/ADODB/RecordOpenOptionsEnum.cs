using System.Runtime.InteropServices;

namespace ADODB;

[Guid("00000571-0000-0010-8000-00AA006D2EA4")]
public enum RecordOpenOptionsEnum
{
	adOpenRecordUnspecified = -1,
	[TypeLibVar(64)]
	adOpenSource = 8388608,
	adOpenOutput = 8388608,
	adOpenAsync = 4096,
	adDelayFetchStream = 16384,
	adDelayFetchFields = 32768,
	adOpenExecuteCommand = 65536
}
