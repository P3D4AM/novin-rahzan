using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Base.Licenses;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiProductIdent : long
{
	Ultimate = 1L,
	Net = 2L,
	Wpf = 3L,
	Web = 4L,
	Silverlight = 5L,
	Js = 6L,
	Java = 7L,
	Php = 8L,
	NetCore = 9L,
	Uwp = 10L,
	Flex = 11L,
	BIDesigner = 12L,
	DbsJs = 13L,
	DbsWin = 14L,
	DbsWeb = 15L,
	BIDesktop = 16L,
	BIServer = 17L,
	BICloud = 18L,
	CloudReports = 20L,
	CloudDashboards = 21L,
	Angular = 22L,
	DbsAngular = 23L,
	DbsPhp = 24L,
	FormsWin = 25L,
	FormsWeb = 26L,
	FormsJs = 27L
}
