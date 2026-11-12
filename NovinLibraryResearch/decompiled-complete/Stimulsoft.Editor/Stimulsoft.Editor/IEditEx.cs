using System.Drawing;

namespace Stimulsoft.Editor;

public interface IEditEx : IEdit
{
	bool Insert(string text);

	bool InsertFromFile(string fileName);

	bool DeleteRight(int len);

	bool DeleteLeft(int len);

	bool InsertBlock(string text);

	bool InsertBlock(string[] strings);

	bool InsertBlock(ISyntaxStrings strings);

	bool DeleteBlock(Rectangle rect);

	bool BreakLine();

	bool UnBreakLine();

	bool NewLine();

	bool NewLineAbove();

	bool NewLineBelow();

	bool IsLineReadonly(int index);

	void SetLineReadonly(int index, bool readOnly);
}
