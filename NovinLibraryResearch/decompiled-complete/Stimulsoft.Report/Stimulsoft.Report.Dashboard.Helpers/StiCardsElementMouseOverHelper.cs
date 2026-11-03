using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Dashboard.Helpers;

internal static class StiCardsElementMouseOverHelper
{
	internal static IStiCardsElement CardsElement { get; set; }

	internal static PointD? MouseOverPoint { get; set; }

	internal static void SetMouseOverPoint(IStiCardsElement cards, PointD point)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		CardsElement = cards;
		MouseOverPoint = point;
	}

	public static PointD? GetMouseOverPoint(IStiCardsElement cards, bool useZoom = false)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		IStiCardsElement cardsElement = CardsElement;
		if (((cardsElement != null) ? ((IStiAppCell)cardsElement).GetKey() : null) == ((cards != null) ? ((IStiAppCell)cards).GetKey() : null) && MouseOverPoint.HasValue)
		{
			if (useZoom)
			{
				PointD value = MouseOverPoint.Value;
				double num = ((PointD)(ref value)).X * cards.Zoom;
				value = MouseOverPoint.Value;
				return new PointD(num, ((PointD)(ref value)).Y * cards.Zoom);
			}
			return MouseOverPoint;
		}
		return null;
	}

	public static void ResetMouseOver(IStiCardsElement cards)
	{
		IStiCardsElement cardsElement = CardsElement;
		if (((cardsElement != null) ? ((IStiAppCell)cardsElement).GetKey() : null) == ((cards != null) ? ((IStiAppCell)cards).GetKey() : null))
		{
			CardsElement = null;
			MouseOverPoint = null;
		}
	}
}
