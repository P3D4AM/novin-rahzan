using System;
using System.Drawing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Design.Wizards;

[StiServiceCategoryBitmap(typeof(StiWizardService), "Stimulsoft.Report.Design.Bmp.Wizards.WizardService.gif")]
[StiServiceBitmap(typeof(StiWizardService), "Stimulsoft.Report.Design.Bmp.Wizards.WizardService.gif")]
[StiToolbox(true)]
public abstract class StiWizardService : StiService
{
	public sealed override string ServiceCategory => "Wizards";

	public sealed override Type ServiceType => typeof(StiWizardService);

	public abstract string HelpUrl { get; }

	public abstract Bitmap Image { get; }

	public abstract StiReport CreateReport(StiReport report);
}
