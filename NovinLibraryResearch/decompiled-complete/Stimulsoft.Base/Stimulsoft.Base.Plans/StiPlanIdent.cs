using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Base.Plans;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiPlanIdent
{
	CloudTrial = 100,
	CloudDeveloper = 101,
	CloudSingle = 102,
	CloudTeam = 103,
	CloudEnterprise = 104,
	ServerTrial = 200,
	ServerTeam5 = 201,
	ServerTeam10 = 202,
	ServerTeam25 = 203,
	ServerTeam50 = 204,
	ServerBusiness = 205,
	ServerEnterprise = 206,
	ServerWorldWide = 207,
	BIServerSingle = 300,
	BIServerTeam = 301,
	BIServerSite = 302,
	BIServerWorldWide = 303
}
