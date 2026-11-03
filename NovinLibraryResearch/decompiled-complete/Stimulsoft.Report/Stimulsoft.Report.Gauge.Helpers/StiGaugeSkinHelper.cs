using Stimulsoft.Report.Gauge.Skins;

namespace Stimulsoft.Report.Gauge.Helpers;

internal static class StiGaugeSkinHelper
{
	public static StiGaugeElementSkin GetMarkerSkin(StiMarkerSkin skin)
	{
		return skin switch
		{
			StiMarkerSkin.Diamond => new StiMarker1Skin(), 
			StiMarkerSkin.Rectangle => new StiMarker2Skin(), 
			StiMarkerSkin.TriangleTop => new StiMarker3Skin(), 
			StiMarkerSkin.TriangleBottom => new StiMarker4Skin(), 
			StiMarkerSkin.PentagonTop => new StiMarker5Skin(), 
			StiMarkerSkin.PentagonBottom => new StiMarker6Skin(), 
			StiMarkerSkin.Ellipse => new StiMarker7Skin(), 
			StiMarkerSkin.RectangularCalloutTop => new StiMarker8Skin(), 
			StiMarkerSkin.RectangularCalloutBottom => new StiMarker9Skin(), 
			StiMarkerSkin.TriangleLeft => new StiMarker10Skin(), 
			StiMarkerSkin.TriangleRight => new StiMarker11Skin(), 
			StiMarkerSkin.PentagonLeft => new StiMarker12Skin(), 
			StiMarkerSkin.PentagonRight => new StiMarker13Skin(), 
			StiMarkerSkin.RectangularCalloutLeft => new StiMarker14Skin(), 
			_ => null, 
		};
	}

	public static StiGaugeElementSkin GetTickMarkSkin(StiTickMarkSkin skin)
	{
		return skin switch
		{
			StiTickMarkSkin.Rectangle => new StiMark1Skin(), 
			StiTickMarkSkin.Ellipse => new StiMark2Skin(), 
			StiTickMarkSkin.Diamond => new StiMark3Skin(), 
			StiTickMarkSkin.TriangleTop => new StiMark4Skin(), 
			StiTickMarkSkin.TriangleRight => new StiMark5Skin(), 
			StiTickMarkSkin.TriangleLeft => new StiMark6Skin(), 
			StiTickMarkSkin.TriangleBottom => new StiMark7Skin(), 
			_ => null, 
		};
	}

	public static StiGaugeElementSkin GetStateIndicatorSkin(StiStateSkin skin)
	{
		return skin switch
		{
			StiStateSkin.Ellipse => new StiState1Skin(), 
			StiStateSkin.Rectangle => new StiState2Skin(), 
			StiStateSkin.Diamond => new StiState3Skin(), 
			_ => null, 
		};
	}

	public static StiGaugeElementSkin GetNeedleIndicatorSkin(StiNeedleSkin skin)
	{
		return skin switch
		{
			StiNeedleSkin.DefaultNeedle => new StiNeedleIndicator1Skin(), 
			StiNeedleSkin.SpeedometerNeedle => new StiNeedleIndicator2Skin(), 
			StiNeedleSkin.SpeedometerNeedle2 => new StiNeedleIndicator3Skin(), 
			StiNeedleSkin.SimpleNeedle => new StiNeedleIndicator4Skin(), 
			_ => null, 
		};
	}
}
