using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Interactions.Design;

public class StiIndicatorDashboardInteractionConverter : StiDashboardInteractionConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiIndicatorDashboardInteraction stiIndicatorDashboardInteraction = value as StiIndicatorDashboardInteraction;
			Type[] types = new Type[15]
			{
				typeof(StiInteractionOnHover),
				typeof(StiInteractionOnClick),
				typeof(StiInteractionOpenHyperlinkDestination),
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(List<StiDashboardDrillDownParameter>),
				typeof(bool),
				typeof(bool),
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(bool),
				typeof(bool),
				typeof(bool)
			};
			object[] objs = new object[15]
			{
				stiIndicatorDashboardInteraction.OnHover, stiIndicatorDashboardInteraction.OnClick, stiIndicatorDashboardInteraction.HyperlinkDestination, stiIndicatorDashboardInteraction.ToolTip, stiIndicatorDashboardInteraction.Hyperlink, stiIndicatorDashboardInteraction.DrillDownPageKey, stiIndicatorDashboardInteraction.DrillDownParameters, stiIndicatorDashboardInteraction.AllowUserDrillDown, stiIndicatorDashboardInteraction.AllowUserSorting, stiIndicatorDashboardInteraction.FileName,
				stiIndicatorDashboardInteraction.HeaderText, stiIndicatorDashboardInteraction.FooterText, stiIndicatorDashboardInteraction.ShowFullScreenButton, stiIndicatorDashboardInteraction.ShowSaveButton, stiIndicatorDashboardInteraction.ShowViewDataButton
			};
			ConstructorInfo constructor = typeof(StiIndicatorDashboardInteraction).GetConstructor(types);
			if (constructor != null)
			{
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
