using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls;

public class StiDrawTreeNodeEventArgs
{
	public Rectangle Bounds { get; }

	public Graphics Graphics { get; }

	public TreeNode Node { get; }

	public StiDrawTreeNodeEventArgs(Graphics graphics, TreeNode node, Rectangle bounds)
	{
		Graphics = graphics;
		Node = node;
		Bounds = bounds;
	}
}
