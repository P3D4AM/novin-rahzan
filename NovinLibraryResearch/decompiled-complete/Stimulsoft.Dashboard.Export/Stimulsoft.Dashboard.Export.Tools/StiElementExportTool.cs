using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Localization;
using Stimulsoft.Dashboard.Drawing.Atoms;
using Stimulsoft.Dashboard.Drawing.Painters;
using Stimulsoft.Dashboard.Export.Settings;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Maps;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Dashboard.Export.Tools;

public class StiElementExportTool : StiExportTool
{
	public override void Render(IStiElement element, StiPanel destination, RectangleD rect, StiDashboardExportSettings settings)
	{
		if (!RenderEmptyDataMessage(element, destination, rect, settings))
		{
			RenderContent(element, destination, rect, settings);
		}
	}

	protected virtual void RenderContent(IStiElement element, StiPanel destination, RectangleD rect, StiDashboardExportSettings settings)
	{
		double scale = 1.0;
		if (settings is StiPdfDashboardExportSettings)
		{
			scale = (double)(settings as StiPdfDashboardExportSettings).ImageQuality / 100.0;
		}
		if (settings is StiExcelDashboardExportSettings)
		{
			scale = (double)(settings as StiExcelDashboardExportSettings).ImageQuality / 100.0;
		}
		if (settings is StiHtmlDashboardExportSettings)
		{
			scale = (double)(settings as StiHtmlDashboardExportSettings).ImageQuality / 100.0;
		}
		Color backColor = StiDashboardStyleHelper.GetBackColor(element);
		destination.Components.Add(new StiImage(rect)
		{
			Name = element.Name + "_Content",
			Stretch = true,
			AspectRatio = false,
			ImageToDraw = (Image)(object)Draw(element, rect, scale),
			Brush = new StiSolidBrush(backColor)
		});
	}

