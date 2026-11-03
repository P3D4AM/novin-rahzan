using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiTreemapAreaCoreXF : StiAreaCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Treemap");

	public override int Position => 130;

	public override StiCellGeom Render(StiContext context, RectangleF rect)
	{
		StiTreemapAreaGeom stiTreemapAreaGeom = new StiTreemapAreaGeom(base.Area, rect);
		List<IStiSeries> series = GetSeries();
		List<double> list = new List<double>();
		foreach (IStiSeries item in series)
		{
			List<double> list2 = new List<double>();
			foreach (double? item2 in item.Values.ToList())
			{
				list2.Add(item2.GetValueOrDefault());
			}
			list.Add(list2.Sum());
		}
		RectangleF rectangleF = new RectangleF(0f, 0f, rect.Width, rect.Height);
		List<double> data = NormalizeDataForArea(list, rectangleF.Width * rectangleF.Height);
		List<RectangleF> list3 = Squarify(data, new List<double?>(), rectangleF, new List<RectangleF>());
		bool flag = true;
		for (int i = 0; i < series.Count(); i++)
		{
			if (list3.Count - 1 >= i)
			{
				series[i].Core.RenderSeries(context, list3[i], stiTreemapAreaGeom, series.ToArray());
				flag = false;
			}
		}
		if (flag)
		{
			stiTreemapAreaGeom.CreateChildGeoms();
			stiTreemapAreaGeom.ChildGeoms.Add(new StiTreemapEmptySeriesElementGeom(rectangleF));
		}
		return stiTreemapAreaGeom;
	}

	private RectangleF CutArea(RectangleF container, double area)
	{
		RectangleF result;
		if (container.Width >= container.Height)
		{
			double num = area / (double)container.Height;
			double num2 = (double)container.Width - num;
			result = new RectangleF(container.X + (float)num, container.Y, (float)num2, container.Height);
		}
		else
		{
			double num3 = area / (double)container.Width;
			double num4 = (double)container.Height - num3;
			result = new RectangleF(container.X, container.Y + (float)num3, container.Width, (float)num4);
		}
		return result;
	}

	public List<RectangleF> Squarify(List<double> data, List<double?> currentrow, RectangleF container, List<RectangleF> stack)
	{
		if (data.Count == 0)
		{
			stack.AddRange(GetCoordinates(container, currentrow));
			return stack;
		}
		float length = Math.Min(container.Width, container.Height);
		double value = data[0];
		if (ImprovesRatio(currentrow, value, length))
		{
			currentrow.Add(value);
			Squarify(data.Skip(1).ToList(), currentrow, container, stack);
		}
		else
		{
			RectangleF container2 = CutArea(container, currentrow.Sum().GetValueOrDefault());
			stack.AddRange(GetCoordinates(container, currentrow));
			List<double?> currentrow2 = new List<double?>();
			Squarify(data, currentrow2, container2, stack);
		}
		return stack;
	}

	private bool ImprovesRatio(List<double?> currentrow, double? nextnode, float length)
	{
		if (currentrow.Count == 0)
		{
			return true;
		}
		List<double?> list = currentrow.ToList();
		list.Add(nextnode);
		double num = CalculateRatio(currentrow, length);
		double num2 = CalculateRatio(list, length);
		return num >= num2;
	}

	private double CalculateRatio(List<double?> row, float length)
	{
		double valueOrDefault = row.Sum().GetValueOrDefault();
		double? num = Math.Pow(length, 2.0) * row.Max() / Math.Pow(valueOrDefault, 2.0);
		double? num2 = Math.Pow(valueOrDefault, 2.0) / (Math.Pow(length, 2.0) * row.Min());
		return Math.Max(num.GetValueOrDefault(), num2.GetValueOrDefault());
	}

	public List<double> NormalizeDataForArea(List<double> data, float area)
	{
		List<double> list = new List<double>();
		double num = data.Sum();
		if (num == 0.0)
		{
			return list;
		}
		double num2 = (double)area / num;
		for (int i = 0; i < data.Count; i++)
		{
			list.Add(data[i] * num2);
		}
		return list;
	}

	private List<RectangleF> GetCoordinates(RectangleF rootContainer, List<double?> rowData)
	{
		List<RectangleF> list = new List<RectangleF>();
		double? num = rowData.Sum();
		if (num == 0.0)
		{
			return list;
		}
		float num2 = rootContainer.X;
		float num3 = rootContainer.Y;
		double? num4 = num / (double)rootContainer.Height;
		double? num5 = num / (double)rootContainer.Width;
		if (rootContainer.Width >= rootContainer.Height)
		{
			for (int i = 0; i < rowData.Count; i++)
			{
				float height = ((num4 != 0.0) ? ((float)(rowData[i].GetValueOrDefault() / num4).Value) : 0f);
				list.Add(new RectangleF(num2, num3, (float)num4.Value, height));
				num3 += (float)(rowData[i].GetValueOrDefault() / num4).Value;
			}
		}
		else
		{
			for (int j = 0; j < rowData.Count; j++)
			{
				float width = ((num5 != 0.0) ? ((float)(rowData[j].GetValueOrDefault() / num5).Value) : 0f);
				list.Add(new RectangleF(num2, num3, width, (float)num5.Value));
				num2 += (float)(rowData[j].GetValueOrDefault() / num5).Value;
			}
		}
		return list;
	}

	protected override void PrepareInfo(RectangleF rect)
	{
	}

	public StiTreemapAreaCoreXF(IStiArea area)
		: base(area)
	{
	}
}
