using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
[Designer(typeof(StiOutlookPanelDesigner))]
public class StiOutlookPanel : Panel
{
	private Bitmap upBitmap;

	private Bitmap downBitmap;

	private bool isMouseOver;

	private bool drawBorder = true;

	private Pen borderPen = new Pen(SystemColors.ControlDark);

	private int panelDistance = 5;

	private int collapsedHeight = 20;

	private Image headerBackgroundImage;

	private int headerHeight = 20;

	private Font titleFont = new Font("Arial", 8f);

	private Color titleColor = Color.Empty;

	private Image image;

	private bool selected;

	private bool collapsed = true;

	private Color headerStartColor;

	private Color headerEndColor;

	private Color selectedHeaderStartColor;

	private Color selectedHeaderEndColor;

	[Browsable(false)]
	public override DockStyle Dock
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).Dock;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).Dock = value;
		}
	}

	[Browsable(false)]
	public BorderStyle BorderStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Panel)this).BorderStyle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Panel)this).BorderStyle = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DockPaddingEdges DockPadding => ((ScrollableControl)this).DockPadding;

	[DefaultValue(true)]
	[Category("Appearance")]
	public bool DrawBorder
	{
		get
		{
			return drawBorder;
		}
		set
		{
			drawBorder = value;
			((Control)this).Invalidate();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Pen BorderPen
	{
		get
		{
			return borderPen;
		}
		set
		{
			borderPen = value;
			((Control)this).Invalidate();
		}
	}

	[Category("Behavior")]
	public int PanelDistance
	{
		get
		{
			return panelDistance;
		}
		set
		{
			panelDistance = value;
			SetLayout();
			((Control)this).Invalidate();
		}
	}

	[Browsable(false)]
	public virtual int CollapsedHeight
	{
		get
		{
			return collapsedHeight;
		}
		set
		{
			collapsedHeight = value;
			SetLayout();
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(null)]
	[Category("Appearance")]
	public virtual Image HeaderBackgroundImage
	{
		get
		{
			return headerBackgroundImage;
		}
		set
		{
			headerBackgroundImage = value;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(20)]
	[Category("Appearance")]
	public virtual int HeaderHeight
	{
		get
		{
			return headerHeight;
		}
		set
		{
			headerHeight = value;
			SetLayout();
			((Control)this).Invalidate();
		}
	}

	[Category("Appearance")]
	public virtual Font TitleFont
	{
		get
		{
			return titleFont;
		}
		set
		{
			if (titleFont != value)
			{
				titleFont = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Category("Appearance")]
	public virtual Color TitleColor
	{
		get
		{
			return titleColor;
		}
		set
		{
			if (titleColor != value)
			{
				titleColor = value;
				((Control)this).Invalidate();
			}
		}
	}

	[DefaultValue(null)]
	[Category("Appearance")]
	public virtual Image Image
	{
		get
		{
			return image;
		}
		set
		{
			if (value is Bitmap)
			{
				StiImageUtils.MakeImageBackgroundAlphaZero((Bitmap)(object)((value is Bitmap) ? value : null));
			}
			image = value;
			((Control)this).Invalidate();
		}
	}

	[Category("Behavior")]
	[Browsable(true)]
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

	[Category("Behavior")]
	[DefaultValue(false)]
	public bool Selected
	{
		get
		{
			return selected;
		}
		set
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Expected O, but got Unknown
			selected = value;
			if (value && ((Control)this).Parent != null)
			{
				foreach (Control item in (ArrangedElementCollection)((Control)this).Parent.Controls)
				{
					Control val = item;
					if (val is StiOutlookPanel && val != this && ((StiOutlookPanel)(object)val).Selected)
					{
						((StiOutlookPanel)(object)val).Selected = false;
						((Control)(StiOutlookPanel)(object)val).Invalidate();
					}
				}
			}
			((Control)this).Invalidate();
		}
	}

	[Category("Behavior")]
	[Browsable(false)]
	public bool Collapsed
	{
		get
		{
			return collapsed;
		}
		set
		{
			if (collapsed != value)
			{
				if (!value)
				{
					CollapsedHeight = ((Control)this).Height;
					((Control)this).Height = HeaderHeight + PanelDistance;
				}
				else
				{
					((Control)this).Height = CollapsedHeight;
				}
			}
			collapsed = value;
			SetLayout();
			((Control)this).Invalidate();
		}
	}

	[Category("Behavior")]
	[DefaultValue(true)]
	public override bool AutoSize
	{
		get
		{
			return ((Panel)this).AutoSize;
		}
		set
		{
			((Panel)this).AutoSize = value;
			SetLayout();
			((Control)this).Invalidate();
		}
	}

	[Category("Behavior")]
	[Browsable(true)]
	public Color HeaderStartColor
	{
		get
		{
			return headerStartColor;
		}
		set
		{
			headerStartColor = value;
			((Control)this).Invalidate();
		}
	}

	[Category("Behavior")]
	[Browsable(true)]
	public Color HeaderEndColor
	{
		get
		{
			return headerEndColor;
		}
		set
		{
			headerEndColor = value;
			((Control)this).Invalidate();
		}
	}

	[Category("Behavior")]
	[Browsable(true)]
	public Color SelectedHeaderStartColor
	{
		get
		{
			return selectedHeaderStartColor;
		}
		set
		{
			selectedHeaderStartColor = value;
			((Control)this).Invalidate();
		}
	}

	[Category("Behavior")]
	[Browsable(true)]
	public Color SelectedHeaderEndColor
	{
		get
		{
			return selectedHeaderEndColor;
		}
		set
		{
			selectedHeaderEndColor = value;
			((Control)this).Invalidate();
		}
	}

	private void CheckHeight()
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		if (!Collapsed)
		{
			((Control)this).Height = HeaderHeight + 2;
			return;
		}
		int num = HeaderHeight + 50;
		if (((ArrangedElementCollection)((Control)this).Controls).Count > 0)
		{
			num = HeaderHeight + 10;
			foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
			{
				Control val = item;
				num = Math.Max(val.Bottom + 10, num);
			}
		}
		((Control)this).Height = num;
	}

	private void SetLayout()
	{
		try
		{
			if (((Control)this).AutoSize)
			{
				CheckHeight();
			}
			if (!Collapsed)
			{
				DockPadding.Bottom = 1000;
			}
			else
			{
				DockPadding.Bottom = PanelDistance + 1;
			}
			DockPadding.Top = HeaderHeight + 2;
		}
		catch
		{
		}
	}

	private void CheckMouseOver()
	{
		bool flag = ((Control)this).RectangleToScreen(GetHeaderRect()).Contains(Cursor.Position);
		if (flag != isMouseOver)
		{
			isMouseOver = flag;
			((Control)this).Invalidate(GetHeaderRect());
		}
	}

	private Rectangle GetHeaderRect()
	{
		return new Rectangle(0, 0, ((Control)this).Width - 1, HeaderHeight);
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		StiColors.InitColors();
	}

	protected override void OnControlAdded(ControlEventArgs e)
	{
		((Control)this).OnControlAdded(e);
		SetLayout();
	}

	protected override void OnControlRemoved(ControlEventArgs e)
	{
		((Control)this).OnControlRemoved(e);
		SetLayout();
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		CheckMouseOver();
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Control)this).OnMouseEnter(e);
		CheckMouseOver();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		CheckMouseOver();
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		Collapsed = !Collapsed;
		((Control)this).Invalidate();
	}

	protected override void OnPaintBackground(PaintEventArgs p)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		Color color = SystemColors.Control;
		if (((Control)this).Parent != null)
		{
			color = ((Control)this).Parent.BackColor;
		}
		Graphics graphics = p.Graphics;
		SolidBrush val = new SolidBrush(color);
		try
		{
			graphics.FillRectangle((Brush)(object)val, p.ClipRectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected override void OnPaint(PaintEventArgs p)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Expected O, but got Unknown
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Expected O, but got Unknown
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Expected O, but got Unknown
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Expected O, but got Unknown
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Expected O, but got Unknown
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Invalid comparison between Unknown and I4
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a0: Expected O, but got Unknown
		Graphics graphics = p.Graphics;
		Rectangle rect = new Rectangle(0, HeaderHeight, ((Control)this).Width, ((Control)this).Height - HeaderHeight - PanelDistance);
		if (((Control)this).BackgroundImage == null)
		{
			SolidBrush val = new SolidBrush(((Control)this).BackColor);
			try
			{
				graphics.FillRectangle((Brush)(object)val, rect.Left, rect.Top, rect.Width + 1, rect.Height + 1);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		else
		{
			StiControlPaint.DrawImageBackground(p.Graphics, ((Control)this).BackgroundImage, rect);
		}
		if (DrawBorder)
		{
			graphics.DrawRectangle(BorderPen, 0, 0, ((Control)this).Width - 1, ((Control)this).Height - PanelDistance);
		}
		Rectangle headerRect = GetHeaderRect();
		Bitmap val2 = ((!Collapsed) ? upBitmap : downBitmap);
		Color color = Color.Black;
		headerRect.Width++;
		headerRect.Height++;
		if (HeaderBackgroundImage == null)
		{
			if (Selected)
			{
				if (isMouseOver)
				{
					LinearGradientBrush val3 = new LinearGradientBrush(headerRect, StiColorUtils.Light(SelectedHeaderStartColor, 20), StiColorUtils.Light(SelectedHeaderEndColor, 20), 90f);
					try
					{
						graphics.FillRectangle((Brush)(object)val3, headerRect);
					}
					finally
					{
						((IDisposable)val3)?.Dispose();
					}
				}
				else
				{
					LinearGradientBrush val4 = new LinearGradientBrush(headerRect, SelectedHeaderStartColor, SelectedHeaderEndColor, 90f);
					try
					{
						graphics.FillRectangle((Brush)(object)val4, headerRect);
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
				}
				color = SystemColors.ActiveCaptionText;
			}
			else if (isMouseOver)
			{
				LinearGradientBrush val5 = new LinearGradientBrush(headerRect, StiColorUtils.Light(HeaderStartColor, 20), StiColorUtils.Light(HeaderEndColor, 20), 90f);
				try
				{
					graphics.FillRectangle((Brush)(object)val5, headerRect);
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
			}
			else
			{
				LinearGradientBrush val6 = new LinearGradientBrush(headerRect, HeaderStartColor, HeaderEndColor, 90f);
				try
				{
					graphics.FillRectangle((Brush)(object)val6, headerRect);
				}
				finally
				{
					((IDisposable)val6)?.Dispose();
				}
			}
		}
		else
		{
			StiControlPaint.DrawImageBackground(p.Graphics, HeaderBackgroundImage, headerRect);
		}
		headerRect.Width--;
		headerRect.Height--;
		if (val2 != null)
		{
			Rectangle rectangle = new Rectangle(headerRect.Width - 18, (headerRect.Height - 16) / 2, 16, 16);
			if (color != Color.Black && val2 != null)
			{
				val2 = StiImageUtils.ReplaceImageColor(val2, color, Color.Black);
			}
			graphics.DrawImage((Image)(object)val2, rectangle);
		}
		int num = 0;
		if (Image != null)
		{
			Rectangle rectangle2 = new Rectangle(headerRect.X + 4, (headerRect.Height - Image.Size.Height) / 2, Image.Size.Width, Image.Size.Height);
			num = rectangle2.Width + 2;
			graphics.DrawImage(Image, rectangle2);
		}
		Rectangle rectangle3 = new Rectangle(5 + num, 0, headerRect.Width - 25 - num, headerRect.Height);
		if (rectangle3.Width > 0)
		{
			StringFormat val7 = new StringFormat();
			try
			{
				val7.LineAlignment = (StringAlignment)1;
				if ((int)((Control)this).RightToLeft == 1)
				{
					val7.FormatFlags = (StringFormatFlags)(val7.FormatFlags | 1);
				}
				val7.FormatFlags = (StringFormatFlags)4096;
				val7.Trimming = (StringTrimming)3;
				val7.HotkeyPrefix = (HotkeyPrefix)2;
				if (!TitleColor.IsEmpty)
				{
					color = TitleColor;
				}
				Brush val8 = (Brush)new SolidBrush(color);
				try
				{
					graphics.DrawString(((Control)this).Text, TitleFont, val8, (RectangleF)rectangle3, val7);
				}
				finally
				{
					((IDisposable)val8)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val7)?.Dispose();
			}
		}
		if (DrawBorder)
		{
			graphics.DrawRectangle(BorderPen, headerRect);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (upBitmap != null)
			{
				((Image)upBitmap).Dispose();
			}
			if (downBitmap != null)
			{
				((Image)downBitmap).Dispose();
			}
		}
		((Control)this).Dispose(disposing);
	}

	public StiOutlookPanel()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		((Control)this).BackColor = StiColors.Content;
		upBitmap = StiImageUtils.GetImage("Stimulsoft.Controls", "Stimulsoft.Controls.Bmp.Up.bmp");
		downBitmap = StiImageUtils.GetImage("Stimulsoft.Controls", "Stimulsoft.Controls.Bmp.Down.bmp");
		headerStartColor = StiColors.ControlStart;
		headerEndColor = StiColors.ControlEnd;
		selectedHeaderStartColor = StiColors.ActiveCaptionStart;
		selectedHeaderEndColor = StiColors.ActiveCaptionEnd;
		DockPaddingEdges dockPadding = DockPadding;
		int left = (DockPadding.Right = 2);
		dockPadding.Left = left;
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)65536, true);
		((Control)this).SetStyle((ControlStyles)512, true);
		((Control)this).Height = 50;
		((Control)this).Text = "outlookPanel";
		SetLayout();
	}
}
