using System.Runtime.InteropServices;

namespace MSFlexGridLib;

[TypeLibType(TypeLibTypeFlags.FHidden)]
[ClassInterface(ClassInterfaceType.None)]
public sealed class DMSFlexGridEvents_SinkHelper : DMSFlexGridEvents
{
	public DMSFlexGridEvents_ClickEventHandler m_ClickDelegate;

	public DMSFlexGridEvents_KeyDownEventHandler m_KeyDownDelegate;

	public DMSFlexGridEvents_DblClickEventHandler m_DblClickDelegate;

	public DMSFlexGridEvents_KeyPressEventHandler m_KeyPressDelegate;

	public DMSFlexGridEvents_KeyUpEventHandler m_KeyUpDelegate;

	public DMSFlexGridEvents_MouseDownEventHandler m_MouseDownDelegate;

	public DMSFlexGridEvents_MouseMoveEventHandler m_MouseMoveDelegate;

	public DMSFlexGridEvents_MouseUpEventHandler m_MouseUpDelegate;

	public DMSFlexGridEvents_SelChangeEventHandler m_SelChangeDelegate;

	public DMSFlexGridEvents_RowColChangeEventHandler m_RowColChangeDelegate;

	public DMSFlexGridEvents_EnterCellEventHandler m_EnterCellDelegate;

	public DMSFlexGridEvents_LeaveCellEventHandler m_LeaveCellDelegate;

	public DMSFlexGridEvents_ScrollEventHandler m_ScrollDelegate;

	public DMSFlexGridEvents_CompareEventHandler m_CompareDelegate;

	public DMSFlexGridEvents_OLEStartDragEventHandler m_OLEStartDragDelegate;

	public DMSFlexGridEvents_OLEGiveFeedbackEventHandler m_OLEGiveFeedbackDelegate;

	public DMSFlexGridEvents_OLESetDataEventHandler m_OLESetDataDelegate;

	public DMSFlexGridEvents_OLECompleteDragEventHandler m_OLECompleteDragDelegate;

	public DMSFlexGridEvents_OLEDragOverEventHandler m_OLEDragOverDelegate;

	public DMSFlexGridEvents_OLEDragDropEventHandler m_OLEDragDropDelegate;

	public int m_dwCookie;

	public void Click()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_ClickDelegate != null)
		{
			m_ClickDelegate();
		}
	}

	public void KeyDown(ref short P_0, short P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_KeyDownDelegate != null)
		{
			m_KeyDownDelegate(ref P_0, P_1);
		}
	}

	public void DblClick()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_DblClickDelegate != null)
		{
			m_DblClickDelegate();
		}
	}

	public void KeyPress(ref short P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_KeyPressDelegate != null)
		{
			m_KeyPressDelegate(ref P_0);
		}
	}

	public void KeyUp(ref short P_0, short P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_KeyUpDelegate != null)
		{
			m_KeyUpDelegate(ref P_0, P_1);
		}
	}

	public void MouseDown(short P_0, short P_1, int P_2, int P_3)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_MouseDownDelegate != null)
		{
			m_MouseDownDelegate(P_0, P_1, P_2, P_3);
		}
	}

	public void MouseMove(short P_0, short P_1, int P_2, int P_3)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_MouseMoveDelegate != null)
		{
			m_MouseMoveDelegate(P_0, P_1, P_2, P_3);
		}
	}

	public void MouseUp(short P_0, short P_1, int P_2, int P_3)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_MouseUpDelegate != null)
		{
			m_MouseUpDelegate(P_0, P_1, P_2, P_3);
		}
	}

	public void SelChange()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_SelChangeDelegate != null)
		{
			m_SelChangeDelegate();
		}
	}

	public void RowColChange()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_RowColChangeDelegate != null)
		{
			m_RowColChangeDelegate();
		}
	}

	public void EnterCell()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_EnterCellDelegate != null)
		{
			m_EnterCellDelegate();
		}
	}

	public void LeaveCell()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_LeaveCellDelegate != null)
		{
			m_LeaveCellDelegate();
		}
	}

	public void Scroll()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_ScrollDelegate != null)
		{
			m_ScrollDelegate();
		}
	}

	public void Compare(int P_0, int P_1, ref short P_2)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_CompareDelegate != null)
		{
			m_CompareDelegate(P_0, P_1, ref P_2);
		}
	}

	public void OLEStartDrag(ref DataObject P_0, ref int P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OLEStartDragDelegate != null)
		{
			m_OLEStartDragDelegate(ref P_0, ref P_1);
		}
	}

	public void OLEGiveFeedback(ref int P_0, ref bool P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OLEGiveFeedbackDelegate != null)
		{
			m_OLEGiveFeedbackDelegate(ref P_0, ref P_1);
		}
	}

	public void OLESetData(ref DataObject P_0, ref short P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OLESetDataDelegate != null)
		{
			m_OLESetDataDelegate(ref P_0, ref P_1);
		}
	}

	public void OLECompleteDrag(ref int P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OLECompleteDragDelegate != null)
		{
			m_OLECompleteDragDelegate(ref P_0);
		}
	}

	public void OLEDragOver(ref DataObject P_0, ref int P_1, ref short P_2, ref short P_3, ref float P_4, ref float P_5, ref short P_6)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OLEDragOverDelegate != null)
		{
			m_OLEDragOverDelegate(ref P_0, ref P_1, ref P_2, ref P_3, ref P_4, ref P_5, ref P_6);
		}
	}

	public void OLEDragDrop(ref DataObject P_0, ref int P_1, ref short P_2, ref short P_3, ref float P_4, ref float P_5)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OLEDragDropDelegate != null)
		{
			m_OLEDragDropDelegate(ref P_0, ref P_1, ref P_2, ref P_3, ref P_4, ref P_5);
		}
	}

	internal DMSFlexGridEvents_SinkHelper()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		m_dwCookie = 0;
		m_ClickDelegate = null;
		m_KeyDownDelegate = null;
		m_DblClickDelegate = null;
		m_KeyPressDelegate = null;
		m_KeyUpDelegate = null;
		m_MouseDownDelegate = null;
		m_MouseMoveDelegate = null;
		m_MouseUpDelegate = null;
		m_SelChangeDelegate = null;
		m_RowColChangeDelegate = null;
		m_EnterCellDelegate = null;
		m_LeaveCellDelegate = null;
		m_ScrollDelegate = null;
		m_CompareDelegate = null;
		m_OLEStartDragDelegate = null;
		m_OLEGiveFeedbackDelegate = null;
		m_OLESetDataDelegate = null;
		m_OLECompleteDragDelegate = null;
		m_OLEDragOverDelegate = null;
		m_OLEDragDropDelegate = null;
	}
}
