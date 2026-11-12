using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MSFlexGridLib;

[ComImport]
[Guid("2334D2B2-713E-11CF-8AE5-00AA00C00905")]
[ClassInterface(0)]
public class DataObjectClass : IVBDataObject, DataObject
{
	[DispId(5)]
	public virtual extern DataObjectFiles Files
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1)]
	public virtual extern void Clear();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2)]
	[return: MarshalAs(UnmanagedType.Struct)]
	public virtual extern object GetData([In] short sFormat);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3)]
	public virtual extern bool GetFormat([In] short sFormat);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(4)]
	public virtual extern void SetData([Optional][In][MarshalAs(UnmanagedType.Struct)] object vValue, [Optional][In][MarshalAs(UnmanagedType.Struct)] object vFormat);
}
