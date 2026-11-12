using System;
using System.Drawing;

namespace Stimulsoft.Base.Dashboard;

public class StiElementConsts
{
	public class Font
	{
		public Color SelectedColor = Color.White;

		private Font cachedFont;

		public string Name { get; }

		public float Size { get; }

		public Color Color { get; }

		public bool IsBold { get; }

		public Font GetGdiFont(double zoom = 1.0, float? fontSize = null, Font baseFont = null)
		{
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Expected O, but got Unknown
			if (baseFont == null)
			{
				FontStyle val = (FontStyle)(IsBold ? 1 : 0);
				return new Font(Name, fontSize.HasValue ? (fontSize.Value * (float)zoom) : (Size * (float)zoom), val);
			}
			return new Font(baseFont.FontFamily.Name, fontSize.HasValue ? (fontSize.Value * (float)zoom) : (baseFont.Size * (float)zoom), baseFont.Style);
		}

		public Font GetCachedGdiFont()
		{
			return cachedFont ?? (cachedFont = GetGdiFont());
		}

		public Font(string name, float size, Color color, bool isBold = false)
		{
			Name = name;
			Size = size;
			Color = color;
			IsBold = isBold;
		}
	}

	public static class Table
	{
		public static class Header
		{
			public static Color BackgroundColor = Color.FromArgb(255, 240, 240, 240);
		}

		public static class CheckBoxCell
		{
			public static Color CheckColor = Color.Gray;

			public static Color IndeterminateCheckColor = Color.LightGray;

			public static Color SelectedBorderColor = Color.White;

			public static Color SelectedIndeterminateCheckColor = Color.White;
		}

		public static Font Font = new Font("Arial", 10f, Color.Black);

		public static Color BorderColor = Color.Gainsboro;

		public static int Height = 28;

		public static int GetHeight(Font font, double scale = 1.0)
		{
			return (int)Math.Max((double)Height * scale, (double)font.GetHeight() * scale * 0.8);
		}
	}

	public static class Chart
	{
		public static Font Font = new Font("Arial", 12f, Color.FromArgb(255, 140, 140, 140));
	}

	public static class Highlight
	{
		public static Color Color = Color.FromArgb(255, 43, 87, 154);

		public static Color DarkColor = Color.FromArgb(255, 91, 135, 202);
	}

	public static class DragDrop
	{
		public static Font Font = new Font("Arial", 8f, Color.DimGray);
	}

	public static class ListBox
	{
		public static int ItemHeight = 32;

		public static int CheckBoxWidth = 20;
	}

	public static class ComboBox
	{
		public static int ItemHeight = 32;
	}

	public static class TreeView
	{
		public static int ItemHeight = 32;
	}

	public static Font TitleFont = new Font("Arial", 12f, Color.Gray);

	public static Color ForegroundColor = Color.DimGray;

	public static Color BackgroundColor = Color.White;
}
