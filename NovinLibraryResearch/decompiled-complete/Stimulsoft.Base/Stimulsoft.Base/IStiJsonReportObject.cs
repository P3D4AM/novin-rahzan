using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base;

public interface IStiJsonReportObject
{
	JObject SaveToJsonObject(StiJsonSaveMode mode);

	void LoadFromJsonObject(JObject jObject);
}
