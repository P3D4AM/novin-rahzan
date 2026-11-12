using System.Collections;

namespace ADODB;

internal class ParameterEnumerator : IEnumerator
{
	private InternalParameters m_parms = null;

	private IEnumerator m_list = null;

	public object Current
	{
		get
		{
			InternalParameter internalParameter = new InternalParameter((Parameter)m_list.Current);
			internalParameter.m_parms = m_parms;
			return internalParameter;
		}
	}

	public ParameterEnumerator(IEnumerator list, InternalParameters parms)
	{
		m_list = list;
		m_parms = parms;
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
