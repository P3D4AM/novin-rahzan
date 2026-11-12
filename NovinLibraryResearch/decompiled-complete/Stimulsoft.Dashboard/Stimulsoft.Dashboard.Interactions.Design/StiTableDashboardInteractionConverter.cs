using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Interactions.Design;

public class StiTableDashboardInteractionConverter : StiDashboardInteractionConverter
{
	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (destinationType == typeof(InstanceDescriptor) && value != null)
		{
			StiTableDashboardInteraction stiTableDashboardInteraction = value as StiTableDashboardInteraction;
			Type[] types = new Type[18]
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
				typeof(bool),
				typeof(string),
				typeof(string),
				typeof(string),
				typeof(bool),
				typeof(bool),
				typeof(bool),
				typeof(bool),
				typeof(bool)
			};
			object[] objs = new object[18]
			{
				stiTableDashboardInteraction.OnHover, stiTableDashboardInteraction.OnClick, stiTableDashboardInteraction.HyperlinkDestination, stiTableDashboardInteraction.ToolTip, stiTableDashboardInteraction.Hyperlink, stiTableDashboardInteraction.DrillDownPageKey, stiTableDashboardInteraction.DrillDownParameters, stiTableDashboardInteraction.AllowUserFiltering, stiTableDashboardInteraction.AllowUserSorting, stiTableDashboardInteraction.AllowUserColumnSelection,
				stiTableDashboardInteraction.FileName, stiTableDashboardInteraction.HeaderText, stiTableDashboardInteraction.FooterText, stiTableDashboardInteraction.ShowFullScreenButton, stiTableDashboardInteraction.ShowSaveButton, stiTableDashboardInteraction.ShowViewDataButton, stiTableDashboardInteraction.DrillDownFiltered, stiTableDashboardInteraction.FullRowSelect
			};
			ConstructorInfo constructor = typeof(StiTableDashboardInteraction).GetConstructor(types);
			if (constructor != null)
			{
				return CreateNewInstanceDescriptor(constructor, objs);
			}
		}
		return base.ConvertTo(context, culture, value, destinationType);
	}
}
