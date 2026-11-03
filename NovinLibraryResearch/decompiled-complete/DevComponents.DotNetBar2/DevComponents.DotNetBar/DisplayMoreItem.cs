using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

[DesignTimeVisible(false)]
[ToolboxItem(false)]
public class DisplayMoreItem : PopupItem
{
	private const int _෯ = 14;

	private bool ݡ;

	private bool _0DF0;

	private eOrientation _0DF1;

	private bool _0DF2;

	public static int FixedSize => 14;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override bool NeedRecalcSize
	{
		get
		{
			return base.NeedRecalcSize;
		}
		set
		{
			m_NeedRecalcSize = value;
		}
	}

	[Browsable(false)]
	public bool IsMouseOver => ݡ;

	public DisplayMoreItem()
	{
		GlobalItem = false;
		ݡ = false;
		_0DF0 = false;
		m_Tooltip = "More Buttons";
		m_SystemItem = true;
		CanCustomize = false;
		m_ShouldSerialize = false;
		using LocalizationManager localizationManager = new LocalizationManager(null);
		m_Tooltip = localizationManager.GetLocalizedString(LocalizationKeys.OverlfowDisplayMoreTooltip);
	}

	public override BaseItem Copy()
	{
		DisplayMoreItem displayMoreItem = new DisplayMoreItem();
		CopyToItem(displayMoreItem);
		return displayMoreItem;
	}

	protected internal override void OnContainerChanged(object objOldContainer)
	{
		base.OnContainerChanged(objOldContainer);
		if (!_0DF2)
		{
			_0DF2 = true;
			using LocalizationManager localizationManager = new LocalizationManager(GetOwner() as IOwnerLocalize);
			m_Tooltip = localizationManager.GetLocalizedString(LocalizationKeys.OverlfowDisplayMoreTooltip);
		}
	}

	public override void Paint(ItemPaintArgs pa)
	{
		if (SuspendLayout)
		{
			return;
		}
		eDotNetBarStyle effectiveStyle = EffectiveStyle;
		if (effectiveStyle == eDotNetBarStyle.Office2000)
		{
			_0DF5(pa);
		}
		else
		{
			if (base.IsThemed)
			{
				_0DF4(pa);
				return;
			}
			if (effectiveStyle == eDotNetBarStyle.Office2003 || effectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(effectiveStyle))
			{
				_0DF6(pa);
				return;
			}
			_0DF3(pa);
		}
		Graphics graphics = pa.Graphics;
		if (m_Orientation == eOrientation.Vertical)
		{
			Point[] array = new Point[3];
			array[0].X = m_Rect.Right - 8;
			array[0].Y = m_Rect.Top + 4;
			array[1].X = array[0].X + 2;
			array[1].Y = array[0].Y + 2;
			array[2].X = array[0].X + 4;
			array[2].Y = array[0].Y;
			graphics.DrawLines(SystemPens.ControlText, array);
			array[0].Y++;
			array[1].Y++;
			array[2].Y++;
			graphics.DrawLines(SystemPens.ControlText, array);
			array[0].Y += 3;
			array[1].Y += 3;
			array[2].Y += 3;
			graphics.DrawLines(SystemPens.ControlText, array);
			array[0].Y++;
			array[1].Y++;
			array[2].Y++;
			graphics.DrawLines(SystemPens.ControlText, array);
			array[0].X = m_Rect.Left + 7;
			array[0].Y = m_Rect.Top + 4;
			array[1].X = array[0].X - 3;
			array[1].Y = array[0].Y + 3;
			array[2].X = array[0].X;
			array[2].Y = array[0].Y + 6;
			graphics.FillPolygon(SystemBrushes.ControlText, array);
		}
		else
		{
			Point[] array2 = new Point[3];
			array2[0].X = m_Rect.Left + (m_Rect.Width - 7) / 2 - 1;
			array2[0].Y = m_Rect.Top + 4;
			array2[1].X = array2[0].X + 2;
			array2[1].Y = array2[0].Y + 2;
			array2[2].X = array2[0].X;
			array2[2].Y = array2[0].Y + 4;
			graphics.DrawLines(SystemPens.ControlText, array2);
			array2[0].X++;
			array2[1].X++;
			array2[2].X++;
			graphics.DrawLines(SystemPens.ControlText, array2);
			array2[0].X += 3;
			array2[1].X += 3;
			array2[2].X += 3;
			graphics.DrawLines(SystemPens.ControlText, array2);
			array2[0].X++;
			array2[1].X++;
			array2[2].X++;
			graphics.DrawLines(SystemPens.ControlText, array2);
			array2[0].X = m_Rect.Left + m_Rect.Width / 2 - 1;
			array2[0].Y = m_Rect.Bottom - 4;
			array2[1].X = array2[0].X - 2;
			array2[1].Y = array2[0].Y - 3;
			array2[2].X = array2[1].X + 5;
			array2[2].Y = array2[1].Y;
			graphics.FillPolygon(SystemBrushes.ControlText, array2);
		}
	}

