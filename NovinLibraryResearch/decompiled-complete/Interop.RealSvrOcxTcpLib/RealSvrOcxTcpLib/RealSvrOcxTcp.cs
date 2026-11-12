using System.Runtime.InteropServices;

namespace RealSvrOcxTcpLib;

[ComImport]
[Guid("9D2CF0EA-0F41-4665-858D-A292F269C2F9")]
[CoClass(typeof(RealSvrOcxTcpClass))]
public interface RealSvrOcxTcp : _DRealSvrOcxTcp, _DRealSvrOcxTcpEvents_Event
{
}
