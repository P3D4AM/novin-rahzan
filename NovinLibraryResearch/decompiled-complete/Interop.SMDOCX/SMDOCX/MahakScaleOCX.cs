using System.Runtime.InteropServices;

namespace SMDOCX;

[ComImport]
[Guid("108F6D7F-6E01-4B22-8E6E-A15CA728A1F0")]
[CoClass(typeof(MahakScaleOCXClass))]
public interface MahakScaleOCX : _MahakScaleOCX, __MahakScaleOCX_Event
{
}
