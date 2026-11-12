using System.Runtime.InteropServices;

namespace ADODB;

[ComImport]
[Guid("00000565-0000-0010-8000-00AA006D2EA4")]
[CoClass(typeof(StreamClass))]
public interface Stream : _Stream
{
}
