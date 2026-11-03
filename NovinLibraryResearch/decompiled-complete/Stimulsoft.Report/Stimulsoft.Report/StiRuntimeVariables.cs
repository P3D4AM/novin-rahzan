using System;
using System.Collections;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report;

public class StiRuntimeVariables : ICloneable
{
	public StiPage Page;

	internal StiSimpleText TextBox;

	public int Line;

	public int Column;

	public int LineThrough;

	public Hashtable DataSourcesPosition = new Hashtable();

	private int pageIndex;

	private int currentPrintPage;

	internal int PageIndex
	{
		get
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Invalid comparison between Unknown and I4
			if (TextBox != null && TextBox.Report != null && (int)TextBox.Report.EngineVersion == 2)
			{
				int num = TextBox.Report.RenderedPages.IndexOf(TextBox.Page);
				if (num != -1)
				{
					return num + 1;
				}
			}
			return pageIndex;
		}
		set
		{
			pageIndex = value;
		}
	}

	public int CurrentPrintPage
	{
		get
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Invalid comparison between Unknown and I4
			if (TextBox != null && TextBox.Report != null && (int)TextBox.Report.EngineVersion == 2)
			{
				int num = TextBox.Report.RenderedPages.IndexOf(TextBox.Page);
				if (num != -1)
				{
					return num + 1;
				}
			}
			return currentPrintPage;
		}
		set
		{
			currentPrintPage = value;
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public void SetVariables(StiReport report)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)report.EngineVersion == 1)
		{
			if (Page.PageInfoV1.TotalPageCount != -1)
			{
				report.TotalPageCount = Page.PageInfoV1.TotalPageCount;
			}
			report.PageNumber = Page.PageInfoV1.PageNumber;
		}
		report.Line = Line;
		report.LineThrough = LineThrough;
		report.Column = Column;
		report.CurrentPrintPage = CurrentPrintPage;
		foreach (StiDataSource dataSource in report.Dictionary.DataSources)
		{
			dataSource.Position = (int)DataSourcesPosition[dataSource.Name];
		}
	}

	public StiRuntimeVariables(StiReport report)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Invalid comparison between Unknown and I4
		Page = (((int)report.EngineVersion == 1) ? report.EngineV1.PageInProgress : report.Engine.Page);
		Line = report.Line;
		LineThrough = report.LineThrough;
		Column = report.Column;
		CurrentPrintPage = report.CurrentPrintPage;
		foreach (StiDataSource dataSource in report.Dictionary.DataSources)
		{
			DataSourcesPosition[dataSource.Name] = dataSource.Position;
		}
	}
}
