using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Expressions.NCalc;
using Stimulsoft.Data.Functions;
using Stimulsoft.Data.Helpers;

namespace Stimulsoft.Data.Parsers;

public class StiDimensionDataParser : StiDataParser
{
	private object[] currentRow;

	private Dictionary<string, Expression> queryToExpression = new Dictionary<string, Expression>();

	private Dictionary<string, string> expressionToColumn = new Dictionary<string, string>();

	protected IEnumerable<IStiDimensionMeter> Dimensions { get; }

	public object[] Calculate(object[] row, List<IStiMeter> meters)
	{
		currentRow = row;
		return NormalizeDates(Dimensions.Select(CalculateDimension).ToArray(), meters);
	}

	private static object[] NormalizeDates(object[] values, List<IStiMeter> meters)
	{
		if (meters == null)
		{
			return values;
		}
		int meterIndex = 0;
		return values.Select((object v) => NormalizeDate(v, GetMeter(meters, meterIndex++))).ToArray();
	}

	private static IStiMeter GetMeter(List<IStiMeter> meters, int meterIndex)
	{
		if (meterIndex >= meters.Count)
		{
			return null;
		}
		return meters[meterIndex];
	}

	private static object NormalizeDate(object value, IStiMeter meter)
	{
		if (meter is IStiDataFormat stiDataFormat && stiDataFormat.GetDataFormat() == StiDataFormatKind.Time)
		{
			return value;
		}
		if (!(value is DateTime))
		{
			return value;
		}
		return StiDateTimeCorrector.Correct((DateTime)value);
	}

	private object CalculateDimension(IStiDimensionMeter dimension)
	{
		if (string.IsNullOrWhiteSpace(dimension.Expression))
		{
			return null;
		}
		string dimensionGroupColumn = GetDimensionGroupColumn(dimension);
		if (dimensionGroupColumn == null)
		{
			return CalculateDimensionExpression(dimension);
		}
		return CalculateDimensionGroup(dimensionGroupColumn);
	}

	private string GetDimensionGroupColumn(IStiDimensionMeter dimension)
	{
		if (base.Table == null)
		{
			return null;
		}
		if (string.IsNullOrWhiteSpace(dimension.Expression))
		{
			return null;
		}
		if (expressionToColumn.ContainsKey(dimension.Expression))
		{
			return expressionToColumn[dimension.Expression];
		}
		string expression = Funcs.ToDataName(dimension.Expression);
		string text = base.Table.Columns.Cast<DataColumn>().FirstOrDefault((DataColumn c) => Funcs.ToDataName(c.ColumnName) == expression)?.ColumnName;
		expressionToColumn.Add(dimension.Expression, text);
		return text;
	}

	public object CalculateDimensionExpression(IStiDimensionMeter dimension)
	{
		return CalculateDimensionExpression(dimension.Expression);
	}

	public object CalculateDimensionExpression(string expression, bool throwExceptionForColumnName = false)
	{
		return GetExpression(expression, throwExceptionForColumnName)?.Evaluate();
	}

	private object CalculateDimensionGroup(string columnName)
	{
		int dataColumnIndex = GetDataColumnIndex(columnName);
		object[] array = currentRow;
		object value = ((array != null) ? array[dataColumnIndex] : null);
		return NormalizeEnum(value, dataColumnIndex);
	}

	private object NormalizeEnum(object value, int columnIndex)
	{
		if (value is int)
		{
			Type dataType = base.Table.Columns[columnIndex].DataType;
			if (dataType.IsEnum)
			{
				value = Enum.ToObject(dataType, value);
			}
		}
		return value;
	}

	private Expression GetExpression(string expression, bool throwExceptionForColumnName = false)
	{
		if (queryToExpression.ContainsKey(expression))
		{
			return queryToExpression[expression];
		}
		Expression expression2 = StiExpressionHelper.NewExpression(expression);
		expression2.EvaluateFunction += delegate(string name, FunctionArgs args)
		{
			args.Result = RunFunction(name, args);
		};
		expression2.EvaluateParameter += delegate(string name, ParameterArgs args)
		{
			if (IsSystemVariable(name))
			{
				args.Result = GetSystemVariableValue(name);
			}
			else if (IsVariable(name))
			{
				args.Result = GetVariableValue(name);
			}
			else
			{
				int dataColumnIndex = GetDataColumnIndex(name);
				if (dataColumnIndex >= 0 && dataColumnIndex < currentRow.Length)
				{
					args.Result = currentRow[dataColumnIndex];
				}
				else
				{
					if (throwExceptionForColumnName)
					{
						throw new Exception("The '" + name + "' ident is not found.");
					}
					args.Result = null;
				}
			}
		};
		queryToExpression[expression] = expression2;
		return expression2;
	}

	public StiDimensionDataParser(IStiAppDictionary dictionary, IStiMeter meter)
		: this(dictionary, null, new List<IStiMeter> { meter })
	{
	}

	public StiDimensionDataParser(IStiAppDictionary dictionary, DataTable table, List<IStiMeter> meters)
		: base(dictionary, table, meters)
	{
		Dimensions = base.Meters.Where((IStiMeter m) => m is IStiDimensionMeter).Cast<IStiDimensionMeter>();
	}
}
