using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Design;

public class SymbolTypeEditor : UITypeEditor
{
	private const int UIntSize = 4;

	private IWindowsFormsEditorService edSvc;

	private static readonly IntPtr HGDI_ERROR = new IntPtr(-1);

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		if (context != null && context.Instance != null && provider != null)
		{
			ref IWindowsFormsEditorService reference = ref edSvc;
			object? service = provider.GetService(typeof(IWindowsFormsEditorService));
			reference = (IWindowsFormsEditorService)((service is IWindowsFormsEditorService) ? service : null);
			string text = null;
			if (value != null)
			{
				text = value.ToString();
			}
			if (edSvc != null)
			{
				TabControl val = new TabControl();
				val.TabPages.Add("Awesome");
				val.TabPages.Add("Material");
				((Control)val).Height = 200;
				ItemPanel itemPanel = new ItemPanel();
				((Control)itemPanel).Font = Symbols.FontAwesome;
				itemPanel.MultiLine = true;
				itemPanel.AutoScroll = true;
				itemPanel.LayoutOrientation = eOrientation.Horizontal;
				itemPanel.SelectedIndexChanged += ItemPanelSelectedIndexChanged;
				((Control)itemPanel).Dock = (DockStyle)5;
				((Control)val.TabPages[0]).Controls.Add((Control)(object)itemPanel);
				SetColors(itemPanel);
				Dictionary<int, bool> fontSupportedChars = GetFontSupportedChars(((Control)itemPanel).Font);
				for (int i = 61440; i <= 61952; i++)
				{
					if (fontSupportedChars.ContainsKey(i))
					{
						ButtonItem buttonItem = new ButtonItem();
						buttonItem.AutoCheckOnClick = true;
						buttonItem.OptionGroup = "sym";
						buttonItem.Text = char.ConvertFromUtf32(i);
						buttonItem.Tooltip = $"{i:x}";
						buttonItem.Tag = i;
						if (buttonItem.Text == text)
						{
							buttonItem.Checked = true;
						}
						itemPanel.Items.Add(buttonItem);
					}
				}
				ItemPanel itemPanel2 = new ItemPanel();
				((Control)itemPanel2).Font = Symbols.FontMaterial;
				itemPanel2.MultiLine = true;
				itemPanel2.AutoScroll = true;
				itemPanel2.LayoutOrientation = eOrientation.Horizontal;
				itemPanel2.SelectedIndexChanged += ItemPanelSelectedIndexChanged;
				((Control)itemPanel2).Dock = (DockStyle)5;
				((Control)val.TabPages[1]).Controls.Add((Control)(object)itemPanel2);
				SetColors(itemPanel2);
				Dictionary<int, bool> fontSupportedChars2 = GetFontSupportedChars(((Control)itemPanel2).Font);
				int num = 0;
				for (int j = 57344; j <= 59658; j++)
				{
					if (fontSupportedChars2.ContainsKey(j))
					{
						ButtonItem buttonItem2 = new ButtonItem();
						buttonItem2.AutoCheckOnClick = true;
						buttonItem2.OptionGroup = "sym";
						buttonItem2.Text = char.ConvertFromUtf32(j);
						buttonItem2.Tooltip = $"{j:x}";
						buttonItem2.Tag = j;
						if (buttonItem2.Text == text)
						{
							buttonItem2.Checked = true;
						}
						itemPanel2.Items.Add(buttonItem2);
						num++;
					}
				}
				eSymbolSet eSymbolSet = eSymbolSet.Awesome;
				string name = context.PropertyDescriptor.Name;
				PropertyInfo property = context.Instance.GetType().GetProperty(name + "Set", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if ((object)property != null && (object)property.PropertyType == typeof(eSymbolSet))
				{
					eSymbolSet = (eSymbolSet)property.GetValue(context.Instance, null);
				}
				if (eSymbolSet == eSymbolSet.Material)
				{
					val.SelectedIndex = 1;
				}
				else
				{
					val.SelectedIndex = 0;
				}
				edSvc.DropDownControl((Control)(object)val);
				itemPanel.SelectedIndexChanged -= ItemPanelSelectedIndexChanged;
				itemPanel2.SelectedIndexChanged -= ItemPanelSelectedIndexChanged;
				if (val.SelectedIndex == 0 && itemPanel.SelectedItem != null)
				{
					if ((object)property != null && eSymbolSet != 0)
					{
						property.SetValue(context.Instance, eSymbolSet.Awesome, null);
					}
					return itemPanel.SelectedItem.Text;
				}
				if (val.SelectedIndex == 1 && itemPanel2.SelectedItem != null)
				{
					if ((object)property != null && eSymbolSet != eSymbolSet.Material)
					{
						property.SetValue(context.Instance, eSymbolSet.Material, null);
					}
					return itemPanel2.SelectedItem.Tag.ToString();
				}
			}
		}
		return value;
	}

