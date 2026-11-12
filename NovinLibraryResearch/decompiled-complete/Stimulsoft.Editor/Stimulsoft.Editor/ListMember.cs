using System;
using System.Xml.Serialization;

namespace Stimulsoft.Editor;

public class ListMember : IListMember, IComparable
{
	private string qualifier = string.Empty;

	private string paramText = string.Empty;

	private string name = string.Empty;

	private string displayText = string.Empty;

	private string description = string.Empty;

	private string dataType = string.Empty;

	private int overloads;

	private bool ownDescription;

	private string ownDescriptionText = string.Empty;

	private int imageIndex = -1;

	private int currentParamIndex = -1;

	private IParameterMember[] parameters;

	private object customData;

	private IListMembers owner;

	[XmlIgnore]
	public IListMembers Owner
	{
		get
		{
			return owner;
		}
		set
		{
			owner = value;
		}
	}

	[XmlElement("Name")]
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

	[XmlElement("DisplayText")]
	public virtual string DisplayText
	{
		get
		{
			string text = ((displayText != string.Empty) ? displayText : Name);
			if (currentParamIndex >= 0 || owner == null || !((CodeCompletionProvider)owner).ShouldFormatName())
			{
				return text;
			}
			return SyntaxConsts.DefaultBoldTag + text + SyntaxConsts.DefaultBoldEndTag;
		}
		set
		{
			displayText = value;
		}
	}

	[XmlElement("DataType")]
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

	[XmlElement("Description")]
	public virtual string Description
	{
		get
		{
			if (parameters != null && currentParamIndex >= 0 && currentParamIndex < parameters.Length)
			{
				string text = parameters[currentParamIndex].Description;
				if (text != string.Empty)
				{
					return text;
				}
			}
			if (ownDescription)
			{
				if (description != null && description != string.Empty)
				{
					return (GetOwnDescription() + ((owner != null && owner.UseHtmlFormatting) ? SyntaxConsts.DefaultBrTag : "\r\n") + description).Trim();
				}
				return GetOwnDescription();
			}
			return description;
		}
		set
		{
			description = value;
		}
	}

	[XmlElement("ImageIndex")]
	public int ImageIndex
	{
		get
		{
			return imageIndex;
		}
		set
		{
			imageIndex = value;
		}
	}

	[XmlElement("Qualifier")]
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

	[XmlIgnore]
	public object CustomData
	{
		get
		{
			return customData;
		}
		set
		{
			customData = value;
		}
	}

	[XmlElement("ParamText")]
	public virtual string ParamText
	{
		get
		{
			return GetParamText(useFormatting: true);
		}
		set
		{
			paramText = value;
		}
	}

	[XmlIgnore]
	public virtual IParameterMember[] Parameters
	{
		get
		{
			return parameters;
		}
		set
		{
			parameters = value;
		}
	}

	[XmlArray]
	[XmlArrayItem("Parameter")]
	public ParameterMember[] Params
	{
		get
		{
			if (parameters != null)
			{
				ParameterMember[] array = new ParameterMember[parameters.Length];
				parameters.CopyTo(array, 0);
				return array;
			}
			return null;
		}
		set
		{
			parameters = value;
		}
	}

	[XmlElement("Overloads")]
	public int Overloads
	{
		get
		{
			return overloads;
		}
		set
		{
			overloads = value;
		}
	}

	public bool OwnDescription
	{
		get
		{
			return ownDescription;
		}
		set
		{
			ownDescription = value;
		}
	}

	public string OwnDescriptionText
	{
		get
		{
			return ownDescriptionText;
		}
		set
		{
			ownDescriptionText = value;
		}
	}

	[XmlElement("CurrentParamIndex")]
	public int CurrentParamIndex
	{
		get
		{
			return currentParamIndex;
		}
		set
		{
			currentParamIndex = value;
		}
	}

	public ListMember()
	{
	}

	public ListMember(IListMembers owner)
	{
		this.owner = owner;
	}

	public virtual void UpdateParamText()
	{
	}

	public virtual string GetParamText(bool useFormatting)
	{
		return paramText;
	}

	public virtual string GetOwnDescription()
	{
		return ownDescriptionText;
	}

	public virtual int CompareTo(object value)
	{
		if (value is IListMember)
		{
			return string.Compare(name, ((IListMember)value).Name, ignoreCase: true);
		}
		throw new ArgumentException();
	}
}
