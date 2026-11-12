using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;

namespace DevComponents.DotNetBar.Design;

public class ComponentDesignerWithAction : ComponentDesigner
{
	private class NoBorderRenderer : ToolStripProfessionalRenderer
	{
		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
		}
	}

	private class TestBehavior : Behavior
	{
	}

	private class TestGlyph : Glyph
	{
		private ComponentDesignerWithAction _Item;

		private BehaviorService _BehaviorSvc;

		public ComponentDesignerWithAction Item => _Item;

		public override Rectangle Bounds
		{
			get
			{
				Rectangle result = Rectangle.Empty;
				if (_Item != null && _Item.GetHostControl() != null)
				{
					Point pos = _BehaviorSvc.ControlToAdornerWindow(_Item.GetHostControl());
					result = _Item.ComponentBounds;
					result.Offset(pos);
				}
				return result;
			}
		}

		public Rectangle CompleteBounds
		{
			get
			{
				Rectangle result = Rectangle.Empty;
				if (_Item != null && _Item.GetHostControl() != null)
				{
					Point pos = _BehaviorSvc.ControlToAdornerWindow(_Item.GetContainerControl());
					result = _Item.ComponentBounds;
					result.Offset(pos);
				}
				return result;
			}
		}

		public TestGlyph(BehaviorService behaviorSvc, ComponentDesignerWithAction item)
			: base((Behavior)(object)new TestBehavior())
		{
			_BehaviorSvc = behaviorSvc;
			_Item = item;
		}

		public override Cursor GetHitTest(Point p)
		{
			if (_Item != null && !_Item.IsSelected)
			{
				return null;
			}
			if (((Glyph)this).Bounds.Contains(p))
			{
				return Cursors.SizeAll;
			}
			return null;
		}

		public override void Paint(PaintEventArgs pe)
		{
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Expected O, but got Unknown
			if (_Item != null && _Item.IsSelected)
			{
				Rectangle completeBounds = CompleteBounds;
				completeBounds.Width--;
				completeBounds.Height--;
				Graphics graphics = pe.Graphics;
				SolidBrush val = new SolidBrush(Color.FromArgb(129, Color.Red));
				try
				{
					graphics.FillRectangle((Brush)(object)val, completeBounds);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
		}
	}

	private Adorner _DesignerActionAdorner;

	public bool IsSelected;

	private Glyph _DesignerGlyph;

	private ToolStripDropDown _ActionListToolStrip;

	private bool _SettingBounds;

	protected virtual Rectangle ComponentBounds
	{
		get
		{
			if (((ComponentDesigner)this).Component is BaseItem baseItem)
			{
				return baseItem.Bounds;
			}
			return Rectangle.Empty;
		}
	}

	protected virtual bool DesignerActionUIEnabled => true;

	public override void Initialize(IComponent component)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		((ComponentDesigner)this).Initialize(component);
		if (DesignerActionUIEnabled)
		{
			ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
			if (selectionService != null)
			{
				selectionService.SelectionChanged += SelectionChanged;
			}
			_DesignerActionAdorner = new Adorner();
			IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentRemoving += ComponentRemoving;
				componentChangeService.ComponentRemoved += ComponentRemoved;
			}
		}
	}

	private void ComponentRemoved(object sender, ComponentEventArgs e)
	{
		if (e.Component is BaseItem)
		{
			BaseItem baseItem = ((ComponentDesigner)this).Component as BaseItem;
			if (e.Component is BaseItem baseItem2 && baseItem != null && baseItem.SubItems.Contains(baseItem2))
			{
				IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
				componentChangeService?.OnComponentChanging(baseItem, TypeDescriptor.GetProperties(baseItem)["SubItems"]);
				baseItem.SubItems.Remove(baseItem2);
				componentChangeService?.OnComponentChanged(baseItem, TypeDescriptor.GetProperties(baseItem)["SubItems"], null, null);
				RecalcLayout();
			}
		}
	}

	public virtual void RecalcLayout()
	{
	}

	private void ComponentRemoving(object sender, ComponentEventArgs e)
	{
		if (e.Component == ((ComponentDesigner)this).Component)
		{
			RemoveGlyph();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (DesignerActionUIEnabled)
		{
			ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
			if (selectionService != null)
			{
				selectionService.SelectionChanged -= SelectionChanged;
			}
			IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentRemoving -= ComponentRemoving;
				componentChangeService.ComponentRemoved -= ComponentRemoved;
			}
		}
		((ComponentDesigner)this).Dispose(disposing);
	}

	private void SelectionChanged(object sender, EventArgs e)
	{
		bool flag = false;
		ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
		if (selectionService == null)
		{
			return;
		}
		if (selectionService.PrimarySelection == ((ComponentDesigner)this).Component)
		{
			flag = true;
		}
		else
		{
			ICollection selectedComponents = selectionService.GetSelectedComponents();
			foreach (object item in selectedComponents)
			{
				if (item.Equals(((ComponentDesigner)this).Component))
				{
					flag = true;
					break;
				}
			}
		}
		OnDesignerSelectionChanged(flag);
		if (flag)
		{
			Control containerControl = GetContainerControl();
			if (containerControl != null)
			{
				Rectangle screenBounds = containerControl.RectangleToScreen(ComponentBounds);
				_DesignerGlyph = CreateDesignerGlyphWrapper((Component)((ComponentDesigner)this).Component, GetParentControl(), screenBounds, ((ComponentDesigner)this).ActionLists);
			}
		}
		else
		{
			RemoveGlyph();
		}
		IsSelected = flag;
	}

	public virtual Control GetHostControl()
	{
		return GetParentControl();
	}

	protected virtual Control GetParentControl()
	{
		return GetContainerControl();
	}

	protected virtual Control GetContainerControl()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		if (((ComponentDesigner)this).Component is BaseItem baseItem)
		{
			return (Control)baseItem.ContainerControl;
		}
		return null;
	}

	protected virtual void OnDesignerSelectionChanged(bool isSelected)
	{
	}

	private void RemoveGlyph()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		BehaviorService val = (BehaviorService)((ComponentDesigner)this).GetService(typeof(BehaviorService));
		if (val != null)
		{
			((CollectionBase)(object)_DesignerActionAdorner.Glyphs).Clear();
			if (val.Adorners.Contains(_DesignerActionAdorner))
			{
				val.Adorners.Remove(_DesignerActionAdorner);
			}
			if (_ActionListToolStrip != null)
			{
				((Control)_ActionListToolStrip).LocationChanged -= HostLocationChanged;
			}
			_ActionListToolStrip = null;
			InvalidateDesignerSurface();
		}
	}

	private void InvalidateDesignerSurface()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		BehaviorService val = (BehaviorService)((ComponentDesigner)this).GetService(typeof(BehaviorService));
		if (val == null)
		{
			return;
		}
		object actionUI = GetActionUI(val);
		if (actionUI != null)
		{
			object valueField = ReflectionHelpers.GetValueField(actionUI, "mainParentWindow", BindingFlags.Instance | BindingFlags.NonPublic);
			if (valueField is IWin32Window)
			{
				WinApi.RedrawWindow(((IWin32Window)valueField).Handle, IntPtr.Zero, IntPtr.Zero, 129u);
			}
		}
	}

	private Glyph CreateDesignerGlyphWrapper(Component targetComponent, Control ownerControl, Rectangle screenBounds, DesignerActionListCollection actionLists)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Expected O, but got Unknown
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		BehaviorService behaviorService = (BehaviorService)((ComponentDesigner)this).GetService(typeof(BehaviorService));
		if (targetComponent != null && behaviorService != null && actionLists != null)
		{
			try
			{
				MethodInvoker val = null;
				object actionUI = GetActionUI(behaviorService);
				FieldInfo fieldInfo = ReflectionHelpers.GetFieldInfo(actionUI, "designerActionHost", BindingFlags.Instance | BindingFlags.NonPublic);
				ToolStripDropDown val2 = null;
				object valueField = ReflectionHelpers.GetValueField(actionUI, "mainParentWindow", BindingFlags.Instance | BindingFlags.NonPublic);
				if (!(fieldInfo.GetValue(actionUI) is ToolStripDropDown))
				{
					ConstructorInfo constructorInfo = ReflectionHelpers.GetConstructorInfo(fieldInfo.FieldType, new Type[2]
					{
						actionUI.GetType(),
						typeof(IWin32Window)
					});
					object obj = constructorInfo.Invoke(new object[2] { actionUI, valueField });
					val2 = (ToolStripDropDown)((obj is ToolStripDropDown) ? obj : null);
					((Control)val2).AutoSize = false;
					((Control)val2).Padding = Padding.Empty;
					((ToolStrip)val2).Renderer = (ToolStripRenderer)(object)new NoBorderRenderer();
					((Control)val2).Text = "DesignerActionTopLevelForm";
					fieldInfo.SetValue(actionUI, val2);
				}
				else
				{
					object? value = fieldInfo.GetValue(actionUI);
					val2 = (ToolStripDropDown)((value is ToolStripDropDown) ? value : null);
				}
				Glyph glyph = default(Glyph);
				ref Glyph reference = ref glyph;
				object obj2 = ReflectionHelpers.InvokeMethod(actionUI, "GetDesignerActionGlyph", BindingFlags.Instance | BindingFlags.NonPublic, new object[2] { targetComponent, actionLists });
				reference = (Glyph)((obj2 is Glyph) ? obj2 : null);
				if (glyph != null)
				{
					if (val == null)
					{
						val = (MethodInvoker)delegate
						{
							SetGlyphBounds(glyph, screenBounds, behaviorService);
						};
					}
					BarUtilities.InvokeDelayed(val, 100);
					if (val2 != null)
					{
						((Control)val2).LocationChanged += HostLocationChanged;
						Point point = new Point(glyph.Bounds.Right, glyph.Bounds.Y);
						((Control)val2).Bounds = new Rectangle(behaviorService.AdornerWindowPointToScreen(point), ((Control)val2).Size);
					}
					FieldInfo fieldInfo2 = ReflectionHelpers.GetFieldInfo(glyph.Behavior, "serviceProvider", BindingFlags.Instance | BindingFlags.NonPublic);
					GlyphServiceProvider value2 = new GlyphServiceProvider((IServiceProvider)fieldInfo2.GetValue(glyph.Behavior));
					fieldInfo2.SetValue(glyph.Behavior, value2);
					behaviorService.Adorners.Add(_DesignerActionAdorner);
					_DesignerActionAdorner.Glyphs.Add(glyph);
					if (valueField is Control)
					{
						((Control)valueField).Invalidate();
					}
				}
				if (valueField is IWin32Window)
				{
					WinApi.RedrawWindow(((IWin32Window)valueField).Handle, IntPtr.Zero, IntPtr.Zero, 129u);
				}
				if (_ActionListToolStrip != val2)
				{
					if (_ActionListToolStrip != null)
					{
						((Control)_ActionListToolStrip).LocationChanged -= HostLocationChanged;
					}
					_ActionListToolStrip = val2;
				}
				return glyph;
			}
			catch
			{
				return null;
			}
		}
		return null;
	}

	private void HostLocationChanged(object sender, EventArgs e)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		if (!_SettingBounds)
		{
			ToolStripDropDown val = (ToolStripDropDown)((sender is ToolStripDropDown) ? sender : null);
			BehaviorService val2 = (BehaviorService)((ComponentDesigner)this).GetService(typeof(BehaviorService));
			if (val != null && _DesignerGlyph != null && val2 != null)
			{
				_SettingBounds = true;
				Point point = new Point(_DesignerGlyph.Bounds.Right, _DesignerGlyph.Bounds.Y);
				Rectangle bounds = new Rectangle(val2.AdornerWindowPointToScreen(point), ((Control)val).Size);
				((Control)val).Bounds = bounds;
				_SettingBounds = false;
			}
		}
	}

	private void SetGlyphBounds(Glyph gl, Rectangle itemBounds, BehaviorService behaviorService)
	{
		Size glyphSize = GetGlyphSize();
		Point location = behaviorService.ScreenToAdornerWindow(new Point(itemBounds.Right - 6, itemBounds.Y));
		behaviorService.ScreenToAdornerWindow(Point.Empty);
		ReflectionHelpers.SetValueField(gl, "bounds", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField, new Rectangle(location, glyphSize));
		InvalidateDesignerSurface();
	}

	private object GetActionUI(BehaviorService behaviorService)
	{
		if (behaviorService == null)
		{
			return null;
		}
		return ReflectionHelpers.GetValueProperty(behaviorService, "DesignerActionUI", BindingFlags.Instance | BindingFlags.NonPublic);
	}

	private Rectangle GetGlyphBounds(Rectangle componentBounds)
	{
		Size glyphSize = GetGlyphSize();
		return new Rectangle(componentBounds.Right - glyphSize.Width, componentBounds.Y, glyphSize.Width, glyphSize.Height);
	}

	private Size GetGlyphSize()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Type type = typeof(ControlDesigner).Assembly.GetType("System.Windows.Forms.DpiHelper");
			if ((object)type != null && (bool)type.GetProperty("IsScalingRequired", BindingFlags.Static | BindingFlags.Public).GetValue(null, null))
			{
				object[] array = new object[2]
				{
					(object)new Bitmap(10, 10),
					0
				};
				type.GetMethod("ScaleBitmapLogicalToDevice", BindingFlags.Static | BindingFlags.Public).Invoke(null, array);
				return ((Image)array[0]).Size;
			}
		}
		catch (Exception value)
		{
			Console.WriteLine(value);
		}
		return new Size(10, 10);
	}
}
