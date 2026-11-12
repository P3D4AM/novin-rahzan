using System;
using System.Collections.Generic;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class LocalViewListener : ILocalViewListener
{
	private readonly IList<Action<InternalEntityEntry, EntityState>> _viewActions = new List<Action<InternalEntityEntry, EntityState>>();

	public virtual void RegisterView(Action<InternalEntityEntry, EntityState> viewAction)
	{
		_viewActions.Add(viewAction);
	}

	public virtual void StateChanged(InternalEntityEntry entry, EntityState oldState, bool fromQuery)
	{
		for (int i = 0; i < _viewActions.Count; i++)
		{
			_viewActions[i](entry, oldState);
		}
	}

	public virtual void StateChanging(InternalEntityEntry entry, EntityState newState)
	{
	}
}
