using System;
using System.Threading;

namespace DevComponents.Editors;

public class IpAddressGroup : VisualInputGroup
{
	private EventHandler ᗲ;

	private string _ᗸ;

	private bool __2B3C;

	private bool __22CF;

	public string Value
	{
		get
		{
			if (base.IsFocused && _2B40())
			{
				return _2547();
			}
			return _ᗸ;
		}
		set
		{
			bool flag = _ᗸ != value;
			if (!string.IsNullOrEmpty(value) && !_2B3D(value))
			{
				throw new ArgumentException("Value is not valid IP value");
			}
			_ᗸ = value;
			_2548();
			if (flag)
			{
				_23B3(EventArgs.Empty);
			}
		}
	}

	public event EventHandler ValueChanged
	{
		add
		{
			EventHandler eventHandler = ᗲ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᗲ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᗲ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᗲ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public IpAddressGroup()
	{
		for (int i = 0; i < 4; i++)
		{
			VisualIntegerInput item = new VisualIntegerInput
			{
				MaxValue = 255,
				MinValue = 0
			};
			Items.Add(item);
			if (i < 3)
			{
				VisualLabel item2 = new VisualLabel
				{
					Text = "."
				};
				Items.Add(item2);
			}
		}
	}

	private void _2548()
	{
		if (__2B3C)
		{
			return;
		}
		__2B3C = true;
		try
		{
			if (string.IsNullOrEmpty(_ᗸ))
			{
				for (int i = 0; i < Items.Count; i++)
				{
					if (Items[i] is VisualIntegerInput)
					{
						VisualIntegerInput visualIntegerInput = Items[i] as VisualIntegerInput;
						visualIntegerInput.IsEmpty = true;
					}
				}
				return;
			}
			int[] array = _2B3F(_ᗸ);
			int num = 0;
			for (int j = 0; j < Items.Count; j++)
			{
				if (Items[j] is VisualIntegerInput)
				{
					VisualIntegerInput visualIntegerInput2 = Items[j] as VisualIntegerInput;
					visualIntegerInput2.Value = array[num];
					num++;
				}
			}
		}
		finally
		{
			__2B3C = false;
		}
	}

	private void _23B3(EventArgs ؿ)
	{
		EventHandler eventHandler = ᗲ;
		if (eventHandler != null)
		{
			ᗲ(this, ؿ);
		}
	}

	internal bool _2B3D(string _2B3E)
	{
		int[] array = _2B3F(_2B3E);
		return array != null;
	}

	private int[] _2B3F(string _2B3E)
	{
		if (string.IsNullOrEmpty(_2B3E))
		{
			return null;
		}
		string[] array = _2B3E.Split(new char[1] { '.' });
		if (array.Length != 4)
		{
			return null;
		}
		int[] array2 = new int[4];
		for (int i = 0; i < array.Length; i++)
		{
			int result = 0;
			if (int.TryParse(array[i], out result))
			{
				if (result < 0 || result > 255)
				{
					return null;
				}
				array2[i] = result;
				continue;
			}
			return null;
		}
		return array2;
	}

	private bool _2B40()
	{
		bool flag = true;
		for (int i = 0; i < Items.Count; i++)
		{
			if (!(Items[i] is VisualIntegerInput visualIntegerInput))
			{
				continue;
			}
			if (visualIntegerInput.IsEmpty)
			{
				if (!flag && base.AllowEmptyState)
				{
					return false;
				}
				continue;
			}
			if (flag && i > 0)
			{
				return false;
			}
			flag = false;
		}
		return true;
	}

	protected override void OnLostFocus()
	{
		_2B41();
		base.OnLostFocus();
	}

	protected override void OnInputChanged(VisualInputBase input)
	{
		if (!__2B3C && _2547() != null)
		{
			__2B3C = true;
			try
			{
				_2B41();
			}
			finally
			{
				__2B3C = false;
			}
		}
		base.OnInputChanged(input);
	}

	private string _2547()
	{
		int[] array = new int[4];
		int num = 0;
		bool flag = false;
		for (int i = 0; i < Items.Count; i++)
		{
			if (!(Items[i] is VisualIntegerInput visualIntegerInput))
			{
				continue;
			}
			if (visualIntegerInput.IsEmpty)
			{
				if (base.AllowEmptyState)
				{
					flag = true;
					break;
				}
				array[num] = 0;
			}
			array[num] = visualIntegerInput.Value;
			num++;
		}
		if (flag)
		{
			return null;
		}
		string text = "";
		for (int j = 0; j < array.Length; j++)
		{
			text += array[j];
			if (j < array.Length - 1)
			{
				text += ".";
			}
		}
		return text;
	}

	private void _2B41()
	{
		Value = _2547();
	}

	protected override void ResetValue()
	{
		__22CF = true;
		try
		{
			Value = null;
		}
		finally
		{
			__22CF = false;
		}
	}
}
