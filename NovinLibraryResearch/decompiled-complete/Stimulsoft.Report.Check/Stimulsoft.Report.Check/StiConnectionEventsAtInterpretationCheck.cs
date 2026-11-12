using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiConnectionEventsAtInterpretationCheck : StiConnectionCheck
{
	public override bool PreviewVisible => false;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiEventsAtInterpretationCheckShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiEventsAtInterpretationCheckLong"), EventName, ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Warning;

	public string EventName { get; set; }

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		if (!(base.Element is StiDatabase) || (report.CalculationMode != StiCalculationMode.Interpretation && !StiOptions.Engine.ForceInterpretationMode))
		{
			return null;
		}
		StiDatabase stiDatabase = base.Element as StiDatabase;
		StiEventsCollection events = stiDatabase.GetEvents();
		if (events == null)
		{
			return null;
		}
		IEnumerable<string> enumerable = from StiEvent e in events
			where !string.IsNullOrWhiteSpace(e.Script) && !e.Script.StartsWith("<xml xmlns=\"https://developers.google.com/blockly/xml\">")
			select e.PropertyName;
		if (enumerable == null || !enumerable.Any())
		{
			return null;
		}
		string eventName = string.Join(", ", enumerable);
		return new StiConnectionEventsAtInterpretationCheck
		{
			Element = obj,
			EventName = eventName
		};
	}
}
