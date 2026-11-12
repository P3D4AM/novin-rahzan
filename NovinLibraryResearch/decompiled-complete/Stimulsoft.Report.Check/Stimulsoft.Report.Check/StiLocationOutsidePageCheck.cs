using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiLocationOutsidePageCheck : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage
	{
		get
		{
			if (IsOutsidePage)
			{
				return string.Format(StiLocalizationExt.Get("CheckComponent", "StiLocationOutsidePageCheckShort"), ElementName);
			}
			return string.Format(StiLocalizationExt.Get("CheckComponent", "StiLocationOutsidePrintableAreaCheckShort"), ElementName);
		}
	}

	public override string LongMessage
	{
		get
		{
			if (base.Element != null)
			{
				if (IsOutsidePage)
				{
					return string.Format(StiLocalizationExt.Get("CheckComponent", "StiLocationOutsidePageCheckLong"), ElementName);
				}
				return string.Format(StiLocalizationExt.Get("CheckComponent", "StiLocationOutsidePrintableAreaCheckLong"), ElementName);
			}
			return string.Format(StiLocalizationExt.Get("CheckComponent", "StiLocationOutsidePageCheckLong"), ElementName);
		}
	}

	public override StiCheckStatus Status => StiCheckStatus.Warning;

	public bool IsOutsidePage
	{
		get
		{
			StiComponent stiComponent = base.Element as StiComponent;
			StiPage page = stiComponent.Page;
			if (page == null)
			{
				return false;
			}
			RectangleD paintRectangle = stiComponent.GetPaintRectangle(convertToHInches: false, convertByZoom: false);
			if (stiComponent is StiHorizontalLinePrimitive)
			{
				paintRectangle.Height = 0.0;
			}
			if (stiComponent is StiVerticalLinePrimitive)
			{
				paintRectangle.Width = 0.0;
			}
			double num = (page.PageHeight - page.Margins.Top - page.Margins.Bottom) * (double)page.SegmentPerHeight;
			num *= page.LargeHeightAutoFactor;
			RectangleD rectangleD = new RectangleD(0.0 - page.Margins.Left, 0.0 - page.Margins.Top, page.Width + page.Margins.Left + page.Margins.Right, num + page.Margins.Top + page.Margins.Bottom);
			if (page.IsDashboard)
			{
				rectangleD = new RectangleD(0.0 - page.Margins.Left, 0.0 - page.Margins.Top, page.ClientRectangle.Width + page.Margins.Left + page.Margins.Right, page.ClientRectangle.Height + page.Margins.Top + page.Margins.Bottom);
			}
			if ((decimal)paintRectangle.Left < (decimal)rectangleD.Left || (decimal)paintRectangle.Top < (decimal)rectangleD.Top || (decimal)paintRectangle.Right > (decimal)rectangleD.Right)
			{
				return true;
			}
			if ((decimal)paintRectangle.Bottom > (decimal)rectangleD.Bottom)
			{
				if (stiComponent is StiBand)
				{
					return false;
				}
				if (stiComponent.Parent != stiComponent.Page && stiComponent.Bottom < num)
				{
					return false;
				}
				return true;
			}
			return false;
		}
	}

	public bool IsOutsidePrintableArea
	{
		get
		{
			StiComponent stiComponent = base.Element as StiComponent;
			StiPage page = stiComponent.Page;
			if (page == null)
			{
				return false;
			}
			RectangleD paintRectangle = stiComponent.GetPaintRectangle(convertToHInches: false, convertByZoom: false);
			if (stiComponent is StiHorizontalLinePrimitive)
			{
				paintRectangle.Height = 0.0;
			}
			if (stiComponent is StiVerticalLinePrimitive)
			{
				paintRectangle.Width = 0.0;
			}
			double num = (page.PageHeight - page.Margins.Top - page.Margins.Bottom) * (double)page.SegmentPerHeight;
			num *= page.LargeHeightAutoFactor;
			RectangleD rectangleD = new RectangleD(0.0, 0.0, page.Width, num);
			if (page.IsDashboard)
			{
				rectangleD = new RectangleD(0.0, 0.0, page.ClientRectangle.Width, page.ClientRectangle.Height);
			}
			if ((decimal)paintRectangle.Left < (decimal)rectangleD.Left || (decimal)paintRectangle.Top < (decimal)rectangleD.Top || (decimal)paintRectangle.Right > (decimal)(rectangleD.Right * 1.01))
			{
				return true;
			}
			if ((decimal)paintRectangle.Bottom > (decimal)(rectangleD.Bottom * 1.01))
			{
				if (stiComponent is StiBand)
				{
					return false;
				}
				if (stiComponent.Parent != stiComponent.Page && stiComponent.Bottom < num)
				{
					return false;
				}
				return true;
			}
			return false;
		}
	}

	private bool NotAllowToDelete
	{
		get
		{
			if (base.Element is StiComponent)
			{
				return !(base.Element as StiComponent).AllowDelete;
			}
			return false;
		}
	}

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		try
		{
			if (IsOutsidePage || IsOutsidePrintableArea)
			{
				StiLocationOutsidePageCheck stiLocationOutsidePageCheck = new StiLocationOutsidePageCheck();
				stiLocationOutsidePageCheck.Element = obj;
				if (IsOutsidePage)
				{
					stiLocationOutsidePageCheck.Actions.Add(new StiMoveComponentToPageAreaAction());
				}
				stiLocationOutsidePageCheck.Actions.Add(new StiMoveComponentToPrintablePageAreaAction());
				if (!NotAllowToDelete)
				{
					stiLocationOutsidePageCheck.Actions.Add(new StiDeleteComponentAction());
				}
				return stiLocationOutsidePageCheck;
			}
			return null;
		}
		finally
		{
			base.Element = null;
		}
	}
}
