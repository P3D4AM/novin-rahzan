using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ProvideProperty("ImageIndex", typeof(ToolStripMenuItem))]
[ProvideProperty("ImageList", typeof(ToolStripMenuItem))]
[ProvideProperty("Draw", typeof(ToolStripMenuItem))]
[ToolboxItem(false)]
[ToolboxBitmap(typeof(StiMenuProvider), "Toolbox.StiMenuProvider.bmp")]
public class StiMenuProvider : Component, IExtenderProvider
{
	private int menuBarWidth = 24;

	private int imageSize = 22;

	private Hashtable imageIndexes;

	private Hashtable imageLists;

	[Category("Behavior")]
	public Font Font { get; set; }

	protected override void Dispose(bool disposing)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		if (disposing)
		{
			if (Font != null)
			{
				Font.Dispose();
			}
			if (imageIndexes != null)
			{
				imageIndexes.Clear();
				imageIndexes = null;
			}
			if (imageLists != null)
			{
				foreach (ImageList value in imageLists.Values)
				{
					((Component)value)?.Dispose();
				}
				imageLists.Clear();
				imageLists = null;
			}
		}
		base.Dispose(disposing);
	}

	public void Clear()
	{
		imageLists.Clear();
		imageIndexes.Clear();
	}

	private static ToolStrip GetParent(ToolStripItem menuItem)
	{
		while (menuItem.OwnerItem != null)
		{
			menuItem = menuItem.OwnerItem;
		}
		return menuItem.GetCurrentParent();
	}

	private bool FindRightToLeft(ToolStripItem menuItem)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		while (menuItem.OwnerItem != null)
		{
			menuItem = menuItem.OwnerItem;
		}
		ToolStrip currentParent = menuItem.GetCurrentParent();
		if (currentParent is MenuStrip)
		{
			((Control)(MenuStrip)currentParent).RightToLeft = (RightToLeft)1;
		}
		if (currentParent is ContextMenuStrip)
		{
			((Control)(ContextMenuStrip)currentParent).RightToLeft = (RightToLeft)1;
		}
		return false;
	}

	public virtual void DrawItem(object sender, DrawItemEventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		ToolStripMenuItem val = (ToolStripMenuItem)((sender is ToolStripMenuItem) ? sender : null);
		Bitmap val2 = new Bitmap(e.Bounds.Width, e.Bounds.Height, (PixelFormat)2498570);
		try
		{
			Graphics val3 = Graphics.FromImage((Image)(object)val2);
			try
			{
				bool rightToLeft = FindRightToLeft((ToolStripItem)(object)val);
				if (((ToolStripItem)val).Text == "-")
				{
					DrawMenuItemSeparator(val3, e, val, rightToLeft);
				}
				else if (((ToolStripItem)val).Text.Length >= 4 && ((ToolStripItem)val).Text.Substring(0, 2) == "--" && ((ToolStripItem)val).Text.Substring(((ToolStripItem)val).Text.Length - 2) == "--")
				{
					DrawMenuItemTitle(val3, e, val, rightToLeft);
				}
				else
				{
					DrawMenuItemBackground(val3, e, val, rightToLeft);
					DrawMenuItemText(val3, e, val, rightToLeft);
					DrawMenuItemShortcut(val3, e, val, rightToLeft);
					DrawMenuItemCheck(val3, e, val, rightToLeft);
					DrawMenuItemImage(val3, e, val, rightToLeft);
				}
				e.Graphics.DrawImageUnscaled((Image)(object)val2, e.Bounds.Left, e.Bounds.Top, e.Bounds.Width, e.Bounds.Height);
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

	private void DrawMenuItemTitle(Graphics g, DrawItemEventArgs e, ToolStripMenuItem menuItem, bool rightToLeft)
	{
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		Rectangle bounds = e.Bounds;
		bounds.Location = new Point(0, 0);
		DrawMenuItemBar(g, e, menuItem, rightToLeft);
		if (!rightToLeft)
		{
			bounds.X += menuBarWidth;
		}
		bounds.Width -= menuBarWidth;
		bounds.Width--;
		bounds.Height--;
		SolidBrush val = new SolidBrush(StiColorUtils.Dark(SystemColors.Control, 20));
		try
		{
			g.FillRectangle((Brush)(object)val, bounds);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		Rectangle rectangle = new Rectangle(bounds.Left + 5, bounds.Top, bounds.Width - 5, bounds.Height);
		StringFormat val2 = new StringFormat();
		try
		{
			if (rightToLeft)
			{
				val2.Alignment = (StringAlignment)2;
			}
			else
			{
				val2.Alignment = (StringAlignment)0;
			}
			val2.LineAlignment = (StringAlignment)1;
			val2.FormatFlags = (StringFormatFlags)4096;
			val2.Trimming = (StringTrimming)3;
			val2.HotkeyPrefix = (HotkeyPrefix)2;
			g.DrawString(((ToolStripItem)menuItem).Text.Substring(2, ((ToolStripItem)menuItem).Text.Length - 4), Font, SystemBrushes.ControlText, (RectangleF)rectangle, val2);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		Pen val3 = new Pen(StiColors.Content);
		try
		{
			if (rightToLeft)
			{
				g.DrawLine(val3, bounds.Right, bounds.Y, bounds.Right, bounds.Bottom);
			}
			else
			{
				g.DrawLine(val3, bounds.X, bounds.Y, bounds.X, bounds.Bottom);
			}
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
	}

	private void DrawMenuItemBar(Graphics g, DrawItemEventArgs e, Rectangle rect, bool rightToLeft)
	{
	}

	private void DrawMenuItemCheck(Graphics g, DrawItemEventArgs e, ToolStripMenuItem item, bool rightToLeft)
	{
		Rectangle bounds = e.Bounds;
		bounds.Location = new Point(0, 0);
		if (!item.Checked)
		{
			return;
		}
		bounds.X++;
		bounds.Y++;
		if (rightToLeft)
		{
			bounds.X = bounds.Right - (menuBarWidth - 1);
		}
		bounds.Width = menuBarWidth - 4;
		bounds.Height -= 3;
		g.FillRectangle(StiBrushes.Selected, bounds);
		g.DrawRectangle(StiPens.SelectedText, bounds);
		ImageList imageList = GetImageList(item);
		int imageIndex = GetImageIndex(item);
		if (imageList == null || imageIndex == -1 || imageIndex >= imageList.Images.Count)
		{
			if (rightToLeft)
			{
				StiControlPaint.DrawCheck(g, e.Bounds.Right - menuBarWidth + imageSize / 2, bounds.Y + imageSize / 2, enabled: true);
			}
			else
			{
				StiControlPaint.DrawCheck(g, imageSize / 2, bounds.Y + imageSize / 2, enabled: true);
			}
		}
	}

	private void DrawMenuItemShortcut(Graphics g, DrawItemEventArgs e, ToolStripMenuItem item, bool rightToLeft)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		Rectangle bounds = e.Bounds;
		bounds.Location = new Point(0, 0);
		bounds.Width -= 15;
		if (!item.ShowShortcutKeys || GetParent((ToolStripItem)(object)item) is MenuStrip)
		{
			return;
		}
		StringFormat val = new StringFormat();
		try
		{
			if (rightToLeft)
			{
				bounds.X += 10;
				val.Alignment = (StringAlignment)0;
			}
			else
			{
				val.Alignment = (StringAlignment)2;
			}
			val.LineAlignment = (StringAlignment)1;
			string text = ConvertShortcut(item.ShortcutKeys);
			if (((ToolStripItem)item).Enabled)
			{
				g.DrawString(text, Font, SystemBrushes.ControlText, (RectangleF)bounds, val);
				return;
			}
			SolidBrush val2 = new SolidBrush(StiColorUtils.Light(SystemColors.ControlText, 150));
			try
			{
				g.DrawString(text, Font, (Brush)(object)val2, (RectangleF)bounds, val);
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

	private void DrawMenuItemText(Graphics g, DrawItemEventArgs e, ToolStripMenuItem item, bool rightToLeft)
	{
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		Rectangle bounds = e.Bounds;
		bounds.Location = new Point(0, 0);
		if (GetParent((ToolStripItem)(object)item) is MenuStrip)
		{
			return;
		}
		bounds = ((!rightToLeft) ? new Rectangle(bounds.Left + menuBarWidth + 5, bounds.Top, bounds.Width, bounds.Height) : new Rectangle(bounds.Left + 20, bounds.Top, bounds.Width - menuBarWidth - 25, bounds.Height));
		StringFormat val = new StringFormat();
		try
		{
			val.Alignment = (StringAlignment)(rightToLeft ? 2 : 0);
			val.LineAlignment = (StringAlignment)1;
			val.FormatFlags = (StringFormatFlags)(val.FormatFlags | 0x1000);
			val.HotkeyPrefix = (HotkeyPrefix)1;
			if (((ToolStripItem)item).Enabled)
			{
				g.DrawString(((ToolStripItem)item).Text, Font, SystemBrushes.ControlText, (RectangleF)bounds, val);
				return;
			}
			SolidBrush val2 = new SolidBrush(StiColorUtils.Light(SystemColors.ControlText, 150));
			try
			{
				g.DrawString(((ToolStripItem)item).Text, Font, (Brush)(object)val2, (RectangleF)bounds, val);
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

	private void DrawMenuItemImage(Graphics g, DrawItemEventArgs e, ToolStripMenuItem item, bool rightToLeft)
	{
		Rectangle rectangle = e.Bounds;
		rectangle.Location = new Point(0, 0);
		Image val = null;
		ImageList imageList = GetImageList(item);
		int imageIndex = GetImageIndex(item);
		if (imageList != null && imageIndex != -1 && imageIndex < imageList.Images.Count)
		{
			val = imageList.Images[imageIndex];
		}
		if (val != null)
		{
			rectangle = ((!rightToLeft) ? new Rectangle((menuBarWidth - imageList.Images[imageIndex].Width) / 2, rectangle.Y + (imageSize - imageList.Images[imageIndex].Height) / 2, val.Width, val.Height) : new Rectangle(rectangle.Right - menuBarWidth + (menuBarWidth - imageList.Images[imageIndex].Width) / 2, rectangle.Y + (imageSize - imageList.Images[imageIndex].Height) / 2, val.Width, val.Height));
		}
		if (val != null)
		{
			if (((ToolStripItem)item).Enabled)
			{
				imageList.Draw(g, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, imageIndex);
			}
			else
			{
				StiControlPaint.DrawImageDisabled(g, (Bitmap)(object)((val is Bitmap) ? val : null), rectangle.X, rectangle.Y);
			}
		}
	}

	private void DrawMenuItemBackground(Graphics g, DrawItemEventArgs e, ToolStripMenuItem item, bool rightToLeft)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Invalid comparison between Unknown and I4
		Rectangle bounds = e.Bounds;
		bounds.Location = new Point(0, 0);
		if (!(GetParent((ToolStripItem)(object)item) is MenuStrip))
		{
			if (((e.State & 1) > 0 || (e.State & 0x40) > 0) && ((ToolStripItem)item).Enabled)
			{
				bounds.Width--;
				bounds.Height--;
				g.FillRectangle(StiBrushes.Focus, bounds);
			}
			else
			{
				g.FillRectangle(Brushes.White, bounds);
			}
		}
	}

	private void DrawMenuItemBar(Graphics g, DrawItemEventArgs e, ToolStripMenuItem item, bool rightToLeft)
	{
		Rectangle bounds = e.Bounds;
		bounds.Location = new Point(0, 0);
		if (rightToLeft)
		{
			bounds.X = bounds.Right - menuBarWidth;
		}
		bounds.Width = menuBarWidth;
		DrawMenuItemBar(g, e, bounds, rightToLeft);
	}

	private void DrawMenuItemSeparator(Graphics g, DrawItemEventArgs e, ToolStripMenuItem item, bool rightToLeft)
	{
		DrawMenuItemBackground(g, e, item, rightToLeft);
		DrawMenuItemBar(g, e, item, rightToLeft);
		Rectangle bounds = e.Bounds;
		bounds.Location = new Point(0, 0);
		if (!rightToLeft)
		{
			bounds.X += menuBarWidth + 8;
		}
		bounds.Width -= menuBarWidth + 8;
		int num = bounds.Y + bounds.Height / 2;
		g.DrawLine(SystemPens.ControlDark, bounds.X, num, bounds.Right, num);
	}

	private string ConvertShortcut(Keys shortcut)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		if ((int)shortcut == 0)
		{
			return string.Empty;
		}
		return TypeDescriptor.GetConverter(typeof(Keys)).ConvertToString(shortcut);
	}

	public virtual void MeasureItem(object sender, MeasureItemEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		ToolStripMenuItem val = (ToolStripMenuItem)sender;
		bool flag = FindRightToLeft((ToolStripItem)(object)val);
		Graphics graphics = e.Graphics;
		if (((ToolStripItem)val).Text.Length >= 4 && ((ToolStripItem)val).Text.Substring(0, 2) == "--" && ((ToolStripItem)val).Text.Substring(((ToolStripItem)val).Text.Length - 2) == "--")
		{
			e.ItemWidth = (int)graphics.MeasureString(((ToolStripItem)val).Text.Substring(2, ((ToolStripItem)val).Text.Length - 4), Font).Width + menuBarWidth;
			e.ItemHeight = 25;
		}
		else if (((ToolStripItem)val).Text != "-")
		{
			int num = 0;
			if (val.ShowShortcutKeys)
			{
				num = (int)graphics.MeasureString(ConvertShortcut(val.ShortcutKeys), Font).Width;
			}
			if (num != 0)
			{
				num += 15;
			}
			if (flag)
			{
				num += 10;
			}
			e.ItemHeight = (int)Font.GetHeight() + 6;
			if (e.ItemHeight < imageSize)
			{
				e.ItemHeight = imageSize;
			}
			e.ItemWidth = (int)graphics.MeasureString(((ToolStripItem)val).Text, Font).Width + num;
			if (!(GetParent((ToolStripItem)(object)val) is MenuStrip))
			{
				e.ItemWidth += imageSize * 2;
			}
		}
		else
		{
			e.ItemWidth = 20;
			e.ItemHeight = 3;
		}
	}

	public static void AddMenuProviderToMenuItem(ToolStripMenuItem menuItem, StiMenuProvider provider)
	{
		AddMenuProviderToMenuItem(menuItem, provider, null, -1);
	}

	public static void AddMenuProviderToMenuItem(ToolStripMenuItem menuItem, StiMenuProvider provider, ImageList imageList, int imageIndex)
	{
		if (!(GetParent((ToolStripItem)(object)menuItem) is MenuStrip))
		{
			provider.SetImageList(menuItem, imageList);
			provider.SetImageIndex(menuItem, imageIndex);
		}
	}

	public bool CanExtend(object e)
	{
		return e is ToolStripMenuItem;
	}

	[TypeConverter(typeof(ImageIndexConverter))]
	[Editor(typeof(StiImageIndexEditor), typeof(UITypeEditor))]
	public void SetImageIndex(ToolStripMenuItem control, int value)
	{
		if (!imageIndexes.Contains(control))
		{
			imageIndexes.Add(control, value);
		}
		else
		{
			imageIndexes[control] = value;
		}
	}

	[TypeConverter(typeof(ImageIndexConverter))]
	[Editor(typeof(StiImageIndexEditor), typeof(UITypeEditor))]
	public int GetImageIndex(ToolStripMenuItem control)
	{
		if (imageIndexes[control] != null)
		{
			return (int)imageIndexes[control];
		}
		return -1;
	}

	public ImageList GetImageList(ToolStripMenuItem control)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		if (imageLists[control] == null)
		{
			return null;
		}
		return (ImageList)imageLists[control];
	}

	public void SetImageList(ToolStripMenuItem control, ImageList value)
	{
		if (!imageLists.Contains(control))
		{
			imageLists.Add(control, value);
		}
		else
		{
			imageLists[control] = value;
		}
	}

	public void SetDraw(ToolStripMenuItem control, bool value)
	{
		if (!base.DesignMode && !value && imageIndexes.Contains(control))
		{
			imageIndexes.Remove(control);
		}
	}

	public StiMenuProvider()
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		double step = StiScaleUI.Step;
		menuBarWidth = (int)Math.Ceiling((double)menuBarWidth * step);
		imageSize = (int)Math.Ceiling((double)imageSize * step);
		Font = new Font("Arial", 8f);
		imageIndexes = new Hashtable();
		imageLists = new Hashtable();
	}
}
