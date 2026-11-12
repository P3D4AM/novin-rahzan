using System.Runtime.InteropServices;

namespace ADODB;

[Guid("00000546-0000-0010-8000-00AA006D2EA4")]
public enum FilterGroupEnum
{
	adFilterNone,
	adFilterPendingRecords,
	adFilterAffectedRecords,
	adFilterFetchedRecords,
	[TypeLibVar(64)]
	adFilterPredicate,
	adFilterConflictingRecords
}
