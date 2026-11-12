using Stimulsoft.Base.Databases;
using Stimulsoft.Base.Json;

namespace Stimulsoft.Base;

public abstract class StiKeyObject : StiObject
{
	[StiTableField]
	public string Key { get; set; }

	[StiIgnoreTableField]
	[JsonIgnore]
	protected internal bool? IsStored { get; set; }

	public StiKeyObject()
	{
		Key = StiKeyHelper.GenerateKey();
	}
}
