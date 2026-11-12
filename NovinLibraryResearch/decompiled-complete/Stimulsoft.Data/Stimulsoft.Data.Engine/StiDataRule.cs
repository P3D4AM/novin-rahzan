using System;

namespace Stimulsoft.Data.Engine;

public abstract class StiDataRule : ICloneable
{
	public object Clone()
	{
		return MemberwiseClone();
	}

	public abstract int GetUniqueCode();
}
