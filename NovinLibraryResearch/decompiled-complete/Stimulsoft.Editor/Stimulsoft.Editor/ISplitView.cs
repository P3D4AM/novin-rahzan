using System;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public interface ISplitView
{
	ISyntaxEdit VertSplitEdit { get; }

	ISyntaxEdit HorzSplitEdit { get; }

	Splitter VertSplitter { get; }

	Splitter HorzSplitter { get; }

	event EventHandler SplitHorz;

	event EventHandler SplitVert;

	event EventHandler UnsplitHorz;

	event EventHandler UnsplitVert;

	bool CanSplitHorz();

	bool CanSplitVert();

	bool CanUnsplitHorz();

	bool CanUnsplitVert();

	void SplitViewHorz();

	void SplitViewVert();

	void UnsplitViewHorz();

	void UnsplitViewVert();
}
