using System;

namespace Stimulsoft.Base;

public static class StiGuidUtils
{
	public static string NewGuid()
	{
		return Guid.NewGuid().ToString().Replace("-", "");
	}
}
