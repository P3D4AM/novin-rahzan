using System.Xml.Linq;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Json;

namespace Stimulsoft.Base;

public abstract class StiObjectSchema
{
	public string Name { get; set; }

	public virtual void LoadFromString(string str)
	{
		JsonConvert.PopulateObject(str, this);
	}

	public virtual void LoadFromBytes(byte[] bytes)
	{
		string text = StiPacker.UnpackToString(bytes);
		if (text != null)
		{
			LoadFromString(text);
		}
	}

	public virtual string SaveToString()
	{
		return JsonConvert.SerializeObject(this, Formatting.Indented, StiJsonHelper.DefaultSerializerSettings);
	}

	public virtual byte[] SaveToBytes()
	{
		return StiPacker.PackToBytes(SaveToString());
	}

	public virtual XDocument SaveToXml()
	{
		return JsonConvert.DeserializeXNode(SaveToString());
	}
}
