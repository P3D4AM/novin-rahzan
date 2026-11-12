using System;
using Stimulsoft.Blockly.Model;

namespace Stimulsoft.Blockly.Blocks.Maths;

public class MathNumberProperty : IronBlock
{
	public override object Evaluate(Context context)
	{
		string text = base.Fields.Get("PROPERTY");
		double num = Convert.ToDouble(base.Values.Evaluate("NUMBER_TO_CHECK", context));
		return text switch
		{
			"EVEN" => 0.0 == num % 2.0, 
			"ODD" => 1.0 == num % 2.0, 
			"PRIME" => IsPrime((int)num), 
			"WHOLE" => 0.0 == num % 1.0, 
			"POSITIVE" => num > 0.0, 
			"NEGATIVE" => num < 0.0, 
			"DIVISIBLE_BY" => 0.0 == num % (double)base.Values.Evaluate("DIVISOR", context), 
			_ => throw new ApplicationException("Unknown PROPERTY " + text), 
		};
	}

	private static bool IsPrime(int number)
	{
		switch (number)
		{
		case 1:
			return false;
		case 2:
			return true;
		default:
		{
			if (number % 2 == 0)
			{
				return false;
			}
			int num = (int)Math.Floor(Math.Sqrt(number));
			for (int i = 3; i <= num; i += 2)
			{
				if (number % i == 0)
				{
					return false;
				}
			}
			return true;
		}
		}
	}
}
