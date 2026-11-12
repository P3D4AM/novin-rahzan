using System.Collections;

namespace ADODB;

internal class InternalEnumerator : IEnumerator
{
	private InternalFields m_fields = null;

	private IEnumerator m_list = null;

	public object Current
	{
		get
		{
			InternalField internalField = new InternalField((Field)m_list.Current);
			internalField.m_fields = m_fields;
			return internalField;
		}
	}

	public InternalEnumerator(IEnumerator list, InternalFields Fields)
	{
		m_list = list;
		m_fields = Fields;
	}

	public bool MoveNext()
	{
		return m_list.MoveNext();
	}

	public void Reset()
	{
		m_list.Reset();
	}
}
