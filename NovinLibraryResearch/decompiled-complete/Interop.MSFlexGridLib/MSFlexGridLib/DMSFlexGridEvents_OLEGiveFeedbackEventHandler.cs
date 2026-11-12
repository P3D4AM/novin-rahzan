using System.Runtime.InteropServices;

namespace MSFlexGridLib;

[ComVisible(false)]
[TypeLibType(16)]
public delegate void DMSFlexGridEvents_OLEGiveFeedbackEventHandler([In][Out] ref int Effect, [In][Out] ref bool DefaultCursors);
