using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ADODB;

[ComImport]
[Guid("00000560-0000-0010-8000-00AA006D2EA4")]
[ClassInterface(0)]
[DefaultMember("Fields")]
[TypeLibType(6)]
public class RecordClass : _Record, Record
{
	[DispId(500)]
	public virtual extern Properties Properties
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(500)]
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "ADODB.PropertiesToInternalPropertiesMarshaler")]
		get;
	}

	[DispId(1)]
	public virtual extern object ActiveConnection
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

	[DispId(2)]
	public virtual extern ObjectStateEnum State
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		get;
	}

	[DispId(3)]
	public virtual extern object Source
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

	[DispId(4)]
	public virtual extern ConnectModeEnum Mode
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
	public virtual extern string ParentURL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(0)]
	public virtual extern Fields Fields
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(0)]
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "ADODB.FieldsToInternalFieldsMarshaler")]
		get;
	}

	[DispId(11)]
	public virtual extern RecordTypeEnum RecordType
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(11)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[SpecialName]
	[DispId(1)]
	public virtual extern void let_ActiveConnection([In][MarshalAs(UnmanagedType.BStr)] string pvar);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[SpecialName]
	[DispId(3)]
	public virtual extern void let_Source([In][MarshalAs(UnmanagedType.BStr)] string pvar);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string MoveRecord([In][MarshalAs(UnmanagedType.BStr)] string Source = "", [In][MarshalAs(UnmanagedType.BStr)] string Destination = "", [In][MarshalAs(UnmanagedType.BStr)] string UserName = "", [In][MarshalAs(UnmanagedType.BStr)] string Password = "", [In] MoveRecordOptionsEnum Options = MoveRecordOptionsEnum.adMoveUnspecified, [In] bool Async = false);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(7)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string CopyRecord([In][MarshalAs(UnmanagedType.BStr)] string Source = "", [In][MarshalAs(UnmanagedType.BStr)] string Destination = "", [In][MarshalAs(UnmanagedType.BStr)] string UserName = "", [In][MarshalAs(UnmanagedType.BStr)] string Password = "", [In] CopyRecordOptionsEnum Options = CopyRecordOptionsEnum.adCopyUnspecified, [In] bool Async = false);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8)]
	public virtual extern void DeleteRecord([In][MarshalAs(UnmanagedType.BStr)] string Source = "", [In] bool Async = false);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(9)]
	public virtual extern void Open([Optional][In][MarshalAs(UnmanagedType.Struct)] object Source, [Optional][In][MarshalAs(UnmanagedType.Struct)] object ActiveConnection, [In] ConnectModeEnum Mode = ConnectModeEnum.adModeUnknown, [In] RecordCreateOptionsEnum CreateOptions = RecordCreateOptionsEnum.adFailIfNotExists, [In] RecordOpenOptionsEnum Options = RecordOpenOptionsEnum.adOpenRecordUnspecified, [In][MarshalAs(UnmanagedType.BStr)] string UserName = "", [In][MarshalAs(UnmanagedType.BStr)] string Password = "");

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10)]
	public virtual extern void Close();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(12)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern Recordset GetChildren();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(13)]
	public virtual extern void Cancel();
}
