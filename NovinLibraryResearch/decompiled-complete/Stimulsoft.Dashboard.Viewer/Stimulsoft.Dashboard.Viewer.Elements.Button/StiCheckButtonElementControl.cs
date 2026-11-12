using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Button;
using Stimulsoft.Dashboard.Drawing.Painters;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Dashboard.Viewer.Elements.Button;

[ToolboxItem(false)]
public class StiCheckButtonElementControl : CheckBox, IStiRefreshControl, IStiElementControl
{
	private bool isPressed;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IStiElement Element { get; set; }

	private bool AlwaysTrueOnChecked { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsMouseOver => ((Control)this).ClientRectangle.Contains(((Control)this).PointToClient(Cursor.Position));

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsPressed
	{
		get
		{
			return isPressed;
		}
		set
		{
			if (isPressed != value)
			{
				isPressed = value;
				((Control)this).Invalidate();
			}
		}
	}

	protected bool IsRadioButton { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiButtonElement ButtonElement => Element as StiButtonElement;

	public void RefreshControl(object sender, StiReportControlUpdateEventArgs e)
	{
		if (sender == ButtonElement)
		{
			((CheckBox)this).Checked = ButtonElement.Checked;
			((Control)this).Invalidate();
		}
	}

	protected override void OnMouseDown(MouseEventArgs mevent)
	{
		((ButtonBase)this).OnMouseDown(mevent);
		IsPressed = true;
	}

	protected override void OnMouseUp(MouseEventArgs mevent)
	{
		((CheckBox)this).OnMouseUp(mevent);
		IsPressed = false;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		((ButtonBase)this).OnPaint(e);
		Graphics graphics = e.Graphics;
		RectangleD rect = new RectangleD(0.0, 0.0, ((Control)this).Width - 1, ((Control)this).Height - 1);
		StiButtonVisualState visualState = GetVisualState();
		StiButtonElementGdiPainter.PaintButton(graphics, rect, Element, visualState, ((CheckBox)this).Checked);
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
	}

	protected override void OnClick(EventArgs e)
	{
		((CheckBox)this).OnClick(e);
		try
		{
			StiDashboardViewerHelper.EnableRefreshViewerProcessing(StiDashboardViewerHelper.FindViewer((Control)(object)this));
			ButtonElement?.InvokeClick(ButtonElement, e);
		}
		finally
		{
			StiDashboardViewerHelper.DisableRefreshViewerProcessing(StiDashboardViewerHelper.FindViewer((Control)(object)this));
		}
	}

	protected override void OnCheckedChanged(EventArgs e)
	{
		((CheckBox)this).OnCheckedChanged(e);
		if (AlwaysTrueOnChecked)
		{
			((CheckBox)this).Checked = true;
		}
		if (ButtonElement != null)
		{
			ButtonElement.Checked = ((CheckBox)this).Checked;
			try
			{
				StiDashboardViewerHelper.EnableRefreshViewerProcessing(StiDashboardViewerHelper.FindViewer((Control)(object)this));
				ButtonElement?.InvokeClick(ButtonElement, e);
			}
			finally
			{
				StiDashboardViewerHelper.DisableRefreshViewerProcessing(StiDashboardViewerHelper.FindViewer((Control)(object)this));
			}
		}
		if (!((CheckBox)this).Checked || !IsRadioButton || ((Control)this).Parent == null || Element == null)
		{
			return;
		}
		StiContainer parentComp = (Element as StiComponent)?.Parent;
		if (parentComp == null)
		{
			return;
		}
		StiDashboardViewerControl viewer = StiDashboardViewerHelper.FindViewer((Control)(object)this);
		if (viewer != null)
		{
			string group = SimplifyGroup(ButtonElement.Group);
			IEnumerable<StiButtonElement> source = from StiButtonElement c in from c in parentComp.Components.ToList()
					where c != Element && c is StiButtonElement
					where c.Parent == parentComp
					select c
				where c.Type == StiButtonType.RadioButton && SimplifyGroup(c.Group) == @group
				select c;
			(from c in source
				select viewer.ElementToControl[c] into c
				where c is StiCheckButtonElementControl
				select c).Cast<StiCheckButtonElementControl>().ToList().ForEach(delegate(StiCheckButtonElementControl c)
			{
				c.AlwaysTrueOnChecked = false;
				((CheckBox)c).Checked = false;
				c.AlwaysTrueOnChecked = true;
			});
		}
	}

	private StiButtonVisualState GetVisualState()
	{
		if (IsPressed)
		{
			return StiButtonVisualStatesHelper.GetPressed(ButtonElement);
		}
		if (IsMouseOver)
		{
			return StiButtonVisualStatesHelper.GetHovered(ButtonElement);
		}
		if (((CheckBox)this).Checked)
		{
			return StiButtonVisualStatesHelper.GetChecked(ButtonElement);
		}
		return StiButtonVisualStatesHelper.GetDefault(ButtonElement);
	}

	private string SimplifyGroup(string group)
	{
		if (group == null)
		{
			return string.Empty;
		}
		return group.Trim();
	}

	public StiCheckButtonElementControl(IStiElement element = null, bool isRadioButton = false)
	{
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)131072, true);
		((CheckBox)this).Appearance = (Appearance)1;
		Element = element;
		IsRadioButton = isRadioButton;
		((Control)this).BackColor = Color.Transparent;
		((ButtonBase)this).FlatStyle = (FlatStyle)0;
		((ButtonBase)this).FlatAppearance.BorderSize = 0;
		((ButtonBase)this).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)this).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)this).FlatAppearance.MouseOverBackColor = Color.Transparent;
		StiButtonElement stiButtonElement = element as StiButtonElement;
		((CheckBox)this).Checked = stiButtonElement.IsChecked;
	}
}
