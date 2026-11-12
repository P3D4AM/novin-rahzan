using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Stimulsoft.Base.Localization;
using Stimulsoft.BlocklyEditor.Wpf.ControlsV2;
using Stimulsoft.BlocklyEditor.Wpf.ControlsV2.Helpers;
using Stimulsoft.BlocklyEditor.Wpf.Helper;
using Stimulsoft.BlocklyEditor.Wpf.QuickStart.Samples;
using Stimulsoft.Report;

namespace Stimulsoft.BlocklyEditor.Wpf;

public class StiBlocklyQuickStartWindow : Window, IComponentConnector
{
	private List<string> Colors = new List<string>
	{
		"#00b6ad", "#f3aa60", "#415c71", "#814968", "#00b1e1", "#2969b0", "#91c854", "#aa0d43", "#00948a", "#9365b8",
		"#ecc01c", "#a55b80"
	};

	internal StiColorListBox sampleList;

	internal CheckBox checkBoxShowNextTime;

	internal StiButton buttonClose;

	private bool _contentLoaded;

	public string SampleUrl { get; set; }

	public StiBlocklyQuickStartWindow()
	{
		InitializeComponent();
		StiBlocksV2ThemeHelper.InitTheme((Control)(object)this);
		if (!DesignerProperties.GetIsInDesignMode((DependencyObject)(object)this))
		{
			((Window)this).SourceInitialized += delegate
			{
				((Window)(object)this).HideMinimizeAndMaximizeButtons();
			};
			StiSettings.Load();
			((ToggleButton)checkBoxShowNextTime).IsChecked = StiSettings.GetBool("BlocklyQuickStartWindow", "ShowNextTime", nullValue: true);
			Localize();
			Build();
		}
	}

	private void CheckBoxShowNextTime_Checked(object sender, RoutedEventArgs e)
	{
		StiSettings.Load();
		StiSettings.Set("BlocklyQuickStartWindow", "ShowNextTime", ((ToggleButton)checkBoxShowNextTime).IsChecked);
		StiSettings.Save();
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

	private void ButtonClose_Click(object sender, RoutedEventArgs e)
	{
		((Window)this).Close();
	}

	private void Item_Selected(object sender, RoutedEventArgs e)
	{
		StiColorListBoxItem stiColorListBoxItem = sender as StiColorListBoxItem;
		SampleUrl = ((FrameworkElement)stiColorListBoxItem).Tag as string;
		((Window)this).DialogResult = true;
		((Window)this).Close();
	}

	public void Localize()
	{
		((ContentControl)buttonClose).Content = StiLocalization.Get("Buttons", "Close").Replace("&", "_");
	}

	public void Build()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		List<StiBlocklySample> samples = StiBlocklySampleHelper.GetSamples();
		for (int i = 0; i < samples.Count; i++)
		{
			StiBlocklySample sample = samples[i];
			Color color = GetColor(i);
			StiColorListBoxItem sampleItem = GetSampleItem(sample, color);
			((ItemsControl)sampleList).Items.Add((object)sampleItem);
		}
	}

	private Color GetColor(int index)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		int index2 = index % Colors.Count;
		string text = Colors[index2];
		return (Color)ColorConverter.ConvertFromString(text);
	}

	private StiColorListBoxItem GetSampleItem(StiBlocklySample sample, Color color)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		StiColorListBoxItem stiColorListBoxItem = new StiColorListBoxItem();
		((ContentControl)stiColorListBoxItem).Content = sample.Name;
		((FrameworkElement)stiColorListBoxItem).Tag = sample.Source;
		((Control)stiColorListBoxItem).Background = (Brush)new SolidColorBrush(color);
		StiColorListBoxItem stiColorListBoxItem2 = stiColorListBoxItem;
		((ListBoxItem)stiColorListBoxItem2).Selected += new RoutedEventHandler(Item_Selected);
		return stiColorListBoxItem2;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "6.0.0.0")]
	public void InitializeComponent()
	{
		if (!_contentLoaded)
		{
			_contentLoaded = true;
			Uri uri = new Uri("/Stimulsoft.BlocklyEditor.Wpf;component/quickstart/stiblocklyquickstartwindow.xaml", UriKind.Relative);
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
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		switch (connectionId)
		{
		case 1:
			((UIElement)(StiBlocklyQuickStartWindow)target).KeyDown += new KeyEventHandler(This_KeyDown);
			break;
		case 2:
			sampleList = (StiColorListBox)target;
			break;
		case 3:
			checkBoxShowNextTime = (CheckBox)target;
			((ToggleButton)checkBoxShowNextTime).Checked += new RoutedEventHandler(CheckBoxShowNextTime_Checked);
			((ToggleButton)checkBoxShowNextTime).Unchecked += new RoutedEventHandler(CheckBoxShowNextTime_Checked);
			break;
		case 4:
			buttonClose = (StiButton)target;
			break;
		default:
			_contentLoaded = true;
			break;
		}
	}
}
