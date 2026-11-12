using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class MDISystemItem : PopupItem
{
	private bool m_IsSystemIcon;

	private Icon m_Icon;

	private bool m_MinimizeEnabled;

	private bool m_RestoreEnabled;

	private bool m_CloseEnabled;

	private SystemButton m_MouseOver;

	private SystemButton m_MouseDown;

	private SystemButton m_LastButtonClick;

	public bool IsSystemIcon
	{
		get
		{
			return m_IsSystemIcon;
		}
		set
		{
			if (m_IsSystemIcon != value)
			{
				m_IsSystemIcon = value;
				NeedRecalcSize = true;
				if (m_IsSystemIcon)
				{
					CreateSystemMenu();
					ShowSubItems = true;
				}
				else
				{
					SubItems.Clear();
					ShowSubItems = false;
				}
				ShowSubItems = m_IsSystemIcon;
				Refresh();
			}
		}
	}

	public Icon Icon
	{
		get
		{
			return m_Icon;
		}
		set
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			if (value != null)
			{
				m_Icon = new Icon(value, 16, 16);
			}
			else
			{
				m_Icon = null;
			}
			Refresh();
		}
	}

	internal SystemButton LastButtonClick => m_LastButtonClick;

	public bool MinimizeEnabled
	{
		get
		{
			return m_MinimizeEnabled;
		}
		set
		{
			if (value != m_MinimizeEnabled)
			{
				m_MinimizeEnabled = value;
				Refresh();
			}
		}
	}

	public bool RestoreEnabled
	{
		get
		{
			return m_RestoreEnabled;
		}
		set
		{
			if (value != m_RestoreEnabled)
			{
				m_RestoreEnabled = value;
				Refresh();
			}
		}
	}

	public bool CloseEnabled
	{
		get
		{
			return m_CloseEnabled;
		}
		set
		{
			if (value != m_CloseEnabled)
			{
				m_CloseEnabled = value;
				Refresh();
			}
		}
	}

	internal SystemButton MouseOverButton => m_MouseOver;

	internal SystemButton MouseDownButton => m_MouseDown;

	public MDISystemItem()
		: this("")
	{
	}

	public MDISystemItem(string sName)
		: base(sName)
	{
		GlobalItem = false;
		SetSystemItem(b: true);
		m_IsSystemIcon = false;
		m_Icon = null;
		m_MinimizeEnabled = true;
		m_RestoreEnabled = true;
		m_CloseEnabled = true;
		m_MouseOver = SystemButton.None;
		m_MouseDown = SystemButton.None;
		m_LastButtonClick = SystemButton.None;
		CanCustomize = false;
		Tooltip = "";
		ShowSubItems = false;
		m_ShouldSerialize = false;
		IsAccessible = false;
	}

	protected override void Dispose(bool disposing)
	{
		if (m_Icon != null)
		{
			m_Icon.Dispose();
			m_Icon = null;
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
		mDISystemItem.MinimizeEnabled = m_MinimizeEnabled;
		mDISystemItem.RestoreEnabled = m_RestoreEnabled;
		mDISystemItem.CloseEnabled = m_CloseEnabled;
		return mDISystemItem;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		MDISystemItem mDISystemItem = copy as MDISystemItem;
		base.CopyToItem((BaseItem)mDISystemItem);
		mDISystemItem.IsSystemIcon = m_IsSystemIcon;
	}

	public override void Paint(ItemPaintArgs pa)
	{
		if (SuspendLayout)
		{
			return;
		}
		if (BarFunctions.IsOffice2007Style(EffectiveStyle) && pa.Renderer != null)
		{
			pa.Renderer.DrawMdiSystemItem(new MdiSystemItemRendererEventArgs(pa.Graphics, this));
			return;
		}
		Graphics graphics = pa.Graphics;
		Rectangle displayRectangle;
		if (m_IsSystemIcon)
		{
			displayRectangle = DisplayRectangle;
			displayRectangle.Offset((displayRectangle.Width - 16) / 2, (displayRectangle.Height - 16) / 2);
			if (m_Icon != null)
			{
				graphics.DrawIconUnstretched(m_Icon, displayRectangle);
			}
		}
		else if (EffectiveStyle == eDotNetBarStyle.Office2000)
		{
			displayRectangle = new Rectangle(DisplayRectangle.Location, GetButtonSize());
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
			if (!m_MinimizeEnabled)
			{
				ControlPaint.DrawCaptionButton(graphics, displayRectangle, (CaptionButton)1, (ButtonState)256);
			}
			else if (m_MouseDown == SystemButton.Minimize)
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
			if (!m_RestoreEnabled)
			{
				ControlPaint.DrawCaptionButton(graphics, displayRectangle, (CaptionButton)3, (ButtonState)256);
			}
			else if (m_MouseDown == SystemButton.Restore)
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
			if (!m_CloseEnabled)
			{
				ControlPaint.DrawCaptionButton(graphics, displayRectangle, (CaptionButton)0, (ButtonState)256);
			}
			else if (m_MouseDown == SystemButton.Close)
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
			displayRectangle = new Rectangle(DisplayRectangle.Location, GetButtonSize());
			displayRectangle.Inflate(-1, -1);
			if (Orientation == eOrientation.Horizontal)
			{
				displayRectangle.Offset(0, (DisplayRectangle.Height - displayRectangle.Height) / 2);
			}
			else
			{
				displayRectangle.Offset((WidthInternal - displayRectangle.Width) / 2, 0);
			}
			PaintButton(pa, SystemButton.Minimize, displayRectangle);
			if (Orientation == eOrientation.Horizontal)
			{
				displayRectangle.Offset(displayRectangle.Width, 0);
			}
			else
			{
				displayRectangle.Offset(0, displayRectangle.Height);
			}
			PaintButton(pa, SystemButton.Restore, displayRectangle);
			if (Orientation == eOrientation.Horizontal)
			{
				displayRectangle.Offset(displayRectangle.Width + 2, 0);
			}
			else
			{
				displayRectangle.Offset(0, displayRectangle.Height + 2);
			}
			PaintButton(pa, SystemButton.Close, displayRectangle);
		}
	}

	private void PaintButton(ItemPaintArgs pa, SystemButton btn, Rectangle r)
	{
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Expected O, but got Unknown
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Expected O, but got Unknown
		Graphics graphics = pa.Graphics;
		if (base.IsThemed)
		{
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				ThemeWindow themeWindow = pa.ThemeWindow;
				ThemeWindowParts part = ThemeWindowParts.MdiMinButton;
				ThemeWindowStates state = ThemeWindowStates.ButtonNormal;
				switch (btn)
				{
				case SystemButton.Close:
					part = ThemeWindowParts.MdiCloseButton;
					break;
				case SystemButton.Help:
					part = ThemeWindowParts.MdiHelpButton;
					break;
				case SystemButton.Restore:
					part = ThemeWindowParts.MdiRestoreButton;
					break;
				}
				if (btn == m_MouseDown)
				{
					state = ThemeWindowStates.ButtonPushed;
				}
				else if (btn == m_MouseOver)
				{
					state = ThemeWindowStates.ButtonHot;
				}
				themeWindow.DrawBackground(graphics, part, state, r);
				return;
			}
		}
		if (btn == m_MouseDown)
		{
			if (pa.Colors.ItemPressedBackground2.IsEmpty)
			{
				graphics.FillRectangle((Brush)new SolidBrush(pa.Colors.ItemPressedBackground), r);
			}
			else
			{
				LinearGradientBrush val2 = BarFunctions.CreateLinearGradientBrush(r, pa.Colors.ItemPressedBackground, pa.Colors.ItemPressedBackground2, pa.Colors.ItemPressedBackgroundGradientAngle);
				graphics.FillRectangle((Brush)(object)val2, r);
				((Brush)val2).Dispose();
			}
			NativeFunctions.DrawRectangle(graphics, SystemPens.Highlight, r);
		}
		else if (btn == m_MouseOver)
		{
			if (!pa.Colors.ItemHotBackground2.IsEmpty)
			{
				LinearGradientBrush val3 = BarFunctions.CreateLinearGradientBrush(r, pa.Colors.ItemHotBackground, pa.Colors.ItemHotBackground2, pa.Colors.ItemHotBackgroundGradientAngle);
				graphics.FillRectangle((Brush)(object)val3, r);
				((Brush)val3).Dispose();
			}
			else
			{
				graphics.FillRectangle((Brush)new SolidBrush(pa.Colors.ItemHotBackground), r);
			}
			NativeFunctions.DrawRectangle(graphics, new Pen(pa.Colors.ItemHotBorder), r);
		}
		Bitmap buttonBitmap = GetButtonBitmap(graphics, btn, r, pa.Colors);
		try
		{
			if ((btn == SystemButton.Minimize && !m_MinimizeEnabled) || (btn == SystemButton.Restore && !m_RestoreEnabled) || (btn == SystemButton.Close && !m_CloseEnabled))
			{
				ColorMatrix colorMatrix = new ColorMatrix(new float[5][]
				{
					new float[5],
					new float[5],
					new float[5],
					new float[5] { 0.5f, 0.5f, 0.5f, 0.5f, 0f },
					new float[5]
				});
				ImageAttributes val4 = new ImageAttributes();
				val4.ClearColorKey();
				val4.SetColorMatrix(colorMatrix);
				graphics.DrawImage((Image)(object)buttonBitmap, r, 0, 0, ((Image)buttonBitmap).Width, ((Image)buttonBitmap).Height, (GraphicsUnit)2, val4);
			}
			else
			{
				if (btn == m_MouseDown)
				{
					r.Offset(1, 1);
				}
				graphics.DrawImageUnscaled((Image)(object)buttonBitmap, r);
			}
		}
		finally
		{
			((IDisposable)buttonBitmap)?.Dispose();
		}
	}

	internal Bitmap GetButtonBitmap(Graphics g, SystemButton btn, Rectangle r, ColorScheme colorScheme)
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
		Bitmap val = new Bitmap(r.Width, r.Height, g);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		Rectangle rectangle = new Rectangle(0, 0, r.Width, r.Height);
		rectangle.Inflate(0, -1);
		Rectangle clip = rectangle;
		clip.Inflate(-1, -1);
		SolidBrush val3 = new SolidBrush(SystemColors.Control);
		try
		{
			val2.FillRectangle((Brush)(object)val3, 0, 0, r.Width, r.Height);
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		val2.SetClip(clip);
		ControlPaint.DrawCaptionButton(val2, rectangle, (CaptionButton)btn, (ButtonState)16384);
		val2.ResetClip();
		val2.Dispose();
		if (!colorScheme.MdiSystemItemForeground.IsEmpty)
		{
			Bitmap val4 = new Bitmap((Image)(object)val, ((Image)val).Width, ((Image)val).Height);
			Graphics val5 = Graphics.FromImage((Image)(object)val4);
			try
			{
				val5.Clear(Color.Transparent);
				ImageAttributes val6 = new ImageAttributes();
				ColorMap val7 = new ColorMap();
				val7.OldColor = Color.Black;
				val7.NewColor = colorScheme.MdiSystemItemForeground;
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
		SystemButton button = GetButton(point.X, point.Y);
		using LocalizationManager localizationManager = new LocalizationManager(GetOwner() as IOwnerLocalize);
		switch (button)
		{
		case SystemButton.Minimize:
			Tooltip = localizationManager.GetLocalizedString(LocalizationKeys.MdiSystemItemMinimizeTooltip);
			break;
		case SystemButton.Restore:
			Tooltip = localizationManager.GetLocalizedString(LocalizationKeys.MdiSystemItemRestoreTooltip);
			break;
		case SystemButton.Maximize:
			Tooltip = localizationManager.GetLocalizedString(LocalizationKeys.MdiSystemItemMenuMaximize);
			break;
		case SystemButton.Close:
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
		SystemButton button = GetButton(objArg.X, objArg.Y);
		if (button == SystemButton.Help)
		{
			m_MouseDown = SystemButton.Help;
			Refresh();
		}
		else if (m_MinimizeEnabled && button == SystemButton.Minimize)
		{
			m_MouseDown = SystemButton.Minimize;
			Refresh();
		}
		else if (m_RestoreEnabled && button == SystemButton.Restore)
		{
			m_MouseDown = SystemButton.Restore;
			Refresh();
		}
		else if (!m_RestoreEnabled && button == SystemButton.Maximize)
		{
			m_MouseDown = SystemButton.Maximize;
			Refresh();
		}
		else if (m_CloseEnabled && button == SystemButton.Close)
		{
			m_MouseDown = SystemButton.Close;
			Refresh();
		}
		else
		{
			m_MouseDown = SystemButton.None;
			Refresh();
		}
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		base.InternalMouseUp(objArg);
		if (m_MouseDown != SystemButton.None)
		{
			m_MouseDown = SystemButton.None;
			Refresh();
		}
	}

	public override void InternalClick(MouseButtons mb, Point mpos)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		if (IsSystemIcon)
		{
			m_LastButtonClick = SystemButton.None;
			base.InternalClick(mb, mpos);
			return;
		}
		Point point = mpos;
		m_LastButtonClick = GetButton(point.X, point.Y);
		if ((m_LastButtonClick == SystemButton.Close && !m_CloseEnabled) || (m_LastButtonClick == SystemButton.Minimize && !m_MinimizeEnabled) || (m_LastButtonClick == SystemButton.Restore && !m_RestoreEnabled) || (m_LastButtonClick == SystemButton.Maximize && m_RestoreEnabled))
		{
			m_LastButtonClick = SystemButton.None;
		}
		if (m_LastButtonClick != SystemButton.None)
		{
			base.InternalClick(mb, mpos);
		}
	}

	public override void InternalMouseLeave()
	{
		base.InternalMouseLeave();
		m_MouseDown = SystemButton.None;
		m_MouseOver = SystemButton.None;
		Refresh();
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		base.InternalMouseMove(objArg);
		switch (GetButton(objArg.X, objArg.Y))
		{
		case SystemButton.Help:
			if (m_MouseOver != SystemButton.Help)
			{
				m_MouseOver = SystemButton.Help;
				if (base.ToolTipVisible)
				{
					HideToolTip();
					ResetHover();
				}
				Refresh();
			}
			return;
		case SystemButton.Minimize:
			if (m_MouseOver != SystemButton.Minimize)
			{
				m_MouseOver = SystemButton.Minimize;
				if (base.ToolTipVisible)
				{
					HideToolTip();
					ResetHover();
				}
				Refresh();
			}
			return;
		case SystemButton.Restore:
			if (m_MouseOver != SystemButton.Restore)
			{
				m_MouseOver = SystemButton.Restore;
				if (base.ToolTipVisible)
				{
					HideToolTip();
					ResetHover();
				}
				Refresh();
			}
			return;
		case SystemButton.Maximize:
			if (m_MouseOver != SystemButton.Maximize)
			{
				m_MouseOver = SystemButton.Maximize;
				if (base.ToolTipVisible)
				{
					HideToolTip();
					ResetHover();
				}
				Refresh();
			}
			return;
		case SystemButton.Close:
			if (m_MouseOver != 0)
			{
				m_MouseOver = SystemButton.Close;
				if (base.ToolTipVisible)
				{
					HideToolTip();
					ResetHover();
				}
				Refresh();
			}
			return;
		}
		if (m_MouseOver != SystemButton.None)
		{
			m_MouseOver = SystemButton.None;
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
			if (m_IsSystemIcon)
			{
				m_Rect.Size = new Size(16, 16);
			}
			else if (Orientation == eOrientation.Horizontal)
			{
				m_Rect.Size = new Size(GetButtonSize().Width * 3 + 2, GetButtonSize().Height);
			}
			else
			{
				m_Rect.Size = new Size(GetButtonSize().Width, GetButtonSize().Height * 3 + 2);
			}
			base.RecalcSize();
		}
	}

	internal void SetIcon(Icon icon)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		if (m_Icon != null)
		{
			m_Icon.Dispose();
		}
		if (icon != null)
		{
			m_Icon = new Icon(icon, 16, 16);
		}
		else
		{
			m_Icon = null;
		}
	}

	internal virtual Size GetButtonSize()
	{
		return SystemInformation.MenuButtonSize;
	}

	internal virtual SystemButton GetButton(int x, int y)
	{
		Rectangle rectangle = new Rectangle(DisplayRectangle.Location, GetButtonSize());
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
		if (rectangle.Contains(x, y))
		{
			return SystemButton.Minimize;
		}
		if (Orientation == eOrientation.Horizontal)
		{
			rectangle.Offset(rectangle.Width + 1, 0);
		}
		else
		{
			rectangle.Offset(0, rectangle.Height + 1);
		}
		if (rectangle.Contains(x, y))
		{
			if (m_RestoreEnabled)
			{
				return SystemButton.Restore;
			}
			return SystemButton.Maximize;
		}
		if (Orientation == eOrientation.Horizontal)
		{
			rectangle.Offset(rectangle.Width + 3, 0);
		}
		else
		{
			rectangle.Offset(0, rectangle.Height + 3);
		}
		if (rectangle.Contains(x, y))
		{
			return SystemButton.Close;
		}
		return SystemButton.None;
	}

	private Bitmap GetSysImage(SystemButton btn)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		if (btn == SystemButton.None)
		{
			return null;
		}
		Rectangle rectangle = new Rectangle(0, 0, GetButtonSize().Width - 2, GetButtonSize().Height - 2);
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
		ControlPaint.DrawCaptionButton(val2, rectangle, (CaptionButton)btn, (ButtonState)16384);
		val2.ResetClip();
		val2.Dispose();
		val.MakeTransparent(SystemColors.Control);
		return val;
	}

	private void CreateSystemMenu()
	{
		using LocalizationManager localizationManager = new LocalizationManager(GetOwner() as IOwnerLocalize);
		SubItems.Clear();
		PopupType = ePopupType.Menu;
		ButtonItem buttonItem = new ButtonItem("dotnetbarsysmenurestore");
		buttonItem.GlobalItem = false;
		buttonItem.Text = localizationManager.GetLocalizedString(LocalizationKeys.MdiSystemItemMenuRestore);
		buttonItem.Image = (Image)(object)GetSysImage(SystemButton.Restore);
		buttonItem.Click += MenuClick;
		buttonItem.Enabled = m_RestoreEnabled;
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
		buttonItem.Image = (Image)(object)GetSysImage(SystemButton.Minimize);
		buttonItem.Click += MenuClick;
		buttonItem.Enabled = m_MinimizeEnabled;
		SubItems.Add(buttonItem);
		buttonItem = new ButtonItem();
		buttonItem.GlobalItem = false;
		buttonItem.Text = localizationManager.GetLocalizedString(LocalizationKeys.MdiSystemItemMenuMaximize);
		buttonItem.Image = (Image)(object)GetSysImage(SystemButton.Maximize);
		buttonItem.Enabled = false;
		SubItems.Add(buttonItem);
		buttonItem = new ButtonItem("dotnetbarsysmenuclose");
		buttonItem.GlobalItem = false;
		buttonItem.Text = localizationManager.GetLocalizedString(LocalizationKeys.MdiSystemItemMenuClose);
		buttonItem.Image = (Image)(object)GetSysImage(SystemButton.Close);
		buttonItem.BeginGroup = true;
		buttonItem.Click += MenuClick;
		buttonItem.Shortcuts.Add(eShortcut.CtrlF4);
		buttonItem.Enabled = m_CloseEnabled;
		SubItems.Add(buttonItem);
		buttonItem = new ButtonItem("dotnetbarsysmenunext");
		buttonItem.GlobalItem = false;
		buttonItem.Text = localizationManager.GetLocalizedString(LocalizationKeys.MdiSystemItemMenuNext);
		buttonItem.BeginGroup = true;
		buttonItem.Shortcuts.Add(eShortcut.CtrlF6);
		buttonItem.Click += MenuClick;
		SubItems.Add(buttonItem);
	}

	private void MenuClick(object sender, EventArgs e)
	{
		BaseItem.CollapseAll(this);
		if (sender is BaseItem baseItem)
		{
			m_LastButtonClick = SystemButton.None;
			if (baseItem.Name == "dotnetbarsysmenurestore")
			{
				m_LastButtonClick = SystemButton.Restore;
			}
			else if (baseItem.Name == "dotnetbarsysmenuminimize")
			{
				m_LastButtonClick = SystemButton.Minimize;
			}
			else if (baseItem.Name == "dotnetbarsysmenuclose")
			{
				m_LastButtonClick = SystemButton.Close;
			}
			else if (baseItem.Name == "dotnetbarsysmenunext")
			{
				m_LastButtonClick = SystemButton.NextWindow;
			}
			if (m_LastButtonClick != SystemButton.None)
			{
				RaiseClick();
			}
		}
	}
}
