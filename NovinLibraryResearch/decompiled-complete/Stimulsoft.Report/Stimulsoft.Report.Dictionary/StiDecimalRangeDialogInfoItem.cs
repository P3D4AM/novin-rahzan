using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Dictionary;

public class StiDecimalRangeDialogInfoItem : StiRangeDialogInfoItem
{
	public override StiComponentId ComponentId => StiComponentId.StiDecimalRangeDialogInfoItem;

	[StiOrder(100)]
	[Description("Gets or sets value which will be used as From key of range item in GUI.")]
	public decimal From
	{
		get
		{
			return (decimal)base.KeyObject;
		}
		set
		{
			base.KeyObject = value;
		}
	}

	[StiOrder(200)]
	[Description("Gets or sets value which will be used as To key of range item in GUI.")]
	public decimal To
	{
		get
		{
			return (decimal)base.KeyObjectTo;
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
			propertiesHelper.RangeDecimalFromDialogInfo(),
			propertiesHelper.RangeDecimalToDialogInfo(),
			propertiesHelper.DialogInfoValue()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Misc, value);
		return stiPropertyCollection;
	}

	public StiDecimalRangeDialogInfoItem()
	{
		base.KeyObject = 0m;
		base.KeyObjectTo = 0m;
	}
}
