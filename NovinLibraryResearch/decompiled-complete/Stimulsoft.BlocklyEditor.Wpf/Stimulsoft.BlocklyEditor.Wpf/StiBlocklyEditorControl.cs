using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Markup;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using Microsoft.Win32;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.BlocklyEditor.Wpf.ControlsV2;
using Stimulsoft.BlocklyEditor.Wpf.Helper;
using Stimulsoft.BlocklyEditor.Wpf.src;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.BlocklyEditor.Wpf;

[DesignTimeVisible(false)]
public class StiBlocklyEditorControl : UserControl, IDisposable, IComponentConnector
{
	internal Grid grid;

	internal WebView2 browser;

	internal StiButton buttonOpen;

	internal StiButton buttonSave;

	internal StiButton buttonSample;

	private bool _contentLoaded;

	public StiDomManipulationDotnetInterop DomManipulationDotnetInterop { get; set; }

	public string Xml
	{
		get
		{
			return DomManipulationDotnetInterop.Xml;
		}
		set
		{
			DomManipulationDotnetInterop.Xml = value;
		}
	}

	public StiReport Report { get; set; }

	public bool ShowCurrentValue
	{
		get
		{
			return DomManipulationDotnetInterop.ShowCurrentValue;
		}
		set
		{
			DomManipulationDotnetInterop.ShowCurrentValue = value;
		}
	}

	public StiEvent EventValue { get; set; }

	public event EventHandler ControlLoaded;

