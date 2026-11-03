using System;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;

namespace Stimulsoft.Report.Dictionary;

[StiServiceBitmap(typeof(StiTypeService), "Stimulsoft.Report.Bmp.Type.bmp")]
public abstract class StiTypeService : StiService
{
	public sealed override string ServiceCategory => StiLocalization.Get("Services", "categoryDictionary");

	public sealed override Type ServiceType => typeof(StiTypeService);

	public StiTypesCollection Types { get; set; } = new StiTypesCollection();

}
