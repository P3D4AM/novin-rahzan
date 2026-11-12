using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[Designer(typeof(StiDockingPanelDesigner))]
[ToolboxItem(false)]
public class StiDockingPanel : Panel
{
	private StiDockingManager manager;

	private bool collapsed;

	private Rectangle resizeBounds;

	private Rectangle panelBounds;

	private Rectangle tabsBounds;

	private Rectangle titleBounds;

	private bool isSelected;

	private bool isAutoHideButtonHover;

	private bool isCloseButtonHover;

	private bool isAutoHideButtonPressed;

	private bool isCloseButtonPressed;

	private bool autoHide;

	private bool showCloseButton = true;

	private bool showHideButton = true;

	private bool allowResize = true;

	private Size collapsedSize;

	private StiDockingControl selectedControl;

	private StiFloatingForm floatingForm;

	internal bool IsResizing;

	internal const int TabHeight = 24;

	internal const int ResizeBarSize = 4;

	internal const int TabControlHeight = 20;

	internal const int TitleHeight = 20;

	internal const int AutoHideHeight = 23;

	internal const int buttonWidth = 20;

	private int floatingWidth = 200;

	private int floatingHeight = 300;

	private bool isDockingWindowActive;

	private Rectangle dockingWindowRectanle = Rectangle.Empty;

	private Rectangle stopDockingRectanle;

	private bool isDockingControl;

	private bool isFloatingMoving;

	private bool isMoving;

	private Point lastPosition;

	private Timer autoHideTimer = new Timer();

	private Image imageClose;

	private Image imageAutoHideOn;

	private Image imageAutoHideOff;

	internal Form LastDockForm;

	internal DockStyle LastDock;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool AllowDrop
	{
		get
		{
			return ((Control)this).AllowDrop;
		}
		set
		{
			((Control)this).AllowDrop = value;
		}
	}

	[Browsable(false)]
	public StiDockingManager Manager
	{
		get
		{
			return manager;
		}
		set
		{
			manager = value;
		}
	}

	[DefaultValue(1000)]
	public int AutoHideTime
	{
		get
		{
			return autoHideTimer.Interval;
		}
		set
		{
			autoHideTimer.Interval = value;
		}
	}

	internal int AutoHideBarSize
	{
		get
		{
			if (!AutoHide)
			{
				return 0;
			}
			return 23;
		}
	}

