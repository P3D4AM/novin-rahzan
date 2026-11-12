using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Assessment.Configuration.Json;
using Microsoft.SqlServer.Management.Assessment.Tools;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Assessment.Probes;

internal abstract class DbaProbe : SingleToolProbe<ISqlTool>
{
	protected const string NameParameterName = "TargetName";

	[JsonProperty("query")]
	[JsonConverter(typeof(SqlProbeQueryPropertyConverter))]
	public string[] CommandText { get; set; }

	[JsonProperty("useDatabase", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public bool UseDatabase { get; set; }

	protected override async Task<IDataView> GetDataImpl(RunSpace runSpace, ISqlTool sqlTool, IReadOnlyDictionary<string, object> parameters)
	{
		if (runSpace == null)
		{
			throw new ArgumentNullException("runSpace");
		}
		if (UseDatabase && runSpace.Target.Type == SqlObjectType.Database && !string.IsNullOrWhiteSpace(runSpace.Target.Name))
		{
			sqlTool.ChangeDatabase(runSpace.Target.Name);
		}
		using DbCommand command = sqlTool.CreateCommand();
		List<Measurement> resultList = new List<Measurement>();
		string[] commandText = CommandText;
		foreach (string commandText2 in commandText)
		{
			PrepareCommand(command, runSpace.Target, commandText2, parameters);
			List<Measurement> list = resultList;
			list.AddRange(await ReadMeasurementsAsync(command).ConfigureAwait(continueOnCapturedContext: false));
		}
		return new MeasurementCollection(resultList.ToArray());
	}

	protected virtual IEnumerable<Measurement> GetEmptyMeasurements()
	{
		return Enumerable.Empty<Measurement>();
	}

	protected virtual void PrepareCommand(DbCommand command, ISqlObjectLocator locator, string commandText, IReadOnlyDictionary<string, object> parameters)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		command.CommandText = commandText;
		foreach (KeyValuePair<string, object> parameter in parameters)
		{
			SetCommandParameter(command, parameter.Key, parameter.Value);
		}
		if (!string.IsNullOrWhiteSpace(locator.Name))
		{
			SetCommandParameter(command, "TargetName", locator.Name);
		}
	}

	protected abstract IEnumerable<Measurement> GetData(DbDataReader reader);

	protected static void SetCommandParameter(IDbCommand command, string name, object value)
	{
		string parameterName = "@" + name;
		if (command.Parameters.Contains(parameterName))
		{
			((IDbDataParameter)command.Parameters[parameterName]).Value = value ?? DBNull.Value;
			return;
		}
		IDbDataParameter dbDataParameter = command.CreateParameter();
		dbDataParameter.ParameterName = parameterName;
		dbDataParameter.Value = value ?? DBNull.Value;
		command.Parameters.Add(dbDataParameter);
	}

	private async Task<Measurement[]> ReadMeasurementsAsync(DbCommand command)
	{
		using DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(continueOnCapturedContext: false);
		return GetResultSets(reader).SelectMany(GetData).ToArray();
	}

	private static IEnumerable<DbDataReader> GetResultSets(DbDataReader reader)
	{
		do
		{
			if (reader.HasRows && reader.Read())
			{
				yield return reader;
			}
		}
		while (reader.NextResult());
	}
}