	private void _0DF3(ItemPaintArgs _0BD2)
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Expected O, but got Unknown
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Expected O, but got Unknown
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Expected O, but got Unknown
		Graphics graphics = _0BD2.Graphics;
		if (m_Expanded)
		{
			Rectangle rectangle = new Rectangle(m_Rect.Left + 2, m_Rect.Top + 2, m_Rect.Width - 2, m_Rect.Height - 2);
			if (!_0BD2.Colors.ItemExpandedShadow.IsEmpty)
			{
				SolidBrush val = new SolidBrush(_0BD2.Colors.ItemExpandedShadow);
				try
				{
					graphics.FillRectangle((Brush)(object)val, rectangle);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			rectangle.Offset(-2, -2);
			rectangle.Height += 2;
			if (_0BD2.Colors.ItemExpandedBackground2.IsEmpty)
			{
				graphics.FillRectangle((Brush)new SolidBrush(_0BD2.Colors.ItemExpandedBackground), rectangle);
			}
			else
			{
				LinearGradientBrush val2 = BarFunctions.ॴ(rectangle, _0BD2.Colors.ItemExpandedBackground, _0BD2.Colors.ItemExpandedBackground2, _0BD2.Colors.ItemExpandedBackgroundGradientAngle);
				graphics.FillRectangle((Brush)(object)val2, rectangle);
				((Brush)val2).Dispose();
			}
			_089F.र(graphics, new Pen(_0BD2.Colors.MenuBorder, 1f), rectangle);
		}
		else if (ݡ)
		{
			Rectangle rectangle = new Rectangle(m_Rect.Left, m_Rect.Top, m_Rect.Width - 2, m_Rect.Height);
			if (!_0BD2.Colors.ItemHotBackground2.IsEmpty)
			{
				LinearGradientBrush val3 = BarFunctions.ॴ(rectangle, _0BD2.Colors.ItemHotBackground, _0BD2.Colors.ItemHotBackground2, _0BD2.Colors.ItemHotBackgroundGradientAngle);
				graphics.FillRectangle((Brush)(object)val3, rectangle);
				((Brush)val3).Dispose();
			}
			else
			{
				graphics.FillRectangle((Brush)new SolidBrush(_0BD2.Colors.ItemHotBackground), rectangle);
			}
			_089F.र(graphics, new Pen(_0BD2.Colors.ItemHotBorder), rectangle);
		}
	}

	private void _0DF4(ItemPaintArgs _0BD2)
	{
		_0A7A obj = _0BD2._0A7A;
		ཤ ཀྵ = ཤ.ཀྵ;
		ཫ య = ཫ.ཬ;
		if (m_Expanded)
		{
			య = ཫ._0F6E;
		}
		else if (ݡ)
		{
			య = ཫ._0F6D;
		}
		obj._0EE5(_0BD2.Graphics, ཀྵ, య, m_Rect);
	}

	private void _0DF5(ItemPaintArgs _0BD2)
	{
		Graphics graphics = _0BD2.Graphics;
		if (m_Expanded)
		{
			ControlPaint.DrawBorder3D(graphics, m_Rect, (Border3DStyle)2);
		}
		else if (ݡ)
		{
			ControlPaint.DrawBorder3D(graphics, m_Rect, (Border3DStyle)4);
		}
	}

	private void _0DF6(ItemPaintArgs _0BD2)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_087a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0881: Expected O, but got Unknown
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0483: Expected O, but got Unknown
		//IL_08a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b0: Expected O, but got Unknown
		//IL_0af5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afc: Expected O, but got Unknown
		Graphics graphics = _0BD2.Graphics;
		Rectangle rect = m_Rect;
		GraphicsPath val = new GraphicsPath();
		if (Orientation == eOrientation.Vertical)
		{
			rect.Y += 2;
			rect.Height--;
			rect.X -= 2;
			rect.Width += 2;
			val.AddLine(rect.X, rect.Y, rect.X + 2, rect.Y + 2);
			val.AddLine(rect.Right - 2, rect.Y + 2, rect.Right, rect.Y);
			val.AddLine(rect.Right, rect.Bottom - 2, rect.Right - 2, rect.Bottom);
			val.AddLine(rect.X + 2, rect.Bottom, rect.X, rect.Bottom - 2);
			val.CloseAllFigures();
		}
		else
		{
			rect.X += 2;
			rect.Width--;
			rect.Y -= 2;
			rect.Height += 3;
			val.AddLine(rect.X, rect.Y, rect.X + 2, rect.Y + 2);
			val.AddLine(rect.X + 2, rect.Bottom - 2, rect.X, rect.Bottom);
			val.AddLine(rect.Right - 2, rect.Bottom, rect.Right, rect.Bottom - 2);
			val.AddLine(rect.Right, rect.Y + 2, rect.Right - 2, rect.Y);
			val.CloseAllFigures();
		}
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		graphics.SmoothingMode = (SmoothingMode)4;
		if (Expanded)
		{
			LinearGradientBrush val2 = BarFunctions.ॴ(rect, _0BD2.Colors.ItemPressedBackground, _0BD2.Colors.ItemPressedBackground2, _0BD2.Colors.ItemPressedBackgroundGradientAngle);
			graphics.FillPath((Brush)(object)val2, val);
			((Brush)val2).Dispose();
		}
		else if (ݡ)
		{
			LinearGradientBrush val3 = BarFunctions.ॴ(rect, _0BD2.Colors.ItemHotBackground, _0BD2.Colors.ItemHotBackground2, _0BD2.Colors.ItemHotBackgroundGradientAngle);
			graphics.FillPath((Brush)(object)val3, val);
			((Brush)val3).Dispose();
		}
		else
		{
			LinearGradientBrush val4 = BarFunctions.ॴ(rect, _0BD2.Colors.CustomizeBackground, _0BD2.Colors.CustomizeBackground2, _0BD2.Colors.CustomizeBackgroundGradientAngle);
			graphics.FillPath((Brush)(object)val4, val);
			((Brush)val4).Dispose();
		}
		graphics.SmoothingMode = smoothingMode;
		if (Orientation == eOrientation.Vertical)
		{
			graphics.DrawLine(SystemPens.HighlightText, rect.Left + (m_Rect.Width - 4) / 2, rect.Bottom - 10 + 1, rect.Left + (m_Rect.Width - 4) / 2 + 4, rect.Bottom - 10 + 1);
			graphics.DrawLine(SystemPens.HighlightText, rect.Left + (m_Rect.Width - 4) / 2 - 1 + 1, rect.Top + 6 + 1, rect.Left + (m_Rect.Width - 4) / 2 - 1 + 1, rect.Top + 8 + 1);
			graphics.DrawLine(SystemPens.HighlightText, rect.Left + (m_Rect.Width - 4) / 2 + 3 + 1, rect.Top + 6 + 1, rect.Left + (m_Rect.Width - 4) / 2 + 3 + 1, rect.Top + 8 + 1);
			graphics.DrawLine(SystemPens.HighlightText, rect.Left + (m_Rect.Width - 4) / 2 - 1 + 1, rect.Top + 7 + 1, rect.Left + (m_Rect.Width - 4) / 2 + 1, rect.Top + 7 + 1);
			graphics.DrawLine(SystemPens.HighlightText, rect.Left + (m_Rect.Width - 4) / 2 + 3 + 1, rect.Top + 7 + 1, rect.Left + (m_Rect.Width - 4) / 2 + 4 + 1, rect.Top + 7 + 1);
			Pen val5 = new Pen(_0BD2.Colors.CustomizeText, 1f);
			try
			{
				graphics.DrawLine(val5, rect.Left + (m_Rect.Width - 4) / 2, rect.Bottom - 10, rect.Left + (m_Rect.Width - 4) / 2 + 4, rect.Bottom - 10);
				graphics.DrawLine(val5, rect.Left + (m_Rect.Width - 4) / 2 - 1, rect.Top + 6, rect.Left + (m_Rect.Width - 4) / 2 - 1, rect.Top + 8);
				graphics.DrawLine(val5, rect.Left + (m_Rect.Width - 4) / 2 + 3, rect.Top + 6, rect.Left + (m_Rect.Width - 4) / 2 + 3, rect.Top + 8);
				graphics.DrawLine(val5, rect.Left + (m_Rect.Width - 4) / 2 - 1, rect.Top + 7, rect.Left + (m_Rect.Width - 4) / 2, rect.Top + 7);
				graphics.DrawLine(val5, rect.Left + (m_Rect.Width - 4) / 2 + 3, rect.Top + 7, rect.Left + (m_Rect.Width - 4) / 2 + 4, rect.Top + 7);
				return;
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
		}
		graphics.DrawLine(SystemPens.HighlightText, rect.Left + (m_Rect.Width - 4) / 2 + 1, rect.Bottom - 11 + 1, rect.Left + (m_Rect.Width - 4) / 2 + 4 + 1, rect.Bottom - 11 + 1);
		graphics.DrawLine(SystemPens.HighlightText, rect.Left + (m_Rect.Width - 4) / 2 - 1 + 1, rect.Top + 6 + 1, rect.Left + (m_Rect.Width - 4) / 2 - 1 + 1, rect.Top + 8 + 1);
		graphics.DrawLine(SystemPens.HighlightText, rect.Left + (m_Rect.Width - 4) / 2 + 3 + 1, rect.Top + 6 + 1, rect.Left + (m_Rect.Width - 4) / 2 + 3 + 1, rect.Top + 8 + 1);
		graphics.DrawLine(SystemPens.HighlightText, rect.Left + (m_Rect.Width - 4) / 2 - 1 + 1, rect.Top + 7 + 1, rect.Left + (m_Rect.Width - 4) / 2 + 1, rect.Top + 7 + 1);
		graphics.DrawLine(SystemPens.HighlightText, rect.Left + (m_Rect.Width - 4) / 2 + 3 + 1, rect.Top + 7 + 1, rect.Left + (m_Rect.Width - 4) / 2 + 4 + 1, rect.Top + 7 + 1);
		Point[] array = new Point[3];
		array[0].X = rect.Left + (m_Rect.Width - 4) / 2 + 2 + 1;
		array[0].Y = rect.Bottom - 5 + 1;
		array[1].X = array[0].X - 2;
		array[1].Y = array[0].Y - 3;
		array[2].X = array[1].X + 5;
		array[2].Y = array[1].Y;
		SolidBrush val6 = new SolidBrush(SystemColors.HighlightText);
		try
		{
			graphics.FillPolygon((Brush)(object)val6, array);
		}
		finally
		{
			((IDisposable)val6)?.Dispose();
		}
		Pen val7 = new Pen(_0BD2.Colors.CustomizeText, 1f);
		try
		{
			graphics.DrawLine(val7, rect.Left + (m_Rect.Width - 4) / 2, rect.Bottom - 11, rect.Left + (m_Rect.Width - 4) / 2 + 4, rect.Bottom - 11);
			graphics.DrawLine(val7, rect.Left + (m_Rect.Width - 4) / 2 - 1, rect.Top + 6, rect.Left + (m_Rect.Width - 4) / 2 - 1, rect.Top + 8);
			graphics.DrawLine(val7, rect.Left + (m_Rect.Width - 4) / 2 + 3, rect.Top + 6, rect.Left + (m_Rect.Width - 4) / 2 + 3, rect.Top + 8);
			graphics.DrawLine(val7, rect.Left + (m_Rect.Width - 4) / 2 - 1, rect.Top + 7, rect.Left + (m_Rect.Width - 4) / 2, rect.Top + 7);
			graphics.DrawLine(val7, rect.Left + (m_Rect.Width - 4) / 2 + 3, rect.Top + 7, rect.Left + (m_Rect.Width - 4) / 2 + 4, rect.Top + 7);
		}
		finally
		{
			((IDisposable)val7)?.Dispose();
		}
		array = new Point[3];
		array[0].X = rect.Left + (m_Rect.Width - 4) / 2 + 2;
		array[0].Y = rect.Bottom - 5;
		array[1].X = array[0].X - 2;
		array[1].Y = array[0].Y - 3;
		array[2].X = array[1].X + 5;
		array[2].Y = array[1].Y;
		SolidBrush val8 = new SolidBrush(_0BD2.Colors.CustomizeText);
		try
		{
			graphics.FillPolygon((Brush)(object)val8, array);
		}
		finally
		{
			((IDisposable)val8)?.Dispose();
		}
	}

	public override void RecalcSize()
	{
		if (!SuspendLayout)
		{
			if (m_Orientation == eOrientation.Vertical)
			{
				m_Rect.Height = 14;
			}
			else
			{
				m_Rect.Width = 14;
			}
			base.RecalcSize();
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseEnter()
	{
		base.InternalMouseEnter();
		ݡ = true;
		Refresh();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseLeave()
	{
		base.InternalMouseLeave();
		ݡ = false;
		Refresh();
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		base.InternalMouseDown(objArg);
		if ((int)objArg.Button == 1048576)
		{
			Expanded = !m_Expanded;
		}
	}

	public override void Popup(Point p)
	{
		Popup(p.X, p.Y);
	}

	public override void Popup(int x, int y)
	{
		if (_0DF0)
		{
			PopupMenu(x, y);
		}
		else
		{
			PopupBar(x, y);
		}
	}

	public override void PopupBar(int x, int y)
	{
		AddItems();
		m_NeedRecalcSize = false;
		base.PopupBar(x, y);
		m_NeedRecalcSize = false;
	}

	public override void PopupMenu(int x, int y)
	{
		AddItems();
		m_NeedRecalcSize = false;
		base.PopupMenu(x, y);
		m_NeedRecalcSize = false;
	}

	public override void ClosePopup()
	{
		base.ClosePopup();
		SubItems.Clear();
	}

	protected virtual void AddItems()
	{
		if (m_SubItems == null)
		{
			m_SubItems = new SubItemsCollection(this);
		}
		_0DF1 = Orientation;
		foreach (BaseItem subItem in m_Parent.SubItems)
		{
			if (subItem.Displayed || !subItem.Visible)
			{
				continue;
			}
			m_SubItems.__0AF3(subItem);
			if (subItem is ImageItem { ImageSize: { Width: var width } } imageItem)
			{
				if (width > SubItemsImageSize.Width)
				{
					SubItemsImageSize = new Size(imageItem.ImageSize.Width, SubItemsImageSize.Height);
				}
				if (imageItem.ImageSize.Height > SubItemsImageSize.Height)
				{
					SubItemsImageSize = new Size(SubItemsImageSize.Width, imageItem.ImageSize.Height);
				}
			}
		}
		foreach (BaseItem subItem2 in m_SubItems)
		{
			object containerControl = subItem2.ContainerControl;
			m_Parent.SubItems._Remove(subItem2);
			subItem2.SetOrientation(eOrientation.Horizontal);
			subItem2.SetParent(this);
			subItem2.ContainerControl = null;
			subItem2.OnContainerChanged(containerControl);
		}
		NeedRecalcSize = true;
	}

	protected virtual int GetReInsertIndex()
	{
		int num = m_Parent.SubItems.Count;
		for (int num2 = num - 1; num2 >= 0; num2--)
		{
			if (m_Parent.SubItems[num2] is CustomizeItem)
			{
				num = num2;
				break;
			}
		}
		return num;
	}

	protected virtual void RemoveItems()
	{
		m_HotSubItem = null;
		int num = GetReInsertIndex();
		foreach (BaseItem subItem in m_SubItems)
		{
			_ = subItem.ContainerControl;
			subItem.Orientation = _0DF1;
			m_Parent.SubItems.__0AF3(subItem, num);
			num++;
			subItem.SetParent(m_Parent);
			subItem.ContainerControl = null;
		}
		ArrayList arrayList = new ArrayList(m_SubItems.Count);
		m_SubItems.CopyTo(arrayList);
		while (m_SubItems.Count > 0)
		{
			m_SubItems._Remove(m_SubItems[0]);
		}
		foreach (BaseItem item in arrayList)
		{
			item.Displayed = false;
		}
		base.OnExpandChange();
		if (base.PopupControl is Bar)
		{
			Bar bar = (Bar)(object)base.PopupControl;
			bar.ParentItem = null;
		}
		BaseItem parent = m_Parent;
		Control val = null;
		if (parent != null)
		{
			object containerControl = parent.ContainerControl;
			val = (Control)((containerControl is Control) ? containerControl : null);
		}
		bool flag = true;
		if (val != null)
		{
			flag = !BarFunctions.InvokeRecalcLayout(val, invalidate: false);
		}
		if (flag && parent != null)
		{
			parent.RecalcSize();
			parent.Refresh();
		}
	}

	protected internal override void OnExpandChange()
	{
		if (!Expanded && m_SubItems != null && m_Parent != null)
		{
			RemoveItems();
		}
		else
		{
			base.OnExpandChange();
		}
	}

	public override void Refresh()
	{
		if (SuspendLayout)
		{
			return;
		}
		if ((EffectiveStyle == eDotNetBarStyle.Office2003 || EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(EffectiveStyle)) && !BaseItem.IsOnPopup(this))
		{
			if ((!m_Visible && !base.IsOnCustomizeMenu) || !m_Displayed)
			{
				return;
			}
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val == null || !BaseItem.IsHandleValid(val) || val is ᆓ)
			{
				return;
			}
			if (m_NeedRecalcSize)
			{
				RecalcSize();
				if (m_Parent != null)
				{
					m_Parent.SubItemSizeChanged(this);
				}
			}
			Rectangle rect = m_Rect;
			rect.Inflate(2, 2);
			val.Invalidate(rect, true);
		}
		else
		{
			base.Refresh();
		}
	}
}
