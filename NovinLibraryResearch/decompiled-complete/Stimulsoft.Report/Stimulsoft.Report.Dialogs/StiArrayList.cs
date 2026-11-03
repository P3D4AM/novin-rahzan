using System;
using System.Collections;

namespace Stimulsoft.Report.Dialogs;

public class StiArrayList : ArrayList
{
	public event EventHandler CollectionChanged;

	private void InvokeCollectionChanged()
	{
		this.CollectionChanged?.Invoke(this, EventArgs.Empty);
	}

	public override int Add(object value)
	{
		int result = base.Add(value);
		InvokeCollectionChanged();
		return result;
	}

	public int AddCore(object value)
	{
		return base.Add(value);
	}

	public override void AddRange(ICollection c)
	{
		base.AddRange(c);
		InvokeCollectionChanged();
	}

	public void AddRangeCore(ICollection c)
	{
		base.AddRange(c);
	}

	public override void Clear()
	{
		base.Clear();
		InvokeCollectionChanged();
	}

	public void ClearCore()
	{
		base.Clear();
	}

	public override void Insert(int index, object value)
	{
		base.Insert(index, value);
		InvokeCollectionChanged();
	}

	public override void InsertRange(int index, ICollection c)
	{
		base.InsertRange(index, c);
		InvokeCollectionChanged();
	}

	public override void Remove(object obj)
	{
		base.Remove(obj);
		InvokeCollectionChanged();
	}

	public override void RemoveAt(int index)
	{
		base.RemoveAt(index);
		InvokeCollectionChanged();
	}

	public override void RemoveRange(int index, int count)
	{
		base.RemoveRange(index, count);
		InvokeCollectionChanged();
	}
}
