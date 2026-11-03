using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Dictionary;

public class StiDateTimeRangeDialogInfoItem : StiRangeDialogInfoItem
{
	public override StiComponentId ComponentId => StiComponentId.StiDateTimeRangeDialogInfoItem;

	[StiOrder(100)]
	[Description("Gets or sets value which will be used as From key of range item in GUI.")]
	public DateTime From
	{
		get
		{
			return (DateTime)base.KeyObject;
		}
		set
		{
			base.KeyObject = value;
		}
	}

	[StiOrder(200)]
	[Description("Gets or sets value which will be used as To key of range item in GUI.")]
	public DateTime To
	{
		get
		{
			return (DateTime)base.KeyObjectTo;
		}
		set
		{
			base.KeyObjectTo = value;
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[3]
		{
			propertiesHelper.RangeDateFromDialogInfo(),
			propertiesHelper.RangeDateToDialogInfo(),
			propertiesHelper.DialogInfoValue()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Misc, value);
		return stiPropertyCollection;
	}

	public StiDateTimeRangeDialogInfoItem()
	{
		base.KeyObject = DateTime.Now;
		base.KeyObjectTo = DateTime.Now;
	}
}