	internal int TabsBarSize
	{
		get
		{
			if (((ArrangedElementCollection)((Control)this).Controls).Count > 1 && !AutoHide)
			{
				return 24;
			}
			return 1;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Rectangle AutoHideBarBounds
	{
		get
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected I4, but got Unknown
			int num = AutoHideBarSize;
			if (!AutoHide)
			{
				num = 0;
			}
			DockStyle dock = ((Control)this).Dock;
			return (dock - 1) switch
			{
				0 => new Rectangle(0, 0, ((Control)this).Width, num), 
				1 => new Rectangle(0, ((Control)this).Height - num, ((Control)this).Width, num), 
				2 => new Rectangle(0, 0, num, ((Control)this).Height), 
				_ => new Rectangle(((Control)this).Width - num, 0, num, ((Control)this).Height), 
			};
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool Collapsed
	{
		get
		{
			return collapsed;
		}
		set
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Invalid comparison between Unknown and I4
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Invalid comparison between Unknown and I4
			collapsed = value;
			if (value)
			{
				CollapsedSize = ((Control)this).ClientSize;
				DockStyle dock = ((Control)this).Dock;
				if (dock - 1 > 1)
				{
					if (dock - 3 <= 1)
					{
						((Control)this).Width = AutoHideBarSize;
					}
				}
				else
				{
					((Control)this).Height = AutoHideBarSize;
				}
			}
			else
			{
				((Control)this).ClientSize = CollapsedSize;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal Rectangle ResizeBounds => resizeBounds;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal Rectangle PanelBounds => panelBounds;

	internal Rectangle TabsBounds => tabsBounds;

	internal Rectangle TitleBounds => titleBounds;

	internal Rectangle TextBounds
	{
		get
		{
			int num = 0;
			if (SelectedControl != null && SelectedControl.Image != null)
			{
				num = SelectedControl.Image.Width;
			}
			Rectangle result = TitleBounds;
			result.X += num + 4;
			result.Width -= 5 + num + 20;
			if (FloatingForm == null)
			{
				result.Width -= 20;
			}
			if (result.Width == 0)
			{
				return Rectangle.Empty;
			}
			return result;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal bool IsSelected
	{
		get
		{
			return isSelected;
		}
		set
		{
			isSelected = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal bool IsAutoHideButtonHover
	{
		get
		{
			return isAutoHideButtonHover;
		}
		set
		{
			isAutoHideButtonHover = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal bool IsCloseButtonHover
	{
		get
		{
			return isCloseButtonHover;
		}
		set
		{
			isCloseButtonHover = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal bool IsAutoHideButtonPressed
	{
		get
		{
			return isAutoHideButtonPressed;
		}
		set
		{
			isAutoHideButtonPressed = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal bool IsCloseButtonPressed
	{
		get
		{
			return isCloseButtonPressed;
		}
		set
		{
			isCloseButtonPressed = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool AutoHide
	{
		get
		{
			return autoHide;
		}
		set
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Invalid comparison between Unknown and I4
			autoHide = value;
			DockStyle dock = ((Control)this).Dock;
			if (dock - 3 <= 1)
			{
				((Control)this).Width = ((Control)this).Width + (AutoHide ? 21 : (-21));
			}
			SetLayout();
			((Control)this).Invalidate();
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	public bool ShowCloseButton
	{
		get
		{
			return showCloseButton;
		}
		set
		{
			showCloseButton = value;
			((Control)this).Invalidate();
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	public bool ShowHideButton
	{
		get
		{
			return showHideButton;
		}
		set
		{
			showHideButton = value;
			((Control)this).Invalidate();
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	public bool AllowResize
	{
		get
		{
			return allowResize;
		}
		set
		{
			allowResize = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal Size CollapsedSize
	{
		get
		{
			return collapsedSize;
		}
		set
		{
			collapsedSize = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiDockingControl SelectedControl
	{
		get
		{
			return selectedControl;
		}
		set
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Expected O, but got Unknown
			foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
			{
				Control val = item;
				if (val == value)
				{
					try
					{
						val.Dock = (DockStyle)5;
						val.Show();
					}
					catch
					{
						continue;
					}
					break;
				}
			}
			foreach (Control item2 in (ArrangedElementCollection)((Control)this).Controls)
			{
				Control val2 = item2;
				if (val2 != value)
				{
					val2.Hide();
				}
			}
			if (selectedControl != value)
			{
				selectedControl = value;
				((Control)this).Invalidate();
			}
			SetLayout();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal StiFloatingForm FloatingForm
	{
		get
		{
			return floatingForm;
		}
		set
		{
			floatingForm = value;
		}
	}

	private Rectangle AutoHideButtonBounds
	{
		get
		{
			Rectangle closeButtonBounds = CloseButtonBounds;
			closeButtonBounds = new Rectangle(closeButtonBounds.X, closeButtonBounds.Y, 20, closeButtonBounds.Height);
			if (ShowCloseButton)
			{
				closeButtonBounds.X -= 20;
			}
			return closeButtonBounds;
		}
	}

	private Rectangle CloseButtonBounds
	{
		get
		{
			Rectangle rectangle = TitleBounds;
			rectangle = new Rectangle(rectangle.Right - 20 - 3, rectangle.Y + 2, 20, 15);
			if (!ShowCloseButton)
			{
				rectangle.Width = 0;
			}
			return rectangle;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override AnchorStyles Anchor
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).Anchor;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).Anchor = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Cursor Cursor
	{
		get
		{
			return ((Control)this).Cursor;
		}
		set
		{
			((Control)this).Cursor = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override Color BackColor
	{
		get
		{
			return ((Control)this).BackColor;
		}
		set
		{
			((Control)this).BackColor = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override Image BackgroundImage
	{
		get
		{
			return ((Control)this).BackgroundImage;
		}
		set
		{
			((Control)this).BackgroundImage = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override Color ForeColor
	{
		get
		{
			return ((Control)this).ForeColor;
		}
		set
		{
			((Control)this).ForeColor = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override string Text
	{
		get
		{
			return ((Panel)this).Text;
		}
		set
		{
			((Panel)this).Text = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DockPaddingEdges DockPadding => ((ScrollableControl)this).DockPadding;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int FloatingWidth
	{
		get
		{
			return floatingWidth;
		}
		set
		{
			floatingWidth = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int FloatingHeight
	{
		get
		{
			return floatingHeight;
		}
		set
		{
			floatingHeight = value;
		}
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		StiColors.InitColors();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Expected O, but got Unknown
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Invalid comparison between Unknown and I4
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b5: Invalid comparison between Unknown and I4
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Invalid comparison between Unknown and I4
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Invalid comparison between Unknown and I4
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Expected O, but got Unknown
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bd: Invalid comparison between Unknown and I4
		//IL_06a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ab: Expected O, but got Unknown
		//IL_05c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c6: Invalid comparison between Unknown and I4
		//IL_06c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ce: Invalid comparison between Unknown and I4
		//IL_0621: Unknown result type (might be due to invalid IL or missing references)
		//IL_0627: Invalid comparison between Unknown and I4
		//IL_06e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e6: Invalid comparison between Unknown and I4
		//IL_06d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_062a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0630: Invalid comparison between Unknown and I4
		//IL_06e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ef: Invalid comparison between Unknown and I4
		Graphics graphics = e.Graphics;
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			if (val != SelectedControl && val.Visible)
			{
				val.Hide();
			}
		}
		if (!Collapsed && !ResizeBounds.IsEmpty)
		{
			e.Graphics.FillRectangle(StiBrushes.ContentDark, ResizeBounds);
		}
		if (!TitleBounds.IsEmpty && TitleBounds.Width != 0 && TitleBounds.Height != 0)
		{
			Color color = SystemColors.ControlText;
			if (IsSelected)
			{
				Brush activeCaptionBrush = StiBrushes.GetActiveCaptionBrush(TitleBounds, 90f);
				try
				{
					graphics.FillRectangle(activeCaptionBrush, TitleBounds);
					color = SystemColors.ActiveCaptionText;
				}
				finally
				{
					((IDisposable)activeCaptionBrush)?.Dispose();
				}
			}
			else
			{
				Brush controlBrush = StiBrushes.GetControlBrush(TitleBounds, 90f);
				try
				{
					graphics.FillRectangle(controlBrush, TitleBounds);
				}
				finally
				{
					((IDisposable)controlBrush)?.Dispose();
				}
			}
			graphics.DrawRectangle(SystemPens.ControlDark, PanelBounds.X, PanelBounds.Y, PanelBounds.Width - 1, PanelBounds.Height - 1);
			graphics.DrawRectangle(SystemPens.ControlDark, TitleBounds.X, TitleBounds.Y, TitleBounds.Width - 1, TitleBounds.Height - 1);
			int num = 0;
			if (SelectedControl != null && SelectedControl.Image != null && TitleBounds.Width > 55)
			{
				num = SelectedControl.Image.Width;
				Rectangle rectangle = new Rectangle(TitleBounds.X + 3, TitleBounds.Y + (TitleBounds.Height - SelectedControl.Image.Height) / 2, num, SelectedControl.Image.Height);
				if (num != 0)
				{
					graphics.DrawImage(SelectedControl.Image, rectangle);
				}
			}
			if (SelectedControl != null)
			{
				StringFormat val2 = new StringFormat();
				try
				{
					val2.LineAlignment = (StringAlignment)1;
					if ((int)((Control)this).RightToLeft == 1)
					{
						val2.FormatFlags = (StringFormatFlags)(val2.FormatFlags | 1);
					}
					val2.FormatFlags = (StringFormatFlags)4096;
					val2.Trimming = (StringTrimming)3;
					val2.HotkeyPrefix = (HotkeyPrefix)2;
					if ((int)((Control)this).RightToLeft == 1)
					{
						val2.FormatFlags = (StringFormatFlags)(val2.FormatFlags | 1);
					}
					Brush val3 = (Brush)new SolidBrush(color);
					try
					{
						Rectangle textBounds = TextBounds;
						if (!textBounds.IsEmpty)
						{
							graphics.DrawString(((Control)SelectedControl).Text, ((Control)this).Font, val3, (RectangleF)textBounds, val2);
						}
					}
					finally
					{
						((IDisposable)val3)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			if (FloatingForm == null && ((Control)this).Width > 48 + AutoHideBarSize && ShowHideButton)
			{
				PaintAutoHideButton(graphics);
			}
			if (ShowCloseButton)
			{
				PaintCloseButton(graphics);
			}
		}
		if (!AutoHide && (((ArrangedElementCollection)((Control)this).Controls).Count > 1 || ((Component)this).DesignMode) && TabsBounds.Width != 0 && TabsBounds.Height != 0)
		{
			graphics.FillRectangle(StiBrushes.ContentDark, TabsBounds);
			for (int num2 = ((ArrangedElementCollection)((Control)this).Controls).Count - 1; num2 >= 0; num2--)
			{
				StiDockingControl stiDockingControl = ((Control)this).Controls[num2] as StiDockingControl;
				if (stiDockingControl != SelectedControl)
				{
					DrawDockingControl(graphics, stiDockingControl);
				}
			}
			if (SelectedControl != null)
			{
				DrawDockingControl(graphics, SelectedControl);
			}
		}
		if (!AutoHide || AutoHideBarBounds.Width == 0 || AutoHideBarBounds.Height == 0)
		{
			return;
		}
		e.Graphics.FillRectangle(StiBrushes.ContentDark, AutoHideBarBounds);
		foreach (StiDockingControl item2 in (ArrangedElementCollection)((Control)this).Controls)
		{
			Rectangle autoHideControlBounds = GetAutoHideControlBounds(item2);
			if (autoHideControlBounds.Width <= 0 || autoHideControlBounds.Height <= 0)
			{
				continue;
			}
			if ((int)((Control)this).Dock == 3 || (int)((Control)this).Dock == 4)
			{
				if (autoHideControlBounds.Contains(((Control)this).PointToClient(Cursor.Position)) && !((Component)this).DesignMode)
				{
					Brush controlLightBrush = StiBrushes.GetControlLightBrush(autoHideControlBounds, 0f);
					try
					{
						graphics.FillRectangle(controlLightBrush, autoHideControlBounds);
					}
					finally
					{
						((IDisposable)controlLightBrush)?.Dispose();
					}
				}
				else
				{
					Brush controlBrush2 = StiBrushes.GetControlBrush(autoHideControlBounds, 0f);
					try
					{
						graphics.FillRectangle(controlBrush2, autoHideControlBounds);
					}
					finally
					{
						((IDisposable)controlBrush2)?.Dispose();
					}
				}
			}
			else if (autoHideControlBounds.Contains(((Control)this).PointToClient(Cursor.Position)) && !((Component)this).DesignMode)
			{
				Brush controlLightBrush2 = StiBrushes.GetControlLightBrush(autoHideControlBounds, 90f);
				try
				{
					graphics.FillRectangle(controlLightBrush2, autoHideControlBounds);
				}
				finally
				{
					((IDisposable)controlLightBrush2)?.Dispose();
				}
			}
			else
			{
				Brush controlBrush3 = StiBrushes.GetControlBrush(autoHideControlBounds, 90f);
				try
				{
					graphics.FillRectangle(controlBrush3, autoHideControlBounds);
				}
				finally
				{
					((IDisposable)controlBrush3)?.Dispose();
				}
			}
			graphics.DrawRectangle(SystemPens.ControlDark, autoHideControlBounds);
			int num3 = 0;
			if (item2.Image != null)
			{
				Rectangle rectangle2 = Rectangle.Empty;
				if ((int)((Control)this).Dock == 3 || (int)((Control)this).Dock == 4)
				{
					num3 = item2.Image.Height + 2;
					rectangle2 = new Rectangle(autoHideControlBounds.X + (autoHideControlBounds.Width - item2.Image.Width) / 2, autoHideControlBounds.Y + 2, item2.Image.Width, item2.Image.Height);
				}
				else if ((int)((Control)this).Dock == 1 || (int)((Control)this).Dock == 2)
				{
					num3 = item2.Image.Width + 2;
					rectangle2 = new Rectangle(autoHideControlBounds.X + 2, autoHideControlBounds.Y + (autoHideControlBounds.Height - item2.Image.Height) / 2, item2.Image.Width, item2.Image.Height);
				}
				graphics.DrawImage(item2.Image, rectangle2);
			}
			if (item2 != SelectedControl)
			{
				continue;
			}
			StringFormat val4 = new StringFormat();
			try
			{
				val4.Alignment = (StringAlignment)0;
				val4.LineAlignment = (StringAlignment)1;
				val4.FormatFlags = (StringFormatFlags)4096;
				if ((int)((Control)this).RightToLeft == 1)
				{
					val4.FormatFlags = (StringFormatFlags)(val4.FormatFlags | 1);
				}
				if ((int)((Control)this).Dock == 4 || (int)((Control)this).Dock == 3)
				{
					autoHideControlBounds.Y += num3;
					autoHideControlBounds.Height -= num3;
					StiControlPaint.DrawString(graphics, ((Control)item2).Text, ((Control)this).Font, SystemBrushes.ControlDarkDark, autoHideControlBounds, val4, 90f);
				}
				else
				{
					autoHideControlBounds.X += num3;
					autoHideControlBounds.Width -= num3;
					StiControlPaint.DrawString(graphics, ((Control)item2).Text, ((Control)this).Font, SystemBrushes.ControlDarkDark, autoHideControlBounds, val4, 0f);
				}
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
		}
	}

	protected override void OnDockChanged(EventArgs e)
	{
		((Control)this).OnDockChanged(e);
		SetLayout();
	}

	protected override void OnLeave(EventArgs e)
	{
		((Control)this).OnLeave(e);
		((Control)this).Invalidate();
		if (AutoHide)
		{
			autoHideTimer.Enabled = true;
		}
	}

	protected override void OnEnter(EventArgs e)
	{
		((Control)this).OnEnter(e);
		((Control)this).Invalidate();
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Invalid comparison between Unknown and I4
		((Control)this).OnMouseDown(e);
		if ((e.Button & 0x100000) > 0)
		{
			((Control)this).Focus();
			if (ShowCloseButton && CloseButtonBounds.Contains(e.X, e.Y))
			{
				IsCloseButtonPressed = true;
				InvalidateTitle();
				return;
			}
			if (ShowHideButton && FloatingForm == null && AutoHideButtonBounds.Contains(e.X, e.Y) && ((Control)this).Width > 50)
			{
				IsAutoHideButtonPressed = true;
				InvalidateTitle();
				return;
			}
			if (!AutoHide)
			{
				StiDockingControl dockingControlAt = GetDockingControlAt(e.X, e.Y);
				if (dockingControlAt != null)
				{
					SelectedControl = dockingControlAt;
					if (Manager != null && !Manager.LockDockingPanels)
					{
						isMoving = true;
					}
					stopDockingRectanle = TabsBounds;
					isDockingControl = true;
					isDockingWindowActive = false;
					dockingWindowRectanle.Location = ((Control)this).PointToScreen(((Control)this).ClientRectangle.Location);
					dockingWindowRectanle.Width = FloatingWidth;
					dockingWindowRectanle.Height = FloatingHeight;
					dockingWindowRectanle.Y = Cursor.Position.Y - 5;
					if (!dockingWindowRectanle.Contains(Cursor.Position))
					{
						dockingWindowRectanle.X = Cursor.Position.X - dockingWindowRectanle.Width / 2;
					}
					lastPosition = Cursor.Position;
					return;
				}
			}
			if (!AutoHide && TitleBounds.Contains(((Control)this).PointToClient(Cursor.Position)))
			{
				if (FloatingForm != null)
				{
					if (Manager != null && !Manager.LockDockingPanels)
					{
						isFloatingMoving = true;
					}
					isDockingWindowActive = false;
					isDockingControl = false;
				}
				else
				{
					if (Manager != null && !Manager.LockDockingPanels)
					{
						isMoving = true;
					}
					stopDockingRectanle = TitleBounds;
					isDockingControl = false;
					isDockingWindowActive = false;
					dockingWindowRectanle.Location = ((Control)this).PointToScreen(((Control)this).ClientRectangle.Location);
					dockingWindowRectanle.Width = FloatingWidth;
					dockingWindowRectanle.Height = FloatingHeight;
					if (!dockingWindowRectanle.Contains(Cursor.Position))
					{
						dockingWindowRectanle.X = Cursor.Position.X - dockingWindowRectanle.Width / 2;
					}
				}
				lastPosition = Cursor.Position;
			}
			if (ResizeBounds.Contains(e.X, e.Y) && !Collapsed && AllowResize)
			{
				StiControlResize.StartResize(this, ((Control)this).Parent.RectangleToClient(((Control)this).RectangleToScreen(ResizeBounds)), GetMinimumSize(), GetMaximumSize());
			}
			if (AutoHide)
			{
				SelectControl(e.X, e.Y);
			}
		}
		if ((e.Button & 0x200000) > 0)
		{
			if (IsResizing)
			{
				StiControlResize.StopResize(cancel: true);
			}
			isMoving = false;
			isFloatingMoving = false;
			if (isDockingWindowActive)
			{
				isDockingWindowActive = false;
				StiDockingWindow.StopDockingWindow();
			}
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		if (AutoHide)
		{
			autoHideTimer.Enabled = false;
			autoHideTimer.Enabled = true;
		}
		if (IsCloseButtonHover || IsAutoHideButtonHover)
		{
			IsCloseButtonHover = false;
			IsAutoHideButtonHover = false;
			((Control)this).Cursor = Cursors.Default;
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		if (isFloatingMoving)
		{
			int num = lastPosition.X - Cursor.Position.X;
			int num2 = lastPosition.Y - Cursor.Position.Y;
			StiFloatingForm stiFloatingForm = FloatingForm;
			((Control)stiFloatingForm).Left = ((Control)stiFloatingForm).Left - num;
			StiFloatingForm stiFloatingForm2 = FloatingForm;
			((Control)stiFloatingForm2).Top = ((Control)stiFloatingForm2).Top - num2;
			lastPosition = Cursor.Position;
			StiDockingWindow.panel = this;
			Rectangle dockRectangle = StiDockingWindow.GetDockRectangle(((Control)FloatingForm).RectangleToScreen(((Control)FloatingForm).ClientRectangle));
			if (dockRectangle != ((Control)FloatingForm).RectangleToScreen(((Control)FloatingForm).ClientRectangle))
			{
				if (!isDockingWindowActive)
				{
					isDockingWindowActive = true;
					StiDockingWindow.StartDockingWindow(this, dockRectangle);
				}
				else
				{
					StiDockingWindow.RefreshDockingWindow(dockRectangle);
				}
			}
			else if (isDockingWindowActive)
			{
				isDockingWindowActive = false;
				StiDockingWindow.StopDockingWindow();
			}
		}
		else if (isMoving)
		{
			Point pt = ((Control)this).PointToClient(Cursor.Position);
			int num3 = lastPosition.X - Cursor.Position.X;
			int num4 = lastPosition.Y - Cursor.Position.Y;
			dockingWindowRectanle.X -= num3;
			dockingWindowRectanle.Y -= num4;
			if (isDockingControl)
			{
				StiDockingControl dockingControlAt = GetDockingControlAt(e.X, e.Y);
				if (dockingControlAt != null)
				{
					int num5 = ((Control)this).Controls.IndexOf((Control)(object)dockingControlAt);
					int num6 = ((Control)this).Controls.IndexOf((Control)(object)SelectedControl);
					((Control)this).Controls.SetChildIndex((Control)(object)dockingControlAt, num6);
					((Control)this).Controls.SetChildIndex((Control)(object)SelectedControl, num5);
					((Control)this).Invalidate();
				}
			}
			if (stopDockingRectanle.Contains(pt))
			{
				if (isDockingWindowActive)
				{
					isDockingWindowActive = false;
					StiDockingWindow.StopDockingWindow();
				}
			}
			else if (!isDockingWindowActive)
			{
				isDockingWindowActive = true;
				StiDockingWindow.StartDockingWindow(this, dockingWindowRectanle);
			}
			else
			{
				StiDockingWindow.RefreshDockingWindow(dockingWindowRectanle);
			}
			lastPosition = Cursor.Position;
		}
		else
		{
			IsCloseButtonHover = false;
			IsAutoHideButtonHover = false;
			IsCloseButtonHover = CloseButtonBounds.Contains(e.X, e.Y);
			IsAutoHideButtonHover = AutoHideButtonBounds.Contains(e.X, e.Y);
			InvalidateTitle();
			if (AutoHideBarBounds.Contains(e.X, e.Y))
			{
				InvalidateAutoHideBar();
			}
			if (TabsBounds.Contains(e.X, e.Y))
			{
				InvalidateTabs();
			}
			if (IsResizing)
			{
				StiControlResize.RefreshResize();
				Cursor.Current = GetResizeCursor();
			}
			else if (ResizeBounds.Contains(e.X, e.Y) && AllowResize)
			{
				Cursor.Current = GetResizeCursor();
			}
			else
			{
				Cursor.Current = Cursors.Arrow;
			}
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		((Control)this).OnMouseUp(e);
		if ((e.Button & 0x100000) <= 0)
		{
			return;
		}
		if (isMoving && isDockingWindowActive)
		{
			isDockingWindowActive = false;
			StiDockingWindow.StopDockingWindow();
			Rectangle dockRectangle = StiDockingWindow.GetDockRectangle(dockingWindowRectanle);
			if (dockRectangle != dockingWindowRectanle)
			{
				DoDock();
			}
			else
			{
				if (isDockingControl && ((ArrangedElementCollection)((Control)this).Controls).Count > 1)
				{
					UndockControl(SelectedControl, dockingWindowRectanle, createForm: true, ((Control)this).Parent);
				}
				else
				{
					UndockPanel(dockingWindowRectanle, createForm: true);
				}
				SetLayoutAllPanels();
			}
		}
		else if (isFloatingMoving && isDockingWindowActive)
		{
			isDockingWindowActive = false;
			StiDockingWindow.StopDockingWindow();
			if (!StiDockingWindow.GetDockRectangle(dockingWindowRectanle).IsEmpty)
			{
				DoDock();
			}
		}
		if (ShowCloseButton && IsCloseButtonPressed)
		{
			IsCloseButtonPressed = false;
			InvalidateTitle();
			if (IsCloseButtonHover && !Collapsed)
			{
				if (floatingForm != null)
				{
					while (((ArrangedElementCollection)((Control)this).Controls).Count > 0)
					{
						DoClose((Control)(object)SelectedControl);
					}
				}
				else
				{
					DoClose((Control)(object)SelectedControl);
				}
				SetLayoutAllPanels();
			}
		}
		if (IsAutoHideButtonPressed)
		{
			IsAutoHideButtonPressed = false;
			InvalidateTitle();
			if (IsAutoHideButtonHover)
			{
				AutoHide = !AutoHide;
				SetLayoutAllPanels();
			}
		}
		if (IsResizing)
		{
			StiControlResize.StopResize();
			SetLayout();
		}
		isFloatingMoving = false;
		isMoving = false;
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		((Control)this).OnSizeChanged(e);
		SetLayout();
		((Control)this).Invalidate();
	}

	protected override void OnControlAdded(ControlEventArgs e)
	{
		((Control)this).OnControlAdded(e);
		if (SelectedControl == null)
		{
			SelectedControl = e.Control as StiDockingControl;
		}
		e.Control.Dock = (DockStyle)5;
		SetLayout();
		((Control)this).Invalidate();
	}

	protected override void OnControlRemoved(ControlEventArgs e)
	{
		((Control)this).OnControlRemoved(e);
		if (SelectedControl == e.Control)
		{
			if (((ArrangedElementCollection)((Control)this).Controls).Count == 0)
			{
				SelectedControl = null;
			}
			else
			{
				SelectedControl = ((Control)this).Controls[0] as StiDockingControl;
			}
		}
		SetLayout();
		((Control)this).Invalidate();
	}

	protected override void WndProc(ref Message msg)
	{
		if (((Message)(ref msg)).Msg == 675)
		{
			InvalidateTitle();
			InvalidateTabs();
			InvalidateAutoHideBar();
		}
		((ScrollableControl)this).WndProc(ref msg);
	}

	private void OnAutoHideTick(object sender, EventArgs e)
	{
		if (((Control)this).Parent != null && AutoHide && !Collapsed && !((Control)this).ContainsFocus && !((Control)this).ClientRectangle.Contains(((Control)this).PointToClient(Cursor.Position)))
		{
			autoHideTimer.Enabled = false;
			Collapsed = true;
			SetLayoutAllPanels();
		}
	}

	public void DoClose(Control control)
	{
		CancelEventArgs cancelEventArgs = new CancelEventArgs(cancel: false);
		StiDockingControl stiDockingControl = (StiDockingControl)(object)control;
		stiDockingControl.InvokeClosing(cancelEventArgs);
		if (cancelEventArgs.Cancel)
		{
			return;
		}
		if (!manager.ClosedControls.Contains(stiDockingControl))
		{
			manager.ClosedControls.Add(stiDockingControl);
		}
		if (floatingForm != null)
		{
			((Control)this).Controls.Remove(control);
			if (((ArrangedElementCollection)((Control)this).Controls).Count == 0)
			{
				manager.RemoveUndockingPanels(this);
				((Control)this).Parent.Controls.Remove((Control)(object)this);
			}
			stiDockingControl.InvokeClosed(EventArgs.Empty);
			SetLayoutAllPanels();
		}
		else if (((ArrangedElementCollection)((Control)this).Controls).Count > 1 && control != null)
		{
			((Control)this).Controls.Remove(control);
			stiDockingControl.InvokeClosed(EventArgs.Empty);
			((Control)this).Invalidate();
		}
		else
		{
			if (((Control)this).Parent != null)
			{
				((Control)this).Parent.Controls.Remove((Control)(object)this);
			}
			stiDockingControl.InvokeClosed(EventArgs.Empty);
		}
	}

	private void DoDock()
	{
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		if (isDockingControl && ((ArrangedElementCollection)((Control)this).Controls).Count > 1)
		{
			if (StiDockingWindow.ControlToDock is Form)
			{
				StiDockingControl control = SelectedControl;
				Control controlToDock = StiDockingWindow.ControlToDock;
				DockControlToForm(control, (Form)(object)((controlToDock is Form) ? controlToDock : null), StiDockingWindow.DockType);
			}
			else
			{
				DockControlToPanel(StiDockingWindow.ControlToDock as StiDockingPanel);
			}
		}
		else if (StiDockingWindow.ControlToDock is Form)
		{
			Control controlToDock2 = StiDockingWindow.ControlToDock;
			DockPanelToForm((Form)(object)((controlToDock2 is Form) ? controlToDock2 : null), StiDockingWindow.DockType);
		}
		else
		{
			DockPanelToPanel(StiDockingWindow.ControlToDock as StiDockingPanel);
		}
		SetLayoutAllPanels();
	}

	private StringFormat GetStringFormatTabs()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Invalid comparison between Unknown and I4
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		StringFormat val = new StringFormat
		{
			LineAlignment = (StringAlignment)1,
			Alignment = (StringAlignment)1,
			FormatFlags = (StringFormatFlags)4096,
			Trimming = (StringTrimming)3
		};
		if ((int)((Control)this).RightToLeft == 1)
		{
			val.FormatFlags = (StringFormatFlags)(val.FormatFlags | 1);
		}
		return val;
	}

	internal void SetLayoutAllPanels()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		if (manager == null || manager.ParentForm == null)
		{
			return;
		}
		foreach (Control item in (ArrangedElementCollection)((Control)manager.ParentForm).Controls)
		{
			Control val = item;
			if (val is StiDockingPanel)
			{
				((StiDockingPanel)(object)val).SetLayout();
			}
		}
		foreach (StiDockingPanel undockedPanel in manager.UndockedPanels)
		{
			undockedPanel.SetLayout();
		}
	}

	internal void SetLayout()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected I4, but got Unknown
		try
		{
			if (Manager == null || Manager.LockLayout)
			{
				return;
			}
			((Control)this).Refresh();
			DockStyle dock = ((Control)this).Dock;
			switch (dock - 1)
			{
			case 0:
				DockPadding.Left = 1;
				DockPadding.Right = 1;
				DockPadding.Top = AutoHideBarSize + 20;
				DockPadding.Bottom = 4 + TabsBarSize;
				resizeBounds = new Rectangle(0, ((Control)this).Height - 4, ((Control)this).Width, 4);
				titleBounds = new Rectangle(0, AutoHideBarSize, ((Control)this).Width, 20);
				panelBounds = new Rectangle(0, AutoHideBarSize, ((Control)this).Width, ((Control)this).Height - 4 - AutoHideBarSize);
				tabsBounds = new Rectangle(0, ((Control)this).Height - TabsBarSize - 4, ((Control)this).Width - AutoHideBarSize, TabsBarSize);
				break;
			case 1:
				DockPadding.Left = 1;
				DockPadding.Right = 1;
				DockPadding.Top = 24;
				DockPadding.Bottom = AutoHideBarSize + TabsBarSize;
				resizeBounds = new Rectangle(0, 0, ((Control)this).Width, 4);
				titleBounds = new Rectangle(0, 4, ((Control)this).Width, 20);
				panelBounds = new Rectangle(0, 4, ((Control)this).Width, ((Control)this).Height - 4 - AutoHideBarSize);
				tabsBounds = new Rectangle(0, ((Control)this).Height - TabsBarSize, ((Control)this).Width - AutoHideBarSize, TabsBarSize);
				break;
			case 2:
				DockPadding.Left = AutoHideBarSize + 1;
				DockPadding.Right = 5;
				DockPadding.Top = 20;
				DockPadding.Bottom = TabsBarSize;
				resizeBounds = new Rectangle(((Control)this).Width - 4, 0, 4, ((Control)this).Height);
				titleBounds = new Rectangle(AutoHideBarSize, 0, ((Control)this).Width - 4 - AutoHideBarSize, 20);
				panelBounds = new Rectangle(AutoHideBarSize, 0, ((Control)this).Width - 4 - AutoHideBarSize, ((Control)this).Height);
				tabsBounds = new Rectangle(0, ((Control)this).Height - TabsBarSize, ((Control)this).Width - 4 - AutoHideBarSize, TabsBarSize);
				break;
			case 3:
				DockPadding.Left = 5;
				DockPadding.Right = AutoHideBarSize + 1;
				DockPadding.Top = 20;
				DockPadding.Bottom = TabsBarSize;
				resizeBounds = new Rectangle(0, 0, 4, ((Control)this).Height);
				titleBounds = new Rectangle(4, 0, ((Control)this).Width - 4 - AutoHideBarSize, 20);
				panelBounds = new Rectangle(4, 0, ((Control)this).Width - 4 - AutoHideBarSize, ((Control)this).Height);
				tabsBounds = new Rectangle(4, ((Control)this).Height - TabsBarSize, ((Control)this).Width - 4 - AutoHideBarSize, TabsBarSize);
				break;
			case 4:
				DockPadding.Left = 1;
				DockPadding.Right = 1;
				DockPadding.Top = 20;
				DockPadding.Bottom = TabsBarSize;
				resizeBounds = new Rectangle(0, 0, 0, ((Control)this).Height);
				titleBounds = new Rectangle(0, 0, ((Control)this).Width, 20);
				panelBounds = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
				tabsBounds = new Rectangle(0, ((Control)this).Height - TabsBarSize, ((Control)this).Width, TabsBarSize);
				break;
			}
			foreach (StiDockingControl item in (ArrangedElementCollection)((Control)this).Controls)
			{
				if (((Control)item).Visible && SelectedControl != item)
				{
					((Control)item).Visible = false;
					selectedControl = SelectedControl;
					((Control)selectedControl).Visible = true;
					break;
				}
			}
			((Control)this).UpdateStyles();
		}
		catch
		{
		}
	}

	private int GetMinimumSize()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected I4, but got Unknown
		DockStyle dock = ((Control)this).Dock;
		switch (dock - 2)
		{
		case 2:
			if (!AutoHide)
			{
				return ((Control)this).Bounds.Right - 400;
			}
			return ((Control)this).Bounds.Right - AutoHideBarSize - 400;
		case 0:
			if (!AutoHide)
			{
				return ((Control)this).Bounds.Bottom - 400;
			}
			return ((Control)this).Bounds.Bottom - AutoHideBarSize - 400;
		case 1:
			if (!AutoHide)
			{
				return ((Control)this).Left + 30;
			}
			return ((Control)this).Left + AutoHideBarSize + 30;
		default:
			if (!AutoHide)
			{
				return ((Control)this).Top + 50;
			}
			return ((Control)this).Top + AutoHideBarSize + 50;
		}
	}

	private int GetMaximumSize()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected I4, but got Unknown
		DockStyle dock = ((Control)this).Dock;
		switch (dock - 2)
		{
		case 2:
			if (!AutoHide)
			{
				return ((Control)this).Bounds.Right - 34;
			}
			return ((Control)this).Bounds.Right - AutoHideBarSize - 34;
		case 0:
			if (!AutoHide)
			{
				return ((Control)this).Bounds.Bottom - 54;
			}
			return ((Control)this).Bounds.Bottom - AutoHideBarSize - 54;
		case 1:
			if (!AutoHide)
			{
				return 400 + ((Control)this).Left;
			}
			return AutoHideBarSize + 400 + ((Control)this).Left;
		default:
			if (!AutoHide)
			{
				return 400 + ((Control)this).Top;
			}
			return AutoHideBarSize + 400 + ((Control)this).Top;
		}
	}

	private void InvalidateAutoHideBar()
	{
		if (AutoHideBarBounds.Width != 0 && AutoHideBarBounds.Height != 0)
		{
			((Control)this).Invalidate(AutoHideBarBounds);
		}
	}

	internal void InvalidateTitle()
	{
		if (TitleBounds.Width != 0 && TitleBounds.Height != 0)
		{
			((Control)this).Invalidate(TitleBounds);
		}
	}

	internal void InvalidateTabs()
	{
		if (TabsBounds.Width != 0 && TabsBounds.Height != 0)
		{
			((Control)this).Invalidate(TabsBounds);
		}
	}

	internal Cursor GetResizeCursor()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Invalid comparison between Unknown and I4
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Invalid comparison between Unknown and I4
		if (!Collapsed)
		{
			DockStyle dock = ((Control)this).Dock;
			if (dock - 1 <= 1)
			{
				return Cursors.HSplit;
			}
			if (dock - 3 <= 1)
			{
				return Cursors.VSplit;
			}
		}
		return Cursors.Arrow;
	}

	private StiDockingControl GetControlAt(int x, int y)
	{
		if (AutoHide)
		{
			foreach (StiDockingControl item in (ArrangedElementCollection)((Control)this).Controls)
			{
				if (GetAutoHideControlBounds(item).Contains(x, y))
				{
					return item;
				}
			}
		}
		return null;
	}

	private void SelectControl(int x, int y)
	{
		StiDockingControl controlAt = GetControlAt(x, y);
		if (controlAt != null)
		{
			if (Collapsed)
			{
				Collapsed = false;
			}
			SelectedControl = controlAt;
			SetLayoutAllPanels();
		}
	}

	internal void DockPanel(Form form)
	{
		((Control)form).Controls.Add((Control)(object)this);
		Manager.RemoveUndockingPanels(this);
		SetLayout();
		((Control)this).Focus();
		LastDockForm = null;
	}

	public void DockControlToForm(StiDockingControl control, Form form, DockStyle dockType)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		StiDockingPanel stiDockingPanel = UndockControl(control, Rectangle.Empty, createForm: false, null);
		stiDockingPanel.DockPanelToForm(form, dockType);
		((Control)stiDockingPanel).Focus();
	}

	public void DockControlToPanel(StiDockingPanel panel)
	{
		StiDockingPanel stiDockingPanel = UndockControl(SelectedControl, Rectangle.Empty, createForm: false, ((Control)this).Parent);
		stiDockingPanel.DockPanelToPanel(panel);
	}

	public void DockPanelToPanel(StiDockingPanel panel)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		List<Control> list = new List<Control>();
		foreach (Control item2 in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control item = item2;
			list.Add(item);
		}
		((Control)panel).Controls.AddRange(list.ToArray());
		if (((Control)this).Parent != null)
		{
			((Control)this).Parent.Controls.Remove((Control)(object)this);
		}
		panel.SetLayout();
		if (FloatingForm != null)
		{
			panel.manager.RemoveUndockingPanels(this);
		}
		((Control)this).Focus();
	}

	public void DockPanelToForm(Form form, DockStyle dockType)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Invalid comparison between Unknown and I4
		StiDockingPanel stiDockingPanel = null;
		foreach (Control item in (ArrangedElementCollection)((Control)form).Controls)
		{
			Control val = item;
			if (val is StiDockingPanel && val.Dock == dockType)
			{
				stiDockingPanel = val as StiDockingPanel;
				break;
			}
		}
		if (stiDockingPanel == null)
		{
			StiDockingPanel stiDockingPanel2 = new StiDockingPanel(Manager);
			((Control)stiDockingPanel2).Width = FloatingWidth;
			((Control)stiDockingPanel2).Height = FloatingHeight / 2;
			((Control)stiDockingPanel2).Dock = dockType;
			stiDockingPanel2.LastDockForm = null;
			stiDockingPanel = stiDockingPanel2;
			((Control)form).Controls.Add((Control)(object)stiDockingPanel);
			((Control)form).Controls.SetChildIndex((Control)(object)stiDockingPanel, 0);
			foreach (Control item2 in (ArrangedElementCollection)((Control)form).Controls)
			{
				Control val2 = item2;
				if ((int)val2.Dock == 5)
				{
					((Control)form).Controls.SetChildIndex(val2, 0);
					break;
				}
			}
		}
		DockPanelToPanel(stiDockingPanel);
	}

	public void UndockPanel(Rectangle rect, bool createForm)
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		rect.Size = new Size(FloatingWidth, FloatingHeight);
		if (createForm)
		{
			Manager.AddUndockingPanels(this);
			StiFloatingForm stiFloatingForm = new StiFloatingForm(this);
			((ContainerControl)stiFloatingForm).AutoScaleMode = (AutoScaleMode)0;
			StiFloatingForm stiFloatingForm2 = stiFloatingForm;
			if (LastDockForm == null)
			{
				if (((Control)this).Parent != null && Manager.ShowingUndocked)
				{
					((Form)((Control)this).Parent).AddOwnedForm((Form)(object)stiFloatingForm2);
				}
				ref Form lastDockForm = ref LastDockForm;
				Control parent = ((Control)this).Parent;
				lastDockForm = (Form)(object)((parent is Form) ? parent : null);
				LastDock = ((Control)this).Dock;
			}
			else if (Manager.ShowingUndocked)
			{
				LastDockForm.AddOwnedForm((Form)(object)stiFloatingForm2);
			}
			if (rect.Left == -1)
			{
				((Form)stiFloatingForm2).StartPosition = (FormStartPosition)1;
				((Form)stiFloatingForm2).ClientSize = new Size(rect.Width, rect.Height);
			}
			else
			{
				((Form)stiFloatingForm2).Location = rect.Location;
				((Form)stiFloatingForm2).ClientSize = new Size(rect.Width, rect.Height);
			}
			((Control)stiFloatingForm2).Controls.Add((Control)(object)this);
			if (Manager.ShowingUndocked)
			{
				((Control)stiFloatingForm2).Show();
			}
		}
		SetLayout();
		((Control)this).Focus();
	}

	public StiDockingPanel UndockControl(StiDockingControl control, Rectangle rect, bool createForm, Control parent)
	{
		StiDockingPanel obj = new StiDockingPanel(Manager)
		{
			LastDockForm = LastDockForm
		};
		((Control)obj).Parent = parent;
		StiDockingPanel stiDockingPanel = obj;
		((Control)stiDockingPanel).Controls.Add((Control)(object)control);
		stiDockingPanel.UndockPanel(rect, createForm);
		if (((ArrangedElementCollection)((Control)this).Controls).Count == 0 && ((Control)this).Parent != null)
		{
			((Control)this).Parent.Controls.Remove((Control)(object)this);
		}
		return stiDockingPanel;
	}

	private void PaintButton(Graphics g, Rectangle rect, Image image, bool isHover, bool isPressed)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		if (isHover || isPressed)
		{
			Color color = StiColors.Focus;
			if (isPressed)
			{
				color = Color.FromArgb(100, color);
			}
			SolidBrush val = new SolidBrush(color);
			try
			{
				g.FillRectangle((Brush)(object)val, rect);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			g.DrawRectangle(StiPens.SelectedText, rect);
		}
		if (image != null)
		{
			if (IsSelected)
			{
				image = (Image)(object)StiImageUtils.ReplaceImageColor((Bitmap)image, SystemColors.ActiveCaptionText, Color.Black);
			}
			g.DrawImage(image, new Rectangle(rect.X + (rect.Width - image.Width) / 2 + 1, rect.Y + (rect.Height - image.Height) / 2, image.Width, image.Height));
		}
	}

	private void PaintCloseButton(Graphics g)
	{
		if (imageClose != null)
		{
			PaintButton(g, CloseButtonBounds, imageClose, IsCloseButtonHover, IsCloseButtonPressed);
		}
	}

	private void PaintAutoHideButton(Graphics g)
	{
		if (imageAutoHideOn != null && imageAutoHideOff != null)
		{
			if (AutoHide)
			{
				PaintButton(g, AutoHideButtonBounds, imageAutoHideOn, IsAutoHideButtonHover, IsAutoHideButtonPressed);
			}
			else
			{
				PaintButton(g, AutoHideButtonBounds, imageAutoHideOff, IsAutoHideButtonHover, IsAutoHideButtonPressed);
			}
		}
	}

	private int GetTabWidth(Graphics g, StiDockingControl control)
	{
		int num = 0;
		if (control.Image != null)
		{
			num = control.Image.Width;
		}
		StringFormat stringFormatTabs = GetStringFormatTabs();
		try
		{
			return (int)g.MeasureString(((Control)control).Text, ((Control)this).Font, 1000, stringFormatTabs).Width + 10 + num;
		}
		finally
		{
			((IDisposable)stringFormatTabs)?.Dispose();
		}
	}

	private int GetTotalWidth(Graphics g)
	{
		int num = 0;
		foreach (StiDockingControl item in (ArrangedElementCollection)((Control)this).Controls)
		{
			num += GetTabWidth(g, item);
		}
		return num;
	}

	internal Rectangle GetAutoHideControlBounds(StiDockingControl dockingControl)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Invalid comparison between Unknown and I4
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Invalid comparison between Unknown and I4
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected I4, but got Unknown
		Graphics val = Graphics.FromHwnd(((Control)this).Handle);
		Rectangle result;
		try
		{
			int num = 0;
			int size = 0;
			DockStyle dock = ((Control)this).Dock;
			if (dock - 1 > 1)
			{
				if (dock - 3 <= 1)
				{
					result = AutoHideBarBounds;
					size = result.Height;
				}
			}
			else
			{
				result = AutoHideBarBounds;
				size = result.Width;
			}
			foreach (StiDockingControl item in (ArrangedElementCollection)((Control)this).Controls)
			{
				Rectangle tabRectangle = GetTabRectangle(val, item, size);
				tabRectangle.Width -= 10;
				if (item != SelectedControl)
				{
					tabRectangle.Width = 16;
				}
				if (item == dockingControl)
				{
					DockStyle dock2 = ((Control)this).Dock;
					switch (dock2 - 1)
					{
					case 2:
						result = new Rectangle(-1, num, 20, tabRectangle.Width + 5);
						goto end_IL_000c;
					case 3:
						result = new Rectangle(((Control)this).Width - 20, num, 20, tabRectangle.Width + 5);
						goto end_IL_000c;
					case 0:
						result = new Rectangle(num, -1, tabRectangle.Width + 5, 20);
						goto end_IL_000c;
					case 1:
						result = new Rectangle(num, ((Control)this).Height - 20, tabRectangle.Width + 5, 20);
						goto end_IL_000c;
					}
				}
				num += tabRectangle.Width + 8;
			}
			result = Rectangle.Empty;
			end_IL_000c:;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		return result;
	}

	internal Rectangle GetTabRectangle(Graphics g, StiDockingControl dockingControl)
	{
		return GetTabRectangle(g, dockingControl, 0);
	}

	internal Rectangle GetTabRectangle(Graphics g, StiDockingControl dockingControl, int size)
	{
		int num = 5 + TabsBounds.X;
		int width = 10;
		int totalWidth = GetTotalWidth(g);
		if (size == 0)
		{
			size = TabsBounds.Width;
		}
		double num2 = 1.0;
		if (totalWidth + num * 2 + 5 > size)
		{
			num2 = (double)size / ((double)totalWidth + (double)num * 2.0 + 5.0);
		}
		foreach (StiDockingControl item in (ArrangedElementCollection)((Control)this).Controls)
		{
			if (item != dockingControl)
			{
				num += (int)((double)GetTabWidth(g, item) * num2);
				continue;
			}
			width = (int)((double)GetTabWidth(g, item) * num2);
			break;
		}
		Rectangle result = new Rectangle(num, TabsBounds.Y, width, 20);
		return result;
	}

	private void DrawDockingControl(Graphics g, StiDockingControl dockingControl)
	{
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Expected O, but got Unknown
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Expected O, but got Unknown
		Rectangle tabRectangle = GetTabRectangle(g, dockingControl);
		Point[] array = new Point[4]
		{
			new Point(tabRectangle.X, tabRectangle.Y),
			new Point(tabRectangle.X, tabRectangle.Bottom),
			new Point(tabRectangle.Right - 5, tabRectangle.Bottom),
			new Point(tabRectangle.Right + 5, tabRectangle.Y)
		};
		GraphicsPath val = new GraphicsPath((FillMode)0);
		try
		{
			val.AddPolygon(array);
			Point pt = ((Control)this).PointToClient(Cursor.Position);
			if (SelectedControl == dockingControl)
			{
				if (tabRectangle.Contains(pt))
				{
					Brush controlLightBrush = StiBrushes.GetControlLightBrush(tabRectangle, 90f);
					try
					{
						g.FillPath(controlLightBrush, val);
					}
					finally
					{
						((IDisposable)controlLightBrush)?.Dispose();
					}
				}
				else
				{
					Brush controlBrush = StiBrushes.GetControlBrush(tabRectangle, 90f);
					try
					{
						g.FillPath(controlBrush, val);
					}
					finally
					{
						((IDisposable)controlBrush)?.Dispose();
					}
				}
			}
			else if (tabRectangle.Contains(pt))
			{
				SolidBrush val2 = new SolidBrush(StiColorUtils.Light(StiColors.Content, 15));
				try
				{
					g.FillPath((Brush)(object)val2, val);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			else
			{
				g.FillPath(StiBrushes.Content, val);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		Color color = ((Control)this).ForeColor;
		if (dockingControl != SelectedControl)
		{
			color = SystemColors.ControlDark;
		}
		int num = 0;
		if (dockingControl.Image != null)
		{
			num = dockingControl.Image.Width + 2;
			Rectangle rectangle = new Rectangle(tabRectangle.X + 2, tabRectangle.Y + (tabRectangle.Height - dockingControl.Image.Height) / 2, dockingControl.Image.Width, dockingControl.Image.Height);
			if (num != 0)
			{
				g.DrawImage(dockingControl.Image, rectangle);
			}
		}
		Rectangle rectangle2 = new Rectangle(tabRectangle.X + num, tabRectangle.Y, tabRectangle.Width - num, tabRectangle.Height);
		SolidBrush val3 = new SolidBrush(color);
		try
		{
			StringFormat stringFormatTabs = GetStringFormatTabs();
			try
			{
				g.DrawString(((Control)dockingControl).Text, ((Control)this).Font, (Brush)(object)val3, (RectangleF)rectangle2, stringFormatTabs);
			}
			finally
			{
				((IDisposable)stringFormatTabs)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		if (SelectedControl == dockingControl)
		{
			g.DrawLine(SystemPens.ControlLightLight, array[0], array[1]);
			g.DrawLine(SystemPens.ControlDark, array[1], array[2]);
			g.DrawLine(SystemPens.ControlDark, array[2], array[3]);
			g.DrawLine(SystemPens.ControlDark, TabsBounds.X, array[0].Y, array[0].X, array[0].Y);
			g.DrawLine(SystemPens.ControlDark, array[3].X, array[0].Y, TabsBounds.Right - 1, array[0].Y);
			return;
		}
		Pen val4 = new Pen(StiColorUtils.Dark(SystemColors.Control, 40));
		try
		{
			g.DrawLine(val4, array[0], array[1]);
			g.DrawLine(val4, array[1], array[2]);
			g.DrawLine(val4, array[2], array[3]);
		}
		finally
		{
			((IDisposable)val4)?.Dispose();
		}
	}

	public StiDockingControl GetDockingControlAt(int x, int y)
	{
		Graphics val = Graphics.FromHwnd(((Control)this).Handle);
		try
		{
			foreach (StiDockingControl item in (ArrangedElementCollection)((Control)this).Controls)
			{
				if (GetTabRectangle(val, item).Contains(x, y))
				{
					return item;
				}
			}
			return null;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (imageClose != null)
			{
				imageClose.Dispose();
			}
			if (imageAutoHideOn != null)
			{
				imageAutoHideOn.Dispose();
			}
			if (imageAutoHideOff != null)
			{
				imageAutoHideOff.Dispose();
			}
		}
		((Control)this).Dispose(disposing);
	}

	public StiDockingPanel()
		: this(null)
	{
	}

	public StiDockingPanel(StiDockingManager manager)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		this.manager = manager;
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)65536, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).Dock = (DockStyle)4;
		AllowDrop = true;
		autoHideTimer.Interval = 1000;
		autoHideTimer.Tick += OnAutoHideTick;
		imageClose = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Controls", "Stimulsoft.Controls.Bmp.Close.bmp");
		imageAutoHideOn = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Controls", "Stimulsoft.Controls.Bmp.AutoHideOn.bmp");
		imageAutoHideOff = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Controls", "Stimulsoft.Controls.Bmp.AutoHideOff.bmp");
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)65536, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		SetLayout();
	}
}
