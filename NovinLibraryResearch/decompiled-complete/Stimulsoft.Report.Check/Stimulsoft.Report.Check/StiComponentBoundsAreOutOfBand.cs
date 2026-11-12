using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiComponentBoundsAreOutOfBand : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiComponentBoundsAreOutOfBandShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiComponentBoundsAreOutOfBandLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Warning;

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

	private bool Check()
	{
		if (((StiComponent)base.Element).Parent is StiBand stiBand && !(base.Element is StiContainer))
		{
			RectangleD paintRectangle = ((StiComponent)base.Element).GetPaintRectangle(convertToHInches: false, convertByZoom: false);
			if (((StiComponent)base.Element) is StiHorizontalLinePrimitive)
			{
				paintRectangle.Height = 0.0;
			}
			if (((StiComponent)base.Element) is StiVerticalLinePrimitive)
			{
				paintRectangle.Width = 0.0;
			}
			RectangleD paintRectangle2 = stiBand.GetPaintRectangle(convertToHInches: false, convertByZoom: false);
			if ((decimal)paintRectangle.Left < (decimal)paintRectangle2.Left || (decimal)paintRectangle.Top < (decimal)paintRectangle2.Top || (decimal)paintRectangle.Right > (decimal)paintRectangle2.Right || (decimal)paintRectangle.Bottom > (decimal)paintRectangle2.Bottom)
			{
				return true;
			}
		}
		return false;
	}

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		try
		{
			if (Check())
			{
				StiComponentBoundsAreOutOfBand stiComponentBoundsAreOutOfBand = new StiComponentBoundsAreOutOfBand();
				stiComponentBoundsAreOutOfBand.Element = obj;
				if (!NotAllowToDelete)
				{
					stiComponentBoundsAreOutOfBand.Actions.Add(new StiDeleteComponentAction());
				}
				return stiComponentBoundsAreOutOfBand;
			}
			return null;
		}
		finally
		{
			base.Element = null;
		}
	}
}
