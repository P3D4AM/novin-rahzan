using System;

namespace Stimulsoft.Base;

public class StiTestConnectionResult
{
	public bool Success { get; set; }

	public string Notice { get; set; }

	public static StiTestConnectionResult MakeWrong(string notice)
	{
		return new StiTestConnectionResult
		{
			Success = false,
			Notice = notice
		};
	}

	public static StiTestConnectionResult MakeWrong(Exception exception)
	{
		return new StiTestConnectionResult
		{
			Success = false,
			Notice = exception.Message
		};
	}

	public static StiTestConnectionResult MakeWrong()
	{
		return new StiTestConnectionResult
		{
			Success = false
		};
	}

	public static StiTestConnectionResult MakeFine()
	{
		return new StiTestConnectionResult
		{
			Success = true
		};
	}
}
