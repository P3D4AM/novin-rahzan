using System.Reflection;
using System.Runtime.InteropServices;

namespace ADODB;

[DefaultMember("Invoke")]
[ComVisible(false)]
public delegate void ConnectionEvents_InfoMessageEventHandler([In][MarshalAs(UnmanagedType.Interface)] Error pError, [In][Out] ref EventStatusEnum adStatus, [In][MarshalAs(UnmanagedType.Interface)] Connection pConnection);
