namespace mCore;

public interface IInbox
{
	bool Concatenate { get; set; }

	bool HasMessages { get; }

	bool IsEmpty { get; }

	int Count { get; }

	Message Message(int Index);

	bool Refresh();

	bool Refresh(MessageType ReadMessageType);

	bool Refresh(MessageType ReadMessageType, bool CleanUpMemory);

	bool Clear();
}
