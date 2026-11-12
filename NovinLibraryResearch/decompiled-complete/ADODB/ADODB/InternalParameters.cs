using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace ADODB;

[ClassInterface(ClassInterfaceType.None)]
public class InternalParameters : Parameters, _DynaCollection, _Collection, IEnumerable
{
	private Parameters m_parms = null;

	public int Count => m_parms.Count;

	public Parameter this[object index]
	{
		get
		{
			InternalParameter internalParameter = new InternalParameter(m_parms[index]);
			internalParameter.m_parms = this;
			return internalParameter;
		}
	}

	internal InternalParameters(Parameters parms)
	{
		m_parms = parms;
	}

	internal IntPtr GetParms()
	{
		return Marshal.GetIUnknownForObject(m_parms);
	}

	public IEnumerator GetEnumerator()
	{
		return new ParameterEnumerator(m_parms.GetEnumerator(), this);
	}

	public void Refresh()
	{
		m_parms.Refresh();
	}

	public void Delete(object Index)
	{
		m_parms.Delete(Index);
	}

	public void Append(object oParam)
	{
		m_parms.Append(((InternalParameter)oParam).GetParm());
		((InternalParameter)oParam).m_parms = this;
	}
}
