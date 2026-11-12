using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AXIMAGELib;

[ComImport]
[InterfaceType(2)]
[Guid("4FED1AC5-6CF3-11D6-BB83-9FDD9223647D")]
[TypeLibType(4096)]
public interface _DAxImageEvents
{
	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(-600)]
	void Click();
}
