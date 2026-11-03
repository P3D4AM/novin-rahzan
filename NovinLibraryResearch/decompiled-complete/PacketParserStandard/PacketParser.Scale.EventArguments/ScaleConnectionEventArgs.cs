using System;

namespace PacketParser.Scale.EventArguments;

public class ScaleConnectionEventArgs : System.EventArgs
{
	public MicroScaleInfo Scale { get; set; }

	public bool Status { get; set; }

	public ScaleConnectionEventArgs(MicroScaleInfo scale, bool status)
	{
		Scale = scale;
		Status = status;
	}
}
