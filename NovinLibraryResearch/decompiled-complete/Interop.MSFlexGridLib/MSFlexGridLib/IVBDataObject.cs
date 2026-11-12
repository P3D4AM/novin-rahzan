using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MSFlexGridLib;

[ComImport]
[TypeLibType(4304)]
[Guid("2334D2B1-713E-11CF-8AE5-00AA00C00905")]
public interface IVBDataObject
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1)]
	void Clear();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2)]
	[return: MarshalAs(UnmanagedType.Struct)]
	object GetData([In] short sFormat);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3)]
	bool GetFormat([In] short sFormat);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(4)]
	void SetData([Optional][In][MarshalAs(UnmanagedType.Struct)] object vValue, [Optional][In][MarshalAs(UnmanagedType.Struct)] object vFormat);

	[DispId(5)]
	DataObjectFiles Files
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}
}
