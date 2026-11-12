using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ADODB;

[ComImport]
[Guid("00000517-0000-0010-8000-00AA006D2EA4")]
[TypeLibType(512)]
[InterfaceType(1)]
public interface ADOCommandConstruction
{
	[DispId(1610678272)]
	object OLEDBCommand
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.IUnknown)]
		set;
	}
}
