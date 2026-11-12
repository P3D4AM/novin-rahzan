using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ADODB;

[ComImport]
[DefaultMember("Fields")]
[Guid("00000562-0000-0010-8000-00AA006D2EA4")]
[TypeLibType(4176)]
public interface _Record : _ADO
{
	[DispId(500)]
	new Properties Properties
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(500)]
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "ADODB.PropertiesToInternalPropertiesMarshaler")]
		get;
	}

	[DispId(1)]
	object ActiveConnection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.Struct)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[SpecialName]
	[DispId(1)]
	void let_ActiveConnection([In][MarshalAs(UnmanagedType.BStr)] string pvar);

	[DispId(2)]
	ObjectStateEnum State
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		get;
	}

	[DispId(3)]
	object Source
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3)]
		[return: MarshalAs(UnmanagedType.Struct)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3)]
		[param: In]
		[param: MarshalAs(UnmanagedType.IDispatch)]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[SpecialName]
	[DispId(3)]
	void let_Source([In][MarshalAs(UnmanagedType.BStr)] string pvar);

	[DispId(4)]
	ConnectModeEnum Mode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(4)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(4)]
		[param: In]
		set;
	}

	[DispId(5)]
	string ParentURL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string MoveRecord([In][MarshalAs(UnmanagedType.BStr)] string Source = "", [In][MarshalAs(UnmanagedType.BStr)] string Destination = "", [In][MarshalAs(UnmanagedType.BStr)] string UserName = "", [In][MarshalAs(UnmanagedType.BStr)] string Password = "", [In] MoveRecordOptionsEnum Options = MoveRecordOptionsEnum.adMoveUnspecified, [In] bool Async = false);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(7)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string CopyRecord([In][MarshalAs(UnmanagedType.BStr)] string Source = "", [In][MarshalAs(UnmanagedType.BStr)] string Destination = "", [In][MarshalAs(UnmanagedType.BStr)] string UserName = "", [In][MarshalAs(UnmanagedType.BStr)] string Password = "", [In] CopyRecordOptionsEnum Options = CopyRecordOptionsEnum.adCopyUnspecified, [In] bool Async = false);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8)]
	void DeleteRecord([In][MarshalAs(UnmanagedType.BStr)] string Source = "", [In] bool Async = false);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(9)]
	void Open([Optional][In][MarshalAs(UnmanagedType.Struct)] object Source, [Optional][In][MarshalAs(UnmanagedType.Struct)] object ActiveConnection, [In] ConnectModeEnum Mode = ConnectModeEnum.adModeUnknown, [In] RecordCreateOptionsEnum CreateOptions = RecordCreateOptionsEnum.adFailIfNotExists, [In] RecordOpenOptionsEnum Options = RecordOpenOptionsEnum.adOpenRecordUnspecified, [In][MarshalAs(UnmanagedType.BStr)] string UserName = "", [In][MarshalAs(UnmanagedType.BStr)] string Password = "");

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10)]
	void Close();

	[DispId(0)]
	Fields Fields
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(0)]
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "ADODB.FieldsToInternalFieldsMarshaler")]
		get;
	}

	[DispId(11)]
	RecordTypeEnum RecordType
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(11)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(12)]
	[return: MarshalAs(UnmanagedType.Interface)]
	Recordset GetChildren();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(13)]
	void Cancel();
}
