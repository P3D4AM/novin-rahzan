using System.Runtime.InteropServices;

namespace ADODB;

[Guid("00000531-0000-0010-8000-00AA006D2EA4")]
public enum EventReasonEnum
{
	adRsnAddNew = 1,
	adRsnDelete,
	adRsnUpdate,
	adRsnUndoUpdate,
	adRsnUndoAddNew,
	adRsnUndoDelete,
	adRsnRequery,
	adRsnResynch,
	adRsnClose,
	adRsnMove,
	adRsnFirstChange,
	adRsnMoveFirst,
	adRsnMoveNext,
	adRsnMovePrevious,
	adRsnMoveLast
}
