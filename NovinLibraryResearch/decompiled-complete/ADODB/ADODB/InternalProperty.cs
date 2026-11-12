using System.Reflection;
using System.Runtime.InteropServices;

namespace ADODB;

[ClassInterface(ClassInterfaceType.None)]
[DefaultMember("Value")]
public class InternalProperty : Property
{
	internal InternalProperties m_props;

	internal Property m_prop;

	public int Attributes
	{
		get
		{
			return m_prop.Attributes;
		}
		set
		{
			m_prop.Attributes = value;
		}
	}

	public string Name => m_prop.Name;

	public DataTypeEnum Type => m_prop.Type;

	public object Value
	{
		get
		{
			return m_prop.Value;
		}
		set
		{
			m_prop.Value = value;
		}
	}

	internal InternalProperty(Property field)
	{
		m_prop = field;
	}
}
