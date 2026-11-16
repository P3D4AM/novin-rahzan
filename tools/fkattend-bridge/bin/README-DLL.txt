NP756+ SDK DLLs — SHIPPED WITH THE APP (v1.5.1+)
==============================================

Since v1.5.1 the Windows-Setup zip CARRIES the complete verified SDK DLL
set in THIS folder. Installing HozorPlus is the ONLY step — there is no
manual DLL copy, no SDK package to hunt for, no extra download.

The 7 files below are the complete closure (verified 2026-11 by PE
import-table analysis of every file; see docs/FKATTEND-CAPABILITIES.md):

  FK623Attend.dll  ( 41,984)  entry DLL called by the bridge (port 4238)
  FKAttend.dll     (2,141,184)  imported by FK623Attend.dll (by ordinal)
  FKViaDev.dll     (1,651,200)  ┐
  LFWViaDev.dll    (1,648,128)  │ imported by FKAttend.dll
  FpDataConv.dll  (  147,456)  │ (communication / data /
  FaceDataConv.dll ( 94,208)  │  conversion / crypto helpers)
  FKPwdEncDec.dll (  47,104)  ┘

Everything below those is standard Windows (KERNEL32, WS2_32, SETUPAPI,
GDI, etc.) — nothing else needs to be shipped. FK623AttendDllCSSample.dll
(the vendor's C# WinForms demo) is deliberately NOT included: the bridge
declares its own P/Invoke signatures and never needs it.

SHA-256 (verify with: certutil -hashfile <file> SHA256):
  FK623Attend.dll  bb0cff87b7fcf33d3de57139e62b74ef7da59c0cb725ea3d2c456f280b174177
  FKAttend.dll     05d8e9bd344ea94323e619e4edc16ea9b0760f6ed25d657b4eb6e7eb21bf2d93
  FKViaDev.dll    d5f01ed016cc86bb58c776c968ede7264dff410068451d110d84ff5b21b0035a
  LFWViaDev.dll   8c33637c9da58d5268ce170caacde0808752051d89f2f3baef884efb2c03eb83
  FKPwdEncDec.dll d979588f746dab436eea1f3ce9541a79d2d1be5721255298b2d7cf9c25651e39
  FpDataConv.dll  90fd91a9a0a66767cfdbf8b6f711ed6fd30924a357b9353ee76c1150a1dca837
  FaceDataConv.dll 261afd0a72db6ef8e84095da3d935b11635c78812010c4934b91c7a4d911230c

These hashes are bit-identical to the vendor SDK package the app was
validated against (recorded successful reads on a real NP756+).

If a DLL is MISSING on an installed machine (e.g. antivirus quarantined
it), do not hand-copy random SDK files: re-extract the HozorPlus zip (or
re-install) so the set stays exactly this closure — the app checks
FK623Attend.dll first and says exactly what is missing.

DLL directory lookup order (unchanged, first match wins):
  1. FKBRIDGE_DLLDIR environment variable (advanced override)
  2. this folder  (tools\fkattend-bridge\bin)      <- shipped default
  3. tools\fkattend-bridge itself                   (legacy)

Licensing: these binaries are Novin Pardaz proprietary IP. They are
committed to this repository and shipped inside the installer by the
explicit decision of the repository owner (the SDK package licensee), so
that end users never need manual DLL handling. Do not redistribute this
zip outside your licensed deployment.
