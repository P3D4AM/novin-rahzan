using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals110;

internal class StateOnlyParser : Babel.ParserGenerator.StateOnlyParser
{
	internal StateOnlyParser(SimpleParserScanner baseScanner)
		: base(baseScanner.CreateTemporaryParserScanner())
	{
	}

	public override Babel.ParserGenerator.StateOnlyParser Create()
	{
		return new StateOnlyParser(scanner);
	}

	public override void DoAction(int action)
	{
		switch (action / 50)
		{
		case 0:
			DoAction_0_49(action);
			break;
		case 1:
			DoAction_50_99(action);
			break;
		case 2:
			DoAction_100_149(action);
			break;
		case 3:
			DoAction_150_199(action);
			break;
		case 4:
			DoAction_200_249(action);
			break;
		case 5:
			DoAction_250_299(action);
			break;
		case 6:
			DoAction_300_349(action);
			break;
		case 7:
			DoAction_350_399(action);
			break;
		case 8:
			DoAction_400_449(action);
			break;
		case 9:
			DoAction_450_499(action);
			break;
		case 10:
			DoAction_500_549(action);
			break;
		case 11:
			DoAction_550_599(action);
			break;
		case 12:
			DoAction_600_649(action);
			break;
		case 13:
			DoAction_650_699(action);
			break;
		case 14:
			DoAction_700_749(action);
			break;
		case 15:
			DoAction_750_799(action);
			break;
		case 16:
			DoAction_800_849(action);
			break;
		case 17:
			DoAction_850_899(action);
			break;
		case 18:
			DoAction_900_949(action);
			break;
		case 19:
			DoAction_950_999(action);
			break;
		case 20:
			DoAction_1000_1049(action);
			break;
		case 21:
			DoAction_1050_1099(action);
			break;
		case 22:
			DoAction_1100_1149(action);
			break;
		case 23:
			DoAction_1150_1199(action);
			break;
		case 24:
			DoAction_1200_1249(action);
			break;
		case 25:
			DoAction_1250_1299(action);
			break;
		case 26:
			DoAction_1300_1349(action);
			break;
		case 27:
			DoAction_1350_1399(action);
			break;
		case 28:
			DoAction_1400_1449(action);
			break;
		case 29:
			DoAction_1450_1499(action);
			break;
		case 30:
			DoAction_1500_1549(action);
			break;
		case 31:
			DoAction_1550_1599(action);
			break;
		case 32:
			DoAction_1600_1649(action);
			break;
		case 33:
			DoAction_1650_1699(action);
			break;
		case 34:
			DoAction_1700_1749(action);
			break;
		case 35:
			DoAction_1750_1799(action);
			break;
		case 36:
			DoAction_1800_1849(action);
			break;
		case 37:
			DoAction_1850_1899(action);
			break;
		case 38:
			DoAction_1900_1949(action);
			break;
		case 39:
			DoAction_1950_1999(action);
			break;
		case 40:
			DoAction_2000_2049(action);
			break;
		case 41:
			DoAction_2050_2099(action);
			break;
		case 42:
			DoAction_2100_2149(action);
			break;
		case 43:
			DoAction_2150_2199(action);
			break;
		case 44:
			DoAction_2200_2249(action);
			break;
		case 45:
			DoAction_2250_2299(action);
			break;
		case 46:
			DoAction_2300_2349(action);
			break;
		case 47:
			DoAction_2350_2399(action);
			break;
		case 48:
			DoAction_2400_2449(action);
			break;
		case 49:
			DoAction_2450_2499(action);
			break;
		case 50:
			DoAction_2500_2549(action);
			break;
		case 51:
			DoAction_2550_2599(action);
			break;
		case 52:
			DoAction_2600_2649(action);
			break;
		case 53:
			DoAction_2650_2699(action);
			break;
		case 54:
			DoAction_2700_2749(action);
			break;
		case 55:
			DoAction_2750_2799(action);
			break;
		case 56:
			DoAction_2800_2849(action);
			break;
		case 57:
			DoAction_2850_2899(action);
			break;
		case 58:
			DoAction_2900_2949(action);
			break;
		case 59:
			DoAction_2950_2999(action);
			break;
		case 60:
			DoAction_3000_3049(action);
			break;
		case 61:
			DoAction_3050_3099(action);
			break;
		case 62:
			DoAction_3100_3149(action);
			break;
		case 63:
			DoAction_3150_3161(action);
			break;
		}
	}

