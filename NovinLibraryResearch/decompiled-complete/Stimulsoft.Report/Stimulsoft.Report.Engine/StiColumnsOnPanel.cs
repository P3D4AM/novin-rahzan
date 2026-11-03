using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

internal class StiColumnsOnPanel
{
	internal StiEngine Engine { get; }

	internal int Count
	{
		get
		{
			if (!(Engine.TemplateContainer is StiPanel stiPanel))
			{
				return 0;
			}
			return stiPanel.Columns;
		}
	}

	internal bool RightToLeft
	{
		get
		{
			if (Engine.TemplateContainer is StiPanel stiPanel)
			{
				return stiPanel.RightToLeft;
			}
			return false;
		}
	}

	internal double ColumnGaps
	{
		get
		{
			if (!(Engine.TemplateContainer is StiPanel stiPanel))
			{
				return 0.0;
			}
			return stiPanel.ColumnGaps;
		}
	}

	internal int CurrentColumn { get; set; } = 1;


	internal double GetColumnWidth()
	{
		if (!(Engine.TemplateContainer is StiPanel stiPanel))
		{
			return 0.0;
		}
		return stiPanel.GetColumnWidth();
	}

	public StiColumnsOnPanel(StiEngine engine)
	{
		Engine = engine;
	}
}
