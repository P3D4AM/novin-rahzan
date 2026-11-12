using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class WizardPageCollection : CollectionBase
{
	private Wizard m_Parent;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IgnoreEvents;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Wizard Parent => m_Parent;

	internal Wizard ParentWizard
	{
		get
		{
			return m_Parent;
		}
		set
		{
			m_Parent = value;
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
		if (!IgnoreEvents && m_Parent != null)
		{
			m_Parent.OnWizardPageRemoved(value as WizardPage);
		}
	}

	protected override void OnInsertComplete(int index, object value)
	{
		base.OnInsertComplete(index, value);
		if (!IgnoreEvents && m_Parent != null)
		{
			m_Parent.OnWizardPageAdded(value as WizardPage);
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
		if (!IgnoreEvents && m_Parent != null)
		{
			foreach (WizardPage item in base.List)
			{
				m_Parent.OnWizardPageRemoved(item);
			}
		}
		base.OnClear();
	}
}
