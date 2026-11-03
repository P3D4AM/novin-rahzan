using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Dictionary.Design;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiResourceFileConverter))]
[StiSerializable]
public class StiResource : IStiName, IStiAlias, IStiAppCell, ICloneable, IStiInherited, IStiPropertyGridObject, IStiJsonReportObject
{
	public enum Order
	{
		Name = 100,
		Alias = 200,
		Category = 300,
		AvailableInTheViewer = 400
	}

	private string name;

	[Browsable(false)]
	public StiComponentId ComponentId => StiComponentId.StiResource;

	[Browsable(false)]
	public string PropName => Name;

	[Browsable(false)]
	[DefaultValue(false)]
	public bool Inherited { get; set; }

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[ParenthesizePropertyName(true)]
	[StiOrder(100)]
	[Description("Gets or sets a name of the resource.")]
	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			if (!(name == value))
			{
				if (name == Alias)
				{
					Alias = value;
				}
				name = value;
			}
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[ParenthesizePropertyName(true)]
	[StiOrder(200)]
	[Description("Gets or sets an alias of the resource.")]
	public string Alias { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(false)]
	[StiOrder(400)]
	[Description("Gets or sets value which makes the resource available for download in the viewer.")]
	public bool AvailableInTheViewer { get; set; }

	[Browsable(false)]
	public byte[] Content { get; set; }

	[DefaultValue(null)]
	[StiSerializable]
	[Browsable(false)]
	public string Key { get; set; }

	[DefaultValue(StiResourceType.Image)]
	[StiSerializable]
	[Browsable(false)]
	public StiResourceType Type { get; set; }

	public object Clone()
	{
		StiResource stiResource = MemberwiseClone() as StiResource;
		stiResource.Content = Content?.Clone() as byte[];
		return stiResource;
	}

	string IStiAppCell.GetKey()
	{
		Key = StiKeyHelper.GetOrGeneratedKey(Key);
		return Key;
	}

