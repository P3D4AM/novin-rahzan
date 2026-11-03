using System;
using System.Drawing;
using DevComponents.DotNetBar;

namespace DevComponents.AdvTree;

public class NodeStyles
{
	private static ElementStyle _29E2;

	private static ElementStyle _29E3;

	private static ElementStyle _29E4;

	private static ElementStyle _29E5;

	private static ElementStyle _29E6;

	private static ElementStyle _29E7;

	private static ElementStyle _29E8;

	private static ElementStyle _29E9;

	private static ElementStyle _29EA;

	private static ElementStyle _29EB;

	private static ElementStyle _29EC;

	private static ElementStyle _29ED;

	private static ElementStyle _29EE;

	private static ElementStyle _29EF;

	private static ElementStyle _29F0;

	private static ElementStyle _29F1;

	private static ElementStyle _29F2;

	private static ElementStyle _29F3;

	private static ElementStyle _29F4;

	private static ElementStyle _29F5;

	public static ElementStyle Apple
	{
		get
		{
			if (_29E2 == null)
			{
				_29E2 = _29F6(ePredefinedElementStyle.Apple);
			}
			return _29E2;
		}
	}

	public static ElementStyle Blue
	{
		get
		{
			if (_29E3 == null)
			{
				_29E3 = _29F6(ePredefinedElementStyle.Blue);
			}
			return _29E3;
		}
	}

	public static ElementStyle BlueLight
	{
		get
		{
			if (_29E4 == null)
			{
				_29E4 = _29F6(ePredefinedElementStyle.BlueLight);
			}
			return _29E4;
		}
	}

	public static ElementStyle BlueNight
	{
		get
		{
			if (_29E5 == null)
			{
				_29E5 = _29F6(ePredefinedElementStyle.BlueNight);
			}
			return _29E5;
		}
	}

	public static ElementStyle BlueMist
	{
		get
		{
			if (_29E6 == null)
			{
				_29E6 = _29F6(ePredefinedElementStyle.BlueMist);
			}
			return _29E6;
		}
	}

	public static ElementStyle Cyan
	{
		get
		{
			if (_29E7 == null)
			{
				_29E7 = _29F6(ePredefinedElementStyle.Cyan);
			}
			return _29E7;
		}
	}

	public static ElementStyle Green
	{
		get
		{
			if (_29E8 == null)
			{
				_29E8 = _29F6(ePredefinedElementStyle.Green);
			}
			return _29E8;
		}
	}

	public static ElementStyle Lemon
	{
		get
		{
			if (_29E9 == null)
			{
				_29E9 = _29F6(ePredefinedElementStyle.Lemon);
			}
			return _29E9;
		}
	}

	public static ElementStyle Magenta
	{
		get
		{
			if (_29EA == null)
			{
				_29EA = _29F6(ePredefinedElementStyle.Magenta);
			}
			return _29EA;
		}
	}

	public static ElementStyle Orange
	{
		get
		{
			if (_29EB == null)
			{
				_29EB = _29F6(ePredefinedElementStyle.Orange);
			}
			return _29EB;
		}
	}

	public static ElementStyle OrangeLight
	{
		get
		{
			if (_29EC == null)
			{
				_29EC = _29F6(ePredefinedElementStyle.OrangeLight);
			}
			return _29EC;
		}
	}

	public static ElementStyle Purple
	{
		get
		{
			if (_29ED == null)
			{
				_29ED = _29F6(ePredefinedElementStyle.Purple);
			}
			return _29ED;
		}
	}

	public static ElementStyle PurpleMist
	{
		get
		{
			if (_29EE == null)
			{
				_29EE = _29F6(ePredefinedElementStyle.PurpleMist);
			}
			return _29EE;
		}
	}

	public static ElementStyle Red
	{
		get
		{
			if (_29EF == null)
			{
				_29EF = _29F6(ePredefinedElementStyle.Red);
			}
			return _29EF;
		}
	}

	public static ElementStyle Silver
	{
		get
		{
			if (_29F0 == null)
			{
				_29F0 = _29F6(ePredefinedElementStyle.Silver);
			}
			return _29F0;
		}
	}

	public static ElementStyle SilverMist
	{
		get
		{
			if (_29F1 == null)
			{
				_29F1 = _29F6(ePredefinedElementStyle.SilverMist);
			}
			return _29F1;
		}
	}

	public static ElementStyle Tan
	{
		get
		{
			if (_29F2 == null)
			{
				_29F2 = _29F6(ePredefinedElementStyle.Tan);
			}
			return _29F2;
		}
	}

	public static ElementStyle Teal
	{
		get
		{
			if (_29F3 == null)
			{
				_29F3 = _29F6(ePredefinedElementStyle.Teal);
			}
			return _29F3;
		}
	}

