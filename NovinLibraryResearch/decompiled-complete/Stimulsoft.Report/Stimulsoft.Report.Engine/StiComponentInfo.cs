using System;

namespace Stimulsoft.Report.Engine;

public class StiComponentInfo : ICloneable
{
	public object Clone()
	{
		return MemberwiseClone() as StiComponentInfo;
	}
}
