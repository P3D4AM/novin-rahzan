using System.Collections.Generic;

namespace Stimulsoft.Base.Map;

public interface IStiMapKeyHelper
{
	List<string> GetMapIdents(string key);

	string GetIsoAlpha2FromName(string country, string mapId, string lang, IStiReport report = null);

	string GetIsoAlpha3FromName(string country, string mapId, string lang, IStiReport report = null);

	string GetNameFromIsoAlpha2(string alpha3, string mapId, string lang, IStiReport report = null);

	string GetNameFromIsoAlpha3(string alpha3, string mapId, string lang, IStiReport report = null);

	string NormalizeName(string name, string mapId, string lang, IStiReport report = null);
}