	protected virtual Bitmap Draw(IStiElement element, RectangleD rect, double scale = 1.0)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		RectangleD rect2 = new RectangleD(0.0, 0.0, rect.Width * StiScale.Factor * scale, rect.Height * StiScale.Factor * scale);
		Bitmap val = new Bitmap((int)rect2.Width, (int)rect2.Height);
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				if (element is IStiBackColor)
				{
					Color color = StiDashboardStyleHelper.GetBackColor(element);
					if (color.A < byte.MaxValue)
					{
						color = Color.White;
					}
					val2.Clear(color);
					StiDrawing.FillRectangle(rect: new Rectangle(0, 0, ((Image)val).Width, ((Image)val).Height), g: val2, color: color);
				}
				else
				{
					val2.Clear(Color.White);
				}
				PaintContent(val2, rect2, element);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		catch
		{
		}
		return val;
	}

	protected virtual void PaintContent(Graphics g, RectangleD rect, IStiElement element)
	{
		StiErrorStack.SetOk(element.GetKey());
		GraphicsState val = g.Save();
		try
		{
			g.SetClip(rect.ToRectangleF());
			PaintAtom(g, rect, element);
		}
		catch (Exception ex)
		{
			StiErrorStack.SetError(element.GetKey(), ex.Message);
			if (StiPainter.GetPainter(element.GetType(), StiGuiMode.Gdi) is StiElementGdiPainter stiElementGdiPainter)
			{
				stiElementGdiPainter.PaintError(g, rect.ToRectangleF(), element);
			}
		}
		finally
		{
			g.Restore(val);
		}
	}

	public virtual void PaintAtom(Graphics g, RectangleD rect, IStiElement element)
	{
		StiDataTable stiDataTable = null;
		if (element.IsQuerable)
		{
			stiDataTable = GetDataTable(element);
			if (stiDataTable == null)
			{
				return;
			}
		}
		if ((stiDataTable == null || stiDataTable.IsEmpty) && element.IsQuerable)
		{
			if (StiPainter.GetPainter(element.GetType(), StiGuiMode.Gdi) is StiElementGdiPainter stiElementGdiPainter)
			{
				stiElementGdiPainter.PaintNoResult(g, rect, element);
			}
		}
		else if (element is IStiOnlineMapElement { IsDesigning: not false })
		{
			PaintOnlineMapAtom(g, rect, element, stiDataTable);
		}
		else
		{
			StiElementAtom.PaintElement(g, rect, element, stiDataTable, isExporting: true);
		}
	}

	public static StiDataTable GetDataTable(IStiElement element)
	{
		StiDataTable stiDataTable = (element as IStiManuallyEnteredData)?.GetManuallyEnteredDataTable();
		if (stiDataTable != null)
		{
			return stiDataTable;
		}
		return StiElementDataCache.TryToGetOrCreate(element);
	}

	private void PaintOnlineMapAtom(Graphics g, RectangleD rect, IStiElement element, StiDataTable dataTable)
	{
		StiOnlineMapAtom stiOnlineMapAtom = StiElementAtom.Get(rect, element, dataTable) as StiOnlineMapAtom;
		int uniqueCode = (element as IStiOnlineMapElement).GetUniqueCode();
		WebBrowser val = StiOnlineMapRepaintHelper.Get(element, uniqueCode);
		StiBingMapHelper.DrawImage(element, stiOnlineMapAtom.Size, (val == null) ? stiOnlineMapAtom.MapData : null, g, stiOnlineMapAtom.Location, element.Key, uniqueCode);
	}

	protected bool RenderEmptyDataMessage(IStiElement element, StiPanel destination, RectangleD rect, StiDashboardExportSettings settings)
	{
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		if (settings == null || !(settings is StiSvgDashboardExportSettings) || ((StiSvgDashboardExportSettings)settings).RenderEmptyContent)
		{
			return false;
		}
		if (element is IStiRegionMapElement && ((IStiRegionMapElement)element).DataFrom == StiMapSource.Manual)
		{
			return false;
		}
		if (element is IStiButtonElement)
		{
			return false;
		}
		if (element is IStiImageElement && ((element as IStiImageElement).Image != null || (element as IStiImageElement).Icon.HasValue || !string.IsNullOrEmpty((element as IStiImageElement).ImageHyperlink)))
		{
			return false;
		}
		if (element.GetMeters().Count <= 0)
		{
			IStiManuallyEnteredData obj = element as IStiManuallyEnteredData;
			if (obj == null || obj.DataMode != StiDataMode.ManuallyEnteringData)
			{
				SizeD size = rect.Size;
				Font val = new Font("Arial", 9f);
				string text = ((element is IStiImageElement) ? Loc.Get("Dashboard", "ImageNotSpecified") : Loc.Get("Dashboard", "DragDropDataFromDictionary"));
				SizeF sizeF = destination.GetMeasureGraphics().MeasureString(text, val);
				rect.Y += rect.Height / 2.0 - 48.0;
				rect.Height = Math.Min(32.0, rect.Height);
				if (size.Width >= 48.0 && size.Height >= 64.0)
				{
					rect.Y += ((size.Width > (double)(sizeF.Width + 16f)) ? 16 : 32);
					destination.Components.Add(new StiImage(rect)
					{
						Name = element.Name + "_EmptyImage",
						HorAlignment = StiHorAlignment.Center,
						ImageBytes = GetEmptyDataImage(element)
					});
				}
				if (size.Width > (double)(sizeF.Width + 16f) && size.Height >= 32.0)
				{
					rect.Y += 32.0;
					destination.Components.Add(new StiText(rect)
					{
						Name = element.Name + "_EmptyText",
						HorAlignment = StiTextHorAlignment.Center,
						VertAlignment = StiVertAlignment.Center,
						Font = val,
						Text = text,
						TextBrush = new StiSolidBrush(Color.DarkGray)
					});
				}
				return true;
			}
		}
		return false;
	}

	private byte[] GetEmptyDataImage(IStiElement element)
	{
		string name = element.GetType().Name;
		Assembly assembly = typeof(StiElementExportTool).Assembly;
		using Stream stream = assembly.GetManifestResourceStream("Stimulsoft.Dashboard.Export.Images.HintLabels." + name + ".png");
		byte[] array = new byte[stream.Length];
		stream.Read(array, 0, array.Length);
		return array;
	}

	public virtual string Format(IStiControlElement element, object value)
	{
		if (value != null && element != null)
		{
			if (value is DateTime && !(element.TextFormat is StiDateFormatService))
			{
				return ((DateTime)value).ToShortDateString();
			}
			return element.TextFormat?.Format(value);
		}
		return string.Empty;
	}
}
