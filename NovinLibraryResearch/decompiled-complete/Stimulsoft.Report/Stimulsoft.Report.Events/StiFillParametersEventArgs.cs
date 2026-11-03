using System;
using System.Collections.Generic;
using Stimulsoft.Base.Blocks;

namespace Stimulsoft.Report.Events;

public class StiFillParametersEventArgs : EventArgs, IStiBlocklyValueEventArgs
{
	public virtual Dictionary<string, object> Value { get; set; } = new Dictionary<string, object>();


	public StiFillParametersEventArgs()
		: this(new Dictionary<string, object>())
	{
	}

	public StiFillParametersEventArgs(Dictionary<string, object> value)
	{
		Value = value;
	}
}
