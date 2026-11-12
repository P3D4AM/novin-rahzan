using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo.Wmi;

internal class WmiProperty : ObjectProperty
{
	private string m_PhysicalName;

	private string m_BaseType;

	public string PhysicalName
	{
		get
		{
			return m_PhysicalName;
		}
		set
		{
			m_PhysicalName = value;
		}
	}

	public string BaseType
	{
		get
		{
			return m_BaseType;
		}
		set
		{
			base.ExtendedType = true;
			m_BaseType = value;
		}
	}

	public WmiProperty(string name, string type)
	{
		base.Name = name;
		PhysicalName = name;
		base.Type = type;
		BaseType = type;
		base.Usage = ObjectPropertyUsages.Filter | ObjectPropertyUsages.Request | ObjectPropertyUsages.Reserved1;
		base.ExtendedType = false;
	}
}
