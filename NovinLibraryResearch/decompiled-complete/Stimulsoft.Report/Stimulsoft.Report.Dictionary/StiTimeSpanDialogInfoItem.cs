using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Dictionary;

public class StiTimeSpanDialogInfoItem : StiDialogInfoItem
{
	public override StiComponentId ComponentId => StiComponentId.StiTimeSpanDialogInfoItem;

	[StiOrder(100)]
	[Description("Gets or sets value which will be used as key of item in GUI.")]
	public TimeSpan Key
	{
		get
		{
			return (TimeSpan)base.KeyObject;
		}
		set
		{
			base.KeyObject = value;
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[2]
		{
			propertiesHelper.TimeDialogInfoKey(),
			propertiesHelper.DialogInfoValue()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Misc, value);
		return stiPropertyCollection;
	}

	public StiTimeSpanDialogInfoItem()
	{
		base.KeyObject = TimeSpan.Zero;
	}
}
