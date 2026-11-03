namespace Stimulsoft.Report.Dictionary;

public interface IStiEnumerator
{
	int Position { get; set; }

	int Count { get; }

	bool IsEof { get; set; }

	bool IsBof { get; set; }

	bool IsEmpty { get; }

	void First();

	void Prior();

	void Next();

	void Last();
}
