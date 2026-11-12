using System.Runtime.InteropServices;

namespace ADODB;

[Guid("0000051E-0000-0010-8000-00AA006D2EA4")]
public enum ExecuteOptionEnum
{
	[TypeLibVar(64)]
	adOptionUnspecified = -1,
	adAsyncExecute = 16,
	adAsyncFetch = 32,
	adAsyncFetchNonBlocking = 64,
	adExecuteNoRecords = 128,
	adExecuteStream = 1024,
	[TypeLibVar(64)]
	adExecuteRecord = 2048
}
