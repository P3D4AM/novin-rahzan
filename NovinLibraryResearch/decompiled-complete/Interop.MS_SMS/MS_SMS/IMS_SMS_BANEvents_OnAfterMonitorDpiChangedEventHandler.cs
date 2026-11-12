using System.Runtime.InteropServices;

namespace MS_SMS;

[ComVisible(false)]
[TypeLibType(16)]
public delegate void IMS_SMS_BANEvents_OnAfterMonitorDpiChangedEventHandler([In] int OldDPI, [In] int NewDPI);
