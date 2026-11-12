using System;
using System.Drawing;
using System.IO;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Image;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Dashboard.Drawing.Atoms;

public class StiImageAtom : StiElementAtom
{
	private object imageLoadedFromDataColumn;

	protected StiImageElement ImageElement => base.Element as StiImageElement;

	private object GetImageObject()
	{
		if (ImageElement.IsImageHyperlinkDataColumnDefined)
		{
			if (StiHyperlinkProcessor.IsHttpOrHttpsHyperlink(imageLoadedFromDataColumn as string))
			{
				return StiBytesFromURL.Load(imageLoadedFromDataColumn as string);
			}
			if (StiHyperlinkProcessor.IsFilePath(imageLoadedFromDataColumn as string))
			{
				return File.ReadAllBytes(imageLoadedFromDataColumn as string);
			}
			return imageLoadedFromDataColumn;
		}
		return ImageElement.GetImageBytes();
	}

	public override void Paint(Graphics g)
	{
		object imageObject = GetImageObject();
		if (imageObject == null)
		{
			return;
		}
		int num = (int)Math.Min(base.Location.Width, base.Location.Height);
		if (num < StiScale.XXI(200.0))
		{
			num = StiScale.XXI(200.0);
		}
		Image imageFromObject = StiImageHelper.GetImageFromObject(imageObject, num, num);
		try
		{
			if (imageFromObject != null)
			{
				RectangleD rectangleD = base.Location;
				if (ImageElement.AspectRatio)
				{
					double num2 = rectangleD.Width / (double)imageFromObject.Width;
					double num3 = rectangleD.Height / (double)imageFromObject.Height;
					num2 = (num3 = ((num2 < num3) ? num2 : num3));
					float num4 = (float)((double)imageFromObject.Width * num2);
					float num5 = (float)((double)imageFromObject.Height * num3);
					float num6 = CalculateHorAlignmentPosition(ImageElement, rectangleD.ToRectangleF(), num4);
					float num7 = CalculateVertAlignmentPosition(ImageElement, rectangleD.ToRectangleF(), num5);
					rectangleD = new RectangleD(num6, num7, num4, num5);
				}
				g.DrawImage(imageFromObject, rectangleD.ToRectangleF());
			}
		}
		finally
		{
			((IDisposable)imageFromObject)?.Dispose();
		}
	}

	public override void Render()
	{
		if (ImageElement.IsImageHyperlinkDataColumnDefined)
		{
			object[] array = DataTable?.Rows.FirstOrDefault();
			if (array != null)
			{
				imageLoadedFromDataColumn = array.FirstOrDefault();
			}
		}
	}

	private float CalculateHorAlignmentPosition(StiImageElement element, RectangleF rect, float imageWidth)
	{
		return element.HorAlignment switch
		{
			StiHorAlignment.Left => rect.X, 
			StiHorAlignment.Right => rect.Right - imageWidth, 
			_ => rect.X + (rect.Width - imageWidth) / 2f, 
		};
	}

	private float CalculateVertAlignmentPosition(StiImageElement element, RectangleF rect, float imageHeight)
	{
		return element.VertAlignment switch
		{
			StiVertAlignment.Top => rect.Y, 
			StiVertAlignment.Bottom => rect.Bottom - imageHeight, 
			_ => rect.Y + (rect.Height - imageHeight) / 2f, 
		};
	}

	public StiImageAtom(IStiElement element, RectangleD location, StiDataTable dataTable)
		: base(element, location, dataTable)
	{
	}
}
