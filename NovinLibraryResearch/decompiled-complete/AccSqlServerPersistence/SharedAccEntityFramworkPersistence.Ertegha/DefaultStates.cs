using System;
using System.Collections.Generic;
using EntityCache.Business;
using PacketParser.Entities;

namespace SharedAccEntityFramworkPersistence.Ertegha;

public class DefaultStates
{
	private static List<VisitorAreaBusiness> list = new List<VisitorAreaBusiness>();

	private static VisitorAreaBusiness SetDef(string contextGuid, string guid, string name)
	{
		try
		{
			return new VisitorAreaBusiness(contextGuid)
			{
				Name = name,
				Guid = Guid.Parse(guid),
				Modified = DateTime.Now,
				Status = true,
				ChangeStatus = EntityChangeSt.New,
				Code = 0L,
				Level = 1,
				ParentGuid = null,
				Description = ""
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetDef", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\DefaultStates.cs", 32);
			return null;
		}
	}

	public static List<VisitorAreaBusiness> ListDefaultData(string contextGuid)
	{
		try
		{
			list.Add(SetDef(contextGuid, "ee00bb9e-4a1a-4e3f-a5b3-c8255cb44618", "آذربایجان شرقی"));
			list.Add(SetDef(contextGuid, "6c098341-b681-4e46-8c34-397c0ca6ddb5", "آذربایجان غربی"));
			list.Add(SetDef(contextGuid, "34bd0148-acbe-4f7f-ba04-cae7620565ad", "اردبیل"));
			list.Add(SetDef(contextGuid, "aa875186-706e-417e-a10b-8056b9ce4b87", "اصفهان"));
			list.Add(SetDef(contextGuid, "7b95bca3-4d85-44da-97de-62de6da6add3", "البرز"));
			list.Add(SetDef(contextGuid, "c1800cc1-727a-400e-b14e-9437d5864abb", "ایلام"));
			list.Add(SetDef(contextGuid, "89441e7c-106b-4167-851e-8b8c0249b278", "بوشهر"));
			list.Add(SetDef(contextGuid, "4a9d6d3f-d8d2-4aed-94fd-197dc6116b85", "تهران"));
			list.Add(SetDef(contextGuid, "a09df869-efdc-43a6-8560-cc3978e6e450", "چهارمحال و بختیاری"));
			list.Add(SetDef(contextGuid, "31f8fe9e-151e-4cf4-85a7-880231b114b9", "خراسان جنوبی"));
			list.Add(SetDef(contextGuid, "c22580f8-619c-4eca-a4f2-09b662caf6bb", "خراسان رضوی"));
			list.Add(SetDef(contextGuid, "978bde86-9e17-4378-8b76-41267fa9643b", "خراسان شمالی"));
			list.Add(SetDef(contextGuid, "26f7b941-65a6-4646-ad58-4ad19947d105", "خوزستان"));
			list.Add(SetDef(contextGuid, "1440ac70-b710-4c53-95e6-bea548773b04", "زنجان"));
			list.Add(SetDef(contextGuid, "3dfa2ba7-becb-4e55-bd4f-f555e390fb57", "سمنان"));
			list.Add(SetDef(contextGuid, "c6e6885b-3e77-45bc-9a39-0a0cf42e4836", "سیستان و بلوچستان"));
			list.Add(SetDef(contextGuid, "35c9ed77-2a7c-46bc-b5d6-990aa93f30a2", "فارس"));
			list.Add(SetDef(contextGuid, "0ff879dc-7848-4ac4-8a90-59404d21726d", "قزوین"));
			list.Add(SetDef(contextGuid, "e6b0058d-71f1-4af5-a4ce-49f6d92dee9d", "قم"));
			list.Add(SetDef(contextGuid, "91c12783-465d-47c1-9bdb-21b952085bef", "کردستان"));
			list.Add(SetDef(contextGuid, "1c411e20-cc85-49dc-999e-873df23bee66", "کرمان"));
			list.Add(SetDef(contextGuid, "bc5b798b-4d53-4fc6-a656-a5ce020200fc", "کرمانشاه"));
			list.Add(SetDef(contextGuid, "e066670d-4e12-4624-a4c0-0dff93076a0f", "کهگیلویه و بویراحمد"));
			list.Add(SetDef(contextGuid, "f0051512-86ae-4015-9d6c-5ced348e6cd1", "گلستان"));
			list.Add(SetDef(contextGuid, "844328dc-98fa-468d-bee0-c05fcf5474b2", "گیلان"));
			list.Add(SetDef(contextGuid, "09357331-5c45-41f7-92c8-a56a577e1baf", "لرستان"));
			list.Add(SetDef(contextGuid, "4ae51e47-da3a-4fca-aec5-4627e5ae6be1", "مازندران"));
			list.Add(SetDef(contextGuid, "ad1ac1f1-5c07-4853-9989-1f73ad034a31", "مرکزی"));
			list.Add(SetDef(contextGuid, "6c92f484-3557-48eb-b742-07f0a4d9eed9", "هرمزگان"));
			list.Add(SetDef(contextGuid, "ecdcce7d-2940-4532-b5ec-6790d9ae45a1", "همدان"));
			list.Add(SetDef(contextGuid, "d07a2d73-affb-4580-aa56-e84d11cf0401", "یزد"));
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ListDefaultData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\DefaultStates.cs", 75);
			return null;
		}
	}
}
