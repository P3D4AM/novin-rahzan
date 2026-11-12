using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiPopup : ToolStripDropDown
{
	private ToolStripControlHost host;

	public Control Content { get; set; }

	protected void UpdateRegion()
	{
		if (((ToolStripDropDown)this).Region != null)
		{
			((ToolStripDropDown)this).Region.Dispose();
			((ToolStripDropDown)this).Region = null;
		}
		if (Content.Region != null)
		{
			((ToolStripDropDown)this).Region = Content.Region.Clone();
		}
	}

	public void Show(Rectangle area)
	{
		Point point = new Point(area.Left, area.Top + area.Height);
		Rectangle workingArea = Screen.FromControl((Control)(object)this).WorkingArea;
		if (point.X + ((Control)this).Size.Width > workingArea.Left + workingArea.Width)
		{
			point.X = workingArea.Left + workingArea.Width - ((Control)this).Size.Width;
		}
		if (point.Y + ((Control)this).Size.Height > workingArea.Top + workingArea.Height)
		{
			point.Y -= ((Control)this).Size.Height + area.Height;
		}
		((ToolStripDropDown)this).Show(point, (ToolStripDropDownDirection)3);
	}

	protected override void OnLayout(LayoutEventArgs e)
	{
		Size preferredSize = ((Control)this).GetPreferredSize(Size.Empty);
		if (((Control)this).AutoSize && preferredSize != ((Control)this).Size)
		{
			((Control)this).Size = preferredSize;
		}
		((ToolStrip)this).SetDisplayedItems();
		((ToolStrip)this).OnLayoutCompleted(EventArgs.Empty);
		((Control)this).Invalidate();
	}

	protected override void OnOpening(CancelEventArgs e)
	{
		if (Content.IsDisposed || Content.Disposing)
		{
			e.Cancel = true;
			return;
		}
		UpdateRegion();
		((ToolStripDropDown)this).OnOpening(e);
	}

	public StiPopup(Control content)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		StiPopup stiPopup = this;
		Content = content;
		((Control)this).AutoSize = false;
		((Control)this).DoubleBuffered = true;
		((Control)this).ResizeRedraw = true;
		host = new ToolStripControlHost(content);
		ToolStripControlHost obj = host;
		Padding val = (((ToolStripItem)host).Margin = Padding.Empty);
		Padding val3 = (((ToolStripItem)obj).Padding = val);
		Padding padding = (((Control)this).Margin = val3);
		((Control)this).Padding = padding;
		((Control)this).MinimumSize = content.MinimumSize;
		content.MinimumSize = content.Size;
		((Control)this).MaximumSize = content.MaximumSize;
		content.MaximumSize = content.Size;
		((Control)this).Size = content.Size;
		bool tabStop = (content.TabStop = true);
		((ToolStrip)this).TabStop = tabStop;
		content.Location = Point.Empty;
		((ToolStrip)this).Items.Add((ToolStripItem)(object)host);
		((Component)(object)content).Disposed += delegate
		{
			content = null;
			((Component)(object)stiPopup).Dispose(disposing: true);
		};
		UpdateRegion();
	}
}
