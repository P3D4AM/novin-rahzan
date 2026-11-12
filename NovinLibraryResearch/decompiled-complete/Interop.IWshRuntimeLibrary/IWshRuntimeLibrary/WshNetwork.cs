using System.Runtime.InteropServices;

namespace IWshRuntimeLibrary;

[ComImport]
[Guid("24BE5A31-EDFE-11D2-B933-00104B365C9F")]
[CoClass(typeof(WshNetworkClass))]
public interface WshNetwork : IWshNetwork2
{
}
