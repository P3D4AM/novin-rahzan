using System.Runtime.InteropServices;

namespace MSFlexGridLib;

[ComImport]
[CoClass(typeof(MSFlexGridClass))]
[Guid("5F4DF280-531B-11CF-91F6-C2863C385E30")]
public interface MSFlexGrid : IMSFlexGrid, DMSFlexGridEvents_Event
{
}