	public StiBlocklyEditorControl()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		InitializeComponent();
		Localize();
		DoScale();
		DomManipulationDotnetInterop = new StiDomManipulationDotnetInterop();
		((FrameworkElement)this).Loaded += new RoutedEventHandler(This_Loaded);
	}

	private void InvokeControlLoaded()
	{
		this.ControlLoaded?.Invoke(this, null);
	}

	private async void This_Loaded(object sender, RoutedEventArgs e)
	{
		await InitializeBrowserAsync();
		browser.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
		string htmlBlockly = GetHtmlBlockly();
		browser.NavigationCompleted += Browser_NavigationCompleted;
		browser.NavigateToString(htmlBlockly);
	}

	private async void Browser_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
	{
		browser.CoreWebView2.AddHostObjectToScript("dotnet", (object)DomManipulationDotnetInterop);
		await browser.ExecuteScriptAsync("initScript()");
		await browser.ExecuteScriptAsync("init()");
		InvokeControlLoaded();
	}

	private async void ButtonOpen_Click(object sender, RoutedEventArgs e)
	{
		e.Handled = true;
		try
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			try
			{
				((FileDialog)openFileDialog).Filter = StiFileDialogHelper.GetBlocklyFilter();
				((FileDialog)openFileDialog).RestoreDirectory = true;
				if ((int)((CommonDialog)openFileDialog).ShowDialog() == 1)
				{
					Xml = File.ReadAllText(((FileDialog)openFileDialog).FileName);
					await browser.ExecuteScriptAsync("update()");
				}
			}
			finally
			{
				((IDisposable)openFileDialog)?.Dispose();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private async void ButtonSave_Click(object sender, RoutedEventArgs e)
	{
		e.Handled = true;
		SaveFileDialog dialog = new SaveFileDialog();
		((FileDialog)dialog).FileName = EventValue.ToString();
		((FileDialog)dialog).Filter = StiFileDialogHelper.GetBlocklyFilter();
		((FileDialog)dialog).RestoreDirectory = true;
		if (((CommonDialog)dialog).ShowDialog() == true)
		{
			try
			{
				string contents = await GetXmlAsync();
				File.WriteAllText(((FileDialog)dialog).FileName, contents);
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}
	}

	private async void ButtonSample_Click(object sender, RoutedEventArgs e)
	{
		Window window = Window.GetWindow((DependencyObject)(object)this);
		StiBlocklyQuickStartWindow stiBlocklyQuickStartWindow = new StiBlocklyQuickStartWindow();
		((Window)stiBlocklyQuickStartWindow).Owner = window;
		if (((Window)stiBlocklyQuickStartWindow).ShowDialog().GetValueOrDefault() && !string.IsNullOrEmpty(stiBlocklyQuickStartWindow.SampleUrl))
		{
			await LoadSampleAsync(stiBlocklyQuickStartWindow.SampleUrl);
		}
	}

	public void Localize()
	{
		((FrameworkElement)buttonOpen).ToolTip = StiLocalization.Get("Buttons", "Open");
		((FrameworkElement)buttonSave).ToolTip = StiLocalization.Get("Buttons", "Save");
		((FrameworkElement)buttonSample).ToolTip = StiLocalization.Get("FormFormatEditor", "Sample");
	}

	private async Task InitializeBrowserAsync()
	{
		CoreWebView2Environment val = await CoreWebView2Environment.CreateAsync((string)null, GetRootDataStimulsoftFolder(), (CoreWebView2EnvironmentOptions)null);
		await browser.EnsureCoreWebView2Async(val);
		browser.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
	}

	private static string GetRootDataStimulsoftFolder()
	{
		string text = (string.IsNullOrEmpty(StiOptions.Designer.Blockly.WebView2UserDataFolder) ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Stimulsoft", "WebView2") : StiOptions.Designer.Blockly.WebView2UserDataFolder);
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		return text;
	}

	public async Task LoadSampleAsync(string sampleeUrl)
	{
		string resourceFileText = GetResourceFileText("src.samples." + sampleeUrl + ".blockly", "Stimulsoft.BlocklyEditor.Wpf");
		await browser.ExecuteScriptAsync("setSample('" + resourceFileText + "')");
	}

	private void DoScale()
	{
		((ContentControl)buttonOpen).Content = Stimulsoft.BlocklyEditor.Wpf.Helper.StiImageHelper.GetImage("Open");
		((ContentControl)buttonSave).Content = Stimulsoft.BlocklyEditor.Wpf.Helper.StiImageHelper.GetImage("Save");
		((ContentControl)buttonSample).Content = Stimulsoft.BlocklyEditor.Wpf.Helper.StiImageHelper.GetImage("Information");
	}

	public string GetXML()
	{
		return Xml;
	}

	private string GetHtmlBlockly()
	{
		string text = GetResourceFileText("src.BlockyHTML.html", "Stimulsoft.BlocklyEditor.Wpf");
		string drillDownFontList = GetDrillDownFontList();
		if (!string.IsNullOrEmpty(drillDownFontList))
		{
			text = text.Replace("[\"empty\", \"EMPTY\"]", drillDownFontList);
		}
		drillDownFontList = GetDrillDownComponentList();
		if (!string.IsNullOrEmpty(drillDownFontList))
		{
			text = text.Replace("[\"Component\", \"EMPTYCOMPONENT\"]", drillDownFontList);
		}
		drillDownFontList = GetDrillDownComponentGetComponentsList();
		if (!string.IsNullOrEmpty(drillDownFontList))
		{
			text = text.Replace("[\"ComponentGetComponents\", \"EMPTYCOMPONENTGETCOMPONENTS\"]", drillDownFontList);
		}
		drillDownFontList = GetDrillDownVariableList();
		if (!string.IsNullOrEmpty(drillDownFontList))
		{
			text = text.Replace("[\"Variable\", \"EMPTYVARIABLE\"]", drillDownFontList);
		}
		drillDownFontList = GetDrillDownDataSourceList();
		if (!string.IsNullOrEmpty(drillDownFontList))
		{
			text = text.Replace("[\"DataSource\", \"EMPTYDATASOURCE\"]", drillDownFontList);
		}
		drillDownFontList = GetDrillDownSystenVariableList();
		if (!string.IsNullOrEmpty(drillDownFontList))
		{
			text = text.Replace("[\"empty\", \"EMPTYSYSTEMVARIABLE\"]", drillDownFontList);
		}
		drillDownFontList = GetDrillDownDataSourcePropertyList();
		if (!string.IsNullOrEmpty(drillDownFontList))
		{
			text = text.Replace("[\"empty\", \"EMPTYDATASOURCEPROPERTY\"]", drillDownFontList);
		}
		drillDownFontList = GetDrillDownDataSourceMethodsList();
		if (!string.IsNullOrEmpty(drillDownFontList))
		{
			text = text.Replace("[\"empty\", \"EMPTYDATASOURCEMETHODS\"]", drillDownFontList);
		}
		text = text.Replace("/*empty_lable*/", StiLocalization.Get("Dashboard", "DragDropBlocks"));
		return text.Replace("/*functions*/", StiFunctionBlocksParser.GetInitFunctionBlocks());
	}

	private string GetDrillDownFontList()
	{
		List<string> list = StiFontHelper.LoadFonts(Report);
		return GetDrillDownOptions(list);
	}

	private string GetDrillDownComponentList()
	{
		List<string> list = StiComponentHelper.LoadComponents(Report);
		return GetDrillDownOptions(list);
	}

	private string GetDrillDownComponentGetComponentsList()
	{
		List<string> list = StiComponentHelper.LoadContainerComponents(Report);
		return GetDrillDownOptions(list);
	}

	private string GetDrillDownVariableList()
	{
		List<string> list = StiVariableHelper.LoadVariables(Report);
		return GetDrillDownOptions(list);
	}

	private string GetDrillDownDataSourceList()
	{
		List<string> list = Stimulsoft.BlocklyEditor.Wpf.Helper.StiDataSourceHelper.LoadDataSource(Report);
		return GetDrillDownOptions(list);
	}

	private string GetDrillDownDataSourcePropertyList()
	{
		List<string> dataSourceProperties = Stimulsoft.BlocklyEditor.Wpf.Helper.StiDataSourceHelper.GetDataSourceProperties();
		return GetDrillDownOptions(dataSourceProperties);
	}

	private string GetDrillDownDataSourceMethodsList()
	{
		List<string> dataSourceMethods = Stimulsoft.BlocklyEditor.Wpf.Helper.StiDataSourceHelper.GetDataSourceMethods();
		return GetDrillDownOptions(dataSourceMethods);
	}

	private string GetDrillDownSystenVariableList()
	{
		List<string> systemVariables = StiSystemVariablesHelper.GetSystemVariables(Report);
		return GetDrillDownOptions(systemVariables);
	}

	private string GetDrillDownOptions(List<string> list)
	{
		string text = "";
		int num = 1;
		foreach (string item in list)
		{
			text = text + "[\"" + item + "\",\"" + item + "\"]";
			if (num != list.Count)
			{
				text += ",";
			}
			num++;
		}
		return text;
	}

	public async Task<string> GetXmlAsync()
	{
		if (Convert.ToInt32(await browser.CoreWebView2.ExecuteScriptAsync("getBlockCount()")) > 0)
		{
			string text = await browser.CoreWebView2.ExecuteScriptAsync("getXmlText()");
			return Encoding.UTF8.GetString(Convert.FromBase64String(text.Replace("\"", "")));
		}
		return string.Empty;
	}

	public string GetResourceFileText(string filename, string assemblyName)
	{
		string empty = string.Empty;
		using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(assemblyName + "." + filename);
		using StreamReader streamReader = new StreamReader(stream);
		return streamReader.ReadToEnd();
	}

	public void Dispose()
	{
		((HwndHost)browser).Dispose();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "6.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri uri = new Uri("/Stimulsoft.BlocklyEditor.Wpf;component/stiblocklyeditorcontrol.xaml", UriKind.Relative);
			Application.LoadComponent((object)this, uri);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "6.0.0.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "6.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	void IComponentConnector.Connect(int connectionId, object target)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		switch (connectionId)
		{
		case 1:
			grid = (Grid)target;
			break;
		case 2:
			browser = (WebView2)target;
			break;
		case 3:
			buttonOpen = (StiButton)target;
			break;
		case 4:
			buttonSave = (StiButton)target;
			break;
		case 5:
			buttonSample = (StiButton)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
