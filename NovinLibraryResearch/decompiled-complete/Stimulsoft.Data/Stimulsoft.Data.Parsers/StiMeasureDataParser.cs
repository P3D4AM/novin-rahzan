using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Expressions.NCalc;
using Stimulsoft.Data.Functions;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Data.Types;

namespace Stimulsoft.Data.Parsers;

public class StiMeasureDataParser : StiDataParser
{
	private IEnumerable<IGrouping<object[], object[]>> grandRows;

	private IEnumerable<object[]> currentRows;

	private Dictionary<string, Expression> queryToExpression = new Dictionary<string, Expression>();

	private Dictionary<string, StiFunctionColumnPair> expressionToPair = new Dictionary<string, StiFunctionColumnPair>();

	public IEnumerable<object[]> Calculate()
	{
		return grandRows.Select((IGrouping<object[], object[]> rows) => base.Meters.Select((IStiMeter meter) => CalculateMeter(meter, rows.Key, rows)).ToArray());
	}

	public object CalculateMeter(IStiMeter meter, object[] keys = null, IEnumerable<object[]> rows = null)
	{
		if (meter is IStiDimensionMeter dimension)
		{
			return CalculateDimension(dimension, keys);
		}
		if (meter is IStiMeasureMeter measure)
		{
			StiFunctionColumnPair measureColumn = GetMeasureColumn(measure);
			if (measureColumn == null)
			{
				return CalculateMeasureExpression(measure, rows);
			}
			return CalculateMeasureFunction(measureColumn.Function, measureColumn.ColumnName, rows);
		}
		throw new NotSupportedException();
	}

	private object CalculateDimension(IStiDimensionMeter dimension, object[] keys)
	{
		int dimensionIndex = GetDimensionIndex(dimension);
		if (dimensionIndex == -1)
		{
			return null;
		}
		object obj = ((keys != null) ? keys[dimensionIndex] : null);
		if (obj is SimpleValue)
		{
			obj = ((SimpleValue)obj).Value;
		}
		else if (obj is DateTimeValue)
		{
			obj = ((DateTimeValue)obj).Value;
		}
		return obj;
	}

	private object CalculateMeasureFunction(string function, string columnName, IEnumerable<object[]> rows)
	{
		int columnIndex = GetDataColumnIndex(columnName);
		if (columnIndex == -1)
		{
			return null;
		}
		IEnumerable<object> values = rows.Select((object[] row) => row[columnIndex]);
		return Funcs.Calculate(function, values);
	}

	private object CalculateMeasureExpression(IStiMeasureMeter measure, IEnumerable<object[]> rows)
	{
		if (string.IsNullOrWhiteSpace(measure.Expression))
		{
			return null;
		}
		Expression expression = GetExpression(measure.Expression);
		currentRows = rows;
		return expression.Evaluate();
	}

	private StiFunctionColumnPair GetMeasureColumn(IStiMeasureMeter measure)
	{
		if (base.Table == null)
		{
			return null;
		}
		if (string.IsNullOrWhiteSpace(measure.Expression))
		{
			return null;
		}
		if (expressionToPair.ContainsKey(measure.Expression))
		{
			return expressionToPair[measure.Expression];
		}
		string expression = measure.Expression.Trim().ToLowerInvariant();
		foreach (string measureFunction in Funcs.GetMeasureFunctions())
		{
			string lowerFunction = Funcs.ToLowerCase(measureFunction);
			string text = base.Table.Columns.Cast<DataColumn>().FirstOrDefault(delegate(DataColumn c)
			{
				string text2 = Funcs.ToLowerCase(c.ColumnName).Trim();
				return lowerFunction + "(" + text2 + ")" == expression || lowerFunction + "([" + text2 + "])" == expression;
			})?.ColumnName;
			if (text != null)
			{
				StiFunctionColumnPair stiFunctionColumnPair = new StiFunctionColumnPair
				{
					Function = measureFunction,
					ColumnName = text
				};
				expressionToPair.Add(measure.Expression, stiFunctionColumnPair);
				return stiFunctionColumnPair;
			}
		}
		return null;
	}

	private Expression GetExpression(string expression)
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
				args.Result = ((!base.IsGrandTotal) ? currentRows?.Select((object[] row) => GetDataRowValue(name, row)) : grandRows?.SelectMany((IGrouping<object[], object[]> rows) => rows.Select((object[] row) => GetDataRowValue(name, row))));
			}
		};
		queryToExpression[expression] = expression2;
		return expression2;
	}

	private object GetDataRowValue(string name, object[] row)
	{
		int dataColumnIndex = GetDataColumnIndex(name);
		if (dataColumnIndex != -1)
		{
			return row[GetDataColumnIndex(name)];
		}
		return null;
	}

	public StiMeasureDataParser(IStiAppDictionary dictionary, IStiMeter meter)
		: this(dictionary, null, new List<IStiMeter> { meter }, null)
	{
	}

	public StiMeasureDataParser(IStiAppDictionary dictionary, DataTable table, List<IStiMeter> meters, IEnumerable<IGrouping<object[], object[]>> grandRows)
		: base(dictionary, table, meters)
	{
		this.grandRows = grandRows;
	}
}
