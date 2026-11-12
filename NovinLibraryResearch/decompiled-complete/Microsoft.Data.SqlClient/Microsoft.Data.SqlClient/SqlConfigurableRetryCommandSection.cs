using System.Configuration;

namespace Microsoft.Data.SqlClient;

internal sealed class SqlConfigurableRetryCommandSection : SqlConfigurableRetryConnectionSection, ISqlConfigurableRetryCommandSection, ISqlConfigurableRetryConnectionSection
{
	public new const string Name = "SqlConfigurableRetryLogicCommand";

	[ConfigurationProperty("authorizedSqlCondition", IsRequired = false)]
	public string AuthorizedSqlCondition
	{
		get
		{
			return ((ConfigurationElement)this)["authorizedSqlCondition"] as string;
		}
		set
		{
			((ConfigurationElement)this)["authorizedSqlCondition"] = value;
		}
	}
}
