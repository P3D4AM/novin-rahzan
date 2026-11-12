using System.Runtime.InteropServices;

namespace ADODB;

[ComVisible(false)]
public delegate void RecordsetEvents_WillChangeRecordsetEventHandler([In] EventReasonEnum adReason, [In][Out] ref EventStatusEnum adStatus, [In][MarshalAs(UnmanagedType.Interface)] Recordset pRecordset);
