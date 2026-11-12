using System.Runtime.InteropServices;

namespace ADODB;

[Guid("00000524-0000-0010-8000-00AA006D2EA4")]
public enum XactAttributeEnum
{
	adXactCommitRetaining = 0x20000,
	adXactAbortRetaining = 0x40000,
	[TypeLibVar(64)]
	adXactAsyncPhaseOne = 0x80000,
	[TypeLibVar(64)]
	adXactSyncPhaseOne = 0x100000
}
