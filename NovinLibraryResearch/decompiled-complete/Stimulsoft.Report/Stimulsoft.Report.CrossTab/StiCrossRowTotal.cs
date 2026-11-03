using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.CrossTab;

public class StiCrossRowTotal : StiCrossTotal
{
	public override StiComponentId ComponentId => StiComponentId.StiCrossRowTotal;

	public override string LocalizedName => StiLocalization.Get("Components", "StiCrossRowTotal");

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Invalid comparison between Unknown and I4
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.Text() };
		stiPropertyCollection.Add(StiPropertyCategories.Text, value);
		value = (((int)level == 0) ? new StiPropertyObject[4]
		{
			propertiesHelper.TextBrush(),
			propertiesHelper.fAngle(),
			propertiesHelper.Font(),
			propertiesHelper.WordWrap()
		} : (((int)level != 1) ? new StiPropertyObject[7]
		{
			propertiesHelper.AllowHtmlTags(),
			propertiesHelper.TextBrush(),
			propertiesHelper.fAngle(),
			propertiesHelper.Font(),
			propertiesHelper.Margins(),
			propertiesHelper.TextOptions(),
			propertiesHelper.WordWrap()
		} : new StiPropertyObject[6]
		{
			propertiesHelper.AllowHtmlTags(),
			propertiesHelper.TextBrush(),
			propertiesHelper.fAngle(),
			propertiesHelper.Font(),
			propertiesHelper.Margins(),
			propertiesHelper.WordWrap()
		}));
		stiPropertyCollection.Add(StiPropertyCategories.TextAdditional, value);
		if ((int)level != 0)
		{
			value = new StiPropertyObject[2]
			{
				propertiesHelper.MinSize(),
				propertiesHelper.MaxSize()
			};
			stiPropertyCollection.Add(StiPropertyCategories.Position, value);
		}
		value = (((int)level != 0) ? new StiPropertyObject[7]
		{
			propertiesHelper.Brush(),
			propertiesHelper.Border(),
			propertiesHelper.Conditions(),
			propertiesHelper.ComponentStyle(),
			propertiesHelper.HorAlignment(),
			propertiesHelper.VertAlignment(),
			propertiesHelper.UseParentStyles()
		} : new StiPropertyObject[6]
		{
			propertiesHelper.Brush(),
			propertiesHelper.Border(),
			propertiesHelper.Conditions(),
			propertiesHelper.ComponentStyle(),
			propertiesHelper.HorAlignment(),
			propertiesHelper.VertAlignment()
		});
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		value = (((int)level != 0) ? new StiPropertyObject[3]
		{
			propertiesHelper.InteractionEditor(),
			propertiesHelper.MergeHeaders(),
			propertiesHelper.Enabled()
		} : new StiPropertyObject[2]
		{
			propertiesHelper.MergeHeaders(),
			propertiesHelper.Enabled()
		});
		stiPropertyCollection.Add(StiPropertyCategories.Behavior, value);
		return stiPropertyCollection;
	}

	public override StiComponent CreateNew()
	{
		return new StiCrossRowTotal();
	}

	public StiCrossRowTotal()
	{
		Text = "Total";
	}
}
