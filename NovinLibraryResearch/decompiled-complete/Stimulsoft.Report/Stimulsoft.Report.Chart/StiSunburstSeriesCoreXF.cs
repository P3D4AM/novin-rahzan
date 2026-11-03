using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Chart;

public class StiSunburstSeriesCoreXF : StiSeriesCoreXF
{
	public override string LocalizedName => StiLocalization.Get("Chart", "Sunburst");

	public override void RenderSeries(StiContext context, RectangleF rect, StiAreaGeom geom, IStiSeries[] seriesCollection)
	{
		DataTable dataTable = GetDataTable(seriesCollection);
		if (dataTable.Rows.Count < 1)
		{
			float radius = GetRadius(context, rect);
			PointF pointCenter = GetPointCenter(rect);
			RectangleF clientRectangle = new RectangleF(pointCenter.X - radius, pointCenter.Y - radius, 2f * radius, 2f * radius);
			geom.CreateChildGeoms();
			geom.ChildGeoms.Add(new StiSunburstEmptySeriesElementGeom(clientRectangle));
			return;
		}
		int columnIndex = dataTable.Columns.Count - 1;
		for (int i = 0; i < dataTable.Rows.Count; i++)
		{
			for (int j = 0; j < seriesCollection.Length; j++)
			{
				if (seriesCollection.Length != 0 && seriesCollection[j].Arguments.Length > i)
				{
					dataTable.Rows[i][j + 1] = seriesCollection[j].Arguments[i];
				}
				else
				{
					dataTable.Rows[i][j + 1] = DBNull.Value;
				}
			}
			if (seriesCollection.Length != 0 && seriesCollection[0].Values.Length > i)
			{
				dataTable.Rows[i][columnIndex] = seriesCollection[0].Values[i];
			}
			else
			{
				dataTable.Rows[i][columnIndex] = DBNull.Value;
			}
		}
		Dictionary<string, DataTable> dictionary = new Dictionary<string, DataTable>();
		foreach (DataRow row in dataTable.Rows)
		{
			string key = row[1].ToString();
			if (dictionary.ContainsKey(key))
			{
				DataTable dataTable2 = dictionary[key];
				dataTable2.Rows.Add(row.ItemArray);
			}
			else
			{
				DataTable dataTable3 = dataTable.Clone();
				dataTable3.Rows.Add(row.ItemArray);
				dictionary.Add(key, dataTable3);
			}
		}
		float radius2 = GetRadius(context, rect);
		PointF pointCenter2 = GetPointCenter(rect);
		float gradPerValue = GetGradPerValue(dataTable);
		if (float.IsInfinity(gradPerValue))
		{
			return;
		}
		float num = 0f;
		int num2 = 0;
		List<StiSeriesLabelsGeom> labelGeoms = new List<StiSeriesLabelsGeom>();
		foreach (DataTable value in dictionary.Values)
		{
			double sumColumn = GetSumColumn(value, "Value");
			float num3 = (float)((double)gradPerValue * sumColumn);
			RenderComputeSeries(context, geom, value, pointCenter2, radius2, num, gradPerValue, num2, dictionary.Values.Count, ref labelGeoms);
			num += num3;
			num2++;
		}
		foreach (StiSeriesLabelsGeom item in labelGeoms)
		{
			if (item != null)
			{
				geom.CreateChildGeoms();
				geom.ChildGeoms.Add(item);
			}
		}
	}

