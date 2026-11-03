using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Dictionary;

public class StiByteArrayRangeDialogInfoItem : StiRangeDialogInfoItem
{
	public override StiComponentId ComponentId => StiComponentId.StiByteArrayRangeDialogInfoItem;

	[StiOrder(100)]
	[Description("Gets or sets value which will be used as From key of range item in GUI.")]
	[Browsable(false)]
	public byte[] From
	{
		get
		{
			return (byte[])base.KeyObject;
		}
		set
		{
			base.KeyObject = value;
		}
	}

	[StiOrder(200)]
	[Description("Gets or sets value which will be used as To key of range item in GUI.")]
	[Browsable(false)]
	public byte[] To
	{
		get
		{
			return (byte[])base.KeyObjectTo;
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
			propertiesHelper.RangeGuidFromDialogInfo(),
			propertiesHelper.RangeGuidToDialogInfo(),
			propertiesHelper.DialogInfoValue()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Misc, value);
		return stiPropertyCollection;
	}
}
