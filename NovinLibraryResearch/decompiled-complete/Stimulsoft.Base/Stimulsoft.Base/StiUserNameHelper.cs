using System;

namespace Stimulsoft.Base;

public static class StiUserNameHelper
{
	private static string userName;

	private static object lockObject = new object();

	public static string GetUserName()
	{
		lock (lockObject)
		{
			if (userName != null)
			{
				return userName;
			}
			try
			{
				userName = Environment.UserName;
			}
			catch
			{
				userName = "Undefined";
			}
			return userName;
		}
	}
}