	private void DoAction_0_49(int action)
	{
		switch (action)
		{
		case 33:
			scanner.ResetAllParseStates();
			break;
		case 1:
		case 2:
		case 3:
		case 4:
		case 5:
		case 6:
		case 7:
		case 8:
		case 9:
		case 10:
		case 11:
		case 12:
		case 13:
		case 14:
		case 15:
		case 16:
		case 17:
		case 18:
		case 19:
		case 20:
		case 21:
		case 22:
		case 23:
		case 24:
		case 25:
		case 26:
		case 27:
		case 28:
		case 29:
		case 30:
		case 31:
		case 32:
		case 34:
		case 35:
		case 36:
		case 37:
		case 38:
		case 39:
		case 40:
		case 41:
		case 42:
		case 43:
		case 44:
		case 45:
		case 46:
		case 47:
		case 48:
		case 49:
			break;
		}
	}

	private void DoAction_50_99(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_100_149(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_150_199(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_200_249(int action)
	{
		switch (action)
		{
		case 219:
			scanner.SetQueryStatus(QueryStatus.Q_ODBC_ESCAPE);
			break;
		case 220:
			scanner.ClearQueryStatus(QueryStatus.Q_ODBC_ESCAPE);
			break;
		case 224:
			scanner.SetQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 225:
			scanner.ClearQueryStatus(QueryStatus.Q_CRDB);
			break;
		case 226:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 200:
		case 201:
		case 202:
		case 203:
		case 204:
		case 205:
		case 206:
		case 207:
		case 208:
		case 209:
		case 210:
		case 211:
		case 212:
		case 213:
		case 214:
		case 215:
		case 216:
		case 217:
		case 218:
		case 221:
		case 222:
		case 223:
		case 227:
		case 228:
		case 229:
		case 230:
		case 231:
		case 232:
		case 233:
		case 234:
		case 235:
		case 236:
		case 237:
		case 238:
		case 239:
		case 240:
		case 241:
		case 242:
		case 243:
		case 244:
		case 245:
		case 246:
		case 247:
		case 248:
		case 249:
			break;
		}
	}

	private void DoAction_250_299(int action)
	{
		switch (action)
		{
		case 282:
			scanner.SetQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 285:
			scanner.ClearQueryStatus(QueryStatus.Q_ALTDB);
			break;
		case 286:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 287:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 250:
		case 251:
		case 252:
		case 253:
		case 254:
		case 255:
		case 256:
		case 257:
		case 258:
		case 259:
		case 260:
		case 261:
		case 262:
		case 263:
		case 264:
		case 265:
		case 266:
		case 267:
		case 268:
		case 269:
		case 270:
		case 271:
		case 272:
		case 273:
		case 274:
		case 275:
		case 276:
		case 277:
		case 278:
		case 279:
		case 280:
		case 281:
		case 283:
		case 284:
		case 288:
		case 289:
		case 290:
		case 291:
		case 292:
		case 293:
		case 294:
		case 295:
		case 296:
		case 297:
		case 298:
		case 299:
			break;
		}
	}

	private void DoAction_300_349(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_350_399(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_400_449(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_450_499(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_500_549(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_550_599(int action)
	{
		switch (action)
		{
		case 588:
			scanner.SetParseState(EParseState.e_ParseIdxInclude);
			break;
		case 550:
		case 551:
		case 552:
		case 553:
		case 554:
		case 555:
		case 556:
		case 557:
		case 558:
		case 559:
		case 560:
		case 561:
		case 562:
		case 563:
		case 564:
		case 565:
		case 566:
		case 567:
		case 568:
		case 569:
		case 570:
		case 571:
		case 572:
		case 573:
		case 574:
		case 575:
		case 576:
		case 577:
		case 578:
		case 579:
		case 580:
		case 581:
		case 582:
		case 583:
		case 584:
		case 585:
		case 586:
		case 587:
		case 589:
		case 590:
		case 591:
		case 592:
		case 593:
		case 594:
		case 595:
		case 596:
		case 597:
		case 598:
		case 599:
			break;
		}
	}

	private void DoAction_600_649(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_650_699(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_700_749(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_750_799(int action)
	{
		switch (action)
		{
		case 769:
			scanner.SetParseState(EParseState.e_ParseLoad);
			break;
		case 770:
			scanner.SetParseState(EParseState.e_ParseLoad);
			break;
		case 772:
			scanner.SetParseState(EParseState.e_ParseOpen);
			break;
		case 773:
			scanner.SetParseState(EParseState.e_ParseClose);
			break;
		case 750:
		case 751:
		case 752:
		case 753:
		case 754:
		case 755:
		case 756:
		case 757:
		case 758:
		case 759:
		case 760:
		case 761:
		case 762:
		case 763:
		case 764:
		case 765:
		case 766:
		case 767:
		case 768:
		case 771:
		case 774:
		case 775:
		case 776:
		case 777:
		case 778:
		case 779:
		case 780:
		case 781:
		case 782:
		case 783:
		case 784:
		case 785:
		case 786:
		case 787:
		case 788:
		case 789:
		case 790:
		case 791:
		case 792:
		case 793:
		case 794:
		case 795:
		case 796:
		case 797:
		case 798:
		case 799:
			break;
		}
	}

	private void DoAction_800_849(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_850_899(int action)
	{
		switch (action)
		{
		case 888:
			scanner.SetParseState(EParseState.e_ParseDump);
			break;
		case 889:
			scanner.SetParseState(EParseState.e_ParseDump);
			break;
		case 850:
		case 851:
		case 852:
		case 853:
		case 854:
		case 855:
		case 856:
		case 857:
		case 858:
		case 859:
		case 860:
		case 861:
		case 862:
		case 863:
		case 864:
		case 865:
		case 866:
		case 867:
		case 868:
		case 869:
		case 870:
		case 871:
		case 872:
		case 873:
		case 874:
		case 875:
		case 876:
		case 877:
		case 878:
		case 879:
		case 880:
		case 881:
		case 882:
		case 883:
		case 884:
		case 885:
		case 886:
		case 887:
		case 890:
		case 891:
		case 892:
		case 893:
		case 894:
		case 895:
		case 896:
		case 897:
		case 898:
		case 899:
			break;
		}
	}

	private void DoAction_900_949(int action)
	{
		switch (action)
		{
		case 903:
			scanner.ClearQueryStatus(QueryStatus.Q_CR_AL_FTI);
			break;
		case 946:
			scanner.ClearQueryStatus(QueryStatus.Q_CR_AL_FTI);
			break;
		case 900:
		case 901:
		case 902:
		case 904:
		case 905:
		case 906:
		case 907:
		case 908:
		case 909:
		case 910:
		case 911:
		case 912:
		case 913:
		case 914:
		case 915:
		case 916:
		case 917:
		case 918:
		case 919:
		case 920:
		case 921:
		case 922:
		case 923:
		case 924:
		case 925:
		case 926:
		case 927:
		case 928:
		case 929:
		case 930:
		case 931:
		case 932:
		case 933:
		case 934:
		case 935:
		case 936:
		case 937:
		case 938:
		case 939:
		case 940:
		case 941:
		case 942:
		case 943:
		case 944:
		case 945:
		case 947:
		case 948:
		case 949:
			break;
		}
	}

	private void DoAction_950_999(int action)
	{
		switch (action)
		{
		case 999:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 950:
		case 951:
		case 952:
		case 953:
		case 954:
		case 955:
		case 956:
		case 957:
		case 958:
		case 959:
		case 960:
		case 961:
		case 962:
		case 963:
		case 964:
		case 965:
		case 966:
		case 967:
		case 968:
		case 969:
		case 970:
		case 971:
		case 972:
		case 973:
		case 974:
		case 975:
		case 976:
		case 977:
		case 978:
		case 979:
		case 980:
		case 981:
		case 982:
		case 983:
		case 984:
		case 985:
		case 986:
		case 987:
		case 988:
		case 989:
		case 990:
		case 991:
		case 992:
		case 993:
		case 994:
		case 995:
		case 996:
		case 997:
		case 998:
			break;
		}
	}

	private void DoAction_1000_1049(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_1050_1099(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_1100_1149(int action)
	{
		switch (action)
		{
		case 1129:
			scanner.SetParseState(EParseState.e_ParseBegin);
			break;
		case 1100:
		case 1101:
		case 1102:
		case 1103:
		case 1104:
		case 1105:
		case 1106:
		case 1107:
		case 1108:
		case 1109:
		case 1110:
		case 1111:
		case 1112:
		case 1113:
		case 1114:
		case 1115:
		case 1116:
		case 1117:
		case 1118:
		case 1119:
		case 1120:
		case 1121:
		case 1122:
		case 1123:
		case 1124:
		case 1125:
		case 1126:
		case 1127:
		case 1128:
		case 1130:
		case 1131:
		case 1132:
		case 1133:
		case 1134:
		case 1135:
		case 1136:
		case 1137:
		case 1138:
		case 1139:
		case 1140:
		case 1141:
		case 1142:
		case 1143:
		case 1144:
		case 1145:
		case 1146:
		case 1147:
		case 1148:
		case 1149:
			break;
		}
	}

	private void DoAction_1150_1199(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_1200_1249(int action)
	{
		switch (action)
		{
		case 1234:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 1200:
		case 1201:
		case 1202:
		case 1203:
		case 1204:
		case 1205:
		case 1206:
		case 1207:
		case 1208:
		case 1209:
		case 1210:
		case 1211:
		case 1212:
		case 1213:
		case 1214:
		case 1215:
		case 1216:
		case 1217:
		case 1218:
		case 1219:
		case 1220:
		case 1221:
		case 1222:
		case 1223:
		case 1224:
		case 1225:
		case 1226:
		case 1227:
		case 1228:
		case 1229:
		case 1230:
		case 1231:
		case 1232:
		case 1233:
		case 1235:
		case 1236:
		case 1237:
		case 1238:
		case 1239:
		case 1240:
		case 1241:
		case 1242:
		case 1243:
		case 1244:
		case 1245:
		case 1246:
		case 1247:
		case 1248:
		case 1249:
			break;
		}
	}

	private void DoAction_1250_1299(int action)
	{
		switch (action)
		{
		case 1275:
			scanner.SetQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 1276:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 1277:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 1250:
		case 1251:
		case 1252:
		case 1253:
		case 1254:
		case 1255:
		case 1256:
		case 1257:
		case 1258:
		case 1259:
		case 1260:
		case 1261:
		case 1262:
		case 1263:
		case 1264:
		case 1265:
		case 1266:
		case 1267:
		case 1268:
		case 1269:
		case 1270:
		case 1271:
		case 1272:
		case 1273:
		case 1274:
		case 1278:
		case 1279:
		case 1280:
		case 1281:
		case 1282:
		case 1283:
		case 1284:
		case 1285:
		case 1286:
		case 1287:
		case 1288:
		case 1289:
		case 1290:
		case 1291:
		case 1292:
		case 1293:
		case 1294:
		case 1295:
		case 1296:
		case 1297:
		case 1298:
		case 1299:
			break;
		}
	}

	private void DoAction_1300_1349(int action)
	{
		switch (action)
		{
		case 1332:
			scanner.SetParseState(EParseState.e_ParseWith);
			break;
		case 1346:
			scanner.SetParseState(EParseState.e_ParseAfterWith);
			break;
		case 1300:
		case 1301:
		case 1302:
		case 1303:
		case 1304:
		case 1305:
		case 1306:
		case 1307:
		case 1308:
		case 1309:
		case 1310:
		case 1311:
		case 1312:
		case 1313:
		case 1314:
		case 1315:
		case 1316:
		case 1317:
		case 1318:
		case 1319:
		case 1320:
		case 1321:
		case 1322:
		case 1323:
		case 1324:
		case 1325:
		case 1326:
		case 1327:
		case 1328:
		case 1329:
		case 1330:
		case 1331:
		case 1333:
		case 1334:
		case 1335:
		case 1336:
		case 1337:
		case 1338:
		case 1339:
		case 1340:
		case 1341:
		case 1342:
		case 1343:
		case 1344:
		case 1345:
		case 1347:
		case 1348:
		case 1349:
			break;
		}
	}

	private void DoAction_1350_1399(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_1400_1449(int action)
	{
		switch (action)
		{
		case 1420:
			scanner.ClearQueryStatus(QueryStatus.Q_MERGE);
			break;
		case 1400:
		case 1401:
		case 1402:
		case 1403:
		case 1404:
		case 1405:
		case 1406:
		case 1407:
		case 1408:
		case 1409:
		case 1410:
		case 1411:
		case 1412:
		case 1413:
		case 1414:
		case 1415:
		case 1416:
		case 1417:
		case 1418:
		case 1419:
		case 1421:
		case 1422:
		case 1423:
		case 1424:
		case 1425:
		case 1426:
		case 1427:
		case 1428:
		case 1429:
		case 1430:
		case 1431:
		case 1432:
		case 1433:
		case 1434:
		case 1435:
		case 1436:
		case 1437:
		case 1438:
		case 1439:
		case 1440:
		case 1441:
		case 1442:
		case 1443:
		case 1444:
		case 1445:
		case 1446:
		case 1447:
		case 1448:
		case 1449:
			break;
		}
	}

	private void DoAction_1450_1499(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_1500_1549(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_1550_1599(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_1600_1649(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_1650_1699(int action)
	{
		switch (action)
		{
		case 1656:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 1650:
		case 1651:
		case 1652:
		case 1653:
		case 1654:
		case 1655:
		case 1657:
		case 1658:
		case 1659:
		case 1660:
		case 1661:
		case 1662:
		case 1663:
		case 1664:
		case 1665:
		case 1666:
		case 1667:
		case 1668:
		case 1669:
		case 1670:
		case 1671:
		case 1672:
		case 1673:
		case 1674:
		case 1675:
		case 1676:
		case 1677:
		case 1678:
		case 1679:
		case 1680:
		case 1681:
		case 1682:
		case 1683:
		case 1684:
		case 1685:
		case 1686:
		case 1687:
		case 1688:
		case 1689:
		case 1690:
		case 1691:
		case 1692:
		case 1693:
		case 1694:
		case 1695:
		case 1696:
		case 1697:
		case 1698:
		case 1699:
			break;
		}
	}

	private void DoAction_1700_1749(int action)
	{
		switch (action)
		{
		case 1722:
			scanner.SetGDRState();
			break;
		case 1723:
			scanner.SetGDRState();
			break;
		case 1724:
			scanner.SetGDRState();
			break;
		case 1726:
			scanner.SetGDRState();
			break;
		case 1727:
			scanner.SetGDRState();
			break;
		case 1741:
			scanner.SetGDRState();
			break;
		case 1700:
		case 1701:
		case 1702:
		case 1703:
		case 1704:
		case 1705:
		case 1706:
		case 1707:
		case 1708:
		case 1709:
		case 1710:
		case 1711:
		case 1712:
		case 1713:
		case 1714:
		case 1715:
		case 1716:
		case 1717:
		case 1718:
		case 1719:
		case 1720:
		case 1721:
		case 1725:
		case 1728:
		case 1729:
		case 1730:
		case 1731:
		case 1732:
		case 1733:
		case 1734:
		case 1735:
		case 1736:
		case 1737:
		case 1738:
		case 1739:
		case 1740:
		case 1742:
		case 1743:
		case 1744:
		case 1745:
		case 1746:
		case 1747:
		case 1748:
		case 1749:
			break;
		}
	}

	private void DoAction_1750_1799(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_1800_1849(int action)
	{
		switch (action)
		{
		case 1825:
			scanner.SetParseState(EParseState.e_ParseEventNotifObjectOn);
			break;
		case 1800:
		case 1801:
		case 1802:
		case 1803:
		case 1804:
		case 1805:
		case 1806:
		case 1807:
		case 1808:
		case 1809:
		case 1810:
		case 1811:
		case 1812:
		case 1813:
		case 1814:
		case 1815:
		case 1816:
		case 1817:
		case 1818:
		case 1819:
		case 1820:
		case 1821:
		case 1822:
		case 1823:
		case 1824:
		case 1826:
		case 1827:
		case 1828:
		case 1829:
		case 1830:
		case 1831:
		case 1832:
		case 1833:
		case 1834:
		case 1835:
		case 1836:
		case 1837:
		case 1838:
		case 1839:
		case 1840:
		case 1841:
		case 1842:
		case 1843:
		case 1844:
		case 1845:
		case 1846:
		case 1847:
		case 1848:
		case 1849:
			break;
		}
	}

	private void DoAction_1850_1899(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_1900_1949(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_1950_1999(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2000_2049(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2050_2099(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2100_2149(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2150_2199(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2200_2249(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2250_2299(int action)
	{
		switch (action)
		{
		case 2260:
			scanner.SetQueryState(QueryState.None);
			break;
		case 2264:
			scanner.SetParseState(EParseState.e_ParseNeutral);
			break;
		case 2266:
			scanner.SetQueryStatus(QueryStatus.Q_BYALL);
			break;
		case 2250:
		case 2251:
		case 2252:
		case 2253:
		case 2254:
		case 2255:
		case 2256:
		case 2257:
		case 2258:
		case 2259:
		case 2261:
		case 2262:
		case 2263:
		case 2265:
		case 2267:
		case 2268:
		case 2269:
		case 2270:
		case 2271:
		case 2272:
		case 2273:
		case 2274:
		case 2275:
		case 2276:
		case 2277:
		case 2278:
		case 2279:
		case 2280:
		case 2281:
		case 2282:
		case 2283:
		case 2284:
		case 2285:
		case 2286:
		case 2287:
		case 2288:
		case 2289:
		case 2290:
		case 2291:
		case 2292:
		case 2293:
		case 2294:
		case 2295:
		case 2296:
		case 2297:
		case 2298:
		case 2299:
			break;
		}
	}

	private void DoAction_2300_2349(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2350_2399(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2400_2449(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2450_2499(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2500_2549(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2550_2599(int action)
	{
		switch (action)
		{
		case 2570:
			scanner.ClearQueryStatus(QueryStatus.Q_FN_PARSE);
			break;
		case 2550:
		case 2551:
		case 2552:
		case 2553:
		case 2554:
		case 2555:
		case 2556:
		case 2557:
		case 2558:
		case 2559:
		case 2560:
		case 2561:
		case 2562:
		case 2563:
		case 2564:
		case 2565:
		case 2566:
		case 2567:
		case 2568:
		case 2569:
		case 2571:
		case 2572:
		case 2573:
		case 2574:
		case 2575:
		case 2576:
		case 2577:
		case 2578:
		case 2579:
		case 2580:
		case 2581:
		case 2582:
		case 2583:
		case 2584:
		case 2585:
		case 2586:
		case 2587:
		case 2588:
		case 2589:
		case 2590:
		case 2591:
		case 2592:
		case 2593:
		case 2594:
		case 2595:
		case 2596:
		case 2597:
		case 2598:
		case 2599:
			break;
		}
	}

	private void DoAction_2600_2649(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2650_2699(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2700_2749(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2750_2799(int action)
	{
		switch (action)
		{
		case 2762:
			scanner.SetQueryStatus(QueryStatus.P_BREAKDECPOINT);
			break;
		case 2763:
			scanner.ClearQueryStatus(QueryStatus.P_BREAKDECPOINT);
			break;
		case 2750:
		case 2751:
		case 2752:
		case 2753:
		case 2754:
		case 2755:
		case 2756:
		case 2757:
		case 2758:
		case 2759:
		case 2760:
		case 2761:
		case 2764:
		case 2765:
		case 2766:
		case 2767:
		case 2768:
		case 2769:
		case 2770:
		case 2771:
		case 2772:
		case 2773:
		case 2774:
		case 2775:
		case 2776:
		case 2777:
		case 2778:
		case 2779:
		case 2780:
		case 2781:
		case 2782:
		case 2783:
		case 2784:
		case 2785:
		case 2786:
		case 2787:
		case 2788:
		case 2789:
		case 2790:
		case 2791:
		case 2792:
		case 2793:
		case 2794:
		case 2795:
		case 2796:
		case 2797:
		case 2798:
		case 2799:
			break;
		}
	}

	private void DoAction_2800_2849(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2850_2899(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_2900_2949(int action)
	{
		switch (action)
		{
		case 2940:
			scanner.SetQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 2941:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 2900:
		case 2901:
		case 2902:
		case 2903:
		case 2904:
		case 2905:
		case 2906:
		case 2907:
		case 2908:
		case 2909:
		case 2910:
		case 2911:
		case 2912:
		case 2913:
		case 2914:
		case 2915:
		case 2916:
		case 2917:
		case 2918:
		case 2919:
		case 2920:
		case 2921:
		case 2922:
		case 2923:
		case 2924:
		case 2925:
		case 2926:
		case 2927:
		case 2928:
		case 2929:
		case 2930:
		case 2931:
		case 2932:
		case 2933:
		case 2934:
		case 2935:
		case 2936:
		case 2937:
		case 2938:
		case 2939:
		case 2942:
		case 2943:
		case 2944:
		case 2945:
		case 2946:
		case 2947:
		case 2948:
		case 2949:
			break;
		}
	}

	private void DoAction_2950_2999(int action)
	{
		switch (action)
		{
		case 2950:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 2951:
			scanner.SetQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 2952:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 2953:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 2979:
			scanner.SetGDRState();
			break;
		case 2980:
			scanner.SetGDRState();
			break;
		case 2981:
			scanner.SetGDRState();
			break;
		case 2954:
		case 2955:
		case 2956:
		case 2957:
		case 2958:
		case 2959:
		case 2960:
		case 2961:
		case 2962:
		case 2963:
		case 2964:
		case 2965:
		case 2966:
		case 2967:
		case 2968:
		case 2969:
		case 2970:
		case 2971:
		case 2972:
		case 2973:
		case 2974:
		case 2975:
		case 2976:
		case 2977:
		case 2978:
		case 2982:
		case 2983:
		case 2984:
		case 2985:
		case 2986:
		case 2987:
		case 2988:
		case 2989:
		case 2990:
		case 2991:
		case 2992:
		case 2993:
		case 2994:
		case 2995:
		case 2996:
		case 2997:
		case 2998:
		case 2999:
			break;
		}
	}

	private void DoAction_3000_3049(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_3050_3099(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_3100_3149(int action)
	{
		switch (action)
		{
		case 3121:
			scanner.SetParseState(EParseState.e_ParseBegin);
			break;
		case 3146:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			scanner.SetQueryStatus(QueryStatus.Q_CRDB);
			break;
		case 3147:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			scanner.SetQueryStatus(QueryStatus.Q_ALTDB);
			break;
		case 3148:
			scanner.SetQueryStatus(QueryStatus.Q_CR_AL_FTI);
			break;
		case 3149:
			scanner.SetQueryStatus(QueryStatus.Q_CR_AL_FTI);
			break;
		case 3100:
		case 3101:
		case 3102:
		case 3103:
		case 3104:
		case 3105:
		case 3106:
		case 3107:
		case 3108:
		case 3109:
		case 3110:
		case 3111:
		case 3112:
		case 3113:
		case 3114:
		case 3115:
		case 3116:
		case 3117:
		case 3118:
		case 3119:
		case 3120:
		case 3122:
		case 3123:
		case 3124:
		case 3125:
		case 3126:
		case 3127:
		case 3128:
		case 3129:
		case 3130:
		case 3131:
		case 3132:
		case 3133:
		case 3134:
		case 3135:
		case 3136:
		case 3137:
		case 3138:
		case 3139:
		case 3140:
		case 3141:
		case 3142:
		case 3143:
		case 3144:
		case 3145:
			break;
		}
	}

	private void DoAction_3150_3161(int action)
	{
		switch (action)
		{
		case 3150:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 3151:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 3152:
			scanner.SetGDRState();
			break;
		case 3153:
			scanner.SetParseState(EParseState.e_ParseAfterWith);
			break;
		case 3154:
			scanner.SetQueryStatus(QueryStatus.Q_MERGE);
			break;
		case 3155:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 3156:
		{
			int tokenId = 173;
			if (OptLookup.OptStmtStarters.TryGetTokenId(scanner.GetStringToken(), ref tokenId))
			{
				scanner.RepeatToken(tokenId);
			}
			else
			{
				scanner.RepeatToken();
			}
			break;
		}
		case 3157:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 3158:
			scanner.SetQueryState(QueryState.IN_WHERE_CLAUSE);
			break;
		case 3159:
			scanner.SetParseState(EParseState.e_ParseGroupBy);
			break;
		case 3160:
			scanner.SetParseState(EParseState.e_ParseNeutral);
			break;
		case 3161:
			scanner.SetQueryStatus(QueryStatus.Q_FN_PARSE);
			break;
		}
	}
}
