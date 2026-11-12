using System.Runtime.InteropServices;

namespace ADODB;

[Guid("0000052E-0000-0010-8000-00AA006D2EA4")]
public enum CommandTypeEnum
{
	[TypeLibVar(64)]
	adCmdUnspecified = -1,
	adCmdUnknown = 8,
	adCmdText = 1,
	adCmdTable = 2,
	adCmdStoredProc = 4,
	adCmdFile = 256,
	adCmdTableDirect = 512
}
