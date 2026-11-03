using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

public class MultiFormAppContext : ApplicationContext
{
	private List<WeakReference> __23D4 = new List<WeakReference>();

	private static MultiFormAppContext __23D5;

	public List<Form> OpenedForms
	{
		get
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Expected O, but got Unknown
			List<Form> list = new List<Form>();
			foreach (WeakReference item in __23D4)
			{
				object target = item.Target;
				if (target != null && target is Form)
				{
					list.Add((Form)target);
				}
			}
			return list;
		}
	}

	public static MultiFormAppContext Current
	{
		get
		{
			return __23D5;
		}
		set
		{
			__23D5 = value;
		}
	}

	public MultiFormAppContext(Form initialForm)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		Application.ApplicationExit += OnApplicationExit;
		WeakReference item = new WeakReference(initialForm);
		__23D4.Add(item);
		initialForm.FormClosed += new FormClosedEventHandler(_23D6);
		((Control)initialForm).Show();
	}

	public void RegisterOpenForm(Form form)
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		bool flag = false;
		foreach (WeakReference item2 in __23D4)
		{
			object target = item2.Target;
			if (target != null && target.Equals(form))
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			WeakReference item = new WeakReference(form);
			__23D4.Add(item);
			form.FormClosed += new FormClosedEventHandler(_23D6);
		}
	}

	private void _23D6(object _٠, FormClosedEventArgs ؿ)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		Form val = (Form)_٠;
		val.FormClosed -= new FormClosedEventHandler(_23D6);
		foreach (WeakReference item in __23D4)
		{
			object target = item.Target;
			if (target != null && target.Equals(val))
			{
				__23D4.Remove(item);
				break;
			}
		}
		if (__23D4.Count == 0)
		{
			((ApplicationContext)this).ExitThread();
		}
	}

	protected virtual void OnApplicationExit(object sender, EventArgs e)
	{
	}
}
