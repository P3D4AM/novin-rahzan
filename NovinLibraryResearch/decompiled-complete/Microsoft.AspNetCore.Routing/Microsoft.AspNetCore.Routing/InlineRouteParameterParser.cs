using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing.Template;

namespace Microsoft.AspNetCore.Routing;

public static class InlineRouteParameterParser
{
	private enum ParseState
	{
		Start,
		ParsingName,
		InsideParenthesis,
		End
	}

	private struct ConstraintParseResults
	{
		public int CurrentIndex;

		public IEnumerable<InlineConstraint> Constraints;
	}

	public static TemplatePart ParseRouteParameter(string routeParameter)
	{
		if (routeParameter == null)
		{
			throw new ArgumentNullException("routeParameter");
		}
		if (routeParameter.Length == 0)
		{
			return TemplatePart.CreateParameter(string.Empty, isCatchAll: false, isOptional: false, null, null);
		}
		int num = 0;
		int num2 = routeParameter.Length - 1;
		bool isCatchAll = false;
		bool isOptional = false;
		if (routeParameter[0] == '*')
		{
			isCatchAll = true;
			num++;
		}
		if (routeParameter[num2] == '?')
		{
			isOptional = true;
			num2--;
		}
		int i = num;
		string name = string.Empty;
		for (; i <= num2; i++)
		{
			char c = routeParameter[i];
			if ((c == ':' || c == '=') && num != i)
			{
				name = routeParameter.Substring(num, i - num);
				i--;
				break;
			}
			if (i == num2)
			{
				name = routeParameter.Substring(num, i - num + 1);
			}
		}
		ConstraintParseResults constraintParseResults = ParseConstraints(routeParameter, i, num2);
		i = constraintParseResults.CurrentIndex;
		string defaultValue = null;
		if (i <= num2 && routeParameter[i] == '=')
		{
			defaultValue = routeParameter.Substring(i + 1, num2 - i);
		}
		return TemplatePart.CreateParameter(name, isCatchAll, isOptional, defaultValue, constraintParseResults.Constraints);
	}

	private static ConstraintParseResults ParseConstraints(string routeParameter, int currentIndex, int endIndex)
	{
		List<InlineConstraint> list = new List<InlineConstraint>();
		ParseState parseState = ParseState.Start;
		int num = currentIndex;
		do
		{
			char? c = ((currentIndex > endIndex) ? null : new char?(routeParameter[currentIndex]));
			switch (parseState)
			{
			case ParseState.Start:
				switch (c)
				{
				case null:
					parseState = ParseState.End;
					break;
				case ':':
					parseState = ParseState.ParsingName;
					num = currentIndex + 1;
					break;
				case '(':
					parseState = ParseState.InsideParenthesis;
					break;
				case '=':
					parseState = ParseState.End;
					currentIndex--;
					break;
				}
				break;
			case ParseState.InsideParenthesis:
				switch (c)
				{
				case null:
				{
					parseState = ParseState.End;
					string constraint2 = routeParameter.Substring(num, currentIndex - num);
					list.Add(new InlineConstraint(constraint2));
					break;
				}
				case ')':
					switch ((currentIndex + 1 > endIndex) ? null : new char?(routeParameter[currentIndex + 1]))
					{
					case null:
					{
						parseState = ParseState.End;
						string constraint2 = routeParameter.Substring(num, currentIndex - num + 1);
						list.Add(new InlineConstraint(constraint2));
						break;
					}
					case ':':
					{
						parseState = ParseState.Start;
						string constraint2 = routeParameter.Substring(num, currentIndex - num + 1);
						list.Add(new InlineConstraint(constraint2));
						num = currentIndex + 1;
						break;
					}
					case '=':
					{
						parseState = ParseState.End;
						string constraint2 = routeParameter.Substring(num, currentIndex - num + 1);
						list.Add(new InlineConstraint(constraint2));
						break;
					}
					}
					break;
				case ':':
				case '=':
				{
					int num2 = routeParameter.IndexOf(')', currentIndex + 1);
					if (num2 == -1)
					{
						string constraint2 = routeParameter.Substring(num, currentIndex - num);
						list.Add(new InlineConstraint(constraint2));
						if (c == ':')
						{
							parseState = ParseState.ParsingName;
							num = currentIndex + 1;
						}
						else
						{
							parseState = ParseState.End;
							currentIndex--;
						}
					}
					else
					{
						currentIndex = num2;
					}
					break;
				}
				}
				break;
			case ParseState.ParsingName:
				switch (c)
				{
				case null:
				{
					parseState = ParseState.End;
					string constraint = routeParameter.Substring(num, currentIndex - num);
					list.Add(new InlineConstraint(constraint));
					break;
				}
				case ':':
				{
					string constraint = routeParameter.Substring(num, currentIndex - num);
					list.Add(new InlineConstraint(constraint));
					num = currentIndex + 1;
					break;
				}
				case '(':
					parseState = ParseState.InsideParenthesis;
					break;
				case '=':
				{
					parseState = ParseState.End;
					string constraint = routeParameter.Substring(num, currentIndex - num);
					list.Add(new InlineConstraint(constraint));
					currentIndex--;
					break;
				}
				}
				break;
			}
			currentIndex++;
		}
		while (parseState != ParseState.End);
		ConstraintParseResults result = default(ConstraintParseResults);
		result.CurrentIndex = currentIndex;
		result.Constraints = list;
		return result;
	}
}
