using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FormCustom.CurrencyTextBox;
using FormCustom.Forms;
using PacketParser;
using PacketParser.Entities;

namespace FormCustom;

public static class ControlUtilities
{
	private static ConcurrentDictionary<BaseForm, List<Tuple<Control, EventHolder>>> _forms = new ConcurrentDictionary<BaseForm, List<Tuple<Control, EventHolder>>>();

	private static SemaphoreSlim _fromDisposeSemaphore = new SemaphoreSlim(1, 1);

	internal static void Transelate(BaseForm parent, BaseFormConfig config, EventHolder events)
	{
		try
		{
			if (LanguageAndCalendar.Instance.TranslateUi() && ((Control)parent).Visible && !parent.IsDisposed)
			{
				List<Tuple<Control, EventHolder>> list = null;
				if (events != null)
				{
					list = new List<Tuple<Control, EventHolder>>();
					_forms.TryAdd(parent, list);
					((Component)(object)parent).Disposed += Parent_Disposed;
				}
				CeckControl(list, ((Control)parent).Controls, config, events);
				if (((Control)parent).ContextMenuStrip != null)
				{
					ChangeControlText(list, (Control)(object)((Control)parent).ContextMenuStrip, config, events);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Transelate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ControlUtilities.cs", 43);
		}
	}

	private static async void Parent_Disposed(object sender, EventArgs e)
	{
		try
		{
			await _fromDisposeSemaphore.WaitAsync();
			GC.Collect();
			BaseForm baseForm = (BaseForm)sender;
			((Component)(object)baseForm).Disposed -= Parent_Disposed;
			if (!_forms.TryRemove(baseForm, out var value))
			{
				return;
			}
			foreach (Tuple<Control, EventHolder> item2 in value)
			{
				if (item2.Item2.c_OnLeave != null)
				{
					item2.Item1.Leave -= item2.Item2.c_OnLeave;
				}
				if (item2.Item2.c_OnEnter != null)
				{
					item2.Item1.Enter -= item2.Item2.c_OnEnter;
				}
				if (item2.Item2.c_ControlChanged != null)
				{
					Control item = item2.Item1;
					CheckBox val = (CheckBox)(object)((item is CheckBox) ? item : null);
					if (val != null)
					{
						val.CheckedChanged -= item2.Item2.c_ControlChanged;
					}
					else
					{
						item2.Item1.TextChanged -= item2.Item2.c_ControlChanged;
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Parent_Disposed", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ControlUtilities.cs", 70);
		}
		finally
		{
			_fromDisposeSemaphore.Release();
		}
	}

	public static void Transelate(this Control control, BaseFormConfig config = null)
	{
		try
		{
			if (LanguageAndCalendar.Instance.TranslateUi() && control.Visible && !control.IsDisposed && !control.IsDisposed)
			{
				CeckControl(null, control.Controls, config, null);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Transelate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ControlUtilities.cs", 84);
		}
	}

	private static void CeckControl(List<Tuple<Control, EventHolder>> controlsEvent, ControlCollection objects, BaseFormConfig config, EventHolder events)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		try
		{
			foreach (Control item in (ArrangedElementCollection)objects)
			{
				Control val = item;
				ChangeControlText(controlsEvent, val, config, events);
				if (((ArrangedElementCollection)val.Controls).Count > 0)
				{
					CeckControl(controlsEvent, val.Controls, config, events);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CeckControl", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ControlUtilities.cs", 98);
		}
	}

	private static void ChangeControlText(List<Tuple<Control, EventHolder>> handledEvents, Control item, BaseFormConfig config, EventHolder events)
	{
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		//IL_0480: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Expected O, but got Unknown
		//IL_055e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0565: Expected O, but got Unknown
		//IL_0508: Unknown result type (might be due to invalid IL or missing references)
		//IL_050f: Expected O, but got Unknown
		//IL_05c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cd: Expected O, but got Unknown
		try
		{
			if (config == null)
			{
				config = new BaseFormConfig();
			}
			if (item.ContextMenuStrip != null)
			{
				ChangeControlText(handledEvents, (Control)(object)item.ContextMenuStrip, config, events);
			}
			DataGridView val = (DataGridView)(object)((item is DataGridView) ? item : null);
			if (val == null)
			{
				if (!(item is RibbonBar ribbonBar))
				{
					if (!(item is RibbonControl ribbonControl))
					{
						if (!(item is TabControl tabControl))
						{
							if (!(item is SuperTabControl superTabControl))
							{
								StatusStrip val2 = (StatusStrip)(object)((item is StatusStrip) ? item : null);
								if (val2 == null)
								{
									ContextMenuStrip val3 = (ContextMenuStrip)(object)((item is ContextMenuStrip) ? item : null);
									if (val3 == null)
									{
										UserControl val4 = (UserControl)(object)((item is UserControl) ? item : null);
										if (val4 == null)
										{
											if (!(item is GroupPanel groupPanel))
											{
												MenuStrip val5 = (MenuStrip)(object)((item is MenuStrip) ? item : null);
												if (val5 == null)
												{
													if (!(item is WebBrowser))
													{
														ToolStrip val6 = (ToolStrip)(object)((item is ToolStrip) ? item : null);
														if (val6 == null)
														{
															TextBox val7 = (TextBox)(object)((item is TextBox) ? item : null);
															if (val7 == null)
															{
																if (!(item is ComboBox) && !(item is SplitContainer))
																{
																	item.Text = LanguageAndCalendar.Instance.GetValue(item.Text);
																}
															}
															else if (val7 is TextBoxX textBoxX)
															{
																textBoxX.WatermarkText = LanguageAndCalendar.Instance.GetValue(textBoxX.WatermarkText);
															}
														}
														else
														{
															foreach (object item3 in (ArrangedElementCollection)val6.Items)
															{
																ToolStripLabel val8 = (ToolStripLabel)((item3 is ToolStripLabel) ? item3 : null);
																if (val8 != null)
																{
																	((ToolStripItem)val8).Text = LanguageAndCalendar.Instance.GetValue(((ToolStripItem)val8).Text);
																}
															}
														}
													}
												}
												else
												{
													foreach (ToolStripMenuItem item4 in (ArrangedElementCollection)((ToolStrip)val5).Items)
													{
														ToolStripMenuItem val9 = item4;
														((ToolStripItem)val9).Text = LanguageAndCalendar.Instance.GetValue(((ToolStripItem)val9).Text);
														ChangeControlText(val9);
													}
												}
											}
											else
											{
												((Control)groupPanel).Text = LanguageAndCalendar.Instance.GetValue(((Control)groupPanel).Text);
											}
										}
										else
										{
											if (((Control)val4).ContextMenuStrip != null && ((ArrangedElementCollection)((ToolStrip)((Control)val4).ContextMenuStrip).Items).Count > 0)
											{
												foreach (ToolStripItem item5 in (ArrangedElementCollection)((ToolStrip)((Control)val4).ContextMenuStrip).Items)
												{
													ToolStripItem val10 = item5;
													val10.Text = LanguageAndCalendar.Instance.GetValue(val10.Text);
												}
											}
											foreach (Control item6 in (ArrangedElementCollection)((Control)val4).Controls)
											{
												Control item2 = item6;
												ChangeControlText(handledEvents, item2, config, events);
											}
										}
									}
									else
									{
										foreach (object item7 in (ArrangedElementCollection)((ToolStrip)val3).Items)
										{
											if (!(item7 is ToolStripSeparator))
											{
												((ToolStripItem)item7).Text = LanguageAndCalendar.Instance.GetValue(((ToolStripItem)item7).Text);
												ChangeControlText((ToolStripItem)item7);
											}
										}
									}
								}
								else
								{
									foreach (object item8 in (ArrangedElementCollection)((ToolStrip)val2).Items)
									{
										ToolStripSplitButton val11 = (ToolStripSplitButton)((item8 is ToolStripSplitButton) ? item8 : null);
										if (val11 == null)
										{
											ToolStripStatusLabel val12 = (ToolStripStatusLabel)((item8 is ToolStripStatusLabel) ? item8 : null);
											if (val12 == null)
											{
												ToolStripDropDownButton val13 = (ToolStripDropDownButton)((item8 is ToolStripDropDownButton) ? item8 : null);
												if (val13 == null)
												{
													throw new Exception("statusStrip.Items contains not defined type(" + item8.GetType().Name + ")");
												}
												((ToolStripItem)val13).Text = LanguageAndCalendar.Instance.GetValue(((ToolStripItem)val13).Text);
												if (!((ToolStripDropDownItem)val13).HasDropDownItems)
												{
													continue;
												}
												foreach (object item9 in (ArrangedElementCollection)((ToolStripDropDownItem)val13).DropDownItems)
												{
													ToolStripDropDownItem val14 = (ToolStripDropDownItem)((item9 is ToolStripDropDownItem) ? item9 : null);
													if (val14 != null)
													{
														((ToolStripItem)val14).Text = LanguageAndCalendar.Instance.GetValue(((ToolStripItem)val14).Text);
													}
												}
											}
											else
											{
												((ToolStripItem)val12).Text = LanguageAndCalendar.Instance.GetValue(((ToolStripItem)val12).Text);
											}
											continue;
										}
										((ToolStripItem)val11).Text = LanguageAndCalendar.Instance.GetValue(((ToolStripItem)val11).Text);
										if (!((ToolStripDropDownItem)val11).HasDropDownItems)
										{
											continue;
										}
										foreach (object item10 in (ArrangedElementCollection)((ToolStripDropDownItem)val11).DropDownItems)
										{
											ToolStripDropDownItem val15 = (ToolStripDropDownItem)((item10 is ToolStripDropDownItem) ? item10 : null);
											if (val15 != null)
											{
												((ToolStripItem)val15).Text = LanguageAndCalendar.Instance.GetValue(((ToolStripItem)val15).Text);
											}
										}
									}
								}
							}
							else
							{
								foreach (SuperTabItem tab in superTabControl.Tabs)
								{
									tab.Text = LanguageAndCalendar.Instance.GetValue(tab.Text);
								}
							}
						}
						else
						{
							foreach (TabItem tab2 in tabControl.Tabs)
							{
								tab2.Text = LanguageAndCalendar.Instance.GetValue(tab2.Text);
							}
						}
					}
					else
					{
						foreach (BaseItem item11 in ribbonControl.Items)
						{
							item11.Text = LanguageAndCalendar.Instance.GetValue(item11.Text);
						}
					}
				}
				else
				{
					foreach (BaseItem item12 in ribbonBar.Items)
					{
						item12.Text = LanguageAndCalendar.Instance.GetValue(item12.Text);
					}
				}
			}
			else
			{
				foreach (DataGridViewColumn item13 in (BaseCollection)val.Columns)
				{
					DataGridViewColumn val16 = item13;
					val16.HeaderText = LanguageAndCalendar.Instance.GetValue(val16.HeaderText);
				}
			}
			if (handledEvents == null || events == null || (events.c_OnEnter == null && events.c_OnEnter == null && events.c_ControlChanged == null))
			{
				return;
			}
			bool flag = false;
			CheckBox val17 = (CheckBox)(object)((item is CheckBox) ? item : null);
			if (val17 != null)
			{
				if (events.c_ControlChanged != null)
				{
					val17.CheckedChanged += events.c_ControlChanged;
					flag = true;
				}
			}
			else if (events.c_ControlChanged != null)
			{
				item.TextChanged += events.c_ControlChanged;
				flag = true;
			}
			Type type = ((object)item).GetType();
			if (events.c_OnLeave != null && events.c_OnEnter != null && (type == typeof(TextBox) || type == typeof(TextBoxX) || type == typeof(ComboBox) || type == typeof(ComboBoxEx) || type == typeof(NumericUpDown) || type == typeof(MaskedTextBoxAdv) || type == typeof(CheckBox) || type == typeof(TextBoxFa) || type == typeof(DateBoxFa)))
			{
				item.Leave += events.c_OnLeave;
				item.Enter += events.c_OnEnter;
				flag = true;
			}
			if (flag)
			{
				handledEvents.Add(new Tuple<Control, EventHolder>(item, events));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChangeControlText", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ControlUtilities.cs", 241);
		}
	}

	private static void ChangeControlText(ToolStripMenuItem menuItem)
	{
		try
		{
			if (((ToolStripDropDownItem)menuItem).DropDownItems == null)
			{
				return;
			}
			foreach (object item in (ArrangedElementCollection)((ToolStripDropDownItem)menuItem).DropDownItems)
			{
				ToolStripMenuItem val = (ToolStripMenuItem)((item is ToolStripMenuItem) ? item : null);
				if (val != null)
				{
					((ToolStripItem)val).Text = LanguageAndCalendar.Instance.GetValue(((ToolStripItem)val).Text);
					ChangeControlText(val);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChangeControlText", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ControlUtilities.cs", 255);
		}
	}

	private static void ChangeControlText(ToolStripItem menuItem)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			ToolStripMenuItem val = (ToolStripMenuItem)(object)((menuItem is ToolStripMenuItem) ? menuItem : null);
			if (val == null || ((ToolStripDropDownItem)val).DropDownItems == null)
			{
				return;
			}
			foreach (ToolStripItem item in (ArrangedElementCollection)((ToolStripDropDownItem)val).DropDownItems)
			{
				ToolStripItem val2 = item;
				val2.Text = LanguageAndCalendar.Instance.GetValue(val2.Text);
				ChangeControlText(val2);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChangeControlText", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ControlUtilities.cs", 271);
		}
	}
}
