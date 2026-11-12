using System;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public class ComputedColumnInfo
{
	private readonly bool m_isPersisted;

	private readonly string m_text;

	public bool IsPersisted => m_isPersisted;

	public string Text => m_text;

	public ComputedColumnInfo(string text, bool isPersisted)
	{
		if (text == null)
		{
			throw new ArgumentNullException("text");
		}
		m_isPersisted = isPersisted;
		m_text = text;
	}
}
