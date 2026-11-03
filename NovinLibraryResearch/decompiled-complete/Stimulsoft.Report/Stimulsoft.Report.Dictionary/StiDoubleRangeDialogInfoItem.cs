using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Dictionary;

public class StiDoubleRangeDialogInfoItem : StiRangeDialogInfoItem
{
	public override StiComponentId ComponentId => StiComponentId.StiDoubleRangeDialogInfoItem;

	[StiOrder(100)]
	[DefaultValue(0.0)]
	[Description("Gets or sets value which will be used as From key of range item in GUI.")]
	public double From
	{
		get
		{
			return (double)base.KeyObject;
		}
		set
		{
			base.KeyObject = value;
		}
	}

	[StiOrder(200)]
	[DefaultValue(0.0)]
	[Description("Gets or sets value which will be used as To key of range item in GUI.")]
	public double To
	{
		get
		{
			return (double)base.KeyObjectTo;
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
			propertiesHelper.RangeDoubleFromDialogInfo(),
			propertiesHelper.RangeDoubleToDialogInfo(),
			propertiesHelper.DialogInfoValue()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Misc, value);
		return stiPropertyCollection;
	}

	public StiDoubleRangeDialogInfoItem()
	{
		base.KeyObject = 0.0;
		base.KeyObjectTo = 0.0;
	}
}
