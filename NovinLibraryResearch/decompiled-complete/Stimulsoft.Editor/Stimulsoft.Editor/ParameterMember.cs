using System.Xml.Serialization;

namespace Stimulsoft.Editor;

public class ParameterMember : IParameterMember
{
	private string qualifier = string.Empty;

	private string name = string.Empty;

	private string description = string.Empty;

	private string dataType = string.Empty;

	private string text = string.Empty;

	private ParameterModifer modifiers;

	[XmlElement("ParamName")]
	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			name = value;
		}
	}

	[XmlElement("ParamDataType")]
	public string DataType
	{
		get
		{
			return dataType;
		}
		set
		{
			dataType = value;
		}
	}

	[XmlElement("ParamDescription")]
	public string Description
	{
		get
		{
			return description;
		}
		set
		{
			description = value;
		}
	}

	[XmlElement("ParamQualifier")]
	public string Qualifier
	{
		get
		{
			return qualifier;
		}
		set
		{
			qualifier = value;
		}
	}

	[XmlElement("ParamText")]
	public virtual string Text
	{
		get
		{
			if (!(text != string.Empty))
			{
				return ((Qualifier + " " + DataType).Trim() + " " + Name).Trim();
			}
			return text;
		}
		set
		{
			text = value;
		}
	}

	public ParameterModifer Modifiers
	{
		get
		{
			return modifiers;
		}
		set
		{
			modifiers = value;
		}
	}
}
