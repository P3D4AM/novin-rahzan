namespace mCore;

public interface INewUSSDEventArgs
{
	string Message { get; }

	USSDStatus Status { get; }
}
