using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Gis;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Dashboard.Components.OnlineMap;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Extensions;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Dashboard.Drawing.Atoms;

public class StiGisMapAtom : StiElementAtom
{
	private bool isExporting;

	protected StiOnlineMapElement OnlineMapElement => base.Element as StiOnlineMapElement;

	public Size Size => base.Location.Size.ToSize();

	public override StiDataTable DataTable
	{
		get
		{
			return base.DataTable;
		}
		set
		{
			if (DataTable != value)
			{
				base.DataTable = value;
			}
		}
	}

	public override void Paint(Graphics g)
	{
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Expected O, but got Unknown
		if (StiDbsThumbnaiModelHelper.AllowPage(base.Element.Page))
		{
			return;
		}
		try
		{
			StiGisMapData data = new StiGisMapData(OnlineMapElement.GisSettings.Icon, OnlineMapElement.GisSettings.IconColor)
			{
				Size = Size,
				GeometryColor = OnlineMapElement.GisSettings.Color,
				GeometryLineSize = OnlineMapElement.GisSettings.Size,
				Commands = GetStringValues(DataTable?.Meters?.FirstOrDefault((IStiMeter f) => f is StiGisMapMeter)),
				Colors = GetStringValues(DataTable?.Meters?.FirstOrDefault((IStiMeter f) => f is StiGisMapColorMeter)),
				LineSizes = GetObjectValues(DataTable?.Meters?.FirstOrDefault((IStiMeter f) => f is StiGisMapLineSizeMeter)),
				Descriptions = GetStringValues(DataTable?.Meters?.FirstOrDefault((IStiMeter f) => f is StiGisMapDescriptionMeter)),
				ShowPlacemark = OnlineMapElement.GisSettings.ShowPlacemark,
				Language = OnlineMapElement.GisSettings.Language
			};
			if (isExporting)
			{
				IStiGisMapSimpleView forExport = StiGisMapRepaintHelper.GetForExport(OnlineMapElement.Provider, data);
				if (forExport != null)
				{
					forExport.RunAndWait();
					forExport.DrawMap(g);
				}
				return;
			}
			IStiGisMapSimpleView stiGisMapSimpleView = StiGisMapRepaintHelper.Get(OnlineMapElement, OnlineMapElement.Provider, data);
			if (stiGisMapSimpleView != null)
			{
				GraphicsState val = g.Save();
				g.TranslateTransform((float)base.Location.X, (float)base.Location.Y);
				stiGisMapSimpleView.DrawMap(g);
				g.Restore(val);
				return;
			}
			Font gdiFont = StiElementConsts.DragDrop.Font.GetGdiFont(base.Element.Zoom);
			try
			{
				StringFormat val2 = new StringFormat();
				try
				{
					val2.LineAlignment = (StringAlignment)1;
					val2.Alignment = (StringAlignment)1;
					StiTextDrawing.DrawString(g, StiLocalization.Get("A_WebViewer", "Loading"), gdiFont, StiDashboardStyleHelper.GetForeColor(base.Element), base.Location, val2);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)gdiFont)?.Dispose();
			}
		}
		catch (Exception e)
		{
			PaintInError(g, e);
		}
	}

	public override void Render()
	{
	}

	protected List<string> GetStringValues(IStiMeter meter)
	{
		if (DataTable == null || meter == null)
		{
			return null;
		}
		int num = DataTable.Meters.IndexOf(meter);
		if (num == -1)
		{
			return null;
		}
		return DataTable.Rows.GetStringArrayItem(num).ToList();
	}

	protected List<object> GetObjectValues(IStiMeter meter)
	{
		if (DataTable == null || meter == null)
		{
			return null;
		}
		int num = DataTable.Meters.IndexOf(meter);
		if (num == -1)
		{
			return null;
		}
		return DataTable.Rows.GetArrayItem(num).ToList();
	}

	private void PaintInError(Graphics g, Exception e)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		Font gdiFont = StiElementConsts.DragDrop.Font.GetGdiFont(base.Element.Zoom);
		try
		{
			StringFormat val = new StringFormat();
			try
			{
				val.LineAlignment = (StringAlignment)1;
				val.Alignment = (StringAlignment)1;
				StiTextDrawing.DrawString(g, e.Message, gdiFont, Color.Red, base.Location, val);
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

	public StiGisMapAtom(IStiElement element, RectangleD location, StiDataTable dataTable, bool isExporting)
		: base(element, location, dataTable)
	{
		this.isExporting = isExporting;
	}
}
