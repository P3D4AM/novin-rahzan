using System;
using System.Collections;

namespace DevComponents.DotNetBar.Primitives;

public class NotifyCollectionChangedEventArgs : EventArgs
{
	private NotifyCollectionChangedAction _ᨾ;

	private IList _ᨿ;

	private int _ᩀ;

	private IList _ᩁ;

	private int _ᩂ;

	public NotifyCollectionChangedAction Action => _ᨾ;

	public IList NewItems => _ᨿ;

	public int NewStartingIndex => _ᩀ;

	public IList OldItems => _ᩁ;

	public int OldStartingIndex => _ᩂ;

	public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action)
	{
		_ᩀ = -1;
		_ᩂ = -1;
		if (action != NotifyCollectionChangedAction.Reset)
		{
			throw new ArgumentException("WrongActionForCtor");
		}
		ᩃ(action, null, -1);
	}

	public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems)
	{
		_ᩀ = -1;
		_ᩂ = -1;
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
			ᩃ(action, null, -1);
		}
		else
		{
			if (changedItems == null)
			{
				throw new ArgumentNullException("changedItems");
			}
			ᩆ(action, changedItems, -1);
		}
	}

	public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem)
	{
		_ᩀ = -1;
		_ᩂ = -1;
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
			ᩃ(action, null, -1);
		}
		else
		{
			ᩆ(action, new object[1] { changedItem }, -1);
		}
	}

	public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems)
	{
		_ᩀ = -1;
		_ᩂ = -1;
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
		ᩉ(action, newItems, oldItems, -1, -1);
	}

	public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems, int startingIndex)
	{
		_ᩀ = -1;
		_ᩂ = -1;
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
			ᩃ(action, null, -1);
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
			ᩆ(action, changedItems, startingIndex);
		}
	}

	public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index)
	{
		_ᩀ = -1;
		_ᩂ = -1;
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
			ᩃ(action, null, -1);
		}
		else
		{
			ᩆ(action, new object[1] { changedItem }, index);
		}
	}

	public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem)
	{
		_ᩀ = -1;
		_ᩂ = -1;
		if (action != NotifyCollectionChangedAction.Replace)
		{
			throw new ArgumentException("WrongActionForCtor");
		}
		ᩉ(action, new object[1] { newItem }, new object[1] { oldItem }, -1, -1);
	}

	public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex)
	{
		_ᩀ = -1;
		_ᩂ = -1;
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
		ᩉ(action, newItems, oldItems, startingIndex, startingIndex);
	}

	public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems, int index, int oldIndex)
	{
		_ᩀ = -1;
		_ᩂ = -1;
		if (action != NotifyCollectionChangedAction.Move)
		{
			throw new ArgumentException("WrongActionForCtor");
		}
		if (index < 0)
		{
			throw new ArgumentException("IndexCannotBeNegative");
		}
		ᩉ(action, changedItems, changedItems, index, oldIndex);
	}

	public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index, int oldIndex)
	{
		_ᩀ = -1;
		_ᩂ = -1;
		if (action != NotifyCollectionChangedAction.Move)
		{
			throw new ArgumentException("WrongActionForCtor");
		}
		if (index < 0)
		{
			throw new ArgumentException("IndexCannotBeNegative");
		}
		object[] array = new object[1] { changedItem };
		ᩉ(action, array, array, index, oldIndex);
	}

	public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem, int index)
	{
		_ᩀ = -1;
		_ᩂ = -1;
		if (action != NotifyCollectionChangedAction.Replace)
		{
			throw new ArgumentException("WrongActionForCtor");
		}
		ᩉ(action, new object[1] { newItem }, new object[1] { oldItem }, index, index);
	}

	private void ᩃ(NotifyCollectionChangedAction ڃ, IList ᩄ, int ᩅ)
	{
		_ᨾ = ڃ;
		_ᨿ = ((ᩄ == null) ? null : ArrayList.ReadOnly(ᩄ));
		_ᩀ = ᩅ;
	}

	private void ᩆ(NotifyCollectionChangedAction ڃ, IList ᩇ, int ᩈ)
	{
		switch (ڃ)
		{
		case NotifyCollectionChangedAction.Add:
			ᩃ(ڃ, ᩇ, ᩈ);
			break;
		case NotifyCollectionChangedAction.Remove:
			ᩌ(ڃ, ᩇ, ᩈ);
			break;
		}
	}

	private void ᩉ(NotifyCollectionChangedAction ڃ, IList ᩄ, IList ᩊ, int ᩈ, int ᩋ)
	{
		ᩃ(ڃ, ᩄ, ᩈ);
		ᩌ(ڃ, ᩊ, ᩋ);
	}

	private void ᩌ(NotifyCollectionChangedAction ڃ, IList ᩊ, int ᩋ)
	{
		_ᨾ = ڃ;
		_ᩁ = ((ᩊ == null) ? null : ArrayList.ReadOnly(ᩊ));
		_ᩂ = ᩋ;
	}
}