	void IStiAppCell.SetKey(string key)
	{
		Key = key;
	}

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Name", Name);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Alias", Alias);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Key", Key);
		JsonExtensions.AddPropertyBool(val, "Inherited", Inherited, false, false);
		JsonExtensions.AddPropertyEnum(val, "Type", (Enum)Type, (Enum)StiResourceType.Image);
		JsonExtensions.AddPropertyBool(val, "AvailableInTheViewer", AvailableInTheViewer, false, false);
		val.Add("Image", JToken.op_Implicit(StiPacker.PackAndEncryptToString(Content)));
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Name":
				Name = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Alias":
				Alias = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Key":
				Key = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Inherited":
				Inherited = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Type":
				Type = StiJsonReportObjectHelper.DeserializeEnum<StiResourceType>(item);
				break;
			case "AvailableInTheViewer":
				AvailableInTheViewer = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Image":
				Content = StiPacker.UnpackAndDecrypt(StiJsonReportObjectHelper.DeserializeString(item));
				break;
			}
		}
	}

	public StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[3]
		{
			propertiesHelper.Name(),
			propertiesHelper.Alias(),
			propertiesHelper.AvailableinTheViewer()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Data, value);
		return stiPropertyCollection;
	}

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	public Image GetResourceAsImage()
	{
		if (Content == null || Type != 0)
		{
			return null;
		}
		try
		{
			MemoryStream memoryStream = new MemoryStream(Content);
			return Image.FromStream((Stream)memoryStream);
		}
		catch
		{
		}
		return null;
	}

	public override string ToString()
	{
		return ToString(onlyAlias: false);
	}

	public string ToString(bool onlyAlias)
	{
		if (onlyAlias && !string.IsNullOrWhiteSpace(Alias))
		{
			return Alias;
		}
		if (Name == Alias || string.IsNullOrWhiteSpace(Alias))
		{
			return Name;
		}
		return Name + " [" + Alias + "]";
	}

	public string GetContentType()
	{
		switch (Type)
		{
		case StiResourceType.Report:
		case StiResourceType.ReportSnapshot:
			return "application/octet-stream";
		case StiResourceType.Pdf:
			return "application/pdf";
		case StiResourceType.Txt:
			return "text/plain";
		case StiResourceType.Json:
			return "text/plain";
		case StiResourceType.Rtf:
			return "application/rtf";
		case StiResourceType.Word:
			return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
		case StiResourceType.Excel:
			return "application/vnd.ms-excel";
		case StiResourceType.Csv:
			return "text/csv";
		case StiResourceType.Dbf:
			return "application/dbf";
		case StiResourceType.Image:
			if (StiImageHelper.IsPng(Content))
			{
				return "image/png";
			}
			if (StiImageHelper.IsMetafile(Content))
			{
				return "image/x-wmf";
			}
			if (StiImageHelper.IsBmp(Content))
			{
				return "image/bmp";
			}
			if (StiImageHelper.IsJpeg(Content))
			{
				return "image/jpeg";
			}
			if (StiImageHelper.IsGif(Content))
			{
				return "image/gif";
			}
			if (StiSvgHelper.IsSvg(Content))
			{
				return "image/svg+xml";
			}
			if (StiImageHelper.IsTiff(Content))
			{
				return "image/tiff";
			}
			if (StiImageHelper.IsEmf(Content))
			{
				return "image/x-emf";
			}
			return "image/png";
		default:
			return "text/plain";
		}
	}

	public string GetFileExt()
	{
		switch (Type)
		{
		case StiResourceType.ReportSnapshot:
			return ".mdc";
		case StiResourceType.Report:
			return ".mrt";
		case StiResourceType.Pdf:
			return ".pdf";
		case StiResourceType.Txt:
			return ".txt";
		case StiResourceType.Rtf:
			return ".rtf";
		case StiResourceType.Word:
			return ".docx";
		case StiResourceType.Excel:
			return ".xlsx";
		case StiResourceType.Csv:
			return ".csv";
		case StiResourceType.Dbf:
			return ".dbf";
		case StiResourceType.Json:
			return ".json";
		case StiResourceType.Image:
			if (StiImageHelper.IsPng(Content))
			{
				return ".png";
			}
			if (StiImageHelper.IsMetafile(Content))
			{
				return ".wmf";
			}
			if (StiImageHelper.IsBmp(Content))
			{
				return ".bmp";
			}
			if (StiImageHelper.IsJpeg(Content))
			{
				return ".jpg";
			}
			if (StiImageHelper.IsGif(Content))
			{
				return ".gif";
			}
			if (StiSvgHelper.IsSvg(Content))
			{
				return ".svg";
			}
			if (StiImageHelper.IsTiff(Content))
			{
				return ".tiff";
			}
			if (StiImageHelper.IsEmf(Content))
			{
				return ".emf";
			}
			return ".png";
		default:
			return string.Empty;
		}
	}

	public StiFileDatabase CreateFileDatabase()
	{
		return Type switch
		{
			StiResourceType.Csv => new StiCsvDatabase
			{
				Name = Name,
				Alias = Name,
				PathData = "resource://" + Name
			}, 
			StiResourceType.Dbf => new StiDBaseDatabase
			{
				Name = Name,
				Alias = Name,
				PathData = "resource://" + Name
			}, 
			StiResourceType.Excel => new StiExcelDatabase
			{
				Name = Name,
				Alias = Name,
				PathData = "resource://" + Name
			}, 
			StiResourceType.Json => new StiJsonDatabase
			{
				Name = Name,
				Alias = Name,
				PathData = "resource://" + Name
			}, 
			StiResourceType.Gis => new StiGisDatabase
			{
				Name = Name,
				Alias = Name,
				PathData = "resource://" + Name
			}, 
			StiResourceType.Xml => new StiXmlDatabase
			{
				Name = Name,
				Alias = Name,
				PathData = "resource://" + Name
			}, 
			StiResourceType.Xsd => new StiXmlDatabase
			{
				Name = Name,
				Alias = Name,
				PathSchema = "resource://" + Name
			}, 
			_ => null, 
		};
	}

	public StiResource()
		: this(string.Empty, StiResourceType.Image, null)
	{
	}

	public StiResource(string name, StiResourceType type, byte[] content)
		: this(name, name, type, content, availableInTheViewer: false)
	{
	}

	public StiResource(string name, StiResourceType type, byte[] content, bool availableInTheViewer)
		: this(name, name, type, content, availableInTheViewer)
	{
	}

	public StiResource(string name, string alias, StiResourceType type, byte[] content)
		: this(name, alias, type, content, availableInTheViewer: false)
	{
	}

	public StiResource(string name, string alias, StiResourceType type, byte[] content, bool availableInTheViewer)
		: this(name, alias, inherited: false, type, content, availableInTheViewer)
	{
	}

	public StiResource(string name, string alias, bool inherited, StiResourceType type, byte[] content, bool availableInTheViewer)
	{
		Name = name;
		Alias = alias;
		Inherited = inherited;
		Type = type;
		Content = content;
		AvailableInTheViewer = availableInTheViewer;
	}
}
