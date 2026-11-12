using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Controls.Win.DotNetBar.Controls;
using Stimulsoft.Controls.Win.DotNetBar.Metro;
using Stimulsoft.Controls.Win.DotNetBar.Metro.ColorTables;
using Stimulsoft.Controls.Win.DotNetBar.Metro.Rendering;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxBitmap(typeof(StyleManager), "StyleManager.ico")]
[ToolboxItem(false)]
public class StyleManager : Component
{
	private static eStyle _PreviousStyle;

	private static MetroColorGeneratorParameters _MetroColorGeneratorParameters;

	private static eStyle _Style;

	private static eDotNetBarStyle _EffectiveStyle;

	private static Color _ColorTint;

	private static ReaderWriterLock _ReadWriteClientsListLock;

	private static ArrayList _RegisteredControls;

	private static ArrayList _RegisteredAmbients;

	private static ReaderWriterLock _ReadWriteAmbientListLock;

	[DefaultValue(eDotNetBarStyle.Office2007)]
	[Category("Appearance")]
	[Description("Indicates global style for the controls that have Style=ManagerControlled.")]
	public eStyle ManagerStyle
	{
		get
		{
			return Style;
		}
		set
		{
			Style = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates color current style is tinted with.")]
	public Color ManagerColorTint
	{
		get
		{
			return ColorTint;
		}
		set
		{
			ColorTint = value;
		}
	}

	public static eStyle PreviousStyle => _PreviousStyle;

	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.Metro.MetroColorThemeEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public MetroColorGeneratorParameters MetroColorParameters
	{
		get
		{
			return MetroColorGeneratorParameters;
		}
		set
		{
			MetroColorGeneratorParameters = value;
		}
	}

	public static MetroColorGeneratorParameters MetroColorGeneratorParameters
	{
		get
		{
			return _MetroColorGeneratorParameters;
		}
		set
		{
			_MetroColorGeneratorParameters = value;
			if (_Style == eStyle.Metro)
			{
				OnStyleChanged(eStyle.Metro, eStyle.Metro);
			}
		}
	}

	public static eStyle Style
	{
		get
		{
			return _Style;
		}
		set
		{
			eStyle style = _Style;
			_Style = value;
			_EffectiveStyle = DotNetBarStyleFromStyle(value);
			if (style != _Style)
			{
				_PreviousStyle = style;
			}
			OnStyleChanged(style, value);
		}
	}

	public static Color ColorTint
	{
		get
		{
			return _ColorTint;
		}
		set
		{
			Color colorTint = _ColorTint;
			_ColorTint = value;
			OnColorTintChanged(colorTint, value);
		}
	}

	static StyleManager()
	{
		_PreviousStyle = eStyle.Office2007Blue;
		_MetroColorGeneratorParameters = MetroColorGeneratorParameters.Default;
		_Style = eStyle.Office2007Blue;
		_EffectiveStyle = eDotNetBarStyle.Office2007;
		_ColorTint = Color.Empty;
		_RegisteredControls = new ArrayList();
		_RegisteredAmbients = new ArrayList();
		_ReadWriteClientsListLock = new ReaderWriterLock();
		_ReadWriteAmbientListLock = new ReaderWriterLock();
	}

	public StyleManager()
	{
	}

	public StyleManager(IContainer container)
		: this()
	{
		container.Add(this);
	}

	internal static void UpdateMetroAmbientColors(Control control)
	{
		MetroColorTable colorTable = MetroRender.GetColorTable();
		UpdateAmbientProperties(control, colorTable);
	}

	private static void UpdateAmbientProperties(Control item, MetroColorTable colorTable)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Expected O, but got Unknown
		Color empty = Color.Empty;
		Color empty2 = Color.Empty;
		eAmbientSettings ambientSettings = GetAmbientSettings(item);
		if (ambientSettings != 0)
		{
			if (item is PageSliderPage)
			{
				if ((ambientSettings & eAmbientSettings.BackColor) == eAmbientSettings.BackColor)
				{
					item.BackColor = colorTable.CanvasColor;
				}
				if ((ambientSettings & eAmbientSettings.ForeColor) == eAmbientSettings.ForeColor)
				{
					item.ForeColor = colorTable.ForeColor;
				}
			}
			else if (item is Panel && ShouldUpdateAmbientPanel((Panel)item))
			{
				if (item.BackColor != Color.Transparent && (ambientSettings & eAmbientSettings.BackColor) == eAmbientSettings.BackColor)
				{
					item.BackColor = colorTable.CanvasColorShadeLight;
				}
				if ((ambientSettings & eAmbientSettings.ForeColor) == eAmbientSettings.ForeColor)
				{
					item.ForeColor = colorTable.ForeColor;
				}
				if (item is GroupPanel && (ambientSettings & eAmbientSettings.BackColor) == eAmbientSettings.BackColor)
				{
					((GroupPanel)(object)item).CanvasColor = colorTable.CanvasColor;
					((Control)(GroupPanel)(object)item).BackColor = colorTable.CanvasColor;
				}
			}
			else
			{
				if (item.BackColor != Color.Transparent && (ambientSettings & eAmbientSettings.BackColor) == eAmbientSettings.BackColor)
				{
					item.BackColor = colorTable.CanvasColor;
				}
				if ((ambientSettings & eAmbientSettings.ForeColor) == eAmbientSettings.ForeColor)
				{
					item.ForeColor = colorTable.ForeColor;
				}
			}
			empty = item.BackColor;
			empty2 = item.ForeColor;
		}
		if ((ambientSettings & eAmbientSettings.ChildControls) != eAmbientSettings.ChildControls)
		{
			return;
		}
		foreach (Control item3 in (ArrangedElementCollection)item.Controls)
		{
			Control item2 = item3;
			UpdateAmbientProperties(item2, colorTable);
		}
	}

	private static bool ShouldUpdateAmbientPanel(Panel item)
	{
		return !(item is MetroTabPanel);
	}

	private static bool ShouldUpdateAmbientProperties(Control item)
	{
		return !(item is ButtonX);
	}

	private static eAmbientSettings GetAmbientSettings(Control control)
	{
		if (control is ButtonX)
		{
			return eAmbientSettings.None;
		}
		foreach (WeakReference registeredAmbient in _RegisteredAmbients)
		{
			if (registeredAmbient.IsAlive)
			{
				StyleManagerAmbient styleManagerAmbient = (StyleManagerAmbient)registeredAmbient.Target;
				if (styleManagerAmbient.Contains(control))
				{
					return styleManagerAmbient.GetEnableAmbientSettings(control);
				}
			}
		}
		return eAmbientSettings.All;
	}

	public static void UpdateAmbientColors(Control c)
	{
		if (Style == eStyle.Metro)
		{
			UpdateMetroAmbientColors(c);
		}
		else if (PreviousStyle == eStyle.Metro)
		{
			c.BackColor = SystemColors.Control;
			c.ForeColor = SystemColors.ControlText;
		}
	}

	private static MetroColorTable GetMetroColorTable()
	{
		return MetroRender.GetColorTable();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeManagerColorTint()
	{
		return !ManagerColorTint.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetManagerColorTint()
	{
		ManagerColorTint = Color.Empty;
	}

	public static void SetOfficeXPStyle()
	{
		_EffectiveStyle = eDotNetBarStyle.OfficeXP;
	}

	public static void SetOffice2000Style()
	{
		_EffectiveStyle = eDotNetBarStyle.Office2000;
	}

	private static eDotNetBarStyle DotNetBarStyleFromStyle(eStyle value)
	{
		switch (value)
		{
		case eStyle.Windows7Blue:
			return eDotNetBarStyle.Windows7;
		case eStyle.Office2010Silver:
		case eStyle.Office2010Blue:
		case eStyle.Office2010Black:
		case eStyle.VisualStudio2010Blue:
			return eDotNetBarStyle.Office2010;
		case eStyle.Metro:
			return eDotNetBarStyle.Metro;
		default:
			return eDotNetBarStyle.Office2007;
		}
	}

	public static eDotNetBarStyle GetEffectiveStyle()
	{
		return _EffectiveStyle;
	}

	public static void ChangeStyle(eStyle newStyle, Color colorTint)
	{
		_ColorTint = colorTint;
		Style = newStyle;
	}

	private static void OnColorTintChanged(Color oldValue, Color newValue)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		ChangeColorTable(_Style, colorBlendChanged: true);
		NotifyOnStyleChange();
		foreach (Form item in (ReadOnlyCollectionBase)(object)Application.OpenForms)
		{
			Form form = item;
			if (((Control)form).InvokeRequired)
			{
				if (form is OfficeForm)
				{
					((Control)form).Invoke((Delegate)(MethodInvoker)delegate
					{
						((OfficeForm)(object)form).InvalidateNonClient(invalidateForm: true);
					});
				}
				((Control)form).Invoke((Delegate)(MethodInvoker)delegate
				{
					((Control)form).Invalidate(true);
				});
			}
			else
			{
				if (form is OfficeForm)
				{
					((OfficeForm)(object)form).InvalidateNonClient(invalidateForm: true);
				}
				((Control)form).Invalidate(true);
			}
		}
	}

	private static void ChangeColorTable(eStyle style)
	{
		ChangeColorTable(style, colorBlendChanged: false);
	}

	private static void ChangeColorTable(eStyle style, bool colorBlendChanged)
	{
		switch (style)
		{
		case eStyle.Office2010Silver:
			if (GlobalManager.Renderer is Office2007Renderer)
			{
				Office2007Renderer office2007Renderer = (Office2007Renderer)GlobalManager.Renderer;
				if (colorBlendChanged || !(office2007Renderer.ColorTable is Office2010ColorTable) || (office2007Renderer.ColorTable is Office2010ColorTable && ((Office2010ColorTable)office2007Renderer.ColorTable).ColorScheme != 0))
				{
					((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2010ColorTable(eOffice2010ColorScheme.Silver, _ColorTint);
				}
			}
			break;
		case eStyle.Office2010Blue:
			if (GlobalManager.Renderer is Office2007Renderer)
			{
				Office2007Renderer office2007Renderer2 = (Office2007Renderer)GlobalManager.Renderer;
				if (colorBlendChanged || !(office2007Renderer2.ColorTable is Office2010ColorTable) || (office2007Renderer2.ColorTable is Office2010ColorTable && ((Office2010ColorTable)office2007Renderer2.ColorTable).ColorScheme != eOffice2010ColorScheme.Blue))
				{
					((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2010ColorTable(eOffice2010ColorScheme.Blue, _ColorTint);
				}
			}
			break;
		case eStyle.Office2010Black:
			if (GlobalManager.Renderer is Office2007Renderer)
			{
				Office2007Renderer office2007Renderer3 = (Office2007Renderer)GlobalManager.Renderer;
				if (colorBlendChanged || !(office2007Renderer3.ColorTable is Office2010ColorTable) || (office2007Renderer3.ColorTable is Office2010ColorTable && ((Office2010ColorTable)office2007Renderer3.ColorTable).ColorScheme != eOffice2010ColorScheme.Black))
				{
					((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2010ColorTable(eOffice2010ColorScheme.Black, _ColorTint);
				}
			}
			break;
		case eStyle.VisualStudio2010Blue:
			if (GlobalManager.Renderer is Office2007Renderer)
			{
				Office2007Renderer office2007Renderer4 = (Office2007Renderer)GlobalManager.Renderer;
				if (colorBlendChanged || !(office2007Renderer4.ColorTable is Office2010ColorTable) || (office2007Renderer4.ColorTable is Office2010ColorTable && ((Office2010ColorTable)office2007Renderer4.ColorTable).ColorScheme != eOffice2010ColorScheme.VS2010))
				{
					((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2010ColorTable(eOffice2010ColorScheme.VS2010, _ColorTint);
				}
			}
			break;
		case eStyle.Windows7Blue:
			if (GlobalManager.Renderer is Office2007Renderer && (colorBlendChanged || !(((Office2007Renderer)GlobalManager.Renderer).ColorTable is Windows7ColorTable)))
			{
				((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Windows7ColorTable(eWindows7ColorScheme.Blue, _ColorTint);
			}
			break;
		case eStyle.Office2007Blue:
		case eStyle.Office2007Silver:
		case eStyle.Office2007Black:
		case eStyle.Office2007VistaGlass:
			if (GlobalManager.Renderer is Office2007Renderer && (colorBlendChanged || ((Office2007Renderer)GlobalManager.Renderer).ColorTable is Office2010ColorTable || ((Office2007Renderer)GlobalManager.Renderer).ColorTable is Windows7ColorTable || ((Office2007Renderer)GlobalManager.Renderer).ColorTable.InitialColorScheme != ColorSchemeFromStyle(style) || _PreviousStyle != style))
			{
				((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2007ColorTable(ColorSchemeFromStyle(style), _ColorTint);
			}
			break;
		case eStyle.Metro:
			MetroRender.UpdateColorTable(_MetroColorGeneratorParameters);
			break;
		}
	}

	private static eOffice2007ColorScheme ColorSchemeFromStyle(eStyle style)
	{
		eOffice2007ColorScheme result = eOffice2007ColorScheme.Blue;
		switch (style)
		{
		case eStyle.Office2007Silver:
			result = eOffice2007ColorScheme.Silver;
			break;
		case eStyle.Office2007Black:
			result = eOffice2007ColorScheme.Black;
			break;
		case eStyle.Office2007VistaGlass:
			result = eOffice2007ColorScheme.VistaGlass;
			break;
		}
		return result;
	}

	private static void NotifyOnStyleChange()
	{
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		_ReadWriteClientsListLock.AcquireReaderLock(-1);
		WeakReference[] array;
		try
		{
			array = new WeakReference[_RegisteredControls.Count];
			_RegisteredControls.CopyTo(array);
		}
		finally
		{
			_ReadWriteClientsListLock.ReleaseReaderLock();
		}
		WeakReference[] array2 = array;
		Control control = default(Control);
		foreach (WeakReference weakReference in array2)
		{
			object target = weakReference.Target;
			if (!weakReference.IsAlive || target == null)
			{
				continue;
			}
			ref Control reference = ref control;
			object obj = target;
			reference = (Control)((obj is Control) ? obj : null);
			if (control != null && control.InvokeRequired)
			{
				control.Invoke((Delegate)(MethodInvoker)delegate
				{
					InvokeStyleManagerStyleChanged(target, _EffectiveStyle);
				});
				if (BarFunctions.IsHandleValid(control))
				{
					control.Invoke((Delegate)(MethodInvoker)delegate
					{
						control.Invalidate(true);
					});
				}
			}
			else
			{
				InvokeStyleManagerStyleChanged(target, _EffectiveStyle);
				if (control != null && BarFunctions.IsHandleValid(control))
				{
					control.Invalidate(true);
				}
			}
		}
	}

	private static void OnStyleChanged(eStyle oldValue, eStyle newValue)
	{
		ChangeColorTable(newValue, !_ColorTint.IsEmpty);
		NotifyOnStyleChange();
	}

	private static void InvokeStyleManagerStyleChanged(object target, eDotNetBarStyle newStyle)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (target == null)
			{
				return;
			}
			if (target is Control && ((Control)target).InvokeRequired)
			{
				Control control = default(Control);
				ref Control reference = ref control;
				object obj = target;
				reference = (Control)((obj is Control) ? obj : null);
				control.Invoke((Delegate)(EventHandler)delegate
				{
					MethodInfo method2 = ((object)control).GetType().GetMethod("StyleManagerStyleChanged");
					if (method2 != null)
					{
						method2.Invoke(target, new object[1] { newStyle });
					}
				});
			}
			else
			{
				MethodInfo method = target.GetType().GetMethod("StyleManagerStyleChanged");
				if (method != null)
				{
					method.Invoke(target, new object[1] { newStyle });
				}
			}
		}
		catch
		{
		}
	}

	public static void Register(Control control)
	{
		if (GetControlReference(control) != null)
		{
			return;
		}
		WeakReference weakReference = new WeakReference(control);
		weakReference.Target = control;
		LockCookie lockCookie = default(LockCookie);
		bool isReaderLockHeld = _ReadWriteClientsListLock.IsReaderLockHeld;
		if (isReaderLockHeld)
		{
			lockCookie = _ReadWriteClientsListLock.UpgradeToWriterLock(-1);
		}
		else
		{
			_ReadWriteClientsListLock.AcquireWriterLock(-1);
		}
		try
		{
			_RegisteredControls.Add(weakReference);
		}
		finally
		{
			if (isReaderLockHeld)
			{
				_ReadWriteClientsListLock.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				_ReadWriteClientsListLock.ReleaseWriterLock();
			}
		}
		if (_Style == eStyle.Metro && ShouldUpdateAmbientProperties(control))
		{
			UpdateAmbientColors(control);
		}
		control = null;
	}

	private static WeakReference GetControlReference(Control control)
	{
		ArrayList registeredControls = _RegisteredControls;
		if (registeredControls == null)
		{
			return null;
		}
		_ReadWriteClientsListLock.AcquireReaderLock(-1);
		try
		{
			foreach (WeakReference item in registeredControls)
			{
				object target = item.Target;
				if (target != null && target.Equals(control))
				{
					return item;
				}
			}
		}
		finally
		{
			_ReadWriteClientsListLock.ReleaseReaderLock();
		}
		return null;
	}

	public static void Unregister(Control control)
	{
		LockCookie lockCookie = default(LockCookie);
		bool isReaderLockHeld = _ReadWriteClientsListLock.IsReaderLockHeld;
		if (isReaderLockHeld)
		{
			lockCookie = _ReadWriteClientsListLock.UpgradeToWriterLock(-1);
		}
		else
		{
			_ReadWriteClientsListLock.AcquireWriterLock(-1);
		}
		try
		{
			foreach (WeakReference registeredControl in _RegisteredControls)
			{
				object target = registeredControl.Target;
				if (target != null && target.Equals(control))
				{
					_RegisteredControls.Remove(registeredControl);
					break;
				}
			}
		}
		finally
		{
			if (isReaderLockHeld)
			{
				_ReadWriteClientsListLock.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				_ReadWriteClientsListLock.ReleaseWriterLock();
			}
		}
	}

	internal static void RegisterAmbientManager(StyleManagerAmbient ambientProvider)
	{
		if (GetAmbientReference(ambientProvider) != null)
		{
			return;
		}
		WeakReference value = new WeakReference(ambientProvider);
		LockCookie lockCookie = default(LockCookie);
		bool isReaderLockHeld = _ReadWriteAmbientListLock.IsReaderLockHeld;
		if (isReaderLockHeld)
		{
			lockCookie = _ReadWriteAmbientListLock.UpgradeToWriterLock(-1);
		}
		else
		{
			_ReadWriteAmbientListLock.AcquireWriterLock(-1);
		}
		try
		{
			_RegisteredAmbients.Add(value);
		}
		finally
		{
			if (isReaderLockHeld)
			{
				_ReadWriteAmbientListLock.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				_ReadWriteAmbientListLock.ReleaseWriterLock();
			}
		}
		ambientProvider = null;
	}

	internal static void UnregisterAmbientManager(StyleManagerAmbient ambientProvider)
	{
		LockCookie lockCookie = default(LockCookie);
		bool isReaderLockHeld = _ReadWriteAmbientListLock.IsReaderLockHeld;
		if (isReaderLockHeld)
		{
			lockCookie = _ReadWriteAmbientListLock.UpgradeToWriterLock(-1);
		}
		else
		{
			_ReadWriteAmbientListLock.AcquireWriterLock(-1);
		}
		try
		{
			foreach (WeakReference registeredAmbient in _RegisteredAmbients)
			{
				object target = registeredAmbient.Target;
				if (target != null && target.Equals(ambientProvider))
				{
					_RegisteredControls.Remove(registeredAmbient);
					break;
				}
			}
		}
		finally
		{
			if (isReaderLockHeld)
			{
				_ReadWriteAmbientListLock.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				_ReadWriteAmbientListLock.ReleaseWriterLock();
			}
		}
	}

	private static WeakReference GetAmbientReference(StyleManagerAmbient ambientProvider)
	{
		ArrayList registeredAmbients = _RegisteredAmbients;
		if (registeredAmbients == null)
		{
			return null;
		}
		_ReadWriteAmbientListLock.AcquireReaderLock(-1);
		try
		{
			foreach (WeakReference item in registeredAmbients)
			{
				object target = item.Target;
				if (target != null && target.Equals(ambientProvider))
				{
					return item;
				}
			}
		}
		finally
		{
			_ReadWriteAmbientListLock.ReleaseReaderLock();
		}
		return null;
	}
}
