using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals120;

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
			DoAction_3150_3199(action);
			break;
		case 64:
			DoAction_3200_3244(action);
			break;
		}
	}

	private void DoAction_0_49(int action)
	{
		switch (action)
		{
		case 34:
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
		case 33:
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
		case 223:
			scanner.SetQueryStatus(QueryStatus.Q_ODBC_ESCAPE);
			break;
		case 224:
			scanner.ClearQueryStatus(QueryStatus.Q_ODBC_ESCAPE);
			break;
		case 228:
			scanner.SetQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 229:
			scanner.ClearQueryStatus(QueryStatus.Q_CRDB);
			break;
		case 230:
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
		case 219:
		case 220:
		case 221:
		case 222:
		case 225:
		case 226:
		case 227:
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
		case 286:
			scanner.SetQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 289:
			scanner.ClearQueryStatus(QueryStatus.Q_ALTDB);
			break;
		case 290:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 291:
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
		case 282:
		case 283:
		case 284:
		case 285:
		case 287:
		case 288:
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
		}
	}

	private void DoAction_600_649(int action)
	{
		switch (action)
		{
		case 641:
			scanner.SetParseState(EParseState.e_ParseIdxInclude);
			break;
		case 600:
		case 601:
		case 602:
		case 603:
		case 604:
		case 605:
		case 606:
		case 607:
		case 608:
		case 609:
		case 610:
		case 611:
		case 612:
		case 613:
		case 614:
		case 615:
		case 616:
		case 617:
		case 618:
		case 619:
		case 620:
		case 621:
		case 622:
		case 623:
		case 624:
		case 625:
		case 626:
		case 627:
		case 628:
		case 629:
		case 630:
		case 631:
		case 632:
		case 633:
		case 634:
		case 635:
		case 636:
		case 637:
		case 638:
		case 639:
		case 640:
		case 642:
		case 643:
		case 644:
		case 645:
		case 646:
		case 647:
		case 648:
		case 649:
			break;
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
		}
	}

	private void DoAction_800_849(int action)
	{
		switch (action)
		{
		case 823:
			scanner.SetParseState(EParseState.e_ParseLoad);
			break;
		case 824:
			scanner.SetParseState(EParseState.e_ParseLoad);
			break;
		case 826:
			scanner.SetParseState(EParseState.e_ParseOpen);
			break;
		case 827:
			scanner.SetParseState(EParseState.e_ParseClose);
			break;
		case 800:
		case 801:
		case 802:
		case 803:
		case 804:
		case 805:
		case 806:
		case 807:
		case 808:
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
		case 825:
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
		case 844:
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
		}
	}

	private void DoAction_900_949(int action)
	{
		switch (action)
		{
		case 942:
			scanner.SetParseState(EParseState.e_ParseDump);
			break;
		case 943:
			scanner.SetParseState(EParseState.e_ParseDump);
			break;
		case 900:
		case 901:
		case 902:
		case 903:
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
		case 944:
		case 945:
		case 946:
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
		case 957:
			scanner.ClearQueryStatus(QueryStatus.Q_CR_AL_FTI);
			break;
		case 950:
		case 951:
		case 952:
		case 953:
		case 954:
		case 955:
		case 956:
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
		case 999:
			break;
		}
	}

	private void DoAction_1000_1049(int action)
	{
		switch (action)
		{
		case 1000:
			scanner.ClearQueryStatus(QueryStatus.Q_CR_AL_FTI);
			break;
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
		case 1022:
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
		case 1053:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 1050:
		case 1051:
		case 1052:
		case 1054:
		case 1055:
		case 1056:
		case 1057:
		case 1058:
		case 1059:
		case 1060:
		case 1061:
		case 1062:
		case 1063:
		case 1064:
		case 1065:
		case 1066:
		case 1067:
		case 1068:
		case 1069:
		case 1070:
		case 1071:
		case 1072:
		case 1073:
		case 1074:
		case 1075:
		case 1076:
		case 1077:
		case 1078:
		case 1079:
		case 1080:
		case 1081:
		case 1082:
		case 1083:
		case 1084:
		case 1085:
		case 1086:
		case 1087:
		case 1088:
		case 1089:
		case 1090:
		case 1091:
		case 1092:
		case 1093:
		case 1094:
		case 1095:
		case 1096:
		case 1097:
		case 1098:
		case 1099:
			break;
		}
	}

	private void DoAction_1100_1149(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_1150_1199(int action)
	{
		switch (action)
		{
		case 1183:
			scanner.SetParseState(EParseState.e_ParseBegin);
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
		case 1163:
		case 1164:
		case 1165:
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
		}
	}

	private void DoAction_1250_1299(int action)
	{
		switch (action)
		{
		case 1288:
			scanner.SetStmtDelimiter(value: true);
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
		case 1275:
		case 1276:
		case 1277:
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
		case 1329:
			scanner.SetQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 1330:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			break;
		case 1331:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
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
		case 1386:
			scanner.SetParseState(EParseState.e_ParseWith);
			break;
		case 1350:
		case 1351:
		case 1352:
		case 1353:
		case 1354:
		case 1355:
		case 1356:
		case 1357:
		case 1358:
		case 1359:
		case 1360:
		case 1361:
		case 1362:
		case 1363:
		case 1364:
		case 1365:
		case 1366:
		case 1367:
		case 1368:
		case 1369:
		case 1370:
		case 1371:
		case 1372:
		case 1373:
		case 1374:
		case 1375:
		case 1376:
		case 1377:
		case 1378:
		case 1379:
		case 1380:
		case 1381:
		case 1382:
		case 1383:
		case 1384:
		case 1385:
		case 1387:
		case 1388:
		case 1389:
		case 1390:
		case 1391:
		case 1392:
		case 1393:
		case 1394:
		case 1395:
		case 1396:
		case 1397:
		case 1398:
		case 1399:
			break;
		}
	}

	private void DoAction_1400_1449(int action)
	{
		switch (action)
		{
		case 1400:
			scanner.SetParseState(EParseState.e_ParseAfterWith);
			break;
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
		case 1420:
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
		case 1474:
			scanner.ClearQueryStatus(QueryStatus.Q_MERGE);
			break;
		case 1450:
		case 1451:
		case 1452:
		case 1453:
		case 1454:
		case 1455:
		case 1456:
		case 1457:
		case 1458:
		case 1459:
		case 1460:
		case 1461:
		case 1462:
		case 1463:
		case 1464:
		case 1465:
		case 1466:
		case 1467:
		case 1468:
		case 1469:
		case 1470:
		case 1471:
		case 1472:
		case 1473:
		case 1475:
		case 1476:
		case 1477:
		case 1478:
		case 1479:
		case 1480:
		case 1481:
		case 1482:
		case 1483:
		case 1484:
		case 1485:
		case 1486:
		case 1487:
		case 1488:
		case 1489:
		case 1490:
		case 1491:
		case 1492:
		case 1493:
		case 1494:
		case 1495:
		case 1496:
		case 1497:
		case 1498:
		case 1499:
			break;
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
		}
	}

	private void DoAction_1700_1749(int action)
	{
		switch (action)
		{
		case 1716:
			scanner.SetStmtDelimiter(value: true);
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
		case 1717:
		case 1718:
		case 1719:
		case 1720:
		case 1721:
		case 1722:
		case 1723:
		case 1724:
		case 1725:
		case 1726:
		case 1727:
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
		case 1741:
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
		case 1787:
			scanner.SetGDRState();
			break;
		case 1788:
			scanner.SetGDRState();
			break;
		case 1789:
			scanner.SetGDRState();
			break;
		case 1791:
			scanner.SetGDRState();
			break;
		case 1792:
			scanner.SetGDRState();
			break;
		case 1750:
		case 1751:
		case 1752:
		case 1753:
		case 1754:
		case 1755:
		case 1756:
		case 1757:
		case 1758:
		case 1759:
		case 1760:
		case 1761:
		case 1762:
		case 1763:
		case 1764:
		case 1765:
		case 1766:
		case 1767:
		case 1768:
		case 1769:
		case 1770:
		case 1771:
		case 1772:
		case 1773:
		case 1774:
		case 1775:
		case 1776:
		case 1777:
		case 1778:
		case 1779:
		case 1780:
		case 1781:
		case 1782:
		case 1783:
		case 1784:
		case 1785:
		case 1786:
		case 1790:
		case 1793:
		case 1794:
		case 1795:
		case 1796:
		case 1797:
		case 1798:
		case 1799:
			break;
		}
	}

	private void DoAction_1800_1849(int action)
	{
		switch (action)
		{
		case 1806:
			scanner.SetGDRState();
			break;
		case 1800:
		case 1801:
		case 1802:
		case 1803:
		case 1804:
		case 1805:
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
		case 1825:
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
		case 1897:
			scanner.SetParseState(EParseState.e_ParseEventNotifObjectOn);
			break;
		case 1850:
		case 1851:
		case 1852:
		case 1853:
		case 1854:
		case 1855:
		case 1856:
		case 1857:
		case 1858:
		case 1859:
		case 1860:
		case 1861:
		case 1862:
		case 1863:
		case 1864:
		case 1865:
		case 1866:
		case 1867:
		case 1868:
		case 1869:
		case 1870:
		case 1871:
		case 1872:
		case 1873:
		case 1874:
		case 1875:
		case 1876:
		case 1877:
		case 1878:
		case 1879:
		case 1880:
		case 1881:
		case 1882:
		case 1883:
		case 1884:
		case 1885:
		case 1886:
		case 1887:
		case 1888:
		case 1889:
		case 1890:
		case 1891:
		case 1892:
		case 1893:
		case 1894:
		case 1895:
		case 1896:
		case 1898:
		case 1899:
			break;
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
		}
	}

	private void DoAction_2300_2349(int action)
	{
		switch (action)
		{
		case 2329:
			scanner.SetQueryState(QueryState.None);
			break;
		case 2333:
			scanner.SetParseState(EParseState.e_ParseNeutral);
			break;
		case 2335:
			scanner.SetQueryStatus(QueryStatus.Q_BYALL);
			break;
		case 2300:
		case 2301:
		case 2302:
		case 2303:
		case 2304:
		case 2305:
		case 2306:
		case 2307:
		case 2308:
		case 2309:
		case 2310:
		case 2311:
		case 2312:
		case 2313:
		case 2314:
		case 2315:
		case 2316:
		case 2317:
		case 2318:
		case 2319:
		case 2320:
		case 2321:
		case 2322:
		case 2323:
		case 2324:
		case 2325:
		case 2326:
		case 2327:
		case 2328:
		case 2330:
		case 2331:
		case 2332:
		case 2334:
		case 2336:
		case 2337:
		case 2338:
		case 2339:
		case 2340:
		case 2341:
		case 2342:
		case 2343:
		case 2344:
		case 2345:
		case 2346:
		case 2347:
		case 2348:
		case 2349:
			break;
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
		}
	}

	private void DoAction_2600_2649(int action)
	{
		switch (action)
		{
		case 2639:
			scanner.ClearQueryStatus(QueryStatus.Q_FN_PARSE);
			break;
		case 2600:
		case 2601:
		case 2602:
		case 2603:
		case 2604:
		case 2605:
		case 2606:
		case 2607:
		case 2608:
		case 2609:
		case 2610:
		case 2611:
		case 2612:
		case 2613:
		case 2614:
		case 2615:
		case 2616:
		case 2617:
		case 2618:
		case 2619:
		case 2620:
		case 2621:
		case 2622:
		case 2623:
		case 2624:
		case 2625:
		case 2626:
		case 2627:
		case 2628:
		case 2629:
		case 2630:
		case 2631:
		case 2632:
		case 2633:
		case 2634:
		case 2635:
		case 2636:
		case 2637:
		case 2638:
		case 2640:
		case 2641:
		case 2642:
		case 2643:
		case 2644:
		case 2645:
		case 2646:
		case 2647:
		case 2648:
		case 2649:
			break;
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
		}
	}

	private void DoAction_2800_2849(int action)
	{
		switch (action)
		{
		case 2831:
			scanner.SetQueryStatus(QueryStatus.P_BREAKDECPOINT);
			break;
		case 2832:
			scanner.ClearQueryStatus(QueryStatus.P_BREAKDECPOINT);
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
		case 2816:
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
		case 2833:
		case 2834:
		case 2835:
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
		}
	}

	private void DoAction_2950_2999(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_3000_3049(int action)
	{
		switch (action)
		{
		case 3013:
			scanner.SetQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3014:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3023:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3024:
			scanner.SetQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3025:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3026:
			scanner.ClearQueryStatus(QueryStatus.Q_SERVER);
			break;
		case 3000:
		case 3001:
		case 3002:
		case 3003:
		case 3004:
		case 3005:
		case 3006:
		case 3007:
		case 3008:
		case 3009:
		case 3010:
		case 3011:
		case 3012:
		case 3015:
		case 3016:
		case 3017:
		case 3018:
		case 3019:
		case 3020:
		case 3021:
		case 3022:
		case 3027:
		case 3028:
		case 3029:
		case 3030:
		case 3031:
		case 3032:
		case 3033:
		case 3034:
		case 3035:
		case 3036:
		case 3037:
		case 3038:
		case 3039:
		case 3040:
		case 3041:
		case 3042:
		case 3043:
		case 3044:
		case 3045:
		case 3046:
		case 3047:
		case 3048:
		case 3049:
			break;
		}
	}

	private void DoAction_3050_3099(int action)
	{
		switch (action)
		{
		case 3052:
			scanner.SetGDRState();
			break;
		case 3053:
			scanner.SetGDRState();
			break;
		case 3054:
			scanner.SetGDRState();
			break;
		case 3050:
		case 3051:
		case 3055:
		case 3056:
		case 3057:
		case 3058:
		case 3059:
		case 3060:
		case 3061:
		case 3062:
		case 3063:
		case 3064:
		case 3065:
		case 3066:
		case 3067:
		case 3068:
		case 3069:
		case 3070:
		case 3071:
		case 3072:
		case 3073:
		case 3074:
		case 3075:
		case 3076:
		case 3077:
		case 3078:
		case 3079:
		case 3080:
		case 3081:
		case 3082:
		case 3083:
		case 3084:
		case 3085:
		case 3086:
		case 3087:
		case 3088:
		case 3089:
		case 3090:
		case 3091:
		case 3092:
		case 3093:
		case 3094:
		case 3095:
		case 3096:
		case 3097:
		case 3098:
		case 3099:
			break;
		}
	}

	private void DoAction_3100_3149(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_3150_3199(int action)
	{
		switch (action)
		{
		}
	}

	private void DoAction_3200_3244(int action)
	{
		switch (action)
		{
		case 3204:
			scanner.SetParseState(EParseState.e_ParseBegin);
			break;
		case 3229:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			scanner.SetQueryStatus(QueryStatus.Q_CRDB);
			break;
		case 3230:
			scanner.ClearQueryStatus(QueryStatus.Q_DATABASE);
			scanner.SetQueryStatus(QueryStatus.Q_ALTDB);
			break;
		case 3231:
			scanner.SetQueryStatus(QueryStatus.Q_CR_AL_FTI);
			break;
		case 3232:
			scanner.SetQueryStatus(QueryStatus.Q_CR_AL_FTI);
			break;
		case 3233:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 3234:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 3235:
			scanner.SetGDRState();
			break;
		case 3236:
			scanner.SetParseState(EParseState.e_ParseAfterWith);
			break;
		case 3237:
			scanner.SetQueryStatus(QueryStatus.Q_MERGE);
			break;
		case 3238:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 3239:
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
		case 3240:
			scanner.SetStmtDelimiter(value: true);
			break;
		case 3241:
			scanner.SetQueryState(QueryState.IN_WHERE_CLAUSE);
			break;
		case 3242:
			scanner.SetParseState(EParseState.e_ParseGroupBy);
			break;
		case 3243:
			scanner.SetParseState(EParseState.e_ParseNeutral);
			break;
		case 3244:
			scanner.SetQueryStatus(QueryStatus.Q_FN_PARSE);
			break;
		case 3200:
		case 3201:
		case 3202:
		case 3203:
		case 3205:
		case 3206:
		case 3207:
		case 3208:
		case 3209:
		case 3210:
		case 3211:
		case 3212:
		case 3213:
		case 3214:
		case 3215:
		case 3216:
		case 3217:
		case 3218:
		case 3219:
		case 3220:
		case 3221:
		case 3222:
		case 3223:
		case 3224:
		case 3225:
		case 3226:
		case 3227:
		case 3228:
			break;
		}
	}
}
