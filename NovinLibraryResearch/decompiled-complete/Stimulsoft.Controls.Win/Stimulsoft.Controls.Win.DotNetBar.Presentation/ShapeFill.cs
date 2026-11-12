using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar.Presentation;

internal class ShapeFill : Shape
{
	public Color Color1 = Color.Empty;

	public Color Color2 = Color.Empty;

	public int GradientAngle = 90;

	public BackgroundColorBlendCollection BackgroundColorBlend;

	public eGradientType GradientType;

	public ShapeFill()
	{
	}

	public ShapeFill(Color color1, Color color2)
	{
		Color1 = color1;
		Color2 = color2;
	}

	public ShapeFill(LinearGradientColorTable table)
	{
		Color1 = table.Start;
		Color2 = table.End;
	}

	public ShapeFill(Color color1)
	{
		Color1 = color1;
		Color2 = Color.Empty;
	}

	public void Apply(LinearGradientColorTable table)
	{
		if (table == null)
		{
			Color1 = Color.Empty;
			Color2 = Color.Empty;
			GradientAngle = 90;
		}
		else
		{
			Color1 = table.Start;
			Color2 = table.End;
			GradientAngle = table.GradientAngle;
		}
	}

	public Brush CreateBrush(System.Drawing.Rectangle r)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		if (r.Width <= 0 || r.Height <= 0)
		{
			return null;
		}
		if (BackgroundColorBlend != null && BackgroundColorBlend.Count > 0)
		{
			return DisplayHelp.CreateBrush(r, BackgroundColorBlend, GradientAngle, GradientType);
		}
		if (Color2.IsEmpty && !Color1.IsEmpty)
		{
			return (Brush)new SolidBrush(Color1);
		}
		if (!Color2.IsEmpty && !Color1.IsEmpty)
		{
			if (GradientType == eGradientType.Linear)
			{
				return (Brush)new LinearGradientBrush(r, Color1, Color2, (float)GradientAngle);
			}
			int num = (int)Math.Sqrt(r.Width * r.Width + r.Height * r.Height) + 4;
			GraphicsPath val = new GraphicsPath();
			val.AddEllipse(r.X - (num - r.Width) / 2, r.Y - (num - r.Height) / 2, num, num);
			PathGradientBrush val2 = new PathGradientBrush(val);
			val2.CenterColor = Color1;
			val2.SurroundColors = new Color[1] { Color2 };
			return (Brush)(object)val2;
		}
		return null;
	}
}
