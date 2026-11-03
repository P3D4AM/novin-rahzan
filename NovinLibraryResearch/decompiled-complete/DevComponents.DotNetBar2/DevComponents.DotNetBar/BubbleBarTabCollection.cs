using System.Collections;
using DevComponents.UI.ContentManager;

namespace DevComponents.DotNetBar;

public class BubbleBarTabCollection : CollectionBase
{
	private BubbleBar _0AC3;

	public BubbleBarTab this[int index]
	{
		get
		{
			return (BubbleBarTab)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public BubbleBarTabCollection(BubbleBar owner)
	{
		_0AC3 = owner;
	}

	public int Add(BubbleBarTab tab)
	{
		return base.List.Add(tab);
	}

	public void Insert(int index, BubbleBarTab value)
	{
		base.List.Insert(index, value);
	}

	public int IndexOf(BubbleBarTab value)
	{
		return base.List.IndexOf(value);
	}

	public bool Contains(BubbleBarTab value)
	{
		return base.List.Contains(value);
	}

	public void Remove(BubbleBarTab value)
	{
		base.List.Remove(value);
	}

	protected override void OnRemoveComplete(int index, object value)
	{
		base.OnRemoveComplete(index, value);
		BubbleBarTab bubbleBarTab = value as BubbleBarTab;
		_0AC3._2810(bubbleBarTab);
		bubbleBarTab.SetParent(null);
	}

	protected override void OnInsertComplete(int index, object value)
	{
		base.OnInsertComplete(index, value);
		BubbleBarTab bubbleBarTab = value as BubbleBarTab;
		bubbleBarTab.SetParent(_0AC3);
		_0AC3.ᕥ(bubbleBarTab);
	}

	public void CopyTo(BubbleBarTab[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	internal void _0CD0(IBlock[] ঀ)
	{
		base.List.CopyTo(ঀ, 0);
	}

	internal void _0CD0(ISimpleTab[] ঀ)
	{
		base.List.CopyTo(ঀ, 0);
	}

	protected override void OnClear()
	{
		_0AC3._2811();
		base.OnClear();
	}

	internal BubbleBarTab ᾬ(BubbleBarTab _281A)
	{
		int num = 0;
		if (_281A != null)
		{
			num = IndexOf(_281A) + 1;
		}
		for (int i = num; i < base.Count; i++)
		{
			if (this[i].Visible)
			{
				return this[i];
			}
		}
		return null;
	}

	internal BubbleBarTab _281B(BubbleBarTab _281A)
	{
		int num = 0;
		if (_281A != null)
		{
			num = IndexOf(_281A) - 1;
		}
		for (int num2 = num; num2 >= 0; num2--)
		{
			if (this[num2].Visible)
			{
				return this[num2];
			}
		}
		return null;
	}
}
