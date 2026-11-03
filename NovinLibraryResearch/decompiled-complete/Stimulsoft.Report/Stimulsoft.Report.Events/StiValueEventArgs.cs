using System;
using Stimulsoft.Base.Blocks;

namespace Stimulsoft.Report.Events;

public class StiValueEventArgs : EventArgs, IStiBlocklyValueEventArgs
{
	public virtual object Value { get; set; }

	public virtual string DisplayValue { get; set; }

	public StiValueEventArgs()
		: this(null)
	{
	}

	public StiValueEventArgs(object value)
	{
		Value = value;
	}
}
