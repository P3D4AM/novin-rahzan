using System.ComponentModel;

namespace DevComponents.AdvTree;

public class HeaderDefinition
{
	private string Ӿ = "";

	private ColumnHeaderCollection Չ = new ColumnHeaderCollection();

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Columns")]
	[Browsable(true)]
	[Description("Gets the reference to the collection that contains the columns associated with header.")]
	public ColumnHeaderCollection Columns => Չ;

	[Browsable(true)]
	[Description("Indicates name associated with this header definition.")]
	[Category("Design")]
	[DefaultValue("")]
	public string Name
	{
		get
		{
			return Ӿ;
		}
		set
		{
			Ӿ = value;
		}
	}
}
