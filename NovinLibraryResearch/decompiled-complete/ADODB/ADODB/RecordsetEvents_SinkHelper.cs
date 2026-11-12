using System.Runtime.InteropServices;

namespace ADODB;

[ClassInterface(ClassInterfaceType.None)]
public sealed class RecordsetEvents_SinkHelper : RecordsetEvents
{
	public RecordsetEvents_FetchCompleteEventHandler m_FetchCompleteDelegate;

	public RecordsetEvents_FetchProgressEventHandler m_FetchProgressDelegate;

	public RecordsetEvents_EndOfRecordsetEventHandler m_EndOfRecordsetDelegate;

	public RecordsetEvents_MoveCompleteEventHandler m_MoveCompleteDelegate;

	public RecordsetEvents_WillMoveEventHandler m_WillMoveDelegate;

	public RecordsetEvents_RecordsetChangeCompleteEventHandler m_RecordsetChangeCompleteDelegate;

	public RecordsetEvents_WillChangeRecordsetEventHandler m_WillChangeRecordsetDelegate;

	public RecordsetEvents_RecordChangeCompleteEventHandler m_RecordChangeCompleteDelegate;

	public RecordsetEvents_WillChangeRecordEventHandler m_WillChangeRecordDelegate;

	public RecordsetEvents_FieldChangeCompleteEventHandler m_FieldChangeCompleteDelegate;

	public RecordsetEvents_WillChangeFieldEventHandler m_WillChangeFieldDelegate;

	public int m_dwCookie;

	public void FetchComplete(Error A_1, ref EventStatusEnum A_2, Recordset A_3)
	{
		if (m_FetchCompleteDelegate != null)
		{
			m_FetchCompleteDelegate(A_1, ref A_2, A_3);
		}
	}

	public void FetchProgress(int A_1, int A_2, ref EventStatusEnum A_3, Recordset A_4)
	{
		if (m_FetchProgressDelegate != null)
		{
			m_FetchProgressDelegate(A_1, A_2, ref A_3, A_4);
		}
	}

	public void EndOfRecordset(ref bool A_1, ref EventStatusEnum A_2, Recordset A_3)
	{
		if (m_EndOfRecordsetDelegate != null)
		{
			m_EndOfRecordsetDelegate(ref A_1, ref A_2, A_3);
		}
	}

	public void MoveComplete(EventReasonEnum A_1, Error A_2, ref EventStatusEnum A_3, Recordset A_4)
	{
		if (m_MoveCompleteDelegate != null)
		{
			m_MoveCompleteDelegate(A_1, A_2, ref A_3, A_4);
		}
	}

	public void WillMove(EventReasonEnum A_1, ref EventStatusEnum A_2, Recordset A_3)
	{
		if (m_WillMoveDelegate != null)
		{
			m_WillMoveDelegate(A_1, ref A_2, A_3);
		}
	}

	public void RecordsetChangeComplete(EventReasonEnum A_1, Error A_2, ref EventStatusEnum A_3, Recordset A_4)
	{
		if (m_RecordsetChangeCompleteDelegate != null)
		{
			m_RecordsetChangeCompleteDelegate(A_1, A_2, ref A_3, A_4);
		}
	}

	public void WillChangeRecordset(EventReasonEnum A_1, ref EventStatusEnum A_2, Recordset A_3)
	{
		if (m_WillChangeRecordsetDelegate != null)
		{
			m_WillChangeRecordsetDelegate(A_1, ref A_2, A_3);
		}
	}

	public void RecordChangeComplete(EventReasonEnum A_1, int A_2, Error A_3, ref EventStatusEnum A_4, Recordset A_5)
	{
		if (m_RecordChangeCompleteDelegate != null)
		{
			m_RecordChangeCompleteDelegate(A_1, A_2, A_3, ref A_4, A_5);
		}
	}

	public void WillChangeRecord(EventReasonEnum A_1, int A_2, ref EventStatusEnum A_3, Recordset A_4)
	{
		if (m_WillChangeRecordDelegate != null)
		{
			m_WillChangeRecordDelegate(A_1, A_2, ref A_3, A_4);
		}
	}

	public void FieldChangeComplete(int A_1, object A_2, Error A_3, ref EventStatusEnum A_4, Recordset A_5)
	{
		if (m_FieldChangeCompleteDelegate != null)
		{
			m_FieldChangeCompleteDelegate(A_1, A_2, A_3, ref A_4, A_5);
		}
	}

	public void WillChangeField(int A_1, object A_2, ref EventStatusEnum A_3, Recordset A_4)
	{
		if (m_WillChangeFieldDelegate != null)
		{
			m_WillChangeFieldDelegate(A_1, A_2, ref A_3, A_4);
		}
	}

	internal RecordsetEvents_SinkHelper()
	{
		m_dwCookie = 0;
		m_FetchCompleteDelegate = null;
		m_FetchProgressDelegate = null;
		m_EndOfRecordsetDelegate = null;
		m_MoveCompleteDelegate = null;
		m_WillMoveDelegate = null;
		m_RecordsetChangeCompleteDelegate = null;
		m_WillChangeRecordsetDelegate = null;
		m_RecordChangeCompleteDelegate = null;
		m_WillChangeRecordDelegate = null;
		m_FieldChangeCompleteDelegate = null;
		m_WillChangeFieldDelegate = null;
	}
}
