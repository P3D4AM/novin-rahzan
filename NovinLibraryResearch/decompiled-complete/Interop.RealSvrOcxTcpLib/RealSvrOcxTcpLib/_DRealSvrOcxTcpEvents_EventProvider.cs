using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

namespace RealSvrOcxTcpLib;

internal sealed class _DRealSvrOcxTcpEvents_EventProvider : _DRealSvrOcxTcpEvents_Event, IDisposable
{
	private IConnectionPointContainer m_ConnectionPointContainer;

	private ArrayList m_aEventSinkHelpers;

	private IConnectionPoint m_ConnectionPoint;

	private void Init()
	{
		IConnectionPoint ppCP = null;
		Guid riid = new Guid(new byte[16]
		{
			202, 40, 8, 205, 245, 213, 180, 72, 176, 124,
			147, 182, 84, 92, 25, 244
		});
		m_ConnectionPointContainer.FindConnectionPoint(ref riid, out ppCP);
		m_ConnectionPoint = ppCP;
		m_aEventSinkHelpers = new ArrayList();
	}

	public void add_OnReceiveGLogData(_DRealSvrOcxTcpEvents_OnReceiveGLogDataEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_DRealSvrOcxTcpEvents_SinkHelper dRealSvrOcxTcpEvents_SinkHelper = new _DRealSvrOcxTcpEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(dRealSvrOcxTcpEvents_SinkHelper, out pdwCookie);
			dRealSvrOcxTcpEvents_SinkHelper.m_dwCookie = pdwCookie;
			dRealSvrOcxTcpEvents_SinkHelper.m_OnReceiveGLogDataDelegate = P_0;
			m_aEventSinkHelpers.Add(dRealSvrOcxTcpEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnReceiveGLogData(_DRealSvrOcxTcpEvents_OnReceiveGLogDataEventHandler P_0)
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
				_DRealSvrOcxTcpEvents_SinkHelper dRealSvrOcxTcpEvents_SinkHelper = (_DRealSvrOcxTcpEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dRealSvrOcxTcpEvents_SinkHelper.m_OnReceiveGLogDataDelegate != null && ((dRealSvrOcxTcpEvents_SinkHelper.m_OnReceiveGLogDataDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dRealSvrOcxTcpEvents_SinkHelper.m_dwCookie);
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

	public void add_OnReceiveGLogDataExtend(_DRealSvrOcxTcpEvents_OnReceiveGLogDataExtendEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_DRealSvrOcxTcpEvents_SinkHelper dRealSvrOcxTcpEvents_SinkHelper = new _DRealSvrOcxTcpEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(dRealSvrOcxTcpEvents_SinkHelper, out pdwCookie);
			dRealSvrOcxTcpEvents_SinkHelper.m_dwCookie = pdwCookie;
			dRealSvrOcxTcpEvents_SinkHelper.m_OnReceiveGLogDataExtendDelegate = P_0;
			m_aEventSinkHelpers.Add(dRealSvrOcxTcpEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnReceiveGLogDataExtend(_DRealSvrOcxTcpEvents_OnReceiveGLogDataExtendEventHandler P_0)
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
				_DRealSvrOcxTcpEvents_SinkHelper dRealSvrOcxTcpEvents_SinkHelper = (_DRealSvrOcxTcpEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dRealSvrOcxTcpEvents_SinkHelper.m_OnReceiveGLogDataExtendDelegate != null && ((dRealSvrOcxTcpEvents_SinkHelper.m_OnReceiveGLogDataExtendDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dRealSvrOcxTcpEvents_SinkHelper.m_dwCookie);
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

	public void add_OnReceiveGLogTextAndImage(_DRealSvrOcxTcpEvents_OnReceiveGLogTextAndImageEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_DRealSvrOcxTcpEvents_SinkHelper dRealSvrOcxTcpEvents_SinkHelper = new _DRealSvrOcxTcpEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(dRealSvrOcxTcpEvents_SinkHelper, out pdwCookie);
			dRealSvrOcxTcpEvents_SinkHelper.m_dwCookie = pdwCookie;
			dRealSvrOcxTcpEvents_SinkHelper.m_OnReceiveGLogTextAndImageDelegate = P_0;
			m_aEventSinkHelpers.Add(dRealSvrOcxTcpEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnReceiveGLogTextAndImage(_DRealSvrOcxTcpEvents_OnReceiveGLogTextAndImageEventHandler P_0)
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
				_DRealSvrOcxTcpEvents_SinkHelper dRealSvrOcxTcpEvents_SinkHelper = (_DRealSvrOcxTcpEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dRealSvrOcxTcpEvents_SinkHelper.m_OnReceiveGLogTextAndImageDelegate != null && ((dRealSvrOcxTcpEvents_SinkHelper.m_OnReceiveGLogTextAndImageDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dRealSvrOcxTcpEvents_SinkHelper.m_dwCookie);
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

	public void add_OnReceiveGLogText(_DRealSvrOcxTcpEvents_OnReceiveGLogTextEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_DRealSvrOcxTcpEvents_SinkHelper dRealSvrOcxTcpEvents_SinkHelper = new _DRealSvrOcxTcpEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(dRealSvrOcxTcpEvents_SinkHelper, out pdwCookie);
			dRealSvrOcxTcpEvents_SinkHelper.m_dwCookie = pdwCookie;
			dRealSvrOcxTcpEvents_SinkHelper.m_OnReceiveGLogTextDelegate = P_0;
			m_aEventSinkHelpers.Add(dRealSvrOcxTcpEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnReceiveGLogText(_DRealSvrOcxTcpEvents_OnReceiveGLogTextEventHandler P_0)
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
				_DRealSvrOcxTcpEvents_SinkHelper dRealSvrOcxTcpEvents_SinkHelper = (_DRealSvrOcxTcpEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dRealSvrOcxTcpEvents_SinkHelper.m_OnReceiveGLogTextDelegate != null && ((dRealSvrOcxTcpEvents_SinkHelper.m_OnReceiveGLogTextDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dRealSvrOcxTcpEvents_SinkHelper.m_dwCookie);
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

	public void add_OnReceiveGLogTextOnDoorOpen(_DRealSvrOcxTcpEvents_OnReceiveGLogTextOnDoorOpenEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_DRealSvrOcxTcpEvents_SinkHelper dRealSvrOcxTcpEvents_SinkHelper = new _DRealSvrOcxTcpEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(dRealSvrOcxTcpEvents_SinkHelper, out pdwCookie);
			dRealSvrOcxTcpEvents_SinkHelper.m_dwCookie = pdwCookie;
			dRealSvrOcxTcpEvents_SinkHelper.m_OnReceiveGLogTextOnDoorOpenDelegate = P_0;
			m_aEventSinkHelpers.Add(dRealSvrOcxTcpEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OnReceiveGLogTextOnDoorOpen(_DRealSvrOcxTcpEvents_OnReceiveGLogTextOnDoorOpenEventHandler P_0)
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
				_DRealSvrOcxTcpEvents_SinkHelper dRealSvrOcxTcpEvents_SinkHelper = (_DRealSvrOcxTcpEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dRealSvrOcxTcpEvents_SinkHelper.m_OnReceiveGLogTextOnDoorOpenDelegate != null && ((dRealSvrOcxTcpEvents_SinkHelper.m_OnReceiveGLogTextOnDoorOpenDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dRealSvrOcxTcpEvents_SinkHelper.m_dwCookie);
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

	public _DRealSvrOcxTcpEvents_EventProvider(object P_0)
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
					_DRealSvrOcxTcpEvents_SinkHelper dRealSvrOcxTcpEvents_SinkHelper = (_DRealSvrOcxTcpEvents_SinkHelper)m_aEventSinkHelpers[num];
					m_ConnectionPoint.Unadvise(dRealSvrOcxTcpEvents_SinkHelper.m_dwCookie);
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
