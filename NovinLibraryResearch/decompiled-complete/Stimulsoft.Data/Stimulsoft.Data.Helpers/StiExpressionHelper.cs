using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Data.Expressions.NCalc;
using Stimulsoft.Data.Expressions.NCalc.Domain;
using Stimulsoft.Data.Functions;

namespace Stimulsoft.Data.Helpers;

public class StiExpressionHelper
{
	private class ArgumentExtractionVisitor : LogicalExpressionVisitor
	{
		public HashSet<string> Parameters = new HashSet<string>();

		public override void Visit(Identifier function)
		{
			Parameters.Add(function.Name);
		}

		public override void Visit(UnaryExpression expression)
		{
		}

		public override void Visit(BinaryExpression expression)
		{
			expression.LeftExpression.Accept(this);
			expression.RightExpression.Accept(this);
		}

		public override void Visit(TernaryExpression expression)
		{
			expression.LeftExpression.Accept(this);
			expression.RightExpression.Accept(this);
			expression.MiddleExpression.Accept(this);
		}

		public override void Visit(Function function)
		{
			LogicalExpression[] expressions = function.Expressions;
			foreach (LogicalExpression logicalExpression in expressions)
			{
				logicalExpression.Accept(this);
			}
		}

		public override void Visit(LogicalExpression expression)
		{
			expression.Accept(this);
		}

		public override void Visit(ValueExpression expression)
		{
		}
	}

	private static Dictionary<string, List<string>> expressionToArguments = new Dictionary<string, List<string>>();

	public static Expression NewExpression(string expression)
	{
		return new Expression(PrepareExpression(expression), EvaluateOptions.IgnoreCase);
	}

	public static string PrepareExpression(string expression)
	{
		expression = (string.IsNullOrWhiteSpace(expression) ? expression : expression.Replace("\"", "'"));
		return EscapeExpression(expression);
	}

	public static string EscapeExpression(string expression)
	{
		if (string.IsNullOrWhiteSpace(expression))
		{
			return expression;
		}
		try
		{
			List<StiToken> list = new List<StiToken>();
			StiLexer stiLexer = new StiLexer(expression);
			while (true)
			{
				StiToken token = stiLexer.GetToken();
				if (token == null || token.Type == StiTokenType.EOF)
				{
					break;
				}
				list.Add(token);
			}
			StringBuilder stringBuilder = new StringBuilder(expression);
			int num = 0;
			for (int i = 0; i < list.Count - 2; i++)
			{
				if (list[i].Type == StiTokenType.Ident && list[i + 1].Type == StiTokenType.Dot && list[i + 2].Type == StiTokenType.Ident && (i == 0 || i + 3 >= list.Count || (list[i - 1].Type != StiTokenType.LBracket && list[i + 3].Type != StiTokenType.RBracket)))
				{
					int index = list[i].Index;
					int num2 = list[i + 2].Index + list[i + 2].Length;
					stringBuilder.Insert(index + num, "[");
					num++;
					stringBuilder.Insert(num2 + num, "]");
					num++;
					i++;
				}
			}
			return stringBuilder.ToString();
		}
		catch
		{
			return expression;
		}
	}

	public static string ReplaceFunction(string expression, string newFunction)
	{
		string function = GetFunction(expression);
		if (function != null)
		{
			expression = expression.Substring(function.Length);
		}
		else
		{
			expression = expression.Trim();
			if (!expression.StartsWith("("))
			{
				expression = "(" + expression;
			}
			if (!expression.EndsWith(")"))
			{
				expression += ")";
			}
		}
		return newFunction + expression;
	}

	public static string RemoveFunction(string expression)
	{
		if (expression == null)
		{
			return expression;
		}
		string function = GetFunction(expression);
		if (function != null)
		{
			expression = expression.Substring(function.Length);
		}
		expression = expression.Trim();
		if (expression.StartsWith("("))
		{
			expression = expression.Substring(1);
		}
		if (expression.EndsWith(")"))
		{
			expression = expression.Substring(0, expression.Length - 1);
		}
		return expression;
	}

	public static bool IsPercentOfGrandTotal(string expression)
	{
		string function = GetFunction(expression);
		if (string.IsNullOrWhiteSpace(function))
		{
			return false;
		}
		return function.ToLowerInvariant().Trim() == "percentofgrandtotal";
	}

	public static bool IsAggregationFunctionPresent(string expression)
	{
		string function = GetFunction(expression);
		if (string.IsNullOrWhiteSpace(function))
		{
			return false;
		}
		return Funcs.IsAggregationFunction(function);
	}

	public static bool IsFunctionPresent(string expression)
	{
		string function = GetFunction(expression);
		return !string.IsNullOrWhiteSpace(function);
	}

