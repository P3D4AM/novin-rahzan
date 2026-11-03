namespace Stimulsoft.Report.Dictionary;

public class StiFunctionsProgramming
{
	public static object Choose(int index, params object[] args)
	{
		if (args.Length == 0)
		{
			return null;
		}
		if (index < 1 || index > args.Length)
		{
			return null;
		}
		return args[index - 1];
	}

	public static object Choose(int? index, params object[] args)
	{
		if (!index.HasValue)
		{
			return null;
		}
		if (args.Length == 0)
		{
			return null;
		}
		if (index.Value < 1 || index.Value > args.Length)
		{
			return null;
		}
		return args[index.Value - 1];
	}

	public static object Switch(params object[] args)
	{
		for (int i = 0; i < args.Length; i += 2)
		{
			object obj = args[i];
			object result = args[i + 1];
			if (obj is bool && (bool)obj)
			{
				return result;
			}
			if (obj is bool? flag && flag == true)
			{
				return result;
			}
		}
		return null;
	}
}
