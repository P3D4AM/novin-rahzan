using System;
using System.Runtime.InteropServices;
using MSFlexGridLib;

namespace AxMSFlexGridLib;

[ClassInterface(ClassInterfaceType.None)]
public class AxMSFlexGridEventMulticaster : DMSFlexGridEvents
{
	private AxMSFlexGrid parent;

	public AxMSFlexGridEventMulticaster(AxMSFlexGrid parent)
	{
		this.parent = parent;
	}

	public virtual void Click()
	{
		EventArgs e = new EventArgs();
		parent.RaiseOnClickEvent(parent, e);
	}

	public virtual void KeyDown(ref short keyCode, short shift)
	{
		DMSFlexGridEvents_KeyDownEvent dMSFlexGridEvents_KeyDownEvent = new DMSFlexGridEvents_KeyDownEvent(keyCode, shift);
		parent.RaiseOnKeyDownEvent(parent, dMSFlexGridEvents_KeyDownEvent);
		keyCode = dMSFlexGridEvents_KeyDownEvent.keyCode;
	}

	public virtual void DblClick()
	{
		EventArgs e = new EventArgs();
		parent.RaiseOnDblClick(parent, e);
	}

	public virtual void KeyPress(ref short keyAscii)
	{
		DMSFlexGridEvents_KeyPressEvent dMSFlexGridEvents_KeyPressEvent = new DMSFlexGridEvents_KeyPressEvent(keyAscii);
		parent.RaiseOnKeyPressEvent(parent, dMSFlexGridEvents_KeyPressEvent);
		keyAscii = dMSFlexGridEvents_KeyPressEvent.keyAscii;
	}

	public virtual void KeyUp(ref short keyCode, short shift)
	{
		DMSFlexGridEvents_KeyUpEvent dMSFlexGridEvents_KeyUpEvent = new DMSFlexGridEvents_KeyUpEvent(keyCode, shift);
		parent.RaiseOnKeyUpEvent(parent, dMSFlexGridEvents_KeyUpEvent);
		keyCode = dMSFlexGridEvents_KeyUpEvent.keyCode;
	}

	public virtual void MouseDown(short button, short shift, int x, int y)
	{
		DMSFlexGridEvents_MouseDownEvent e = new DMSFlexGridEvents_MouseDownEvent(button, shift, x, y);
		parent.RaiseOnMouseDownEvent(parent, e);
	}

	public virtual void MouseMove(short button, short shift, int x, int y)
	{
		DMSFlexGridEvents_MouseMoveEvent e = new DMSFlexGridEvents_MouseMoveEvent(button, shift, x, y);
		parent.RaiseOnMouseMoveEvent(parent, e);
	}

	public virtual void MouseUp(short button, short shift, int x, int y)
	{
		DMSFlexGridEvents_MouseUpEvent e = new DMSFlexGridEvents_MouseUpEvent(button, shift, x, y);
		parent.RaiseOnMouseUpEvent(parent, e);
	}

	public virtual void SelChange()
	{
		EventArgs e = new EventArgs();
		parent.RaiseOnSelChange(parent, e);
	}

	public virtual void RowColChange()
	{
		EventArgs e = new EventArgs();
		parent.RaiseOnRowColChange(parent, e);
	}

	public virtual void EnterCell()
	{
		EventArgs e = new EventArgs();
		parent.RaiseOnEnterCell(parent, e);
	}

	public virtual void LeaveCell()
	{
		EventArgs e = new EventArgs();
		parent.RaiseOnLeaveCell(parent, e);
	}

	public virtual void Scroll()
	{
		EventArgs e = new EventArgs();
		parent.RaiseOnScroll(parent, e);
	}

	public virtual void Compare(int row1, int row2, ref short cmp)
	{
		DMSFlexGridEvents_CompareEvent dMSFlexGridEvents_CompareEvent = new DMSFlexGridEvents_CompareEvent(row1, row2, cmp);
		parent.RaiseOnCompare(parent, dMSFlexGridEvents_CompareEvent);
		cmp = dMSFlexGridEvents_CompareEvent.cmp;
	}

