using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;

namespace Stimulsoft.Base.Design;

[ParenthesizePropertyName(true)]
[TypeConverter(typeof(StiDataBindingsCollectionConverter))]
[Editor("Stimulsoft.Base.Design.StiDataBindingsCollectionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
public class StiDataBindingsCollection : CollectionBase
{
	public object Control { get; }

	public void Add(StiDataBinding binding)
	{
		base.List.Add(binding);
	}

	public void Add(string propertyName, string displayName)
	{
		base.List.Add(new StiDataBinding(this, propertyName, displayName));
	}

	public void Remove(StiDataBinding binding)
	{
		base.List.Remove(binding);
	}

	public StiDataBindingsCollection()
		: this(null)
	{
	}

	public StiDataBindingsCollection(object control)
	{
		Control = control;
	}
}
