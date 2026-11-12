using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ADODB;

[ComImport]
[TypeLibType(4096)]
[InterfaceType(2)]
[Guid("00000400-0000-0010-8000-00AA006D2EA4")]
[DefaultMember("InfoMessage")]
public interface ConnectionEvents
{
	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(0)]
	void InfoMessage([In][MarshalAs(UnmanagedType.Interface)] Error pError, [In][Out] ref EventStatusEnum adStatus, [In][MarshalAs(UnmanagedType.Interface)] Connection pConnection);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1)]
	void BeginTransComplete([In] int TransactionLevel, [In][MarshalAs(UnmanagedType.Interface)] Error pError, [In][Out] ref EventStatusEnum adStatus, [In][MarshalAs(UnmanagedType.Interface)] Connection pConnection);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3)]
	void CommitTransComplete([In][MarshalAs(UnmanagedType.Interface)] Error pError, [In][Out] ref EventStatusEnum adStatus, [In][MarshalAs(UnmanagedType.Interface)] Connection pConnection);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2)]
	void RollbackTransComplete([In][MarshalAs(UnmanagedType.Interface)] Error pError, [In][Out] ref EventStatusEnum adStatus, [In][MarshalAs(UnmanagedType.Interface)] Connection pConnection);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(4)]
	void WillExecute([In][Out][MarshalAs(UnmanagedType.BStr)] ref string Source, [In][Out] ref CursorTypeEnum CursorType, [In][Out] ref LockTypeEnum LockType, [In][Out] ref int Options, [In][Out] ref EventStatusEnum adStatus, [In][MarshalAs(UnmanagedType.Interface)] Command pCommand, [In][MarshalAs(UnmanagedType.Interface)] Recordset pRecordset, [In][MarshalAs(UnmanagedType.Interface)] Connection pConnection);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5)]
	void ExecuteComplete([In] int RecordsAffected, [In][MarshalAs(UnmanagedType.Interface)] Error pError, [In][Out] ref EventStatusEnum adStatus, [In][MarshalAs(UnmanagedType.Interface)] Command pCommand, [In][MarshalAs(UnmanagedType.Interface)] Recordset pRecordset, [In][MarshalAs(UnmanagedType.Interface)] Connection pConnection);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6)]
	void WillConnect([In][Out][MarshalAs(UnmanagedType.BStr)] ref string ConnectionString, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string UserID, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Password, [In][Out] ref int Options, [In][Out] ref EventStatusEnum adStatus, [In][MarshalAs(UnmanagedType.Interface)] Connection pConnection);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(7)]
	void ConnectComplete([In][MarshalAs(UnmanagedType.Interface)] Error pError, [In][Out] ref EventStatusEnum adStatus, [In][MarshalAs(UnmanagedType.Interface)] Connection pConnection);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8)]
	void Disconnect([In][Out] ref EventStatusEnum adStatus, [In][MarshalAs(UnmanagedType.Interface)] Connection pConnection);
}
