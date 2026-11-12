using System.Runtime.InteropServices;

namespace ADODB;

[Guid("0000051D-0000-0010-8000-00AA006D2EA4")]
public enum LockTypeEnum
{
	[TypeLibVar(64)]
	adLockUnspecified = -1,
	adLockReadOnly = 1,
	adLockPessimistic = 2,
	adLockOptimistic = 3,
	adLockBatchOptimistic = 4
}
