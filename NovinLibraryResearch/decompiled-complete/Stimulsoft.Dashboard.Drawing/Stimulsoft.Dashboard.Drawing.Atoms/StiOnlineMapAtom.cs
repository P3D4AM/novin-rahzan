using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Dashboard.Components.OnlineMap;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;

namespace Stimulsoft.Dashboard.Drawing.Atoms;

public class StiOnlineMapAtom : StiElementAtom
{
	private RectangleD lastRect = RectangleD.Empty;

	private bool isExporting;

	private Hashtable mapData;

	protected StiOnlineMapElement OnlineMapElement => base.Element as StiOnlineMapElement;

	public Size Size => base.Location.Size.ToSize();

	internal Hashtable MapData
	{
		get
		{
			if (mapData == null)
			{
				mapData = StiOnlineMapHelper.CalculateMapData(DataTable, OnlineMapElement);
			}
			return mapData;
		}
	}

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
				mapData = null;
			}
		}
	}

	public override void Paint(Graphics g)
	{
		if (StiDbsThumbnaiModelHelper.AllowPage(base.Element.Page))
		{
			return;
		}
		try
		{
			RectangleD rectangleD = base.Location;
			SizeD size = SizeD.CreateFromSize(Size);
			StiCornerRadius stiCornerRadius = (base.Element as IStiCornerRadius)?.CornerRadius;
			double scale = StiElementScale.Factor(base.Element);
			if (stiCornerRadius != null && !stiCornerRadius.IsEmpty)
			{
				rectangleD = StiCornerRadiusHelper.FitRectIntoCornerRadius(rectangleD, ref size, stiCornerRadius, scale, base.Element is IStiFixedHeightElement);
			}
			Size size2 = size.ToSize();
			int uniqueCode = (base.Element as IStiOnlineMapElement).GetUniqueCode();
			bool flag = IsResizing();
			WebBrowser val = StiOnlineMapRepaintHelper.Get(base.Element, uniqueCode, flag);
			StiBingMapHelper.DrawImage(base.Element, size2, (val == null || isExporting) ? MapData : null, g, rectangleD, base.Element.Key, uniqueCode, flag, isExporting);
		}
		catch (Exception e)
		{
			PaintInError(g, e);
		}
	}

	public override void Render()
	{
		mapData = null;
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

	private bool IsResizing()
	{
		bool result = OnlineMapElement.IsDesigning && (!OnlineMapElement.Report.GetCurrentPage().OffsetRectangle.IsEmpty || (lastRect != OnlineMapElement.ClientRectangle && !lastRect.IsEmpty));
		lastRect = OnlineMapElement.ClientRectangle;
		return result;
	}

	public StiOnlineMapAtom(IStiElement element, RectangleD location, StiDataTable dataTable, bool isExporting)
		: base(element, location, dataTable)
	{
		this.isExporting = isExporting;
	}
}
