using System;
using System.Collections.Generic;

namespace Stimulsoft.Base.Services;

public class StiServiceActionEventArgs : EventArgs
{
	public List<StiService> Services { get; set; }

	public bool Processed { get; set; }

	public StiServiceActionType ActionType { get; }

	public StiServiceActionEventArgs(StiServiceActionType type)
	{
		ActionType = type;
	}
}
