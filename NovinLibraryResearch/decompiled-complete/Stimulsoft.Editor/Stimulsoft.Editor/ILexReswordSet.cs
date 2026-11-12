namespace Stimulsoft.Editor;

public interface ILexReswordSet
{
	string Name { get; set; }

	ILexStyle ReswordStyle { get; set; }

	string[] Reswords { get; set; }

	int Index { get; }

	int AddResword(string resword);

	bool FindResword(string resword);

	void Clear();
}
