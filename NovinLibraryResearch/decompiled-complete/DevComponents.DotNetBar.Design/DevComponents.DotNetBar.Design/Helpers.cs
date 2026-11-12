using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace DevComponents.DotNetBar.Design;

internal static class Helpers
{
	internal static Bitmap LoadBitmap(string imageName)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		if (Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(DotNetBarResourcesAttribute)) is DotNetBarResourcesAttribute dotNetBarResourcesAttribute && dotNetBarResourcesAttribute.NamespacePrefix != "")
		{
			return new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream(dotNetBarResourcesAttribute.NamespacePrefix + "." + imageName));
		}
		return new Bitmap(typeof(DotNetBarManager), imageName);
	}

	internal static Icon LoadIcon(string imageName)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		if (Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(DotNetBarResourcesAttribute)) is DotNetBarResourcesAttribute dotNetBarResourcesAttribute && dotNetBarResourcesAttribute.NamespacePrefix != "")
		{
			return new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream(dotNetBarResourcesAttribute.NamespacePrefix + "." + imageName));
		}
		return new Icon(typeof(DotNetBarManager), imageName);
	}

	public static void DrawDesignTimeSelection(Graphics g, Rectangle r, Color backColor, Color border, int penWidth)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		if (r.Width <= 0 || r.Height <= 0)
		{
			return;
		}
		if (!backColor.IsEmpty && backColor != Color.Transparent)
		{
			border = ((!((double)backColor.GetBrightness() < 0.5)) ? ControlPaint.Dark(backColor) : ControlPaint.Light(backColor));
		}
		Pen val = new Pen(border, (float)penWidth);
		try
		{
			val.DashStyle = (DashStyle)1;
			r.Width--;
			r.Height--;
			g.DrawRectangle(val, r);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static bool IsOffice2007Style(eDotNetBarStyle style)
	{
		if (style == eDotNetBarStyle.StyleManagerControlled)
		{
			style = StyleManager.GetEffectiveStyle();
		}
		if (style != eDotNetBarStyle.Office2007 && style != eDotNetBarStyle.Office2010 && style != eDotNetBarStyle.Windows7)
		{
			return StyleManager.IsMetro(style);
		}
		return true;
	}

	internal static string GetItemErrorInfo(XmlElement xmlItem)
	{
		string text = "";
		if (xmlItem.HasAttribute("assembly"))
		{
			text += xmlItem.GetAttribute("assembly");
		}
		if (xmlItem.HasAttribute("class"))
		{
			text += xmlItem.GetAttribute("class");
		}
		return text;
	}
}
