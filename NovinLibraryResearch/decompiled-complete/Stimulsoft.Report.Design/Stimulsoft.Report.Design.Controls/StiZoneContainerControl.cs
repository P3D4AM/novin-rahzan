using System;
using System.ComponentModel;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Report.Design.Controls;

[ToolboxItem(false)]
public class StiZoneContainerControl : UserControl
{
	internal static class NativeMethods
	{
		public const int WS_EX_CONTROLPARENT = 65536;

		public const int WS_EX_CLIENTEDGE = 512;

		public const int WS_BORDER = 8388608;
	}

	private Container components;

	private Point startPoint;

	private Rectangle dragBoxFromMouseDown;

	[Browsable(false)]
	private Image emptyImage;

	protected override CreateParams CreateParams
	{
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		get
		{
			CreateParams createParams = ((UserControl)this).CreateParams;
			if (ShowBorder)
			{
				createParams.ExStyle |= 0x10000;
				createParams.ExStyle &= -513;
				createParams.Style &= -8388609;
				createParams.Style |= 0x800000;
			}
			return createParams;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color ForeColor
	{
		get
		{
			if (!StiUX.IsDark)
			{
				return ((Control)this).ForeColor;
			}
			return StiUX.InputForeground;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((Control)this).ForeColor = value;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color BackColor
	{
		get
		{
			if (!StiUX.IsDark)
			{
				return ((Control)this).BackColor;
			}
			return StiUX.BackgroundDark;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((Control)this).BackColor = value;
			}
		}
	}

	public bool ShowBorder { get; set; } = true;


	public StiZoneControl FocusedControl
	{
		get
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
			{
				Control val = item;
				if (val is StiZoneControl { IsFocused: not false } stiZoneControl)
				{
					return stiZoneControl;
				}
			}
			return null;
		}
	}

	public Image EmptyImage
	{
		get
		{
			return emptyImage;
		}
		set
		{
			emptyImage = (Image)(object)ImageHelper.CreateGrayScaleImage(value);
		}
	}

	public string EmptyText { get; set; }

	public event EventHandler ChangeFocusedControl;

	public StiZoneContainerControl()
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
		((Control)this).SuspendLayout();
		((ScrollableControl)this).AutoScroll = true;
		((Control)this).Name = "StiZoneContainerControl";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Size = new Size(528, 576);
		((Control)this).ResumeLayout(false);
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
			if (!(((Control)this).Controls[i] is StiZoneControl { IsFocused: not false } stiZoneControl))
			{
				continue;
			}
			((Control)this).Controls.Remove((Control)(object)stiZoneControl);
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

	private void RemoveZoneControl(StiZoneControl zone)
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
		if (FocusedControl != null)
		{
			int num = ((Control)this).Controls.IndexOf((Control)(object)FocusedControl);
			if (num < ((ArrangedElementCollection)((Control)this).Controls).Count - 1)
			{
				((Control)this).Controls.SetChildIndex((Control)(object)FocusedControl, num + 1);
				((Control)FocusedControl).Focus();
			}
		}
	}

	public void MoveDownFocused()
	{
		if (FocusedControl != null)
		{
			int num = ((Control)this).Controls.IndexOf((Control)(object)FocusedControl);
			if (num > 0)
			{
				((Control)this).Controls.SetChildIndex((Control)(object)FocusedControl, num - 1);
				((Control)FocusedControl).Focus();
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
			if (item is StiZoneControl stiZoneControl)
			{
				stiZoneControl.IsDragOver = false;
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
		if (e.Control is StiZoneControl stiZoneControl)
		{
			stiZoneControl.Closed += ZoneControl_Closed;
		}
		e.Control.Dock = (DockStyle)1;
		((Control)this).Controls.SetChildIndex(e.Control, 0);
		e.Control.Focus();
		((Control)this).Invalidate();
	}

	private void ZoneControl_Closed(object sender, EventArgs e)
	{
		StiZoneControl zone = sender as StiZoneControl;
		RemoveZoneControl(zone);
		if (((ArrangedElementCollection)((Control)this).Controls).Count == 0)
		{
			((Control)this).Invalidate();
		}
	}

	protected override void OnDragOver(DragEventArgs e)
	{
		((Control)this).OnDragOver(e);
		RemoveDragFocus();
		Point point = ((Control)this).PointToClient(new Point(e.X, e.Y));
		Control childAtPoint = ((Control)this).GetChildAtPoint(new Point(point.X, point.Y));
		if (childAtPoint != null)
		{
			StiZoneControl stiZoneControl = childAtPoint as StiZoneControl;
			stiZoneControl.IsDragOver = true;
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

	protected override void OnPaint(PaintEventArgs p)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Expected O, but got Unknown
		((Control)this).OnPaint(p);
		Graphics graphics = p.Graphics;
		Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		if (((ArrangedElementCollection)((Control)this).Controls).Count == 0 && EmptyImage != null && !string.IsNullOrEmpty(EmptyText))
		{
			Font val = new Font("Arial", 8f);
			Size size = TextRenderer.MeasureText(EmptyText, val);
			Rectangle rectangle2 = new Rectangle(rectangle.X + (rectangle.Width - EmptyImage.Width) / 2, rectangle.Y + (rectangle.Height - EmptyImage.Height - size.Height) / 2, EmptyImage.Width, EmptyImage.Height);
			graphics.DrawImage(EmptyImage, rectangle2);
			SolidBrush val2 = new SolidBrush(StiUX.InputForegroundHint);
			try
			{
				graphics.DrawString(EmptyText, val, (Brush)(object)val2, (PointF)new Point(rectangle.X + (rectangle.Width - size.Width) / 2, rectangle2.Bottom + size.Height / 2));
				return;
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		if (StiUX.IsDark)
		{
			StiDrawing.FillRectangle(graphics, StiUX.BackgroundDark, rectangle);
		}
		else
		{
			graphics.FillRectangle(Brushes.White, rectangle);
		}
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
}
