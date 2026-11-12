using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
[ToolboxBitmap(typeof(StiToolButton), "Toolbox.StiToolButton.bmp")]
[Designer(typeof(StiToolButtonDesigner))]
public class StiToolButton : Button
{
	private int arrowWidth = StiScaleUI.I6;

	private int arrowHeight = StiScaleUI.I3;

	private bool lockClick;

	protected bool IsMouseOver;

	protected bool IsPressed;

	private static Image dropDownArrowBitmap;

	private StiTextAndImageAlign textAndImageAlign = StiTextAndImageAlign.Center;

	private bool wordWrap;

	private StiToolBarButtonStyle style = StiToolBarButtonStyle.ToggleButton;

	private bool pushed;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public FlatStyle FlatStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((ButtonBase)this).FlatStyle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((ButtonBase)this).FlatStyle = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color BackColor
	{
		get
		{
			return ((ButtonBase)this).BackColor;
		}
		set
		{
			((ButtonBase)this).BackColor = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
	public bool TabStop
	{
		get
		{
			return ((Control)this).TabStop;
		}
		set
		{
			((Control)this).TabStop = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ShowSimpleArrow { get; set; } = true;


	[DefaultValue(StiControlStyle.Flat)]
	public StiControlStyle ControlStyle { get; set; }

	[Category("Appearance")]
	[DefaultValue(StiTextAndImageAlign.Center)]
	public StiTextAndImageAlign TextAndImageAlign
	{
		get
		{
			return textAndImageAlign;
		}
		set
		{
			textAndImageAlign = value;
			((Control)this).Invalidate();
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	public override bool AutoSize
	{
		get
		{
			return ((ButtonBase)this).AutoSize;
		}
		set
		{
			((ButtonBase)this).AutoSize = value;
			if (value)
			{
				UpdateSize();
			}
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(true)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	protected virtual bool AllowDrawText => true;

	[DefaultValue(true)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	protected virtual bool AllowDrawImage => true;

	[DefaultValue(true)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	protected virtual bool AllowDrawDrawDropDownArrow => true;

	[Category("Behavior")]
	[DefaultValue(false)]
	public bool WordWrap
	{
		get
		{
			return wordWrap;
		}
		set
		{
			wordWrap = value;
			((Control)this).Invalidate();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsDrawDropDownArrow { get; set; }

	[Browsable(false)]
	public bool HotDragDrop { get; set; }

	[Browsable(false)]
	public bool Draw3DButton { get; set; }

	[DefaultValue(false)]
	[Category("Behavior")]
	public bool AllowAllUp { get; set; }

	[Category("Behavior")]
	[DefaultValue(null)]
	public ContextMenuStrip DropDownMenu { get; set; }

	[DefaultValue(StiToolBarButtonStyle.ToggleButton)]
	[Category("Behavior")]
	public StiToolBarButtonStyle Style
	{
		get
		{
			return style;
		}
		set
		{
			style = value;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	public bool Pushed
	{
		get
		{
			return pushed;
		}
		set
		{
			if (style == StiToolBarButtonStyle.PushButton)
			{
				if (value && AllowAllUp)
				{
					ResetPushed();
				}
				pushed = value;
			}
			else
			{
				pushed = false;
			}
			((Control)this).Invalidate();
		}
	}

	public event EventHandler StartDragDrop;

	public void PerformClick()
	{
		((Control)this).OnClick(EventArgs.Empty);
	}

	private void CalcRectangles(out Rectangle textRect, out Rectangle imageRect, out Rectangle dropDownArrowRect, Rectangle rect, Graphics g, bool drawDropDownArrow)
	{
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Invalid comparison between Unknown and I4
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Invalid comparison between Unknown and I4
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Invalid comparison between Unknown and I4
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Invalid comparison between Unknown and I4
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Expected I4, but got Unknown
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Invalid comparison between Unknown and I4
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Invalid comparison between Unknown and I4
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Invalid comparison between Unknown and I4
		//IL_0384: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Invalid comparison between Unknown and I4
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Invalid comparison between Unknown and I4
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Invalid comparison between Unknown and I4
		dropDownArrowRect = Rectangle.Empty;
		double step = StiScaleUI.Step;
		if (drawDropDownArrow)
		{
			dropDownArrowRect.X = rect.Right - (int)Math.Ceiling(5.0 * StiScaleUI.X);
			dropDownArrowRect.Y = rect.Y + (rect.Height - dropDownArrowBitmap.Size.Height) / 2 + 1;
			dropDownArrowRect.Size = dropDownArrowBitmap.Size;
			rect.Width -= (int)Math.Ceiling(10.0 * StiScaleUI.X);
		}
		Size size = Size.Empty;
		if (((ButtonBase)this).ImageList != null && ((ButtonBase)this).ImageIndex >= 0 && ((ButtonBase)this).ImageIndex < ((ButtonBase)this).ImageList.Images.Count)
		{
			size = ((ButtonBase)this).ImageList.ImageSize;
		}
		try
		{
			if (size.IsEmpty && ((ButtonBase)this).Image != null)
			{
				size = new Size(((ButtonBase)this).Image.Width, ((ButtonBase)this).Image.Height);
			}
		}
		catch
		{
			size = new Size((int)Math.Ceiling(16.0 * step), (int)Math.Ceiling(16.0 * step));
		}
		StringFormat stringFormat = GetStringFormat();
		SizeF sizeF;
		try
		{
			sizeF = g.MeasureString(((Control)this).Text, ((Control)this).Font, new SizeF(rect.Width, rect.Height), stringFormat);
		}
		finally
		{
			((IDisposable)stringFormat)?.Dispose();
		}
		textRect = rect;
		imageRect = new Rectangle(rect.X, rect.Y, size.Width, size.Height);
		if ((int)((ButtonBase)this).TextAlign == 32 && (int)((ButtonBase)this).ImageAlign == 32)
		{
			imageRect.Y = rect.Y + (rect.Height - size.Height) / 2;
			switch (TextAndImageAlign)
			{
			case StiTextAndImageAlign.Center:
			{
				textRect.X += size.Width;
				textRect.Width -= size.Width;
				int num = (int)(((float)(rect.Width - size.Width) - sizeF.Width) / 2f);
				imageRect.X = rect.X + num;
				break;
			}
			case StiTextAndImageAlign.Left:
				imageRect.X = (int)Math.Ceiling(4.0 * StiScaleUI.X);
				textRect.X = imageRect.Right;
				textRect.Width = (int)sizeF.Width + (int)Math.Ceiling(4.0 * StiScaleUI.X);
				break;
			case StiTextAndImageAlign.Right:
				imageRect.X = rect.Width - size.Width + (int)Math.Ceiling(4.0 * StiScaleUI.X);
				textRect.X = rect.Width - imageRect.Width - (int)sizeF.Width + (int)Math.Ceiling(4.0 * StiScaleUI.X);
				textRect.Width = (int)sizeF.Width + (int)Math.Ceiling(4.0 * StiScaleUI.X);
				break;
			}
			return;
		}
		ContentAlignment imageAlign = ((ButtonBase)this).ImageAlign;
		if ((int)imageAlign <= 32)
		{
			switch (imageAlign - 1)
			{
			case 0:
				imageRect.X = rect.X;
				imageRect.Y = rect.Y;
				return;
			case 1:
				imageRect.X = rect.X + (rect.Width - size.Width) / 2;
				imageRect.Y = rect.Y;
				return;
			case 3:
				imageRect.X = rect.Right - size.Width;
				imageRect.Y = rect.Y;
				return;
			case 2:
				return;
			}
			if ((int)imageAlign != 16)
			{
				if ((int)imageAlign == 32)
				{
					imageRect.X = rect.X + (rect.Width - size.Width) / 2;
					imageRect.Y = rect.Y + (rect.Height - size.Height) / 2;
				}
			}
			else
			{
				imageRect.X = rect.X;
				imageRect.Y = rect.Y + (rect.Height - size.Height) / 2;
			}
		}
		else if ((int)imageAlign <= 256)
		{
			if ((int)imageAlign != 64)
			{
				if ((int)imageAlign == 256)
				{
					imageRect.X = rect.X;
					imageRect.Y = rect.Bottom - size.Height;
				}
			}
			else
			{
				imageRect.X = rect.Right - size.Width;
				imageRect.Y = rect.Y + (rect.Height - size.Height) / 2;
			}
		}
		else if ((int)imageAlign != 512)
		{
			if ((int)imageAlign == 1024)
			{
				imageRect.X = rect.Right - size.Width;
				imageRect.Y = rect.Bottom - size.Height;
			}
		}
		else
		{
			imageRect.X = rect.X + (rect.Width - size.Width) / 2;
			imageRect.Y = rect.Bottom - size.Height;
		}
	}

	private Size GetImageSize()
	{
		if ((((ButtonBase)this).ImageList != null && ((ButtonBase)this).ImageIndex >= 0 && ((ButtonBase)this).ImageIndex < ((ButtonBase)this).ImageList.Images.Count) || ((ButtonBase)this).Image != null)
		{
			if (((ButtonBase)this).ImageList != null && ((ButtonBase)this).ImageIndex >= 0 && ((ButtonBase)this).ImageIndex < ((ButtonBase)this).ImageList.Images.Count)
			{
				return ((ButtonBase)this).ImageList.ImageSize;
			}
			if (((ButtonBase)this).Image != null)
			{
				return ((ButtonBase)this).Image.Size;
			}
		}
		return Size.Empty;
	}

	protected virtual void DrawImage(Graphics g, Rectangle imageRect)
	{
		if ((((ButtonBase)this).ImageList == null || ((ButtonBase)this).ImageIndex < 0 || ((ButtonBase)this).ImageIndex >= ((ButtonBase)this).ImageList.Images.Count) && ((ButtonBase)this).Image == null)
		{
			return;
		}
		if (((ButtonBase)this).ImageList != null && ((ButtonBase)this).ImageIndex >= 0 && ((ButtonBase)this).ImageIndex < ((ButtonBase)this).ImageList.Images.Count)
		{
			if (((Control)this).Enabled)
			{
				((ButtonBase)this).ImageList.Draw(g, imageRect.X, imageRect.Y, imageRect.Width, imageRect.Height, ((ButtonBase)this).ImageIndex);
			}
			else
			{
				StiControlPaint.DrawImageDisabled(g, ((ButtonBase)this).ImageList.Images[((ButtonBase)this).ImageIndex], imageRect.Left, imageRect.Top);
			}
		}
		else if (((Control)this).Enabled)
		{
			g.DrawImage(((ButtonBase)this).Image, imageRect.X, imageRect.Y, imageRect.Width, imageRect.Height);
		}
		else
		{
			StiControlPaint.DrawImageDisabled(g, ((ButtonBase)this).Image, imageRect.Left, imageRect.Top);
		}
	}

	protected void DrawDropDownArrow(Graphics g, Rectangle rect)
	{
		if (!IsDrawDropDownArrow && DropDownMenu == null)
		{
			return;
		}
		if (!ShowSimpleArrow)
		{
			if (((Control)this).Enabled)
			{
				g.DrawImage(dropDownArrowBitmap, rect.X, rect.Y, rect.Width, rect.Height);
			}
			else
			{
				StiControlPaint.DrawImageDisabled(g, dropDownArrowBitmap, rect.Left, rect.Top);
			}
		}
		else
		{
			StiButtonHelper.DrawDownArrow(g, rect, ((Control)this).Enabled);
		}
	}

	private StringFormat GetStringFormat()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Invalid comparison between Unknown and I4
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Invalid comparison between Unknown and I4
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Invalid comparison between Unknown and I4
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Invalid comparison between Unknown and I4
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Invalid comparison between Unknown and I4
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected I4, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Invalid comparison between Unknown and I4
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Invalid comparison between Unknown and I4
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Invalid comparison between Unknown and I4
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Invalid comparison between Unknown and I4
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Invalid comparison between Unknown and I4
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Invalid comparison between Unknown and I4
		StringFormat val = new StringFormat
		{
			Trimming = (StringTrimming)3,
			FormatFlags = (StringFormatFlags)0,
			HotkeyPrefix = (HotkeyPrefix)1
		};
		if (!wordWrap)
		{
			val.FormatFlags = (StringFormatFlags)4096;
		}
		if ((int)((Control)this).RightToLeft == 1)
		{
			val.FormatFlags = (StringFormatFlags)(val.FormatFlags | 1);
		}
		if ((int)((ButtonBase)this).TextAlign == 32 && (int)((ButtonBase)this).ImageAlign == 32)
		{
			val.Alignment = (StringAlignment)1;
			val.LineAlignment = (StringAlignment)1;
		}
		else
		{
			ContentAlignment textAlign = ((ButtonBase)this).TextAlign;
			if ((int)textAlign <= 32)
			{
				switch (textAlign - 1)
				{
				default:
					if ((int)textAlign != 16)
					{
						if ((int)textAlign == 32)
						{
							val.Alignment = (StringAlignment)1;
							val.LineAlignment = (StringAlignment)1;
						}
					}
					else
					{
						val.Alignment = (StringAlignment)0;
						val.LineAlignment = (StringAlignment)1;
					}
					break;
				case 0:
					val.Alignment = (StringAlignment)0;
					val.LineAlignment = (StringAlignment)0;
					break;
				case 1:
					val.Alignment = (StringAlignment)1;
					val.LineAlignment = (StringAlignment)0;
					break;
				case 3:
					val.Alignment = (StringAlignment)2;
					val.LineAlignment = (StringAlignment)0;
					break;
				case 2:
					break;
				}
			}
			else if ((int)textAlign <= 256)
			{
				if ((int)textAlign != 64)
				{
					if ((int)textAlign == 256)
					{
						val.Alignment = (StringAlignment)0;
						val.LineAlignment = (StringAlignment)2;
					}
				}
				else
				{
					val.Alignment = (StringAlignment)2;
					val.LineAlignment = (StringAlignment)1;
				}
			}
			else if ((int)textAlign != 512)
			{
				if ((int)textAlign == 1024)
				{
					val.Alignment = (StringAlignment)2;
					val.LineAlignment = (StringAlignment)2;
				}
			}
			else
			{
				val.Alignment = (StringAlignment)1;
				val.LineAlignment = (StringAlignment)2;
			}
		}
		return val;
	}

	protected void DrawText(Graphics g)
	{
		DrawText(g, GetTextRect(g));
	}

	protected void DrawText(Graphics g, Rectangle textRect)
	{
		if (string.IsNullOrEmpty(((Control)this).Text))
		{
			return;
		}
		StringFormat stringFormat = GetStringFormat();
		try
		{
			if (((Control)this).Enabled)
			{
				TextRenderer.DrawText((IDeviceContext)(object)g, ((Control)this).Text, ((Control)this).Font, textRect, ((Control)this).ForeColor);
			}
			else
			{
				ControlPaint.DrawStringDisabled(g, ((Control)this).Text, ((Control)this).Font, StiUX.ButtonForegroundDisabled, (RectangleF)textRect, stringFormat);
			}
		}
		finally
		{
			((IDisposable)stringFormat)?.Dispose();
		}
	}

	private Size GetActualSize()
	{
		Size result = TextRenderer.MeasureText(((Control)this).Text, ((Control)this).Font);
		Size imageSize = GetImageSize();
		result.Width += imageSize.Width;
		return result;
	}

	private void UpdateSize()
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected I4, but got Unknown
		if (!((Control)this).AutoSize)
		{
			return;
		}
		Size actualSize = GetActualSize();
		actualSize.Width += StiScaleUI.I14;
		actualSize.Height += StiScaleUI.I14;
		if (!(((Control)this).Size != actualSize))
		{
			return;
		}
		DockStyle dock = ((Control)this).Dock;
		switch ((int)dock)
		{
		case 0:
			if (((Control)this).Width != actualSize.Width && ((Control)this).Height != actualSize.Height)
			{
				((Control)this).Size = actualSize;
			}
			break;
		case 3:
		case 4:
			if (((Control)this).Width != actualSize.Width)
			{
				((Control)this).Width = actualSize.Width;
			}
			break;
		case 1:
		case 2:
			if (((Control)this).Height != actualSize.Height)
			{
				((Control)this).Height = actualSize.Height;
			}
			break;
		}
	}

	private void ResetPushed()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		if (((Control)this).Parent == null)
		{
			return;
		}
		foreach (Control item in (ArrangedElementCollection)((Control)this).Parent.Controls)
		{
			Control val = item;
			if (val != this && val is StiToolButton && ((StiToolButton)(object)val).Pushed)
			{
				((StiToolButton)(object)val).Pushed = false;
			}
		}
	}

	protected override bool ProcessMnemonic(char charCode)
	{
		if (Control.IsMnemonic(charCode, ((Control)this).Text))
		{
			((Control)this).OnClick(EventArgs.Empty);
			lockClick = false;
			return true;
		}
		return ((Button)this).ProcessMnemonic(charCode);
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		StiColors.InitColors();
	}

	protected override void OnPaint(PaintEventArgs p)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Invalid comparison between Unknown and I4
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Invalid comparison between Unknown and I4
		UpdateSize();
		Graphics graphics = p.Graphics;
		Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		if (Style == StiToolBarButtonStyle.Separator)
		{
			Color color = (StiUX.IsDark ? StiUX.Separator : SystemColors.ControlDark);
			Color color2 = (StiUX.IsDark ? StiUX.Separator : SystemColors.ControlLightLight);
			Pen val = new Pen(color);
			try
			{
				Pen val2 = new Pen(color2);
				try
				{
					if ((int)((Control)this).Dock == 1 || (int)((Control)this).Dock == 2)
					{
						int num = ((Control)this).Height / 2 - 1;
						graphics.DrawLine(val, (int)Math.Ceiling(2.0 * StiScaleUI.X), num, ((Control)this).Width - (int)Math.Ceiling(4.0 * StiScaleUI.X), num);
						graphics.DrawLine(val2, (int)Math.Ceiling(2.0 * StiScaleUI.X), num + 1, ((Control)this).Width - (int)Math.Ceiling(4.0 * StiScaleUI.X), num + 1);
					}
					else
					{
						int num2 = ((Control)this).Width / 2 - 1;
						graphics.DrawLine(val, num2, (int)Math.Ceiling(2.0 * StiScaleUI.Y), num2, ((Control)this).Height - (int)Math.Ceiling(4.0 * StiScaleUI.Y));
						graphics.DrawLine(val2, num2 + 1, (int)Math.Ceiling(2.0 * StiScaleUI.Y), num2 + 1, ((Control)this).Height - (int)Math.Ceiling(4.0 * StiScaleUI.Y));
					}
					return;
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		bool flag = false;
		if (((Control)this).Parent != null)
		{
			if (((Control)this).Parent is StiToolBar { ControlStyle: StiControlStyle.Office2013Blue })
			{
				flag = true;
			}
			if (ControlStyle == StiControlStyle.Office2013Blue)
			{
				flag = true;
			}
		}
		if (flag)
		{
			if (((Control)this).Enabled && (Pushed || IsPressed))
			{
				if (IsPressed)
				{
					StiDrawing.FillRectangle(graphics, StiUX.IsDark ? StiUX.ItemBackgroundPressed : StiStyleOffice2013Blue.ColorButtonPressed, rectangle);
				}
				else
				{
					StiDrawing.FillRectangle(graphics, StiUX.IsDark ? StiUX.ItemBackground : StiColors.Selected, rectangle);
				}
			}
			if (!IsPressed)
			{
				if (IsMouseOver && !((Component)this).DesignMode)
				{
					StiDrawing.FillRectangle(graphics, StiUX.IsDark ? StiUX.ItemBackgroundHover : StiStyleOffice2013Blue.ColorButtonButtonMouseOver, rectangle);
				}
				else if (((Component)this).DesignMode || Draw3DButton)
				{
					ControlPaint.DrawBorder3D(graphics, rectangle, (Border3DStyle)4, (Border3DSide)2063);
				}
			}
			rectangle = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
			CalcRectangles(out var textRect, out var imageRect, out var dropDownArrowRect, new Rectangle(rectangle.X + StiScaleUI.I4, rectangle.Y + StiScaleUI.I4, rectangle.Width - StiScaleUI.I8, rectangle.Height - StiScaleUI.I8), graphics, DropDownMenu != null || IsDrawDropDownArrow);
			try
			{
				if (AllowDrawImage)
				{
					DrawImage(graphics, imageRect);
				}
				if (AllowDrawText)
				{
					DrawText(graphics, textRect);
				}
				if (AllowDrawDrawDropDownArrow)
				{
					DrawDropDownArrow(graphics, dropDownArrowRect);
				}
				return;
			}
			catch
			{
				return;
			}
		}
		if (((Control)this).Enabled && (Pushed || IsPressed))
		{
			rectangle.X++;
			rectangle.Y++;
			rectangle.Width -= 2;
			rectangle.Height -= 2;
			if (IsPressed)
			{
				StiDrawing.FillRectangle(graphics, StiUX.IsDark ? StiUX.ItemBackgroundPressed : StiColorUtils.Dark(StiColors.Selected, 50), rectangle);
			}
			else
			{
				StiDrawing.FillRectangle(graphics, StiUX.IsDark ? StiUX.ItemBackgroundSelected : StiColors.Selected, rectangle);
			}
			rectangle.Width--;
			rectangle.Height--;
			StiDrawing.DrawRectangle(graphics, StiUX.IsDark ? StiUX.ItemForeground : StiColors.SelectedText, rectangle);
			rectangle.X--;
			rectangle.Y--;
			rectangle.Width += 3;
			rectangle.Height += 3;
		}
		if (!IsPressed)
		{
			if (IsMouseOver && !((Component)this).DesignMode)
			{
				StiDrawing.FillRectangle(graphics, StiUX.IsDark ? StiUX.ItemBackgroundSelected : StiColors.Focus, rectangle);
				rectangle.Width--;
				rectangle.Height--;
				StiDrawing.DrawRectangle(graphics, StiUX.IsDark ? StiUX.ItemForegroundHover : StiColors.SelectedText, rectangle);
			}
			else if (((Component)this).DesignMode || Draw3DButton)
			{
				ControlPaint.DrawBorder3D(graphics, rectangle, (Border3DStyle)4, (Border3DSide)2063);
			}
		}
		rectangle = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		CalcRectangles(out var textRect2, out var imageRect2, out var dropDownArrowRect2, new Rectangle(rectangle.X + (int)Math.Ceiling(4.0 * StiScaleUI.X), rectangle.Y + (int)Math.Ceiling(4.0 * StiScaleUI.Y), rectangle.Width - (int)Math.Ceiling(8.0 * StiScaleUI.X), rectangle.Height - (int)Math.Ceiling(8.0 * StiScaleUI.Y)), graphics, DropDownMenu != null || IsDrawDropDownArrow);
		try
		{
			if (AllowDrawImage)
			{
				DrawImage(graphics, imageRect2);
			}
			if (AllowDrawText)
			{
				DrawText(graphics, textRect2);
			}
			if (AllowDrawDrawDropDownArrow)
			{
				DrawDropDownArrow(graphics, dropDownArrowRect2);
			}
		}
		catch
		{
		}
	}

	protected Rectangle GetTextRect(Graphics g)
	{
		Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		CalcRectangles(out var textRect, out var _, out var _, new Rectangle(rectangle.X + (int)Math.Ceiling(4.0 * StiScaleUI.X), rectangle.Y + (int)Math.Ceiling(4.0 * StiScaleUI.Y), rectangle.Width - (int)Math.Ceiling(8.0 * StiScaleUI.X), rectangle.Height - (int)Math.Ceiling(8.0 * StiScaleUI.Y)), g, DropDownMenu != null || IsDrawDropDownArrow);
		return textRect;
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Button)this).OnMouseEnter(e);
		IsMouseOver = true;
		((Control)this).Invalidate();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Button)this).OnMouseLeave(e);
		IsMouseOver = false;
		((Control)this).Invalidate();
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((ButtonBase)this).OnMouseMove(e);
		Point pt = ((Control)this).PointToClient(Cursor.Position);
		if (IsPressed && HotDragDrop && !((Control)this).ClientRectangle.Contains(pt))
		{
			this.StartDragDrop?.Invoke(this, EventArgs.Empty);
			IsPressed = false;
			((Control)this).Invalidate();
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((ButtonBase)this).OnMouseDown(e);
		if (DropDownMenu != null)
		{
			((ToolStripDropDown)DropDownMenu).Show((Control)(object)this, new Point(0, ((Control)this).Height));
		}
		else
		{
			IsPressed = true;
		}
		((Control)this).Invalidate();
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Button)this).OnMouseUp(e);
		lockClick = false;
		IsPressed = false;
		((Control)this).Invalidate();
	}

	protected override void OnClick(EventArgs e)
	{
		if (!lockClick)
		{
			lockClick = true;
			ContextMenuStrip dropDownMenu = DropDownMenu;
			if (dropDownMenu != null)
			{
				((ToolStripDropDown)dropDownMenu).Show((Control)(object)this, new Point(0, ((Control)this).Height));
			}
			if (style == StiToolBarButtonStyle.PushButton)
			{
				Pushed = !Pushed;
			}
			((Button)this).OnClick(e);
		}
	}

	protected override void OnTextChanged(EventArgs e)
	{
		((Button)this).OnTextChanged(e);
		UpdateSize();
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		((Control)this).OnSizeChanged(e);
		UpdateSize();
	}

	public StiToolButton()
	{
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)512, false);
		((Control)this).SetStyle((ControlStyles)2048, true);
		((Control)this).SetStyle((ControlStyles)4, false);
		TabStop = false;
		((Control)this).Height = (int)Math.Ceiling(22.0 * StiScaleUI.Y);
		((Control)this).Width = (int)Math.Ceiling(22.0 * StiScaleUI.X);
		((Control)this).BackColor = Color.Transparent;
	}

	static StiToolButton()
	{
		double step = StiScaleUI.Step;
		if (step > 1.5)
		{
			dropDownArrowBitmap = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Controls", "Stimulsoft.Controls.Bmp.DropDownArrow_x2.bmp");
		}
		else if (step > 1.0)
		{
			dropDownArrowBitmap = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Controls", "Stimulsoft.Controls.Bmp.DropDownArrow_x1_5.bmp");
		}
		else
		{
			dropDownArrowBitmap = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Controls", "Stimulsoft.Controls.Bmp.DropDownArrow.bmp");
		}
	}
}
