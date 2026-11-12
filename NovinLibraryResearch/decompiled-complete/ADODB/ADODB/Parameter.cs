using System.Runtime.InteropServices;

namespace ADODB;

[ComImport]
[CoClass(typeof(ParameterClass))]
[Guid("0000050C-0000-0010-8000-00AA006D2EA4")]
public interface Parameter : _Parameter
{
}
