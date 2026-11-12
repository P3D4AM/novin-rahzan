using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals105;

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
			DoAction_2850_2865(action);
			break;
		}
	}

	private void DoAction_0_49(int action)
	{
		switch (action)
		{
		case 31:
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
		case 32:
		case 33:
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
		case 207:
			scanner.SetQueryStatus(QueryStatus.Q_ODBC_ESCAPE);
			break;
		case 208:
			scanner.ClearQueryStatus(QueryStatus.Q_ODBC_ESCAPE);
			break;
		case 212:
			scanner.SetQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 213:
			scanner.ClearQueryStatus(QueryStatus.Q_CRDB);
			break;
		case 214:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 200:
		case 201:
		case 202:
		case 203:
		case 204:
		case 205:
		case 206:
		case 209:
		case 210:
		case 211:
		case 215:
		case 216:
		case 217:
		case 218:
		case 219:
		case 220:
		case 221:
		case 222:
		case 223:
		case 224:
		case 225:
		case 226:
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
		case 255:
			scanner.SetQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 256:
			scanner.ClearQueryStatus(QueryStatus.Q_ALTDB);
			break;
		case 257:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 258:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 250:
		case 251:
		case 252:
		case 253:
		case 254:
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
		case 282:
		case 283:
		case 284:
		case 285:
		case 286:
		case 287:
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
		case 513:
			scanner.SetParseState(EParseState.e_ParseIdxInclude);
			break;
		case 500:
		case 501:
		case 502:
		case 503:
		case 504:
		case 505:
		case 506:
		case 507:
		case 508:
		case 509:
		case 510:
		case 511:
		case 512:
		case 514:
		case 515:
		case 516:
		case 517:
		case 518:
		case 519:
		case 520:
		case 521:
		case 522:
		case 523:
		case 524:
		case 525:
		case 526:
		case 527:
		case 528:
		case 529:
		case 530:
		case 531:
		case 532:
		case 533:
		case 534:
		case 535:
		case 536:
		case 537:
		case 538:
		case 539:
		case 540:
		case 541:
		case 542:
		case 543:
		case 544:
		case 545:
		case 546:
		case 547:
		case 548:
		case 549:
			break;
		}
	}

	private void DoAction_550_599(int action)
	{
		switch (action)
		{
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
		case 672:
			scanner.SetParseState(EParseState.e_ParseLoad);
			break;
		case 673:
			scanner.SetParseState(EParseState.e_ParseLoad);
			break;
		case 675:
			scanner.SetParseState(EParseState.e_ParseOpen);
			break;
		case 676:
			scanner.SetParseState(EParseState.e_ParseClose);
			break;
		case 650:
		case 651:
		case 652:
		case 653:
		case 654:
		case 655:
		case 656:
		case 657:
		case 658:
		case 659:
		case 660:
		case 661:
		case 662:
		case 663:
		case 664:
		case 665:
		case 666:
		case 667:
		case 668:
		case 669:
		case 670:
		case 671:
		case 674:
		case 677:
		case 678:
		case 679:
		case 680:
		case 681:
		case 682:
		case 683:
		case 684:
		case 685:
		case 686:
		case 687:
		case 688:
		case 689:
		case 690:
		case 691:
		case 692:
		case 693:
		case 694:
		case 695:
		case 696:
		case 697:
		case 698:
		case 699:
			break;
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
		case 789:
			scanner.SetParseState(EParseState.e_ParseDump);
			break;
		case 790:
			scanner.SetParseState(EParseState.e_ParseDump);
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
		case 769:
		case 770:
		case 771:
		case 772:
		case 773:
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
		case 808:
			scanner.ClearQueryStatus(QueryStatus.Q_CR_AL_FTI);
			break;
		case 844:
			scanner.ClearQueryStatus(QueryStatus.Q_CR_AL_FTI);
			break;
		case 800:
		case 801:
		case 802:
		case 803:
		case 804:
		case 805:
		case 806:
		case 807:
		case 809:
		case 810:
		case 811:
		case 812:
		case 813:
		case 814:
		case 815:
		case 816:
		case 817:
		case 818:
		case 819:
		case 820:
		case 821:
		case 822:
		case 823:
		case 824:
		case 825:
		case 826:
		case 827:
		case 828:
		case 829:
		case 830:
		case 831:
		case 832:
		case 833:
		case 834:
		case 835:
		case 836:
		case 837:
		case 838:
		case 839:
		case 840:
		case 841:
		case 842:
		case 843:
		case 845:
		case 846:
		case 847:
		case 848:
		case 849:
			break;
		}
	}

	private void DoAction_850_899(int action)
	{
		switch (action)
		{
		case 892:
			scanner.SetStmtDelimiter(value: true);
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
		case 888:
		case 889:
		case 890:
		case 891:
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
		}
	}

	private void DoAction_950_999(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_1000_1049(int action)
	{
		switch (action)
		{
		case 1022:
			scanner.SetParseState(EParseState.e_ParseBegin);
			break;
		case 1000:
		case 1001:
		case 1002:
		case 1003:
		case 1004:
		case 1005:
		case 1006:
		case 1007:
		case 1008:
		case 1009:
		case 1010:
		case 1011:
		case 1012:
		case 1013:
		case 1014:
		case 1015:
		case 1016:
		case 1017:
		case 1018:
		case 1019:
		case 1020:
		case 1021:
		case 1023:
		case 1024:
		case 1025:
		case 1026:
		case 1027:
		case 1028:
		case 1029:
		case 1030:
		case 1031:
		case 1032:
		case 1033:
		case 1034:
		case 1035:
		case 1036:
		case 1037:
		case 1038:
		case 1039:
		case 1040:
		case 1041:
		case 1042:
		case 1043:
		case 1044:
		case 1045:
		case 1046:
		case 1047:
		case 1048:
		case 1049:
			break;
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
		case 1122:
			scanner.SetStmtDelimiter(value: true);
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
		case 1123:
		case 1124:
		case 1125:
		case 1126:
		case 1127:
		case 1128:
		case 1129:
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
		case 1163:
			scanner.SetQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 1164:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 1165:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 1150:
		case 1151:
		case 1152:
		case 1153:
		case 1154:
		case 1155:
		case 1156:
		case 1157:
		case 1158:
		case 1159:
		case 1160:
		case 1161:
		case 1162:
		case 1166:
		case 1167:
		case 1168:
		case 1169:
		case 1170:
		case 1171:
		case 1172:
		case 1173:
		case 1174:
		case 1175:
		case 1176:
		case 1177:
		case 1178:
		case 1179:
		case 1180:
		case 1181:
		case 1182:
		case 1183:
		case 1184:
		case 1185:
		case 1186:
		case 1187:
		case 1188:
		case 1189:
		case 1190:
		case 1191:
		case 1192:
		case 1193:
		case 1194:
		case 1195:
		case 1196:
		case 1197:
		case 1198:
		case 1199:
			break;
		}
	}

	private void DoAction_1200_1249(int action)
	{
		switch (action)
		{
		case 1218:
			scanner.SetParseState(EParseState.e_ParseWith);
			break;
		case 1230:
			scanner.SetParseState(EParseState.e_ParseAfterWith);
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
		case 1231:
		case 1232:
		case 1233:
		case 1234:
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
		}
	}

	private void DoAction_1300_1349(int action)
	{
		switch (action)
		{
		case 1304:
			scanner.ClearQueryStatus(QueryStatus.Q_MERGE);
			break;
		case 1300:
		case 1301:
		case 1302:
		case 1303:
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
		case 1332:
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
		case 1346:
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
		case 1511:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 1500:
		case 1501:
		case 1502:
		case 1503:
		case 1504:
		case 1505:
		case 1506:
		case 1507:
		case 1508:
		case 1509:
		case 1510:
		case 1512:
		case 1513:
		case 1514:
		case 1515:
		case 1516:
		case 1517:
		case 1518:
		case 1519:
		case 1520:
		case 1521:
		case 1522:
		case 1523:
		case 1524:
		case 1525:
		case 1526:
		case 1527:
		case 1528:
		case 1529:
		case 1530:
		case 1531:
		case 1532:
		case 1533:
		case 1534:
		case 1535:
		case 1536:
		case 1537:
		case 1538:
		case 1539:
		case 1540:
		case 1541:
		case 1542:
		case 1543:
		case 1544:
		case 1545:
		case 1546:
		case 1547:
		case 1548:
		case 1549:
			break;
		}
	}

	private void DoAction_1550_1599(int action)
	{
		switch (action)
		{
		case 1576:
			scanner.SetGDRState();
			break;
		case 1577:
			scanner.SetGDRState();
			break;
		case 1578:
			scanner.SetGDRState();
			break;
		case 1580:
			scanner.SetGDRState();
			break;
		case 1581:
			scanner.SetGDRState();
			break;
		case 1595:
			scanner.SetGDRState();
			break;
		case 1550:
		case 1551:
		case 1552:
		case 1553:
		case 1554:
		case 1555:
		case 1556:
		case 1557:
		case 1558:
		case 1559:
		case 1560:
		case 1561:
		case 1562:
		case 1563:
		case 1564:
		case 1565:
		case 1566:
		case 1567:
		case 1568:
		case 1569:
		case 1570:
		case 1571:
		case 1572:
		case 1573:
		case 1574:
		case 1575:
		case 1579:
		case 1582:
		case 1583:
		case 1584:
		case 1585:
		case 1586:
		case 1587:
		case 1588:
		case 1589:
		case 1590:
		case 1591:
		case 1592:
		case 1593:
		case 1594:
		case 1596:
		case 1597:
		case 1598:
		case 1599:
			break;
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
		case 1679:
			scanner.SetParseState(EParseState.e_ParseEventNotifObjectOn);
			break;
		case 1650:
		case 1651:
		case 1652:
		case 1653:
		case 1654:
		case 1655:
		case 1656:
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
		case 2110:
			scanner.SetQueryState(QueryState.None);
			break;
		case 2114:
			scanner.SetParseState(EParseState.e_ParseNeutral);
			break;
		case 2116:
			scanner.SetQueryStatus(QueryStatus.Q_BYALL);
			break;
		case 2100:
		case 2101:
		case 2102:
		case 2103:
		case 2104:
		case 2105:
		case 2106:
		case 2107:
		case 2108:
		case 2109:
		case 2111:
		case 2112:
		case 2113:
		case 2115:
		case 2117:
		case 2118:
		case 2119:
		case 2120:
		case 2121:
		case 2122:
		case 2123:
		case 2124:
		case 2125:
		case 2126:
		case 2127:
		case 2128:
		case 2129:
		case 2130:
		case 2131:
		case 2132:
		case 2133:
		case 2134:
		case 2135:
		case 2136:
		case 2137:
		case 2138:
		case 2139:
		case 2140:
		case 2141:
		case 2142:
		case 2143:
		case 2144:
		case 2145:
		case 2146:
		case 2147:
		case 2148:
		case 2149:
			break;
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
		case 2583:
			scanner.SetQueryStatus(QueryStatus.P_BREAKDECPOINT);
			break;
		case 2584:
			scanner.ClearQueryStatus(QueryStatus.P_BREAKDECPOINT);
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
		case 2570:
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
		case 2742:
			scanner.SetQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 2743:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 2747:
			scanner.SetQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 2748:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 2749:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 2700:
		case 2701:
		case 2702:
		case 2703:
		case 2704:
		case 2705:
		case 2706:
		case 2707:
		case 2708:
		case 2709:
		case 2710:
		case 2711:
		case 2712:
		case 2713:
		case 2714:
		case 2715:
		case 2716:
		case 2717:
		case 2718:
		case 2719:
		case 2720:
		case 2721:
		case 2722:
		case 2723:
		case 2724:
		case 2725:
		case 2726:
		case 2727:
		case 2728:
		case 2729:
		case 2730:
		case 2731:
		case 2732:
		case 2733:
		case 2734:
		case 2735:
		case 2736:
		case 2737:
		case 2738:
		case 2739:
		case 2740:
		case 2741:
		case 2744:
		case 2745:
		case 2746:
			break;
		}
	}

	private void DoAction_2750_2799(int action)
	{
		switch (action)
		{
		case 2775:
			scanner.SetGDRState();
			break;
		case 2776:
			scanner.SetGDRState();
			break;
		case 2777:
			scanner.SetGDRState();
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
		case 2762:
		case 2763:
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
		case 2816:
			scanner.SetParseState(EParseState.e_ParseBegin);
			break;
		case 2835:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 2800:
		case 2801:
		case 2802:
		case 2803:
		case 2804:
		case 2805:
		case 2806:
		case 2807:
		case 2808:
		case 2809:
		case 2810:
		case 2811:
		case 2812:
		case 2813:
		case 2814:
		case 2815:
		case 2817:
		case 2818:
		case 2819:
		case 2820:
		case 2821:
		case 2822:
		case 2823:
		case 2824:
		case 2825:
		case 2826:
		case 2827:
		case 2828:
		case 2829:
		case 2830:
		case 2831:
		case 2832:
		case 2833:
		case 2834:
		case 2836:
		case 2837:
		case 2838:
		case 2839:
		case 2840:
		case 2841:
		case 2842:
		case 2843:
		case 2844:
		case 2845:
		case 2846:
		case 2847:
		case 2848:
		case 2849:
			break;
		}
	}

	private void DoAction_2850_2865(int action)
	{
		switch (action)
		{
		case 2851:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			scanner.SetQueryStatus(QueryStatus.Q_CRDB);
			break;
		case 2852:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			scanner.SetQueryStatus(QueryStatus.Q_ALTDB);
			break;
		case 2853:
			scanner.SetQueryStatus(QueryStatus.Q_CR_AL_FTI);
			break;
		case 2854:
			scanner.SetQueryStatus(QueryStatus.Q_CR_AL_FTI);
			break;
		case 2855:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 2856:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 2857:
			scanner.SetGDRState();
			break;
		case 2858:
			scanner.SetParseState(EParseState.e_ParseAfterWith);
			break;
		case 2859:
			scanner.SetQueryStatus(QueryStatus.Q_MERGE);
			break;
		case 2860:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 2861:
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
		case 2862:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 2863:
			scanner.SetQueryState(QueryState.IN_WHERE_CLAUSE);
			break;
		case 2864:
			scanner.SetParseState(EParseState.e_ParseGroupBy);
			break;
		case 2865:
			scanner.SetParseState(EParseState.e_ParseNeutral);
			break;
		case 2850:
			break;
		}
	}
}
