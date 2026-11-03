using System;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.QuickButtons;

public class StiQuickButtonHelper
{
	public static RectangleD GetQuickButtonRect(StiQuickButton[] buttons, int buttonIndex, RectangleD rect)
	{
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		double num2 = ((RectangleD)(ref rect)).Left + (double)StiScale.I1;
		double num3 = ((RectangleD)(ref rect)).Right - (double)StiScale.I11;
		foreach (StiQuickButton stiQuickButton in buttons)
		{
			if (num == buttonIndex)
			{
				if (StiOptions.Viewer.Pins.QuickButtonsRightToLeft)
				{
					double num4 = num2;
					if (stiQuickButton.Alignment == StiQuickButtonAlignment.Left)
					{
						num4 = num3;
					}
					return new RectangleD(num4, ((RectangleD)(ref rect)).Top + (double)StiScale.I3, (double)StiScale.I10, (double)StiScale.I10);
				}
				double num5 = num3;
				if (stiQuickButton.Alignment == StiQuickButtonAlignment.Left)
				{
					num5 = num2;
				}
				return new RectangleD(num5, ((RectangleD)(ref rect)).Top + (double)StiScale.I3, (double)StiScale.I10, (double)StiScale.I10);
			}
			if (StiOptions.Viewer.Pins.QuickButtonsRightToLeft)
			{
				if (stiQuickButton.Alignment == StiQuickButtonAlignment.Right)
				{
					num2 += (double)StiScale.I12;
				}
				else
				{
					num3 -= (double)StiScale.I12;
				}
			}
			else if (stiQuickButton.Alignment == StiQuickButtonAlignment.Left)
			{
				num2 += (double)StiScale.I12;
			}
			else
			{
				num3 -= (double)StiScale.I12;
			}
			num++;
		}
		return RectangleD.Empty;
	}

	public static StiQuickButton[] GetQuickButtons(Type type)
	{
		if (!(type.GetCustomAttributes(typeof(StiQuickButtonAttribute), inherit: false) is StiQuickButtonAttribute[] array) || array.Length == 0)
		{
			return null;
		}
		if (array.Length > 1)
		{
			Array.Sort(array, new StiQuickButtonAttribute.Sorter());
		}
		StiQuickButton[] array2 = new StiQuickButton[array.Length];
		int num = 0;
		StiQuickButtonAttribute[] array3 = array;
		foreach (StiQuickButtonAttribute stiQuickButtonAttribute in array3)
		{
			Type type2 = Type.GetType(stiQuickButtonAttribute.QuickButtonTypeName);
			array2[num] = StiActivator.CreateObject(type2, new object[0]) as StiQuickButton;
			num++;
		}
		return array2;
	}

	public static bool CheckVisibleQuickButtons(RectangleD rect)
	{
		switch (StiOptions.Designer.ShowQuickButtons)
		{
		case StiQuickButtonVisibility.None:
			return false;
		case StiQuickButtonVisibility.Always:
			if (((RectangleD)(ref rect)).Height >= (double)StiScale.I4)
			{
				return ((RectangleD)(ref rect)).Width >= (double)StiScale.I4;
			}
			return false;
		case StiQuickButtonVisibility.HideSmallSize:
			if (((RectangleD)(ref rect)).Height >= (double)StiScale.I(25))
			{
				return ((RectangleD)(ref rect)).Width >= (double)StiScale.I(25);
			}
			return false;
		default:
			return ((RectangleD)(ref rect)).Height >= (double)StiScale.I4;
		}
	}
}
