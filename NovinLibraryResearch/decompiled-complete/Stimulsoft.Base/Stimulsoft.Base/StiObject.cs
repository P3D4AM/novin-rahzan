using System;
using System.Xml.Linq;
using Stimulsoft.Base.Databases;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Json;

namespace Stimulsoft.Base;

public abstract class StiObject : ICloneable
{
	public virtual object Clone()
	{
		return MemberwiseClone();
	}

	protected virtual void ResetProperties()
	{
		StiPropertyHelper.Reset(this);
	}

	public virtual void LoadFromString(string str)
	{
		ResetProperties();
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

	public virtual byte[] SaveToBytes(bool allowPacking = true)
	{
		return StiPacker.PackToBytes(SaveToString(), allowPacking);
	}

	public virtual XDocument SaveToXml()
	{
		return JsonConvert.DeserializeXNode(SaveToString());
	}
}
