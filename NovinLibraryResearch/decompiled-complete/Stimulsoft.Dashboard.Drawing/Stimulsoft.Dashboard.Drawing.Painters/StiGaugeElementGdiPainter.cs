using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Dashboard.Components.Gauge;
using Stimulsoft.Gauge.Painters;
using Stimulsoft.Report.Components.Gauge;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Gauge;

namespace Stimulsoft.Dashboard.Drawing.Painters;

public class StiGaugeElementGdiPainter : StiAtomElementGdiPainter
{
	public override void PaintCheckBoxTitle(Graphics g, RectangleD rect, IStiElement element)
	{
		base.PaintCheckBoxTitle(g, rect, element);
		StiSortElementGdiPainter.PaintSortButton(g, rect, element);
	}

	public override void PaintEmptyData(Graphics g, RectangleD rect, IStiElement element, double? zoom = null)
	{
		StiGaugeElement stiGaugeElement = (StiGaugeElement)element;
		switch (stiGaugeElement.Type)
		{
		case StiGaugeType.FullCircular:
			PaintFullCircularEmptyData(g, rect, element, zoom);
			break;
		case StiGaugeType.HalfCircular:
			PaintHalfCircularEmptyData(g, rect, element, zoom);
			break;
		case StiGaugeType.Linear:
			PaintLinearEmptyData(g, rect, element, zoom);
			break;
		case StiGaugeType.HorizontalLinear:
			PaintHorizontalLinearEmptyData(g, rect, element, zoom);
			break;
		case StiGaugeType.Bullet:
			PaintBulletEmptyData(g, rect, element, zoom);
			break;
		}
	}

	private void PaintBulletEmptyData(Graphics g, RectangleD rect, IStiElement element, double? zoom)
	{
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		Color dataEmptyColor = StiDashboardStyleHelper.GetDataEmptyColor(element);
		Color dataEmptyForeColor = StiDashboardStyleHelper.GetDataEmptyForeColor(element);
		StiGauge stiGauge = new StiGauge();
		StiLinearScale stiLinearScale = new StiLinearScale
		{
			AllowApplyStyle = false,
			RelativeHeight = 0.85f,
			Orientation = (Orientation)0,
			StartWidth = 0.1f,
			EndWidth = 0.1f,
			Maximum = 1f,
			MajorInterval = 1f,
			BorderBrush = new StiEmptyBrush(),
			Brush = new StiSolidBrush(dataEmptyColor)
		};
		StiLinearTickLabelMajor element2 = new StiLinearTickLabelMajor
		{
			AllowApplyStyle = false,
			TextBrush = new StiSolidBrush(dataEmptyForeColor)
		};
		stiLinearScale.Items.Add(element2);
		StiLinearTickLabelCustom stiLinearTickLabelCustom = new StiLinearTickLabelCustom
		{
			Offset = 0.1f,
			Font = new Font("Arial", 14f),
			TextBrush = new StiSolidBrush(dataEmptyForeColor),
			Placement = StiPlacement.Inside,
			TextObj = StiLocalization.Get("Dashboard", "Blank")
		};
		stiLinearTickLabelCustom.Values.Add(new StiLinearTickLabelCustomValue(0.5f, StiLocalization.Get("Dashboard", "Blank")));
		stiLinearScale.Items.Add(stiLinearTickLabelCustom);
		stiGauge.Scales.Add(stiLinearScale);
		float zoom2 = ((float?)zoom) ?? 1f;
		StiGdiGaugeContextPainter stiGdiGaugeContextPainter = new StiGdiGaugeContextPainter(g, stiGauge, rect.ToRectangleF(), zoom2);
		stiGauge.DrawGauge(stiGdiGaugeContextPainter);
		stiGdiGaugeContextPainter.Render();
	}

