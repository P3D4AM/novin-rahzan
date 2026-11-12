using System.Runtime.InteropServices;

namespace ADODB;

[Guid("00000527-0000-0010-8000-00AA006D2EA4")]
public enum RecordStatusEnum
{
	adRecOK = 0,
	adRecNew = 1,
	adRecModified = 2,
	adRecDeleted = 4,
	adRecUnmodified = 8,
	adRecInvalid = 0x10,
	adRecMultipleChanges = 0x40,
	adRecPendingChanges = 0x80,
	adRecCanceled = 0x100,
	adRecCantRelease = 0x400,
	adRecConcurrencyViolation = 0x800,
	adRecIntegrityViolation = 0x1000,
	adRecMaxChangesExceeded = 0x2000,
	adRecObjectOpen = 0x4000,
	adRecOutOfMemory = 0x8000,
	adRecPermissionDenied = 0x10000,
	adRecSchemaViolation = 0x20000,
	adRecDBDeleted = 0x40000
}
