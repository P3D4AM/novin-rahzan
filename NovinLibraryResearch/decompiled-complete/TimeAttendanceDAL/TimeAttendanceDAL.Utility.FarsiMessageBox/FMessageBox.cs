using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using TimeAttendanceDAL.Properties;

namespace TimeAttendanceDAL.Utility.FarsiMessageBox;

public class FMessageBox
{
	public enum FMessageBoxIcon
	{
		Error,
		Exclamation,
		Information,
		Question,
		None
	}

	public enum FMessageBoxDefaultButton
	{
		Button1,
		Button2,
		Button3
	}

	public enum FMessageBoxButtons
	{
		OK,
		OKCancel,
		YesNo,
		YesNoCancel,
		CustomeButtons
	}

	[CompilerGenerated]
	private static class _003C_003EO
	{
		public static EventHandler _003C0_003E__FarsiMessegeBox_Click;

		public static FormClosingEventHandler _003C1_003E__Box_FormClosing;

		public static EventHandler _003C2_003E__Box_Shown;
	}

	private static DialogResult result;

	private static List<string> buttonTextList;

	private static Form box;

	private static Button[] boxButtons;

	private static Label boxMessege;

	private static PictureBox boxIcon;

	private static bool buttonIsClicked;

	public static DialogResult Show(string messege)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Expected O, but got Unknown
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Expected O, but got Unknown
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		messege = messege.Replace(".", ".\n");
		box = new Form();
		boxMessege = new Label
		{
			AutoSize = true,
			Text = messege
		};
		((Control)box).BackColor = Color.AliceBlue;
		((Control)box).RightToLeft = (RightToLeft)1;
		box.RightToLeftLayout = true;
		((Control)box).AutoSize = false;
		box.ShowInTaskbar = false;
		string[] array = ((Control)boxMessege).Text.Split(new char[1] { '\n' });
		Size size = TextRenderer.MeasureText(array[0], ((Control)boxMessege).Font);
		int num = size.Width + 120;
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			size = TextRenderer.MeasureText(array2[i], ((Control)boxMessege).Font);
			if (size.Width + 120 > num)
			{
				num = size.Width + 120;
			}
		}
		int num2 = array.Length * size.Height + 125;
		if (num < 210)
		{
			num = 210;
		}
		if (num2 < 130)
		{
			num2 = 130;
		}
		box.Size = new Size(num, num2);
		((Control)box).AutoSize = false;
		box.FormBorderStyle = (FormBorderStyle)5;
		box.StartPosition = (FormStartPosition)4;
		boxMessege.TextAlign = (ContentAlignment)16;
		((Control)boxMessege).RightToLeft = (RightToLeft)1;
		((Control)boxMessege).Font = new Font("Tahoma", 8f);
		((Control)boxMessege).BackColor = Color.Transparent;
		((Control)box).Controls.Add((Control)(object)boxMessege);
		int num3 = (60 - array.Length * size.Height) / 2;
		if (num3 < 10)
		{
			num3 = 10;
		}
		((Control)boxMessege).Location = new Point(15, num3);
		boxButtons = (Button[])(object)new Button[1];
		boxButtons[0] = new Button
		{
			Text = "تایید",
			DialogResult = (DialogResult)1,
			Image = (Image)(object)Resources.ok.ToBitmap(),
			Size = new Size(100, 30)
		};
		((Control)box).Controls.Add((Control)(object)boxButtons[0]);
		((Control)boxButtons[0]).Location = new Point(((Control)box).Width / 2 - 50, ((Control)box).Height - 90);
		((Control)boxButtons[0]).Anchor = (AnchorStyles)2;
		((ButtonBase)boxButtons[0]).UseVisualStyleBackColor = true;
		((Control)boxButtons[0]).Font = new Font("Tahoma", 8f);
		((ButtonBase)boxButtons[0]).ImageAlign = (ContentAlignment)16;
		((Control)boxButtons[0]).Click += FarsiMessegeBox_Click;
		Form obj = box;
		object obj2 = _003C_003EO._003C1_003E__Box_FormClosing;
		if (obj2 == null)
		{
			FormClosingEventHandler val = Box_FormClosing;
			_003C_003EO._003C1_003E__Box_FormClosing = val;
			obj2 = (object)val;
		}
		obj.FormClosing += (FormClosingEventHandler)obj2;
		box.Shown += Box_Shown;
		SystemSounds.Asterisk.Play();
		box.ShowDialog();
		return result;
	}

	public static DialogResult Show(string messege, string title)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Expected O, but got Unknown
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Expected O, but got Unknown
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Expected O, but got Unknown
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Expected O, but got Unknown
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		messege = messege.Replace(".", ".\n");
		box = new Form();
		boxMessege = new Label
		{
			AutoSize = true,
			Text = messege
		};
		((Control)box).BackColor = Color.AliceBlue;
		((Control)box).RightToLeft = (RightToLeft)1;
		box.RightToLeftLayout = true;
		((Control)box).AutoSize = false;
		((Control)box).Text = title;
		box.ShowInTaskbar = false;
		string[] array = ((Control)boxMessege).Text.Split(new char[1] { '\n' });
		Size size = TextRenderer.MeasureText(array[0], ((Control)boxMessege).Font);
		int num = size.Width + 120;
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			size = TextRenderer.MeasureText(array2[i], ((Control)boxMessege).Font);
			if (size.Width + 120 > num)
			{
				num = size.Width + 120;
			}
		}
		int num2 = array.Length * size.Height + 125;
		if (num < 210)
		{
			num = 210;
		}
		if (num2 < 130)
		{
			num2 = 130;
		}
		box.Size = new Size(num, num2);
		((Control)box).AutoSize = false;
		box.FormBorderStyle = (FormBorderStyle)5;
		box.StartPosition = (FormStartPosition)4;
		boxMessege.TextAlign = (ContentAlignment)16;
		((Control)boxMessege).RightToLeft = (RightToLeft)1;
		((Control)boxMessege).Font = new Font("Tahoma", 8f);
		((Control)boxMessege).BackColor = Color.Transparent;
		((Control)box).Controls.Add((Control)(object)boxMessege);
		int num3 = (60 - array.Length * size.Height) / 2;
		if (num3 < 10)
		{
			num3 = 10;
		}
		((Control)boxMessege).Location = new Point(15, num3);
		boxButtons = (Button[])(object)new Button[1];
		boxButtons[0] = new Button
		{
			Text = "تایید",
			DialogResult = (DialogResult)1,
			Image = (Image)(object)Resources.ok.ToBitmap()
		};
		((Control)boxButtons[0]).Size = new Size(100, 30);
		((Control)box).Controls.Add((Control)(object)boxButtons[0]);
		((Control)boxButtons[0]).Location = new Point(((Control)box).Width / 2 - 50, ((Control)box).Height - 90);
		((Control)boxButtons[0]).Anchor = (AnchorStyles)2;
		((ButtonBase)boxButtons[0]).UseVisualStyleBackColor = true;
		((Control)boxButtons[0]).Font = new Font("Tahoma", 8f);
		((ButtonBase)boxButtons[0]).ImageAlign = (ContentAlignment)16;
		((Control)boxButtons[0]).Click += FarsiMessegeBox_Click;
		Form obj = box;
		object obj2 = _003C_003EO._003C1_003E__Box_FormClosing;
		if (obj2 == null)
		{
			FormClosingEventHandler val = Box_FormClosing;
			_003C_003EO._003C1_003E__Box_FormClosing = val;
			obj2 = (object)val;
		}
		obj.FormClosing += (FormClosingEventHandler)obj2;
		box.Shown += Box_Shown;
		SystemSounds.Asterisk.Play();
		box.ShowDialog();
		return result;
	}

	public static DialogResult Show(string messege, string title, FMessageBoxButtons fMessageBoxButtons)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Expected O, but got Unknown
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Expected O, but got Unknown
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Expected O, but got Unknown
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Expected O, but got Unknown
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0591: Unknown result type (might be due to invalid IL or missing references)
		//IL_059c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c3: Expected O, but got Unknown
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_063b: Expected O, but got Unknown
		//IL_07b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_07da: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ea: Expected O, but got Unknown
		//IL_085b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0865: Expected O, but got Unknown
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_0460: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Expected O, but got Unknown
		//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fe: Expected O, but got Unknown
		//IL_0676: Unknown result type (might be due to invalid IL or missing references)
		//IL_067b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0686: Unknown result type (might be due to invalid IL or missing references)
		//IL_068d: Unknown result type (might be due to invalid IL or missing references)
		//IL_069d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ad: Expected O, but got Unknown
		//IL_071b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0725: Expected O, but got Unknown
		//IL_08a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d7: Expected O, but got Unknown
		//IL_0945: Unknown result type (might be due to invalid IL or missing references)
		//IL_094f: Expected O, but got Unknown
		//IL_0542: Unknown result type (might be due to invalid IL or missing references)
		//IL_0548: Unknown result type (might be due to invalid IL or missing references)
		//IL_0769: Unknown result type (might be due to invalid IL or missing references)
		//IL_076f: Unknown result type (might be due to invalid IL or missing references)
		//IL_098a: Unknown result type (might be due to invalid IL or missing references)
		//IL_098f: Unknown result type (might be due to invalid IL or missing references)
		//IL_099a: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c1: Expected O, but got Unknown
		//IL_0a2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a39: Expected O, but got Unknown
		//IL_0a7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a83: Unknown result type (might be due to invalid IL or missing references)
		messege = messege.Replace(".", ".\n");
		box = new Form();
		boxMessege = new Label
		{
			AutoSize = true,
			Text = messege
		};
		((Control)box).BackColor = Color.AliceBlue;
		((Control)box).RightToLeft = (RightToLeft)1;
		box.RightToLeftLayout = true;
		((Control)box).AutoSize = false;
		((Control)box).Text = title;
		box.ShowInTaskbar = false;
		((Control)box).AutoSize = false;
		box.FormBorderStyle = (FormBorderStyle)5;
		box.StartPosition = (FormStartPosition)4;
		boxMessege.TextAlign = (ContentAlignment)16;
		((Control)boxMessege).RightToLeft = (RightToLeft)1;
		((Control)boxMessege).Font = new Font("Tahoma", 8f);
		((Control)boxMessege).BackColor = Color.Transparent;
		((Control)box).Controls.Add((Control)(object)boxMessege);
		Form obj = box;
		object obj2 = _003C_003EO._003C1_003E__Box_FormClosing;
		if (obj2 == null)
		{
			FormClosingEventHandler val = Box_FormClosing;
			_003C_003EO._003C1_003E__Box_FormClosing = val;
			obj2 = (object)val;
		}
		obj.FormClosing += (FormClosingEventHandler)obj2;
		box.Shown += Box_Shown;
		string[] array = ((Control)boxMessege).Text.Split(new char[1] { '\n' });
		Size size = TextRenderer.MeasureText(array[0], ((Control)boxMessege).Font);
		int num = size.Width + 120;
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			size = TextRenderer.MeasureText(array2[i], ((Control)boxMessege).Font);
			if (size.Width + 120 > num)
			{
				num = size.Width + 120;
			}
		}
		int num2 = array.Length * size.Height + 125;
		switch (fMessageBoxButtons)
		{
		case FMessageBoxButtons.OK:
			if (num < 210)
			{
				num = 210;
			}
			if (num2 < 130)
			{
				num2 = 130;
			}
			box.Size = new Size(num, num2);
			boxButtons = (Button[])(object)new Button[1];
			boxButtons[0] = new Button
			{
				Text = "تایید",
				DialogResult = (DialogResult)1,
				Image = (Image)(object)Resources.ok.ToBitmap(),
				Size = new Size(100, 30)
			};
			((Control)box).Controls.Add((Control)(object)boxButtons[0]);
			((Control)boxButtons[0]).Location = new Point(((Control)box).Width / 2 - 50, ((Control)box).Height - 90);
			((Control)boxButtons[0]).Anchor = (AnchorStyles)2;
			((ButtonBase)boxButtons[0]).UseVisualStyleBackColor = true;
			((Control)boxButtons[0]).Font = new Font("Tahoma", 8f);
			((ButtonBase)boxButtons[0]).ImageAlign = (ContentAlignment)16;
			((Control)boxButtons[0]).Click += FarsiMessegeBox_Click;
			SystemSounds.Asterisk.Play();
			box.ShowDialog();
			return result;
		case FMessageBoxButtons.OKCancel:
			if (num < 300)
			{
				num = 300;
			}
			if (num2 < 130)
			{
				num2 = 130;
			}
			box.Size = new Size(num, num2);
			boxButtons = (Button[])(object)new Button[2];
			boxButtons[0] = new Button
			{
				Text = "تایید",
				DialogResult = (DialogResult)1,
				Image = (Image)(object)Resources.ok.ToBitmap()
			};
			((Control)boxButtons[0]).Size = new Size(100, 30);
			((Control)box).Controls.Add((Control)(object)boxButtons[0]);
			((Control)boxButtons[0]).Location = new Point(((Control)box).Width / 2 - 110, ((Control)box).Height - 90);
			((Control)boxButtons[0]).Anchor = (AnchorStyles)2;
			((ButtonBase)boxButtons[0]).UseVisualStyleBackColor = true;
			((Control)boxButtons[0]).Font = new Font("Tahoma", 8f);
			((ButtonBase)boxButtons[0]).ImageAlign = (ContentAlignment)16;
			((Control)boxButtons[0]).Click += FarsiMessegeBox_Click;
			boxButtons[1] = new Button
			{
				Text = "لغو",
				DialogResult = (DialogResult)2,
				Image = (Image)(object)Resources.cancel.ToBitmap()
			};
			((Control)boxButtons[1]).Size = new Size(100, 30);
			((Control)box).Controls.Add((Control)(object)boxButtons[1]);
			((Control)boxButtons[1]).Location = new Point(((Control)box).Width / 2 + 10, ((Control)box).Height - 90);
			((Control)boxButtons[1]).Anchor = (AnchorStyles)2;
			((ButtonBase)boxButtons[1]).UseVisualStyleBackColor = true;
			((Control)boxButtons[1]).Font = new Font("Tahoma", 8f);
			((ButtonBase)boxButtons[1]).ImageAlign = (ContentAlignment)16;
			((Control)boxButtons[1]).Click += FarsiMessegeBox_Click;
			SystemSounds.Asterisk.Play();
			box.ShowDialog();
			return result;
		case FMessageBoxButtons.YesNo:
			if (num < 300)
			{
				num = 300;
			}
			if (num2 < 130)
			{
				num2 = 130;
			}
			box.Size = new Size(num, num2);
			boxButtons = (Button[])(object)new Button[2];
			boxButtons[0] = new Button
			{
				Text = "بله",
				DialogResult = (DialogResult)6,
				Image = (Image)(object)Resources.ok.ToBitmap(),
				Size = new Size(100, 30)
			};
			((Control)box).Controls.Add((Control)(object)boxButtons[0]);
			((Control)boxButtons[0]).Location = new Point(((Control)box).Width / 2 - 110, ((Control)box).Height - 90);
			((Control)boxButtons[0]).Anchor = (AnchorStyles)2;
			((ButtonBase)boxButtons[0]).UseVisualStyleBackColor = true;
			((Control)boxButtons[0]).Font = new Font("Tahoma", 8f);
			((ButtonBase)boxButtons[0]).ImageAlign = (ContentAlignment)16;
			((Control)boxButtons[0]).Click += FarsiMessegeBox_Click;
			boxButtons[1] = new Button
			{
				Text = "خیر",
				DialogResult = (DialogResult)7,
				Image = (Image)(object)Resources.cancel.ToBitmap(),
				Size = new Size(100, 30)
			};
			((Control)box).Controls.Add((Control)(object)boxButtons[1]);
			((Control)boxButtons[1]).Location = new Point(((Control)box).Width / 2 + 10, ((Control)box).Height - 90);
			((Control)boxButtons[1]).Anchor = (AnchorStyles)2;
			((ButtonBase)boxButtons[1]).UseVisualStyleBackColor = true;
			((Control)boxButtons[1]).Font = new Font("Tahoma", 8f);
			((ButtonBase)boxButtons[1]).ImageAlign = (ContentAlignment)16;
			((Control)boxButtons[1]).Click += FarsiMessegeBox_Click;
			SystemSounds.Asterisk.Play();
			box.ShowDialog();
			return result;
		case FMessageBoxButtons.YesNoCancel:
			if (num < 370)
			{
				num = 370;
			}
			if (num2 < 130)
			{
				num2 = 130;
			}
			box.Size = new Size(num, num2);
			boxButtons = (Button[])(object)new Button[3];
			boxButtons[0] = new Button
			{
				Text = "بله",
				DialogResult = (DialogResult)6,
				Image = (Image)(object)Resources.ok.ToBitmap(),
				Size = new Size(100, 30)
			};
			((Control)box).Controls.Add((Control)(object)boxButtons[0]);
			((Control)boxButtons[0]).Location = new Point(((Control)box).Width / 2 - 160, ((Control)box).Height - 90);
			((Control)boxButtons[0]).Anchor = (AnchorStyles)2;
			((ButtonBase)boxButtons[0]).UseVisualStyleBackColor = true;
			((Control)boxButtons[0]).Font = new Font("Tahoma", 8f);
			((ButtonBase)boxButtons[0]).ImageAlign = (ContentAlignment)16;
			((Control)boxButtons[0]).Click += FarsiMessegeBox_Click;
			boxButtons[1] = new Button
			{
				Text = "خیر",
				DialogResult = (DialogResult)7,
				Image = (Image)(object)Resources.cancel.ToBitmap(),
				Size = new Size(100, 30)
			};
			((Control)box).Controls.Add((Control)(object)boxButtons[1]);
			((Control)boxButtons[1]).Location = new Point(((Control)box).Width / 2 - 50, ((Control)box).Height - 90);
			((Control)boxButtons[1]).Anchor = (AnchorStyles)2;
			((ButtonBase)boxButtons[1]).UseVisualStyleBackColor = true;
			((Control)boxButtons[1]).Font = new Font("Tahoma", 8f);
			((ButtonBase)boxButtons[1]).ImageAlign = (ContentAlignment)16;
			((Control)boxButtons[1]).Click += FarsiMessegeBox_Click;
			boxButtons[2] = new Button
			{
				Text = "لغو",
				DialogResult = (DialogResult)2,
				Image = (Image)(object)Resources.cancel.ToBitmap(),
				Size = new Size(100, 30)
			};
			((Control)box).Controls.Add((Control)(object)boxButtons[2]);
			((Control)boxButtons[2]).Location = new Point(((Control)box).Width / 2 + 60, ((Control)box).Height - 90);
			((Control)boxButtons[2]).Anchor = (AnchorStyles)2;
			((ButtonBase)boxButtons[2]).UseVisualStyleBackColor = true;
			((Control)boxButtons[2]).Font = new Font("Tahoma", 8f);
			((ButtonBase)boxButtons[2]).ImageAlign = (ContentAlignment)16;
			((Control)boxButtons[2]).Click += FarsiMessegeBox_Click;
			SystemSounds.Asterisk.Play();
			box.ShowDialog();
			return result;
		default:
		{
			int num3 = (60 - array.Length * size.Height) / 2;
			if (num3 < 10)
			{
				num3 = 10;
			}
			((Control)boxMessege).Location = new Point(15, num3);
			return (DialogResult)2;
		}
		}
	}

	public static DialogResult Show(string messege, string title, FMessageBoxButtons fMessageBoxButtons, FMessageBoxIcon fMessageBoxIcon)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Expected O, but got Unknown
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Expected O, but got Unknown
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Expected O, but got Unknown
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0384: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0396: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Expected O, but got Unknown
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Expected O, but got Unknown
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_0595: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c7: Expected O, but got Unknown
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_063f: Expected O, but got Unknown
		//IL_07a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d8: Expected O, but got Unknown
		//IL_0849: Unknown result type (might be due to invalid IL or missing references)
		//IL_0853: Expected O, but got Unknown
		//IL_0469: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Unknown result type (might be due to invalid IL or missing references)
		//IL_0480: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a0: Expected O, but got Unknown
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0518: Expected O, but got Unknown
		//IL_067a: Unknown result type (might be due to invalid IL or missing references)
		//IL_067f: Unknown result type (might be due to invalid IL or missing references)
		//IL_068a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0691: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b1: Expected O, but got Unknown
		//IL_071f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0729: Expected O, but got Unknown
		//IL_088e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0893: Unknown result type (might be due to invalid IL or missing references)
		//IL_089e: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c5: Expected O, but got Unknown
		//IL_0933: Unknown result type (might be due to invalid IL or missing references)
		//IL_093d: Expected O, but got Unknown
		//IL_0adb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0978: Unknown result type (might be due to invalid IL or missing references)
		//IL_097d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0988: Unknown result type (might be due to invalid IL or missing references)
		//IL_098f: Unknown result type (might be due to invalid IL or missing references)
		//IL_099f: Unknown result type (might be due to invalid IL or missing references)
		//IL_09af: Expected O, but got Unknown
		//IL_0a1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a27: Expected O, but got Unknown
		messege = messege.Replace(".", ".\n");
		box = new Form();
		boxMessege = new Label();
		boxIcon = new PictureBox
		{
			BackgroundImageLayout = (ImageLayout)2,
			Size = new Size(50, 50)
		};
		((Control)box).Controls.Add((Control)(object)boxIcon);
		((Control)boxMessege).AutoSize = true;
		((Control)boxMessege).Text = messege;
		((Control)box).BackColor = Color.AliceBlue;
		((Control)box).RightToLeft = (RightToLeft)1;
		box.RightToLeftLayout = true;
		((Control)box).AutoSize = false;
		((Control)box).Text = title;
		box.ShowInTaskbar = false;
		((Control)box).AutoSize = false;
		box.FormBorderStyle = (FormBorderStyle)5;
		box.StartPosition = (FormStartPosition)4;
		boxMessege.TextAlign = (ContentAlignment)16;
		((Control)boxMessege).RightToLeft = (RightToLeft)1;
		((Control)boxMessege).Font = new Font("Tahoma", 8f);
		((Control)boxMessege).BackColor = Color.Transparent;
		((Control)box).Controls.Add((Control)(object)boxMessege);
		Form obj = box;
		object obj2 = _003C_003EO._003C1_003E__Box_FormClosing;
		if (obj2 == null)
		{
			FormClosingEventHandler val = Box_FormClosing;
			_003C_003EO._003C1_003E__Box_FormClosing = val;
			obj2 = (object)val;
		}
		obj.FormClosing += (FormClosingEventHandler)obj2;
		box.Shown += Box_Shown;
		string[] array = ((Control)boxMessege).Text.Split(new char[1] { '\n' });
		Size size = TextRenderer.MeasureText(array[0], ((Control)boxMessege).Font);
		int num = size.Width + 120;
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			size = TextRenderer.MeasureText(array2[i], ((Control)boxMessege).Font);
			if (size.Width + 120 > num)
			{
				num = size.Width + 120;
			}
		}
		int num2 = array.Length * size.Height + 125;
		switch (fMessageBoxButtons)
		{
		case FMessageBoxButtons.OK:
			if (num < 260)
			{
				num = 260;
			}
			if (num2 < 150)
			{
				num2 = 150;
			}
			box.Size = new Size(num, num2);
			boxButtons = (Button[])(object)new Button[1];
			boxButtons[0] = new Button
			{
				Text = "تایید",
				DialogResult = (DialogResult)1,
				Image = (Image)(object)Resources.ok.ToBitmap(),
				Size = new Size(100, 30)
			};
			((Control)box).Controls.Add((Control)(object)boxButtons[0]);
			((Control)boxButtons[0]).Location = new Point(((Control)box).Width / 2 - 50, ((Control)box).Height - 90);
			((Control)boxButtons[0]).Anchor = (AnchorStyles)2;
			((ButtonBase)boxButtons[0]).UseVisualStyleBackColor = true;
			((Control)boxButtons[0]).Font = new Font("Tahoma", 8f);
			((ButtonBase)boxButtons[0]).ImageAlign = (ContentAlignment)16;
			((Control)boxButtons[0]).Click += FarsiMessegeBox_Click;
			break;
		case FMessageBoxButtons.OKCancel:
			if (num < 300)
			{
				num = 300;
			}
			if (num2 < 150)
			{
				num2 = 150;
			}
			box.Size = new Size(num, num2);
			boxButtons = (Button[])(object)new Button[2];
			boxButtons[0] = new Button
			{
				Text = "تایید",
				DialogResult = (DialogResult)1,
				Image = (Image)(object)Resources.ok.ToBitmap(),
				Size = new Size(100, 30)
			};
			((Control)box).Controls.Add((Control)(object)boxButtons[0]);
			((Control)boxButtons[0]).Location = new Point(((Control)box).Width / 2 - 110, ((Control)box).Height - 90);
			((Control)boxButtons[0]).Anchor = (AnchorStyles)2;
			((ButtonBase)boxButtons[0]).UseVisualStyleBackColor = true;
			((Control)boxButtons[0]).Font = new Font("Tahoma", 8f);
			((ButtonBase)boxButtons[0]).ImageAlign = (ContentAlignment)16;
			((Control)boxButtons[0]).Click += FarsiMessegeBox_Click;
			boxButtons[1] = new Button
			{
				Text = "لغو",
				DialogResult = (DialogResult)2,
				Image = (Image)(object)Resources.cancel.ToBitmap(),
				Size = new Size(100, 30)
			};
			((Control)box).Controls.Add((Control)(object)boxButtons[1]);
			((Control)boxButtons[1]).Location = new Point(((Control)box).Width / 2 + 10, ((Control)box).Height - 90);
			((Control)boxButtons[1]).Anchor = (AnchorStyles)2;
			((ButtonBase)boxButtons[1]).UseVisualStyleBackColor = true;
			((Control)boxButtons[1]).Font = new Font("Tahoma", 8f);
			((ButtonBase)boxButtons[1]).ImageAlign = (ContentAlignment)16;
			((Control)boxButtons[1]).Click += FarsiMessegeBox_Click;
			break;
		case FMessageBoxButtons.YesNo:
			if (num < 300)
			{
				num = 300;
			}
			if (num2 < 150)
			{
				num2 = 150;
			}
			box.Size = new Size(num, num2);
			boxButtons = (Button[])(object)new Button[2];
			boxButtons[0] = new Button
			{
				Text = "بله",
				DialogResult = (DialogResult)6,
				Image = (Image)(object)Resources.ok.ToBitmap(),
				Size = new Size(100, 30)
			};
			((Control)box).Controls.Add((Control)(object)boxButtons[0]);
			((Control)boxButtons[0]).Location = new Point(((Control)box).Width / 2 - 110, ((Control)box).Height - 90);
			((Control)boxButtons[0]).Anchor = (AnchorStyles)2;
			((ButtonBase)boxButtons[0]).UseVisualStyleBackColor = true;
			((Control)boxButtons[0]).Font = new Font("Tahoma", 8f);
			((ButtonBase)boxButtons[0]).ImageAlign = (ContentAlignment)16;
			((Control)boxButtons[0]).Click += FarsiMessegeBox_Click;
			boxButtons[1] = new Button
			{
				Text = "خیر",
				DialogResult = (DialogResult)7,
				Image = (Image)(object)Resources.cancel.ToBitmap(),
				Size = new Size(100, 30)
			};
			((Control)box).Controls.Add((Control)(object)boxButtons[1]);
			((Control)boxButtons[1]).Location = new Point(((Control)box).Width / 2 + 10, ((Control)box).Height - 90);
			((Control)boxButtons[1]).Anchor = (AnchorStyles)2;
			((ButtonBase)boxButtons[1]).UseVisualStyleBackColor = true;
			((Control)boxButtons[1]).Font = new Font("Tahoma", 8f);
			((ButtonBase)boxButtons[1]).ImageAlign = (ContentAlignment)16;
			((Control)boxButtons[1]).Click += FarsiMessegeBox_Click;
			break;
		case FMessageBoxButtons.YesNoCancel:
			if (num < 370)
			{
				num = 370;
			}
			if (num2 < 150)
			{
				num2 = 150;
			}
			box.Size = new Size(num, num2);
			boxButtons = (Button[])(object)new Button[3];
			boxButtons[0] = new Button
			{
				Text = "بله",
				DialogResult = (DialogResult)6,
				Image = (Image)(object)Resources.ok.ToBitmap(),
				Size = new Size(100, 30)
			};
			((Control)box).Controls.Add((Control)(object)boxButtons[0]);
			((Control)boxButtons[0]).Location = new Point(((Control)box).Width / 2 - 160, ((Control)box).Height - 90);
			((Control)boxButtons[0]).Anchor = (AnchorStyles)2;
			((ButtonBase)boxButtons[0]).UseVisualStyleBackColor = true;
			((Control)boxButtons[0]).Font = new Font("Tahoma", 8f);
			((ButtonBase)boxButtons[0]).ImageAlign = (ContentAlignment)16;
			((Control)boxButtons[0]).Click += FarsiMessegeBox_Click;
			boxButtons[1] = new Button
			{
				Text = "خیر",
				DialogResult = (DialogResult)7,
				Image = (Image)(object)Resources.cancel.ToBitmap(),
				Size = new Size(100, 30)
			};
			((Control)box).Controls.Add((Control)(object)boxButtons[1]);
			((Control)boxButtons[1]).Location = new Point(((Control)box).Width / 2 - 50, ((Control)box).Height - 90);
			((Control)boxButtons[1]).Anchor = (AnchorStyles)2;
			((ButtonBase)boxButtons[1]).UseVisualStyleBackColor = true;
			((Control)boxButtons[1]).Font = new Font("Tahoma", 8f);
			((ButtonBase)boxButtons[1]).ImageAlign = (ContentAlignment)16;
			((Control)boxButtons[1]).Click += FarsiMessegeBox_Click;
			boxButtons[2] = new Button
			{
				Text = "لغو",
				DialogResult = (DialogResult)2,
				Image = (Image)(object)Resources.cancel.ToBitmap(),
				Size = new Size(100, 30)
			};
			((Control)box).Controls.Add((Control)(object)boxButtons[2]);
			((Control)boxButtons[2]).Location = new Point(((Control)box).Width / 2 + 60, ((Control)box).Height - 90);
			((Control)boxButtons[2]).Anchor = (AnchorStyles)2;
			((ButtonBase)boxButtons[2]).UseVisualStyleBackColor = true;
			((Control)boxButtons[2]).Font = new Font("Tahoma", 8f);
			((ButtonBase)boxButtons[2]).ImageAlign = (ContentAlignment)16;
			((Control)boxButtons[2]).Click += FarsiMessegeBox_Click;
			break;
		}
		int num3 = (60 - array.Length * size.Height) / 2;
		if (num3 < 10)
		{
			num3 = 10;
		}
		((Control)boxIcon).Location = new Point(5, 10);
		((Control)boxMessege).Location = new Point(60, num3);
		switch (fMessageBoxIcon)
		{
		case FMessageBoxIcon.Error:
			boxIcon.Image = (Image)(object)Resources.error.ToBitmap();
			PlaySound(fMessageBoxIcon);
			box.ShowDialog();
			return result;
		case FMessageBoxIcon.Exclamation:
			boxIcon.Image = (Image)(object)Resources.exclamtion.ToBitmap();
			PlaySound(fMessageBoxIcon);
			box.ShowDialog();
			return result;
		case FMessageBoxIcon.Information:
			boxIcon.Image = (Image)(object)Resources.information.ToBitmap();
			PlaySound(fMessageBoxIcon);
			box.ShowDialog();
			return result;
		case FMessageBoxIcon.Question:
			boxIcon.Image = (Image)(object)Resources.question.ToBitmap();
			PlaySound(fMessageBoxIcon);
			box.ShowDialog();
			return result;
		case FMessageBoxIcon.None:
			boxIcon.Image = null;
			box.ShowDialog();
			return result;
		default:
			return (DialogResult)2;
		}
	}

	public static DialogResult Show(string messege, string title, FMessageBoxButtons fMessageBoxButtons, FMessageBoxIcon fMessageBoxIcon, FMessageBoxDefaultButton fMessageBoxDefaultButton)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Expected O, but got Unknown
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Expected O, but got Unknown
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Expected O, but got Unknown
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Expected O, but got Unknown
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0446: Expected O, but got Unknown
		//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0605: Unknown result type (might be due to invalid IL or missing references)
		//IL_060c: Unknown result type (might be due to invalid IL or missing references)
		//IL_061c: Unknown result type (might be due to invalid IL or missing references)
		//IL_062c: Expected O, but got Unknown
		//IL_069a: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a4: Expected O, but got Unknown
		//IL_0853: Unknown result type (might be due to invalid IL or missing references)
		//IL_0858: Unknown result type (might be due to invalid IL or missing references)
		//IL_0863: Unknown result type (might be due to invalid IL or missing references)
		//IL_086a: Unknown result type (might be due to invalid IL or missing references)
		//IL_087a: Unknown result type (might be due to invalid IL or missing references)
		//IL_088a: Expected O, but got Unknown
		//IL_08fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0905: Expected O, but got Unknown
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Expected O, but got Unknown
		//IL_0526: Unknown result type (might be due to invalid IL or missing references)
		//IL_0530: Expected O, but got Unknown
		//IL_06df: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0706: Unknown result type (might be due to invalid IL or missing references)
		//IL_0716: Expected O, but got Unknown
		//IL_0784: Unknown result type (might be due to invalid IL or missing references)
		//IL_078e: Expected O, but got Unknown
		//IL_0940: Unknown result type (might be due to invalid IL or missing references)
		//IL_0945: Unknown result type (might be due to invalid IL or missing references)
		//IL_0950: Unknown result type (might be due to invalid IL or missing references)
		//IL_0957: Unknown result type (might be due to invalid IL or missing references)
		//IL_0967: Unknown result type (might be due to invalid IL or missing references)
		//IL_0977: Expected O, but got Unknown
		//IL_09e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ef: Expected O, but got Unknown
		//IL_0bf3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c24: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a61: Expected O, but got Unknown
		//IL_0acf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad9: Expected O, but got Unknown
		messege = messege.Replace(".", ".\n");
		box = new Form();
		boxMessege = new Label();
		boxIcon = new PictureBox
		{
			BackgroundImageLayout = (ImageLayout)2,
			Size = new Size(50, 50)
		};
		((Control)box).Controls.Add((Control)(object)boxIcon);
		((Control)boxMessege).AutoSize = true;
		((Control)boxMessege).Text = messege;
		((Control)box).BackColor = Color.AliceBlue;
		((Control)box).RightToLeft = (RightToLeft)1;
		box.RightToLeftLayout = true;
		((Control)box).AutoSize = false;
		((Control)box).Text = title;
		box.ShowInTaskbar = false;
		((Control)box).AutoSize = false;
		box.FormBorderStyle = (FormBorderStyle)5;
		box.StartPosition = (FormStartPosition)4;
		boxMessege.TextAlign = (ContentAlignment)16;
		((Control)boxMessege).RightToLeft = (RightToLeft)1;
		((Control)boxMessege).Font = new Font("Tahoma", 8f);
		((Control)boxMessege).BackColor = Color.Transparent;
		((Control)box).Controls.Add((Control)(object)boxMessege);
		Form obj = box;
		object obj2 = _003C_003EO._003C1_003E__Box_FormClosing;
		if (obj2 == null)
		{
			FormClosingEventHandler val = Box_FormClosing;
			_003C_003EO._003C1_003E__Box_FormClosing = val;
			obj2 = (object)val;
		}
		obj.FormClosing += (FormClosingEventHandler)obj2;
		box.Shown += Box_Shown;
		string[] array = ((Control)boxMessege).Text.Split(new char[1] { '\n' });
		Size size = TextRenderer.MeasureText(array[0], ((Control)boxMessege).Font);
		int num = size.Width + 120;
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			size = TextRenderer.MeasureText(array2[i], ((Control)boxMessege).Font);
			if (size.Width + 120 > num)
			{
				num = size.Width + 120;
			}
		}
		int num2 = array.Length * size.Height + 125;
		switch (fMessageBoxButtons)
		{
		case FMessageBoxButtons.OK:
			if (num < 260)
			{
				num = 260;
			}
			if (num2 < 150)
			{
				num2 = 150;
			}
			box.Size = new Size(num, num2);
			boxButtons = (Button[])(object)new Button[1];
			boxButtons[0] = new Button
			{
				Text = "تایید",
				DialogResult = (DialogResult)1,
				Image = (Image)(object)Resources.ok.ToBitmap(),
				Size = new Size(100, 30)
			};
			((Control)box).Controls.Add((Control)(object)boxButtons[0]);
			((Control)boxButtons[0]).Location = new Point(((Control)box).Width / 2 - 50, ((Control)box).Height - 90);
			((Control)boxButtons[0]).Anchor = (AnchorStyles)2;
			((ButtonBase)boxButtons[0]).UseVisualStyleBackColor = true;
			((Control)boxButtons[0]).Font = new Font("Tahoma", 8f);
			((ButtonBase)boxButtons[0]).ImageAlign = (ContentAlignment)16;
			((Control)boxButtons[0]).Click += FarsiMessegeBox_Click;
			if (fMessageBoxDefaultButton == FMessageBoxDefaultButton.Button1)
			{
				box.AcceptButton = (IButtonControl)(object)boxButtons[0];
			}
			break;
		case FMessageBoxButtons.OKCancel:
			if (num < 300)
			{
				num = 300;
			}
			if (num2 < 150)
			{
				num2 = 150;
			}
			box.Size = new Size(num, num2);
			boxButtons = (Button[])(object)new Button[2];
			boxButtons[0] = new Button
			{
				Text = "تایید",
				DialogResult = (DialogResult)1,
				Image = (Image)(object)Resources.ok.ToBitmap(),
				Size = new Size(100, 30)
			};
			((Control)box).Controls.Add((Control)(object)boxButtons[0]);
			((Control)boxButtons[0]).Location = new Point(((Control)box).Width / 2 - 110, ((Control)box).Height - 90);
			((Control)boxButtons[0]).Anchor = (AnchorStyles)2;
			((ButtonBase)boxButtons[0]).UseVisualStyleBackColor = true;
			((Control)boxButtons[0]).Font = new Font("Tahoma", 8f);
			((ButtonBase)boxButtons[0]).ImageAlign = (ContentAlignment)16;
			((Control)boxButtons[0]).Click += FarsiMessegeBox_Click;
			boxButtons[1] = new Button
			{
				Text = "لغو",
				DialogResult = (DialogResult)2,
				Image = (Image)(object)Resources.cancel.ToBitmap(),
				Size = new Size(100, 30)
			};
			((Control)box).Controls.Add((Control)(object)boxButtons[1]);
			((Control)boxButtons[1]).Location = new Point(((Control)box).Width / 2 + 10, ((Control)box).Height - 90);
			((Control)boxButtons[1]).Anchor = (AnchorStyles)2;
			((ButtonBase)boxButtons[1]).UseVisualStyleBackColor = true;
			((Control)boxButtons[1]).Font = new Font("Tahoma", 8f);
			((ButtonBase)boxButtons[1]).ImageAlign = (ContentAlignment)16;
			((Control)boxButtons[1]).Click += FarsiMessegeBox_Click;
			switch (fMessageBoxDefaultButton)
			{
			case FMessageBoxDefaultButton.Button1:
				box.AcceptButton = (IButtonControl)(object)boxButtons[0];
				((Control)boxButtons[0]).Select();
				break;
			case FMessageBoxDefaultButton.Button2:
				box.AcceptButton = (IButtonControl)(object)boxButtons[1];
				((Control)boxButtons[1]).Select();
				break;
			}
			break;
		case FMessageBoxButtons.YesNo:
			if (num < 300)
			{
				num = 300;
			}
			if (num2 < 150)
			{
				num2 = 150;
			}
			box.Size = new Size(num, num2);
			boxButtons = (Button[])(object)new Button[2];
			boxButtons[0] = new Button
			{
				Text = "بله",
				DialogResult = (DialogResult)6,
				Image = (Image)(object)Resources.ok.ToBitmap(),
				Size = new Size(100, 30)
			};
			((Control)box).Controls.Add((Control)(object)boxButtons[0]);
			((Control)boxButtons[0]).Location = new Point(((Control)box).Width / 2 - 110, ((Control)box).Height - 90);
			((Control)boxButtons[0]).Anchor = (AnchorStyles)2;
			((ButtonBase)boxButtons[0]).UseVisualStyleBackColor = true;
			((Control)boxButtons[0]).Font = new Font("Tahoma", 8f);
			((ButtonBase)boxButtons[0]).ImageAlign = (ContentAlignment)16;
			((Control)boxButtons[0]).Click += FarsiMessegeBox_Click;
			boxButtons[1] = new Button
			{
				Text = "خیر",
				DialogResult = (DialogResult)7,
				Image = (Image)(object)Resources.cancel.ToBitmap(),
				Size = new Size(100, 30)
			};
			((Control)box).Controls.Add((Control)(object)boxButtons[1]);
			((Control)boxButtons[1]).Location = new Point(((Control)box).Width / 2 + 10, ((Control)box).Height - 90);
			((Control)boxButtons[1]).Anchor = (AnchorStyles)2;
			((ButtonBase)boxButtons[1]).UseVisualStyleBackColor = true;
			((Control)boxButtons[1]).Font = new Font("Tahoma", 8f);
			((ButtonBase)boxButtons[1]).ImageAlign = (ContentAlignment)16;
			((Control)boxButtons[1]).Click += FarsiMessegeBox_Click;
			switch (fMessageBoxDefaultButton)
			{
			case FMessageBoxDefaultButton.Button1:
				box.AcceptButton = (IButtonControl)(object)boxButtons[0];
				((Control)boxButtons[0]).Select();
				break;
			case FMessageBoxDefaultButton.Button2:
				box.AcceptButton = (IButtonControl)(object)boxButtons[1];
				((Control)boxButtons[1]).Select();
				break;
			}
			break;
		case FMessageBoxButtons.YesNoCancel:
			if (num < 370)
			{
				num = 370;
			}
			if (num2 < 150)
			{
				num2 = 150;
			}
			box.Size = new Size(num, num2);
			boxButtons = (Button[])(object)new Button[3];
			boxButtons[0] = new Button
			{
				Text = "بله",
				DialogResult = (DialogResult)6,
				Image = (Image)(object)Resources.ok.ToBitmap(),
				Size = new Size(100, 30)
			};
			((Control)box).Controls.Add((Control)(object)boxButtons[0]);
			((Control)boxButtons[0]).Location = new Point(((Control)box).Width / 2 - 160, ((Control)box).Height - 90);
			((Control)boxButtons[0]).Anchor = (AnchorStyles)2;
			((ButtonBase)boxButtons[0]).UseVisualStyleBackColor = true;
			((Control)boxButtons[0]).Font = new Font("Tahoma", 8f);
			((ButtonBase)boxButtons[0]).ImageAlign = (ContentAlignment)16;
			((Control)boxButtons[0]).Click += FarsiMessegeBox_Click;
			boxButtons[1] = new Button
			{
				Text = "خیر",
				DialogResult = (DialogResult)7,
				Image = (Image)(object)Resources.cancel.ToBitmap(),
				Size = new Size(100, 30)
			};
			((Control)box).Controls.Add((Control)(object)boxButtons[1]);
			((Control)boxButtons[1]).Location = new Point(((Control)box).Width / 2 - 50, ((Control)box).Height - 90);
			((Control)boxButtons[1]).Anchor = (AnchorStyles)2;
			((ButtonBase)boxButtons[1]).UseVisualStyleBackColor = true;
			((Control)boxButtons[1]).Font = new Font("Tahoma", 8f);
			((ButtonBase)boxButtons[1]).ImageAlign = (ContentAlignment)16;
			((Control)boxButtons[1]).Click += FarsiMessegeBox_Click;
			boxButtons[2] = new Button
			{
				Text = "لغو",
				DialogResult = (DialogResult)2,
				Image = (Image)(object)Resources.cancel.ToBitmap(),
				Size = new Size(100, 30)
			};
			((Control)box).Controls.Add((Control)(object)boxButtons[2]);
			((Control)boxButtons[2]).Location = new Point(((Control)box).Width / 2 + 60, ((Control)box).Height - 90);
			((Control)boxButtons[2]).Anchor = (AnchorStyles)2;
			((ButtonBase)boxButtons[2]).UseVisualStyleBackColor = true;
			((Control)boxButtons[2]).Font = new Font("Tahoma", 8f);
			((ButtonBase)boxButtons[2]).ImageAlign = (ContentAlignment)16;
			((Control)boxButtons[2]).Click += FarsiMessegeBox_Click;
			switch (fMessageBoxDefaultButton)
			{
			case FMessageBoxDefaultButton.Button1:
				box.AcceptButton = (IButtonControl)(object)boxButtons[0];
				((Control)boxButtons[0]).Select();
				break;
			case FMessageBoxDefaultButton.Button2:
				box.AcceptButton = (IButtonControl)(object)boxButtons[1];
				((Control)boxButtons[1]).Select();
				break;
			default:
				box.AcceptButton = (IButtonControl)(object)boxButtons[2];
				((Control)boxButtons[2]).Select();
				break;
			}
			break;
		}
		int num3 = (60 - array.Length * size.Height) / 2;
		if (num3 < 10)
		{
			num3 = 10;
		}
		((Control)boxIcon).Location = new Point(5, 10);
		((Control)boxMessege).Location = new Point(60, num3);
		switch (fMessageBoxIcon)
		{
		case FMessageBoxIcon.Error:
			boxIcon.Image = (Image)(object)Resources.error.ToBitmap();
			PlaySound(fMessageBoxIcon);
			box.ShowDialog();
			return result;
		case FMessageBoxIcon.Exclamation:
			boxIcon.Image = (Image)(object)Resources.exclamtion.ToBitmap();
			PlaySound(fMessageBoxIcon);
			box.ShowDialog();
			return result;
		case FMessageBoxIcon.Information:
			boxIcon.Image = (Image)(object)Resources.information.ToBitmap();
			PlaySound(fMessageBoxIcon);
			box.ShowDialog();
			return result;
		case FMessageBoxIcon.Question:
			boxIcon.Image = (Image)(object)Resources.ok.ToBitmap();
			PlaySound(fMessageBoxIcon);
			box.ShowDialog();
			return result;
		case FMessageBoxIcon.None:
			boxIcon.Image = null;
			box.ShowDialog();
			return result;
		default:
			return (DialogResult)2;
		}
	}

	public static DialogResult Show(string messege, string title, FMessageBoxButtons fMessageBoxButtons, FMessageBoxIcon fMessageBoxIcon, FMessageBoxDefaultButton fMessageBoxDefaultButton, List<string> buttonsText)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Expected O, but got Unknown
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0638: Unknown result type (might be due to invalid IL or missing references)
		//IL_063d: Unknown result type (might be due to invalid IL or missing references)
		//IL_08be: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ccb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Expected O, but got Unknown
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Expected O, but got Unknown
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Expected O, but got Unknown
		//IL_046b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Expected O, but got Unknown
		//IL_065c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0663: Unknown result type (might be due to invalid IL or missing references)
		//IL_0673: Unknown result type (might be due to invalid IL or missing references)
		//IL_0683: Expected O, but got Unknown
		//IL_06f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fb: Expected O, but got Unknown
		//IL_08e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0909: Expected O, but got Unknown
		//IL_097a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0984: Expected O, but got Unknown
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0736: Unknown result type (might be due to invalid IL or missing references)
		//IL_073b: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04db: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Expected O, but got Unknown
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_0573: Expected O, but got Unknown
		//IL_075a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0761: Unknown result type (might be due to invalid IL or missing references)
		//IL_0771: Unknown result type (might be due to invalid IL or missing references)
		//IL_0781: Expected O, but got Unknown
		//IL_07ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f9: Expected O, but got Unknown
		//IL_09e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0a: Expected O, but got Unknown
		//IL_0a78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a82: Expected O, but got Unknown
		//IL_0abd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b08: Expected O, but got Unknown
		//IL_0b76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b80: Expected O, but got Unknown
		buttonTextList = buttonsText;
		messege = messege.Replace(".", ".\n");
		box = new Form();
		boxMessege = new Label();
		boxIcon = new PictureBox
		{
			BackgroundImageLayout = (ImageLayout)2,
			Size = new Size(50, 50)
		};
		((Control)box).Controls.Add((Control)(object)boxIcon);
		((Control)boxMessege).AutoSize = true;
		((Control)boxMessege).Text = messege;
		((Control)box).BackColor = Color.AliceBlue;
		((Control)box).RightToLeft = (RightToLeft)1;
		box.RightToLeftLayout = true;
		((Control)box).AutoSize = false;
		((Control)box).Text = title;
		box.ShowInTaskbar = false;
		((Control)box).AutoSize = false;
		box.FormBorderStyle = (FormBorderStyle)5;
		box.StartPosition = (FormStartPosition)4;
		boxMessege.TextAlign = (ContentAlignment)16;
		((Control)boxMessege).RightToLeft = (RightToLeft)1;
		((Control)boxMessege).Font = new Font("Tahoma", 8f);
		((Control)boxMessege).BackColor = Color.Transparent;
		((Control)box).Controls.Add((Control)(object)boxMessege);
		Form obj = box;
		object obj2 = _003C_003EO._003C1_003E__Box_FormClosing;
		if (obj2 == null)
		{
			FormClosingEventHandler val = Box_FormClosing;
			_003C_003EO._003C1_003E__Box_FormClosing = val;
			obj2 = (object)val;
		}
		obj.FormClosing += (FormClosingEventHandler)obj2;
		box.Shown += Box_Shown;
		string[] array = ((Control)boxMessege).Text.Split(new char[1] { '\n' });
		Size size = TextRenderer.MeasureText(array[0], ((Control)boxMessege).Font);
		int num = size.Width + 120;
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			size = TextRenderer.MeasureText(array2[i], ((Control)boxMessege).Font);
			if (size.Width + 120 > num)
			{
				num = size.Width + 120;
			}
		}
		int num2 = array.Length * size.Height + 125;
		switch (fMessageBoxButtons)
		{
		case FMessageBoxButtons.OK:
			if (num < 260)
			{
				num = 260;
			}
			if (num2 < 150)
			{
				num2 = 150;
			}
			box.Size = new Size(num, num2);
			boxButtons = (Button[])(object)new Button[1];
			boxButtons[0] = new Button
			{
				Text = ((buttonsText.Count > 0) ? buttonsText[0] : "تایید"),
				DialogResult = (DialogResult)1,
				Image = (Image)(object)Resources.ok.ToBitmap(),
				Size = new Size(100, 30)
			};
			((Control)box).Controls.Add((Control)(object)boxButtons[0]);
			((Control)boxButtons[0]).Location = new Point(((Control)box).Width / 2 - 50, ((Control)box).Height - 90);
			((Control)boxButtons[0]).Anchor = (AnchorStyles)2;
			((ButtonBase)boxButtons[0]).UseVisualStyleBackColor = true;
			((Control)boxButtons[0]).Font = new Font("Tahoma", 8f);
			((ButtonBase)boxButtons[0]).ImageAlign = (ContentAlignment)16;
			((Control)boxButtons[0]).Click += FarsiMessegeBox_Click;
			if (fMessageBoxDefaultButton == FMessageBoxDefaultButton.Button1)
			{
				box.AcceptButton = (IButtonControl)(object)boxButtons[0];
			}
			break;
		case FMessageBoxButtons.OKCancel:
			if (num < 300)
			{
				num = 300;
			}
			if (num2 < 150)
			{
				num2 = 150;
			}
			box.Size = new Size(num, num2);
			boxButtons = (Button[])(object)new Button[2];
			boxButtons[0] = new Button
			{
				Text = ((buttonsText.Count > 0) ? buttonsText[0] : "تایید"),
				DialogResult = (DialogResult)1,
				Image = (Image)(object)Resources.ok.ToBitmap(),
				Size = new Size(100, 30)
			};
			((Control)box).Controls.Add((Control)(object)boxButtons[0]);
			((Control)boxButtons[0]).Location = new Point(((Control)box).Width / 2 - 110, ((Control)box).Height - 90);
			((Control)boxButtons[0]).Anchor = (AnchorStyles)2;
			((ButtonBase)boxButtons[0]).UseVisualStyleBackColor = true;
			((Control)boxButtons[0]).Font = new Font("Tahoma", 8f);
			((ButtonBase)boxButtons[0]).ImageAlign = (ContentAlignment)16;
			((Control)boxButtons[0]).Click += FarsiMessegeBox_Click;
			boxButtons[1] = new Button
			{
				Text = ((buttonsText.Count > 1) ? buttonsText[1] : "لغو"),
				DialogResult = (DialogResult)2,
				Image = (Image)(object)Resources.cancel.ToBitmap(),
				Size = new Size(100, 30)
			};
			((Control)box).Controls.Add((Control)(object)boxButtons[1]);
			((Control)boxButtons[1]).Location = new Point(((Control)box).Width / 2 + 10, ((Control)box).Height - 90);
			((Control)boxButtons[1]).Anchor = (AnchorStyles)2;
			((ButtonBase)boxButtons[1]).UseVisualStyleBackColor = true;
			((Control)boxButtons[1]).Font = new Font("Tahoma", 8f);
			((ButtonBase)boxButtons[1]).ImageAlign = (ContentAlignment)16;
			((Control)boxButtons[1]).Click += FarsiMessegeBox_Click;
			switch (fMessageBoxDefaultButton)
			{
			case FMessageBoxDefaultButton.Button1:
				box.AcceptButton = (IButtonControl)(object)boxButtons[0];
				((Control)boxButtons[0]).Select();
				break;
			case FMessageBoxDefaultButton.Button2:
				box.AcceptButton = (IButtonControl)(object)boxButtons[1];
				((Control)boxButtons[1]).Select();
				break;
			}
			break;
		case FMessageBoxButtons.YesNo:
			if (num < 300)
			{
				num = 300;
			}
			if (num2 < 150)
			{
				num2 = 150;
			}
			box.Size = new Size(num, num2);
			boxButtons = (Button[])(object)new Button[2];
			boxButtons[0] = new Button
			{
				Text = ((buttonsText.Count > 0) ? buttonsText[0] : "بله"),
				DialogResult = (DialogResult)6,
				Image = (Image)(object)Resources.ok.ToBitmap(),
				Size = new Size(100, 30)
			};
			((Control)box).Controls.Add((Control)(object)boxButtons[0]);
			((Control)boxButtons[0]).Location = new Point(((Control)box).Width / 2 - 110, ((Control)box).Height - 90);
			((Control)boxButtons[0]).Anchor = (AnchorStyles)2;
			((ButtonBase)boxButtons[0]).UseVisualStyleBackColor = true;
			((Control)boxButtons[0]).Font = new Font("Tahoma", 8f);
			((ButtonBase)boxButtons[0]).ImageAlign = (ContentAlignment)16;
			((Control)boxButtons[0]).Click += FarsiMessegeBox_Click;
			boxButtons[1] = new Button
			{
				Text = ((buttonsText.Count > 1) ? buttonsText[1] : "خیر"),
				DialogResult = (DialogResult)7,
				Image = (Image)(object)Resources.cancel.ToBitmap(),
				Size = new Size(100, 30)
			};
			((Control)box).Controls.Add((Control)(object)boxButtons[1]);
			((Control)boxButtons[1]).Location = new Point(((Control)box).Width / 2 + 10, ((Control)box).Height - 90);
			((Control)boxButtons[1]).Anchor = (AnchorStyles)2;
			((ButtonBase)boxButtons[1]).UseVisualStyleBackColor = true;
			((Control)boxButtons[1]).Font = new Font("Tahoma", 8f);
			((ButtonBase)boxButtons[1]).ImageAlign = (ContentAlignment)16;
			((Control)boxButtons[1]).Click += FarsiMessegeBox_Click;
			switch (fMessageBoxDefaultButton)
			{
			case FMessageBoxDefaultButton.Button1:
				box.AcceptButton = (IButtonControl)(object)boxButtons[0];
				((Control)boxButtons[0]).Select();
				break;
			case FMessageBoxDefaultButton.Button2:
				box.AcceptButton = (IButtonControl)(object)boxButtons[1];
				((Control)boxButtons[1]).Select();
				break;
			}
			break;
		case FMessageBoxButtons.YesNoCancel:
			if (num < 370)
			{
				num = 370;
			}
			if (num2 < 150)
			{
				num2 = 150;
			}
			box.Size = new Size(num, num2);
			boxButtons = (Button[])(object)new Button[3];
			boxButtons[0] = new Button
			{
				Text = ((buttonsText.Count > 0) ? buttonsText[0] : "بله"),
				DialogResult = (DialogResult)6,
				Image = (Image)(object)Resources.ok.ToBitmap(),
				Size = new Size(100, 30)
			};
			((Control)box).Controls.Add((Control)(object)boxButtons[0]);
			((Control)boxButtons[0]).Location = new Point(((Control)box).Width / 2 - 160, ((Control)box).Height - 90);
			((Control)boxButtons[0]).Anchor = (AnchorStyles)2;
			((ButtonBase)boxButtons[0]).UseVisualStyleBackColor = true;
			((Control)boxButtons[0]).Font = new Font("Tahoma", 8f);
			((ButtonBase)boxButtons[0]).ImageAlign = (ContentAlignment)16;
			((Control)boxButtons[0]).Click += FarsiMessegeBox_Click;
			boxButtons[1] = new Button
			{
				Text = ((buttonsText.Count > 1) ? buttonsText[1] : "خیر"),
				DialogResult = (DialogResult)7,
				Image = (Image)(object)Resources.cancel.ToBitmap(),
				Size = new Size(100, 30)
			};
			((Control)box).Controls.Add((Control)(object)boxButtons[1]);
			((Control)boxButtons[1]).Location = new Point(((Control)box).Width / 2 - 50, ((Control)box).Height - 90);
			((Control)boxButtons[1]).Anchor = (AnchorStyles)2;
			((ButtonBase)boxButtons[1]).UseVisualStyleBackColor = true;
			((Control)boxButtons[1]).Font = new Font("Tahoma", 8f);
			((ButtonBase)boxButtons[1]).ImageAlign = (ContentAlignment)16;
			((Control)boxButtons[1]).Click += FarsiMessegeBox_Click;
			boxButtons[2] = new Button
			{
				Text = ((buttonsText.Count > 2) ? buttonsText[2] : "لغو"),
				DialogResult = (DialogResult)2,
				Image = (Image)(object)Resources.cancel.ToBitmap(),
				Size = new Size(100, 30)
			};
			((Control)box).Controls.Add((Control)(object)boxButtons[2]);
			((Control)boxButtons[2]).Location = new Point(((Control)box).Width / 2 + 60, ((Control)box).Height - 90);
			((Control)boxButtons[2]).Anchor = (AnchorStyles)2;
			((ButtonBase)boxButtons[2]).UseVisualStyleBackColor = true;
			((Control)boxButtons[2]).Font = new Font("Tahoma", 8f);
			((ButtonBase)boxButtons[2]).ImageAlign = (ContentAlignment)16;
			((Control)boxButtons[2]).Click += FarsiMessegeBox_Click;
			switch (fMessageBoxDefaultButton)
			{
			case FMessageBoxDefaultButton.Button1:
				box.AcceptButton = (IButtonControl)(object)boxButtons[0];
				((Control)boxButtons[0]).Select();
				break;
			case FMessageBoxDefaultButton.Button2:
				box.AcceptButton = (IButtonControl)(object)boxButtons[1];
				((Control)boxButtons[1]).Select();
				break;
			default:
				box.AcceptButton = (IButtonControl)(object)boxButtons[2];
				((Control)boxButtons[2]).Select();
				break;
			}
			break;
		}
		int num3 = (60 - array.Length * size.Height) / 2;
		if (num3 < 10)
		{
			num3 = 10;
		}
		((Control)boxIcon).Location = new Point(5, 10);
		((Control)boxMessege).Location = new Point(60, num3);
		switch (fMessageBoxIcon)
		{
		case FMessageBoxIcon.Error:
			boxIcon.Image = (Image)(object)Resources.error.ToBitmap();
			PlaySound(fMessageBoxIcon);
			box.ShowDialog();
			return result;
		case FMessageBoxIcon.Exclamation:
			boxIcon.Image = (Image)(object)Resources.exclamtion.ToBitmap();
			PlaySound(fMessageBoxIcon);
			box.ShowDialog();
			return result;
		case FMessageBoxIcon.Information:
			boxIcon.Image = (Image)(object)Resources.information.ToBitmap();
			PlaySound(fMessageBoxIcon);
			box.ShowDialog();
			return result;
		case FMessageBoxIcon.Question:
			boxIcon.Image = (Image)(object)Resources.ok.ToBitmap();
			PlaySound(fMessageBoxIcon);
			box.ShowDialog();
			return result;
		case FMessageBoxIcon.None:
			boxIcon.Image = null;
			box.ShowDialog();
			return result;
		default:
			return (DialogResult)2;
		}
	}

	private static void FarsiMessegeBox_Click(object sender, EventArgs e)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		buttonIsClicked = true;
		if (((Control)sender).Text == "تایید")
		{
			result = (DialogResult)1;
			return;
		}
		if (((Control)sender).Text == "لغو")
		{
			result = (DialogResult)2;
			return;
		}
		if (((Control)sender).Text == "بله")
		{
			result = (DialogResult)6;
			return;
		}
		List<string> list = buttonTextList;
		if (list != null && list.Count > 0 && ((Control)sender).Text == buttonTextList[0])
		{
			result = (DialogResult)1;
			return;
		}
		List<string> list2 = buttonTextList;
		if (list2 != null && list2.Count > 1 && ((Control)sender).Text == buttonTextList[1])
		{
			result = (DialogResult)7;
			return;
		}
		List<string> list3 = buttonTextList;
		if (list3 != null && list3.Count > 2 && ((Control)sender).Text == buttonTextList[2])
		{
			result = (DialogResult)2;
		}
		else if (((Control)sender).Text == "خیر")
		{
			result = (DialogResult)7;
		}
	}

	private static void Box_FormClosing(object sender, FormClosingEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		if (buttonIsClicked)
		{
			return;
		}
		if (boxButtons.Length == 3)
		{
			result = (DialogResult)2;
			return;
		}
		if (boxButtons.Length != 2)
		{
			List<string> list = buttonTextList;
			if (list == null || list.Count <= 1 || !(((Control)sender).Text == buttonTextList[1]))
			{
				result = (DialogResult)1;
				return;
			}
		}
		if (((Control)boxButtons[1]).Text == "لغو")
		{
			result = (DialogResult)2;
		}
		else
		{
			result = (DialogResult)7;
		}
	}

	private static void Box_Shown(object sender, EventArgs e)
	{
		buttonIsClicked = false;
	}

	private static void PlaySound(FMessageBoxIcon icon)
	{
		SystemSound val = null;
		switch (icon)
		{
		case FMessageBoxIcon.Error:
			val = SystemSounds.Hand;
			break;
		case FMessageBoxIcon.Exclamation:
			val = SystemSounds.Exclamation;
			break;
		case FMessageBoxIcon.Information:
			val = SystemSounds.Asterisk;
			break;
		case FMessageBoxIcon.Question:
			val = SystemSounds.Asterisk;
			break;
		}
		if (val != null)
		{
			val.Play();
		}
	}
}
