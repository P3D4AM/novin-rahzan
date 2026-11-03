using System.Collections;

namespace Stimulsoft.Report;

public class StiReportsCollection : CollectionBase
{
	private StiReport owner;

	public StiReport this[int index]
	{
		get
		{
			return (StiReport)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public void Add(StiReport report)
	{
		Add(report, resetPageNumber: false, printOnPreviousPage: false);
	}

	public void Add(StiReport report, bool resetPageNumber, bool printOnPreviousPage)
	{
		if (report != owner)
		{
			report.SubReportsPrintOnPreviousPage = printOnPreviousPage;
			report.SubReportsResetPageNumber = resetPageNumber;
			report.Unit = owner.Unit;
			if (report.CompiledReport != null)
			{
				report.CompiledReport.Unit = owner.Unit;
			}
			base.List.Add(report);
		}
	}

	public void AddRange(StiReportsCollection reports)
	{
		foreach (StiReport report in reports)
		{
			Add(report);
		}
	}

	public void AddRange(StiReport[] reports)
	{
		foreach (StiReport report in reports)
		{
			Add(report);
		}
	}

	public bool Contains(StiReport report)
	{
		return base.List.Contains(report);
	}

	public int IndexOf(StiReport report)
	{
		return base.List.IndexOf(report);
	}

	public void Insert(int index, StiReport report)
	{
		base.List.Insert(index, report);
	}

	public void Remove(StiReport report)
	{
		base.List.Remove(report);
	}

	internal StiReportsCollection(StiReport owner)
	{
		this.owner = owner;
	}
}
