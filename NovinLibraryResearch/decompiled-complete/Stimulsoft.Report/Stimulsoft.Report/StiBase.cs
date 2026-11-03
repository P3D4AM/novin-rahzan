using System.ComponentModel;
using System.Drawing.Design;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;

namespace Stimulsoft.Report;

public abstract class StiBase : StiService, IStiName
{
	private string name = "";

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[StiSerializable]
	[ParenthesizePropertyName(true)]
	[Description("Indicates the name which is used to identify the component.")]
	[Editor("Stimulsoft.Report.Components.Design.StiSimpleTextEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual string Name
	{
		get
		{
			return name;
		}
		set
		{
			name = string.Intern(value);
		}
	}

	[Browsable(false)]
	public virtual string LocalizedName => Name;

	[Browsable(false)]
	public override string ServiceName => LocalizedName;

	[Browsable(false)]
	public virtual string LocalizedCategory => Name;
}
