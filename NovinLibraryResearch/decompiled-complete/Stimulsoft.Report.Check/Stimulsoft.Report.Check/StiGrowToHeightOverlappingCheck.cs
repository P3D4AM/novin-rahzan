using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiGrowToHeightOverlappingCheck : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiGrowToHeightOverlappingShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiGrowToHeightOverlappingLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Warning;

	private bool Check()
	{
		StiComponent stiComponent = base.Element as StiComponent;
		if (stiComponent.GrowToHeight && stiComponent.Parent != null && !(stiComponent.Parent is StiPage) && !(stiComponent.Parent is StiTable) && stiComponent.Parent.Components.Count > 1)
		{
			RectangleD clientRectangle = stiComponent.ClientRectangle;
			RectangleD rectangleD = new RectangleD(clientRectangle.X, clientRectangle.Y, clientRectangle.Width, stiComponent.Parent.Height - clientRectangle.Y);
			foreach (StiComponent component in stiComponent.Parent.Components)
			{
				if (component != stiComponent && !(component.Top < clientRectangle.Bottom) && rectangleD.IntersectsWith(component.ClientRectangle))
				{
					return true;
				}
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
				StiGrowToHeightOverlappingCheck stiGrowToHeightOverlappingCheck = new StiGrowToHeightOverlappingCheck();
				stiGrowToHeightOverlappingCheck.Element = obj;
				stiGrowToHeightOverlappingCheck.Actions.Add(new StiGrowToHeightOverlappingAction());
				return stiGrowToHeightOverlappingCheck;
			}
			return null;
		}
		finally
		{
			base.Element = null;
		}
	}
}
