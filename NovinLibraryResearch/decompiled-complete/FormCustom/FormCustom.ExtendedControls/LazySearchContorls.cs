using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PacketParser;

namespace FormCustom.ExtendedControls;

public class LazySearchContorls<TItem, UContorl> : IDisposable where UContorl : DataGridView
{
	private readonly BindingSource bindingSource;

	private readonly UContorl control;

	private int perPage = 50;

	private SemaphoreSlim addDataTogridSemaphor = new SemaphoreSlim(1, 1);

	private bool isDisposed;

	public IEnumerable<TItem> Items { get; set; }

	public bool IsDisposed
	{
		get
		{
			if (!isDisposed)
			{
				return ((Control)(object)control).IsDisposed;
			}
			return true;
		}
		private set
		{
			isDisposed = value;
		}
	}

	public IEnumerable<TItem> SearchedItems { get; set; }

	public Func<TItem, bool> FirstExpresion { get; set; }

	public Func<TItem, bool> OtherExpresion { get; set; }

	public string SeachText { get; set; }

	public LazySearchContorls(List<TItem> items, UContorl control, BindingSource bindingSource)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		this.bindingSource = bindingSource;
		Items = items;
		this.control = control;
		((DataGridView)this.control).Scroll += new ScrollEventHandler(ScrollBar_Scroll);
	}

	public virtual void SetExpersions(Func<TItem, bool> firstExpresion, Func<TItem, bool> otherExpresion)
	{
		FirstExpresion = firstExpresion;
		OtherExpresion = otherExpresion;
	}

	private async void ScrollBar_Scroll(object sender, ScrollEventArgs e)
	{
		if (Items == null || bindingSource == null || Items.Count() == 0 || 100 * e.NewValue / bindingSource.Count <= 70)
		{
			return;
		}
		await addDataTogridSemaphor.WaitAsync();
		try
		{
			int num = Items.Count() / perPage;
			int num2 = bindingSource.Count / perPage;
			if (num != num2)
			{
				if (string.IsNullOrEmpty(SeachText))
				{
					AddToBindigSource(Items.Skip(bindingSource.Count).Take(perPage).ToList());
				}
				else
				{
					AddToBindigSource(SearchedItems.Skip(bindingSource.Count).Take(perPage).ToList());
				}
			}
		}
		finally
		{
			addDataTogridSemaphor.Release();
		}
	}

	private void AddToBindigSource(IEnumerable<TItem> items)
	{
		if (items == null)
		{
			return;
		}
		foreach (TItem item in items)
		{
			bindingSource.Add((object)item);
		}
	}

	public virtual Task Search(string seachText, CancellationToken token, Func<TItem, bool> firstExpersion)
	{
		return Search(seachText, token, firstExpersion, OtherExpresion);
	}

	public virtual Task Search(string seachText, CancellationToken token)
	{
		return Search(seachText, token, FirstExpresion, OtherExpresion);
	}

	public virtual async Task Search(string seachText, CancellationToken token, Func<TItem, bool> firstExpresion, Func<TItem, bool> otherExpresion)
	{
		SeachText = seachText?.FixString()?.RemoveCharactersForLinkSearch() ?? "";
		while (!((Control)(object)control).IsHandleCreated)
		{
			await Task.Delay(200);
			if (token.IsCancellationRequested || IsDisposed)
			{
				return;
			}
		}
		FirstExpresion = firstExpresion;
		OtherExpresion = otherExpresion;
		if (FirstExpresion == null || string.IsNullOrEmpty(SeachText))
		{
			SetControlDataSource(Items.Take(perPage));
			return;
		}
		SearchedItems = Items.ToList();
		bool flag = true;
		using (List<string>.Enumerator enumerator = seachText.SplitString().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				string value = (SeachText = enumerator.Current);
				if (token.IsCancellationRequested || IsDisposed)
				{
					return;
				}
				if (!string.IsNullOrEmpty(value))
				{
					if (flag || OtherExpresion == null)
					{
						flag = false;
						SearchedItems = SearchedItems?.Where(FirstExpresion);
					}
					else
					{
						SearchedItems = SearchedItems?.Where(OtherExpresion);
					}
				}
			}
		}
		if (!token.IsCancellationRequested && !IsDisposed)
		{
			SetControlDataSource(SearchedItems.Take(perPage).ToList());
		}
	}

	private void SetControlDataSource(IEnumerable<TItem> items)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		if (Items == null || Items.Count() == 0)
		{
			return;
		}
		if (((Control)(object)control).InvokeRequired)
		{
			((Control)(object)control).Invoke((Delegate)(MethodInvoker)delegate
			{
				SetControlDataSource(items);
			});
		}
		else
		{
			bindingSource.DataSource = items;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	public void Dispose(bool disposing)
	{
		if (!IsDisposed)
		{
			IsDisposed = true;
			Items = null;
			SearchedItems = null;
			bindingSource.DataSource = null;
			FirstExpresion = null;
			OtherExpresion = null;
		}
	}
}