	public static string GetFunction(string expression)
	{
		if (string.IsNullOrWhiteSpace(expression))
		{
			return null;
		}
		try
		{
			LogicalExpression logicalExpression = Expression.Compile(PrepareExpression(expression), nocache: true);
			string text = ((!(logicalExpression is Function function)) ? null : function.Identifier?.Name);
			return (text != null && expression.Trim().StartsWith(text)) ? text : null;
		}
		catch
		{
			return null;
		}
	}

	public static List<string> GetArguments(string expression)
	{
		if (string.IsNullOrWhiteSpace(expression))
		{
			return new List<string>();
		}
		try
		{
			lock (expressionToArguments)
			{
				if (expressionToArguments.ContainsKey(expression))
				{
					return expressionToArguments[expression];
				}
			}
			LogicalExpression logicalExpression = Compile(expression);
			ArgumentExtractionVisitor argumentExtractionVisitor = new ArgumentExtractionVisitor();
			logicalExpression.Accept(argumentExtractionVisitor);
			List<string> list = argumentExtractionVisitor.Parameters.ToList();
			lock (expressionToArguments)
			{
				expressionToArguments[expression] = list;
			}
			return list;
		}
		catch
		{
		}
		return new List<string>();
	}

	public static LogicalExpression Compile(string expression)
	{
		return Expression.Compile(PrepareExpression(expression), nocache: true);
	}

	public static string GetFirstArgumentFromExpression(string expression)
	{
		expression = RemoveFunction(expression);
		List<string> arguments = GetArguments(expression);
		if (arguments == null)
		{
			return null;
		}
		string text = arguments.FirstOrDefault();
		if (string.IsNullOrWhiteSpace(text))
		{
			return null;
		}
		return text;
	}

	public static string ParseReportExpression(IStiReport report, string text, bool withBraces, bool allowReturnNull = false)
	{
		if (report != null && !string.IsNullOrEmpty(text))
		{
			if (!withBraces && !text.Contains("{") && !text.Contains("}"))
			{
				text = "{" + text + "}";
			}
			return report.FetchPages()?.FirstOrDefault()?.ParseExpression(text, allowReturnNull);
		}
		return text;
	}

	public static List<string> FetchBlocksFromExpression(string inputExpression)
	{
		if (string.IsNullOrWhiteSpace(inputExpression))
		{
			return null;
		}
		List<string> list = new List<string>();
		int i = 0;
		while (i < inputExpression.Length)
		{
			for (; i < inputExpression.Length && inputExpression[i] != '{'; i++)
			{
			}
			if (i >= inputExpression.Length || inputExpression[i] != '{')
			{
				continue;
			}
			i++;
			int num = i;
			while (i < inputExpression.Length)
			{
				if (inputExpression[i] == '"')
				{
					i++;
					int j;
					for (j = i; j < inputExpression.Length && inputExpression[j] != '"'; j++)
					{
						if (inputExpression[j] == '\\')
						{
							j++;
						}
					}
					i = j + 1;
					continue;
				}
				if (inputExpression[i] == '}')
				{
					string text = inputExpression.Substring(num, i - num);
					if (!string.IsNullOrWhiteSpace(text))
					{
						list.Add(text.Trim());
					}
					i++;
					break;
				}
				i++;
			}
		}
		return list;
	}

	public static string ReplaceExpressionBlocksByValues(string inputExpression, List<string> values)
	{
		if (string.IsNullOrWhiteSpace(inputExpression))
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		int i = 0;
		while (i < inputExpression.Length)
		{
			int num2 = i;
			for (; i < inputExpression.Length && inputExpression[i] != '{'; i++)
			{
			}
			if (i != num2)
			{
				stringBuilder.Append(inputExpression.Substring(num2, i - num2));
			}
			bool flag = false;
			if (i >= inputExpression.Length || inputExpression[i] != '{')
			{
				continue;
			}
			i++;
			num2 = i;
			while (i < inputExpression.Length)
			{
				if (inputExpression[i] == '"')
				{
					i++;
					int j;
					for (j = i; j < inputExpression.Length && inputExpression[j] != '"'; j++)
					{
						if (inputExpression[j] == '\\')
						{
							j++;
						}
					}
					i = j + 1;
					continue;
				}
				if (inputExpression[i] == '}')
				{
					string value = inputExpression.Substring(num2, i - num2);
					if (!string.IsNullOrWhiteSpace(value))
					{
						stringBuilder.Append(values[num++]);
					}
					flag = true;
					i++;
					break;
				}
				i++;
			}
			if (!flag)
			{
				stringBuilder.Append(inputExpression.Substring(num2 - 1));
			}
		}
		return stringBuilder.ToString();
	}

	public static bool IsTimeExpression(string str)
	{
		if (string.IsNullOrWhiteSpace(str))
		{
			return false;
		}
		List<string> list = FetchBlocksFromExpression(str);
		if (list == null || list.Count == 0)
		{
			return false;
		}
		return list.All((string b) => b.ToLowerInvariant().Trim() == "time");
	}
}
