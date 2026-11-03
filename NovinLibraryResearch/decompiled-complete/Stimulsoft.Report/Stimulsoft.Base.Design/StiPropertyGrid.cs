using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Windows.Forms.PropertyGridInternal;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Base.Design;

[ToolboxItem(false)]
public class StiPropertyGrid : PropertyGrid
{
	private ToolStripItem itemCategorized;

	private ToolStripItem itemAlphabetical;

	private static Hashtable hidedProperties = new Hashtable();

	private ToolStrip toolStrip;

	public int ScrollOffset
	{
		get
		{
			FieldInfo field = typeof(PropertyGrid).GetField("gridView", BindingFlags.Instance | BindingFlags.NonPublic);
			if (field == null)
			{
				return 0;
			}
			object value = field.GetValue(this);
			if (value == null)
			{
				return 0;
			}
			MethodInfo method = value.GetType().GetMethod("GetScrollOffset", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod);
			if (method == null)
			{
				return 0;
			}
			object obj = method.Invoke(value, new object[0]);
			if (!(obj is int))
			{
				return 0;
			}
			return (int)obj;
		}
		set
		{
			FieldInfo field = typeof(PropertyGrid).GetField("gridView", BindingFlags.Instance | BindingFlags.NonPublic);
			if (!(field == null))
			{
				object value2 = field.GetValue(this);
				value2?.GetType().GetMethod("SetScrollOffset", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod)?.Invoke(value2, new object[1] { value });
			}
		}
	}

	public override bool HelpVisible
	{
		get
		{
			return StiPropertyGridOptions.ShowDescription;
		}
		set
		{
			((PropertyGrid)this).HelpVisible = StiPropertyGridOptions.ShowDescription;
		}
	}

	public virtual void Localize()
	{
		if (itemCategorized != null)
		{
			itemCategorized.ToolTipText = StiLocalization.Get("Report", "Categorized");
		}
		if (itemAlphabetical != null)
		{
			itemAlphabetical.ToolTipText = StiLocalization.Get("Report", "Alphabetical");
		}
	}

	private static string GetPropertyKey(Type type, string propertyName)
	{
		return GetPropertyKey(type.ToString(), propertyName);
	}

	private static string GetPropertyKey(string type, string propertyName)
	{
		return type + "::" + propertyName;
	}

	public static void HideProperty(Type type, string propertyName)
	{
		HideProperty(type.ToString(), propertyName);
	}

	public static void HideProperty(string type, string propertyName)
	{
		string propertyKey = GetPropertyKey(type, propertyName);
		hidedProperties[propertyKey] = propertyKey;
	}

	public static void ShowProperty(Type type, string propertyName)
	{
		ShowProperty(type.ToString(), propertyName);
	}

	public static void ShowProperty(string type, string propertyName)
	{
		string propertyKey = GetPropertyKey(type, propertyName);
		if (hidedProperties[propertyKey] != null)
		{
			hidedProperties.Remove(propertyKey);
		}
	}

	public static bool IsAllowedProperty(Type type, string propertyName)
	{
		if (type == typeof(Font))
		{
			if (!(propertyName == "GdiCharSet") && !(propertyName == "GdiVerticalFont"))
			{
				return !(propertyName == "Unit");
			}
			return false;
		}
		string propertyKey = GetPropertyKey(type, propertyName);
		string propertyKey2 = GetPropertyKey("All", propertyName);
		if (hidedProperties[propertyKey] == null)
		{
			return hidedProperties[propertyKey2] == null;
		}
		return false;
	}

	public static bool IsAllowedProperty(string propertyName)
	{
		string propertyKey = GetPropertyKey("All", propertyName);
		return hidedProperties[propertyKey] == null;
	}

	public virtual void SetStandardPropertyTabs()
	{
		((PropertyGrid)this).PropertyTabs.RemoveTabType(typeof(PropertiesTab));
		((PropertyGrid)this).PropertyTabs.AddTabType(typeof(StiPropertiesTab), PropertyTabScope.Document);
		SetPropertyGrid();
	}

	public virtual void SetServicesPropertyTabs()
	{
		((PropertyGrid)this).PropertyTabs.RemoveTabType(typeof(PropertiesTab));
		((PropertyGrid)this).PropertyTabs.AddTabType(typeof(StiServicesTab), PropertyTabScope.Document);
		SetPropertyGrid();
	}

	private void SetPropertyGrid()
	{
		foreach (object propertyTab in ((PropertyGrid)this).PropertyTabs)
		{
			if (propertyTab is StiPropertiesTab stiPropertiesTab)
			{
				stiPropertiesTab.PropertyGrid = (PropertyGrid)(object)this;
			}
		}
	}

