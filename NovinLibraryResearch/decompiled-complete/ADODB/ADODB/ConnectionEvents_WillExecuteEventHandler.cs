using System.Runtime.InteropServices;

namespace ADODB;

[ComVisible(false)]
public delegate void ConnectionEvents_WillExecuteEventHandler([In][Out][MarshalAs(UnmanagedType.BStr)] ref string Source, [In][Out] ref CursorTypeEnum CursorType, [In][Out] ref LockTypeEnum LockType, [In][Out] ref int Options, [In][Out] ref EventStatusEnum adStatus, [In][MarshalAs(UnmanagedType.Interface)] Command pCommand, [In][MarshalAs(UnmanagedType.Interface)] Recordset pRecordset, [In][MarshalAs(UnmanagedType.Interface)] Connection pConnection);
