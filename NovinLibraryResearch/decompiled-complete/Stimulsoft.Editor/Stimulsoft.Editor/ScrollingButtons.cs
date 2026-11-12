using System;
using System.Collections;

namespace Stimulsoft.Editor;

[Serializable]
public class ScrollingButtons : ArrayList, IScrollingButtons, IList, ICollection, IEnumerable
{
	private IScrolling owner;

	public new ScrollingButton this[int index]
	{
		get
		{
			return (ScrollingButton)base[index];
		}
		set
		{
			base[index] = value;
		}
	}

	public ScrollingButtons(IScrolling owner)
	{
		this.owner = owner;
	}

	public override void RemoveAt(int index)
	{
		base.RemoveAt(index);
	}

	public override void Clear()
	{
		base.Clear();
		if (owner != null)
		{
			((Scrolling)owner).Update();
		}
	}

	public override int Add(object value)
	{
		if (owner != null)
		{
			((ScrollingButton)value).Owner = owner;
		}
		int result = base.Add(value);
		if (owner != null)
		{
			((Scrolling)owner).Update();
		}
		return result;
	}

	public virtual void Assign(IScrollingButtons source)
	{
		if (owner != null)
		{
			((Scrolling)owner).BeginUpdate();
		}
		try
		{
			Clear();
			foreach (IScrollingButton item in source)
			{
				ScrollingButton scrollingButton = new ScrollingButton();
				scrollingButton.Assign(item);
				Add(scrollingButton);
			}
		}
		finally
		{
			if (owner != null)
			{
				((Scrolling)owner).EndUpdate();
			}
		}
	}

	public int Add(ScrollingButton value)
	{
		if (owner != null)
		{
			value.Owner = owner;
		}
		return base.Add(value);
	}

	public void AddRange(ScrollingButton[] values)
	{
		base.AddRange(values);
		if (owner != null)
		{
			foreach (ScrollingButton scrollingButton in values)
			{
				scrollingButton.Owner = owner;
			}
		}
	}

	public int AddScrollingButton()
	{
		return Add(new ScrollingButton());
	}

	public int AddScrollingButton(string name, string description, int imageIndex)
	{
		ScrollingButton scrollingButton = new ScrollingButton();
		scrollingButton.Name = name;
		scrollingButton.Description = description;
		scrollingButton.ImageIndex = imageIndex;
		return Add(scrollingButton);
	}

	public IScrollingButton GetScrollingButton(int index)
	{
		return this[index];
	}
}
