using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.CrossTab;

public class StiCrossColumnTotal : StiCrossTotal
{
	public override StiComponentId ComponentId => StiComponentId.StiCrossColumnTotal;

	public override string LocalizedName => StiLocalization.Get("Components", "StiCrossColumnTotal");

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Invalid comparison between Unknown and I4
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		stiPropertyCollection.Add(StiPropertyCategories.Text, new StiPropertyObject[1] { propertiesHelper.Text() });
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.TextAdditional, new StiPropertyObject[4]
			{
				propertiesHelper.TextBrush(),
				propertiesHelper.fAngle(),
				propertiesHelper.Font(),
				propertiesHelper.WordWrap()
			});
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.TextAdditional, new StiPropertyObject[6]
			{
				propertiesHelper.AllowHtmlTags(),
				propertiesHelper.TextBrush(),
				propertiesHelper.fAngle(),
				propertiesHelper.Font(),
				propertiesHelper.Margins(),
				propertiesHelper.WordWrap()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.TextAdditional, new StiPropertyObject[7]
			{
				propertiesHelper.AllowHtmlTags(),
				propertiesHelper.TextBrush(),
				propertiesHelper.fAngle(),
				propertiesHelper.Font(),
				propertiesHelper.Margins(),
				propertiesHelper.TextOptions(),
				propertiesHelper.WordWrap()
			});
		}
		if ((int)level != 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Position, new StiPropertyObject[2]
			{
				propertiesHelper.MinSize(),
				propertiesHelper.MaxSize()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[6]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Border(),
				propertiesHelper.Conditions(),
				propertiesHelper.ComponentStyle(),
				propertiesHelper.HorAlignment(),
				propertiesHelper.VertAlignment()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[7]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Border(),
				propertiesHelper.Conditions(),
				propertiesHelper.ComponentStyle(),
				propertiesHelper.HorAlignment(),
				propertiesHelper.VertAlignment(),
				propertiesHelper.UseParentStyles()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[2]
			{
				propertiesHelper.MergeHeaders(),
				propertiesHelper.Enabled()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[3]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.MergeHeaders(),
				propertiesHelper.Enabled()
			});
		}
		return stiPropertyCollection;
	}

	public override StiComponent CreateNew()
	{
		return new StiCrossColumnTotal();
	}

	public StiCrossColumnTotal()
	{
		Text = "Total";
	}
}
