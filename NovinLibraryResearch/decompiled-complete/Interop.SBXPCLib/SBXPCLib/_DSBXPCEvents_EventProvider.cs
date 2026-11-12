using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

namespace SBXPCLib;

internal sealed class _DSBXPCEvents_EventProvider : _DSBXPCEvents_Event, IDisposable
{
	private IConnectionPointContainer m_ConnectionPointContainer;

	private ArrayList m_aEventSinkHelpers;

	private IConnectionPoint m_ConnectionPoint;

	private void Init()
	{
		IConnectionPoint ppCP = null;
		Guid riid = new Guid(new byte[16]
		{
			33, 161, 243, 159, 61, 14, 53, 76, 167, 118,
			201, 199, 102, 251, 20, 190
		});
		m_ConnectionPointContainer.FindConnectionPoint(ref riid, out ppCP);
		m_ConnectionPoint = ppCP;
		m_aEventSinkHelpers = new ArrayList();
	}

	public void add_OnReceiveEventXML(_DSBXPCEvents_OnReceiveEventXMLEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_DSBXPCEvents_SinkHelper dSBXPCEvents_SinkHelper = new _DSBXPCEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(dSBXPCEvents_SinkHelper, out pdwCookie);
			dSBXPCEvents_SinkHelper.m_dwCookie = pdwCookie;
			dSBXPCEvents_SinkHelper.m_OnReceiveEventXMLDelegate = P_0;
			m_aEventSinkHelpers.Add(dSBXPCEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_OnReceiveEventXML(_DSBXPCEvents_OnReceiveEventXMLEventHandler P_0)
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
				_DSBXPCEvents_SinkHelper dSBXPCEvents_SinkHelper = (_DSBXPCEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dSBXPCEvents_SinkHelper.m_OnReceiveEventXMLDelegate != null && ((dSBXPCEvents_SinkHelper.m_OnReceiveEventXMLDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dSBXPCEvents_SinkHelper.m_dwCookie);
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

	public _DSBXPCEvents_EventProvider(object P_0)
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
					_DSBXPCEvents_SinkHelper dSBXPCEvents_SinkHelper = (_DSBXPCEvents_SinkHelper)m_aEventSinkHelpers[num];
					m_ConnectionPoint.Unadvise(dSBXPCEvents_SinkHelper.m_dwCookie);
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
