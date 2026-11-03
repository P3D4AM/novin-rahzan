using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Report.Controls;

public class StiDrawTreeNodeEventArgs
{
	private readonly Rectangle bounds;

	private readonly Graphics graphics;

	private readonly TreeNode node;

	public Rectangle Bounds => bounds;

	public Graphics Graphics => graphics;

	public TreeNode Node => node;

	public StiDrawTreeNodeEventArgs(Graphics graphics, TreeNode node, Rectangle bounds)
	{
		this.graphics = graphics;
		this.node = node;
		this.bounds = bounds;
	}
}
