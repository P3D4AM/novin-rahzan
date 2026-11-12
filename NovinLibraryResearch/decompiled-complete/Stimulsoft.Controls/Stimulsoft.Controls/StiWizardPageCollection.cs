using System.Collections;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace Stimulsoft.Controls;

public class StiWizardPageCollection : CollectionBase
{
	private StiWizard parent;

	internal bool LockActions;

	public StiWizardPage this[int index]
	{
		get
		{
			return (StiWizardPage)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public void Add(StiWizardPage page)
	{
		base.List.Add(page);
	}

	public void AddRange(StiWizardPage[] pages)
	{
		foreach (StiWizardPage page in pages)
		{
			Add(page);
		}
	}

	public void AddRange(StiWizardPageCollection pages)
	{
		foreach (StiWizardPage page in pages)
		{
			Add(page);
		}
	}

	public bool Contains(StiWizardPage page)
	{
		return base.List.Contains(page);
	}

	public int IndexOf(StiWizardPage page)
	{
		return base.List.IndexOf(page);
	}

	public void Insert(int index, StiWizardPage page)
	{
		base.List.Insert(index, page);
	}

	public void Remove(StiWizardPage page)
	{
		base.List.Remove(page);
	}

	protected override void OnInsert(int index, object value)
	{
		if (!LockActions)
		{
			((Control)parent).Controls.Add((Control)((value is Control) ? value : null));
		}
	}

	protected override void OnRemove(int index, object value)
	{
		if (!LockActions)
		{
			((Control)parent).Controls.Remove((Control)((value is Control) ? value : null));
		}
	}

	protected override void OnClear()
	{
		if (LockActions)
		{
			return;
		}
		int num = 0;
		while (num < ((ArrangedElementCollection)((Control)parent).Controls).Count)
		{
			Control val = ((Control)parent).Controls[num];
			if (val is StiWizardPage)
			{
				((Control)parent).Controls.Remove(val);
			}
			else
			{
				num++;
			}
		}
	}

	public StiWizardPageCollection()
		: this(null)
	{
	}

	public StiWizardPageCollection(StiWizard parent)
	{
		this.parent = parent;
	}
}
