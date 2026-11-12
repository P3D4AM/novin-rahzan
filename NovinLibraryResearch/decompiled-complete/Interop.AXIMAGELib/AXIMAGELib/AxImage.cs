using System.Runtime.InteropServices;

namespace AXIMAGELib;

[ComImport]
[Guid("4FED1AC4-6CF3-11D6-BB83-9FDD9223647D")]
[CoClass(typeof(AxImageClass))]
public interface AxImage : _DAxImage, _DAxImageEvents_Event
{
}
