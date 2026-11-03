using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.AdvTree;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Metro;
using DevComponents.DotNetBar.Metro.ColorTables;
using DevComponents.DotNetBar.Metro.Rendering;
using DevComponents.DotNetBar.Rendering;
using DevComponents.Editors;

namespace DevComponents.DotNetBar;

[ToolboxBitmap(typeof(StyleManager), "StyleManager.ico")]
[ToolboxItem(true)]
public class StyleManager : Component
{
	private static eStyle _ᎊ;

	private static MetroColorGeneratorParameters _ᎋ;

	private static eStyle _ᎌ;

	private static eDotNetBarStyle _ᎍ;

	private static Color _ᎎ;

	private static ReaderWriterLock _ᎏ;

	private static ArrayList __1390;

	private static ArrayList __1391;

	private static ReaderWriterLock __1392;

	[DefaultValue(eDotNetBarStyle.Office2007)]
	[Description("Indicates global style for the controls that have Style=ManagerControlled.")]
	[Category("Appearance")]
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

	[Description("Indicates color current style is tinted with.")]
	[Category("Appearance")]
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

	public static eStyle PreviousStyle => _ᎊ;

	[Editor("DevComponents.DotNetBar.Design.Metro.MetroColorThemeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
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
			return _ᎋ;
		}
		set
		{
			_ᎋ = value;
			if (_ᎌ == eStyle.Metro || _ᎌ == eStyle.OfficeMobile2014 || _ᎌ == eStyle.Office2016 || _ᎌ == eStyle.VisualStudio2012Dark || _ᎌ == eStyle.VisualStudio2012Light)
			{
				OnStyleChanged(eStyle.Metro, eStyle.Metro);
			}
		}
	}

	public static eStyle Style
	{
		get
		{
			return _ᎌ;
		}
		set
		{
			eStyle ᎌ = _ᎌ;
			_ᎌ = value;
			_ᎍ = DotNetBarStyleFromStyle(value);
			if (_ᎌ == eStyle.VisualStudio2012Light)
			{
				_ᎋ = MetroColorGeneratorParameters.VisualStudio2012Light;
			}
			else if (_ᎌ == eStyle.VisualStudio2012Dark)
			{
				_ᎋ = MetroColorGeneratorParameters.VisualStudio2012Dark;
			}
			else if (_ᎌ == eStyle.OfficeMobile2014)
			{
				_ᎋ = new MetroColorGeneratorParameters(Color.White, ColorScheme.GetColor(12011306));
			}
			else if (_ᎌ == eStyle.Office2016)
			{
				_ᎋ = MetroColorGeneratorParameters.Office2016Blue;
			}
			if (ᎌ != _ᎌ)
			{
				_ᎊ = ᎌ;
			}
			OnStyleChanged(ᎌ, value);
		}
	}

	public static Color ColorTint
	{
		get
		{
			return _ᎎ;
		}
		set
		{
			Color ᎎ = _ᎎ;
			_ᎎ = value;
			OnColorTintChanged(ᎎ, value);
		}
	}

