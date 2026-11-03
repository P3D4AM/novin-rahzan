using System.Collections;

namespace Stimulsoft.Report.Engine;

internal class StiPageNumberCollection : CollectionBase
{
	public StiPageNumber this[int index]
	{
		get
		{
			return (StiPageNumber)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public void Add(StiPageNumber pageNumber)
	{
		base.List.Add(pageNumber);
	}

	public void AddRange(StiPageNumber[] pageNumbers)
	{
		base.InnerList.AddRange(pageNumbers);
	}

	public bool Contains(StiPageNumber pageNumber)
	{
		return base.List.Contains(pageNumber);
	}

	public int IndexOf(StiPageNumber pageNumber)
	{
		return base.List.IndexOf(pageNumber);
	}

	public void Insert(int index, StiPageNumber pageNumber)
	{
		base.List.Insert(index, pageNumber);
	}

	public void Remove(StiPageNumber pageNumber)
	{
		base.List.Remove(pageNumber);
	}
}
