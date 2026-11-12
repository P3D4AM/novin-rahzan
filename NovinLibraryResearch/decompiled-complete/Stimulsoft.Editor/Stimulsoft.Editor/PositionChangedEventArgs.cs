using System;

namespace Stimulsoft.Editor;

public class PositionChangedEventArgs : EventArgs
{
	public UpdateReason Reason;

	public int DeltaX;

	public int DeltaY;

	public PositionChangedEventArgs(UpdateReason reason, int deltaX, int deltaY)
	{
		Reason = reason;
		DeltaX = deltaX;
		DeltaY = deltaY;
	}
}
