using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class MDISystemItem : PopupItem
{
	private bool ᲁ;

	private Icon ຈ;

	private bool ᲂ;

	private bool ᲃ;

	private bool ᲄ;

	private _1886 ݡ;

	private _1886 ݠ;

	private _1886 ᲅ;

	public bool IsSystemIcon
	{
		get
		{
			return ᲁ;
		}
		set
		{
			if (ᲁ != value)
			{
				ᲁ = value;
				NeedRecalcSize = true;
				if (ᲁ)
				{
					_1C8A();
					ShowSubItems = true;
				}
				else
				{
					SubItems.Clear();
					ShowSubItems = false;
				}
				ShowSubItems = ᲁ;
				Refresh();
			}
		}
	}

	public Icon Icon
	{
		get
		{
			return ຈ;
		}
		set
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			if (value != null)
			{
				ຈ = new Icon(value, 16, 16);
			}
			else
			{
				ຈ = null;
			}
			Refresh();
		}
	}

	internal _1886 _1C7E => ᲅ;

	public bool MinimizeEnabled
	{
		get
		{
			return ᲂ;
		}
		set
		{
			if (value != ᲂ)
			{
				ᲂ = value;
				Refresh();
			}
		}
	}

	public bool RestoreEnabled
	{
		get
		{
			return ᲃ;
		}
		set
		{
			if (value != ᲃ)
			{
				ᲃ = value;
				Refresh();
			}
		}
	}

	public bool CloseEnabled
	{
		get
		{
			return ᲄ;
		}
		set
		{
			if (value != ᲄ)
			{
				ᲄ = value;
				Refresh();
			}
		}
	}

	internal _1886 _1C7F => ݡ;

	internal _1886 ᲀ => ݠ;

	public MDISystemItem()
		: this("")
	{
	}

	public MDISystemItem(string sName)
		: base(sName)
	{
		GlobalItem = false;
		SetSystemItem(b: true);
		ᲁ = false;
		ຈ = null;
		ᲂ = true;
		ᲃ = true;
		ᲄ = true;
		ݡ = _1886.ᘶ;
		ݠ = _1886.ᘶ;
		ᲅ = _1886.ᘶ;
		CanCustomize = false;
		Tooltip = "";
		ShowSubItems = false;
		m_ShouldSerialize = false;
		IsAccessible = false;
	}

	protected override void Dispose(bool disposing)
	{
		if (ຈ != null)
		{
			ຈ.Dispose();
			ຈ = null;
		}
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem is ButtonItem { Image: not null } buttonItem)
			{
				buttonItem.Image.Dispose();
				buttonItem.Image = null;
			}
		}
		base.Dispose(disposing);
	}

	public override BaseItem Copy()
	{
		MDISystemItem mDISystemItem = new MDISystemItem();
		CopyToItem(mDISystemItem);
		mDISystemItem.MinimizeEnabled = ᲂ;
		mDISystemItem.RestoreEnabled = ᲃ;
		mDISystemItem.CloseEnabled = ᲄ;
		return mDISystemItem;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		MDISystemItem mDISystemItem = copy as MDISystemItem;
		base.CopyToItem((BaseItem)mDISystemItem);
		mDISystemItem.IsSystemIcon = ᲁ;
	}

	public override void Paint(ItemPaintArgs pa)
	{
		if (SuspendLayout)
		{
			return;
		}
		if (BarFunctions.IsOffice2007Style(EffectiveStyle) && pa.ໝ != null)
		{
			pa.ໝ.DrawMdiSystemItem(new MdiSystemItemRendererEventArgs(pa.Graphics, this));
			return;
		}
		Graphics graphics = pa.Graphics;
		Rectangle displayRectangle;
		if (ᲁ)
		{
			displayRectangle = DisplayRectangle;
			displayRectangle.Offset((displayRectangle.Width - 16) / 2, (displayRectangle.Height - 16) / 2);
			if (ຈ != null)
			{
				graphics.DrawIconUnstretched(ຈ, displayRectangle);
			}
		}
		else if (EffectiveStyle == eDotNetBarStyle.Office2000)
		{
			displayRectangle = new Rectangle(DisplayRectangle.Location, ᱼ());
			displayRectangle.Inflate(-1, -2);
			displayRectangle.Location = DisplayRectangle.Location;
			if (Orientation == eOrientation.Horizontal)
			{
				displayRectangle.Offset(0, (DisplayRectangle.Height - displayRectangle.Height) / 2);
			}
			else
			{
				displayRectangle.Offset((WidthInternal - displayRectangle.Width) / 2, 0);
			}
			if (!ᲂ)
			{
				ControlPaint.DrawCaptionButton(graphics, displayRectangle, (CaptionButton)1, (ButtonState)256);
			}
			else if (ݠ == _1886._1C8C)
			{
				ControlPaint.DrawCaptionButton(graphics, displayRectangle, (CaptionButton)1, (ButtonState)512);
			}
			else
			{
				ControlPaint.DrawCaptionButton(graphics, displayRectangle, (CaptionButton)1, (ButtonState)0);
			}
			if (Orientation == eOrientation.Horizontal)
			{
				displayRectangle.Offset(displayRectangle.Width + 1, 0);
			}
			else
			{
				displayRectangle.Offset(0, displayRectangle.Height + 1);
			}
			if (!ᲃ)
			{
				ControlPaint.DrawCaptionButton(graphics, displayRectangle, (CaptionButton)3, (ButtonState)256);
			}
			else if (ݠ == _1886._1C8D)
			{
				ControlPaint.DrawCaptionButton(graphics, displayRectangle, (CaptionButton)3, (ButtonState)512);
			}
			else
			{
				ControlPaint.DrawCaptionButton(graphics, displayRectangle, (CaptionButton)3, (ButtonState)0);
			}
			if (Orientation == eOrientation.Horizontal)
			{
				displayRectangle.Offset(displayRectangle.Width + 3, 0);
			}
			else
			{
				displayRectangle.Offset(0, displayRectangle.Height + 3);
			}
			if (!ᲄ)
			{
				ControlPaint.DrawCaptionButton(graphics, displayRectangle, (CaptionButton)0, (ButtonState)256);
			}
			else if (ݠ == _1886.ᆤ)
			{
				ControlPaint.DrawCaptionButton(graphics, displayRectangle, (CaptionButton)0, (ButtonState)512);
			}
			else
			{
				ControlPaint.DrawCaptionButton(graphics, displayRectangle, (CaptionButton)0, (ButtonState)0);
			}
		}
		else
		{
			displayRectangle = new Rectangle(DisplayRectangle.Location, ᱼ());
			displayRectangle.Inflate(-1, -1);
			if (Orientation == eOrientation.Horizontal)
			{
				displayRectangle.Offset(0, (DisplayRectangle.Height - displayRectangle.Height) / 2);
			}
			else
			{
				displayRectangle.Offset((WidthInternal - displayRectangle.Width) / 2, 0);
			}
			_1935(pa, _1886._1C8C, displayRectangle);
			if (Orientation == eOrientation.Horizontal)
			{
				displayRectangle.Offset(displayRectangle.Width, 0);
			}
			else
			{
				displayRectangle.Offset(0, displayRectangle.Height);
			}
			_1935(pa, _1886._1C8D, displayRectangle);
			if (Orientation == eOrientation.Horizontal)
			{
				displayRectangle.Offset(displayRectangle.Width + 2, 0);
			}
			else
			{
				displayRectangle.Offset(0, displayRectangle.Height + 2);
			}
			_1935(pa, _1886.ᆤ, displayRectangle);
		}
	}

	private void _1935(ItemPaintArgs _0BD2, _1886 ख़, Rectangle ल)
	{
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Expected O, but got Unknown
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Expected O, but got Unknown
		Graphics graphics = _0BD2.Graphics;
		if (base.IsThemed)
		{
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				_0A79 obj = _0BD2._0A79;
				_༣ _070B = _༣._0F33;
				ཉ య = ཉ.ཐ;
				switch (ख़)
				{
				case _1886.ᆤ:
					_070B = _༣._0F37;
					break;
				case _1886._1C8E:
					_070B = _༣._0F3B;
					break;
				case _1886._1C8D:
					_070B = _༣._0F39;
					break;
				}
				if (ख़ == ݠ)
				{
					య = ཉ.དྷ;
				}
				else if (ख़ == ݡ)
				{
					య = ཉ.ད;
				}
				obj._0EE5(graphics, _070B, య, ल);
				return;
			}
		}
		if (ख़ == ݠ)
		{
			if (_0BD2.Colors.ItemPressedBackground2.IsEmpty)
			{
				graphics.FillRectangle((Brush)new SolidBrush(_0BD2.Colors.ItemPressedBackground), ल);
			}
			else
			{
				LinearGradientBrush val2 = BarFunctions.ॴ(ल, _0BD2.Colors.ItemPressedBackground, _0BD2.Colors.ItemPressedBackground2, _0BD2.Colors.ItemPressedBackgroundGradientAngle);
				graphics.FillRectangle((Brush)(object)val2, ल);
				((Brush)val2).Dispose();
			}
			_089F.र(graphics, SystemPens.Highlight, ल);
		}
		else if (ख़ == ݡ)
		{
			if (!_0BD2.Colors.ItemHotBackground2.IsEmpty)
			{
				LinearGradientBrush val3 = BarFunctions.ॴ(ल, _0BD2.Colors.ItemHotBackground, _0BD2.Colors.ItemHotBackground2, _0BD2.Colors.ItemHotBackgroundGradientAngle);
				graphics.FillRectangle((Brush)(object)val3, ल);
				((Brush)val3).Dispose();
			}
			else
			{
				graphics.FillRectangle((Brush)new SolidBrush(_0BD2.Colors.ItemHotBackground), ल);
			}
			_089F.र(graphics, new Pen(_0BD2.Colors.ItemHotBorder), ल);
		}
		Bitmap val4 = ᲆ(graphics, ख़, ल, _0BD2.Colors);
		try
		{
			if ((ख़ == _1886._1C8C && !ᲂ) || (ख़ == _1886._1C8D && !ᲃ) || (ख़ == _1886.ᆤ && !ᲄ))
			{
				float[][] array = new float[5][];
				float[] array2 = new float[5];
				array[0] = array2;
				float[] array3 = new float[5];
				array[1] = array3;
				float[] array4 = new float[5];
				array[2] = array4;
				array[3] = new float[5] { 0.5f, 0.5f, 0.5f, 0.5f, 0f };
				float[] array5 = new float[5];
				array[4] = array5;
				ColorMatrix colorMatrix = new ColorMatrix(array);
				ImageAttributes val5 = new ImageAttributes();
				val5.ClearColorKey();
				val5.SetColorMatrix(colorMatrix);
				graphics.DrawImage((Image)(object)val4, ल, 0, 0, ((Image)val4).Width, ((Image)val4).Height, (GraphicsUnit)2, val5);
			}
			else
			{
				if (ख़ == ݠ)
				{
					ल.Offset(1, 1);
				}
				graphics.DrawImageUnscaled((Image)(object)val4, ल);
			}
		}
		finally
		{
			((IDisposable)val4)?.Dispose();
		}
	}

	internal Bitmap ᲆ(Graphics _٥, _1886 ख़, Rectangle ल, ColorScheme ᲇ)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		Bitmap val = new Bitmap(ल.Width, ल.Height, _٥);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		Rectangle rectangle = new Rectangle(0, 0, ल.Width, ल.Height);
		rectangle.Inflate(0, -1);
		Rectangle clip = rectangle;
		clip.Inflate(-1, -1);
		SolidBrush val3 = new SolidBrush(SystemColors.Control);
		try
		{
			val2.FillRectangle((Brush)(object)val3, 0, 0, ल.Width, ल.Height);
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		val2.SetClip(clip);
		ControlPaint.DrawCaptionButton(val2, rectangle, (CaptionButton)ख़, (ButtonState)16384);
		val2.ResetClip();
		val2.Dispose();
		if (!ᲇ.MdiSystemItemForeground.IsEmpty)
		{
			Bitmap val4 = new Bitmap((Image)(object)val, ((Image)val).Width, ((Image)val).Height);
			Graphics val5 = Graphics.FromImage((Image)(object)val4);
			try
			{
				val5.Clear(Color.Transparent);
				ImageAttributes val6 = new ImageAttributes();
				ColorMap val7 = new ColorMap();
				val7.OldColor = Color.Black;
				val7.NewColor = ᲇ.MdiSystemItemForeground;
				val6.SetRemapTable((ColorMap[])(object)new ColorMap[1] { val7 }, (ColorAdjustType)1);
				val5.DrawImage((Image)(object)val, new Rectangle(0, 0, ((Image)val).Width, ((Image)val).Height), 0, 0, ((Image)val).Width, ((Image)val).Height, (GraphicsUnit)2, val6, (DrawImageAbort)null, IntPtr.Zero);
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
			((Image)val).Dispose();
			val = val4;
		}
		val.MakeTransparent(SystemColors.Control);
		return val;
	}

	protected override void OnTooltip(bool bShow)
	{
		base.OnTooltip(bShow);
		if (!bShow || IsSystemIcon)
		{
			return;
		}
		Point point = Control.MousePosition;
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val != null)
		{
			point = val.PointToClient(point);
		}
		_1886 obj = ᱽ(point.X, point.Y);
		using LocalizationManager localizationManager = new LocalizationManager(GetOwner() as IOwnerLocalize);
		switch (obj)
		{
		case _1886._1C8C:
			Tooltip = localizationManager.GetLocalizedString(LocalizationKeys.MdiSystemItemMinimizeTooltip);
			break;
		case _1886._1C8D:
			Tooltip = localizationManager.GetLocalizedString(LocalizationKeys.MdiSystemItemRestoreTooltip);
			break;
		case _1886._1C8F:
			Tooltip = localizationManager.GetLocalizedString(LocalizationKeys.MdiSystemItemMenuMaximize);
			break;
		case _1886.ᆤ:
			Tooltip = localizationManager.GetLocalizedString(LocalizationKeys.MdiSystemItemCloseTooltip);
			break;
		default:
			Tooltip = "";
			break;
		}
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		base.InternalMouseDown(objArg);
		if ((int)objArg.Button != 1048576 || DesignMode || !GetEnabled())
		{
			return;
		}
		if (IsSystemIcon)
		{
			if (m_Parent != null)
			{
				if (Expanded)
				{
					m_Parent.AutoExpand = false;
				}
				else
				{
					m_Parent.AutoExpand = true;
				}
			}
			Expanded = !Expanded;
			return;
		}
		_1886 obj = ᱽ(objArg.X, objArg.Y);
		if (obj == _1886._1C8E)
		{
			ݠ = _1886._1C8E;
			Refresh();
		}
		else if (ᲂ && obj == _1886._1C8C)
		{
			ݠ = _1886._1C8C;
			Refresh();
		}
		else if (ᲃ && obj == _1886._1C8D)
		{
			ݠ = _1886._1C8D;
			Refresh();
		}
		else if (!ᲃ && obj == _1886._1C8F)
		{
			ݠ = _1886._1C8F;
			Refresh();
		}
		else if (ᲄ && obj == _1886.ᆤ)
		{
			ݠ = _1886.ᆤ;
			Refresh();
		}
		else
		{
			ݠ = _1886.ᘶ;
			Refresh();
		}
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		base.InternalMouseUp(objArg);
		if (ݠ != _1886.ᘶ)
		{
			ݠ = _1886.ᘶ;
			Refresh();
		}
	}

	public override void InternalClick(MouseButtons mb, Point mpos)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		if (IsSystemIcon)
		{
			ᲅ = _1886.ᘶ;
			base.InternalClick(mb, mpos);
			return;
		}
		Point point = mpos;
		ᲅ = ᱽ(point.X, point.Y);
		if ((ᲅ == _1886.ᆤ && !ᲄ) || (ᲅ == _1886._1C8C && !ᲂ) || (ᲅ == _1886._1C8D && !ᲃ) || (ᲅ == _1886._1C8F && ᲃ))
		{
			ᲅ = _1886.ᘶ;
		}
		if (ᲅ != _1886.ᘶ)
		{
			base.InternalClick(mb, mpos);
		}
	}

	public override void InternalMouseLeave()
	{
		base.InternalMouseLeave();
		ݠ = _1886.ᘶ;
		ݡ = _1886.ᘶ;
		Refresh();
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		base.InternalMouseMove(objArg);
		switch (ᱽ(objArg.X, objArg.Y))
		{
		case _1886._1C8E:
			if (ݡ != _1886._1C8E)
			{
				ݡ = _1886._1C8E;
				if (base.ToolTipVisible)
				{
					HideToolTip();
					ResetHover();
				}
				Refresh();
			}
			return;
		case _1886._1C8C:
			if (ݡ != _1886._1C8C)
			{
				ݡ = _1886._1C8C;
				if (base.ToolTipVisible)
				{
					HideToolTip();
					ResetHover();
				}
				Refresh();
			}
			return;
		case _1886._1C8D:
			if (ݡ != _1886._1C8D)
			{
				ݡ = _1886._1C8D;
				if (base.ToolTipVisible)
				{
					HideToolTip();
					ResetHover();
				}
				Refresh();
			}
			return;
		case _1886._1C8F:
			if (ݡ != _1886._1C8F)
			{
				ݡ = _1886._1C8F;
				if (base.ToolTipVisible)
				{
					HideToolTip();
					ResetHover();
				}
				Refresh();
			}
			return;
		case _1886.ᆤ:
			if (ݡ != 0)
			{
				ݡ = _1886.ᆤ;
				if (base.ToolTipVisible)
				{
					HideToolTip();
					ResetHover();
				}
				Refresh();
			}
			return;
		}
		if (ݡ != _1886.ᘶ)
		{
			ݡ = _1886.ᘶ;
			if (base.ToolTipVisible)
			{
				HideToolTip();
				ResetHover();
			}
			Refresh();
		}
	}

	public override void RecalcSize()
	{
		if (!SuspendLayout)
		{
			if (ᲁ)
			{
				m_Rect.Size = new Size(Dpi.Width16, Dpi.Height16);
			}
			else if (Orientation == eOrientation.Horizontal)
			{
				m_Rect.Size = new Size(ᱼ().Width * 3 + 2, ᱼ().Height);
			}
			else
			{
				m_Rect.Size = new Size(ᱼ().Width, ᱼ().Height * 3 + 2);
			}
			base.RecalcSize();
		}
	}

	internal void ᲈ(Icon ᆠ)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		if (ຈ != null)
		{
			ຈ.Dispose();
		}
		if (ᆠ != null)
		{
			ຈ = new Icon(ᆠ, Dpi.Width16, Dpi.Height16);
		}
		else
		{
			ຈ = null;
		}
	}

	internal virtual Size ᱼ()
	{
		return SystemInformation.MenuButtonSize;
	}

	internal virtual _1886 ᱽ(int و, int ى)
	{
		Rectangle rectangle = new Rectangle(DisplayRectangle.Location, ᱼ());
		rectangle.Inflate(-1, -2);
		rectangle.Location = DisplayRectangle.Location;
		if (Orientation == eOrientation.Horizontal)
		{
			rectangle.Offset(0, (DisplayRectangle.Height - rectangle.Height) / 2);
		}
		else
		{
			rectangle.Offset((WidthInternal - rectangle.Width) / 2, 0);
		}
		if (rectangle.Contains(و, ى))
		{
			return _1886._1C8C;
		}
		if (Orientation == eOrientation.Horizontal)
		{
			rectangle.Offset(rectangle.Width + 1, 0);
		}
		else
		{
			rectangle.Offset(0, rectangle.Height + 1);
		}
		if (rectangle.Contains(و, ى))
		{
			if (ᲃ)
			{
				return _1886._1C8D;
			}
			return _1886._1C8F;
		}
		if (Orientation == eOrientation.Horizontal)
		{
			rectangle.Offset(rectangle.Width + 3, 0);
		}
		else
		{
			rectangle.Offset(0, rectangle.Height + 3);
		}
		if (rectangle.Contains(و, ى))
		{
			return _1886.ᆤ;
		}
		return _1886.ᘶ;
	}

	private Bitmap _1C89(_1886 ख़)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		if (ख़ == _1886.ᘶ)
		{
			return null;
		}
		Rectangle rectangle = new Rectangle(0, 0, ᱼ().Width - 2, ᱼ().Height - 2);
		Bitmap val = new Bitmap(rectangle.Width, rectangle.Height, (PixelFormat)137224);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		rectangle.Inflate(0, -1);
		Rectangle clip = rectangle;
		clip.Inflate(-1, -1);
		SolidBrush val3 = new SolidBrush(SystemColors.Control);
		try
		{
			val2.FillRectangle((Brush)(object)val3, 0, 0, ((Image)val).Width, ((Image)val).Height);
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		val2.SetClip(clip);
		ControlPaint.DrawCaptionButton(val2, rectangle, (CaptionButton)ख़, (ButtonState)16384);
		val2.ResetClip();
		val2.Dispose();
		val.MakeTransparent(SystemColors.Control);
		return val;
	}

	private void _1C8A()
	{
		using LocalizationManager localizationManager = new LocalizationManager(GetOwner() as IOwnerLocalize);
		SubItems.Clear();
		PopupType = ePopupType.Menu;
		ButtonItem buttonItem = new ButtonItem("dotnetbarsysmenurestore");
		buttonItem.GlobalItem = false;
		buttonItem.Text = localizationManager.GetLocalizedString(LocalizationKeys.MdiSystemItemMenuRestore);
		buttonItem.Image = (Image)(object)_1C89(_1886._1C8D);
		buttonItem.Click += _1C8B;
		buttonItem.Enabled = ᲃ;
		SubItems.Add(buttonItem);
		buttonItem = new ButtonItem();
		buttonItem.GlobalItem = false;
		buttonItem.Text = localizationManager.GetLocalizedString(LocalizationKeys.MdiSystemItemMenuMove);
		buttonItem.Enabled = false;
		SubItems.Add(buttonItem);
		buttonItem = new ButtonItem();
		buttonItem.GlobalItem = false;
		buttonItem.Text = localizationManager.GetLocalizedString(LocalizationKeys.MdiSystemItemMenuSize);
		buttonItem.Enabled = false;
		SubItems.Add(buttonItem);
		buttonItem = new ButtonItem("dotnetbarsysmenuminimize");
		buttonItem.GlobalItem = false;
		buttonItem.Text = localizationManager.GetLocalizedString(LocalizationKeys.MdiSystemItemMenuMinimize);
		buttonItem.Image = (Image)(object)_1C89(_1886._1C8C);
		buttonItem.Click += _1C8B;
		buttonItem.Enabled = ᲂ;
		SubItems.Add(buttonItem);
		buttonItem = new ButtonItem();
		buttonItem.GlobalItem = false;
		buttonItem.Text = localizationManager.GetLocalizedString(LocalizationKeys.MdiSystemItemMenuMaximize);
		buttonItem.Image = (Image)(object)_1C89(_1886._1C8F);
		buttonItem.Enabled = false;
		SubItems.Add(buttonItem);
		buttonItem = new ButtonItem("dotnetbarsysmenuclose");
		buttonItem.GlobalItem = false;
		buttonItem.Text = localizationManager.GetLocalizedString(LocalizationKeys.MdiSystemItemMenuClose);
		buttonItem.Image = (Image)(object)_1C89(_1886.ᆤ);
		buttonItem.BeginGroup = true;
		buttonItem.Click += _1C8B;
		buttonItem.Shortcuts.Add(eShortcut.CtrlF4);
		buttonItem.Enabled = ᲄ;
		SubItems.Add(buttonItem);
		buttonItem = new ButtonItem("dotnetbarsysmenunext");
		buttonItem.GlobalItem = false;
		buttonItem.Text = localizationManager.GetLocalizedString(LocalizationKeys.MdiSystemItemMenuNext);
		buttonItem.BeginGroup = true;
		buttonItem.Shortcuts.Add(eShortcut.CtrlF6);
		buttonItem.Click += _1C8B;
		SubItems.Add(buttonItem);
	}

	private void _1C8B(object _٠, EventArgs ؿ)
	{
		BaseItem.CollapseAll(this);
		if (_٠ is BaseItem baseItem)
		{
			ᲅ = _1886.ᘶ;
			if (baseItem.Name == "dotnetbarsysmenurestore")
			{
				ᲅ = _1886._1C8D;
			}
			else if (baseItem.Name == "dotnetbarsysmenuminimize")
			{
				ᲅ = _1886._1C8C;
			}
			else if (baseItem.Name == "dotnetbarsysmenuclose")
			{
				ᲅ = _1886.ᆤ;
			}
			else if (baseItem.Name == "dotnetbarsysmenunext")
			{
				ᲅ = _1886.Ა;
			}
			if (ᲅ != _1886.ᘶ)
			{
				RaiseClick();
			}
		}
	}
}
