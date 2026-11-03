using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

public static class StiVariableAsParameterHelper
{
	public static bool ParameterExists(List<StiToken> tokens, string name)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		name = name.ToLowerInvariant();
		for (int i = 1; i < tokens.Count; i++)
		{
			StiToken val = tokens[i - 1];
			StiToken val2 = tokens[i];
			if ((int)val.Type == 27 && (int)val2.Type == 40 && val2.Data is string && val2.Data.ToString() == name)
			{
				return true;
			}
		}
		return false;
	}

	public static List<StiVariable> FetchAll(string query, List<StiDataParameter> parameters, StiReport report)
	{
		query = query.ToLowerInvariant();
		List<StiToken> tokens = StiLexer.GetAllTokens(query.ToLowerInvariant());
		return (from v in report.Dictionary.Variables.ToList()
			where v.AllowUseAsSqlParameter && ParameterExists(tokens, v.Name) && !ParameterExists(parameters, v.Name)
			select v).ToList();
	}

	private static bool ParameterExists(List<StiDataParameter> parameters, string name)
	{
		name = name.ToLowerInvariant();
		return parameters.Any((StiDataParameter p) => p.Name.ToLowerInvariant() == name || p.Name.ToLowerInvariant() == "@" + name);
	}
}