	public virtual void OLEStartDrag(ref DataObject data, ref int allowedEffects)
	{
		DMSFlexGridEvents_OLEStartDragEvent dMSFlexGridEvents_OLEStartDragEvent = new DMSFlexGridEvents_OLEStartDragEvent(data, allowedEffects);
		parent.RaiseOnOLEStartDrag(parent, dMSFlexGridEvents_OLEStartDragEvent);
		data = dMSFlexGridEvents_OLEStartDragEvent.data;
		allowedEffects = dMSFlexGridEvents_OLEStartDragEvent.allowedEffects;
	}

	public virtual void OLEGiveFeedback(ref int effect, ref bool defaultCursors)
	{
		DMSFlexGridEvents_OLEGiveFeedbackEvent dMSFlexGridEvents_OLEGiveFeedbackEvent = new DMSFlexGridEvents_OLEGiveFeedbackEvent(effect, defaultCursors);
		parent.RaiseOnOLEGiveFeedback(parent, dMSFlexGridEvents_OLEGiveFeedbackEvent);
		effect = dMSFlexGridEvents_OLEGiveFeedbackEvent.effect;
		defaultCursors = dMSFlexGridEvents_OLEGiveFeedbackEvent.defaultCursors;
	}

	public virtual void OLESetData(ref DataObject data, ref short dataFormat)
	{
		DMSFlexGridEvents_OLESetDataEvent dMSFlexGridEvents_OLESetDataEvent = new DMSFlexGridEvents_OLESetDataEvent(data, dataFormat);
		parent.RaiseOnOLESetData(parent, dMSFlexGridEvents_OLESetDataEvent);
		data = dMSFlexGridEvents_OLESetDataEvent.data;
		dataFormat = dMSFlexGridEvents_OLESetDataEvent.dataFormat;
	}

	public virtual void OLECompleteDrag(ref int effect)
	{
		DMSFlexGridEvents_OLECompleteDragEvent dMSFlexGridEvents_OLECompleteDragEvent = new DMSFlexGridEvents_OLECompleteDragEvent(effect);
		parent.RaiseOnOLECompleteDrag(parent, dMSFlexGridEvents_OLECompleteDragEvent);
		effect = dMSFlexGridEvents_OLECompleteDragEvent.effect;
	}

	public virtual void OLEDragOver(ref DataObject data, ref int effect, ref short button, ref short shift, ref float x, ref float y, ref short state)
	{
		DMSFlexGridEvents_OLEDragOverEvent dMSFlexGridEvents_OLEDragOverEvent = new DMSFlexGridEvents_OLEDragOverEvent(data, effect, button, shift, x, y, state);
		parent.RaiseOnOLEDragOver(parent, dMSFlexGridEvents_OLEDragOverEvent);
		data = dMSFlexGridEvents_OLEDragOverEvent.data;
		effect = dMSFlexGridEvents_OLEDragOverEvent.effect;
		button = dMSFlexGridEvents_OLEDragOverEvent.button;
		shift = dMSFlexGridEvents_OLEDragOverEvent.shift;
		x = dMSFlexGridEvents_OLEDragOverEvent.x;
		y = dMSFlexGridEvents_OLEDragOverEvent.y;
		state = dMSFlexGridEvents_OLEDragOverEvent.state;
	}

	public virtual void OLEDragDrop(ref DataObject data, ref int effect, ref short button, ref short shift, ref float x, ref float y)
	{
		DMSFlexGridEvents_OLEDragDropEvent dMSFlexGridEvents_OLEDragDropEvent = new DMSFlexGridEvents_OLEDragDropEvent(data, effect, button, shift, x, y);
		parent.RaiseOnOLEDragDrop(parent, dMSFlexGridEvents_OLEDragDropEvent);
		data = dMSFlexGridEvents_OLEDragDropEvent.data;
		effect = dMSFlexGridEvents_OLEDragDropEvent.effect;
		button = dMSFlexGridEvents_OLEDragDropEvent.button;
		shift = dMSFlexGridEvents_OLEDragDropEvent.shift;
		x = dMSFlexGridEvents_OLEDragDropEvent.x;
		y = dMSFlexGridEvents_OLEDragDropEvent.y;
	}
}
