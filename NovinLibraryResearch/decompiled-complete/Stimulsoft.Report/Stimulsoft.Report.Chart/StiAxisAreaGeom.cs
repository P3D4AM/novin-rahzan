using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;

namespace Stimulsoft.Report.Chart;

public class StiAxisAreaGeom : StiAreaGeom
{
	private class StiLineF
	{
		public float X1 { get; set; }

		public float Y1 { get; set; }

		public float X2 { get; set; }

		public float Y2 { get; set; }

		public StiLineF(float x1, float y1, float x2, float y2)
		{
			X1 = x1;
			Y1 = y1;
			X2 = x2;
			Y2 = y2;
		}
	}

	private StiAxisAreaViewGeom view;

	private float minWidth = 6f;

	public StiAxisAreaViewGeom View => view;

	private void DrawInterlacingHor(StiContext context, RectangleF rect)
	{
		rect.X = 0f;
		rect.Width = view.ClientRectangle.Width;
		IStiAxisArea stiAxisArea = base.Area as IStiAxisArea;
		if (!stiAxisArea.InterlacingHor.Visible)
		{
			return;
		}
		List<float> list = new List<float>();
		int num = stiAxisArea.YAxis.Info.StripPositions.Length;
		float[] array = new float[num];
		for (int i = 0; i < num; i++)
		{
			array[num - 1 - i] = stiAxisArea.YAxis.Info.StripPositions[i];
		}
		float num2 = 0f;
		bool flag = true;
		int num3 = 0;
		float[] array2 = array;
		foreach (float num4 in array2)
		{
			num3++;
			if (flag)
			{
				num2 = num4;
				list.Add(num4);
				flag = false;
				continue;
			}
			float num5 = num2 - num4;
			if (!(num5 < minWidth) || num3 == array.Length)
			{
				num2 = num4;
				list.Add(num4);
			}
		}
		float[] array3 = new float[list.Count];
		list.CopyTo(array3);
		int num6 = 0;
		bool flag2 = true;
		float[] array4 = array3;
		foreach (float num7 in array4)
		{
			float num8 = array3[num6 + 1];
			if (flag2)
			{
				RectangleF rectangleF = new RectangleF(rect.X, rect.Y + num8, rect.Width, num7 - num8);
				if (rectangleF.Bottom >= 0f && rectangleF.Y < view.ClientRectangle.Height)
				{
					context.FillRectangle(stiAxisArea.InterlacingHor.InterlacedBrush, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
				}
			}
			flag2 = !flag2;
			num6++;
			if (num6 == array3.Length - 1)
			{
				break;
			}
		}
	}

	private void DrawInterlacingVer(StiContext context, RectangleF rect)
	{
		rect.Y = 0f;
		rect.Height = view.ClientRectangle.Height;
		IStiAxisArea stiAxisArea = base.Area as IStiAxisArea;
		if (!stiAxisArea.InterlacingVert.Visible)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		if (stiAxisArea.ReverseHor)
		{
			num2 = 1;
		}
		List<float> list = new List<float>();
		float[] stripPositions = stiAxisArea.XAxis.Info.StripPositions;
		float num3 = 0f;
		bool flag = true;
		int num4 = 0;
		float[] array = stripPositions;
		foreach (float num5 in array)
		{
			num4++;
			if (flag)
			{
				num3 = num5;
				list.Add(num5);
				flag = false;
				continue;
			}
			float num6 = num5 - num3;
			if (!(num6 < minWidth) || num4 == stripPositions.Length)
			{
				num3 = num5;
				list.Add(num5);
			}
		}
		float[] array2 = new float[list.Count];
		list.CopyTo(array2);
		float[] array3 = array2;
		foreach (float num7 in array3)
		{
			float num8 = array2[num + 1];
			if ((num2 & 1) > 0)
			{
				RectangleF rectangleF = new RectangleF(num7 + rect.X, rect.Y, num8 - num7, rect.Height);
				if (rectangleF.Right >= 0f && rectangleF.X < view.ClientRectangle.Width)
				{
					context.FillRectangle(stiAxisArea.InterlacingVert.InterlacedBrush, rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height);
				}
			}
			num2++;
			num++;
			if (num == array2.Length - 1)
			{
				break;
			}
		}
	}

	private List<StiLineF> GetGridLinesHorMajor(RectangleF rect, IStiGridLinesHor gridLinesHor, bool isLeftAxis)
	{
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Invalid comparison between Unknown and I4
		List<StiLineF> list = new List<StiLineF>();
		rect.X = 0f;
		rect.Width = view.ClientRectangle.Width;
		IStiAxisArea stiAxisArea = base.Area as IStiAxisArea;
		float[] array = (isLeftAxis ? stiAxisArea.YAxis.Info.StripPositions : stiAxisArea.YRightAxis.Info.StripPositions);
		int num = array.Length;
		float[] array2 = new float[num];
		for (int i = 0; i < num; i++)
		{
			array2[num - i - 1] = array[i];
		}
		array = array2;
		if (array.Length != 0)
		{
			float num2 = array[0];
			int num3 = 0;
			float[] array3 = array;
			foreach (float num4 in array3)
			{
				float num5 = num2 - num4;
				if (num5 < minWidth && num5 > 0f)
				{
					continue;
				}
				num2 = num4;
				if (gridLinesHor.Visible && (int)gridLinesHor.Style != 6)
				{
					float num6 = num4 + rect.Y;
					if (num6 >= 0f && num6 <= base.ClientRectangle.Height)
					{
						list.Add(new StiLineF(rect.X, num6, rect.Right, num6));
					}
				}
				num3++;
			}
		}
		return list;
	}

	private List<StiLineF> GetGridLinesHorMinor(RectangleF rect, IStiGridLinesHor gridLinesHor, bool isLeftAxis)
	{
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Invalid comparison between Unknown and I4
		List<StiLineF> list = new List<StiLineF>();
		rect.X = 0f;
		rect.Width = view.ClientRectangle.Width;
		IStiAxisArea stiAxisArea = base.Area as IStiAxisArea;
		float[] array = (isLeftAxis ? stiAxisArea.YAxis.Info.StripPositions : stiAxisArea.YRightAxis.Info.StripPositions);
		int num = array.Length;
		float[] array2 = new float[num];
		for (int i = 0; i < num; i++)
		{
			array2[num - i - 1] = array[i];
		}
		array = array2;
		if (array.Length != 0)
		{
			float num2 = array[0];
			int num3 = 0;
			float[] array3 = array;
			foreach (float num4 in array3)
			{
				float num5 = num2 - num4;
				if (num5 < minWidth && num5 > 0f)
				{
					continue;
				}
				num2 = num4;
				if (gridLinesHor.MinorVisible && (int)gridLinesHor.MinorStyle != 6 && num3 != array.Length - 1)
				{
					float num6 = array[num3 + 1];
					if (num4 - num6 < minWidth)
					{
						continue;
					}
					int num7 = ((gridLinesHor.MinorCount == 0) ? stiAxisArea.YAxis.Ticks.MinorCount : gridLinesHor.MinorCount);
					float num8 = num4;
					float num9 = (num6 - num4) / (float)(num7 + 1);
					for (int k = 1; k <= num7; k++)
					{
						num8 = num4 + num9 * (float)k;
						float num10 = num8 + rect.Y;
						if (num10 >= 0f && num10 <= base.ClientRectangle.Height)
						{
							list.Add(new StiLineF(rect.X, num10, rect.Right, num10));
						}
					}
				}
				num3++;
			}
		}
		return list;
	}

	private List<StiLineF> GetGridLinesVerMajor(RectangleF rect, IStiGridLinesVert gridLinesVert, bool isBottomAxis)
	{
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Invalid comparison between Unknown and I4
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		List<StiLineF> list = new List<StiLineF>();
		rect.Y = 0f;
		rect.Height = view.ClientRectangle.Height;
		IStiAxisArea stiAxisArea = base.Area as IStiAxisArea;
		float[] array = (isBottomAxis ? stiAxisArea.XAxis.Info.StripPositions : stiAxisArea.XTopAxis.Info.StripPositions);
		float num = 0f;
		int num2 = 0;
		float[] array2 = array;
		foreach (float num3 in array2)
		{
			float num4 = num3 - num;
			if (num4 < minWidth && num4 > 0f)
			{
				continue;
			}
			num = num3;
			float thickness = 1f;
			if (gridLinesVert.Visible && (int)gridLinesVert.Style != 6)
			{
				StiPenGeom stiPenGeom = new StiPenGeom(gridLinesVert.Color, thickness);
				stiPenGeom.PenStyle = gridLinesVert.Style;
				float num5 = num3 + rect.X;
				if (num5 >= 0f && num5 <= base.ClientRectangle.Width)
				{
					list.Add(new StiLineF(num5, rect.Y, num5, rect.Bottom));
				}
			}
			num2++;
		}
		return list;
	}

	private List<StiLineF> GetGridLinesVerMinor(RectangleF rect, IStiGridLinesVert gridLinesVert, bool isBottomAxis)
	{
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Invalid comparison between Unknown and I4
		List<StiLineF> list = new List<StiLineF>();
		rect.Y = 0f;
		rect.Height = view.ClientRectangle.Height;
		IStiAxisArea stiAxisArea = base.Area as IStiAxisArea;
		float[] array = (isBottomAxis ? stiAxisArea.XAxis.Info.StripPositions : stiAxisArea.XTopAxis.Info.StripPositions);
		float num = 0f;
		int num2 = 0;
		float[] array2 = array;
		foreach (float num3 in array2)
		{
			float num4 = num3 - num;
			if (num4 < minWidth && num4 > 0f)
			{
				continue;
			}
			num = num3;
			if (gridLinesVert.MinorVisible && (int)gridLinesVert.MinorStyle != 6 && num2 != array.Length - 1)
			{
				float num5 = array[num2 + 1];
				if (num5 - num3 < minWidth)
				{
					continue;
				}
				int num6 = ((gridLinesVert.MinorCount == 0) ? stiAxisArea.YAxis.Ticks.MinorCount : gridLinesVert.MinorCount);
				float num7 = num3;
				float num8 = (num5 - num3) / ((float)num6 + 1f);
				for (int j = 1; j <= num6; j++)
				{
					num7 = num3 + num8 * (float)j;
					float num9 = num7 + rect.X;
					if (num9 >= 0f && num9 <= base.ClientRectangle.Width)
					{
						list.Add(new StiLineF(num9, rect.Y, num9, rect.Bottom));
					}
				}
			}
			num2++;
		}
		return list;
	}

	protected override bool AllowChildDrawing(StiCellGeom cellGeom)
	{
		if (cellGeom != null)
		{
			return IsChildVisibleInView(cellGeom);
		}
		return false;
	}

	public bool IsChildVisibleInView(StiCellGeom cellGeom)
	{
		RectangleF rectangleF = View.ClientRectangle;
		rectangleF.X = 0f;
		rectangleF.Y = 0f;
		rectangleF.Inflate(1f, 1f);
		RectangleF rect = cellGeom.ClientRectangle;
		rect.X -= (float)((StiAxisAreaCoreXF)base.Area.Core).ScrollDistanceX;
		rect.Y -= (float)((StiAxisAreaCoreXF)base.Area.Core).ScrollDistanceY;
		if (cellGeom != null)
		{
			return rectangleF.IntersectsWith(rect);
		}
		return false;
	}

	public override void Draw(StiContext context)
	{
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		base.Draw(context);
		IStiAxisArea stiAxisArea = base.Area as IStiAxisArea;
		RectangleF rect = base.ClientRectangle;
		if (!rect.IsEmpty)
		{
			List<IStiSeries> series = stiAxisArea.AxisCore.GetSeries();
			if (series.Count > 0)
			{
				DrawInterlacingHor(context, rect);
				DrawInterlacingVer(context, rect);
			}
			if (series.Count > 0)
			{
				List<StiLineF> gridLinesHorMajor = GetGridLinesHorMajor(rect, stiAxisArea.GridLinesHor, isLeftAxis: true);
				List<StiLineF> gridLinesHorMinor = GetGridLinesHorMinor(rect, stiAxisArea.GridLinesHor, isLeftAxis: true);
				List<StiLineF> gridLinesHorMajor2 = GetGridLinesHorMajor(rect, stiAxisArea.GridLinesHorRight, isLeftAxis: true);
				List<StiLineF> gridLinesHorMinor2 = GetGridLinesHorMinor(rect, stiAxisArea.GridLinesHorRight, isLeftAxis: true);
				List<StiLineF> gridLinesVerMajor = GetGridLinesVerMajor(rect, stiAxisArea.GridLinesVert, isBottomAxis: true);
				List<StiLineF> gridLinesVerMinor = GetGridLinesVerMinor(rect, stiAxisArea.GridLinesVert, isBottomAxis: true);
				StiPenGeom stiPenGeom = new StiPenGeom(stiAxisArea.GridLinesHor.MinorColor, 1f);
				stiPenGeom.PenStyle = stiAxisArea.GridLinesHor.MinorStyle;
				DrawLines(context, gridLinesHorMinor, stiPenGeom);
				stiPenGeom = new StiPenGeom(stiAxisArea.GridLinesHorRight.MinorColor, 1f);
				stiPenGeom.PenStyle = stiAxisArea.GridLinesHorRight.MinorStyle;
				DrawLines(context, gridLinesHorMinor2, stiPenGeom);
				stiPenGeom = new StiPenGeom(stiAxisArea.GridLinesVert.MinorColor, 1f);
				stiPenGeom.PenStyle = stiAxisArea.GridLinesVert.MinorStyle;
				DrawLines(context, gridLinesVerMinor, stiPenGeom);
				stiPenGeom = new StiPenGeom(stiAxisArea.GridLinesHor.Color, 1f);
				stiPenGeom.PenStyle = stiAxisArea.GridLinesHor.Style;
				DrawLines(context, gridLinesHorMajor, stiPenGeom);
				stiPenGeom = new StiPenGeom(stiAxisArea.GridLinesHorRight.Color, 1f);
				stiPenGeom.PenStyle = stiAxisArea.GridLinesHorRight.Style;
				DrawLines(context, gridLinesHorMajor2, stiPenGeom);
				stiPenGeom = new StiPenGeom(stiAxisArea.GridLinesVert.Color, 1f);
				stiPenGeom.PenStyle = stiAxisArea.GridLinesVert.Style;
				DrawLines(context, gridLinesVerMajor, stiPenGeom);
			}
		}
	}

	private void DrawLines(StiContext context, List<StiLineF> lines, StiPenGeom pen)
	{
		foreach (StiLineF line in lines)
		{
			context.DrawLine(pen, line.X1, line.Y1, line.X2, line.Y2);
		}
	}

	public StiAxisAreaGeom(StiAxisAreaViewGeom view, IStiArea area, RectangleF clientRectangle)
		: base(area, clientRectangle)
	{
		this.view = view;
	}
}
