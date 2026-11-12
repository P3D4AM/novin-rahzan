namespace Antlr.Runtime.Tree;

public interface IPositionTrackingStream
{
	object GetKnownPositionElement(bool allowApproximateLocation);

	bool HasPositionInformation(object element);
}
