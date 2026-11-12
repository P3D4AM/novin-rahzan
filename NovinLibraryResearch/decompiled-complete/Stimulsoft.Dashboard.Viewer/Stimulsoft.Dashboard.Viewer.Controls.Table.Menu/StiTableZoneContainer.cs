using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace Stimulsoft.Dashboard.Viewer.Controls.Table.Menu;

[ToolboxItem(false)]
public class StiTableZoneContainer : UserControl
{
	private Point startPoint;

	private Rectangle dragBoxFromMouseDown;

	private IContainer components;

	public StiTableZoneControl FocusedControl
	{
		get
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
			{
				Control val = item;
				if (val is StiTableZoneControl { IsFocused: not false } stiTableZoneControl)
				{
					return stiTableZoneControl;
				}
			}
			return null;
		}
	}

	public event EventHandler ChangeFocusedControl;

	public StiTableZoneContainer()
	{
		InitializeComponent();
		((Control)this).AllowDrop = true;
		((Control)this).SetStyle((ControlStyles)512, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)1, true);
		((ScrollableControl)this).DockPadding.All = 2;
	}

	private void InvokeChangeFocusedControl()
	{
		if (this.ChangeFocusedControl != null)
		{
			this.ChangeFocusedControl(this, EventArgs.Empty);
		}
	}

	public void RemoveFocused()
	{
		for (int i = 0; i < ((ArrangedElementCollection)((Control)this).Controls).Count; i++)
		{
			StiTableZoneControl stiTableZoneControl = ((Control)this).Controls[i] as StiTableZoneControl;
			if (!stiTableZoneControl.IsFocused)
			{
				continue;
			}
			((Control)this).Controls.Remove((Control)(object)stiTableZoneControl);
			if (((ArrangedElementCollection)((Control)this).Controls).Count != 0)
			{
				i--;
				if (i < 0)
				{
					i = 0;
				}
				if (i >= ((ArrangedElementCollection)((Control)this).Controls).Count)
				{
					i = ((ArrangedElementCollection)((Control)this).Controls).Count - 1;
				}
				((Control)this).Controls[i].Focus();
			}
			break;
		}
	}

	private void RemoveZoneControl(StiTableZoneControl zone)
	{
		if (zone == null || !zone.IsFocused)
		{
			return;
		}
		int childIndex = ((Control)this).Controls.GetChildIndex((Control)(object)zone);
		((Control)this).Controls.Remove((Control)(object)zone);
		if (((ArrangedElementCollection)((Control)this).Controls).Count != 0)
		{
			childIndex--;
			if (childIndex < 0)
			{
				childIndex = 0;
			}
			if (childIndex >= ((ArrangedElementCollection)((Control)this).Controls).Count)
			{
				childIndex = ((ArrangedElementCollection)((Control)this).Controls).Count - 1;
			}
			((Control)this).Controls[childIndex].Focus();
		}
	}

	public void MoveUpFocused()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		Control val = null;
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val2 = item;
			StiTableZoneControl stiTableZoneControl = val2 as StiTableZoneControl;
			if (stiTableZoneControl.IsFocused)
			{
				val = (Control)(object)stiTableZoneControl;
				break;
			}
		}
		if (val != null)
		{
			int num = ((Control)this).Controls.IndexOf(val);
			if (num < ((ArrangedElementCollection)((Control)this).Controls).Count - 1)
			{
				((Control)this).Controls.SetChildIndex(val, num + 1);
				val.Focus();
			}
		}
	}

	public void MoveDownFocused()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		Control val = null;
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val2 = item;
			StiTableZoneControl stiTableZoneControl = val2 as StiTableZoneControl;
			if (stiTableZoneControl.IsFocused)
			{
				val = (Control)(object)stiTableZoneControl;
				break;
			}
		}
		if (val != null)
		{
			int num = ((Control)this).Controls.IndexOf(val);
			if (num > 0)
			{
				((Control)this).Controls.SetChildIndex(val, num - 1);
				val.Focus();
			}
		}
	}

	private void MoveTo(int indexTo)
	{
		if (FocusedControl != null)
		{
			((Control)this).Controls.SetChildIndex((Control)(object)FocusedControl, indexTo);
			((Control)FocusedControl).Focus();
		}
	}

	private void RemoveDragFocus()
	{
		foreach (object item in (ArrangedElementCollection)((Control)this).Controls)
		{
			if (item is StiTableZoneControl stiTableZoneControl)
			{
				stiTableZoneControl.IsDragOver = false;
			}
		}
	}

	protected override void OnControlAdded(ControlEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		((Control)this).OnControlAdded(e);
		e.Control.GotFocus += Control_ChangeFocus;
		e.Control.LostFocus += Control_ChangeFocus;
		e.Control.MouseDown += new MouseEventHandler(Control_MouseDown);
		e.Control.MouseMove += new MouseEventHandler(Control_MouseMove);
		if (e.Control is StiTableZoneControl stiTableZoneControl)
		{
			stiTableZoneControl.Closed += ZoneControl_Closed;
		}
		e.Control.Dock = (DockStyle)1;
		((Control)this).Controls.SetChildIndex(e.Control, 0);
		e.Control.Focus();
	}

	private void ZoneControl_Closed(object sender, EventArgs e)
	{
		StiTableZoneControl zone = sender as StiTableZoneControl;
		RemoveZoneControl(zone);
	}

	protected override void OnDragOver(DragEventArgs e)
	{
		((Control)this).OnDragOver(e);
		RemoveDragFocus();
		Point point = ((Control)this).PointToClient(new Point(e.X, e.Y));
		Control childAtPoint = ((Control)this).GetChildAtPoint(new Point(point.X, point.Y));
		if (childAtPoint != null)
		{
			StiTableZoneControl stiTableZoneControl = childAtPoint as StiTableZoneControl;
			stiTableZoneControl.IsDragOver = true;
		}
		e.Effect = (DragDropEffects)2;
		int num = startPoint.Y - point.Y;
		if (num > 7)
		{
			num = 7;
		}
		if (num < -7)
		{
			num = -7;
		}
		int num2 = ((ScrollProperties)((ScrollableControl)this).VerticalScroll).Value - num;
		if (num2 < ((ScrollProperties)((ScrollableControl)this).VerticalScroll).Minimum)
		{
			num2 = ((ScrollProperties)((ScrollableControl)this).VerticalScroll).Minimum;
		}
		if (num2 > ((ScrollProperties)((ScrollableControl)this).VerticalScroll).Maximum)
		{
			num2 = ((ScrollProperties)((ScrollableControl)this).VerticalScroll).Maximum;
		}
		((ScrollProperties)((ScrollableControl)this).VerticalScroll).Value = num2;
	}

	protected override void OnDragDrop(DragEventArgs e)
	{
		((Control)this).OnDragDrop(e);
		RemoveDragFocus();
		Point point = ((Control)this).PointToClient(new Point(e.X, e.Y));
		Control childAtPoint = ((Control)this).GetChildAtPoint(new Point(point.X, point.Y));
		int indexTo = ((Control)this).Controls.IndexOf(childAtPoint);
		MoveTo(indexTo);
	}

	private void Control_MouseDown(object sender, MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		if ((int)e.Button == 1048576)
		{
			startPoint = e.Location;
			startPoint.Offset(((Control)sender).Location.X, ((Control)sender).Location.Y);
			Point location = new Point(e.X - SystemInformation.DragSize.Width / 2, e.Y - SystemInformation.DragSize.Height / 2);
			dragBoxFromMouseDown = new Rectangle(location, SystemInformation.DragSize);
		}
	}

	private void Control_MouseMove(object sender, MouseEventArgs e)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		if (sender != null && (int)e.Button == 1048576)
		{
			if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
			{
				((Control)this).DoDragDrop(sender, (DragDropEffects)2);
			}
		}
		else
		{
			dragBoxFromMouseDown = Rectangle.Empty;
		}
	}

	private void Control_ChangeFocus(object sender, EventArgs e)
	{
		InvokeChangeFocusedControl();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		components = new Container();
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
	}
}
