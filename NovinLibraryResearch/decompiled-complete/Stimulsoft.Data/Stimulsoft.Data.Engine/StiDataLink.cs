using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Data.Design;

namespace Stimulsoft.Data.Engine;

[TypeConverter(typeof(StiDataLinkConverter))]
[StiSerializable]
public class StiDataLink : IStiJsonReportObject
{
	public string ParentTable { get; set; }

	public string ChildTable { get; set; }

	public string ParentColumn { get; set; }

	public string ChildColumn { get; set; }

	public string ParentKey
	{
		get
		{
			if (string.IsNullOrEmpty(ParentTable) || string.IsNullOrEmpty(ParentColumn))
			{
				return null;
			}
			return ParentTable + "." + ParentColumn;
		}
	}

	public string ChildKey
	{
		get
		{
			if (string.IsNullOrEmpty(ChildTable) || string.IsNullOrEmpty(ChildColumn))
			{
				return null;
			}
			return ChildTable + "." + ChildColumn;
		}
	}

	public string Key { get; set; }

	public bool Active { get; set; }

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = new JObject();
		jObject.AddPropertyStringNullOrEmpty("Key", Key);
		return jObject;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (name == "Key")
			{
				Key = item.DeserializeString();
			}
		}
	}

	public static StiDataLink LoadFromJson(JObject json)
	{
		StiDataLink stiDataLink = new StiDataLink();
		stiDataLink.LoadFromJsonObject(json);
		return stiDataLink;
	}

	public override string ToString()
	{
		return NullStr(ParentTable) + "->" + NullStr(ChildTable) + " : " + NullStr(ParentColumn) + "->" + NullStr(ChildColumn);
	}

	private string NullStr(string str)
	{
		return str ?? "";
	}

	public StiDataLink()
	{
	}

	public StiDataLink(string key)
	{
		Key = key;
	}

	public StiDataLink(string parentTable, string childTable, string parentColumn, string childColumn, bool active)
	{
		ParentTable = parentTable;
		ChildTable = childTable;
		ParentColumn = parentColumn;
		ChildColumn = childColumn;
		Active = active;
	}

	public StiDataLink(string parentTable, string childTable, string parentColumn, string childColumn, bool active, string key)
		: this(parentTable, childTable, parentColumn, childColumn, active)
	{
		Key = key;
	}
}
