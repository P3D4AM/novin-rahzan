using System.Collections;

namespace ADODB;

internal class ErrorEnumerator : IEnumerator
{
	private InternalErrors m_Errors = null;

	private IEnumerator m_list = null;

	public object Current
	{
		get
		{
			InternalError internalError = new InternalError((Error)m_list.Current);
			internalError.m_Errors = m_Errors;
			return internalError;
		}
	}

	public ErrorEnumerator(IEnumerator list, InternalErrors Errors)
	{
		m_list = list;
		m_Errors = Errors;
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
