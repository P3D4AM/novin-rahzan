using System;
using System.Collections.Generic;
using System.Linq;

namespace TimeAttendanceDAL.Utility.Settings;

public class ExpressionClass
{
	public List<string> InwardExpSecond;

	public List<string> InwardExpThird;

	public bool Result { get; set; } = true;


	public string ErrorMsg { get; set; } = "";


	public List<string> InwardExpFirst { get; set; } = new List<string>();


	public ExpressionClass(string pExpression)
	{
		if (!isValidFormula(pExpression))
		{
			Result = false;
			ErrorMsg = "فرمول نامعتبر است. تعداد براکت های باز و بسته درست نیست.";
		}
		else
		{
			string inwardExpressionLevel = RemoveSpaceInObject(pExpression);
			SetInwardExpressionLevel1(inwardExpressionLevel);
		}
	}

	private void SetInwardExpressionLevel1(string text2)
	{
		text2 = text2.TrimStart(Array.Empty<char>());
		if (text2.StartsWith("شرط"))
		{
			string inwardExpressionLevel = "";
			string inwardExpressionLevel2 = "";
			string inwardExpressionLevel3 = "";
			int i = 0;
			bool flag = true;
			for (; i < text2.Length && text2[i] != '('; i++)
			{
			}
			if (i >= text2.Length)
			{
				Result = false;
				ErrorMsg = "فرمول نامعتبر است. شرط درست نیست.";
				return;
			}
			text2 = text2.Substring(i + 1);
			for (i = 0; i < text2.Length; i++)
			{
				if (flag && text2[i] == ',')
				{
					inwardExpressionLevel = text2.Substring(0, i);
					break;
				}
				if (text2[i] == '[')
				{
					flag = false;
				}
				if (text2[i] == ']')
				{
					flag = true;
				}
			}
			if (i >= text2.Length)
			{
				Result = false;
				ErrorMsg = "فرمول نامعتبر است. شرط درست نیست.";
				return;
			}
			text2 = text2.Substring(i + 1);
			for (i = 0; i < text2.Length; i++)
			{
				if (flag && text2[i] == ',')
				{
					inwardExpressionLevel2 = text2.Substring(0, i);
					break;
				}
				if (text2[i] == '[')
				{
					flag = false;
				}
				if (text2[i] == ']')
				{
					flag = true;
				}
			}
			if (i >= text2.Length)
			{
				Result = false;
				ErrorMsg = "فرمول نامعتبر است. شرط درست نیست.";
				return;
			}
			text2 = text2.Substring(i + 1);
			for (i = 0; i < text2.Length; i++)
			{
				if (flag && text2[i] == ')')
				{
					inwardExpressionLevel3 = text2.Substring(0, i);
					break;
				}
				if (text2[i] == '[')
				{
					flag = false;
				}
				if (text2[i] == ']')
				{
					flag = true;
				}
			}
			if (i >= text2.Length)
			{
				Result = false;
				ErrorMsg = "فرمول نامعتبر است. شرط درست نیست.";
				return;
			}
			SetInwardExpressionLevel2(inwardExpressionLevel3);
			if (Result)
			{
				InwardExpThird = InwardExpFirst;
				InwardExpFirst = new List<string>();
				SetInwardExpressionLevel2(inwardExpressionLevel2);
				if (Result)
				{
					InwardExpSecond = InwardExpFirst;
					InwardExpFirst = new List<string>();
					SetInwardExpressionLevel2(inwardExpressionLevel);
				}
			}
		}
		else
		{
			SetInwardExpressionLevel2(text2);
		}
	}

	private void SetInwardExpressionLevel2(string pExpression)
	{
		Stack<string> stack = new Stack<string>();
		try
		{
			List<string> list = (from p in pExpression.Split(new char[1] { ' ' })
				where !string.IsNullOrEmpty(p)
				select p).ToList();
			list.Add(")");
			stack.Push("(");
			int num = 0;
			foreach (string item in list)
			{
				if (!"+-*/()".Contains(item))
				{
					if (num == 1 || num == 4)
					{
						Result = false;
						ErrorMsg = "فرمول نامعتبر است";
						break;
					}
					InwardExpFirst.Add(item);
					num = 1;
				}
				else if ("+-*/".Contains(item))
				{
					if (num == 2 || num == 3)
					{
						Result = false;
						ErrorMsg = "فرمول نامعتبر است";
						break;
					}
					while (Precedence(item) <= Precedence(stack.Peek()))
					{
						InwardExpFirst.Add(stack.Pop());
					}
					stack.Push(item);
					num = 2;
				}
				else if (item == "(")
				{
					if (num == 1 || num == 4)
					{
						Result = false;
						ErrorMsg = "فرمول نامعتبر است";
						break;
					}
					stack.Push("(");
					num = 3;
				}
				else if (item == ")")
				{
					if (num == 2 || num == 3)
					{
						Result = false;
						ErrorMsg = "فرمول نامعتبر است";
						break;
					}
					while (stack.Peek() != "(")
					{
						InwardExpFirst.Add(stack.Pop());
					}
					stack.Pop();
					num = 4;
				}
			}
			if (stack.Count > 0)
			{
				Result = false;
				ErrorMsg = "فرمول نامعتبر است";
			}
		}
		catch
		{
			Result = false;
			ErrorMsg = "فرمول نامعتبر است";
		}
	}

	private int Precedence(string pOperator)
	{
		int result = -1;
		switch (pOperator)
		{
		case "+":
		case "-":
			result = 0;
			break;
		case "*":
		case "/":
			result = 1;
			break;
		}
		return result;
	}

	private bool isValidFormula(string pFormula)
	{
		bool result = true;
		int num = 0;
		while (num != -1)
		{
			int num2 = pFormula.IndexOf('[', num);
			int num3 = pFormula.IndexOf(']', num);
			if (num2 == -1)
			{
				if (num3 >= 0)
				{
					result = false;
				}
				break;
			}
			if (num2 != -1 && num3 < num2)
			{
				result = false;
				break;
			}
			num = num2 + 1;
			num2 = pFormula.IndexOf('[', num);
			num3 = pFormula.IndexOf(']', num);
			if (num3 == -1)
			{
				result = false;
				break;
			}
			if (num2 != -1 && num3 > num2)
			{
				result = false;
				break;
			}
			num = num3 + 1;
		}
		return result;
	}

	public static string RemoveSpaceInObject(string xsharh)
	{
		string text = xsharh;
		int startIndex = 0;
		int num = text.IndexOf('[', startIndex);
		int num2 = text.IndexOf(']', startIndex);
		while (num > -1 && num2 > -1)
		{
			text = text.Substring(0, num) + text.Substring(num, num2 - num).Replace(" ", "") + text.Substring(num2);
			num2 = text.IndexOf(']', startIndex);
			startIndex = num2 + 1;
			num = text.IndexOf('[', startIndex);
			num2 = text.IndexOf(']', startIndex);
		}
		return text;
	}
}
