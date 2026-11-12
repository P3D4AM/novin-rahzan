using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Assessment.Properties;
using Microsoft.SqlServer.Management.Assessment.Tools;

namespace Microsoft.SqlServer.Management.Assessment.Probes;

internal abstract class SingleToolProbe<TTool> : Probe where TTool : class, ITool
{
	private static readonly Type[] ToolTypesStatic = new Type[1] { typeof(TTool) };

	public override IEnumerable<Type> ToolTypes => ToolTypesStatic;

	protected override async Task<IDataView> GetDataImpl(RunSpace runSpace, IReadOnlyDictionary<string, object> parameters)
	{
		TTool tool = runSpace.GetTool<TTool>();
		try
		{
			return await GetDataImpl(runSpace, tool, parameters).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (SqlAssessmentProbeParameterException ex)
		{
			throw new SqlAssessmentProbeParameterException(Resources.ErrorEvalWrongProbeParameter + ": " + ex.Message, this, ex.ParameterName);
		}
	}

	protected abstract Task<IDataView> GetDataImpl(RunSpace runSpace, TTool tool, IReadOnlyDictionary<string, object> parameters);
}
