using System;
using System.Windows.Forms;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Design;

public sealed class StiSelectionTransfer
{
	public static void MoveEnter(StiDesignerControl designer)
	{
		StiComponent stiComponent = designer.SelectedComponentsOnPage[0];
		MoveRight(designer);
		if (stiComponent == designer.SelectedComponentsOnPage[0])
		{
			do
			{
				stiComponent = designer.SelectedComponentsOnPage[0];
				MoveLeft(designer);
			}
			while (stiComponent != designer.SelectedComponentsOnPage[0]);
			MoveDown(designer);
		}
	}

	public static void MoveLeft(StiDesignerControl designer)
	{
		StiComponent stiComponent = designer.SelectedComponentsOnPage[0];
		StiComponent stiComponent2 = null;
		double num = 0.0;
		stiComponent.Page.ResetSelection();
		StiComponentsCollection components = stiComponent.Parent.Components;
		double num2 = stiComponent.Left + stiComponent.Width / 2.0;
		double num3 = stiComponent.Top + stiComponent.Height / 2.0;
		foreach (StiComponent item in components)
		{
			double num4 = item.Left + item.Width / 2.0;
			double num5 = item.Top + item.Height / 2.0;
			double num6 = num4 - num2;
			double num7 = num5 - num3;
			double num8 = Math.Sqrt(num6 * num6 + num7 * num7);
			if (item != stiComponent && item.Left < stiComponent.Left && item.Right < stiComponent.Right && (stiComponent2 == null || num >= num8))
			{
				num = num8;
				stiComponent2 = item;
			}
		}
		if (stiComponent2 == null)
		{
			stiComponent2 = stiComponent;
		}
		stiComponent2.Select();
		designer.AddSelectedToPropertyGrid();
		((Control)designer).Refresh();
	}

	public static void MoveRight(StiDesignerControl designer)
	{
		StiComponent stiComponent = designer.SelectedComponentsOnPage[0];
		StiComponent stiComponent2 = null;
		double num = 0.0;
		stiComponent.Page.ResetSelection();
		StiComponentsCollection components = stiComponent.Parent.Components;
		double num2 = stiComponent.Left + stiComponent.Width / 2.0;
		double num3 = stiComponent.Top + stiComponent.Height / 2.0;
		foreach (StiComponent item in components)
		{
			double num4 = item.Left + item.Width / 2.0;
			double num5 = item.Top + item.Height / 2.0;
			double num6 = num4 - num2;
			double num7 = num5 - num3;
			double num8 = Math.Sqrt(num6 * num6 + num7 * num7);
			if (item != stiComponent && item.Left > stiComponent.Left && item.Right > stiComponent.Right && (stiComponent2 == null || num >= num8))
			{
				num = num8;
				stiComponent2 = item;
			}
		}
		if (stiComponent2 == null)
		{
			stiComponent2 = stiComponent;
		}
		stiComponent2.Select();
		designer.AddSelectedToPropertyGrid();
		((Control)designer).Refresh();
	}

