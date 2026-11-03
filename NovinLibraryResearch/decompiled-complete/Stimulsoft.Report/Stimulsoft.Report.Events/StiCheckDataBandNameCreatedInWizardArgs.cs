using System;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Events;

public class StiCheckDataBandNameCreatedInWizardArgs : EventArgs
{
	public virtual StiBand Band { get; set; }

	public StiCheckDataBandNameCreatedInWizardArgs(StiBand band)
	{
		Band = band;
	}
}
