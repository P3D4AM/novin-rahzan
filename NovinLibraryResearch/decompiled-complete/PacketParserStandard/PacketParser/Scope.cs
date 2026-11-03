using System;

namespace PacketParser;

public sealed class Scope : IDisposable
{
	private Action m_fnDispose;

	public static readonly Scope Empty = new Scope(null);

	public static Scope Create(Action fnDispose)
	{
		return new Scope(fnDispose);
	}

	public void Dispose()
	{
		if (m_fnDispose != null)
		{
			m_fnDispose();
			m_fnDispose = null;
		}
	}

	private Scope(Action fnDispose)
	{
		m_fnDispose = fnDispose;
	}

	public void Cancel()
	{
		m_fnDispose = null;
	}

	public Scope Transfer()
	{
		Scope result = new Scope(m_fnDispose);
		m_fnDispose = null;
		return result;
	}
}
