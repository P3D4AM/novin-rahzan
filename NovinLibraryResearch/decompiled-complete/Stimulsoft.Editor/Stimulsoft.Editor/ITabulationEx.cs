namespace Stimulsoft.Editor;

public interface ITabulationEx : ITabulation
{
	string GetTabString(string s);

	int GetTabStop(int pos);

	int GetPrevTabStop(int pos);

	string GetIndentString(int count, int pos);

	int TabPosToPos(string s, int pos);

	int PosToTabPos(string s, int pos);
}
