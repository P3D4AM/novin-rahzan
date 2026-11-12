using System.Runtime.InteropServices;

namespace IWshRuntimeLibrary;

[ComImport]
[Guid("F935DC27-1CF0-11D0-ADB9-00C04FD58A0B")]
[CoClass(typeof(IWshCollection_ClassClass))]
public interface IWshCollection_Class : IWshCollection
{
}
