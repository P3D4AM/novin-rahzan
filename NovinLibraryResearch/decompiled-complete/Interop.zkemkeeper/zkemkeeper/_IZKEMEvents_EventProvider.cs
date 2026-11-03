using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

namespace zkemkeeper;

internal sealed class _IZKEMEvents_EventProvider : _IZKEMEvents_Event, IDisposable
{
	private IConnectionPointContainer m_ConnectionPointContainer;

	private ArrayList m_aEventSinkHelpers;

	private IConnectionPoint m_ConnectionPoint;

	private void Init()
	{
		IConnectionPoint ppCP = null;
		Guid riid = new Guid(new byte[16]
		{
			128, 181, 131, 207, 50, 93, 101, 76, 180, 78,
			190, 220, 117, 12, 223, 168
		});
		m_ConnectionPointContainer.FindConnectionPoint(ref riid, out ppCP);
		m_ConnectionPoint = ppCP;
		m_aEventSinkHelpers = new ArrayList();
	}

	public void add_OnAttTransaction(_IZKEMEvents_OnAttTransactionEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = new _IZKEMEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iZKEMEvents_SinkHelper, out pdwCookie);
			iZKEMEvents_SinkHelper.m_dwCookie = pdwCookie;
			iZKEMEvents_SinkHelper.m_OnAttTransactionDelegate = P_0;
			m_aEventSinkHelpers.Add(iZKEMEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnAttTransaction(_IZKEMEvents_OnAttTransactionEventHandler P_0)
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
				_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = (_IZKEMEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iZKEMEvents_SinkHelper.m_OnAttTransactionDelegate != null && ((iZKEMEvents_SinkHelper.m_OnAttTransactionDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iZKEMEvents_SinkHelper.m_dwCookie);
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

	public void add_OnKeyPress(_IZKEMEvents_OnKeyPressEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = new _IZKEMEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iZKEMEvents_SinkHelper, out pdwCookie);
			iZKEMEvents_SinkHelper.m_dwCookie = pdwCookie;
			iZKEMEvents_SinkHelper.m_OnKeyPressDelegate = P_0;
			m_aEventSinkHelpers.Add(iZKEMEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnKeyPress(_IZKEMEvents_OnKeyPressEventHandler P_0)
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
				_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = (_IZKEMEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iZKEMEvents_SinkHelper.m_OnKeyPressDelegate != null && ((iZKEMEvents_SinkHelper.m_OnKeyPressDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iZKEMEvents_SinkHelper.m_dwCookie);
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

	public void add_OnEnrollFinger(_IZKEMEvents_OnEnrollFingerEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = new _IZKEMEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iZKEMEvents_SinkHelper, out pdwCookie);
			iZKEMEvents_SinkHelper.m_dwCookie = pdwCookie;
			iZKEMEvents_SinkHelper.m_OnEnrollFingerDelegate = P_0;
			m_aEventSinkHelpers.Add(iZKEMEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnEnrollFinger(_IZKEMEvents_OnEnrollFingerEventHandler P_0)
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
				_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = (_IZKEMEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iZKEMEvents_SinkHelper.m_OnEnrollFingerDelegate != null && ((iZKEMEvents_SinkHelper.m_OnEnrollFingerDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iZKEMEvents_SinkHelper.m_dwCookie);
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

	public void add_OnNewUser(_IZKEMEvents_OnNewUserEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = new _IZKEMEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iZKEMEvents_SinkHelper, out pdwCookie);
			iZKEMEvents_SinkHelper.m_dwCookie = pdwCookie;
			iZKEMEvents_SinkHelper.m_OnNewUserDelegate = P_0;
			m_aEventSinkHelpers.Add(iZKEMEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnNewUser(_IZKEMEvents_OnNewUserEventHandler P_0)
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
				_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = (_IZKEMEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iZKEMEvents_SinkHelper.m_OnNewUserDelegate != null && ((iZKEMEvents_SinkHelper.m_OnNewUserDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iZKEMEvents_SinkHelper.m_dwCookie);
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

	public void add_OnEMData(_IZKEMEvents_OnEMDataEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = new _IZKEMEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iZKEMEvents_SinkHelper, out pdwCookie);
			iZKEMEvents_SinkHelper.m_dwCookie = pdwCookie;
			iZKEMEvents_SinkHelper.m_OnEMDataDelegate = P_0;
			m_aEventSinkHelpers.Add(iZKEMEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnEMData(_IZKEMEvents_OnEMDataEventHandler P_0)
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
				_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = (_IZKEMEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iZKEMEvents_SinkHelper.m_OnEMDataDelegate != null && ((iZKEMEvents_SinkHelper.m_OnEMDataDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iZKEMEvents_SinkHelper.m_dwCookie);
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

	public void add_OnConnected(_IZKEMEvents_OnConnectedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = new _IZKEMEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iZKEMEvents_SinkHelper, out pdwCookie);
			iZKEMEvents_SinkHelper.m_dwCookie = pdwCookie;
			iZKEMEvents_SinkHelper.m_OnConnectedDelegate = P_0;
			m_aEventSinkHelpers.Add(iZKEMEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnConnected(_IZKEMEvents_OnConnectedEventHandler P_0)
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
				_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = (_IZKEMEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iZKEMEvents_SinkHelper.m_OnConnectedDelegate != null && ((iZKEMEvents_SinkHelper.m_OnConnectedDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iZKEMEvents_SinkHelper.m_dwCookie);
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

	public void add_OnDisConnected(_IZKEMEvents_OnDisConnectedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = new _IZKEMEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iZKEMEvents_SinkHelper, out pdwCookie);
			iZKEMEvents_SinkHelper.m_dwCookie = pdwCookie;
			iZKEMEvents_SinkHelper.m_OnDisConnectedDelegate = P_0;
			m_aEventSinkHelpers.Add(iZKEMEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnDisConnected(_IZKEMEvents_OnDisConnectedEventHandler P_0)
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
				_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = (_IZKEMEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iZKEMEvents_SinkHelper.m_OnDisConnectedDelegate != null && ((iZKEMEvents_SinkHelper.m_OnDisConnectedDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iZKEMEvents_SinkHelper.m_dwCookie);
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

	public void add_OnFinger(_IZKEMEvents_OnFingerEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = new _IZKEMEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iZKEMEvents_SinkHelper, out pdwCookie);
			iZKEMEvents_SinkHelper.m_dwCookie = pdwCookie;
			iZKEMEvents_SinkHelper.m_OnFingerDelegate = P_0;
			m_aEventSinkHelpers.Add(iZKEMEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnFinger(_IZKEMEvents_OnFingerEventHandler P_0)
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
				_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = (_IZKEMEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iZKEMEvents_SinkHelper.m_OnFingerDelegate != null && ((iZKEMEvents_SinkHelper.m_OnFingerDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iZKEMEvents_SinkHelper.m_dwCookie);
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

	public void add_OnVerify(_IZKEMEvents_OnVerifyEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = new _IZKEMEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iZKEMEvents_SinkHelper, out pdwCookie);
			iZKEMEvents_SinkHelper.m_dwCookie = pdwCookie;
			iZKEMEvents_SinkHelper.m_OnVerifyDelegate = P_0;
			m_aEventSinkHelpers.Add(iZKEMEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnVerify(_IZKEMEvents_OnVerifyEventHandler P_0)
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
				_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = (_IZKEMEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iZKEMEvents_SinkHelper.m_OnVerifyDelegate != null && ((iZKEMEvents_SinkHelper.m_OnVerifyDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iZKEMEvents_SinkHelper.m_dwCookie);
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

	public void add_OnFingerFeature(_IZKEMEvents_OnFingerFeatureEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = new _IZKEMEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iZKEMEvents_SinkHelper, out pdwCookie);
			iZKEMEvents_SinkHelper.m_dwCookie = pdwCookie;
			iZKEMEvents_SinkHelper.m_OnFingerFeatureDelegate = P_0;
			m_aEventSinkHelpers.Add(iZKEMEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnFingerFeature(_IZKEMEvents_OnFingerFeatureEventHandler P_0)
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
				_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = (_IZKEMEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iZKEMEvents_SinkHelper.m_OnFingerFeatureDelegate != null && ((iZKEMEvents_SinkHelper.m_OnFingerFeatureDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iZKEMEvents_SinkHelper.m_dwCookie);
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

	public void add_OnHIDNum(_IZKEMEvents_OnHIDNumEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = new _IZKEMEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iZKEMEvents_SinkHelper, out pdwCookie);
			iZKEMEvents_SinkHelper.m_dwCookie = pdwCookie;
			iZKEMEvents_SinkHelper.m_OnHIDNumDelegate = P_0;
			m_aEventSinkHelpers.Add(iZKEMEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnHIDNum(_IZKEMEvents_OnHIDNumEventHandler P_0)
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
				_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = (_IZKEMEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iZKEMEvents_SinkHelper.m_OnHIDNumDelegate != null && ((iZKEMEvents_SinkHelper.m_OnHIDNumDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iZKEMEvents_SinkHelper.m_dwCookie);
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

	public void add_OnDoor(_IZKEMEvents_OnDoorEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = new _IZKEMEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iZKEMEvents_SinkHelper, out pdwCookie);
			iZKEMEvents_SinkHelper.m_dwCookie = pdwCookie;
			iZKEMEvents_SinkHelper.m_OnDoorDelegate = P_0;
			m_aEventSinkHelpers.Add(iZKEMEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnDoor(_IZKEMEvents_OnDoorEventHandler P_0)
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
				_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = (_IZKEMEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iZKEMEvents_SinkHelper.m_OnDoorDelegate != null && ((iZKEMEvents_SinkHelper.m_OnDoorDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iZKEMEvents_SinkHelper.m_dwCookie);
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

	public void add_OnAlarm(_IZKEMEvents_OnAlarmEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = new _IZKEMEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iZKEMEvents_SinkHelper, out pdwCookie);
			iZKEMEvents_SinkHelper.m_dwCookie = pdwCookie;
			iZKEMEvents_SinkHelper.m_OnAlarmDelegate = P_0;
			m_aEventSinkHelpers.Add(iZKEMEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnAlarm(_IZKEMEvents_OnAlarmEventHandler P_0)
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
				_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = (_IZKEMEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iZKEMEvents_SinkHelper.m_OnAlarmDelegate != null && ((iZKEMEvents_SinkHelper.m_OnAlarmDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iZKEMEvents_SinkHelper.m_dwCookie);
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

	public void add_OnWriteCard(_IZKEMEvents_OnWriteCardEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = new _IZKEMEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iZKEMEvents_SinkHelper, out pdwCookie);
			iZKEMEvents_SinkHelper.m_dwCookie = pdwCookie;
			iZKEMEvents_SinkHelper.m_OnWriteCardDelegate = P_0;
			m_aEventSinkHelpers.Add(iZKEMEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnWriteCard(_IZKEMEvents_OnWriteCardEventHandler P_0)
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
				_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = (_IZKEMEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iZKEMEvents_SinkHelper.m_OnWriteCardDelegate != null && ((iZKEMEvents_SinkHelper.m_OnWriteCardDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iZKEMEvents_SinkHelper.m_dwCookie);
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

	public void add_OnEmptyCard(_IZKEMEvents_OnEmptyCardEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = new _IZKEMEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iZKEMEvents_SinkHelper, out pdwCookie);
			iZKEMEvents_SinkHelper.m_dwCookie = pdwCookie;
			iZKEMEvents_SinkHelper.m_OnEmptyCardDelegate = P_0;
			m_aEventSinkHelpers.Add(iZKEMEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnEmptyCard(_IZKEMEvents_OnEmptyCardEventHandler P_0)
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
				_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = (_IZKEMEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iZKEMEvents_SinkHelper.m_OnEmptyCardDelegate != null && ((iZKEMEvents_SinkHelper.m_OnEmptyCardDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iZKEMEvents_SinkHelper.m_dwCookie);
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

	public void add_OnDeleteTemplate(_IZKEMEvents_OnDeleteTemplateEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = new _IZKEMEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iZKEMEvents_SinkHelper, out pdwCookie);
			iZKEMEvents_SinkHelper.m_dwCookie = pdwCookie;
			iZKEMEvents_SinkHelper.m_OnDeleteTemplateDelegate = P_0;
			m_aEventSinkHelpers.Add(iZKEMEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnDeleteTemplate(_IZKEMEvents_OnDeleteTemplateEventHandler P_0)
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
				_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = (_IZKEMEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iZKEMEvents_SinkHelper.m_OnDeleteTemplateDelegate != null && ((iZKEMEvents_SinkHelper.m_OnDeleteTemplateDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iZKEMEvents_SinkHelper.m_dwCookie);
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

	public void add_OnAttTransactionEx(_IZKEMEvents_OnAttTransactionExEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = new _IZKEMEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iZKEMEvents_SinkHelper, out pdwCookie);
			iZKEMEvents_SinkHelper.m_dwCookie = pdwCookie;
			iZKEMEvents_SinkHelper.m_OnAttTransactionExDelegate = P_0;
			m_aEventSinkHelpers.Add(iZKEMEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnAttTransactionEx(_IZKEMEvents_OnAttTransactionExEventHandler P_0)
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
				_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = (_IZKEMEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iZKEMEvents_SinkHelper.m_OnAttTransactionExDelegate != null && ((iZKEMEvents_SinkHelper.m_OnAttTransactionExDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iZKEMEvents_SinkHelper.m_dwCookie);
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

	public void add_OnEnrollFingerEx(_IZKEMEvents_OnEnrollFingerExEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = new _IZKEMEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iZKEMEvents_SinkHelper, out pdwCookie);
			iZKEMEvents_SinkHelper.m_dwCookie = pdwCookie;
			iZKEMEvents_SinkHelper.m_OnEnrollFingerExDelegate = P_0;
			m_aEventSinkHelpers.Add(iZKEMEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnEnrollFingerEx(_IZKEMEvents_OnEnrollFingerExEventHandler P_0)
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
				_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = (_IZKEMEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iZKEMEvents_SinkHelper.m_OnEnrollFingerExDelegate != null && ((iZKEMEvents_SinkHelper.m_OnEnrollFingerExDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iZKEMEvents_SinkHelper.m_dwCookie);
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

	public _IZKEMEvents_EventProvider(object P_0)
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
					_IZKEMEvents_SinkHelper iZKEMEvents_SinkHelper = (_IZKEMEvents_SinkHelper)m_aEventSinkHelpers[num];
					m_ConnectionPoint.Unadvise(iZKEMEvents_SinkHelper.m_dwCookie);
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
