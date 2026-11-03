using System;

namespace PacketParser;

public sealed class EventInfo<TSource, TEventHandler>
{
	private readonly Action<TSource, TEventHandler> m_fnAddHandler;

	private readonly Action<TSource, TEventHandler> m_fnRemoveHandler;

	public EventInfo(Action<TSource, TEventHandler> fnAddHandler, Action<TSource, TEventHandler> fnRemoveHandler)
	{
		m_fnAddHandler = fnAddHandler;
		m_fnRemoveHandler = fnRemoveHandler;
	}

	public void AddHandler(TSource source, TEventHandler handler)
	{
		m_fnAddHandler(source, handler);
	}

	public void RemoveHandler(TSource source, TEventHandler handler)
	{
		m_fnRemoveHandler(source, handler);
	}

	public Scope Subscribe(TSource source, TEventHandler handler)
	{
		AddHandler(source, handler);
		return Scope.Create(delegate
		{
			RemoveHandler(source, handler);
		});
	}
}
