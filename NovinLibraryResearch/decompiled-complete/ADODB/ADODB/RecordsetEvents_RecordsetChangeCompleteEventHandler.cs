using System.Runtime.InteropServices;

namespace ADODB;

[ComVisible(false)]
public delegate void RecordsetEvents_RecordsetChangeCompleteEventHandler([In] EventReasonEnum adReason, [In][MarshalAs(UnmanagedType.Interface)] Error pError, [In][Out] ref EventStatusEnum adStatus, [In][MarshalAs(UnmanagedType.Interface)] Recordset pRecordset);
