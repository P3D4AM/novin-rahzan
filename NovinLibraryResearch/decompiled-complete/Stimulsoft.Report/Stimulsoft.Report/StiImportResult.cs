using System.Collections;

namespace Stimulsoft.Report;

public class StiImportResult
{
	public StiReport Report { get; }

	public ArrayList Errors { get; }

	public StiImportResult(StiReport report, ArrayList errors)
	{
		Report = report;
		Errors = errors;
	}
}
