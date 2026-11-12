using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Controls;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.Design;

public class ComboBoxExDesigner : ControlDesigner
{
	private DesignerActionListCollection m_ActionLists;

	public override DesignerActionListCollection ActionLists
	{
		get
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			if (m_ActionLists == null)
			{
				m_ActionLists = new DesignerActionListCollection();
				object obj = ((object)this).GetType().Assembly.CreateInstance("System.Windows.Forms.Design.ListControlBoundActionList", ignoreCase: false, BindingFlags.NonPublic, null, new object[1] { this }, null, null);
				if (obj != null)
				{
					m_ActionLists.Add((DesignerActionList)((obj is DesignerActionList) ? obj : null));
				}
			}
			return m_ActionLists;
		}
	}

	public override SelectionRules SelectionRules
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Invalid comparison between Unknown and I4
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Invalid comparison between Unknown and I4
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			SelectionRules selectionRules = ((ControlDesigner)this).SelectionRules;
			ComboBoxEx component = ((ControlDesigner)this).Control as ComboBoxEx;
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(component)["DropDownStyle"];
			if (propertyDescriptor == null)
			{
				return selectionRules;
			}
			ComboBoxStyle val = (ComboBoxStyle)propertyDescriptor.GetValue(component);
			if ((int)val != 1 && (int)val != 2)
			{
				return selectionRules;
			}
			return (SelectionRules)(selectionRules & -4);
		}
	}

	[Description("Gets or sets the combo box flat style")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Browsable(true)]
	public FlatStyle FlatStyle
	{
		get
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			Control control = ((ControlDesigner)this).Control;
			ComboBox val = (ComboBox)(object)((control is ComboBox) ? control : null);
			return val.FlatStyle;
		}
		set
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			Control control = ((ControlDesigner)this).Control;
			ComboBox val = (ComboBox)(object)((control is ComboBox) ? control : null);
			val.FlatStyle = value;
		}
	}

	public override ICollection AssociatedComponents
	{
		get
		{
			ArrayList arrayList = new ArrayList(((ControlDesigner)this).AssociatedComponents);
			ComboBoxEx comboBoxEx = ((ComponentDesigner)this).Component as ComboBoxEx;
			foreach (object item in comboBoxEx.Items)
			{
				if (item is ComboItem)
				{
					arrayList.Add(item);
				}
			}
			return arrayList;
		}
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		((ControlDesigner)this).InitializeNewComponent(defaultValues);
		SetDesignTimeDefaults();
	}

	public override void Initialize(IComponent component)
	{
		((ControlDesigner)this).Initialize(component);
		if (component.Site == null || component.Site.DesignMode)
		{
			IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentRemoving += OnComponentRemoved;
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
		if (componentChangeService != null)
		{
			componentChangeService.ComponentRemoved -= OnComponentRemoved;
		}
		((ControlDesigner)this).Dispose(disposing);
	}

	private void SetDesignTimeDefaults()
	{
		ComboBoxEx comboBoxEx = ((ControlDesigner)this).Control as ComboBoxEx;
		((ListControl)comboBoxEx).DisplayMember = "Text";
		((ComboBox)comboBoxEx).DrawMode = (DrawMode)1;
		((ComboBox)comboBoxEx).ItemHeight = comboBoxEx.GetFontHeight() + 1;
		comboBoxEx.Style = eDotNetBarStyle.StyleManagerControlled;
		((ListControl)comboBoxEx).FormattingEnabled = true;
		PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(comboBoxEx)["Text"];
		if (propertyDescriptor != null && (object)propertyDescriptor.PropertyType == typeof(string) && !propertyDescriptor.IsReadOnly && propertyDescriptor.IsBrowsable)
		{
			propertyDescriptor.SetValue(comboBoxEx, "");
		}
	}

	protected override void PreFilterProperties(IDictionary properties)
	{
		((ControlDesigner)this).PreFilterProperties(properties);
		properties["FlatStyle"] = TypeDescriptor.CreateProperty(((object)this).GetType(), "FlatStyle", typeof(FlatStyle), new BrowsableAttribute(browsable: true), new DefaultValueAttribute((object)(FlatStyle)0));
	}

	private void OnComponentRemoved(object sender, ComponentEventArgs e)
	{
		if (e.Component != ((ComponentDesigner)this).Component)
		{
			return;
		}
		ComboBoxEx comboBoxEx = ((ComponentDesigner)this).Component as ComboBoxEx;
		if (!(((ComponentDesigner)this).GetService(typeof(IDesignerHost)) is IDesignerHost designerHost))
		{
			return;
		}
		foreach (object item in comboBoxEx.Items)
		{
			if (item is ComboItem)
			{
				ComboItem component = item as ComboItem;
				designerHost.DestroyComponent(component);
			}
		}
	}
}
