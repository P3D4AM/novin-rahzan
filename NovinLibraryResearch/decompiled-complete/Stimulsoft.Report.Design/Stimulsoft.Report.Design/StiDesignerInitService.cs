using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Design;

public abstract class StiDesignerInitService : StiService
{
	public override string ServiceCategory => Loc.Get("Services", "categoryDesigner");

	public virtual StiDesignerControl Designer { get; set; }

	public virtual StiReport Report
	{
		get
		{
			return Designer?.Report;
		}
		set
		{
			Designer.Report = value;
		}
	}

	public StiPage CurrentPage => Designer.Report.GetCurrentPage();

	public StiDesignerInfo Info => Report?.Info;

	public virtual void ApplyStyle()
	{
	}

	public virtual void Localize()
	{
	}

	public virtual void Init(StiDesignerControl designer)
	{
		Designer = designer;
	}

	public virtual void Dispose()
	{
		Designer = null;
	}
}
