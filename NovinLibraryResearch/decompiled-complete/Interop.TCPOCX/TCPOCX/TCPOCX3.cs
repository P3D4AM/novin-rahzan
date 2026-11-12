using System.Runtime.InteropServices;

namespace TCPOCX;

[ComImport]
[Guid("A18B23FE-5867-45FE-88FE-E7374E833AAD")]
[CoClass(typeof(TCPOCX3Class))]
public interface TCPOCX3 : _TCPOCX3, __TCPOCX3_Event
{
}
