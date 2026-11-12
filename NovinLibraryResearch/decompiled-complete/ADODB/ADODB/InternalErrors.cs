using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace ADODB;

[ClassInterface(ClassInterfaceType.None)]
public class InternalErrors : Errors, _Collection, IEnumerable
{
	private Errors m_Errors = null;

	public int Count => m_Errors.Count;

	public Error this[object index]
	{
		get
		{
			InternalError internalError = new InternalError(m_Errors[index]);
			internalError.m_Errors = this;
			return internalError;
		}
	}

	internal InternalErrors(Errors errors)
	{
		m_Errors = errors;
	}

	internal IntPtr GetErrors()
	{
		return Marshal.GetIUnknownForObject(m_Errors);
	}

	public IEnumerator GetEnumerator()
	{
		return new ErrorEnumerator(m_Errors.GetEnumerator(), this);
	}

	public void Clear()
	{
		m_Errors.Clear();
	}

	public void Refresh()
	{
		m_Errors.Refresh();
	}
}
