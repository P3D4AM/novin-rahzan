using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using DevComponents.Editors.Primitives;

namespace DevComponents.Editors;

public class VisualStringListInput : VisualListInput
{
	private List<string> _ᡨ = new List<string>();

	private string _ኚ = "";

	private string __255A = "";

	private string __255B = "";

	private bool __255C;

	private EventHandler _05CB;

	public List<string> Items => _ᡨ;

	protected string LastValidatedInputStack
	{
		get
		{
			return __255A;
		}
		set
		{
			__255A = value;
		}
	}

	protected string LastMatch
	{
		get
		{
			return __255B;
		}
		set
		{
			__255B = value;
		}
	}

	protected bool LastMatchComplete
	{
		get
		{
			return __255C;
		}
		set
		{
			__255C = value;
		}
	}

	[DefaultValue("")]
	public string Text
	{
		get
		{
			return GetInputStringValue();
		}
		set
		{
			if (_ኚ != value)
			{
				List<string> items = GetItems();
				if (value == "" && !base.AllowEmptyState && items.Count > 0)
				{
					value = items[0];
				}
				else if (value != null && value.Length > 0 && !items.Contains(value))
				{
					value = ((base.AllowEmptyState || items.Count <= 0) ? "" : items[0]);
				}
				_ኚ = value;
				ResetInputStack();
				OnInputChanged();
				OnSelectedIndexChanged(new EventArgs());
				InvalidateArrange();
				if (IsEmpty)
				{
					IsEmpty = false;
				}
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(-1)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int SelectedIndex
	{
		get
		{
			if (IsEmpty || _ኚ == "")
			{
				return -1;
			}
			List<string> items = GetItems();
			return items.IndexOf(Text);
		}
		set
		{
			if (value >= 0)
			{
				List<string> items = GetItems();
				Text = items[value];
			}
			else
			{
				Text = "";
			}
		}
	}

	public event EventHandler SelectedIndexChanged
	{
		add
		{
			EventHandler eventHandler = _05CB;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05CB, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _05CB;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _05CB, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	protected virtual List<string> GetItems()
	{
		return _ᡨ;
	}

	protected override bool ValidateNewInputStack(string s)
	{
		__255A = "";
		__255B = "";
		__255C = false;
		if (s.Length > 0)
		{
			List<string> items = GetItems();
			_255D _255D = new _255D(s.ToLower(), 2);
			List<string> list = items.FindAll(_255D._2561);
			if (list == null || list.Count == 0)
			{
				return false;
			}
			__255B = list[0];
			__255A = s;
			__255C = list.Count == 1;
			return true;
		}
		return base.ValidateNewInputStack(s);
	}

	protected override void OnInputStackChanged()
	{
		bool flag = false;
		if (__255A.Length > 0 && base.InputStack == __255A)
		{
			flag = _ኚ != __255B;
			_ኚ = __255B;
		}
		else if (base.InputStack.Length == 0)
		{
			flag = _ኚ != "";
			_ኚ = "";
		}
		InvalidateArrange();
		if (flag)
		{
			OnSelectedIndexChanged(new EventArgs());
		}
		base.OnInputStackChanged();
	}

	protected override void OnInputKeyAccepted()
	{
		if (__255C)
		{
			InputComplete(sendNotification: true);
			__255C = false;
		}
		base.OnInputKeyAccepted();
	}

	protected virtual void OnSelectedIndexChanged(EventArgs e)
	{
		if (_05CB != null)
		{
			_05CB(this, e);
		}
	}

	protected override string GetInputStringValue()
	{
		if (_ኚ == null)
		{
			return "";
		}
		return _ኚ;
	}

	public override void SelectNext()
	{
		int num = SelectedIndex + 1;
		List<string> items = GetItems();
		if (num >= items.Count)
		{
			num = 0;
		}
		SelectedIndex = num;
		base.SelectNext();
	}

	public override void SelectPrevious()
	{
		int num = SelectedIndex - 1;
		if (num < 0)
		{
			List<string> items = GetItems();
			num = items.Count - 1;
		}
		SelectedIndex = num;
		base.SelectPrevious();
	}
}
