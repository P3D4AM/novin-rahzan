using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Xml;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class ShortcutsCollection : CollectionBase
{
	private BaseItem m_Parent;

	public eShortcut this[int index]
	{
		get
		{
			return (eShortcut)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public BaseItem Parent
	{
		get
		{
			return m_Parent;
		}
		set
		{
			m_Parent = value;
			if (m_Parent != null)
			{
				m_Parent.RefreshShortcutString();
			}
		}
	}

	public ShortcutsCollection(BaseItem parent)
	{
		m_Parent = parent;
	}

	public int Add(eShortcut key)
	{
		int num = 0;
		return base.List.Add(key);
	}

	protected override void OnInsertComplete(int index, object value)
	{
		base.OnInsertComplete(index, value);
		RefreshOwnerShortcuts();
	}

	public void Insert(int index, eShortcut value)
	{
		base.List.Insert(index, value);
		RefreshOwnerShortcuts();
	}

	public int IndexOf(eShortcut value)
	{
		return base.List.IndexOf(value);
	}

	public string ToString(string Delimiter)
	{
		if (base.List.Count == 0)
		{
			return "";
		}
		StringBuilder stringBuilder = new StringBuilder(base.List.Count * (2 + Delimiter.Length));
		int num = base.List.Count - 1;
		for (int i = 0; i < num; i++)
		{
			stringBuilder.Append((int)base.List[i] + Delimiter);
		}
		stringBuilder.Append(((int)base.List[num]).ToString());
		return stringBuilder.ToString();
	}

	public void FromString(string Data, string Delimiter)
	{
		base.List.Clear();
		string[] array = Data.Split(Delimiter.ToCharArray());
		string[] array2 = array;
		foreach (string s in array2)
		{
			base.List.Add((eShortcut)XmlConvert.ToInt32(s));
		}
	}

	public bool Contains(eShortcut value)
	{
		return base.List.Contains(value);
	}

	public void Remove(eShortcut value)
	{
		base.List.Remove(value);
	}

	protected override void OnRemove(int index, object value)
	{
		RemoveOwnerShortcuts();
		base.OnRemove(index, value);
	}

	protected override void OnRemoveComplete(int index, object value)
	{
		base.OnRemoveComplete(index, value);
		RefreshOwnerShortcuts();
	}

	protected override void OnClear()
	{
		base.OnClear();
		RemoveOwnerShortcuts();
	}

	protected override void OnClearComplete()
	{
		RefreshOwnerShortcuts();
		base.OnClearComplete();
	}

	public void CopyTo(eShortcut[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	private void RefreshOwnerShortcuts()
	{
		if (m_Parent != null)
		{
			m_Parent.RefreshShortcutString();
		}
		IOwner owner = null;
		if (m_Parent != null)
		{
			owner = m_Parent.GetOwner() as IOwner;
		}
		if (m_Parent != null && owner != null)
		{
			owner.RemoveShortcutsFromItem(m_Parent);
			owner.AddShortcutsFromItem(m_Parent);
		}
	}

	private void RemoveOwnerShortcuts()
	{
		IOwner owner = null;
		if (m_Parent != null)
		{
			owner = m_Parent.GetOwner() as IOwner;
		}
		if (m_Parent != null)
		{
			owner?.RemoveShortcutsFromItem(m_Parent);
		}
	}
}
