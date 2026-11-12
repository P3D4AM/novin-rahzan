using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public interface ISearchDialog
{
	bool Visible { get; set; }

	Form OwnerForm { get; set; }

	SearchSettings SearchSettings { get; }

	DialogResult Execute(ISearch search, bool isModal, bool isReplace);

	void DoneSearch(ISearch search);

	void EnsureVisible(Rectangle rect);

	void ToggleHiddenText();

	void ToggleMatchCase();

	void ToggleRegularExpressions();

	void ToggleSearchUp();

	void ToggleWholeWord();

	void Close();
}
