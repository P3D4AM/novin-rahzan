using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Dashboard;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiOnlineMapLocationType
{
	Auto,
	AdminDivision1,
	AdminDivision2,
	CountryRegion,
	Neighborhood,
	PopulatedPlace,
	Postcode1,
	Postcode2,
	Postcode3,
	Postcode4
}