	private void RenderComputeSeries(StiContext context, StiAreaGeom geom, DataTable seriesTable, PointF center, float radius, float startAngle, float gradPerValue, int colorIndex, int colorCount, ref List<StiSeriesLabelsGeom> labelGeoms)
	{
		int num = 1;
		float num2 = radius / (float)(seriesTable.Columns.Count - 1);
		for (int i = 1; i < seriesTable.Columns.Count - 1; i++)
		{
			Dictionary<object, double?> dictionary = new Dictionary<object, double?>();
			foreach (DataRow row in seriesTable.Rows)
			{
				object key = row[i];
				double? value = StiValueHelper.TryToNullableDouble(row[seriesTable.Columns.Count - 1]);
				if (dictionary.ContainsKey(key) && value.HasValue)
				{
					double valueOrDefault = StiValueHelper.TryToNullableDouble((object)dictionary[key]).GetValueOrDefault();
					dictionary[key] = Math.Abs(valueOrDefault) + Math.Abs(value.GetValueOrDefault());
				}
				else
				{
					dictionary[key] = value;
				}
			}
			object[] array = new object[dictionary.Keys.Count];
			dictionary.Keys.CopyTo(array, 0);
			double?[] array2 = new double?[dictionary.Values.Count];
			dictionary.Values.CopyTo(array2, 0);
			RenderLevelSeries(context, geom, array, array2, center, num2 * (float)num, num2 * (float)(num + 1), startAngle, gradPerValue, colorIndex, colorCount, num);
			List<StiSeriesLabelsGeom> collection = RenderLevelSeriesLebels(context, array, array2, center, num2 * (float)num, num2 * (float)(num + 1), startAngle, gradPerValue);
			labelGeoms.AddRange(collection);
			num++;
		}
	}

	private void RenderLevelSeries(StiContext context, StiAreaGeom geom, object[] arguments, double?[] values, PointF center, float radius, float radiusDt, float startAngle, float gradPerValue, int colorIndex, int colorCount, int level)
	{
		Color borderColor = (Color)base.Series.Core.GetSeriesBorderColor(colorIndex, colorCount);
		StiBrush seriesBrush = base.Series.Core.GetSeriesBrush(colorIndex, colorCount);
		IEnumerable<double?> source = values.Select((double? v) => v);
		if (borderColor.A == 0)
		{
			borderColor = Color.White;
		}
		TimeSpan globalDurationElement = StiChartHelper.GlobalDurationElement;
		for (int i = 0; i < source.Count(); i++)
		{
			double? value = source.ToList()[i];
			object obj = arguments[i];
			float num = (float)((double)gradPerValue * Math.Abs(value.GetValueOrDefault()));
			if (!string.IsNullOrEmpty(obj.ToString()))
			{
				StiSunburstSeriesElementGeom stiSunburstSeriesElementGeom = RenderSunburstElement(center, radius, radiusDt, borderColor, seriesBrush, startAngle, num, value, level, colorIndex, i, geom, new TimeSpan(globalDurationElement.Ticks / (colorCount + source.Count()) * (i + colorIndex)));
				if (stiSunburstSeriesElementGeom != null)
				{
					geom.CreateChildGeoms();
					geom.ChildGeoms.Add(stiSunburstSeriesElementGeom);
				}
			}
			startAngle += num;
		}
	}

	private List<StiSeriesLabelsGeom> RenderLevelSeriesLebels(StiContext context, object[] arguments, double?[] values, PointF center, float radius, float radiusDt, float startAngle, float gradPerValue)
	{
		List<StiSeriesLabelsGeom> list = new List<StiSeriesLabelsGeom>();
		IEnumerable<double?> source = values.Select((double? v) => v);
		for (int i = 0; i < source.Count(); i++)
		{
			double? labelValue = source.ToList()[i];
			object obj = arguments[i];
			float num = (float)((double)gradPerValue * Math.Abs(labelValue.GetValueOrDefault()));
			if (!string.IsNullOrEmpty(obj.ToString()))
			{
				IStiSeriesLabels seriesLabels = base.Series.Chart.SeriesLabels;
				if (base.Series.Chart != null && seriesLabels != null && seriesLabels.Visible && seriesLabels is IStiPieSeriesLabels { Visible: not false } stiPieSeriesLabels && (stiPieSeriesLabels.Step == 0 || i % stiPieSeriesLabels.Step == 0))
				{
					RectangleF measureRect;
					StiSeriesLabelsGeom item = ((StiPieSeriesLabelsCoreXF)stiPieSeriesLabels.Core).RenderLabel(base.Series, context, center, radius, radiusDt, startAngle + num / 2f, i, Math.Abs(labelValue.GetValueOrDefault()), labelValue, obj.ToString(), GetTag(i), measure: false, 0, 1, gradPerValue, out measureRect, drawValue: false, 0f);
					list.Add(item);
				}
			}
			startAngle += num;
		}
		return list;
	}

