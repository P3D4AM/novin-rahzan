using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

namespace MSFlexGridLib;

internal sealed class DMSFlexGridEvents_EventProvider : DMSFlexGridEvents_Event, IDisposable
{
	private IConnectionPointContainer m_ConnectionPointContainer;

	private ArrayList m_aEventSinkHelpers;

	private IConnectionPoint m_ConnectionPoint;

	private void Init()
	{
		IConnectionPoint ppCP = null;
		Guid riid = new Guid(new byte[16]
		{
			224, 2, 150, 96, 27, 83, 207, 17, 145, 246,
			194, 134, 60, 56, 94, 48
		});
		m_ConnectionPointContainer.FindConnectionPoint(ref riid, out ppCP);
		m_ConnectionPoint = ppCP;
		m_aEventSinkHelpers = new ArrayList();
	}

	public void add_Click(DMSFlexGridEvents_ClickEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = new DMSFlexGridEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(dMSFlexGridEvents_SinkHelper, out pdwCookie);
			dMSFlexGridEvents_SinkHelper.m_dwCookie = pdwCookie;
			dMSFlexGridEvents_SinkHelper.m_ClickDelegate = P_0;
			m_aEventSinkHelpers.Add(dMSFlexGridEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_Click(DMSFlexGridEvents_ClickEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = (DMSFlexGridEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dMSFlexGridEvents_SinkHelper.m_ClickDelegate != null && ((dMSFlexGridEvents_SinkHelper.m_ClickDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dMSFlexGridEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void add_KeyDown(DMSFlexGridEvents_KeyDownEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = new DMSFlexGridEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(dMSFlexGridEvents_SinkHelper, out pdwCookie);
			dMSFlexGridEvents_SinkHelper.m_dwCookie = pdwCookie;
			dMSFlexGridEvents_SinkHelper.m_KeyDownDelegate = P_0;
			m_aEventSinkHelpers.Add(dMSFlexGridEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_KeyDown(DMSFlexGridEvents_KeyDownEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = (DMSFlexGridEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dMSFlexGridEvents_SinkHelper.m_KeyDownDelegate != null && ((dMSFlexGridEvents_SinkHelper.m_KeyDownDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dMSFlexGridEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void add_DblClick(DMSFlexGridEvents_DblClickEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = new DMSFlexGridEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(dMSFlexGridEvents_SinkHelper, out pdwCookie);
			dMSFlexGridEvents_SinkHelper.m_dwCookie = pdwCookie;
			dMSFlexGridEvents_SinkHelper.m_DblClickDelegate = P_0;
			m_aEventSinkHelpers.Add(dMSFlexGridEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_DblClick(DMSFlexGridEvents_DblClickEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = (DMSFlexGridEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dMSFlexGridEvents_SinkHelper.m_DblClickDelegate != null && ((dMSFlexGridEvents_SinkHelper.m_DblClickDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dMSFlexGridEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void add_KeyPress(DMSFlexGridEvents_KeyPressEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = new DMSFlexGridEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(dMSFlexGridEvents_SinkHelper, out pdwCookie);
			dMSFlexGridEvents_SinkHelper.m_dwCookie = pdwCookie;
			dMSFlexGridEvents_SinkHelper.m_KeyPressDelegate = P_0;
			m_aEventSinkHelpers.Add(dMSFlexGridEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_KeyPress(DMSFlexGridEvents_KeyPressEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = (DMSFlexGridEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dMSFlexGridEvents_SinkHelper.m_KeyPressDelegate != null && ((dMSFlexGridEvents_SinkHelper.m_KeyPressDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dMSFlexGridEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void add_KeyUp(DMSFlexGridEvents_KeyUpEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = new DMSFlexGridEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(dMSFlexGridEvents_SinkHelper, out pdwCookie);
			dMSFlexGridEvents_SinkHelper.m_dwCookie = pdwCookie;
			dMSFlexGridEvents_SinkHelper.m_KeyUpDelegate = P_0;
			m_aEventSinkHelpers.Add(dMSFlexGridEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_KeyUp(DMSFlexGridEvents_KeyUpEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = (DMSFlexGridEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dMSFlexGridEvents_SinkHelper.m_KeyUpDelegate != null && ((dMSFlexGridEvents_SinkHelper.m_KeyUpDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dMSFlexGridEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void add_MouseDown(DMSFlexGridEvents_MouseDownEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = new DMSFlexGridEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(dMSFlexGridEvents_SinkHelper, out pdwCookie);
			dMSFlexGridEvents_SinkHelper.m_dwCookie = pdwCookie;
			dMSFlexGridEvents_SinkHelper.m_MouseDownDelegate = P_0;
			m_aEventSinkHelpers.Add(dMSFlexGridEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_MouseDown(DMSFlexGridEvents_MouseDownEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = (DMSFlexGridEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dMSFlexGridEvents_SinkHelper.m_MouseDownDelegate != null && ((dMSFlexGridEvents_SinkHelper.m_MouseDownDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dMSFlexGridEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void add_MouseMove(DMSFlexGridEvents_MouseMoveEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = new DMSFlexGridEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(dMSFlexGridEvents_SinkHelper, out pdwCookie);
			dMSFlexGridEvents_SinkHelper.m_dwCookie = pdwCookie;
			dMSFlexGridEvents_SinkHelper.m_MouseMoveDelegate = P_0;
			m_aEventSinkHelpers.Add(dMSFlexGridEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_MouseMove(DMSFlexGridEvents_MouseMoveEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = (DMSFlexGridEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dMSFlexGridEvents_SinkHelper.m_MouseMoveDelegate != null && ((dMSFlexGridEvents_SinkHelper.m_MouseMoveDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dMSFlexGridEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void add_MouseUp(DMSFlexGridEvents_MouseUpEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = new DMSFlexGridEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(dMSFlexGridEvents_SinkHelper, out pdwCookie);
			dMSFlexGridEvents_SinkHelper.m_dwCookie = pdwCookie;
			dMSFlexGridEvents_SinkHelper.m_MouseUpDelegate = P_0;
			m_aEventSinkHelpers.Add(dMSFlexGridEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_MouseUp(DMSFlexGridEvents_MouseUpEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = (DMSFlexGridEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dMSFlexGridEvents_SinkHelper.m_MouseUpDelegate != null && ((dMSFlexGridEvents_SinkHelper.m_MouseUpDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dMSFlexGridEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void add_SelChange(DMSFlexGridEvents_SelChangeEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = new DMSFlexGridEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(dMSFlexGridEvents_SinkHelper, out pdwCookie);
			dMSFlexGridEvents_SinkHelper.m_dwCookie = pdwCookie;
			dMSFlexGridEvents_SinkHelper.m_SelChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(dMSFlexGridEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_SelChange(DMSFlexGridEvents_SelChangeEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = (DMSFlexGridEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dMSFlexGridEvents_SinkHelper.m_SelChangeDelegate != null && ((dMSFlexGridEvents_SinkHelper.m_SelChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dMSFlexGridEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void add_RowColChange(DMSFlexGridEvents_RowColChangeEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = new DMSFlexGridEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(dMSFlexGridEvents_SinkHelper, out pdwCookie);
			dMSFlexGridEvents_SinkHelper.m_dwCookie = pdwCookie;
			dMSFlexGridEvents_SinkHelper.m_RowColChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(dMSFlexGridEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_RowColChange(DMSFlexGridEvents_RowColChangeEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = (DMSFlexGridEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dMSFlexGridEvents_SinkHelper.m_RowColChangeDelegate != null && ((dMSFlexGridEvents_SinkHelper.m_RowColChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dMSFlexGridEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void add_EnterCell(DMSFlexGridEvents_EnterCellEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = new DMSFlexGridEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(dMSFlexGridEvents_SinkHelper, out pdwCookie);
			dMSFlexGridEvents_SinkHelper.m_dwCookie = pdwCookie;
			dMSFlexGridEvents_SinkHelper.m_EnterCellDelegate = P_0;
			m_aEventSinkHelpers.Add(dMSFlexGridEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_EnterCell(DMSFlexGridEvents_EnterCellEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = (DMSFlexGridEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dMSFlexGridEvents_SinkHelper.m_EnterCellDelegate != null && ((dMSFlexGridEvents_SinkHelper.m_EnterCellDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dMSFlexGridEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void add_LeaveCell(DMSFlexGridEvents_LeaveCellEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = new DMSFlexGridEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(dMSFlexGridEvents_SinkHelper, out pdwCookie);
			dMSFlexGridEvents_SinkHelper.m_dwCookie = pdwCookie;
			dMSFlexGridEvents_SinkHelper.m_LeaveCellDelegate = P_0;
			m_aEventSinkHelpers.Add(dMSFlexGridEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_LeaveCell(DMSFlexGridEvents_LeaveCellEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = (DMSFlexGridEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dMSFlexGridEvents_SinkHelper.m_LeaveCellDelegate != null && ((dMSFlexGridEvents_SinkHelper.m_LeaveCellDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dMSFlexGridEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void add_Scroll(DMSFlexGridEvents_ScrollEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = new DMSFlexGridEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(dMSFlexGridEvents_SinkHelper, out pdwCookie);
			dMSFlexGridEvents_SinkHelper.m_dwCookie = pdwCookie;
			dMSFlexGridEvents_SinkHelper.m_ScrollDelegate = P_0;
			m_aEventSinkHelpers.Add(dMSFlexGridEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_Scroll(DMSFlexGridEvents_ScrollEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = (DMSFlexGridEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dMSFlexGridEvents_SinkHelper.m_ScrollDelegate != null && ((dMSFlexGridEvents_SinkHelper.m_ScrollDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dMSFlexGridEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void add_Compare(DMSFlexGridEvents_CompareEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = new DMSFlexGridEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(dMSFlexGridEvents_SinkHelper, out pdwCookie);
			dMSFlexGridEvents_SinkHelper.m_dwCookie = pdwCookie;
			dMSFlexGridEvents_SinkHelper.m_CompareDelegate = P_0;
			m_aEventSinkHelpers.Add(dMSFlexGridEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_Compare(DMSFlexGridEvents_CompareEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = (DMSFlexGridEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dMSFlexGridEvents_SinkHelper.m_CompareDelegate != null && ((dMSFlexGridEvents_SinkHelper.m_CompareDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dMSFlexGridEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void add_OLEStartDrag(DMSFlexGridEvents_OLEStartDragEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = new DMSFlexGridEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(dMSFlexGridEvents_SinkHelper, out pdwCookie);
			dMSFlexGridEvents_SinkHelper.m_dwCookie = pdwCookie;
			dMSFlexGridEvents_SinkHelper.m_OLEStartDragDelegate = P_0;
			m_aEventSinkHelpers.Add(dMSFlexGridEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_OLEStartDrag(DMSFlexGridEvents_OLEStartDragEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = (DMSFlexGridEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dMSFlexGridEvents_SinkHelper.m_OLEStartDragDelegate != null && ((dMSFlexGridEvents_SinkHelper.m_OLEStartDragDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dMSFlexGridEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void add_OLEGiveFeedback(DMSFlexGridEvents_OLEGiveFeedbackEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = new DMSFlexGridEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(dMSFlexGridEvents_SinkHelper, out pdwCookie);
			dMSFlexGridEvents_SinkHelper.m_dwCookie = pdwCookie;
			dMSFlexGridEvents_SinkHelper.m_OLEGiveFeedbackDelegate = P_0;
			m_aEventSinkHelpers.Add(dMSFlexGridEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_OLEGiveFeedback(DMSFlexGridEvents_OLEGiveFeedbackEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = (DMSFlexGridEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dMSFlexGridEvents_SinkHelper.m_OLEGiveFeedbackDelegate != null && ((dMSFlexGridEvents_SinkHelper.m_OLEGiveFeedbackDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dMSFlexGridEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void add_OLESetData(DMSFlexGridEvents_OLESetDataEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = new DMSFlexGridEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(dMSFlexGridEvents_SinkHelper, out pdwCookie);
			dMSFlexGridEvents_SinkHelper.m_dwCookie = pdwCookie;
			dMSFlexGridEvents_SinkHelper.m_OLESetDataDelegate = P_0;
			m_aEventSinkHelpers.Add(dMSFlexGridEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_OLESetData(DMSFlexGridEvents_OLESetDataEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = (DMSFlexGridEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dMSFlexGridEvents_SinkHelper.m_OLESetDataDelegate != null && ((dMSFlexGridEvents_SinkHelper.m_OLESetDataDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dMSFlexGridEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void add_OLECompleteDrag(DMSFlexGridEvents_OLECompleteDragEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = new DMSFlexGridEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(dMSFlexGridEvents_SinkHelper, out pdwCookie);
			dMSFlexGridEvents_SinkHelper.m_dwCookie = pdwCookie;
			dMSFlexGridEvents_SinkHelper.m_OLECompleteDragDelegate = P_0;
			m_aEventSinkHelpers.Add(dMSFlexGridEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_OLECompleteDrag(DMSFlexGridEvents_OLECompleteDragEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = (DMSFlexGridEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dMSFlexGridEvents_SinkHelper.m_OLECompleteDragDelegate != null && ((dMSFlexGridEvents_SinkHelper.m_OLECompleteDragDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dMSFlexGridEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void add_OLEDragOver(DMSFlexGridEvents_OLEDragOverEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = new DMSFlexGridEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(dMSFlexGridEvents_SinkHelper, out pdwCookie);
			dMSFlexGridEvents_SinkHelper.m_dwCookie = pdwCookie;
			dMSFlexGridEvents_SinkHelper.m_OLEDragOverDelegate = P_0;
			m_aEventSinkHelpers.Add(dMSFlexGridEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_OLEDragOver(DMSFlexGridEvents_OLEDragOverEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = (DMSFlexGridEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dMSFlexGridEvents_SinkHelper.m_OLEDragOverDelegate != null && ((dMSFlexGridEvents_SinkHelper.m_OLEDragOverDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dMSFlexGridEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void add_OLEDragDrop(DMSFlexGridEvents_OLEDragDropEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = new DMSFlexGridEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(dMSFlexGridEvents_SinkHelper, out pdwCookie);
			dMSFlexGridEvents_SinkHelper.m_dwCookie = pdwCookie;
			dMSFlexGridEvents_SinkHelper.m_OLEDragDropDelegate = P_0;
			m_aEventSinkHelpers.Add(dMSFlexGridEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_OLEDragDrop(DMSFlexGridEvents_OLEDragDropEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = (DMSFlexGridEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dMSFlexGridEvents_SinkHelper.m_OLEDragDropDelegate != null && ((dMSFlexGridEvents_SinkHelper.m_OLEDragDropDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dMSFlexGridEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public DMSFlexGridEvents_EventProvider(object P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		m_ConnectionPointContainer = (IConnectionPointContainer)P_0;
	}

	public void Finalize()
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 < count)
			{
				do
				{
					DMSFlexGridEvents_SinkHelper dMSFlexGridEvents_SinkHelper = (DMSFlexGridEvents_SinkHelper)m_aEventSinkHelpers[num];
					m_ConnectionPoint.Unadvise(dMSFlexGridEvents_SinkHelper.m_dwCookie);
					num++;
				}
				while (num < count);
			}
			Marshal.ReleaseComObject(m_ConnectionPoint);
		}
		catch (Exception)
		{
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void Dispose()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		Finalize();
		GC.SuppressFinalize(this);
	}
}
