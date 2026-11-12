using System.Runtime.InteropServices;

namespace ADODB;

[Guid("00000574-0000-0010-8000-00AA006D2EA4")]
public enum CopyRecordOptionsEnum
{
	adCopyUnspecified = -1,
	adCopyOverWrite = 1,
	adCopyAllowEmulation = 4,
	adCopyNonRecursive = 2
}
