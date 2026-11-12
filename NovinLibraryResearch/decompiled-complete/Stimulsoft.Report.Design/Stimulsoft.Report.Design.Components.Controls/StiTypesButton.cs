using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Report.Design.Components.Controls;

[ToolboxItem(false)]
public class StiTypesButton : ButtonX
{
	private int DropDownButtonWidth => SystemInformation.VerticalScrollBarWidth - 1;

	public bool ShowArrow { get; set; }

	public StiTypesButton()
	{
		((Control)this).SetStyle((ControlStyles)512, false);
	}

	private Rectangle GetDownButtonRect()
	{
		Rectangle result = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		result.X = result.Right - DropDownButtonWidth - 1;
		result.Width = DropDownButtonWidth;
		return result;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		base.OnPaint(e);
		if (ShowArrow)
		{
			Rectangle downButtonRect = GetDownButtonRect();
			StiButtonHelper.DrawDownArrow(e.Graphics, downButtonRect, ((Control)this).Enabled);
		}
	}
}
