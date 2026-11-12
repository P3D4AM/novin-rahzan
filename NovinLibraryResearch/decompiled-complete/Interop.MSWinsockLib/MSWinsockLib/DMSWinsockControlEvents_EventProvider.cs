using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

namespace MSWinsockLib;

internal sealed class DMSWinsockControlEvents_EventProvider : DMSWinsockControlEvents_Event, IDisposable
{
	private IConnectionPointContainer m_ConnectionPointContainer;

	private ArrayList m_aEventSinkHelpers;

	private IConnectionPoint m_ConnectionPoint;

	private void Init()
	{
		IConnectionPoint ppCP = null;
		Guid riid = new Guid(new byte[16]
		{
			147, 216, 141, 36, 69, 187, 207, 17, 154, 188,
			0, 128, 199, 231, 183, 141
		});
		m_ConnectionPointContainer.FindConnectionPoint(ref riid, out ppCP);
		m_ConnectionPoint = ppCP;
		m_aEventSinkHelpers = new ArrayList();
	}

	public void add_Error(DMSWinsockControlEvents_ErrorEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			DMSWinsockControlEvents_SinkHelper dMSWinsockControlEvents_SinkHelper = new DMSWinsockControlEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(dMSWinsockControlEvents_SinkHelper, out pdwCookie);
			dMSWinsockControlEvents_SinkHelper.m_dwCookie = pdwCookie;
			dMSWinsockControlEvents_SinkHelper.m_ErrorDelegate = P_0;
			m_aEventSinkHelpers.Add(dMSWinsockControlEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_Error(DMSWinsockControlEvents_ErrorEventHandler P_0)
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
				DMSWinsockControlEvents_SinkHelper dMSWinsockControlEvents_SinkHelper = (DMSWinsockControlEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dMSWinsockControlEvents_SinkHelper.m_ErrorDelegate != null && ((dMSWinsockControlEvents_SinkHelper.m_ErrorDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dMSWinsockControlEvents_SinkHelper.m_dwCookie);
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

	public void add_DataArrival(DMSWinsockControlEvents_DataArrivalEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			DMSWinsockControlEvents_SinkHelper dMSWinsockControlEvents_SinkHelper = new DMSWinsockControlEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(dMSWinsockControlEvents_SinkHelper, out pdwCookie);
			dMSWinsockControlEvents_SinkHelper.m_dwCookie = pdwCookie;
			dMSWinsockControlEvents_SinkHelper.m_DataArrivalDelegate = P_0;
			m_aEventSinkHelpers.Add(dMSWinsockControlEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_DataArrival(DMSWinsockControlEvents_DataArrivalEventHandler P_0)
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
				DMSWinsockControlEvents_SinkHelper dMSWinsockControlEvents_SinkHelper = (DMSWinsockControlEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dMSWinsockControlEvents_SinkHelper.m_DataArrivalDelegate != null && ((dMSWinsockControlEvents_SinkHelper.m_DataArrivalDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dMSWinsockControlEvents_SinkHelper.m_dwCookie);
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

	public void add_Connect(DMSWinsockControlEvents_ConnectEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			DMSWinsockControlEvents_SinkHelper dMSWinsockControlEvents_SinkHelper = new DMSWinsockControlEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(dMSWinsockControlEvents_SinkHelper, out pdwCookie);
			dMSWinsockControlEvents_SinkHelper.m_dwCookie = pdwCookie;
			dMSWinsockControlEvents_SinkHelper.m_ConnectDelegate = P_0;
			m_aEventSinkHelpers.Add(dMSWinsockControlEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_Connect(DMSWinsockControlEvents_ConnectEventHandler P_0)
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
				DMSWinsockControlEvents_SinkHelper dMSWinsockControlEvents_SinkHelper = (DMSWinsockControlEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dMSWinsockControlEvents_SinkHelper.m_ConnectDelegate != null && ((dMSWinsockControlEvents_SinkHelper.m_ConnectDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dMSWinsockControlEvents_SinkHelper.m_dwCookie);
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

	public void add_ConnectionRequest(DMSWinsockControlEvents_ConnectionRequestEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			DMSWinsockControlEvents_SinkHelper dMSWinsockControlEvents_SinkHelper = new DMSWinsockControlEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(dMSWinsockControlEvents_SinkHelper, out pdwCookie);
			dMSWinsockControlEvents_SinkHelper.m_dwCookie = pdwCookie;
			dMSWinsockControlEvents_SinkHelper.m_ConnectionRequestDelegate = P_0;
			m_aEventSinkHelpers.Add(dMSWinsockControlEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_ConnectionRequest(DMSWinsockControlEvents_ConnectionRequestEventHandler P_0)
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
				DMSWinsockControlEvents_SinkHelper dMSWinsockControlEvents_SinkHelper = (DMSWinsockControlEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dMSWinsockControlEvents_SinkHelper.m_ConnectionRequestDelegate != null && ((dMSWinsockControlEvents_SinkHelper.m_ConnectionRequestDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dMSWinsockControlEvents_SinkHelper.m_dwCookie);
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

	public void add_Close(DMSWinsockControlEvents_CloseEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			DMSWinsockControlEvents_SinkHelper dMSWinsockControlEvents_SinkHelper = new DMSWinsockControlEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(dMSWinsockControlEvents_SinkHelper, out pdwCookie);
			dMSWinsockControlEvents_SinkHelper.m_dwCookie = pdwCookie;
			dMSWinsockControlEvents_SinkHelper.m_CloseDelegate = P_0;
			m_aEventSinkHelpers.Add(dMSWinsockControlEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_Close(DMSWinsockControlEvents_CloseEventHandler P_0)
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
				DMSWinsockControlEvents_SinkHelper dMSWinsockControlEvents_SinkHelper = (DMSWinsockControlEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dMSWinsockControlEvents_SinkHelper.m_CloseDelegate != null && ((dMSWinsockControlEvents_SinkHelper.m_CloseDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dMSWinsockControlEvents_SinkHelper.m_dwCookie);
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

	public void add_SendProgress(DMSWinsockControlEvents_SendProgressEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			DMSWinsockControlEvents_SinkHelper dMSWinsockControlEvents_SinkHelper = new DMSWinsockControlEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(dMSWinsockControlEvents_SinkHelper, out pdwCookie);
			dMSWinsockControlEvents_SinkHelper.m_dwCookie = pdwCookie;
			dMSWinsockControlEvents_SinkHelper.m_SendProgressDelegate = P_0;
			m_aEventSinkHelpers.Add(dMSWinsockControlEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_SendProgress(DMSWinsockControlEvents_SendProgressEventHandler P_0)
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
				DMSWinsockControlEvents_SinkHelper dMSWinsockControlEvents_SinkHelper = (DMSWinsockControlEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dMSWinsockControlEvents_SinkHelper.m_SendProgressDelegate != null && ((dMSWinsockControlEvents_SinkHelper.m_SendProgressDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dMSWinsockControlEvents_SinkHelper.m_dwCookie);
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

	public void add_SendComplete(DMSWinsockControlEvents_SendCompleteEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			DMSWinsockControlEvents_SinkHelper dMSWinsockControlEvents_SinkHelper = new DMSWinsockControlEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(dMSWinsockControlEvents_SinkHelper, out pdwCookie);
			dMSWinsockControlEvents_SinkHelper.m_dwCookie = pdwCookie;
			dMSWinsockControlEvents_SinkHelper.m_SendCompleteDelegate = P_0;
			m_aEventSinkHelpers.Add(dMSWinsockControlEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_SendComplete(DMSWinsockControlEvents_SendCompleteEventHandler P_0)
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
				DMSWinsockControlEvents_SinkHelper dMSWinsockControlEvents_SinkHelper = (DMSWinsockControlEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dMSWinsockControlEvents_SinkHelper.m_SendCompleteDelegate != null && ((dMSWinsockControlEvents_SinkHelper.m_SendCompleteDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dMSWinsockControlEvents_SinkHelper.m_dwCookie);
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

	public DMSWinsockControlEvents_EventProvider(object P_0)
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
					DMSWinsockControlEvents_SinkHelper dMSWinsockControlEvents_SinkHelper = (DMSWinsockControlEvents_SinkHelper)m_aEventSinkHelpers[num];
					m_ConnectionPoint.Unadvise(dMSWinsockControlEvents_SinkHelper.m_dwCookie);
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
