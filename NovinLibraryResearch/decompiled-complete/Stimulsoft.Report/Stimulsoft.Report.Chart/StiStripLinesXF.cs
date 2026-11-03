using System.Collections;

namespace Stimulsoft.Report.Chart;

public class StiStripLinesXF : CollectionBase
{
	public StiStripLineXF this[int index]
	{
		get
		{
			return (StiStripLineXF)base.InnerList[index];
		}
		set
		{
			base.InnerList[index] = value;
		}
	}

	public object Clone()
	{
		StiStripLinesXF stiStripLinesXF = new StiStripLinesXF();
		foreach (StiStripLineXF item in base.List)
		{
			stiStripLinesXF.Add(item.Clone() as StiStripLineXF);
		}
		return stiStripLinesXF;
	}

	public void Add(object valueObject, double value)
	{
		Add(new StiStripLineXF(valueObject, value));
	}

	public void Add(StiStripLineXF line)
	{
		base.List.Add(line);
	}

	public void AddRange(StiStripLineXF[] lines)
	{
		foreach (StiStripLineXF line in lines)
		{
			Add(line);
		}
	}

	public bool Contains(StiStripLineXF value)
	{
		return base.List.Contains(value);
	}

	public int IndexOf(StiStripLineXF value)
	{
		return base.List.IndexOf(value);
	}

	public void Insert(int index, StiStripLineXF value)
	{
		base.List.Insert(index, value);
	}

	public void Remove(StiStripLineXF value)
	{
		base.List.Remove(value);
	}
}
