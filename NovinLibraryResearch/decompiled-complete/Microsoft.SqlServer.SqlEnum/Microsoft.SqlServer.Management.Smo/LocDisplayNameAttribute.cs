using System;
using System.ComponentModel;
using Microsoft.SqlServer.Management.Smo.SqlEnum;

namespace Microsoft.SqlServer.Management.Smo;

[AttributeUsage(AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
internal sealed class LocDisplayNameAttribute : DisplayNameAttribute
{
	private string name;

	public override string DisplayName
	{
		get
		{
			string @string = StringSqlEnumerator.Keys.GetString(name);
			if (@string == null)
			{
				@string = name;
			}
			return @string;
		}
	}

	public LocDisplayNameAttribute(string name)
	{
		this.name = name;
	}
}
