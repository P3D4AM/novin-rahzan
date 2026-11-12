using System.Data;
using System.Globalization;

namespace Microsoft.SqlServer.Management.Smo;

internal class PostProcessFileProperties : PostProcess
{
	public override object GetColumnData(string name, object data, DataProvider dp)
	{
		DataTable dataTable = null;
		string triggeredString = GetTriggeredString(dp, 0);
		int triggeredInt = GetTriggeredInt32(dp, 1);
		if (dataTable == null)
		{
			dataTable = ExecuteSql.ExecuteWithResults(string.Format(CultureInfo.InvariantCulture, "  CREATE TABLE #tempspace (value BIGINT) \r\nIF (SERVERPROPERTY('EngineEdition') = 11) \r\nINSERT INTO #tempspace VALUES(-1) \r\nELSE IF (OBJECT_ID(N'msdb.sys.sp_getVolumeFreeSpace',  N'P')) is null \r\nINSERT INTO #tempspace VALUES(-1)\r\nELSE IF (SERVERPROPERTY('EngineEdition') = 9)\r\nbegin\r\nINSERT INTO #tempspace\r\nSELECT available_bytes/1024\r\nFROM sys.master_files AS f\r\nCROSS APPLY sys.dm_os_volume_stats(f.database_id, f.file_id)\r\nWHERE f.database_id = DB_ID('master') and f.file_id = 2\r\nend\r\nELSE \r\nINSERT INTO #tempspace EXEC msdb.sys.sp_getVolumeFreeSpace {0},{1} \r\nSELECT TOP 1 value  as [freebytes] from #tempspace   as [freebytes] \r\nDROP TABLE #tempspace", Util.MakeSqlString(triggeredString), triggeredInt), base.ConnectionInfo);
		}
		if (dataTable.Rows.Count > 0)
		{
			return (long)dataTable.Rows[0]["freebytes"];
		}
		return data;
	}
}
