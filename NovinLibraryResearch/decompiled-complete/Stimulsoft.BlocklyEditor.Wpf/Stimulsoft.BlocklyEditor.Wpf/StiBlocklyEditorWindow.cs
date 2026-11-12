using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using Stimulsoft.Base;
using Stimulsoft.Base.Blocks;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.BlocklyEditor.Wpf.ControlsV2;
using Stimulsoft.BlocklyEditor.Wpf.ControlsV2.Helpers;
using Stimulsoft.BlocklyEditor.Wpf.Helper;
using Stimulsoft.Report;
using Stimulsoft.Report.Events;

namespace Stimulsoft.BlocklyEditor.Wpf;

public class StiBlocklyEditorWindow : StiWindow, IStiBlocklyEditorWindow, IComponentConnector
{
	internal StiBlocklyEditorControl blocksEditorControl;

	internal StiButton buttonSwitch;

	internal StiButton buttonOK;

	internal StiButton buttonCancel;

	private bool _contentLoaded;

	public string Xml { get; private set; }

	public IStiReport Report
	{
		get
		{
			return blocksEditorControl.Report;
		}
		set
		{
			blocksEditorControl.Report = value as StiReport;
		}
	}

	public object EventValue
	{
		get
		{
			return blocksEditorControl.EventValue;
		}
		set
		{
			blocksEditorControl.EventValue = value as StiEvent;
		}
	}

	public event EventHandler SwitchToCode;

	public StiBlocklyEditorWindow(string xml, IStiReport report, object eventValue, bool showCurrentValue)
	{
		InitializeComponent();
		StiWindowHelper.LoadStateWindow((Window)(object)this);
		StiBlocksV2ThemeHelper.InitTheme((Control)(object)this);
		if (!DesignerProperties.GetIsInDesignMode((DependencyObject)(object)this))
		{
			Report = report;
			EventValue = eventValue;
			Localize();
			((Window)this).SourceInitialized += delegate
			{
				((Window)(object)this).HideMinimizeAndMaximizeButtons();
			};
			blocksEditorControl.Xml = xml;
			blocksEditorControl.ShowCurrentValue = showCurrentValue;
			((UIElement)buttonSwitch).Visibility = (Visibility)((!StiOptions.Designer.IsDeveloperDesigner) ? 2 : 0);
			((Window)this).Title = ((Window)this).Title + $" [{EventValue}]";
			((Window)this).Closed += This_Closed;
			if (string.IsNullOrEmpty(xml))
			{
				blocksEditorControl.ControlLoaded += BlocksEditorControl_ControlLoaded;
			}
		}
	}

	private void InvokeSwitchToCode()
	{
		this.SwitchToCode?.Invoke(this, null);
	}

	public void Localize()
	{
		((ContentControl)buttonOK).Content = StiLocalization.Get("Buttons", "Ok").Replace("&", "_");
		((ContentControl)buttonCancel).Content = StiLocalization.Get("Buttons", "Cancel").Replace("&", "_");
		((ContentControl)buttonSwitch).Content = string.Format(StiLocalization.Get("Buttons", "SwitchTo"), StiLocalization.Get("FormDesigner", "Code"));
	}

	public bool? ShowDialog(IntPtr? intPtr, PointD? centerOwner)
	{
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		if (string.IsNullOrWhiteSpace(StiWebView2Installer.GetWebView2Version()))
		{
			StiInstallWebView2MessageWindow stiInstallWebView2MessageWindow = new StiInstallWebView2MessageWindow();
			((Window)stiInstallWebView2MessageWindow).ShowDialog();
			switch (stiInstallWebView2MessageWindow.Result)
			{
			case InstallMessageBoxResult.GoToCode:
				InvokeSwitchToCode();
				return false;
			case InstallMessageBoxResult.Cancel:
				return false;
			case InstallMessageBoxResult.Ok:
				if (!StiWebView2Installer.InstallWebView2RuntimeAsync())
				{
					return false;
				}
				break;
			}
		}
		if (centerOwner.HasValue)
		{
			((Window)this).Left = centerOwner.GetValueOrDefault().X - ((FrameworkElement)this).Width / 2.0;
			((Window)this).Top = centerOwner.GetValueOrDefault().Y - ((FrameworkElement)this).Height / 2.0;
		}
		if (intPtr.HasValue)
		{
			WindowInteropHelper val = new WindowInteropHelper((Window)(object)this);
			val.Owner = intPtr.GetValueOrDefault();
		}
		return ((Window)this).ShowDialog();
	}

	private void This_Closed(object sender, EventArgs e)
	{
		StiWindowHelper.SaveStateWindow((Window)(object)this);
		blocksEditorControl.Dispose();
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.Key == 13)
		{
			((Window)this).Close();
		}
	}

	private async void ButtonOk_Click(object sender, RoutedEventArgs e)
	{
		Xml = await blocksEditorControl.GetXmlAsync();
		((Window)this).DialogResult = true;
		((Window)this).Close();
	}

	private void ButtonCancel_Click(object sender, RoutedEventArgs e)
	{
		((Window)this).Close();
	}

	private void ButtonSwitch_Click(object sender, RoutedEventArgs e)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		MessageBoxResult val = MessageBox.Show((Window)(object)this, StiLocalization.Get("Messages", "SwitchingBetweenModes"), StiOptions.Designer.GetDesignerTitle(), (MessageBoxButton)1);
		if ((int)val == 1)
		{
			((Window)this).Close();
			InvokeSwitchToCode();
		}
	}

	private void BlocksEditorControl_ControlLoaded(object sender, EventArgs e)
	{
		StiSettings.Load();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "6.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri uri = new Uri("/Stimulsoft.BlocklyEditor.Wpf;component/stiblocklyeditorwindow.xaml", UriKind.Relative);
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
		switch (connectionId)
		{
		case 1:
			blocksEditorControl = (StiBlocklyEditorControl)target;
			break;
		case 2:
			buttonSwitch = (StiButton)target;
			break;
		case 3:
			buttonOK = (StiButton)target;
			break;
		case 4:
			buttonCancel = (StiButton)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
