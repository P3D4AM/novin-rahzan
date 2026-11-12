using System;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Logic;

public class LogicCompare : IronBlock
{
	public override object Evaluate(Context context)
	{
		object value = StiObjConverter.GetValue(base.Values.Evaluate("A", context));
		object value2 = StiObjConverter.GetValue(base.Values.Evaluate("B", context));
		string text = base.Fields.Get("OP");
		if (value is string)
		{
			return Compare(text, value as string, value2 as string);
		}
		if (value is double || value is float)
		{
			return Compare(text, Convert.ToDouble(value), Convert.ToDouble(value2));
		}
		if (value is int)
		{
			return Compare(text, Convert.ToInt32(value), Convert.ToInt32(value2));
		}
		if (value is bool)
		{
			return Compare(text, (bool)value, (bool)value2);
		}
		if (value == null && value2 == null)
		{
			if (text == "EQ")
			{
				return true;
			}
			if (text == "NEQ")
			{
				return false;
			}
		}
		else if (value == null && value2 != null)
		{
			if (text == "EQ")
			{
				return false;
			}
			if (text == "NEQ")
			{
				return true;
			}
		}
		else if (value != null && value2 == null)
		{
			if (text == "EQ")
			{
				return false;
			}
			if (text == "NEQ")
			{
				return true;
			}
		}
		throw new ApplicationException("unexpected value type");
	}

	private bool Compare(string op, string a, string b)
	{
		return op switch
		{
			"EQ" => a == b, 
			"NEQ" => a != b, 
			"LT" => string.Compare(a, b) < 0, 
			"LTE" => string.Compare(a, b) <= 0, 
			"GT" => string.Compare(a, b) > 0, 
			"GTE" => string.Compare(a, b) >= 0, 
			_ => throw new ApplicationException("Unknown OP " + op), 
		};
	}

	private bool Compare(string op, double a, double b)
	{
		return op switch
		{
			"EQ" => a == b, 
			"NEQ" => a != b, 
			"LT" => a < b, 
			"LTE" => a <= b, 
			"GT" => a > b, 
			"GTE" => a >= b, 
			_ => throw new ApplicationException("Unknown OP " + op), 
		};
	}

	private bool Compare(string op, int a, int b)
	{
		return op switch
		{
			"EQ" => a == b, 
			"NEQ" => a != b, 
			"LT" => a < b, 
			"LTE" => a <= b, 
			"GT" => a > b, 
			"GTE" => a >= b, 
			_ => throw new ApplicationException("Unknown OP " + op), 
		};
	}

	private bool Compare(string op, bool a, bool b)
	{
		if (!(op == "EQ"))
		{
			if (op == "NEQ")
			{
				return a != b;
			}
			throw new ApplicationException("Unknown OP " + op);
		}
		return a == b;
	}
}
