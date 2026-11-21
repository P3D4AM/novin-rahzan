'use client'

import { useEffect, useState } from 'react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Badge } from '@/components/ui/badge'
import { Switch } from '@/components/ui/switch'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/table'
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import { Label } from '@/components/ui/label'
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogFooter } from '@/components/ui/dialog'
import { SessionUser, Bootstrap, api, Person, ROLE_FA, full_name, Role } from './types'
import { faDigits } from './jalali-ui'
import { useToast } from '@/hooks/use-toast'
import { Save, Plus, Trash2, Loader2, Bell, Shield, Building, Percent, Database, HardDriveDownload, DatabaseBackup, AlertTriangle } from 'lucide-react'

interface Props { user: SessionUser; boot: Bootstrap; reloadBoot: () => void }

const RULE_FA: Record<string, string> = {
  ABSENT: 'اعلان غیبت روزانه',
  LATE: 'اعلان تأخیر',
  CHECKIN: 'اعلان هر ورود',
  LEAVE_REQUEST: 'اعلان درخواست مرخصی',
  OT_REQUEST: 'اعلان درخواست اضافه‌کاری',
}

export function SettingsView({ user, boot, reloadBoot }: Props) {
  return (
    <Tabs defaultValue="general" dir="rtl">
      <TabsList className="mb-4 flex-wrap">
        <TabsTrigger value="general">عمومی</TabsTrigger>
        <TabsTrigger value="notifications">اعلان‌های هوشمند</TabsTrigger>
        <TabsTrigger value="users">کاربران و نقش‌ها</TabsTrigger>
        <TabsTrigger value="audit">گزارش فعالیت</TabsTrigger>
        {user.role === 'SUPER_ADMIN' && <TabsTrigger value="database">مدیریت دیتابیس</TabsTrigger>}
      </TabsList>
      <TabsContent value="general"><GeneralTab boot={boot} reloadBoot={reloadBoot} /></TabsContent>
      <TabsContent value="notifications"><RulesTab boot={boot} reloadBoot={reloadBoot} /></TabsContent>
      <TabsContent value="users"><UsersTab boot={boot} reloadBoot={reloadBoot} /></TabsContent>
      <TabsContent value="audit"><AuditTab /></TabsContent>
      {user.role === 'SUPER_ADMIN' && <TabsContent value="database"><DatabaseTab /></TabsContent>}
    </Tabs>
  )
}

