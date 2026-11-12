using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ADODB;

[ComImport]
[Guid("00000505-0000-0010-8000-00AA006D2EA4")]
[DefaultMember("Value")]
[TypeLibType(4304)]
public interface Field15 : _ADO
{
	[DispId(500)]
	new Properties Properties
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(500)]
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "ADODB.PropertiesToInternalPropertiesMarshaler")]
		get;
	}

	[DispId(1109)]
	[ComAliasName("ADODB.ADO_LONGPTR")]
	int ActualSize
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1109)]
		[return: ComAliasName("ADODB.ADO_LONGPTR")]
		get;
	}

	[DispId(1114)]
	int Attributes
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1114)]
		get;
	}

	[DispId(1103)]
	[ComAliasName("ADODB.ADO_LONGPTR")]
	int DefinedSize
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1103)]
		[return: ComAliasName("ADODB.ADO_LONGPTR")]
		get;
	}

	[DispId(1100)]
	string Name
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1100)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(1102)]
	DataTypeEnum Type
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1102)]
		get;
	}

	[DispId(0)]
	object Value
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(0)]
		[return: MarshalAs(UnmanagedType.Struct)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(0)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Struct)]
		set;
	}

	[DispId(1112)]
	byte Precision
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1112)]
		get;
	}

	[DispId(1113)]
	byte NumericScale
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1113)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1107)]
	void AppendChunk([In][MarshalAs(UnmanagedType.Struct)] object Data);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1108)]
	[return: MarshalAs(UnmanagedType.Struct)]
	object GetChunk([In] int Length);

	[DispId(1104)]
	object OriginalValue
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1104)]
		[return: MarshalAs(UnmanagedType.Struct)]
		get;
	}

	[DispId(1105)]
	object UnderlyingValue
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1105)]
		[return: MarshalAs(UnmanagedType.Struct)]
		get;
	}
}
