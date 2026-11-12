using System.Runtime.InteropServices;

namespace MSFlexGridLib;

[TypeLibType(16)]
[ComVisible(false)]
public delegate void DMSFlexGridEvents_MouseUpEventHandler(short Button, short Shift, [ComAliasName("stdole.OLE_XPOS_PIXELS")] int x, [ComAliasName("stdole.OLE_YPOS_PIXELS")] int y);
