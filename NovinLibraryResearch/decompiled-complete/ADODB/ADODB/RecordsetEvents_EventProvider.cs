using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;

namespace ADODB;

internal sealed class RecordsetEvents_EventProvider : RecordsetEvents_Event, IDisposable
{
	private UCOMIConnectionPointContainer m_ConnectionPointContainer;

	private ArrayList m_aEventSinkHelpers;

	private UCOMIConnectionPoint m_ConnectionPoint;

	private void Init()
	{
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Expected O, but got Unknown
		UCOMIConnectionPoint val = null;
		Guid guid = new Guid(new byte[16]
		{
			102, 2, 0, 0, 0, 0, 16, 0, 128, 0,
			0, 170, 0, 109, 46, 164
		});
		m_ConnectionPointContainer.FindConnectionPoint(ref guid, ref val);
		m_ConnectionPoint = val;
		m_aEventSinkHelpers = new ArrayList();
	}

	public void add_FetchComplete(RecordsetEvents_FetchCompleteEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			RecordsetEvents_SinkHelper recordsetEvents_SinkHelper = new RecordsetEvents_SinkHelper();
			int dwCookie = 0;
			m_ConnectionPoint.Advise((object)recordsetEvents_SinkHelper, ref dwCookie);
			recordsetEvents_SinkHelper.m_dwCookie = dwCookie;
			recordsetEvents_SinkHelper.m_FetchCompleteDelegate = A_1;
			m_aEventSinkHelpers.Add(recordsetEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_FetchComplete(RecordsetEvents_FetchCompleteEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				RecordsetEvents_SinkHelper recordsetEvents_SinkHelper = (RecordsetEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (recordsetEvents_SinkHelper.m_FetchCompleteDelegate != null && ((recordsetEvents_SinkHelper.m_FetchCompleteDelegate.Equals(A_1) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(recordsetEvents_SinkHelper.m_dwCookie);
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

	public void add_FetchProgress(RecordsetEvents_FetchProgressEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			RecordsetEvents_SinkHelper recordsetEvents_SinkHelper = new RecordsetEvents_SinkHelper();
			int dwCookie = 0;
			m_ConnectionPoint.Advise((object)recordsetEvents_SinkHelper, ref dwCookie);
			recordsetEvents_SinkHelper.m_dwCookie = dwCookie;
			recordsetEvents_SinkHelper.m_FetchProgressDelegate = A_1;
			m_aEventSinkHelpers.Add(recordsetEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_FetchProgress(RecordsetEvents_FetchProgressEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				RecordsetEvents_SinkHelper recordsetEvents_SinkHelper = (RecordsetEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (recordsetEvents_SinkHelper.m_FetchProgressDelegate != null && ((recordsetEvents_SinkHelper.m_FetchProgressDelegate.Equals(A_1) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(recordsetEvents_SinkHelper.m_dwCookie);
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

	public void add_EndOfRecordset(RecordsetEvents_EndOfRecordsetEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			RecordsetEvents_SinkHelper recordsetEvents_SinkHelper = new RecordsetEvents_SinkHelper();
			int dwCookie = 0;
			m_ConnectionPoint.Advise((object)recordsetEvents_SinkHelper, ref dwCookie);
			recordsetEvents_SinkHelper.m_dwCookie = dwCookie;
			recordsetEvents_SinkHelper.m_EndOfRecordsetDelegate = A_1;
			m_aEventSinkHelpers.Add(recordsetEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_EndOfRecordset(RecordsetEvents_EndOfRecordsetEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				RecordsetEvents_SinkHelper recordsetEvents_SinkHelper = (RecordsetEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (recordsetEvents_SinkHelper.m_EndOfRecordsetDelegate != null && ((recordsetEvents_SinkHelper.m_EndOfRecordsetDelegate.Equals(A_1) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(recordsetEvents_SinkHelper.m_dwCookie);
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

	public void add_MoveComplete(RecordsetEvents_MoveCompleteEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			RecordsetEvents_SinkHelper recordsetEvents_SinkHelper = new RecordsetEvents_SinkHelper();
			int dwCookie = 0;
			m_ConnectionPoint.Advise((object)recordsetEvents_SinkHelper, ref dwCookie);
			recordsetEvents_SinkHelper.m_dwCookie = dwCookie;
			recordsetEvents_SinkHelper.m_MoveCompleteDelegate = A_1;
			m_aEventSinkHelpers.Add(recordsetEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_MoveComplete(RecordsetEvents_MoveCompleteEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				RecordsetEvents_SinkHelper recordsetEvents_SinkHelper = (RecordsetEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (recordsetEvents_SinkHelper.m_MoveCompleteDelegate != null && ((recordsetEvents_SinkHelper.m_MoveCompleteDelegate.Equals(A_1) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(recordsetEvents_SinkHelper.m_dwCookie);
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

	public void add_WillMove(RecordsetEvents_WillMoveEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			RecordsetEvents_SinkHelper recordsetEvents_SinkHelper = new RecordsetEvents_SinkHelper();
			int dwCookie = 0;
			m_ConnectionPoint.Advise((object)recordsetEvents_SinkHelper, ref dwCookie);
			recordsetEvents_SinkHelper.m_dwCookie = dwCookie;
			recordsetEvents_SinkHelper.m_WillMoveDelegate = A_1;
			m_aEventSinkHelpers.Add(recordsetEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_WillMove(RecordsetEvents_WillMoveEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				RecordsetEvents_SinkHelper recordsetEvents_SinkHelper = (RecordsetEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (recordsetEvents_SinkHelper.m_WillMoveDelegate != null && ((recordsetEvents_SinkHelper.m_WillMoveDelegate.Equals(A_1) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(recordsetEvents_SinkHelper.m_dwCookie);
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

	public void add_RecordsetChangeComplete(RecordsetEvents_RecordsetChangeCompleteEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			RecordsetEvents_SinkHelper recordsetEvents_SinkHelper = new RecordsetEvents_SinkHelper();
			int dwCookie = 0;
			m_ConnectionPoint.Advise((object)recordsetEvents_SinkHelper, ref dwCookie);
			recordsetEvents_SinkHelper.m_dwCookie = dwCookie;
			recordsetEvents_SinkHelper.m_RecordsetChangeCompleteDelegate = A_1;
			m_aEventSinkHelpers.Add(recordsetEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_RecordsetChangeComplete(RecordsetEvents_RecordsetChangeCompleteEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				RecordsetEvents_SinkHelper recordsetEvents_SinkHelper = (RecordsetEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (recordsetEvents_SinkHelper.m_RecordsetChangeCompleteDelegate != null && ((recordsetEvents_SinkHelper.m_RecordsetChangeCompleteDelegate.Equals(A_1) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(recordsetEvents_SinkHelper.m_dwCookie);
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

	public void add_WillChangeRecordset(RecordsetEvents_WillChangeRecordsetEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			RecordsetEvents_SinkHelper recordsetEvents_SinkHelper = new RecordsetEvents_SinkHelper();
			int dwCookie = 0;
			m_ConnectionPoint.Advise((object)recordsetEvents_SinkHelper, ref dwCookie);
			recordsetEvents_SinkHelper.m_dwCookie = dwCookie;
			recordsetEvents_SinkHelper.m_WillChangeRecordsetDelegate = A_1;
			m_aEventSinkHelpers.Add(recordsetEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_WillChangeRecordset(RecordsetEvents_WillChangeRecordsetEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				RecordsetEvents_SinkHelper recordsetEvents_SinkHelper = (RecordsetEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (recordsetEvents_SinkHelper.m_WillChangeRecordsetDelegate != null && ((recordsetEvents_SinkHelper.m_WillChangeRecordsetDelegate.Equals(A_1) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(recordsetEvents_SinkHelper.m_dwCookie);
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

	public void add_RecordChangeComplete(RecordsetEvents_RecordChangeCompleteEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			RecordsetEvents_SinkHelper recordsetEvents_SinkHelper = new RecordsetEvents_SinkHelper();
			int dwCookie = 0;
			m_ConnectionPoint.Advise((object)recordsetEvents_SinkHelper, ref dwCookie);
			recordsetEvents_SinkHelper.m_dwCookie = dwCookie;
			recordsetEvents_SinkHelper.m_RecordChangeCompleteDelegate = A_1;
			m_aEventSinkHelpers.Add(recordsetEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_RecordChangeComplete(RecordsetEvents_RecordChangeCompleteEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				RecordsetEvents_SinkHelper recordsetEvents_SinkHelper = (RecordsetEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (recordsetEvents_SinkHelper.m_RecordChangeCompleteDelegate != null && ((recordsetEvents_SinkHelper.m_RecordChangeCompleteDelegate.Equals(A_1) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(recordsetEvents_SinkHelper.m_dwCookie);
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

	public void add_WillChangeRecord(RecordsetEvents_WillChangeRecordEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			RecordsetEvents_SinkHelper recordsetEvents_SinkHelper = new RecordsetEvents_SinkHelper();
			int dwCookie = 0;
			m_ConnectionPoint.Advise((object)recordsetEvents_SinkHelper, ref dwCookie);
			recordsetEvents_SinkHelper.m_dwCookie = dwCookie;
			recordsetEvents_SinkHelper.m_WillChangeRecordDelegate = A_1;
			m_aEventSinkHelpers.Add(recordsetEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_WillChangeRecord(RecordsetEvents_WillChangeRecordEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				RecordsetEvents_SinkHelper recordsetEvents_SinkHelper = (RecordsetEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (recordsetEvents_SinkHelper.m_WillChangeRecordDelegate != null && ((recordsetEvents_SinkHelper.m_WillChangeRecordDelegate.Equals(A_1) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(recordsetEvents_SinkHelper.m_dwCookie);
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

	public void add_FieldChangeComplete(RecordsetEvents_FieldChangeCompleteEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			RecordsetEvents_SinkHelper recordsetEvents_SinkHelper = new RecordsetEvents_SinkHelper();
			int dwCookie = 0;
			m_ConnectionPoint.Advise((object)recordsetEvents_SinkHelper, ref dwCookie);
			recordsetEvents_SinkHelper.m_dwCookie = dwCookie;
			recordsetEvents_SinkHelper.m_FieldChangeCompleteDelegate = A_1;
			m_aEventSinkHelpers.Add(recordsetEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_FieldChangeComplete(RecordsetEvents_FieldChangeCompleteEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				RecordsetEvents_SinkHelper recordsetEvents_SinkHelper = (RecordsetEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (recordsetEvents_SinkHelper.m_FieldChangeCompleteDelegate != null && ((recordsetEvents_SinkHelper.m_FieldChangeCompleteDelegate.Equals(A_1) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(recordsetEvents_SinkHelper.m_dwCookie);
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

	public void add_WillChangeField(RecordsetEvents_WillChangeFieldEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			RecordsetEvents_SinkHelper recordsetEvents_SinkHelper = new RecordsetEvents_SinkHelper();
			int dwCookie = 0;
			m_ConnectionPoint.Advise((object)recordsetEvents_SinkHelper, ref dwCookie);
			recordsetEvents_SinkHelper.m_dwCookie = dwCookie;
			recordsetEvents_SinkHelper.m_WillChangeFieldDelegate = A_1;
			m_aEventSinkHelpers.Add(recordsetEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_WillChangeField(RecordsetEvents_WillChangeFieldEventHandler A_1)
	{
		Monitor.Enter(this);
		try
		{
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				RecordsetEvents_SinkHelper recordsetEvents_SinkHelper = (RecordsetEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (recordsetEvents_SinkHelper.m_WillChangeFieldDelegate != null && ((recordsetEvents_SinkHelper.m_WillChangeFieldDelegate.Equals(A_1) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(recordsetEvents_SinkHelper.m_dwCookie);
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

	public RecordsetEvents_EventProvider(object A_1)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		m_ConnectionPointContainer = (UCOMIConnectionPointContainer)A_1;
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
					RecordsetEvents_SinkHelper recordsetEvents_SinkHelper = (RecordsetEvents_SinkHelper)m_aEventSinkHelpers[num];
					m_ConnectionPoint.Unadvise(recordsetEvents_SinkHelper.m_dwCookie);
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
		Finalize();
		GC.SuppressFinalize(this);
	}
}