	static StyleManager()
	{
		_ᎊ = eStyle.Office2007Blue;
		_ᎋ = MetroColorGeneratorParameters.Default;
		_ᎌ = eStyle.Office2007Blue;
		_ᎍ = eDotNetBarStyle.Office2007;
		_ᎎ = Color.Empty;
		__1390 = new ArrayList();
		__1391 = new ArrayList();
		_ᎏ = new ReaderWriterLock();
		__1392 = new ReaderWriterLock();
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
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Expected O, but got Unknown
		_ = Color.Empty;
		_ = Color.Empty;
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
			else if (item is Panel)
			{
				if (ShouldUpdateAmbientPanel((Panel)item))
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
			}
			else if (item is DevComponents.AdvTree.AdvTree || item is ComboTree)
			{
				DevComponents.AdvTree.AdvTree advTree = null;
				if (item is DevComponents.AdvTree.AdvTree)
				{
					advTree = (DevComponents.AdvTree.AdvTree)(object)item;
				}
				else
				{
					if (item.BackColor != Color.Transparent && (ambientSettings & eAmbientSettings.BackColor) == eAmbientSettings.BackColor)
					{
						item.BackColor = colorTable.EditControlBackColor;
					}
					if ((ambientSettings & eAmbientSettings.ForeColor) == eAmbientSettings.ForeColor)
					{
						item.ForeColor = colorTable.ForeColor;
					}
					advTree = ((ComboTree)(object)item).AdvTree;
				}
				if (((Control)advTree).BackColor != Color.Transparent && (ambientSettings & eAmbientSettings.BackColor) == eAmbientSettings.BackColor)
				{
					((Control)advTree).BackColor = colorTable.EditControlBackColor;
				}
				if ((ambientSettings & eAmbientSettings.ForeColor) == eAmbientSettings.ForeColor)
				{
					((Control)advTree).ForeColor = colorTable.ForeColor;
					if (advTree.NodeStyle != null && advTree.NodeStyle.BackColor.IsEmpty && !advTree.NodeStyle.TextColor.IsEmpty)
					{
						advTree.NodeStyle.TextColor = colorTable.ForeColor;
					}
					if (advTree.CellStyleDefault != null && advTree.CellStyleDefault.BackColor.IsEmpty && !advTree.CellStyleDefault.TextColor.IsEmpty)
					{
						advTree.CellStyleDefault.TextColor = colorTable.ForeColor;
					}
					if (advTree.NodesConnector != null)
					{
						advTree.NodesConnector.LineColor = colorTable.MetroPartColors.TextInactiveColor;
					}
				}
			}
			else if (item is TextBoxX)
			{
				if (item.BackColor != Color.Transparent && (ambientSettings & eAmbientSettings.BackColor) == eAmbientSettings.BackColor)
				{
					item.BackColor = colorTable.EditControlBackColor;
					if (((TextBoxX)(object)item).DisabledBackColor.IsEmpty)
					{
						((TextBoxX)(object)item).DisabledBackColor = colorTable.EditControlBackColor;
					}
				}
				if ((ambientSettings & eAmbientSettings.ForeColor) == eAmbientSettings.ForeColor)
				{
					item.ForeColor = colorTable.ForeColor;
				}
			}
			else if (item is MaskedTextBoxAdv)
			{
				if (item.BackColor != Color.Transparent && (ambientSettings & eAmbientSettings.BackColor) == eAmbientSettings.BackColor)
				{
					item.BackColor = colorTable.EditControlBackColor;
					if (((MaskedTextBoxAdv)(object)item).DisabledBackColor.IsEmpty)
					{
						((MaskedTextBoxAdv)(object)item).DisabledBackColor = colorTable.EditControlBackColor;
					}
				}
				if ((ambientSettings & eAmbientSettings.ForeColor) == eAmbientSettings.ForeColor)
				{
					item.ForeColor = colorTable.ForeColor;
				}
			}
			else if (IsEditControl(item))
			{
				if (item.BackColor != Color.Transparent && (ambientSettings & eAmbientSettings.BackColor) == eAmbientSettings.BackColor)
				{
					item.BackColor = colorTable.EditControlBackColor;
				}
				if ((ambientSettings & eAmbientSettings.ForeColor) == eAmbientSettings.ForeColor)
				{
					item.ForeColor = colorTable.ForeColor;
				}
			}
			else if (!(item is Bar) && !(item is SideNav))
			{
				if (item.BackColor != Color.Transparent && (ambientSettings & eAmbientSettings.BackColor) == eAmbientSettings.BackColor)
				{
					if (item is RibbonForm)
					{
						RibbonForm ribbonForm = (RibbonForm)(object)item;
						if (!ribbonForm.BackColorSet)
						{
							((Control)ribbonForm).BackColor = colorTable.CanvasColor;
							ribbonForm.BackColorSet = false;
						}
					}
					else
					{
						item.BackColor = colorTable.CanvasColor;
					}
				}
				if ((ambientSettings & eAmbientSettings.ForeColor) == eAmbientSettings.ForeColor)
				{
					item.ForeColor = colorTable.ForeColor;
				}
			}
			_ = item.BackColor;
			_ = item.ForeColor;
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

	private static bool IsEditControl(Control item)
	{
		if (!(item is TextBox) && !(item is TreeView) && !(item is ListView) && !(item is VisualControlBase) && !(item is ComboBoxEx) && !(item is MaskedTextBox) && !(item is MaskedTextBoxAdv) && !(item is RichTextBox) && !(item is RichTextBoxEx))
		{
			return item is TextBoxDropDown;
		}
		return true;
	}

	private static bool ShouldUpdateAmbientPanel(Panel item)
	{
		if (!(item is MetroTabPanel))
		{
			return !(item is SideNavPanel);
		}
		return false;
	}

	private static bool ShouldUpdateAmbientProperties(Control item)
	{
		return !(item is ButtonX);
	}

	private static eAmbientSettings GetAmbientSettings(Control control)
	{
		if (control is ButtonX || control is ScrollBarAdv)
		{
			return eAmbientSettings.None;
		}
		foreach (WeakReference item in __1391)
		{
			if (item.IsAlive)
			{
				StyleManagerAmbient styleManagerAmbient = (StyleManagerAmbient)item.Target;
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
		if (IsMetro(Style))
		{
			UpdateMetroAmbientColors(c);
		}
		else if (IsMetro(PreviousStyle))
		{
			eAmbientSettings ambientSettings = GetAmbientSettings(c);
			if ((ambientSettings & eAmbientSettings.BackColor) == eAmbientSettings.BackColor)
			{
				c.ResetBackColor();
			}
			if ((ambientSettings & eAmbientSettings.ForeColor) == eAmbientSettings.ForeColor)
			{
				c.ResetForeColor();
			}
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
		default:
			if (!IsVisualStudio2012(value) && value != eStyle.OfficeMobile2014 && value != eStyle.Office2016)
			{
				return eDotNetBarStyle.Office2007;
			}
			goto case eStyle.Metro;
		case eStyle.Metro:
			return eDotNetBarStyle.Metro;
		}
	}

	public static eDotNetBarStyle GetEffectiveStyle()
	{
		return _ᎍ;
	}

	public static void ChangeStyle(eStyle newStyle, Color colorTint)
	{
		_ᎎ = colorTint;
		Style = newStyle;
	}

	private static void OnColorTintChanged(Color oldValue, Color newValue)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		ChangeColorTable(_ᎌ, colorBlendChanged: true);
		NotifyOnStyleChange();
		IEnumerator enumerator = ((ReadOnlyCollectionBase)(object)Application.OpenForms).GetEnumerator();
		try
		{
			MethodInvoker val = null;
			MethodInvoker val2 = null;
			Form form;
			while (enumerator.MoveNext())
			{
				form = (Form)enumerator.Current;
				if (((Control)form).InvokeRequired)
				{
					if (form is OfficeForm)
					{
						Form obj = form;
						if (val == null)
						{
							val = (MethodInvoker)delegate
							{
								((OfficeForm)(object)form).InvalidateNonClient(invalidateForm: true);
							};
						}
						((Control)obj).Invoke((Delegate)(object)val);
					}
					Form obj2 = form;
					if (val2 == null)
					{
						val2 = (MethodInvoker)delegate
						{
							((Control)form).Invalidate(true);
						};
					}
					((Control)obj2).Invoke((Delegate)(object)val2);
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
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
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
					((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2010ColorTable(eOffice2010ColorScheme.Silver, _ᎎ);
				}
			}
			break;
		case eStyle.Office2010Blue:
			if (GlobalManager.Renderer is Office2007Renderer)
			{
				Office2007Renderer office2007Renderer2 = (Office2007Renderer)GlobalManager.Renderer;
				if (colorBlendChanged || !(office2007Renderer2.ColorTable is Office2010ColorTable) || (office2007Renderer2.ColorTable is Office2010ColorTable && ((Office2010ColorTable)office2007Renderer2.ColorTable).ColorScheme != eOffice2010ColorScheme.Blue))
				{
					((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2010ColorTable(eOffice2010ColorScheme.Blue, _ᎎ);
				}
			}
			break;
		case eStyle.Office2010Black:
			if (GlobalManager.Renderer is Office2007Renderer)
			{
				Office2007Renderer office2007Renderer3 = (Office2007Renderer)GlobalManager.Renderer;
				if (colorBlendChanged || !(office2007Renderer3.ColorTable is Office2010ColorTable) || (office2007Renderer3.ColorTable is Office2010ColorTable && ((Office2010ColorTable)office2007Renderer3.ColorTable).ColorScheme != eOffice2010ColorScheme.Black))
				{
					((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2010ColorTable(eOffice2010ColorScheme.Black, _ᎎ);
				}
			}
			break;
		case eStyle.VisualStudio2010Blue:
			if (GlobalManager.Renderer is Office2007Renderer)
			{
				Office2007Renderer office2007Renderer4 = (Office2007Renderer)GlobalManager.Renderer;
				if (colorBlendChanged || !(office2007Renderer4.ColorTable is Office2010ColorTable) || (office2007Renderer4.ColorTable is Office2010ColorTable && ((Office2010ColorTable)office2007Renderer4.ColorTable).ColorScheme != eOffice2010ColorScheme.VS2010))
				{
					((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2010ColorTable(eOffice2010ColorScheme.VS2010, _ᎎ);
				}
			}
			break;
		case eStyle.Windows7Blue:
			if (GlobalManager.Renderer is Office2007Renderer && (colorBlendChanged || !(((Office2007Renderer)GlobalManager.Renderer).ColorTable is Windows7ColorTable)))
			{
				((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Windows7ColorTable(eWindows7ColorScheme.Blue, _ᎎ);
			}
			break;
		case eStyle.Office2007Blue:
		case eStyle.Office2007Silver:
		case eStyle.Office2007Black:
		case eStyle.Office2007VistaGlass:
			if (GlobalManager.Renderer is Office2007Renderer && (colorBlendChanged || ((Office2007Renderer)GlobalManager.Renderer).ColorTable is Office2010ColorTable || ((Office2007Renderer)GlobalManager.Renderer).ColorTable is Windows7ColorTable || ((Office2007Renderer)GlobalManager.Renderer).ColorTable.InitialColorScheme != ColorSchemeFromStyle(style) || _ᎊ != style))
			{
				((Office2007Renderer)GlobalManager.Renderer).ColorTable = new Office2007ColorTable(ColorSchemeFromStyle(style), _ᎎ);
			}
			break;
		case eStyle.Metro:
		case eStyle.OfficeMobile2014:
		case eStyle.Office2016:
			MetroRender._139E(_ᎋ);
			break;
		case eStyle.VisualStudio2012Light:
			MetroRender._139E(MetroColorGeneratorParameters.VisualStudio2012Light);
			break;
		case eStyle.VisualStudio2012Dark:
			MetroRender._139E(MetroColorGeneratorParameters.VisualStudio2012Dark);
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
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		_ᎏ.AcquireReaderLock(-1);
		WeakReference[] array;
		try
		{
			array = new WeakReference[__1390.Count];
			__1390.CopyTo(array);
		}
		finally
		{
			_ᎏ.ReleaseReaderLock();
		}
		WeakReference[] array2 = array;
		Control control = default(Control);
		foreach (WeakReference weakReference in array2)
		{
			MethodInvoker val = null;
			object target = weakReference.Target;
			if (!weakReference.IsAlive || target == null)
			{
				continue;
			}
			MethodInvoker val2 = null;
			ref Control reference = ref control;
			object obj = target;
			reference = (Control)((obj is Control) ? obj : null);
			if (control != null && control.InvokeRequired)
			{
				Control obj2 = control;
				if (val == null)
				{
					val = (MethodInvoker)delegate
					{
						InvokeStyleManagerStyleChanged(target, _ᎍ);
					};
				}
				obj2.Invoke((Delegate)(object)val);
				if (!BarFunctions.IsHandleValid(control))
				{
					continue;
				}
				Control obj3 = control;
				if (val2 == null)
				{
					val2 = (MethodInvoker)delegate
					{
						control.Invalidate(true);
					};
				}
				obj3.Invoke((Delegate)(object)val2);
			}
			else
			{
				InvokeStyleManagerStyleChanged(target, _ᎍ);
				if (control != null && BarFunctions.IsHandleValid(control))
				{
					control.Invalidate(true);
				}
			}
		}
	}

	private static void OnStyleChanged(eStyle oldValue, eStyle newValue)
	{
		ChangeColorTable(newValue, !_ᎎ.IsEmpty);
		NotifyOnStyleChange();
	}

	private static void InvokeStyleManagerStyleChanged(object target, eDotNetBarStyle newStyle)
	{
		if (target == null)
		{
			return;
		}
		try
		{
			target.GetType().GetMethod("StyleManagerStyleChanged")?.Invoke(target, new object[1] { newStyle });
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
		bool isReaderLockHeld = _ᎏ.IsReaderLockHeld;
		if (isReaderLockHeld)
		{
			lockCookie = _ᎏ.UpgradeToWriterLock(-1);
		}
		else
		{
			_ᎏ.AcquireWriterLock(-1);
		}
		try
		{
			__1390.Add(weakReference);
		}
		finally
		{
			if (isReaderLockHeld)
			{
				_ᎏ.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				_ᎏ.ReleaseWriterLock();
			}
		}
		if (IsMetro(_ᎌ) && ShouldUpdateAmbientProperties(control))
		{
			UpdateAmbientColors(control);
		}
		control = null;
	}

	private static WeakReference GetControlReference(Control control)
	{
		ArrayList arrayList = __1390;
		if (arrayList == null)
		{
			return null;
		}
		_ᎏ.AcquireReaderLock(-1);
		try
		{
			foreach (WeakReference item in arrayList)
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
			_ᎏ.ReleaseReaderLock();
		}
		return null;
	}

	public static void Unregister(Control control)
	{
		LockCookie lockCookie = default(LockCookie);
		bool isReaderLockHeld = _ᎏ.IsReaderLockHeld;
		if (isReaderLockHeld)
		{
			lockCookie = _ᎏ.UpgradeToWriterLock(-1);
		}
		else
		{
			_ᎏ.AcquireWriterLock(-1);
		}
		try
		{
			ArrayList arrayList = new ArrayList();
			foreach (WeakReference item in __1390)
			{
				object target = item.Target;
				if (target != null && target.Equals(control))
				{
					arrayList.Add(item);
				}
				else if (target == null)
				{
					arrayList.Add(item);
				}
			}
			foreach (WeakReference item2 in arrayList)
			{
				__1390.Remove(item2);
			}
		}
		finally
		{
			if (isReaderLockHeld)
			{
				_ᎏ.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				_ᎏ.ReleaseWriterLock();
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
		bool isReaderLockHeld = __1392.IsReaderLockHeld;
		if (isReaderLockHeld)
		{
			lockCookie = __1392.UpgradeToWriterLock(-1);
		}
		else
		{
			__1392.AcquireWriterLock(-1);
		}
		try
		{
			__1391.Add(value);
		}
		finally
		{
			if (isReaderLockHeld)
			{
				__1392.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				__1392.ReleaseWriterLock();
			}
		}
		ambientProvider = null;
	}

	internal static void UnregisterAmbientManager(StyleManagerAmbient ambientProvider)
	{
		LockCookie lockCookie = default(LockCookie);
		bool isReaderLockHeld = __1392.IsReaderLockHeld;
		if (isReaderLockHeld)
		{
			lockCookie = __1392.UpgradeToWriterLock(-1);
		}
		else
		{
			__1392.AcquireWriterLock(-1);
		}
		try
		{
			foreach (WeakReference item in __1391)
			{
				object target = item.Target;
				if (target != null && target.Equals(ambientProvider))
				{
					__1390.Remove(item);
					break;
				}
			}
		}
		finally
		{
			if (isReaderLockHeld)
			{
				__1392.DowngradeFromWriterLock(ref lockCookie);
			}
			else
			{
				__1392.ReleaseWriterLock();
			}
		}
	}

	private static WeakReference GetAmbientReference(StyleManagerAmbient ambientProvider)
	{
		ArrayList arrayList = __1391;
		if (arrayList == null)
		{
			return null;
		}
		__1392.AcquireReaderLock(-1);
		try
		{
			foreach (WeakReference item in arrayList)
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
			__1392.ReleaseReaderLock();
		}
		return null;
	}

	public static bool IsMetro(eDotNetBarStyle style)
	{
		return style switch
		{
			eDotNetBarStyle.StyleManagerControlled => IsMetro(Style), 
			eDotNetBarStyle.Metro => true, 
			_ => style == eDotNetBarStyle.OfficeMobile2014, 
		};
	}

	public static bool IsMetro(eStyle style)
	{
		if (style != eStyle.Metro && !IsVisualStudio2012(style) && style != eStyle.OfficeMobile2014)
		{
			return style == eStyle.Office2016;
		}
		return true;
	}

	internal static bool IsVisualStudio2012(eStyle style)
	{
		if (style != eStyle.VisualStudio2012Light)
		{
			return style == eStyle.VisualStudio2012Dark;
		}
		return true;
	}
}
