namespace Stimulsoft.Report.Components;

public static class StiCrossLinePrimitiveHelper
{
	public static void RemoveIncorrectLinesFromContainer(StiComponentsCollection comps)
	{
		int num = 0;
		while (num < comps.Count)
		{
			if (comps[num] is StiCrossLinePrimitive stiCrossLinePrimitive)
			{
				stiCrossLinePrimitive.StoredStartPoint = null;
				stiCrossLinePrimitive.StoredEndPoint = null;
				stiCrossLinePrimitive.GetStartPoint();
				stiCrossLinePrimitive.GetEndPoint();
				if (stiCrossLinePrimitive.StoredStartPoint == null || stiCrossLinePrimitive.StoredEndPoint == null)
				{
					comps.RemoveAt(num);
					if (stiCrossLinePrimitive.StoredStartPoint != null && stiCrossLinePrimitive.StoredStartPoint.Parent != null)
					{
						stiCrossLinePrimitive.StoredStartPoint.Parent.Components.Remove(stiCrossLinePrimitive.StoredStartPoint);
					}
					if (stiCrossLinePrimitive.StoredEndPoint != null && stiCrossLinePrimitive.StoredEndPoint.Parent != null)
					{
						stiCrossLinePrimitive.StoredEndPoint.Parent.Components.Remove(stiCrossLinePrimitive.StoredEndPoint);
					}
					continue;
				}
			}
			num++;
		}
	}
}