/* -------- general -------- */
function GeneralTab({ boot, reloadBoot }: { boot: Bootstrap; reloadBoot: () => void }) {
  const [form, setForm] = useState({
    org_name: boot.settings?.org_name ?? '',
    ot_rate_normal: boot.settings?.ot_rate_normal ?? '1.4',
    ot_rate_friday: boot.settings?.ot_rate_friday ?? '1.6',
    ot_rate_holiday: boot.settings?.ot_rate_holiday ?? '2',
    // Phase 2 S3 (min_excess): overtime activation threshold (minutes) — gates
    // activation, never subtracted from the excess; server validates integer ≥ 0.
    ot_threshold_minutes: boot.settings?.ot_threshold_minutes ?? '30',
    sms_api_key: boot.settings?.sms_api_key ?? '',
    sms_sender: boot.settings?.sms_sender ?? '',
  })
  const [saving, setSaving] = useState(false)
  const { toast } = useToast()

  const save = async () => {
    setSaving(true)
    try {
      await api('/api/settings', { method: 'PUT', body: JSON.stringify({ settings: form }) })
      reloadBoot()
      toast({ title: 'تنظیمات ذخیره شد' })
    } catch (err) {
      toast({ title: 'خطا', description: err instanceof Error ? err.message : '', variant: 'destructive' })
    } finally { setSaving(false) }
  }

  const runSweep = async () => {
    try {
      const res = await api<{ checked: number; absents: number }>('/api/notifications', { method: 'POST' })
      toast({ title: 'بررسی غیبت انجام شد', description: `${faDigits(res.absents)} غیبت جدید از ${faDigits(res.checked)} نفر` })
    } catch (err) {
      toast({ title: 'خطا', description: err instanceof Error ? err.message : '', variant: 'destructive' })
    }
  }

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 gap-4 max-w-4xl">
      <Card>
        <CardHeader className="pb-3"><CardTitle className="text-base flex items-center gap-2"><Building className="h-4 w-4 text-primary" /> سازمان</CardTitle></CardHeader>
        <CardContent className="space-y-3">
          <div className="space-y-1.5"><Label>نام سازمان</Label><Input value={form.org_name} onChange={(e) => setForm({ ...form, org_name: e.target.value })} /></div>
        </CardContent>
      </Card>

      <Card>
        <CardHeader className="pb-3"><CardTitle className="text-base flex items-center gap-2"><Percent className="h-4 w-4 text-primary" /> ضرایب اضافه‌کاری</CardTitle></CardHeader>
        <CardContent className="space-y-3">
          <div className="grid grid-cols-3 gap-3">
            <div className="space-y-1.5"><Label className="text-xs">عادی</Label><Input dir="ltr" type="number" step="0.1" value={form.ot_rate_normal} onChange={(e) => setForm({ ...form, ot_rate_normal: e.target.value })} /></div>
            <div className="space-y-1.5"><Label className="text-xs">جمعه</Label><Input dir="ltr" type="number" step="0.1" value={form.ot_rate_friday} onChange={(e) => setForm({ ...form, ot_rate_friday: e.target.value })} /></div>
            <div className="space-y-1.5"><Label className="text-xs">تعطیل رسمی</Label><Input dir="ltr" type="number" step="0.1" value={form.ot_rate_holiday} onChange={(e) => setForm({ ...form, ot_rate_holiday: e.target.value })} /></div>
          </div>
          <div className="space-y-1.5 border-t pt-3">
            <Label className="text-xs">آستانه فعال‌شدن اضافه‌کاری (دقیقه)</Label>
            <Input dir="ltr" type="number" step="1" min="0" value={form.ot_threshold_minutes} onChange={(e) => setForm({ ...form, ot_threshold_minutes: e.target.value })} />
            <p className="text-[10px] text-muted-foreground">مازاد کارکرد فقط پس از عبور از این آستانه، اضافه‌کاری واقعی می‌شود؛ آستانه از مازاد کم نمی‌شود (پیش‌فرض ۳۰ دقیقه).</p>
          </div>
        </CardContent>
      </Card>

      <Card>
        <CardHeader className="pb-3"><CardTitle className="text-base flex items-center gap-2"><Bell className="h-4 w-4 text-primary" /> پیامک (کاوه‌نگار)</CardTitle></CardHeader>
        <CardContent className="space-y-3">
          <div className="space-y-1.5"><Label>کلید API</Label><Input dir="ltr" type="password" value={form.sms_api_key} onChange={(e) => setForm({ ...form, sms_api_key: e.target.value })} placeholder="اختیاری — برای ارسال پیامک" /></div>
          <div className="space-y-1.5"><Label>شماره فرستنده</Label><Input dir="ltr" value={form.sms_sender} onChange={(e) => setForm({ ...form, sms_sender: e.target.value })} /></div>
        </CardContent>
      </Card>

      <Card>
        <CardHeader className="pb-3"><CardTitle className="text-base">عمليات خودکار</CardTitle></CardHeader>
        <CardContent className="space-y-3">
          <p className="text-sm text-muted-foreground">بررسی غیبت دیروز و ارسال اعلان به مدیران:</p>
          <Button variant="outline" onClick={runSweep} className="gap-1.5"><Bell className="h-4 w-4" /> اجرای بررسی غیبت</Button>
        </CardContent>
      </Card>

      <div className="md:col-span-2">
        <Button onClick={save} disabled={saving} className="gap-1.5">{saving ? <Loader2 className="h-4 w-4 animate-spin" /> : <Save className="h-4 w-4" />} ذخیره تنظیمات</Button>
      </div>
    </div>
  )
}