	private void PaintHorizontalLinearEmptyData(Graphics g, RectangleD rect, IStiElement element, double? zoom)
	{
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		Color dataEmptyColor = StiDashboardStyleHelper.GetDataEmptyColor(element);
		Color dataEmptyForeColor = StiDashboardStyleHelper.GetDataEmptyForeColor(element);
		StiGauge stiGauge = new StiGauge();
		StiLinearScale stiLinearScale = new StiLinearScale
		{
			AllowApplyStyle = false,
			RelativeHeight = 0.92f,
			Orientation = (Orientation)0,
			StartWidth = 0.1f,
			EndWidth = 0.1f,
			Maximum = 1f,
			MajorInterval = 1f,
			BorderBrush = new StiEmptyBrush(),
			Brush = new StiSolidBrush(dataEmptyColor)
		};
		StiLinearTickLabelMajor element2 = new StiLinearTickLabelMajor
		{
			AllowApplyStyle = false,
			TextBrush = new StiSolidBrush(dataEmptyForeColor)
		};
		stiLinearScale.Items.Add(element2);
		StiLinearTickLabelCustom stiLinearTickLabelCustom = new StiLinearTickLabelCustom
		{
			Offset = 0.1f,
			Font = new Font("Arial", 14f),
			TextBrush = new StiSolidBrush(dataEmptyForeColor),
			Placement = StiPlacement.Inside,
			TextObj = StiLocalization.Get("Dashboard", "Blank")
		};
		stiLinearTickLabelCustom.Values.Add(new StiLinearTickLabelCustomValue(0.5f, StiLocalization.Get("Dashboard", "Blank")));
		stiLinearScale.Items.Add(stiLinearTickLabelCustom);
		stiGauge.Scales.Add(stiLinearScale);
		float zoom2 = ((float?)zoom) ?? 1f;
		StiGdiGaugeContextPainter stiGdiGaugeContextPainter = new StiGdiGaugeContextPainter(g, stiGauge, rect.ToRectangleF(), zoom2);
		stiGauge.DrawGauge(stiGdiGaugeContextPainter);
		stiGdiGaugeContextPainter.Render();
	}

	private void PaintLinearEmptyData(Graphics g, RectangleD rect, IStiElement element, double? zoom)
	{
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		Color dataEmptyColor = StiDashboardStyleHelper.GetDataEmptyColor(element);
		Color dataEmptyForeColor = StiDashboardStyleHelper.GetDataEmptyForeColor(element);
		StiGauge stiGauge = new StiGauge();
		StiLinearScale stiLinearScale = new StiLinearScale
		{
			AllowApplyStyle = false,
			RelativeHeight = 0.84f,
			Orientation = (Orientation)1,
			StartWidth = 0.1f,
			EndWidth = 0.1f,
			Maximum = 1f,
			MajorInterval = 1f,
			BorderBrush = new StiEmptyBrush(),
			Brush = new StiSolidBrush(dataEmptyColor)
		};
		StiLinearTickLabelMajor element2 = new StiLinearTickLabelMajor
		{
			AllowApplyStyle = false,
			TextBrush = new StiSolidBrush(dataEmptyForeColor)
		};
		stiLinearScale.Items.Add(element2);
		StiLinearTickLabelCustom stiLinearTickLabelCustom = new StiLinearTickLabelCustom
		{
			Font = new Font("Arial", 14f),
			TextBrush = new StiSolidBrush(dataEmptyForeColor),
			Placement = StiPlacement.Overlay,
			TextObj = StiLocalization.Get("Dashboard", "Blank")
		};
		stiLinearTickLabelCustom.Values.Add(new StiLinearTickLabelCustomValue(0.5f, StiLocalization.Get("Dashboard", "Blank")));
		stiLinearScale.Items.Add(stiLinearTickLabelCustom);
		stiGauge.Scales.Add(stiLinearScale);
		float zoom2 = ((float?)zoom) ?? 1f;
		StiGdiGaugeContextPainter stiGdiGaugeContextPainter = new StiGdiGaugeContextPainter(g, stiGauge, rect.ToRectangleF(), zoom2);
		stiGauge.DrawGauge(stiGdiGaugeContextPainter);
		stiGdiGaugeContextPainter.Render();
	}

