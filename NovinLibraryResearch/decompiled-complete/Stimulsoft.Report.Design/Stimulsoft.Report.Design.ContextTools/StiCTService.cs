using System;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Design.ContextTools;

[StiServiceCategoryBitmap(typeof(StiCTService), "Stimulsoft.Report.Design.Images.Designer.ContextToolService.png")]
public abstract class StiCTService : StiService
{
	public sealed override string ServiceCategory => StiLocalization.Get("Services", "categoryContextTools");

	public sealed override Type ServiceType => typeof(StiCTService);

	public abstract StiPageType PageType { get; }

	public abstract string GroupCategory { get; }

	public abstract int Position { get; }

	protected StiComponent Component { get; set; }

	protected StiDesignerControl Designer { get; set; }

	public abstract Type GetInterfaceType();

	public abstract void CreateMenuItem(StiDesignerControl designer);
}
