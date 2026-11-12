using System;
using System.Collections.Generic;
using System.ComponentModel;
using Stimulsoft.Controls.Win.Editors.Primitives;

namespace Stimulsoft.Controls.Win.Editors;

public class VisualStringListInput : VisualListInput
{
	private List<string> _Items = new List<string>();

	private string _Text = "";

	private string _LastValidatedInputStack = "";

	private string _LastMatch = "";

	private bool _LastMatchComplete;

	public List<string> Items => _Items;

	protected string LastValidatedInputStack
	{
		get
		{
			return _LastValidatedInputStack;
		}
		set
		{
			_LastValidatedInputStack = value;
		}
	}

	protected string LastMatch
	{
		get
		{
			return _LastMatch;
		}
		set
		{
			_LastMatch = value;
		}
	}

	protected bool LastMatchComplete
	{
		get
		{
			return _LastMatchComplete;
		}
		set
		{
			_LastMatchComplete = value;
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
			if (_Text != value)
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
				_Text = value;
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

	[DefaultValue(-1)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int SelectedIndex
	{
		get
		{
			if (IsEmpty || _Text == "")
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

	public event EventHandler SelectedIndexChanged;

	protected virtual List<string> GetItems()
	{
		return _Items;
	}

	protected override bool ValidateNewInputStack(string s)
	{
		_LastValidatedInputStack = "";
		_LastMatch = "";
		_LastMatchComplete = false;
		if (s.Length > 0)
		{
			List<string> items = GetItems();
			StartsWithPredicate startsWithPredicate = new StartsWithPredicate(s.ToLower(), 2);
			List<string> list = items.FindAll(startsWithPredicate.MatchTop);
			if (list == null || list.Count == 0)
			{
				return false;
			}
			_LastMatch = list[0];
			_LastValidatedInputStack = s;
			_LastMatchComplete = list.Count == 1;
			return true;
		}
		return base.ValidateNewInputStack(s);
	}

	protected override void OnInputStackChanged()
	{
		bool flag = false;
		if (_LastValidatedInputStack.Length > 0 && base.InputStack == _LastValidatedInputStack)
		{
			flag = _Text != _LastMatch;
			_Text = _LastMatch;
		}
		else if (base.InputStack.Length == 0)
		{
			flag = _Text != "";
			_Text = "";
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
		if (_LastMatchComplete)
		{
			InputComplete(sendNotification: true);
			_LastMatchComplete = false;
		}
		base.OnInputKeyAccepted();
	}

	protected virtual void OnSelectedIndexChanged(EventArgs e)
	{
		if (this.SelectedIndexChanged != null)
		{
			this.SelectedIndexChanged(this, e);
		}
	}

	protected override string GetInputStringValue()
	{
		if (_Text == null)
		{
			return "";
		}
		return _Text;
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
