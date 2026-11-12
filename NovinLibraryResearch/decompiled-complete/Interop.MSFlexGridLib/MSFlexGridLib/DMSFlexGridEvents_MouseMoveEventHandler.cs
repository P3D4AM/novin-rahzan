using System.Runtime.InteropServices;

namespace MSFlexGridLib;

[ComVisible(false)]
[TypeLibType(16)]
public delegate void DMSFlexGridEvents_MouseMoveEventHandler(short Button, short Shift, [ComAliasName("stdole.OLE_XPOS_PIXELS")] int x, [ComAliasName("stdole.OLE_YPOS_PIXELS")] int y);
