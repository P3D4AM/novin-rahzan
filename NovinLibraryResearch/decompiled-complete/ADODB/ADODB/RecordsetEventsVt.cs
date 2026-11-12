using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ADODB;

[ComImport]
[InterfaceType(1)]
[Guid("00000403-0000-0010-8000-00AA006D2EA4")]
[TypeLibType(16)]
public interface RecordsetEventsVt
{
	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void WillChangeField([In] int cFields, [In][MarshalAs(UnmanagedType.Struct)] object Fields, [In][Out] ref EventStatusEnum adStatus, [In][MarshalAs(UnmanagedType.Interface)] Recordset pRecordset);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void FieldChangeComplete([In] int cFields, [In][MarshalAs(UnmanagedType.Struct)] object Fields, [In][MarshalAs(UnmanagedType.Interface)] Error pError, [In][Out] ref EventStatusEnum adStatus, [In][MarshalAs(UnmanagedType.Interface)] Recordset pRecordset);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void WillChangeRecord([In] EventReasonEnum adReason, [In] int cRecords, [In][Out] ref EventStatusEnum adStatus, [In][MarshalAs(UnmanagedType.Interface)] Recordset pRecordset);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void RecordChangeComplete([In] EventReasonEnum adReason, [In] int cRecords, [In][MarshalAs(UnmanagedType.Interface)] Error pError, [In][Out] ref EventStatusEnum adStatus, [In][MarshalAs(UnmanagedType.Interface)] Recordset pRecordset);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void WillChangeRecordset([In] EventReasonEnum adReason, [In][Out] ref EventStatusEnum adStatus, [In][MarshalAs(UnmanagedType.Interface)] Recordset pRecordset);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void RecordsetChangeComplete([In] EventReasonEnum adReason, [In][MarshalAs(UnmanagedType.Interface)] Error pError, [In][Out] ref EventStatusEnum adStatus, [In][MarshalAs(UnmanagedType.Interface)] Recordset pRecordset);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void WillMove([In] EventReasonEnum adReason, [In][Out] ref EventStatusEnum adStatus, [In][MarshalAs(UnmanagedType.Interface)] Recordset pRecordset);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void MoveComplete([In] EventReasonEnum adReason, [In][MarshalAs(UnmanagedType.Interface)] Error pError, [In][Out] ref EventStatusEnum adStatus, [In][MarshalAs(UnmanagedType.Interface)] Recordset pRecordset);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void EndOfRecordset([In][Out] ref bool fMoreData, [In][Out] ref EventStatusEnum adStatus, [In][MarshalAs(UnmanagedType.Interface)] Recordset pRecordset);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void FetchProgress([In] int Progress, [In] int MaxProgress, [In][Out] ref EventStatusEnum adStatus, [In][MarshalAs(UnmanagedType.Interface)] Recordset pRecordset);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void FetchComplete([In][MarshalAs(UnmanagedType.Interface)] Error pError, [In][Out] ref EventStatusEnum adStatus, [In][MarshalAs(UnmanagedType.Interface)] Recordset pRecordset);
}
