using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using EntityCache.Business.Global;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Events;

namespace FormCustom.UC.Cards;

public interface IMasterCard
{
	Func<Guid, Task<ReturnedSaveFuncInfo>> Action { get; }

	UserControl Control { get; }

	CardBusiness Data { get; }

	EnCardTheme Theme { get; }

	event Func<Guid, EnCardType, Task> OnMoreItemClicked;

	event Func<Guid, Guid, EnCardType, Task> OnItemChangeCard;

	event Func<Guid, Task> OnEditCard;

	event Func<Guid, Guid, EnCardType, Task> OnChangeCard;

	Task SetDataAsync(CardBusiness _data, Guid targetGuid, List<CardActions> _actions, bool isShowContext);

	Task SetTextAsync();

	void SetForeColor();

	void SetFont();

	void SetImage();

	void SetAlignment();

	void SetBackColor();
}