	private void PaintHalfCircularEmptyData(Graphics g, RectangleD rect, IStiElement element, double? zoom)
	{
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		Color dataEmptyColor = StiDashboardStyleHelper.GetDataEmptyColor(element);
		Color dataEmptyForeColor = StiDashboardStyleHelper.GetDataEmptyForeColor(element);
		StiGauge stiGauge = new StiGauge();
		StiRadialScale stiRadialScale = new StiRadialScale
		{
			AllowApplyStyle = false,
			StartAngle = 180f,
			SweepAngle = 180f,
			StartWidth = 0.13f,
			EndWidth = 0.13f,
			Minimum = 0f,
			Maximum = 1f,
			Radius = 0.92f,
			MajorInterval = 1f,
			Center = new PointF(0.5f, 0.7f),
			BorderBrush = new StiEmptyBrush(),
			Brush = new StiSolidBrush(dataEmptyColor)
		};
		StiRadialTickLabelMajor element2 = new StiRadialTickLabelMajor
		{
			AllowApplyStyle = false,
			TextBrush = new StiSolidBrush(dataEmptyForeColor)
		};
		stiRadialScale.Items.Add(element2);
		StiRadialTickLabelCustom stiRadialTickLabelCustom = new StiRadialTickLabelCustom
		{
			Offset = 0.8f,
			Font = new Font("Arial", 14f),
			TextBrush = new StiSolidBrush(dataEmptyForeColor),
			Placement = StiPlacement.Overlay
		};
		stiRadialTickLabelCustom.Values.Add(new StiRadialTickLabelCustomValue(0.5f, StiLocalization.Get("Dashboard", "Blank")));
		stiRadialScale.Items.Add(stiRadialTickLabelCustom);
		stiGauge.Scales.Add(stiRadialScale);
		float zoom2 = ((float?)zoom) ?? 1f;
		StiGdiGaugeContextPainter stiGdiGaugeContextPainter = new StiGdiGaugeContextPainter(g, stiGauge, rect.ToRectangleF(), zoom2);
		stiGauge.DrawGauge(stiGdiGaugeContextPainter);
		stiGdiGaugeContextPainter.Render();
	}

	private void PaintFullCircularEmptyData(Graphics g, RectangleD rect, IStiElement element, double? zoom)
	{
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Expected O, but got Unknown
		Color dataEmptyColor = StiDashboardStyleHelper.GetDataEmptyColor(element);
		Color dataEmptyForeColor = StiDashboardStyleHelper.GetDataEmptyForeColor(element);
		StiGauge stiGauge = new StiGauge();
		StiRadialScale stiRadialScale = new StiRadialScale
		{
			AllowApplyStyle = false,
			StartAngle = 120f,
			SweepAngle = 300f,
			StartWidth = 0.13f,
			EndWidth = 0.13f,
			Minimum = 0f,
			Maximum = 1f,
			MajorInterval = 1f,
			BorderBrush = new StiEmptyBrush(),
			Brush = new StiSolidBrush(dataEmptyColor)
		};
		StiRadialTickLabelMajor element2 = new StiRadialTickLabelMajor
		{
			AllowApplyStyle = false,
			TextBrush = new StiSolidBrush(dataEmptyForeColor)
		};
		stiRadialScale.Items.Add(element2);
		stiGauge.Scales.Add(stiRadialScale);
		float zoom2 = ((float?)zoom) ?? 1f;
		StiGdiGaugeContextPainter stiGdiGaugeContextPainter = new StiGdiGaugeContextPainter(g, stiGauge, rect.ToRectangleF(), zoom2);
		stiGauge.DrawGauge(stiGdiGaugeContextPainter);
		stiGdiGaugeContextPainter.Render();
		string text = StiLocalization.Get("Dashboard", "Blank");
		Font gdiFont = StiElementConsts.DragDrop.Font.GetGdiFont(zoom.GetValueOrDefault(element.Zoom), 14f);
		try
		{
			StringFormat val = new StringFormat();
			try
			{
				val.LineAlignment = (StringAlignment)1;
				val.Alignment = (StringAlignment)1;
				StiTextDrawing.DrawString(g, text, gdiFont, dataEmptyForeColor, rect, val);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)gdiFont)?.Dispose();
		}
	}
}
