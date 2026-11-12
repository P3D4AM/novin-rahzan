using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace ADODB;

[ClassInterface(ClassInterfaceType.None)]
public class InternalProperties : Properties, _Collection, IEnumerable
{
	private Properties m_props = null;

	public int Count => m_props.Count;

	public Property this[object index]
	{
		get
		{
			InternalProperty internalProperty = new InternalProperty(m_props[index]);
			internalProperty.m_props = this;
			return internalProperty;
		}
	}

	internal InternalProperties(Properties fields)
	{
		m_props = fields;
	}

	internal IntPtr GetProperties()
	{
		return Marshal.GetIUnknownForObject(m_props);
	}

	public IEnumerator GetEnumerator()
	{
		return new PropertyEnumerator(m_props.GetEnumerator(), this);
	}

	public void Refresh()
	{
		m_props.Refresh();
	}
}
