using System.Runtime.InteropServices;

namespace ADODB;

[ComVisible(false)]
public delegate void ConnectionEvents_CommitTransCompleteEventHandler([In][MarshalAs(UnmanagedType.Interface)] Error pError, [In][Out] ref EventStatusEnum adStatus, [In][MarshalAs(UnmanagedType.Interface)] Connection pConnection);
