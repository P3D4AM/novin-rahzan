using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Base;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiDatabaseType
{
	MsSql,
	MySql,
	SqlCe
}
