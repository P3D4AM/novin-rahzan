namespace Microsoft.Data.SqlClient;

internal readonly ref struct ValueSqlStatisticsScope
{
	private readonly SqlStatistics _statistics;

	public ValueSqlStatisticsScope(SqlStatistics statistics)
	{
		_statistics = SqlStatistics.StartTimer(statistics);
	}

	public void Dispose()
	{
		SqlStatistics.StopTimer(_statistics);
	}
}
