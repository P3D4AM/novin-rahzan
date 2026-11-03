using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Presentation;

internal class ᙙ : ᙝ
{
	public Color ᙕ = Color.Empty;

	public Color ᙖ = Color.Empty;

	public int ᙗ = 90;

	public BackgroundColorBlendCollection ᙚ;

	public eGradientType ᙛ;

	public ᙙ()
	{
	}

	public ᙙ(Color color1, Color color2)
	{
		ᙕ = color1;
		ᙖ = color2;
	}

	public ᙙ(LinearGradientColorTable table)
	{
		ᙕ = table.Start;
		ᙖ = table.End;
	}

	public ᙙ(Color color1)
	{
		ᙕ = color1;
		ᙖ = Color.Empty;
	}

	public void ᙘ(LinearGradientColorTable ቯ)
	{
		if (ቯ == null)
		{
			ᙕ = Color.Empty;
			ᙖ = Color.Empty;
			ᙗ = 90;
		}
		else
		{
			ᙕ = ቯ.Start;
			ᙖ = ቯ.End;
			ᙗ = ቯ.GradientAngle;
		}
	}

	public Brush ᙜ(Rectangle ल)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		if (ल.Width <= 0 || ल.Height <= 0)
		{
			return null;
		}
		if (ᙚ != null && ᙚ.Count > 0)
		{
			return DisplayHelp.CreateBrush(ल, ᙚ, ᙗ, ᙛ);
		}
		if (ᙖ.IsEmpty && !ᙕ.IsEmpty)
		{
			return (Brush)new SolidBrush(ᙕ);
		}
		if (!ᙖ.IsEmpty && !ᙕ.IsEmpty)
		{
			if (ᙛ == eGradientType.Linear)
			{
				return (Brush)new LinearGradientBrush(ल, ᙕ, ᙖ, (float)ᙗ);
			}
			int num = (int)Math.Sqrt(ल.Width * ल.Width + ल.Height * ल.Height) + 4;
			GraphicsPath val = new GraphicsPath();
			val.AddEllipse(ल.X - (num - ल.Width) / 2, ल.Y - (num - ल.Height) / 2, num, num);
			PathGradientBrush val2 = new PathGradientBrush(val);
			val2.CenterColor = ᙕ;
			val2.SurroundColors = new Color[1] { ᙖ };
			return (Brush)(object)val2;
		}
		return null;
	}
}
