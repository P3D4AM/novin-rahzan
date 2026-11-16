Place the vendor SDK DLLs here (NOT in Git — proprietary Novin Pardaz files)
============================================================================

For NP756+ (FKAttend protocol) the app needs the 32-bit SDK DLLs from the
Novin Pardaz SDK package (e.g. the NP756-SDK-Analysis folder):

    FK623Attend.dll     <- REQUIRED (41,984 bytes — the confirmed read DLL)
    ...plus its sibling DLLs from the same SDK folder, e.g.:
    FKAttend.dll, FKViaDev.dll, LFWViaDev.dll, FKPwdEncDec.dll,
    FpDataConv.dll, FaceDataConv.dll

How to install (one time):
    copy the DLL files from the SDK folder into THIS directory
    (tools\fkattend-bridge\bin, next to the HozorPlus installation).

Alternative lookup locations (first match wins):
    1. FKBRIDGE_DLLDIR environment variable
    2. this folder  (tools\fkattend-bridge\bin)      <- recommended
    3. tools\fkattend-bridge itself                   (legacy)

Nothing else is needed: HozorPlus starts, hides, supervises and stops its
bridge by itself (docs/BRIDGE-SUPERVISION.md). If the DLLs are missing the
app shows an actionable message in the devices page instead of failing.

Licensing note: these binaries belong to Novin Pardaz; do not redistribute
them and never commit them to this repository.
