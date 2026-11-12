using System.Runtime.InteropServices;

namespace ADODB;

[Guid("0000057B-0000-0010-8000-00AA006D2EA4")]
public enum StreamWriteEnum
{
	adWriteChar = 0,
	adWriteLine = 1,
	[TypeLibVar(64)]
	stWriteChar = 0,
	[TypeLibVar(64)]
	stWriteLine = 1
}
