using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

namespace TCPOCX;

internal sealed class __TCPOCX3_EventProvider : __TCPOCX3_Event, IDisposable
{
	private IConnectionPointContainer m_ConnectionPointContainer;

	private ArrayList m_aEventSinkHelpers;

	private IConnectionPoint m_ConnectionPoint;

	private void Init()
	{
		IConnectionPoint ppCP = null;
		Guid riid = new Guid(new byte[16]
		{
			243, 243, 100, 181, 174, 112, 243, 64, 138, 166,
			27, 39, 97, 126, 74, 94
		});
		m_ConnectionPointContainer.FindConnectionPoint(ref riid, out ppCP);
		m_ConnectionPoint = ppCP;
		m_aEventSinkHelpers = new ArrayList();
	}

	public void add_ReceiveSalePacket(__TCPOCX3_ReceiveSalePacketEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			__TCPOCX3_SinkHelper _TCPOCX3_SinkHelper = new __TCPOCX3_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(_TCPOCX3_SinkHelper, out pdwCookie);
			_TCPOCX3_SinkHelper.m_dwCookie = pdwCookie;
			_TCPOCX3_SinkHelper.m_ReceiveSalePacketDelegate = P_0;
			m_aEventSinkHelpers.Add(_TCPOCX3_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_ReceiveSalePacket(__TCPOCX3_ReceiveSalePacketEventHandler P_0)
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
				__TCPOCX3_SinkHelper _TCPOCX3_SinkHelper = (__TCPOCX3_SinkHelper)m_aEventSinkHelpers[num];
				if (_TCPOCX3_SinkHelper.m_ReceiveSalePacketDelegate != null && ((_TCPOCX3_SinkHelper.m_ReceiveSalePacketDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(_TCPOCX3_SinkHelper.m_dwCookie);
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

	public __TCPOCX3_EventProvider(object P_0)
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
					__TCPOCX3_SinkHelper _TCPOCX3_SinkHelper = (__TCPOCX3_SinkHelper)m_aEventSinkHelpers[num];
					m_ConnectionPoint.Unadvise(_TCPOCX3_SinkHelper.m_dwCookie);
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
