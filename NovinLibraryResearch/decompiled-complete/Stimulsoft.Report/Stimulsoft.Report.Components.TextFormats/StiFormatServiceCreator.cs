using System.Linq;

namespace Stimulsoft.Report.Components.TextFormats;

internal static class StiFormatServiceCreator
{
	public static StiFormatService New(string identName)
	{
		return StiOptions.Services.Formats.FirstOrDefault((StiFormatService x) => ((object)x).GetType().Name == identName).CreateNew();
	}
}
