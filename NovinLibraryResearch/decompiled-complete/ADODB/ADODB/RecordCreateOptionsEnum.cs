using System.Runtime.InteropServices;

namespace ADODB;

[Guid("00000570-0000-0010-8000-00AA006D2EA4")]
public enum RecordCreateOptionsEnum
{
	adCreateCollection = 8192,
	adCreateStructDoc = int.MinValue,
	adCreateNonCollection = 0,
	adOpenIfExists = 33554432,
	adCreateOverwrite = 67108864,
	adFailIfNotExists = -1
}
