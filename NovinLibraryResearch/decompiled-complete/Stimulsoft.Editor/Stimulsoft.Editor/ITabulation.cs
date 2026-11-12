namespace Stimulsoft.Editor;

public interface ITabulation
{
	int[] TabStops { get; set; }

	bool UseSpaces { get; set; }

	void ResetTabStops();

	void ResetUseSpaces();
}
