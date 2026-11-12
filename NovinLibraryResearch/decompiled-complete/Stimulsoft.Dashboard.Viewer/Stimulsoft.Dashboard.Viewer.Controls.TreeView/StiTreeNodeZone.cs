using System.Drawing;
using Stimulsoft.Dashboard.Drawing.Helpers;

namespace Stimulsoft.Dashboard.Viewer.Controls.TreeView;

internal class StiTreeNodeZone
{
	public string Text { get; private set; }

	public Rectangle Rect { get; private set; }

	public StiTreeNode Node { get; private set; }

	public bool IsAll { get; private set; }

	public bool IsStacked { get; private set; }

	internal static StiTreeNodeZone MakeAll(Rectangle rect)
	{
		return new StiTreeNodeZone
		{
			Text = StiLocHelper.LocAll,
			IsAll = true,
			Rect = rect
		};
	}

	internal static StiTreeNodeZone MakeStacked(string text, Rectangle rect)
	{
		return new StiTreeNodeZone
		{
			Text = text,
			IsStacked = true,
			Rect = rect
		};
	}

	internal static StiTreeNodeZone Make(string text, Rectangle rect, StiTreeNode node)
	{
		return new StiTreeNodeZone
		{
			Text = text,
			Rect = rect,
			Node = node
		};
	}

	private StiTreeNodeZone()
	{
	}
}