	protected override void OnLayout(LayoutEventArgs e)
	{
		((ContainerControl)this).OnLayout(e);
		if (toolStrip == null)
		{
			return;
		}
		string stepName = StiScale.StepName;
		ToolStrip obj = toolStrip;
		IEnumerable<ToolStripItem> source = ((obj != null) ? ((IEnumerable)obj.Items).Cast<ToolStripItem>() : null);
		itemCategorized = source.FirstOrDefault((Func<ToolStripItem, bool>)delegate(ToolStripItem i)
		{
			object obj7;
			if (i == null)
			{
				obj7 = null;
			}
			else
			{
				AccessibleObject accessibilityObject6 = i.AccessibilityObject;
				obj7 = ((accessibilityObject6 != null) ? accessibilityObject6.Name : null);
			}
			return (string?)obj7 == "Categorized";
		});
		if (itemCategorized != null)
		{
			itemCategorized.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Office2013.Categorized" + stepName + ".png");
		}
		itemAlphabetical = source.FirstOrDefault((Func<ToolStripItem, bool>)delegate(ToolStripItem i)
		{
			object obj6;
			if (i == null)
			{
				obj6 = null;
			}
			else
			{
				AccessibleObject accessibilityObject5 = i.AccessibilityObject;
				obj6 = ((accessibilityObject5 != null) ? accessibilityObject5.Name : null);
			}
			return (string?)obj6 == "Alphabetical";
		});
		if (itemAlphabetical != null)
		{
			itemAlphabetical.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Office2013.Alphabetical" + stepName + ".png");
		}
		ToolStripItem val = source.FirstOrDefault((Func<ToolStripItem, bool>)delegate(ToolStripItem i)
		{
			object obj4;
			if (i == null)
			{
				obj4 = null;
			}
			else
			{
				AccessibleObject accessibilityObject3 = i.AccessibilityObject;
				obj4 = ((accessibilityObject3 != null) ? accessibilityObject3.Name : null);
			}
			if (!((string?)obj4 == "Property Pages"))
			{
				object obj5;
				if (i == null)
				{
					obj5 = null;
				}
				else
				{
					AccessibleObject accessibilityObject4 = i.AccessibilityObject;
					obj5 = ((accessibilityObject4 != null) ? accessibilityObject4.Name : null);
				}
				return (string?)obj5 == Loc.Get("Report", "PropertiesTab");
			}
			return true;
		});
		if (val != null)
		{
			val.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Office2013.Properties" + stepName + ".png");
		}
		val = source.FirstOrDefault((Func<ToolStripItem, bool>)delegate(ToolStripItem i)
		{
			object obj2;
			if (i == null)
			{
				obj2 = null;
			}
			else
			{
				AccessibleObject accessibilityObject = i.AccessibilityObject;
				obj2 = ((accessibilityObject != null) ? accessibilityObject.Name : null);
			}
			if (!((string?)obj2 == "Event Pages"))
			{
				object obj3;
				if (i == null)
				{
					obj3 = null;
				}
				else
				{
					AccessibleObject accessibilityObject2 = i.AccessibilityObject;
					obj3 = ((accessibilityObject2 != null) ? accessibilityObject2.Name : null);
				}
				return (string?)obj3 == Loc.Get("Report", "EventsTab");
			}
			return true;
		});
		if (val != null)
		{
			val.Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Office2013.Events" + stepName + ".png");
		}
	}

	private void PropertyGridView_FontChanged(object sender, EventArgs e)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		foreach (Control item in (ArrangedElementCollection)((PropertyGrid)this).Controls)
		{
			Control val = item;
			Type type = ((object)val).GetType();
			if (!(type.Name != "PropertyGridView"))
			{
				type.GetField("cachedRowHeight", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(val, val.Font.Height + StiScale.I2);
				if (StiScale.X != 1.0)
				{
					type.GetField("outlineSizeExplorerTreeStyle", BindingFlags.Static | BindingFlags.NonPublic)?.SetValue(val, val.Font.Height);
				}
			}
		}
	}

	private void DropDownListBox_DrawItem(object sender, DrawItemEventArgs e)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Invalid comparison between Unknown and I4
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		ListBox val = (ListBox)((sender is ListBox) ? sender : null);
		Graphics graphics = e.Graphics;
		Rectangle bounds = e.Bounds;
		DrawItemState state = e.State;
		bounds.Height++;
		if ((state & 1) > 0)
		{
			bounds.Width--;
		}
		if (e.Index != -1 && val.Items.Count > 0)
		{
			StiControlPaint.DrawItem(graphics, bounds, state, ((ListControl)val).GetItemText(val.Items[e.Index]), (ImageList)null, 0, ((Control)val).Font, ((Control)val).BackColor, ((Control)val).ForeColor, ((Control)val).RightToLeft);
		}
	}

	public StiPropertyGrid()
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Expected O, but got Unknown
		((PropertyGrid)this).LineColor = Color.FromArgb(198, 198, 198);
		((PropertyGrid)this).HelpBackColor = Color.White;
		foreach (Control item in (ArrangedElementCollection)((PropertyGrid)this).Controls)
		{
			Control val = item;
			try
			{
				Type type = ((object)val).GetType();
				if (type.Name == "PropertyGridView")
				{
					val.FontChanged += PropertyGridView_FontChanged;
					val.Font = new Font(val.Font.FontFamily, val.Font.Size, val.Font.Style);
					type.GetField("paintWidth", BindingFlags.Static | BindingFlags.NonPublic)?.SetValue(val, StiScale.XXI(20.0));
					type.GetField("paintIndent", BindingFlags.Static | BindingFlags.NonPublic)?.SetValue(val, StiScale.XXI(26.0));
					object? obj = type.GetProperty("DropDownListBox", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(val, null);
					ListBox val2 = (ListBox)((obj is ListBox) ? obj : null);
					if (val2 != null)
					{
						val2.DrawMode = (DrawMode)2;
						val2.DrawItem += new DrawItemEventHandler(DropDownListBox_DrawItem);
					}
				}
				if (type.Name == "ToolStrip")
				{
					int num = StiScale.XXI(16.0);
					typeof(PropertyGrid).GetField("normalButtonSize", BindingFlags.Static | BindingFlags.NonPublic)?.SetValue(this, new Size(num, num));
					toolStrip = (ToolStrip)(object)((val is ToolStrip) ? val : null);
					toolStrip.ImageScalingSize = new Size(num, num);
					toolStrip.GripMargin = new Padding(StiScale.I2);
					typeof(PropertyGrid).GetMethod("SetupToolbar", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[1] { typeof(bool) }, null)?.Invoke(this, new object[1] { true });
				}
			}
			catch
			{
			}
		}
	}
}
