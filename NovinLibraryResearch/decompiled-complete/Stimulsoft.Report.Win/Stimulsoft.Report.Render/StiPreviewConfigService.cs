using System;
using System.ComponentModel;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Viewer;

namespace Stimulsoft.Report.Render;

[Obsolete("Service 'StiPreviewConfigService' is obsolete. Please use 'StiViewerConfigService' service.")]
[EditorBrowsable(EditorBrowsableState.Never)]
public class StiPreviewConfigService : StiViewerConfigService
{
	public sealed override string ServiceCategory => StiLocalization.Get("Services", "categoryRender");

	public sealed override Type ServiceType => typeof(StiPreviewConfigService);
}
