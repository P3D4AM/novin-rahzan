using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

namespace MS_SMS;

internal sealed class IMS_SMS_BANEvents_EventProvider : IMS_SMS_BANEvents_Event, IDisposable
{
	private IConnectionPointContainer m_ConnectionPointContainer;

	private ArrayList m_aEventSinkHelpers;

	private IConnectionPoint m_ConnectionPoint;

	private void Init()
	{
		IConnectionPoint ppCP = null;
		Guid riid = new Guid(new byte[16]
		{
			220, 74, 85, 131, 110, 73, 81, 67, 154, 205,
			29, 79, 133, 96, 63, 217
		});
		m_ConnectionPointContainer.FindConnectionPoint(ref riid, out ppCP);
		m_ConnectionPoint = ppCP;
		m_aEventSinkHelpers = new ArrayList();
	}

	public void add_OnActivate(IMS_SMS_BANEvents_OnActivateEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IMS_SMS_BANEvents_SinkHelper iMS_SMS_BANEvents_SinkHelper = new IMS_SMS_BANEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iMS_SMS_BANEvents_SinkHelper, out pdwCookie);
			iMS_SMS_BANEvents_SinkHelper.m_dwCookie = pdwCookie;
			iMS_SMS_BANEvents_SinkHelper.m_OnActivateDelegate = P_0;
			m_aEventSinkHelpers.Add(iMS_SMS_BANEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnActivate(IMS_SMS_BANEvents_OnActivateEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
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
				IMS_SMS_BANEvents_SinkHelper iMS_SMS_BANEvents_SinkHelper = (IMS_SMS_BANEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iMS_SMS_BANEvents_SinkHelper.m_OnActivateDelegate != null && ((iMS_SMS_BANEvents_SinkHelper.m_OnActivateDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iMS_SMS_BANEvents_SinkHelper.m_dwCookie);
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
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_OnAfterMonitorDpiChanged(IMS_SMS_BANEvents_OnAfterMonitorDpiChangedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IMS_SMS_BANEvents_SinkHelper iMS_SMS_BANEvents_SinkHelper = new IMS_SMS_BANEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iMS_SMS_BANEvents_SinkHelper, out pdwCookie);
			iMS_SMS_BANEvents_SinkHelper.m_dwCookie = pdwCookie;
			iMS_SMS_BANEvents_SinkHelper.m_OnAfterMonitorDpiChangedDelegate = P_0;
			m_aEventSinkHelpers.Add(iMS_SMS_BANEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnAfterMonitorDpiChanged(IMS_SMS_BANEvents_OnAfterMonitorDpiChangedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
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
				IMS_SMS_BANEvents_SinkHelper iMS_SMS_BANEvents_SinkHelper = (IMS_SMS_BANEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iMS_SMS_BANEvents_SinkHelper.m_OnAfterMonitorDpiChangedDelegate != null && ((iMS_SMS_BANEvents_SinkHelper.m_OnAfterMonitorDpiChangedDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iMS_SMS_BANEvents_SinkHelper.m_dwCookie);
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
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_OnBeforeMonitorDpiChanged(IMS_SMS_BANEvents_OnBeforeMonitorDpiChangedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IMS_SMS_BANEvents_SinkHelper iMS_SMS_BANEvents_SinkHelper = new IMS_SMS_BANEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iMS_SMS_BANEvents_SinkHelper, out pdwCookie);
			iMS_SMS_BANEvents_SinkHelper.m_dwCookie = pdwCookie;
			iMS_SMS_BANEvents_SinkHelper.m_OnBeforeMonitorDpiChangedDelegate = P_0;
			m_aEventSinkHelpers.Add(iMS_SMS_BANEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnBeforeMonitorDpiChanged(IMS_SMS_BANEvents_OnBeforeMonitorDpiChangedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
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
				IMS_SMS_BANEvents_SinkHelper iMS_SMS_BANEvents_SinkHelper = (IMS_SMS_BANEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iMS_SMS_BANEvents_SinkHelper.m_OnBeforeMonitorDpiChangedDelegate != null && ((iMS_SMS_BANEvents_SinkHelper.m_OnBeforeMonitorDpiChangedDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iMS_SMS_BANEvents_SinkHelper.m_dwCookie);
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
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_OnClick(IMS_SMS_BANEvents_OnClickEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IMS_SMS_BANEvents_SinkHelper iMS_SMS_BANEvents_SinkHelper = new IMS_SMS_BANEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iMS_SMS_BANEvents_SinkHelper, out pdwCookie);
			iMS_SMS_BANEvents_SinkHelper.m_dwCookie = pdwCookie;
			iMS_SMS_BANEvents_SinkHelper.m_OnClickDelegate = P_0;
			m_aEventSinkHelpers.Add(iMS_SMS_BANEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnClick(IMS_SMS_BANEvents_OnClickEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
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
				IMS_SMS_BANEvents_SinkHelper iMS_SMS_BANEvents_SinkHelper = (IMS_SMS_BANEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iMS_SMS_BANEvents_SinkHelper.m_OnClickDelegate != null && ((iMS_SMS_BANEvents_SinkHelper.m_OnClickDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iMS_SMS_BANEvents_SinkHelper.m_dwCookie);
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
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_OnCreate(IMS_SMS_BANEvents_OnCreateEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IMS_SMS_BANEvents_SinkHelper iMS_SMS_BANEvents_SinkHelper = new IMS_SMS_BANEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iMS_SMS_BANEvents_SinkHelper, out pdwCookie);
			iMS_SMS_BANEvents_SinkHelper.m_dwCookie = pdwCookie;
			iMS_SMS_BANEvents_SinkHelper.m_OnCreateDelegate = P_0;
			m_aEventSinkHelpers.Add(iMS_SMS_BANEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnCreate(IMS_SMS_BANEvents_OnCreateEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
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
				IMS_SMS_BANEvents_SinkHelper iMS_SMS_BANEvents_SinkHelper = (IMS_SMS_BANEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iMS_SMS_BANEvents_SinkHelper.m_OnCreateDelegate != null && ((iMS_SMS_BANEvents_SinkHelper.m_OnCreateDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iMS_SMS_BANEvents_SinkHelper.m_dwCookie);
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
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_OnDblClick(IMS_SMS_BANEvents_OnDblClickEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IMS_SMS_BANEvents_SinkHelper iMS_SMS_BANEvents_SinkHelper = new IMS_SMS_BANEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iMS_SMS_BANEvents_SinkHelper, out pdwCookie);
			iMS_SMS_BANEvents_SinkHelper.m_dwCookie = pdwCookie;
			iMS_SMS_BANEvents_SinkHelper.m_OnDblClickDelegate = P_0;
			m_aEventSinkHelpers.Add(iMS_SMS_BANEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnDblClick(IMS_SMS_BANEvents_OnDblClickEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
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
				IMS_SMS_BANEvents_SinkHelper iMS_SMS_BANEvents_SinkHelper = (IMS_SMS_BANEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iMS_SMS_BANEvents_SinkHelper.m_OnDblClickDelegate != null && ((iMS_SMS_BANEvents_SinkHelper.m_OnDblClickDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iMS_SMS_BANEvents_SinkHelper.m_dwCookie);
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
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_OnDestroy(IMS_SMS_BANEvents_OnDestroyEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IMS_SMS_BANEvents_SinkHelper iMS_SMS_BANEvents_SinkHelper = new IMS_SMS_BANEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iMS_SMS_BANEvents_SinkHelper, out pdwCookie);
			iMS_SMS_BANEvents_SinkHelper.m_dwCookie = pdwCookie;
			iMS_SMS_BANEvents_SinkHelper.m_OnDestroyDelegate = P_0;
			m_aEventSinkHelpers.Add(iMS_SMS_BANEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnDestroy(IMS_SMS_BANEvents_OnDestroyEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
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
				IMS_SMS_BANEvents_SinkHelper iMS_SMS_BANEvents_SinkHelper = (IMS_SMS_BANEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iMS_SMS_BANEvents_SinkHelper.m_OnDestroyDelegate != null && ((iMS_SMS_BANEvents_SinkHelper.m_OnDestroyDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iMS_SMS_BANEvents_SinkHelper.m_dwCookie);
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
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_OnDeactivate(IMS_SMS_BANEvents_OnDeactivateEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IMS_SMS_BANEvents_SinkHelper iMS_SMS_BANEvents_SinkHelper = new IMS_SMS_BANEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iMS_SMS_BANEvents_SinkHelper, out pdwCookie);
			iMS_SMS_BANEvents_SinkHelper.m_dwCookie = pdwCookie;
			iMS_SMS_BANEvents_SinkHelper.m_OnDeactivateDelegate = P_0;
			m_aEventSinkHelpers.Add(iMS_SMS_BANEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnDeactivate(IMS_SMS_BANEvents_OnDeactivateEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
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
				IMS_SMS_BANEvents_SinkHelper iMS_SMS_BANEvents_SinkHelper = (IMS_SMS_BANEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iMS_SMS_BANEvents_SinkHelper.m_OnDeactivateDelegate != null && ((iMS_SMS_BANEvents_SinkHelper.m_OnDeactivateDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iMS_SMS_BANEvents_SinkHelper.m_dwCookie);
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
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_OnKeyPress(IMS_SMS_BANEvents_OnKeyPressEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IMS_SMS_BANEvents_SinkHelper iMS_SMS_BANEvents_SinkHelper = new IMS_SMS_BANEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iMS_SMS_BANEvents_SinkHelper, out pdwCookie);
			iMS_SMS_BANEvents_SinkHelper.m_dwCookie = pdwCookie;
			iMS_SMS_BANEvents_SinkHelper.m_OnKeyPressDelegate = P_0;
			m_aEventSinkHelpers.Add(iMS_SMS_BANEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnKeyPress(IMS_SMS_BANEvents_OnKeyPressEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
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
				IMS_SMS_BANEvents_SinkHelper iMS_SMS_BANEvents_SinkHelper = (IMS_SMS_BANEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iMS_SMS_BANEvents_SinkHelper.m_OnKeyPressDelegate != null && ((iMS_SMS_BANEvents_SinkHelper.m_OnKeyPressDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iMS_SMS_BANEvents_SinkHelper.m_dwCookie);
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
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_OnMouseEnter(IMS_SMS_BANEvents_OnMouseEnterEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IMS_SMS_BANEvents_SinkHelper iMS_SMS_BANEvents_SinkHelper = new IMS_SMS_BANEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iMS_SMS_BANEvents_SinkHelper, out pdwCookie);
			iMS_SMS_BANEvents_SinkHelper.m_dwCookie = pdwCookie;
			iMS_SMS_BANEvents_SinkHelper.m_OnMouseEnterDelegate = P_0;
			m_aEventSinkHelpers.Add(iMS_SMS_BANEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnMouseEnter(IMS_SMS_BANEvents_OnMouseEnterEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
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
				IMS_SMS_BANEvents_SinkHelper iMS_SMS_BANEvents_SinkHelper = (IMS_SMS_BANEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iMS_SMS_BANEvents_SinkHelper.m_OnMouseEnterDelegate != null && ((iMS_SMS_BANEvents_SinkHelper.m_OnMouseEnterDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iMS_SMS_BANEvents_SinkHelper.m_dwCookie);
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
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_OnMouseLeave(IMS_SMS_BANEvents_OnMouseLeaveEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IMS_SMS_BANEvents_SinkHelper iMS_SMS_BANEvents_SinkHelper = new IMS_SMS_BANEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iMS_SMS_BANEvents_SinkHelper, out pdwCookie);
			iMS_SMS_BANEvents_SinkHelper.m_dwCookie = pdwCookie;
			iMS_SMS_BANEvents_SinkHelper.m_OnMouseLeaveDelegate = P_0;
			m_aEventSinkHelpers.Add(iMS_SMS_BANEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnMouseLeave(IMS_SMS_BANEvents_OnMouseLeaveEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
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
				IMS_SMS_BANEvents_SinkHelper iMS_SMS_BANEvents_SinkHelper = (IMS_SMS_BANEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iMS_SMS_BANEvents_SinkHelper.m_OnMouseLeaveDelegate != null && ((iMS_SMS_BANEvents_SinkHelper.m_OnMouseLeaveDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iMS_SMS_BANEvents_SinkHelper.m_dwCookie);
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
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_OnPaint(IMS_SMS_BANEvents_OnPaintEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IMS_SMS_BANEvents_SinkHelper iMS_SMS_BANEvents_SinkHelper = new IMS_SMS_BANEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iMS_SMS_BANEvents_SinkHelper, out pdwCookie);
			iMS_SMS_BANEvents_SinkHelper.m_dwCookie = pdwCookie;
			iMS_SMS_BANEvents_SinkHelper.m_OnPaintDelegate = P_0;
			m_aEventSinkHelpers.Add(iMS_SMS_BANEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnPaint(IMS_SMS_BANEvents_OnPaintEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
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
				IMS_SMS_BANEvents_SinkHelper iMS_SMS_BANEvents_SinkHelper = (IMS_SMS_BANEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iMS_SMS_BANEvents_SinkHelper.m_OnPaintDelegate != null && ((iMS_SMS_BANEvents_SinkHelper.m_OnPaintDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iMS_SMS_BANEvents_SinkHelper.m_dwCookie);
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
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_OnSMS(IMS_SMS_BANEvents_OnSMSEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IMS_SMS_BANEvents_SinkHelper iMS_SMS_BANEvents_SinkHelper = new IMS_SMS_BANEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iMS_SMS_BANEvents_SinkHelper, out pdwCookie);
			iMS_SMS_BANEvents_SinkHelper.m_dwCookie = pdwCookie;
			iMS_SMS_BANEvents_SinkHelper.m_OnSMSDelegate = P_0;
			m_aEventSinkHelpers.Add(iMS_SMS_BANEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnSMS(IMS_SMS_BANEvents_OnSMSEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
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
				IMS_SMS_BANEvents_SinkHelper iMS_SMS_BANEvents_SinkHelper = (IMS_SMS_BANEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iMS_SMS_BANEvents_SinkHelper.m_OnSMSDelegate != null && ((iMS_SMS_BANEvents_SinkHelper.m_OnSMSDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iMS_SMS_BANEvents_SinkHelper.m_dwCookie);
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
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_OnGSMEvent(IMS_SMS_BANEvents_OnGSMEventEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IMS_SMS_BANEvents_SinkHelper iMS_SMS_BANEvents_SinkHelper = new IMS_SMS_BANEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iMS_SMS_BANEvents_SinkHelper, out pdwCookie);
			iMS_SMS_BANEvents_SinkHelper.m_dwCookie = pdwCookie;
			iMS_SMS_BANEvents_SinkHelper.m_OnGSMEventDelegate = P_0;
			m_aEventSinkHelpers.Add(iMS_SMS_BANEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnGSMEvent(IMS_SMS_BANEvents_OnGSMEventEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
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
				IMS_SMS_BANEvents_SinkHelper iMS_SMS_BANEvents_SinkHelper = (IMS_SMS_BANEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iMS_SMS_BANEvents_SinkHelper.m_OnGSMEventDelegate != null && ((iMS_SMS_BANEvents_SinkHelper.m_OnGSMEventDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iMS_SMS_BANEvents_SinkHelper.m_dwCookie);
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
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public IMS_SMS_BANEvents_EventProvider(object P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		m_ConnectionPointContainer = (IConnectionPointContainer)P_0;
	}

	public void Finalize()
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
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
					IMS_SMS_BANEvents_SinkHelper iMS_SMS_BANEvents_SinkHelper = (IMS_SMS_BANEvents_SinkHelper)m_aEventSinkHelpers[num];
					m_ConnectionPoint.Unadvise(iMS_SMS_BANEvents_SinkHelper.m_dwCookie);
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
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void Dispose()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		Finalize();
		GC.SuppressFinalize(this);
	}
}
