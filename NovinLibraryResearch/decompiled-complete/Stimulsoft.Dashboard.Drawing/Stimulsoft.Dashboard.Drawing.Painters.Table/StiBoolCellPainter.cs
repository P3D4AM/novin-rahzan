using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Report.Dashboard.Helpers;

namespace Stimulsoft.Dashboard.Drawing.Painters.Table;

internal static class StiBoolCellPainter
{
	internal static void Draw(Graphics g, RectangleD rectD, StiTableElement table, object[] rowValues, string[] columnKeys, StiTableColumn column, object boolObj)
	{
		Draw(g, rectD, table, rowValues, columnKeys, column, boolObj, (float)table.Zoom);
	}

	internal static void Draw(Graphics g, RectangleD rectD, StiTableElement table, object[] rowValues, string[] columnKeys, StiTableColumn column, object boolObj, float zoom = 1f, bool isSelected = false)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Invalid comparison between Unknown and I4
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Invalid comparison between Unknown and I4
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Expected O, but got Unknown
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		CheckState state = GetState(boolObj);
		double num = (double)(14f * zoom) * StiScaleUI.Factor;
		Rectangle rectangle = new RectangleD(rectD.X + (rectD.Width - num) / 2.0, rectD.Y + (rectD.Height - num) / 2.0, num, num).ToRectangle();
		Pen val = new Pen(GetBorderColor(table, column, isSelected));
		try
		{
			g.DrawRectangle(val, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if ((int)state == 2)
		{
			rectangle.Inflate(-(int)(3f * zoom), -(int)(3f * zoom));
			rectangle.Width++;
			rectangle.Height++;
			SolidBrush val2 = new SolidBrush(GetIndeterminateCheckColor(table, column, isSelected));
			try
			{
				g.FillRectangle((Brush)(object)val2, rectangle);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		if ((int)state == 1)
		{
			rectangle.Inflate(-(int)(4f * zoom), -(int)(4f * zoom));
			PointF pointF = new PointF(rectangle.X, (float)rectangle.Y + (float)rectangle.Width * 0.7f);
			PointF pointF2 = new PointF((float)rectangle.X + (float)rectangle.Width * 0.5f, rectangle.Bottom);
			PointF pointF3 = new PointF(rectangle.Right, rectangle.Y);
			SmoothingMode smoothingMode = g.SmoothingMode;
			g.SmoothingMode = (SmoothingMode)4;
			Color checkColor = GetCheckColor(table, column, isSelected);
			checkColor = StiTableElementConditionHelper.ProcessForeColor(checkColor, column.Key, table, rowValues, columnKeys);
			Pen val3 = new Pen(checkColor);
			try
			{
				val3.Width = 2f;
				val3.StartCap = (LineCap)2;
				val3.EndCap = (LineCap)2;
				g.DrawLine(val3, pointF, pointF2);
				g.DrawLine(val3, pointF2, pointF3);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			g.SmoothingMode = smoothingMode;
		}
	}

	private static Color GetBorderColor(StiTableElement table, StiTableColumn column, bool isSelected)
	{
		if (column.ForeColor != Color.Transparent)
		{
			return column.ForeColor;
		}
		Color foreColor = StiDashboardExpressionHelper.GetForeColor(table, table.ForeColor);
		if (foreColor != Color.Transparent)
		{
			return foreColor;
		}
		if (!isSelected)
		{
			return StiElementConsts.Table.BorderColor;
		}
		return StiElementConsts.Table.CheckBoxCell.SelectedBorderColor;
	}

	private static Color GetIndeterminateCheckColor(StiTableElement table, StiTableColumn column, bool isSelected)
	{
		if (column.ForeColor != Color.Transparent)
		{
			return column.ForeColor;
		}
		Color foreColor = StiDashboardExpressionHelper.GetForeColor(table, table.ForeColor);
		if (foreColor != Color.Transparent)
		{
			return foreColor;
		}
		if (!isSelected)
		{
			return StiElementConsts.Table.CheckBoxCell.IndeterminateCheckColor;
		}
		return StiElementConsts.Table.CheckBoxCell.SelectedIndeterminateCheckColor;
	}

	private static Color GetCheckColor(StiTableElement table, StiTableColumn column, bool isSelected)
	{
		if (column.ForeColor != Color.Transparent)
		{
			return column.ForeColor;
		}
		Color foreColor = StiDashboardExpressionHelper.GetForeColor(table, table.ForeColor);
		if (foreColor != Color.Transparent)
		{
			return foreColor;
		}
		if (!isSelected)
		{
			return StiElementConsts.Table.CheckBoxCell.CheckColor;
		}
		return Color.White;
	}

	private static CheckState GetState(object boolObj)
	{
		if (boolObj != null && boolObj is bool)
		{
			if ((bool)boolObj)
			{
				return (CheckState)1;
			}
			return (CheckState)0;
		}
		return (CheckState)2;
	}
}
