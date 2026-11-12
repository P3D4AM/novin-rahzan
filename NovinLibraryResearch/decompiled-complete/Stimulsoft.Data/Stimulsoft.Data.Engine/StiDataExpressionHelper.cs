using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Data.Functions;
using Stimulsoft.Data.Helpers;

namespace Stimulsoft.Data.Engine;

public class StiDataExpressionHelper
{
	public static IStiAppDataColumn GetDataColumnFromExpression(IStiQueryObject query, string expression)
	{
		if (string.IsNullOrWhiteSpace(expression))
		{
			return null;
		}
		expression = StiExpressionHelper.RemoveFunction(expression);
		List<string> arguments = StiExpressionHelper.GetArguments(expression);
		if (arguments == null || arguments.Count != 1)
		{
			return null;
		}
		string arg = arguments.FirstOrDefault();
		if (string.IsNullOrWhiteSpace(arg))
		{
			return null;
		}
		arg = Funcs.ToDataName(arg);
		IEnumerable<IStiAppDataSource> enumerable = query?.GetDataSources(new string[1] { arg });
		if (enumerable == null || !enumerable.Any())
		{
			return null;
		}
		IStiAppDataSource dataSource = enumerable.First();
		return dataSource.FetchColumns().FirstOrDefault((IStiAppDataColumn c) => Funcs.IsDataEqual(dataSource, c.GetName(), arg));
	}

	public static bool IsDateDataColumnInExpression(IStiQueryObject query, string expression)
	{
		IStiAppDataColumn dataColumnFromExpression = GetDataColumnFromExpression(query, expression);
		if (dataColumnFromExpression?.GetDataType() != null)
		{
			return dataColumnFromExpression.GetDataType().IsDateType();
		}
		return false;
	}

	public static bool IsNumericDataColumnInExpression(IStiQueryObject query, string expression)
	{
		IStiAppDataColumn dataColumnFromExpression = GetDataColumnFromExpression(query, expression);
		if (dataColumnFromExpression?.GetDataType() != null)
		{
			return dataColumnFromExpression.GetDataType().IsNumericType();
		}
		return false;
	}
}