/* -------- notification rules -------- */
function RulesTab({ boot, reloadBoot }: { boot: Bootstrap; reloadBoot: () => void }) {
  const { toast } = useToast()
  const [rules, setRules] = useState(boot.rules)

  const update = async (id: string, patch: Record<string, unknown>) => {
    setRules(rules.map((r) => (r.id === id ? { ...r, ...patch } as typeof r : r)))
    try {
      await api('/api/notification-rules', { method: 'PUT', body: JSON.stringify({ id, ...patch }) })
    } catch {
      toast({ title: 'خطا در ذخیره', variant: 'destructive' })
      reloadBoot()
    }
  }

  return (
    <Card className="max-w-3xl">
      <CardHeader className="pb-3"><CardTitle className="text-base flex items-center gap-2"><Bell className="h-4 w-4 text-primary" /> قوانین اعلان هوشمند</CardTitle></CardHeader>
      <CardContent className="space-y-2">
        {rules.map((r) => (
          <div key={r.id} className="flex items-center gap-3 p-3 rounded-lg bg-muted/40 flex-wrap">
            <Switch checked={r.enabled} onCheckedChange={(v) => update(r.id, { enabled: v })} />
            <span className="text-sm font-medium flex-1 min-w-36">{RULE_FA[r.type] ?? r.type}</span>
            {r.type === 'LATE' && (
              <div className="flex items-center gap-2 text-xs text-muted-foreground">
                آستانه:
                <Input dir="ltr" type="number" className="w-16 h-7" value={r.thresholdMin}
                  onChange={(e) => update(r.id, { thresholdMin: Number(e.target.value) })} />
                دقیقه
              </div>
            )}
            <Select value={r.notifyRole} onValueChange={(v) => update(r.id, { notifyRole: v })}>
              <SelectTrigger className="w-36 h-8 text-xs"><SelectValue /></SelectTrigger>
              <SelectContent>
                {(Object.keys(ROLE_FA) as Role[]).filter((x) => x !== 'EMPLOYEE').map((role) => (
                  <SelectItem key={role} value={role}>{ROLE_FA[role]}</SelectItem>
                ))}
              </SelectContent>
            </Select>
          </div>
        ))}
        <p className="text-xs text-muted-foreground pt-2">اعلان‌ها به صورت درون‌برنامه‌ای (زنگ بالا) ارسال می‌شوند و در داشبورد زنده قابل مشاهده‌اند.</p>
      </CardContent>
    </Card>
  )
}

