using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Dictionary;

public class StiLongRangeDialogInfoItem : StiRangeDialogInfoItem
{
	public override StiComponentId ComponentId => StiComponentId.StiLongRangeDialogInfoItem;

	[StiOrder(100)]
	[DefaultValue(0)]
	[Description("Gets or sets value which will be used as From key of range item in GUI.")]
	public long From
	{
		get
		{
			return (long)base.KeyObject;
		}
		set
		{
			base.KeyObject = value;
		}
	}

	[StiOrder(200)]
	[DefaultValue(0)]
	[Description("Gets or sets value which will be used as To key of range item in GUI.")]
	public long To
	{
		get
		{
			return (long)base.KeyObjectTo;
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
			propertiesHelper.RangeLongFromDialogInfo(),
			propertiesHelper.RangeLongToDialogInfo(),
			propertiesHelper.DialogInfoValue()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Misc, value);
		return stiPropertyCollection;
	}

	public StiLongRangeDialogInfoItem()
	{
		base.KeyObject = 0L;
		base.KeyObjectTo = 0L;
	}
}
