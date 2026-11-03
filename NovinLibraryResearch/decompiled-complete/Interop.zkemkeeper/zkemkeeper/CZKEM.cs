using System.Runtime.InteropServices;

namespace zkemkeeper;

[ComImport]
[CoClass(typeof(CZKEMClass))]
[Guid("102F4206-E43D-4FC9-BAB0-331CFFE4D25B")]
public interface CZKEM : IZKEM, _IZKEMEvents_Event
{
}
