using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MSFlexGridLib;

[ComImport]
[InterfaceType(2)]
[Guid("609602E0-531B-11CF-91F6-C2863C385E30")]
[TypeLibType(4112)]
public interface DMSFlexGridEvents
{
	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(-600)]
	void Click();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(-602)]
	void KeyDown(ref short KeyCode, short Shift);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(-601)]
	void DblClick();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(-603)]
	void KeyPress(ref short KeyAscii);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(-604)]
	void KeyUp(ref short KeyCode, short Shift);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(-605)]
	void MouseDown(short Button, short Shift, [ComAliasName("stdole.OLE_XPOS_PIXELS")] int x, [ComAliasName("stdole.OLE_YPOS_PIXELS")] int y);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(-606)]
	void MouseMove(short Button, short Shift, [ComAliasName("stdole.OLE_XPOS_PIXELS")] int x, [ComAliasName("stdole.OLE_YPOS_PIXELS")] int y);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(-607)]
	void MouseUp(short Button, short Shift, [ComAliasName("stdole.OLE_XPOS_PIXELS")] int x, [ComAliasName("stdole.OLE_YPOS_PIXELS")] int y);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(69)]
	void SelChange();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(70)]
	void RowColChange();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(71)]
	void EnterCell();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(72)]
	void LeaveCell();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(73)]
	void Scroll();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(74)]
	void Compare(int Row1, int Row2, ref short Cmp);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1550)]
	void OLEStartDrag([In][Out][MarshalAs(UnmanagedType.Interface)] ref DataObject Data, [In][Out] ref int AllowedEffects);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1551)]
	void OLEGiveFeedback([In][Out] ref int Effect, [In][Out] ref bool DefaultCursors);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1552)]
	void OLESetData([In][Out][MarshalAs(UnmanagedType.Interface)] ref DataObject Data, [In][Out] ref short DataFormat);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1553)]
	void OLECompleteDrag([In][Out] ref int Effect);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1554)]
	void OLEDragOver([In][Out][MarshalAs(UnmanagedType.Interface)] ref DataObject Data, [In][Out] ref int Effect, [In][Out] ref short Button, [In][Out] ref short Shift, [In][Out] ref float x, [In][Out] ref float y, [In][Out] ref short State);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1555)]
	void OLEDragDrop([In][Out][MarshalAs(UnmanagedType.Interface)] ref DataObject Data, [In][Out] ref int Effect, [In][Out] ref short Button, [In][Out] ref short Shift, [In][Out] ref float x, [In][Out] ref float y);
}
