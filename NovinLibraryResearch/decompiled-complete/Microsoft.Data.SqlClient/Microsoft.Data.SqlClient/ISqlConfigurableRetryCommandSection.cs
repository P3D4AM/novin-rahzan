namespace Microsoft.Data.SqlClient;

internal interface ISqlConfigurableRetryCommandSection : ISqlConfigurableRetryConnectionSection
{
	string AuthorizedSqlCondition { get; set; }
}
