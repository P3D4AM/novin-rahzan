using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Stimulsoft.Base.Localization;
using Stimulsoft.BlocklyEditor.Wpf.ControlsV2;
using Stimulsoft.BlocklyEditor.Wpf.ControlsV2.Helpers;
using Stimulsoft.BlocklyEditor.Wpf.Helper;
using Stimulsoft.Report;

namespace Stimulsoft.BlocklyEditor.Wpf;

public class StiInstallWebView2MessageWindow : StiWindow, IComponentConnector
{
	internal Image imageIcon;

	internal TextBlock textBlockMessage;

	internal StiButton buttonSwitch;

	internal StiButton buttonOK;

	internal StiButton buttonCancel;

	private bool _contentLoaded;

	public InstallMessageBoxResult Result { get; set; } = InstallMessageBoxResult.Cancel;


	public StiInstallWebView2MessageWindow()
	{
		InitializeComponent();
		StiBlocksV2ThemeHelper.InitTheme((Control)(object)this);
		if (!DesignerProperties.GetIsInDesignMode((DependencyObject)(object)this))
		{
			Localize();
			DoScale();
		}
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.Key == 13)
		{
			((RoutedEventArgs)e).Handled = true;
			((Window)this).Close();
		}
	}

	private void This_Loaded(object sender, RoutedEventArgs e)
	{
		((Window)this).Activate();
	}

	private void ButtonSwitch_Click(object sender, RoutedEventArgs e)
	{
		StiSettings.Load();
		StiOptions.Designer.Editors.ScriptEditor.BlocksScriptEditorByDefault = false;
		StiSettings.Save();
		Result = InstallMessageBoxResult.GoToCode;
		((Window)this).Close();
	}

	private void ButtonOk_Click(object sender, RoutedEventArgs e)
	{
		Result = InstallMessageBoxResult.Ok;
		((Window)this).Close();
	}

	private void ButtonCancel_Click(object sender, RoutedEventArgs e)
	{
		Result = InstallMessageBoxResult.Cancel;
		((Window)this).Close();
	}

	private void Localize()
	{
		((Window)this).Title = (StiLocalization.IsRussian ? "Требуется  установить дополнительный компонент" : "Extra component required");
		textBlockMessage.Text = (StiLocalization.IsRussian ? "Для корректной работы Blockly редактора, необходимо установить среду выполнения WebView2." : "Please install the WebView2 Runtime to work with the Blockly editor correctly.");
		((ContentControl)buttonOK).Content = StiLocalization.Get("NuGet", "DownloadAndInstall");
		((ContentControl)buttonCancel).Content = StiLocalization.Get("Buttons", "Cancel").Replace("&", "_");
		((ContentControl)buttonSwitch).Content = string.Format(StiLocalization.Get("Buttons", "SwitchTo"), StiLocalization.Get("FormDesigner", "Code"));
	}

	private void DoScale()
	{
		StiImageHelper.SetBitmapImage(imageIcon, "Message/Warning", 32, 32);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "6.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri uri = new Uri("/Stimulsoft.BlocklyEditor.Wpf;component/stiinstallwebview2messagewindow.xaml", UriKind.Relative);
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
			imageIcon = (Image)target;
			break;
		case 2:
			textBlockMessage = (TextBlock)target;
			break;
		case 3:
			buttonSwitch = (StiButton)target;
			break;
		case 4:
			buttonOK = (StiButton)target;
			break;
		case 5:
			buttonCancel = (StiButton)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
