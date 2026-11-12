using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class Password : IMutablePassword, IPassword
{
	private string m_value;

	private bool m_isHashed;

	private bool m_mustChange;

	private bool m_checkExpiration;

	private bool m_checkPolicy;

	public string Value
	{
		get
		{
			return m_value;
		}
		set
		{
			m_value = value;
		}
	}

	public bool IsHashed
	{
		get
		{
			return m_isHashed;
		}
		set
		{
			m_isHashed = value;
		}
	}

	public bool MustChange
	{
		get
		{
			return m_mustChange;
		}
		set
		{
			m_mustChange = value;
		}
	}

	public bool CheckPolicy
	{
		get
		{
			return m_checkPolicy;
		}
		set
		{
			m_checkPolicy = value;
		}
	}

	public bool CheckExpiration
	{
		get
		{
			return m_checkExpiration;
		}
		set
		{
			m_checkExpiration = value;
		}
	}
}
