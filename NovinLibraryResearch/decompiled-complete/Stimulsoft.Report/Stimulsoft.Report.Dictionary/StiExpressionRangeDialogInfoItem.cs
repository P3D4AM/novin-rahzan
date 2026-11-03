using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Dictionary;

public class StiExpressionRangeDialogInfoItem : StiRangeDialogInfoItem
{
	public override StiComponentId ComponentId => StiComponentId.StiExpressionRangeDialogInfoItem;

	[StiOrder(100)]
	[Description("Gets or sets expression which will be used as From key of range item in GUI.")]
	public string From
	{
		get
		{
			return (string)base.KeyObject;
		}
		set
		{
			base.KeyObject = value;
		}
	}

	[StiOrder(200)]
	[Description("Gets or sets expression which will be used as To key of range item in GUI.")]
	public string To
	{
		get
		{
			return (string)base.KeyObjectTo;
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
			propertiesHelper.RangeExpressionFromDialogInfo(),
			propertiesHelper.RangeExpressionToDialogInfo(),
			propertiesHelper.DialogInfoValue()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Misc, value);
		return stiPropertyCollection;
	}

	public StiExpressionRangeDialogInfoItem()
	{
		base.KeyObject = string.Empty;
		base.KeyObjectTo = string.Empty;
	}
}