	private StiSunburstSeriesElementGeom RenderSunburstElement(PointF center, float radius, float radiusDt, Color borderColor, StiBrush brush, float start, float angle, double? value, int index1, int index2, int index3, StiAreaGeom areaGeom, TimeSpan beginTime)
	{
		if (angle == 0f || float.IsNaN(angle))
		{
			return null;
		}
		RectangleF rectangleF = new RectangleF(center.X - radius, center.Y - radius, radius * 2f, radius * 2f);
		RectangleF rectangleF2 = new RectangleF(center.X - radiusDt, center.Y - radiusDt, radiusDt * 2f, radiusDt * 2f);
		if (rectangleF.Width <= 0f && rectangleF.Height <= 0f)
		{
			return null;
		}
		List<StiSegmentGeom> list = new List<StiSegmentGeom>();
		list.Add(new StiArcSegmentGeom(rectangleF, start, angle));
		list.Add(new StiLineSegmentGeom(GetPoint(center, radius, start + angle), GetPoint(center, radiusDt, start + angle)));
		list.Add(new StiArcSegmentGeom(rectangleF2, start + angle, 0f - angle));
		list.Add(new StiLineSegmentGeom(GetPoint(center, radiusDt, start), GetPoint(center, radius, start)));
		list.Add(new StiCloseFigureSegmentGeom());
		return new StiSunburstSeriesElementGeom(areaGeom, value.GetValueOrDefault(), index1, index2, index3, base.Series, rectangleF, rectangleF2, list, borderColor, brush, start, start + angle, radius, radiusDt, beginTime);
	}

	protected PointF GetPoint(PointF centerPie, float radius, float angle)
	{
		float num = (float)(Math.PI * (double)angle / 180.0);
		return new PointF(centerPie.X + (float)Math.Cos(num) * radius, centerPie.Y + (float)Math.Sin(num) * radius);
	}

	private DataTable GetDataTable(IStiSeries[] seriesCollection)
	{
		DataTable dataTable = new DataTable();
		DataColumn dataColumn = new DataColumn("Id", Type.GetType("System.Int32"));
		dataColumn.Unique = true;
		dataColumn.AllowDBNull = false;
		dataColumn.AutoIncrement = true;
		dataColumn.AutoIncrementSeed = 0L;
		dataColumn.AutoIncrementStep = 1L;
		dataTable.Columns.Add(dataColumn);
		for (int i = 0; i < seriesCollection.Length; i++)
		{
			DataColumn dataColumn2 = new DataColumn($"Argument-{i}", typeof(string));
			dataColumn2.AllowDBNull = true;
			dataTable.Columns.Add(dataColumn2);
		}
		DataColumn dataColumn3 = new DataColumn("Value", typeof(double));
		dataColumn3.AllowDBNull = true;
		dataTable.Columns.Add(dataColumn3);
		int countRow = GetCountRow(seriesCollection);
		for (int j = 0; j < countRow; j++)
		{
			dataTable.Rows.Add();
		}
		return dataTable;
	}

	private int GetCountRow(IStiSeries[] seriesCollection)
	{
		int num = 0;
		foreach (IStiSeries stiSeries in seriesCollection)
		{
			num = Math.Max(num, stiSeries.Arguments.Length);
		}
		return num;
	}

	private float GetGradPerValue(DataTable dataTable)
	{
		double sumColumn = GetSumColumn(dataTable, "Value");
		return (float)(360.0 / sumColumn);
	}

	protected float GetRadius(StiContext context, RectangleF rect)
	{
		return Math.Min(rect.Width / 2f, rect.Height / 2f) * 0.95f;
	}

	protected PointF GetPointCenter(RectangleF rect)
	{
		return new PointF(rect.Width / 2f, rect.Height / 2f);
	}

	private double GetSumColumn(DataTable dataTable, string columnName)
	{
		return dataTable.AsEnumerable().Sum((DataRow row) => Math.Abs(row.Field<double?>(columnName).GetValueOrDefault()));
	}

	public StiSunburstSeriesCoreXF(IStiSeries series)
		: base(series)
	{
	}
}