	private void SetColors(ItemPanel itemPanel)
	{
		Office2007ColorTable colorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
		ElementStyle elementStyle = (ElementStyle)colorTable.StyleClasses[ElementStyleClassKeys.ItemPanelKey];
		itemPanel.BackgroundStyle.BackColor = elementStyle.BackColor;
		itemPanel.BackgroundStyle.BackColor2 = elementStyle.BackColor2;
	}

	private void ItemPanelSelectedIndexChanged(object sender, EventArgs e)
	{
		if (edSvc != null)
		{
			edSvc.CloseDropDown();
		}
	}

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (context != null && context.Instance != null)
		{
			return (UITypeEditorEditStyle)3;
		}
		return ((UITypeEditor)this).GetEditStyle(context);
	}

	public override bool GetPaintValueSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override void PaintValue(PaintValueEventArgs e)
	{
		string symbol = (string)e.Value;
		ITypeDescriptorContext context = e.Context;
		eSymbolSet symbolSet = eSymbolSet.Awesome;
		if (context != null)
		{
			string name = context.PropertyDescriptor.Name;
			PropertyInfo property = context.Instance.GetType().GetProperty(name + "Set", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if ((object)property != null && (object)property.PropertyType == typeof(eSymbolSet))
			{
				symbolSet = (eSymbolSet)property.GetValue(context.Instance, null);
			}
		}
		Font font = Symbols.GetFont(10f, symbolSet);
		e.Graphics.DrawString(Symbols.GetSymbol(symbol), font, Brushes.Brown, (float)e.Bounds.X, (float)(e.Bounds.Y + 2), StringFormat.GenericDefault);
	}

	[DllImport("gdi32.dll")]
	private static extern uint GetFontUnicodeRanges(IntPtr hdc, IntPtr lpgs);

	[DllImport("gdi32.dll")]
	private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

	private Dictionary<int, bool> GetFontSupportedChars(Font font)
	{
		Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
		IntPtr intPtr = IntPtr.Zero;
		IntPtr intPtr2 = HGDI_ERROR;
		IntPtr intPtr3 = IntPtr.Zero;
		Graphics val = null;
		try
		{
			val = Graphics.FromHwnd(IntPtr.Zero);
			intPtr3 = val.GetHdc();
			IntPtr hgdiobj = font.ToHfont();
			intPtr2 = SelectObject(intPtr3, hgdiobj);
			if (intPtr2 == HGDI_ERROR)
			{
				throw new Exception("Unexpected failure of SelectObject.");
			}
			uint fontUnicodeRanges = GetFontUnicodeRanges(intPtr3, IntPtr.Zero);
			if (fontUnicodeRanges == 0)
			{
				throw new Exception("Unexpected failure of GetFontUnicodeRanges.");
			}
			intPtr = Marshal.AllocHGlobal((int)fontUnicodeRanges);
			if (GetFontUnicodeRanges(intPtr3, intPtr) == 0)
			{
				throw new Exception("Unexpected failure of GetFontUnicodeRanges.");
			}
			int num = 4;
			Marshal.ReadInt32(intPtr, num);
			num += 4;
			Marshal.ReadInt32(intPtr, num);
			num += 4;
			uint num2 = (uint)Marshal.ReadInt32(intPtr, num);
			num += 4;
			for (uint num3 = 0u; num3 < num2; num3++)
			{
				ushort num4 = (ushort)Marshal.ReadInt16(intPtr, num);
				num += Marshal.SizeOf(typeof(ushort));
				ushort num5 = (ushort)Marshal.ReadInt16(intPtr, num);
				num += Marshal.SizeOf(typeof(ushort));
				for (int i = num4; i < num4 + num5; i++)
				{
					dictionary.Add(i, value: true);
				}
			}
			return dictionary;
		}
		finally
		{
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			if (intPtr2 != HGDI_ERROR)
			{
				SelectObject(intPtr3, intPtr2);
			}
			if (val != null)
			{
				if (intPtr3 != IntPtr.Zero)
				{
					val.ReleaseHdc(intPtr3);
				}
				val.Dispose();
			}
		}
	}
}
