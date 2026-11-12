using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace DevComponents.DotNetBar.Design;

public class MicroChartItemDesigner : BaseItemDesigner
{
	public override void Initialize(IComponent component)
	{
		base.Initialize(component);
		if (component != null && component.Site != null && component.Site.DesignMode)
		{
			SetDataPointsDefaults();
		}
	}

	private void SetDataPointsDefaults()
	{
		MicroChartItem microChartItem = (MicroChartItem)((ComponentDesigner)this).Component;
		microChartItem.DataPoints = GetRandomDataPoints();
	}

	public override void InitializeExistingComponent(IDictionary defaultValues)
	{
		SetDataPointsDefaults();
		((ComponentDesigner)this).InitializeExistingComponent(defaultValues);
	}

	protected override void SetDesignTimeDefaults()
	{
		SetDataPointsDefaults();
		base.SetDesignTimeDefaults();
	}

	internal static List<double> GetRandomDataPoints(bool allowNegative)
	{
		return GetRandomDataPoints(allowNegative, 12);
	}

	internal static List<double> GetRandomDataPoints(bool allowNegative, int pointsToCreate)
	{
		List<double> list = new List<double>();
		Random random = new Random((int)DateTime.Now.Ticks);
		Random random2 = new Random();
		for (int i = 0; i < pointsToCreate; i++)
		{
			list.Add(allowNegative ? (((random2.Next(50) > 25) ? 1 : (-1)) * random.Next(100)) : random.Next(100));
		}
		return list;
	}

	internal static List<double> GetRandomDataPoints()
	{
		return GetRandomDataPoints(allowNegative: true);
	}
}
