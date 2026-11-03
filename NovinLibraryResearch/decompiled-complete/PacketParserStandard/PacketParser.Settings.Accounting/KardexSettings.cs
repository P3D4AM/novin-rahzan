using System;
using PacketParser.DefaultsCoding;

namespace PacketParser.Settings.Accounting;

public class KardexSettings
{
	private Guid _kosoratDebitTafsilGuid = Guid.Empty;

	private Guid _kosoratCreditTafsilGuid = Guid.Empty;

	private Guid _ezafatDebitTafsilGuid = Guid.Empty;

	private Guid _ezafatCreditTafsilGuid = Guid.Empty;

	private Guid _kosoratDebitMoeinGuid = Guid.Empty;

	private Guid _kosoratCreditMoeinGuid = Guid.Empty;

	private Guid _ezafatDebitMoeinGuid = Guid.Empty;

	private Guid _ezafatCreditMoeinGuid = Guid.Empty;

	public Guid KosoratDebitTafsilGuid
	{
		get
		{
			return _kosoratDebitTafsilGuid;
		}
		set
		{
			_kosoratDebitTafsilGuid = value;
		}
	}

	public Guid KosoratCreditTafsilGuid
	{
		get
		{
			return _kosoratCreditTafsilGuid;
		}
		set
		{
			_kosoratCreditTafsilGuid = value;
		}
	}

	public Guid EzafatDebitTafsilGuid
	{
		get
		{
			return _ezafatDebitTafsilGuid;
		}
		set
		{
			_ezafatDebitTafsilGuid = value;
		}
	}

	public Guid EzafatCreditTafsilGuid
	{
		get
		{
			return _ezafatCreditTafsilGuid;
		}
		set
		{
			_ezafatCreditTafsilGuid = value;
		}
	}

	public Guid KosoratDebitMoeinGuid
	{
		get
		{
			if (_kosoratDebitMoeinGuid == Guid.Empty)
			{
				_kosoratDebitMoeinGuid = ParentDefaults.HesabDefault.KosoratDebitMoeinGuid;
			}
			return _kosoratDebitMoeinGuid;
		}
		set
		{
			_kosoratDebitMoeinGuid = value;
		}
	}

	public Guid KosoratCreditMoeinGuid
	{
		get
		{
			if (_kosoratCreditMoeinGuid == Guid.Empty)
			{
				_kosoratCreditMoeinGuid = ParentDefaults.HesabDefault.kosoratCreditMoeinGuid;
			}
			return _kosoratCreditMoeinGuid;
		}
		set
		{
			_kosoratCreditMoeinGuid = value;
		}
	}

	public Guid EzafatDebitMoeinGuid
	{
		get
		{
			if (_ezafatDebitMoeinGuid == Guid.Empty)
			{
				_ezafatDebitMoeinGuid = ParentDefaults.HesabDefault.EzafatDebitMoeinGuid;
			}
			return _ezafatDebitMoeinGuid;
		}
		set
		{
			_ezafatDebitMoeinGuid = value;
		}
	}

	public Guid EzafatCreditMoeinGuid
	{
		get
		{
			if (_ezafatCreditMoeinGuid == Guid.Empty)
			{
				_ezafatCreditMoeinGuid = ParentDefaults.HesabDefault.EzafatCreditMoeinGuid;
			}
			return _ezafatCreditMoeinGuid;
		}
		set
		{
			_ezafatCreditMoeinGuid = value;
		}
	}
}
