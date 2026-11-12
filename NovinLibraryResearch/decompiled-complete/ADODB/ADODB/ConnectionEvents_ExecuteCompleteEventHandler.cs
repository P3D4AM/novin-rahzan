using System.Runtime.InteropServices;

namespace ADODB;

[ComVisible(false)]
public delegate void ConnectionEvents_ExecuteCompleteEventHandler([In] int RecordsAffected, [In][MarshalAs(UnmanagedType.Interface)] Error pError, [In][Out] ref EventStatusEnum adStatus, [In][MarshalAs(UnmanagedType.Interface)] Command pCommand, [In][MarshalAs(UnmanagedType.Interface)] Recordset pRecordset, [In][MarshalAs(UnmanagedType.Interface)] Connection pConnection);
