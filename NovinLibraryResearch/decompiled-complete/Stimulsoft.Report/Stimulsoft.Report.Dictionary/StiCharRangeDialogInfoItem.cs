using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Dictionary;

public class StiCharRangeDialogInfoItem : StiRangeDialogInfoItem
{
	public override StiComponentId ComponentId => StiComponentId.StiCharRangeDialogInfoItem;

	[StiOrder(100)]
	[Description("Gets or sets value which will be used as From key of range item in GUI.")]
	public char From
	{
		get
		{
			return (char)base.KeyObject;
		}
		set
		{
			base.KeyObject = value;
		}
	}

	[StiOrder(200)]
	[Description("Gets or sets value which will be used as To key of range item in GUI.")]
	public char To
	{
		get
		{
			return (char)base.KeyObjectTo;
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
			propertiesHelper.RangeCharFromDialogInfo(),
			propertiesHelper.RangeCharToDialogInfo(),
			propertiesHelper.DialogInfoValue()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Misc, value);
		return stiPropertyCollection;
	}

	public StiCharRangeDialogInfoItem()
	{
		base.KeyObject = 'A';
		base.KeyObjectTo = 'Z';
	}
}
