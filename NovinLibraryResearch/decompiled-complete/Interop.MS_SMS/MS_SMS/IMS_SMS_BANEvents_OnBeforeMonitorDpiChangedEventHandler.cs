using System.Runtime.InteropServices;

namespace MS_SMS;

[TypeLibType(16)]
[ComVisible(false)]
public delegate void IMS_SMS_BANEvents_OnBeforeMonitorDpiChangedEventHandler([In] int OldDPI, [In] int NewDPI);