/* -------- users -------- */
function UsersTab({ boot, reloadBoot }: { boot: Bootstrap; reloadBoot: () => void }) {
  const { toast } = useToast()
  const [open, setOpen] = useState(false)
  const [saving, setSaving] = useState(false)
  const [persons, setPersons] = useState<Person[]>([])
  const [form, setForm] = useState({ username: '', password: '', role: 'EMPLOYEE', fullName: '', personnelId: '' })

  useEffect(() => { api<{ personnel: Person[] }>('/api/personnel').then((r) => setPersons(r.personnel)).catch(() => {}) }, [])

  const add = async () => {
    if (!form.username || !form.password) { toast({ title: 'نام کاربری و رمز الزامی است', variant: 'destructive' }); return }
    setSaving(true)
    try {
      await api('/api/users', { method: 'POST', body: JSON.stringify({ ...form, personnelId: form.personnelId || null }) })
      setOpen(false)
      setForm({ username: '', password: '', role: 'EMPLOYEE', fullName: '', personnelId: '' })
      reloadBoot()
      toast({ title: 'کاربر ساخته شد' })
    } catch (err) {
      toast({ title: 'خطا', description: err instanceof Error ? err.message : '', variant: 'destructive' })
    } finally { setSaving(false) }
  }

  const toggleActive = async (u: { id: string; active: boolean }) => {
    await api('/api/users', { method: 'PUT', body: JSON.stringify({ id: u.id, active: !u.active }) })
    reloadBoot()
  }

  const remove = async (id: string) => {
    if (!confirm('حذف این کاربر؟')) return
    try {
      await api(`/api/users?id=${id}`, { method: 'DELETE' })
      reloadBoot()
    } catch (err) {
      toast({ title: 'خطا', description: err instanceof Error ? err.message : '', variant: 'destructive' })
    }
  }

  return (
    <Card className="max-w-3xl">
      <CardHeader className="pb-3 flex flex-row items-center justify-between">
        <CardTitle className="text-base flex items-center gap-2"><Shield className="h-4 w-4 text-primary" /> کاربران و سطوح دسترسی</CardTitle>
        <Button size="sm" onClick={() => setOpen(true)} className="gap-1"><Plus className="h-4 w-4" /> کاربر جدید</Button>
      </CardHeader>
      <CardContent className="p-0">
        <Table>
          <TableHeader>
            <TableRow><TableHead>نام کاربری</TableHead><TableHead>نام</TableHead><TableHead>نقش</TableHead><TableHead>پرسنل متصل</TableHead><TableHead>فعال</TableHead><TableHead /></TableRow>
          </TableHeader>
          <TableBody>
            {boot.users.map((u) => (
              <TableRow key={u.id}>
                <TableCell dir="ltr" className="font-mono text-xs">{u.username}</TableCell>
                <TableCell className="text-sm">{u.fullName}</TableCell>
                <TableCell><Badge variant={u.role === 'SUPER_ADMIN' ? 'default' : 'secondary'} className="text-[11px]">{ROLE_FA[u.role as Role] ?? u.role}</Badge></TableCell>
                <TableCell className="text-xs text-muted-foreground">{persons.find((p) => p.id === u.personnelId) ? full_name(persons.find((p) => p.id === u.personnelId)) : '—'}</TableCell>
                <TableCell><Switch checked={u.active} onCheckedChange={() => toggleActive(u)} /></TableCell>
                <TableCell><Button variant="ghost" size="icon" className="h-7 w-7 text-destructive" onClick={() => remove(u.id)}><Trash2 className="h-3.5 w-3.5" /></Button></TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </CardContent>

      <Dialog open={open} onOpenChange={setOpen}>
        <DialogContent dir="rtl" className="max-w-sm">
          <DialogHeader><DialogTitle>کاربر جدید</DialogTitle></DialogHeader>
          <div className="space-y-3">
            <div className="grid grid-cols-2 gap-2">
              <div className="space-y-1.5"><Label>نام کاربری *</Label><Input dir="ltr" value={form.username} onChange={(e) => setForm({ ...form, username: e.target.value })} /></div>
              <div className="space-y-1.5"><Label>رمز عبور *</Label><Input dir="ltr" type="password" value={form.password} onChange={(e) => setForm({ ...form, password: e.target.value })} /></div>
            </div>
            <div className="space-y-1.5"><Label>نام کامل</Label><Input value={form.fullName} onChange={(e) => setForm({ ...form, fullName: e.target.value })} /></div>
            <div className="space-y-1.5">
              <Label>نقش</Label>
              <Select value={form.role} onValueChange={(v) => setForm({ ...form, role: v })}>
                <SelectTrigger><SelectValue /></SelectTrigger>
                <SelectContent>
                  {(Object.keys(ROLE_FA) as Role[]).map((role) => <SelectItem key={role} value={role}>{ROLE_FA[role]}</SelectItem>)}
                </SelectContent>
              </Select>
            </div>
            <div className="space-y-1.5">
              <Label>پرسنل متصل (برای کارت من)</Label>
              <Select value={form.personnelId || 'none'} onValueChange={(v) => setForm({ ...form, personnelId: v === 'none' ? '' : v })}>
                <SelectTrigger><SelectValue /></SelectTrigger>
                <SelectContent>
                  <SelectItem value="none">—</SelectItem>
                  {persons.map((p) => <SelectItem key={p.id} value={p.id}>{full_name(p)}</SelectItem>)}
                </SelectContent>
              </Select>
            </div>
          </div>
          <DialogFooter>
            <Button variant="outline" onClick={() => setOpen(false)}>انصراف</Button>
            <Button onClick={add} disabled={saving}>{saving && <Loader2 className="h-4 w-4 animate-spin" />} ذخیره</Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </Card>
  )
}

/* -------- database management (Phase 1 — SUPER_ADMIN only) -------- */
interface DbStatusRes {
  status: {
    dbPath: string | null
    dbExists: boolean
    sizeBytes: number | null
    schemaOk: boolean
    hasBusinessData: boolean
    counts: {
      users: number; superAdmins: number; personnel: number; punches: number; rawPunches: number
      dailyAttendance: number; leaveRequests: number; overtimeRequests: number
      devices: number; shifts: number; auditLog: number
    } | null
    lastBackupAt: string | null
    lastBackupPath: string | null
    backupDir: string | null
    backups: Array<{ file: string; bytes: number; at: string }>
  }
}

const DELETED_FA = 'پرسنل و پرونده‌ها، ترددها و ترددهای خام، کارکرد روزانه، مرخصی‌ها و مأموریت‌ها، اضافه‌کاری‌ها، شیفت‌ها و تخصیص‌ها، تعطیلات، انواع مرخصی، ساختار سازمانی (دپارتمان/عنوان/نوع استخدام)'
const KEPT_FA = 'حساب‌های کاربری و رمزهای آن‌ها (ورود شما حفظ می‌شود)، دستگاه‌ها و تنظیمات اتصال و رمز شبکهٔ آن‌ها، تنظیمات نرم‌افزار (نام سازمان، کلید پیامک و…)، قوانین اعلان، سابقهٔ ممیزی'

function DbCountCard({ label, value }: { label: string; value?: number }) {
  return (
    <div className="rounded-lg bg-muted/40 p-3 min-w-24">
      <div className="text-lg font-bold">{value == null ? '—' : faDigits(value)}</div>
      <div className="text-[11px] text-muted-foreground">{label}</div>
    </div>
  )
}

function DatabaseTab() {
  const { toast } = useToast()
  const [st, setSt] = useState<DbStatusRes['status'] | null>(null)
  const [loadErr, setLoadErr] = useState('')
  const [busy, setBusy] = useState(false)
  // destructive-op dialog state: which op + which stage
  const [op, setOp] = useState<'reset' | 'seed' | null>(null)
  const [stage, setStage] = useState<'info' | 'confirm' | 'running' | 'done'>('info')
  const [phrase, setPhrase] = useState('')
  const [result, setResult] = useState<string>('')

  const load = async () => {
    try {
      setLoadErr('')
      const r = await api<DbStatusRes>('/api/db-admin')
      setSt(r.status)
    } catch (err) {
      setLoadErr(err instanceof Error ? err.message : 'خطا')
    }
  }

  useEffect(() => { load() }, [])

  const runBackup = async () => {
    setBusy(true)
    try {
      const r = await api<{ backup: { path: string; bytes: number } }>('/api/db-admin', { method: 'POST', body: JSON.stringify({ action: 'backup' }) })
      toast({ title: 'پشتیبان گرفته شد', description: `${faDigits(r.backup.bytes)} بایت — ${r.backup.path}` })
      load()
    } catch (err) {
      toast({ title: 'خطا در پشتیبان‌گیری', description: err instanceof Error ? err.message : '', variant: 'destructive' })
    } finally { setBusy(false) }
  }

  const CONFIRM = op === 'reset' ? 'پاک‌سازی' : 'نصب نمونه'

  const startOp = (which: 'reset' | 'seed') => {
    setOp(which); setStage('info'); setPhrase(''); setResult('')
  }

  const execOp = async () => {
    setStage('running')
    try {
      const r = await api<{ backup?: { path: string; bytes: number } | null; wiped?: Record<string, number>; seeded?: { persons: number; punches: number; usersCreated: string[] } }>('/api/db-admin', {
        method: 'POST', body: JSON.stringify({ action: op, confirm: phrase }),
      })
      const lines: string[] = []
      if (r.backup) lines.push(`پشتیبان: ${r.backup.path} (${faDigits(r.backup.bytes)} بایت)`)
      if (r.wiped) lines.push(`حذف شد: ${faDigits(r.wiped.personnel ?? 0)} پرسنل، ${faDigits(r.wiped.punchRecord ?? 0)} تردد، ${faDigits(r.wiped.dailyAttendance ?? 0)} کارکرد روزانه`)
      if (r.seeded) lines.push(`ایجاد شد: ${faDigits(r.seeded.persons)} پرسنل و ${faDigits(r.seeded.punches)} تردد نمونه`)
      setResult(lines.join('\n'))
      setStage('done')
      load()
    } catch (err) {
      setStage('confirm')
      toast({ title: 'عملیات ناموفق بود', description: err instanceof Error ? err.message : '', variant: 'destructive' })
    }
  }

  return (
    <div className="grid grid-cols-1 gap-4 max-w-4xl">
      <Card>
        <CardHeader className="pb-3">
          <CardTitle className="text-base flex items-center gap-2"><Database className="h-4 w-4 text-primary" /> وضعیت دیتابیس</CardTitle>
        </CardHeader>
        <CardContent className="space-y-3">
          {loadErr && <div className="text-sm text-destructive">{loadErr}</div>}
          {!st && !loadErr && <div className="flex items-center gap-2 text-sm text-muted-foreground"><Loader2 className="h-4 w-4 animate-spin" /> در حال خواندن…</div>}
          {st && (
            <>
              <div className="text-xs text-muted-foreground">فایل: <span dir="ltr">{st.dbPath ?? '—'}</span></div>
              <div className="flex gap-2 flex-wrap">
                <DbCountCard label="پرسنل" value={st.counts?.personnel} />
                <DbCountCard label="ترددها" value={st.counts?.punches} />
                <DbCountCard label="تردد خام" value={st.counts?.rawPunches} />
                <DbCountCard label="کارکرد روزانه" value={st.counts?.dailyAttendance} />
                <DbCountCard label="کاربران" value={st.counts?.users} />
                <DbCountCard label="دستگاه‌ها" value={st.counts?.devices} />
              </div>
              <div className="text-[11px] text-muted-foreground">
                حجم فایل فقط اطلاعات است و معیار تشخیص خالی‌بودن دیتابیس نیست؛ تشخیص بر اساس تعداد رکوردهای واقعی انجام می‌شود.
              </div>
              <div className="border-t pt-3 space-y-2">
                <div className="text-sm">آخرین پشتیبان: {st.lastBackupAt ? faDigits(st.lastBackupAt.replace('T', ' ').slice(0, 16)) : '—'}</div>
                {st.backups.length > 0 && (
                  <div className="text-xs text-muted-foreground space-y-1">
                    {st.backups.slice(0, 5).map((b) => <div key={b.file} dir="ltr" className="truncate">{b.file} — {faDigits(b.bytes)} bytes</div>)}
                  </div>
                )}
                <div className="text-[11px] text-muted-foreground">بازیابی دستی: برنامه را STOP کنید، فایل پشتیبان را جای db/custom.db قرار دهید و START کنید.</div>
              </div>
            </>
          )}
        </CardContent>
      </Card>

      <Card>
        <CardHeader className="pb-3"><CardTitle className="text-base flex items-center gap-2"><DatabaseBackup className="h-4 w-4 text-primary" /> پشتیبان‌گیری</CardTitle></CardHeader>
        <CardContent>
          <p className="text-sm text-muted-foreground mb-2">یک Snapshot سازگار و قابل‌بازیابی از کل دیتابیس در پوشهٔ db/backups ایجاد می‌کند.</p>
          <div className="flex gap-2">
            <Button variant="outline" onClick={runBackup} disabled={busy} className="gap-1.5">
              {busy ? <Loader2 className="h-4 w-4 animate-spin" /> : <HardDriveDownload className="h-4 w-4" />} تهیه پشتیبان
            </Button>
            <Button variant="ghost" size="sm" onClick={load} disabled={busy}>به‌روزرسانی وضعیت</Button>
          </div>
        </CardContent>
      </Card>

      <Card className="border-destructive/40">
        <CardHeader className="pb-3"><CardTitle className="text-base flex items-center gap-2 text-destructive"><AlertTriangle className="h-4 w-4" /> عملیات مخرب (فقط مدیر ارشد)</CardTitle></CardHeader>
        <CardContent className="grid grid-cols-1 md:grid-cols-2 gap-3">
          <div className="space-y-2">
            <p className="text-sm">پاک‌سازی کامل اطلاعات و ایجاد دیتابیس خالی (اسکیما باقی می‌ماند).</p>
            <Button variant="destructive" onClick={() => startOp('reset')} className="gap-1"><Trash2 className="h-4 w-4" /> پاک‌سازی دیتابیس</Button>
          </div>
          <div className="space-y-2">
            <p className="text-sm">نصب اطلاعات نمونه (Demo) برای آزمایش نرم‌افزار.</p>
            <Button variant="outline" onClick={() => startOp('seed')} className="gap-1"><Database className="h-4 w-4" /> نصب اطلاعات نمونه</Button>
          </div>
        </CardContent>
      </Card>

      <Dialog open={!!op} onOpenChange={(o) => { if (!o && stage !== 'running') { setOp(null) } }}>
        <DialogContent dir="rtl" className="max-w-lg">
          <DialogHeader>
            <DialogTitle>{op === 'reset' ? 'پاک‌سازی کامل دیتابیس' : 'نصب اطلاعات نمونه'}</DialogTitle>
          </DialogHeader>

          {stage === 'info' && (
            <div className="space-y-3">
              <div className="rounded-md bg-destructive/10 border border-destructive/30 p-3 text-sm space-y-1">
                <div className="font-bold text-destructive">حذف می‌شود:</div>
                <div>{DELETED_FA}</div>
              </div>
              <div className="rounded-md bg-muted/60 border p-3 text-sm space-y-1">
                <div className="font-bold">حفظ می‌شود:</div>
                <div>{KEPT_FA}</div>
              </div>
              <p className="text-xs text-muted-foreground">قبل از اجرا، به‌طور خودکار یک پشتیبان قابل‌بازیابی تهیه می‌شود و مسیر آن نمایش داده خواهد شد. عملیات در AuditLog ثبت می‌شود.</p>
              {op === 'seed' && <p className="text-xs text-muted-foreground">حساب‌های نمونه فقط برای نام‌های کاربری آزاد ساخته می‌شوند؛ رمز حساب‌های موجود تغییر نمی‌کند.</p>}
              <DialogFooter>
                <Button variant="outline" onClick={() => setOp(null)}>انصراف</Button>
                <Button variant={op === 'reset' ? 'destructive' : 'default'} onClick={() => setStage('confirm')}>متوجه‌ام، ادامه</Button>
              </DialogFooter>
            </div>
          )}

          {stage === 'confirm' && (
            <div className="space-y-3">
              <div className="text-sm font-bold">تأیید نهایی</div>
              <div className="text-xs text-muted-foreground">برای تأیید، عبارت زیر را دقیقاً وارد کنید:</div>
              <div className="text-center text-lg font-bold select-none rounded bg-muted/60 border py-2" dir="rtl">{CONFIRM}</div>
              <Input value={phrase} onChange={(e) => setPhrase(e.target.value)} placeholder="عبارت تأیید" autoFocus />
              <div className="text-xs text-destructive">این عملیات قابل بازگشت با پشتیبان است اما داده‌های فعلی بالفعل حذف/جایگزین می‌شوند.</div>
              <DialogFooter>
                <Button variant="outline" onClick={() => setOp(null)}>انصراف</Button>
                <Button variant={op === 'reset' ? 'destructive' : 'default'} disabled={phrase.trim() !== CONFIRM || busy} onClick={execOp} className="gap-1.5">
                  {busy ? <Loader2 className="h-4 w-4 animate-spin" /> : null} اجرای نهایی
                </Button>
              </DialogFooter>
            </div>
          )}

          {stage === 'running' && (
            <div className="flex items-center gap-3 py-6 justify-center text-sm"><Loader2 className="h-5 w-5 animate-spin" /> در حال اجرای عملیات… (چند لحظه)</div>
          )}

          {stage === 'done' && (
            <div className="space-y-3">
              <div className="text-sm font-bold text-green-600">عملیات انجام شد.</div>
              {result && <div className="text-xs text-muted-foreground whitespace-pre-line">{result}</div>}
              {op === 'reset' && <div className="text-xs text-muted-foreground">برای ادامه، دوباره وارد شوید (نشست‌ها پاک شده‌اند) — حساب و رمز فعلی شما همان است.</div>}
              <DialogFooter><Button onClick={() => setOp(null)}>بستن</Button></DialogFooter>
            </div>
          )}
        </DialogContent>
      </Dialog>
    </div>
  )
}

/* -------- audit -------- */
function AuditTab() {
  const [logs, setLogs] = useState<Array<{ id: string; action: string; entity?: string | null; entityId?: string | null; details?: string | null; createdAt: string }>>([])

  useEffect(() => {
    api<{ logs: typeof logs }>('/api/users').then((r) => setLogs(r.logs)).catch(() => {})
  }, [])

  const ACTION_FA: Record<string, string> = { PUNCH_CREATE: 'ثبت تردد', PUNCH_EDIT: 'ویرایش تردد', PUNCH_DELETE: 'حذف تردد' }

  return (
    <Card className="max-w-3xl">
      <CardHeader className="pb-3"><CardTitle className="text-base">گزارش فعالیت مدیران</CardTitle></CardHeader>
      <CardContent className="p-0">
        <div className="max-h-[60vh] overflow-auto">
          <Table>
            <TableHeader><TableRow><TableHead>زمان</TableHead><TableHead>عملیات</TableHead><TableHead>جزئیات</TableHead></TableRow></TableHeader>
            <TableBody>
              {logs.map((l) => (
                <TableRow key={l.id}>
                  <TableCell className="text-xs" dir="ltr">{faDigits(l.createdAt.replace('T', ' ').slice(0, 16))}</TableCell>
                  <TableCell className="text-sm">{ACTION_FA[l.action] ?? l.action}</TableCell>
                  <TableCell className="text-xs text-muted-foreground truncate max-w-60">{l.details ?? '—'}</TableCell>
                </TableRow>
              ))}
              {logs.length === 0 && <TableRow><TableCell colSpan={3} className="text-center text-muted-foreground py-8">رکوردی نیست</TableCell></TableRow>}
            </TableBody>
          </Table>
        </div>
      </CardContent>
    </Card>
  )
}
