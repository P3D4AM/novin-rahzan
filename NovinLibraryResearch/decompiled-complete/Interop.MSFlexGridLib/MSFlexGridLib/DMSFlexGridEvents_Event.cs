using System.Runtime.InteropServices;

namespace MSFlexGridLib;

[ComEventInterface(typeof(DMSFlexGridEvents_0000), typeof(DMSFlexGridEvents_EventProvider_0000))]
[ComVisible(false)]
[TypeLibType(16)]
public interface DMSFlexGridEvents_Event
{
	event DMSFlexGridEvents_ClickEventHandler Click;

	event DMSFlexGridEvents_KeyDownEventHandler KeyDown;

	event DMSFlexGridEvents_DblClickEventHandler DblClick;

	event DMSFlexGridEvents_KeyPressEventHandler KeyPress;

	event DMSFlexGridEvents_KeyUpEventHandler KeyUp;

	event DMSFlexGridEvents_MouseDownEventHandler MouseDown;

	event DMSFlexGridEvents_MouseMoveEventHandler MouseMove;

	event DMSFlexGridEvents_MouseUpEventHandler MouseUp;

	event DMSFlexGridEvents_SelChangeEventHandler SelChange;

	event DMSFlexGridEvents_RowColChangeEventHandler RowColChange;

	event DMSFlexGridEvents_EnterCellEventHandler EnterCell;

	event DMSFlexGridEvents_LeaveCellEventHandler LeaveCell;

	event DMSFlexGridEvents_ScrollEventHandler Scroll;

	event DMSFlexGridEvents_CompareEventHandler Compare;

	event DMSFlexGridEvents_OLEStartDragEventHandler OLEStartDrag;

	event DMSFlexGridEvents_OLEGiveFeedbackEventHandler OLEGiveFeedback;

	event DMSFlexGridEvents_OLESetDataEventHandler OLESetData;

	event DMSFlexGridEvents_OLECompleteDragEventHandler OLECompleteDrag;

	event DMSFlexGridEvents_OLEDragOverEventHandler OLEDragOver;

	event DMSFlexGridEvents_OLEDragDropEventHandler OLEDragDrop;
}
