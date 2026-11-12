using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ADODB;

[ComImport]
[Guid("00000564-0000-0010-8000-00AA006D2EA4")]
[TypeLibType(4288)]
public interface Fields : Fields20
{
	[DispId(1)]
	new int Count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(-4)]
	[TypeLibFunc(1)]
	[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "System.Runtime.InteropServices.CustomMarshalers.EnumeratorToEnumVariantMarshaler, CustomMarshalers, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	new IEnumerator GetEnumerator();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2)]
	new void Refresh();

	[DispId(0)]
	new Field this[[In][MarshalAs(UnmanagedType.Struct)] object Index]
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(0)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[TypeLibFunc(64)]
	[DispId(1610874880)]
	new void _Append([In][MarshalAs(UnmanagedType.BStr)] string Name, [In] DataTypeEnum Type, [In][ComAliasName("ADODB.ADO_LONGPTR")] int DefinedSize = 0, [In] FieldAttributeEnum Attrib = FieldAttributeEnum.adFldUnspecified);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(4)]
	new void Delete([In][MarshalAs(UnmanagedType.Struct)] object Index);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3)]
	void Append([In][MarshalAs(UnmanagedType.BStr)] string Name, [In] DataTypeEnum Type, [In][ComAliasName("ADODB.ADO_LONGPTR")] int DefinedSize = 0, [In] FieldAttributeEnum Attrib = FieldAttributeEnum.adFldUnspecified, [Optional][In][MarshalAs(UnmanagedType.Struct)] object FieldValue);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5)]
	void Update();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6)]
	void Resync([In] ResyncEnum ResyncValues = ResyncEnum.adResyncAllValues);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(7)]
	void CancelUpdate();
}
