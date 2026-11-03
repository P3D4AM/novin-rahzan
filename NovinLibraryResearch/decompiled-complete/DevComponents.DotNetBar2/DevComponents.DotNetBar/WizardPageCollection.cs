using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

public class WizardPageCollection : CollectionBase
{
	private Wizard ӭ;

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IgnoreEvents;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Wizard Parent => ӭ;

	internal Wizard に
	{
		get
		{
			return ӭ;
		}
		set
		{
			ӭ = value;
		}
	}

	public WizardPage this[int index]
	{
		get
		{
			return (WizardPage)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public WizardPage this[string name]
	{
		get
		{
			foreach (WizardPage item in base.List)
			{
				if (((Control)item).Name == name)
				{
					return item;
				}
			}
			return null;
		}
	}

	public int Add(WizardPage WizardPage)
	{
		return base.List.Add(WizardPage);
	}

	public void AddRange(WizardPage[] WizardPages)
	{
		foreach (WizardPage value in WizardPages)
		{
			base.List.Add(value);
		}
	}

	public void Insert(int index, WizardPage value)
	{
		base.List.Insert(index, value);
	}

	public int IndexOf(WizardPage value)
	{
		return base.List.IndexOf(value);
	}

	public bool Contains(WizardPage value)
	{
		return base.List.Contains(value);
	}

	public void Remove(WizardPage value)
	{
		base.List.Remove(value);
	}

	protected override void OnRemove(int index, object value)
	{
		base.OnRemove(index, value);
	}

	protected override void OnRemoveComplete(int index, object value)
	{
		base.OnRemoveComplete(index, value);
		if (!IgnoreEvents && ӭ != null)
		{
			ӭ.え(value as WizardPage);
		}
	}

	protected override void OnInsertComplete(int index, object value)
	{
		base.OnInsertComplete(index, value);
		if (!IgnoreEvents && ӭ != null)
		{
			ӭ.ぇ(value as WizardPage);
		}
	}

	public void CopyTo(WizardPage[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void CopyTo(WizardPage[] array)
	{
		base.List.CopyTo(array, 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void CopyTo(WizardPageCollection col)
	{
		foreach (WizardPage item in base.List)
		{
			col.Add(item);
		}
	}

	protected override void OnClear()
	{
		if (!IgnoreEvents && ӭ != null)
		{
			foreach (WizardPage item in base.List)
			{
				ӭ.え(item);
			}
		}
		base.OnClear();
	}
}