	public static void MoveUp(StiDesignerControl designer)
	{
		StiComponent stiComponent = designer.SelectedComponentsOnPage[0];
		StiComponent stiComponent2 = null;
		double num = 0.0;
		stiComponent.Page.ResetSelection();
		double num2 = stiComponent.Left + stiComponent.Width / 2.0;
		double num3 = stiComponent.Top + stiComponent.Height / 2.0;
		StiComponentsCollection components = stiComponent.Parent.Components;
		foreach (StiComponent item in components)
		{
			double num4 = item.Left + item.Width / 2.0;
			double num5 = item.Top + item.Height / 2.0;
			double num6 = num4 - num2;
			double num7 = num5 - num3;
			double num8 = Math.Sqrt(num6 * num6 + num7 * num7);
			if (item != stiComponent && item.Top < stiComponent.Top && item.Bottom < stiComponent.Bottom && (stiComponent2 == null || num >= num8))
			{
				num = num8;
				stiComponent2 = item;
			}
		}
		if (stiComponent2 == null && stiComponent.Parent.Parent != null)
		{
			num2 = stiComponent.Parent.Left + stiComponent.Parent.Width / 2.0;
			num3 = stiComponent.Parent.Top + stiComponent.Parent.Height / 2.0;
			components = stiComponent.Parent.Parent.Components;
			foreach (StiComponent item2 in components)
			{
				double num9 = item2.Left + item2.Width / 2.0;
				double num10 = item2.Top + item2.Height / 2.0;
				double num11 = num9 - num2;
				double num12 = num10 - num3;
				double num13 = Math.Sqrt(num11 * num11 + num12 * num12);
				if (item2 != stiComponent.Parent && item2.Top < stiComponent.Parent.Top && item2.Bottom < stiComponent.Parent.Bottom && (stiComponent2 == null || num >= num13))
				{
					num = num13;
					stiComponent2 = item2;
				}
			}
			stiComponent2 = GetComponentInHorizontal(stiComponent2, stiComponent);
		}
		if (stiComponent2 == null)
		{
			stiComponent2 = stiComponent;
		}
		stiComponent2.Select();
		designer.AddSelectedToPropertyGrid();
		((Control)designer).Refresh();
	}

	public static void MoveDown(StiDesignerControl designer)
	{
		StiComponent stiComponent = designer.SelectedComponentsOnPage[0];
		StiComponent stiComponent2 = null;
		double num = 0.0;
		stiComponent.Page.ResetSelection();
		StiComponentsCollection components = stiComponent.Parent.Components;
		double num2 = stiComponent.Left + stiComponent.Width / 2.0;
		double num3 = stiComponent.Top + stiComponent.Height / 2.0;
		foreach (StiComponent item in components)
		{
			double num4 = item.Left + item.Width / 2.0;
			double num5 = item.Top + item.Height / 2.0;
			double num6 = num4 - num2;
			double num7 = num5 - num3;
			double num8 = Math.Sqrt(num6 * num6 + num7 * num7);
			if (item != stiComponent && item.Top > stiComponent.Top && item.Bottom > stiComponent.Bottom && (stiComponent2 == null || num >= num8))
			{
				num = num8;
				stiComponent2 = item;
			}
		}
		if (stiComponent2 == null && stiComponent.Parent.Parent != null)
		{
			components = stiComponent.Parent.Parent.Components;
			num2 = stiComponent.Parent.Left + stiComponent.Parent.Width / 2.0;
			num3 = stiComponent.Parent.Top + stiComponent.Parent.Height / 2.0;
			foreach (StiComponent item2 in components)
			{
				double num9 = item2.Left + item2.Width / 2.0;
				double num10 = item2.Top + item2.Height / 2.0;
				double num11 = num9 - num2;
				double num12 = num10 - num3;
				double num13 = Math.Sqrt(num11 * num11 + num12 * num12);
				if (item2 != stiComponent.Parent && item2.Top > stiComponent.Parent.Top && item2.Bottom > stiComponent.Parent.Bottom && (stiComponent2 == null || num >= num13))
				{
					num = num13;
					stiComponent2 = item2;
				}
			}
			stiComponent2 = GetComponentInHorizontal(stiComponent2, stiComponent);
		}
		if (stiComponent2 == null)
		{
			stiComponent2 = stiComponent;
		}
		stiComponent2.Select();
		designer.AddSelectedToPropertyGrid();
		((Control)designer).Refresh();
	}

	private static StiComponent GetComponentInHorizontal(StiComponent targetComponent, StiComponent component)
	{
		StiComponent stiComponent = null;
		double num = 0.0;
		if (targetComponent != null && targetComponent is StiContainer)
		{
			StiComponentsCollection components = ((StiContainer)targetComponent).Components;
			foreach (StiComponent item in components)
			{
				if (stiComponent == null || Math.Abs(item.Left - component.Left) < num)
				{
					num = Math.Abs(item.Left - component.Left);
					stiComponent = item;
				}
			}
			if (stiComponent != null)
			{
				return stiComponent;
			}
		}
		return targetComponent;
	}
}
