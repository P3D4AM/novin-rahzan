using System.Runtime.InteropServices;

namespace ADODB;

[Guid("00000525-0000-0010-8000-00AA006D2EA4")]
public enum FieldAttributeEnum
{
	[TypeLibVar(64)]
	adFldUnspecified = -1,
	adFldMayDefer = 2,
	adFldUpdatable = 4,
	adFldUnknownUpdatable = 8,
	adFldFixed = 16,
	adFldIsNullable = 32,
	adFldMayBeNull = 64,
	adFldLong = 128,
	adFldRowID = 256,
	adFldRowVersion = 512,
	adFldCacheDeferred = 4096,
	adFldIsChapter = 8192,
	adFldNegativeScale = 16384,
	adFldKeyColumn = 32768,
	adFldIsRowURL = 65536,
	adFldIsDefaultStream = 131072,
	adFldIsCollection = 262144
}
