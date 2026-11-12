using System.Runtime.InteropServices;

namespace ADODB;

[ComImport]
[Guid("00000562-0000-0010-8000-00AA006D2EA4")]
[CoClass(typeof(RecordClass))]
public interface Record : _Record
{
}
