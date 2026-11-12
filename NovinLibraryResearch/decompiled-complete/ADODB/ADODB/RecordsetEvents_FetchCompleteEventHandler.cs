using System.Runtime.InteropServices;

namespace ADODB;

[ComVisible(false)]
public delegate void RecordsetEvents_FetchCompleteEventHandler([In][MarshalAs(UnmanagedType.Interface)] Error pError, [In][Out] ref EventStatusEnum adStatus, [In][MarshalAs(UnmanagedType.Interface)] Recordset pRecordset);
