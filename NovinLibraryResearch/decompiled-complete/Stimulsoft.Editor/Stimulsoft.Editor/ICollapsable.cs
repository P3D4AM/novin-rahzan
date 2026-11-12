using System.Collections;
using System.Drawing;

namespace Stimulsoft.Editor;

public interface ICollapsable
{
	int CollapsedCount { get; }

	int UpdateCount { get; }

	bool AllowOutlining { get; set; }

	OutlineOptions OutlineOptions { get; set; }

	IOutlineRange Outline(int first, int last, int level);

	IOutlineRange Outline(int first, int last);

	IOutlineRange Outline(Point startPoint, Point endPoint, int level);

	IOutlineRange Outline(Point startPoint, Point endPoint);

	IOutlineRange Outline(int first, int last, int level, string outlineText);

	IOutlineRange Outline(int first, int last, string outlineText);

	IOutlineRange Outline(Point startPoint, Point endPoint, int level, string outlineText);

	IOutlineRange Outline(Point startPoint, Point endPoint, string outlineText);

	void UnOutline(Point position);

	void UnOutline(int index);

	void UnOutline();

	IOutlineRange GetOutlineRange(Point position);

	IOutlineRange GetOutlineRange(int index);

	bool IsExpanded(int index);

	bool IsCollapsed(int index);

	bool IsVisible(Point position);

	bool IsVisible(int index);

	void Collapse(int index);

	void Expand(int index);

	void EnsureExpanded(int index);

	void EnsureExpanded(Point position);

	void FullExpand();

	void FullExpand(IList ranges);

	void FullCollapse();

	void FullCollapse(IList ranges);

	void ToggleOutlining();

	void CollapseToDefinitions();

	string GetOutlineHint(IOutlineRange range);

	int GetOutlineRanges(IList ranges, int index);

	int GetOutlineRanges(IList ranges, Point position);

	int GetOutlineRanges(IList ranges, Point startPoint, Point endPoint);

	int GetOutlineRanges(IList ranges);

	void SetOutlineRanges(IList ranges);

	void SetOutlineRanges(IList ranges, bool preserveVisible);

	int BeginUpdate();

	int EndUpdate();

	void ResetAllowOutlining();

	void ResetOutlineOptions();
}
