using System.Runtime.InteropServices;

namespace ADODB;

[ComImport]
[Guid("00000556-0000-0010-8000-00AA006D2EA4")]
[CoClass(typeof(RecordsetClass))]
public interface Recordset : _Recordset, RecordsetEvents_Event
{
}