	public static ElementStyle Yellow
	{
		get
		{
			if (_29F4 == null)
			{
				_29F4 = _29F6(ePredefinedElementStyle.Yellow);
			}
			return _29F4;
		}
	}

	public static ElementStyle Gray
	{
		get
		{
			if (_29F5 == null)
			{
				_29F5 = _29F6(ePredefinedElementStyle.Gray);
			}
			return _29F5;
		}
	}

	private static ElementStyle _29F6(ePredefinedElementStyle _0BDE)
	{
		Color ጁ = Color.Empty;
		Color ገ = Color.Empty;
		int ॷ = 90;
		Color ໆ = Color.Black;
		Color ዾ = Color.DarkGray;
		switch (_0BDE)
		{
		case ePredefinedElementStyle.Apple:
			ጁ = Color.FromArgb(232, 248, 224);
			ገ = Color.FromArgb(173, 231, 146);
			break;
		case ePredefinedElementStyle.Blue:
			ጁ = Color.FromArgb(221, 230, 247);
			ገ = Color.FromArgb(138, 168, 228);
			break;
		case ePredefinedElementStyle.BlueLight:
			ጁ = Color.FromArgb(255, 255, 255);
			ገ = Color.FromArgb(210, 224, 252);
			ໆ = Color.FromArgb(69, 84, 115);
			break;
		case ePredefinedElementStyle.BlueMist:
			ጁ = Color.FromArgb(227, 236, 243);
			ገ = Color.FromArgb(155, 187, 210);
			break;
		case ePredefinedElementStyle.BlueNight:
			ጁ = Color.FromArgb(77, 108, 152);
			ገ = Color.Navy;
			ໆ = Color.White;
			ዾ = Color.Navy;
			break;
		case ePredefinedElementStyle.Cyan:
			ጁ = Color.FromArgb(227, 236, 243);
			ገ = Color.FromArgb(155, 187, 210);
			break;
		case ePredefinedElementStyle.Green:
			ጁ = Color.FromArgb(234, 240, 226);
			ገ = Color.FromArgb(183, 201, 151);
			break;
		case ePredefinedElementStyle.Lemon:
			ጁ = Color.FromArgb(252, 253, 215);
			ገ = Color.FromArgb(245, 249, 111);
			break;
		case ePredefinedElementStyle.Magenta:
			ጁ = Color.FromArgb(243, 229, 236);
			ገ = Color.FromArgb(213, 164, 187);
			break;
		case ePredefinedElementStyle.Orange:
			ጁ = Color.FromArgb(252, 233, 217);
			ገ = Color.FromArgb(246, 176, 120);
			break;
		case ePredefinedElementStyle.OrangeLight:
			ጁ = Color.FromArgb(255, 239, 201);
			ገ = Color.FromArgb(242, 210, 132);
			ໆ = Color.FromArgb(117, 83, 2);
			break;
		case ePredefinedElementStyle.Purple:
			ጁ = Color.FromArgb(234, 227, 245);
			ገ = Color.FromArgb(180, 158, 222);
			break;
		case ePredefinedElementStyle.PurpleMist:
			ጁ = Color.FromArgb(232, 227, 234);
			ገ = Color.FromArgb(171, 156, 183);
			break;
		case ePredefinedElementStyle.Red:
			ጁ = Color.FromArgb(249, 225, 226);
			ገ = Color.FromArgb(238, 149, 151);
			break;
		case ePredefinedElementStyle.Silver:
			ጁ = Color.FromArgb(225, 225, 232);
			ገ = Color.FromArgb(149, 149, 170);
			break;
		case ePredefinedElementStyle.SilverMist:
			ጁ = Color.FromArgb(243, 244, 250);
			ገ = Color.FromArgb(155, 153, 183);
			ໆ = Color.FromArgb(87, 86, 113);
			break;
		case ePredefinedElementStyle.Tan:
			ጁ = Color.FromArgb(248, 242, 226);
			ገ = Color.FromArgb(232, 209, 153);
			break;
		case ePredefinedElementStyle.Teal:
			ጁ = Color.FromArgb(205, 236, 240);
			ገ = Color.FromArgb(78, 188, 202);
			break;
		case ePredefinedElementStyle.Yellow:
			ጁ = Color.FromArgb(255, 244, 213);
			ገ = Color.FromArgb(255, 216, 105);
			break;
		case ePredefinedElementStyle.Gray:
			ጁ = Color.White;
			ገ = ColorScheme.GetColor("E4E4F0");
			break;
		}
		return Utilities._17D0(new ComponentFactory(), Enum.GetName(typeof(ePredefinedElementStyle), _0BDE), ዾ, ጁ, ገ, ॷ, ໆ);
	}
}
