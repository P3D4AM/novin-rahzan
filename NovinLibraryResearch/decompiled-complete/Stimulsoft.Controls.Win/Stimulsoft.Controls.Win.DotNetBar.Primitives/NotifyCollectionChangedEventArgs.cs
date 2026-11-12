using System;
using System.Collections;

namespace Stimulsoft.Controls.Win.DotNetBar.Primitives;

public class NotifyCollectionChangedEventArgs : EventArgs
{
	private NotifyCollectionChangedAction _Action;

	private IList _NewItems;

	private int _NewStartingIndex;

	private IList _OldItems;

	private int _OldStartingIndex;

	public NotifyCollectionChangedAction Action => _Action;

	public IList NewItems => _NewItems;

	public int NewStartingIndex => _NewStartingIndex;

	public IList OldItems => _OldItems;

	public int OldStartingIndex => _OldStartingIndex;

	public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action)
	{
		_NewStartingIndex = -1;
		_OldStartingIndex = -1;
		if (action != NotifyCollectionChangedAction.Reset)
		{
			throw new ArgumentException("WrongActionForCtor");
		}
		InitializeAdd(action, null, -1);
	}

	public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems)
	{
		_NewStartingIndex = -1;
		_OldStartingIndex = -1;
		if (action != 0 && action != NotifyCollectionChangedAction.Remove && action != NotifyCollectionChangedAction.Reset)
		{
			throw new ArgumentException("MustBeResetAddOrRemoveActionForCtor");
		}
		if (action == NotifyCollectionChangedAction.Reset)
		{
			if (changedItems != null)
			{
				throw new ArgumentException("ResetActionRequiresNullItem");
			}
			InitializeAdd(action, null, -1);
		}
		else
		{
			if (changedItems == null)
			{
				throw new ArgumentNullException("changedItems");
			}
			InitializeAddOrRemove(action, changedItems, -1);
		}
	}

	public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem)
	{
		_NewStartingIndex = -1;
		_OldStartingIndex = -1;
		if (action != 0 && action != NotifyCollectionChangedAction.Remove && action != NotifyCollectionChangedAction.Reset)
		{
			throw new ArgumentException("MustBeResetAddOrRemoveActionForCtor");
		}
		if (action == NotifyCollectionChangedAction.Reset)
		{
			if (changedItem != null)
			{
				throw new ArgumentException("ResetActionRequiresNullItem");
			}
			InitializeAdd(action, null, -1);
		}
		else
		{
			InitializeAddOrRemove(action, new object[1] { changedItem }, -1);
		}
	}

	public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems)
	{
		_NewStartingIndex = -1;
		_OldStartingIndex = -1;
		if (action != NotifyCollectionChangedAction.Replace)
		{
			throw new ArgumentException("WrongActionForCtor");
		}
		if (newItems == null)
		{
			throw new ArgumentNullException("newItems");
		}
		if (oldItems == null)
		{
			throw new ArgumentNullException("oldItems");
		}
		InitializeMoveOrReplace(action, newItems, oldItems, -1, -1);
	}

	public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems, int startingIndex)
	{
		_NewStartingIndex = -1;
		_OldStartingIndex = -1;
		if (action != 0 && action != NotifyCollectionChangedAction.Remove && action != NotifyCollectionChangedAction.Reset)
		{
			throw new ArgumentException("MustBeResetAddOrRemoveActionForCtor");
		}
		if (action == NotifyCollectionChangedAction.Reset)
		{
			if (changedItems != null)
			{
				throw new ArgumentException("ResetActionRequiresNullItem");
			}
			if (startingIndex != -1)
			{
				throw new ArgumentException("ResetActionRequiresIndexMinus1");
			}
			InitializeAdd(action, null, -1);
		}
		else
		{
			if (changedItems == null)
			{
				throw new ArgumentNullException("changedItems");
			}
			if (startingIndex < -1)
			{
				throw new ArgumentException("IndexCannotBeNegative");
			}
			InitializeAddOrRemove(action, changedItems, startingIndex);
		}
	}

	public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index)
	{
		_NewStartingIndex = -1;
		_OldStartingIndex = -1;
		if (action != 0 && action != NotifyCollectionChangedAction.Remove && action != NotifyCollectionChangedAction.Reset)
		{
			throw new ArgumentException("MustBeResetAddOrRemoveActionForCtor");
		}
		if (action == NotifyCollectionChangedAction.Reset)
		{
			if (changedItem != null)
			{
				throw new ArgumentException("ResetActionRequiresNullItem");
			}
			if (index != -1)
			{
				throw new ArgumentException("ResetActionRequiresIndexMinus1");
			}
			InitializeAdd(action, null, -1);
		}
		else
		{
			InitializeAddOrRemove(action, new object[1] { changedItem }, index);
		}
	}

	public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem)
	{
		_NewStartingIndex = -1;
		_OldStartingIndex = -1;
		if (action != NotifyCollectionChangedAction.Replace)
		{
			throw new ArgumentException("WrongActionForCtor");
		}
		InitializeMoveOrReplace(action, new object[1] { newItem }, new object[1] { oldItem }, -1, -1);
	}

	public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex)
	{
		_NewStartingIndex = -1;
		_OldStartingIndex = -1;
		if (action != NotifyCollectionChangedAction.Replace)
		{
			throw new ArgumentException("WrongActionForCtor");
		}
		if (newItems == null)
		{
			throw new ArgumentNullException("newItems");
		}
		if (oldItems == null)
		{
			throw new ArgumentNullException("oldItems");
		}
		InitializeMoveOrReplace(action, newItems, oldItems, startingIndex, startingIndex);
	}

	public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems, int index, int oldIndex)
	{
		_NewStartingIndex = -1;
		_OldStartingIndex = -1;
		if (action != NotifyCollectionChangedAction.Move)
		{
			throw new ArgumentException("WrongActionForCtor");
		}
		if (index < 0)
		{
			throw new ArgumentException("IndexCannotBeNegative");
		}
		InitializeMoveOrReplace(action, changedItems, changedItems, index, oldIndex);
	}

	public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index, int oldIndex)
	{
		_NewStartingIndex = -1;
		_OldStartingIndex = -1;
		if (action != NotifyCollectionChangedAction.Move)
		{
			throw new ArgumentException("WrongActionForCtor");
		}
		if (index < 0)
		{
			throw new ArgumentException("IndexCannotBeNegative");
		}
		object[] array = new object[1] { changedItem };
		InitializeMoveOrReplace(action, array, array, index, oldIndex);
	}

	public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem, int index)
	{
		_NewStartingIndex = -1;
		_OldStartingIndex = -1;
		if (action != NotifyCollectionChangedAction.Replace)
		{
			throw new ArgumentException("WrongActionForCtor");
		}
		InitializeMoveOrReplace(action, new object[1] { newItem }, new object[1] { oldItem }, index, index);
	}

	private void InitializeAdd(NotifyCollectionChangedAction action, IList newItems, int newStartingIndex)
	{
		_Action = action;
		_NewItems = ((newItems == null) ? null : ArrayList.ReadOnly(newItems));
		_NewStartingIndex = newStartingIndex;
	}

	private void InitializeAddOrRemove(NotifyCollectionChangedAction action, IList changedItems, int startingIndex)
	{
		switch (action)
		{
		case NotifyCollectionChangedAction.Add:
			InitializeAdd(action, changedItems, startingIndex);
			break;
		case NotifyCollectionChangedAction.Remove:
			InitializeRemove(action, changedItems, startingIndex);
			break;
		}
	}

	private void InitializeMoveOrReplace(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex, int oldStartingIndex)
	{
		InitializeAdd(action, newItems, startingIndex);
		InitializeRemove(action, oldItems, oldStartingIndex);
	}

	private void InitializeRemove(NotifyCollectionChangedAction action, IList oldItems, int oldStartingIndex)
	{
		_Action = action;
		_OldItems = ((oldItems == null) ? null : ArrayList.ReadOnly(oldItems));
		_OldStartingIndex = oldStartingIndex;
	}
}
