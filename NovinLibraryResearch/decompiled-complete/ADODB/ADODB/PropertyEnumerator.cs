using System.Collections;

namespace ADODB;

internal class PropertyEnumerator : IEnumerator
{
	private InternalProperties m_props = null;

	private IEnumerator m_list = null;

	public object Current
	{
		get
		{
			InternalProperty internalProperty = new InternalProperty((Property)m_list.Current);
			internalProperty.m_props = m_props;
			return internalProperty;
		}
	}

	public PropertyEnumerator(IEnumerator list, InternalProperties props)
	{
		m_list = list;
		m_props = props;
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
