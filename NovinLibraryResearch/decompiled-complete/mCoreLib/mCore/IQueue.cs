namespace mCore;

public interface IQueue
{
	bool Enabled { get; set; }

	int Count { get; }

	bool Clear();
}
