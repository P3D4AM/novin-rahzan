using System.Runtime.InteropServices;

namespace ADODB;

[ComVisible(false)]
public delegate void ConnectionEvents_WillConnectEventHandler([In][Out][MarshalAs(UnmanagedType.BStr)] ref string ConnectionString, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string UserID, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Password, [In][Out] ref int Options, [In][Out] ref EventStatusEnum adStatus, [In][MarshalAs(UnmanagedType.Interface)] Connection pConnection);
