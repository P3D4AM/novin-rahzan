'use client'

import { useEffect, useMemo, useState } from 'react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Badge } from '@/components/ui/badge'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/table'
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select'
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogFooter } from '@/components/ui/dialog'
import { Label } from '@/components/ui/label'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import { Switch } from '@/components/ui/switch'
import { SessionUser, Bootstrap, api, Person, full_name, STATUS_COLOR } from './types'
import { JalaliDateInput } from './jalali-ui'
import { useToast } from '@/hooks/use-toast'
import { Plus, Search, Pencil, Trash2, Building2, Briefcase, BadgeCheck, Upload, Download, Loader2 } from 'lucide-react'

interface Props { user: SessionUser; boot: Bootstrap; reloadBoot: () => void }

const emptyPerson = {
  personnelCode: '', firstName: '', lastName: '', nationalId: '', fatherName: '', gender: 'MALE',
  birthDate: '', phone: '', email: '', departmentId: '', jobTitleId: '', employmentTypeId: '',
  hireDate: '', status: 'ACTIVE', pin: '', annualLeaveDays: 26, defaultShiftId: '', deviceUserId: null as number | null,
}

export function PersonnelView({ user, boot, reloadBoot }: Props) {
  const [persons, setPersons] = useState<Person[]>([])
  const [q, setQ] = useState('')
  const [dept, setDept] = useState('all')
  const [dialogOpen, setDialogOpen] = useState(false)
  const [editing, setEditing] = useState<Person | null>(null)
  const [form, setForm] = useState({ ...emptyPerson })
  const [saving, setSaving] = useState(false)
  const [importOpen, setImportOpen] = useState(false)
  const [importing, setImporting] = useState(false)
  const { toast } = useToast()
  const isAdmin = user.role === 'ADMIN' || user.role === 'SUPER_ADMIN'

  const load = async () => {
    const res = await api<{ personnel: Person[] }>(`/api/personnel?q=${encodeURIComponent(q)}&departmentId=${dept === 'all' ? '' : dept}`)
    setPersons(res.personnel)
  }

  useEffect(() => { load()   }, [q, dept])

  const openNew = () => { setEditing(null); setForm({ ...emptyPerson }); setDialogOpen(true) }
  const openEdit = (p: Person) => {
    setEditing(p)
    setForm({
      personnelCode: p.personnelCode, firstName: p.firstName, lastName: p.lastName,
      nationalId: p.nationalId ?? '', fatherName: p.fatherName ?? '', gender: p.gender ?? 'MALE',
      birthDate: p.birthDate ?? '', phone: p.phone ?? '', email: p.email ?? '',
      departmentId: p.departmentId ?? '', jobTitleId: p.jobTitleId ?? '', employmentTypeId: p.employmentTypeId ?? '',
      hireDate: p.hireDate ?? '', status: p.status, pin: p.pin ?? '', annualLeaveDays: p.annualLeaveDays ?? 26,
      defaultShiftId: p.defaultShiftId ?? '', deviceUserId: p.deviceUserId ?? null,
    })
    setDialogOpen(true)
  }

  const save = async () => {
    if (!form.personnelCode || !form.firstName || !form.lastName) {
      toast({ title: 'خطا', description: 'کد پرسنلی، نام و نام خانوادگی الزامی است', variant: 'destructive' })
      return
    }
    setSaving(true)
    try {
      const payload = {
        ...form,
        departmentId: form.departmentId || null,
        jobTitleId: form.jobTitleId || null,
        employmentTypeId: form.employmentTypeId || null,
        defaultShiftId: form.defaultShiftId || null,
        deviceUserId: form.deviceUserId ?? null,
        birthDate: form.birthDate || null,
        hireDate: form.hireDate || null,
        nationalId: form.nationalId || null,
        fatherName: form.fatherName || null,
        phone: form.phone || null,
        email: form.email || null,
        pin: form.pin || undefined,
      }
      if (editing) {
        await api(`/api/personnel/${editing.id}`, { method: 'PUT', body: JSON.stringify(payload) })
      } else {
        await api('/api/personnel', { method: 'POST', body: JSON.stringify(payload) })
      }
      setDialogOpen(false)
      load()
      reloadBoot()
      toast({ title: 'ذخیره شد', description: 'پرونده پرسنلی با موفقیت ثبت شد' })
    } catch (err) {
      toast({ title: 'خطا', description: err instanceof Error ? err.message : 'خطا', variant: 'destructive' })
    } finally { setSaving(false) }
  }

  const remove = async (p: Person) => {
    if (!confirm(`حذف ${full_name(p)} و همه ترددهایش؟`)) return
    try {
      await api(`/api/personnel/${p.id}`, { method: 'DELETE' })
      load()
    } catch (err) {
      toast({ title: 'خطا', description: err instanceof Error ? err.message : 'خطا', variant: 'destructive' })
    }
  }

  const doImport = async (file: File) => {
    setImporting(true)
    try {
      const fd = new FormData()
      fd.append('file', file)
      fd.append('mode', 'personnel')
      const res = await fetch('/api/import', { method: 'POST', body: fd })
      const data = await res.json()
      if (!res.ok) throw new Error(data.error || 'خطا در ورود داده')
      toast({ title: 'ورود داده انجام شد', description: `${data.created} نفر اضافه، ${data.skipped} رد شد` })
      setImportOpen(false)
      load()
    } catch (err) {
      toast({ title: 'خطا', description: err instanceof Error ? err.message : 'خطا', variant: 'destructive' })
    } finally { setImporting(false) }
  }

  const exportExcel = async () => {
    const XLSX = await import('xlsx')
    const rows = persons.map((p) => ({
      'کد پرسنلی': p.personnelCode, 'نام': p.firstName, 'نام خانوادگی': p.lastName,
      'کد ملی': p.nationalId ?? '', 'موبایل': p.phone ?? '', 'دپارتمان': p.department?.name ?? '',
      'عنوان شغلی': p.jobTitle?.name ?? '', 'نوع استخدام': p.employmentType?.name ?? '',
      'شیفت': p.defaultShift?.name ?? '', 'تاریخ استخدام': p.hireDate ?? '', 'وضعیت': p.status === 'ACTIVE' ? 'فعال' : 'غیرفعال',
    }))
    const wb = XLSX.utils.book_new()
    XLSX.utils.book_append_sheet(wb, XLSX.utils.json_to_sheet(rows), 'پرسنل')
    XLSX.writeFile(wb, 'personnel.xlsx')
  }

  const deptCounts = useMemo(() => {
    const m = new Map<string, number>()
    for (const p of persons) {
      const d = p.department?.name ?? 'بدون دپارتمان'
      m.set(d, (m.get(d) ?? 0) + 1)
    }
    return Array.from(m.entries()).sort((a, b) => b[1] - a[1])
  }, [persons])

  return (
    <Tabs defaultValue="personnel" dir="rtl">
      <TabsList className="mb-4">
        <TabsTrigger value="personnel">پرسنل</TabsTrigger>
        <TabsTrigger value="org">ساختار سازمانی</TabsTrigger>
      </TabsList>

      <TabsContent value="personnel" className="space-y-4">
        <div className="flex flex-wrap items-center gap-2">
          <div className="relative flex-1 min-w-48">
            <Search className="absolute right-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
            <Input placeholder="جستجو: نام، کد پرسنلی، کد ملی…" value={q} onChange={(e) => setQ(e.target.value)} className="pr-9" />
          </div>
          <Select value={dept} onValueChange={setDept}>
            <SelectTrigger className="w-44"><SelectValue placeholder="همه دپارتمان‌ها" /></SelectTrigger>
            <SelectContent>
              <SelectItem value="all">همه دپارتمان‌ها</SelectItem>
              {boot.departments.map((d) => <SelectItem key={d.id} value={d.id}>{d.name}</SelectItem>)}
            </SelectContent>
          </Select>
          {isAdmin && (
            <>
              <Button onClick={openNew} className="gap-1.5"><Plus className="h-4 w-4" /> پرسنل جدید</Button>
              <Button variant="outline" onClick={() => setImportOpen(true)} className="gap-1.5"><Upload className="h-4 w-4" /> ورود از Excel</Button>
            </>
          )}
          <Button variant="outline" onClick={exportExcel} className="gap-1.5"><Download className="h-4 w-4" /> خروجی Excel</Button>
        </div>

        <Card>
          <CardContent className="p-0">
            <div className="max-h-[70vh] overflow-auto">
              <Table>
                <TableHeader className="sticky top-0 bg-background z-10">
                  <TableRow>
                    <TableHead>کد</TableHead>
                    <TableHead>نام و نام خانوادگی</TableHead>
                    <TableHead className="hidden md:table-cell">دپارتمان</TableHead>
                    <TableHead className="hidden lg:table-cell">عنوان شغلی</TableHead>
                    <TableHead className="hidden lg:table-cell">شیفت</TableHead>
                    <TableHead className="hidden sm:table-cell">موبایل</TableHead>
                    <TableHead>وضعیت</TableHead>
                    {isAdmin && <TableHead>عملیات</TableHead>}
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {persons.map((p) => (
                    <TableRow key={p.id} className="cursor-pointer" onClick={() => openEdit(p)}>
                      <TableCell dir="ltr" className="font-mono text-xs">{p.personnelCode}</TableCell>
                      <TableCell className="font-medium">{full_name(p)}</TableCell>
                      <TableCell className="hidden md:table-cell text-muted-foreground">{p.department?.name ?? '—'}</TableCell>
                      <TableCell className="hidden lg:table-cell text-muted-foreground">{p.jobTitle?.name ?? '—'}</TableCell>
                      <TableCell className="hidden lg:table-cell">
                        {p.defaultShift ? (
                          <span className="inline-flex items-center gap-1.5 text-sm">
                            <span className="h-2 w-2 rounded-full" style={{ background: p.defaultShift.color }} />
                            {p.defaultShift.name}
                          </span>
                        ) : '—'}
                      </TableCell>
                      <TableCell dir="ltr" className="hidden sm:table-cell text-muted-foreground text-xs">{p.phone ?? '—'}</TableCell>
                      <TableCell>
                        <span className={`px-2 py-0.5 rounded-full text-xs ${p.status === 'ACTIVE' ? 'bg-emerald-100 text-emerald-700 dark:bg-emerald-900 dark:text-emerald-200' : 'bg-gray-100 text-gray-600 dark:bg-gray-800'}`}>
                          {p.status === 'ACTIVE' ? 'فعال' : p.status === 'TERMINATED' ? 'قطع همکاری' : 'غیرفعال'}
                        </span>
                      </TableCell>
                      {isAdmin && (
                        <TableCell onClick={(e) => e.stopPropagation()}>
                          <div className="flex gap-1">
                            <Button variant="ghost" size="icon" className="h-8 w-8" onClick={() => openEdit(p)}><Pencil className="h-4 w-4" /></Button>
                            <Button variant="ghost" size="icon" className="h-8 w-8 text-destructive" onClick={() => remove(p)}><Trash2 className="h-4 w-4" /></Button>
                          </div>
                        </TableCell>
                      )}
                    </TableRow>
                  ))}
                  {persons.length === 0 && (
                    <TableRow><TableCell colSpan={8} className="text-center text-muted-foreground py-10">موردی یافت نشد</TableCell></TableRow>
                  )}
                </TableBody>
              </Table>
            </div>
          </CardContent>
        </Card>
      </TabsContent>

      <TabsContent value="org">
        <OrgManager boot={boot} reloadBoot={reloadBoot} deptCounts={deptCounts} />
      </TabsContent>

      {/* person dialog */}
      <Dialog open={dialogOpen} onOpenChange={setDialogOpen}>
        <DialogContent className="max-w-2xl max-h-[90vh] overflow-y-auto" dir="rtl">
          <DialogHeader>
            <DialogTitle>{editing ? `ویرایش ${full_name(editing)}` : 'پرونده پرسنلی جدید'}</DialogTitle>
          </DialogHeader>
          <div className="grid grid-cols-1 sm:grid-cols-2 gap-3">
            <div className="space-y-1.5"><Label>کد پرسنلی *</Label><Input dir="ltr" value={form.personnelCode} onChange={(e) => setForm({ ...form, personnelCode: e.target.value })} /></div>
            <div className="space-y-1.5"><Label>پین کیشی (۴ رقم)</Label><Input dir="ltr" maxLength={4} value={form.pin} onChange={(e) => setForm({ ...form, pin: e.target.value.replace(/\D/g, '') })} placeholder="مثلاً 1234" /></div>
            <div className="space-y-1.5"><Label>نام *</Label><Input value={form.firstName} onChange={(e) => setForm({ ...form, firstName: e.target.value })} /></div>
            <div className="space-y-1.5"><Label>نام خانوادگی *</Label><Input value={form.lastName} onChange={(e) => setForm({ ...form, lastName: e.target.value })} /></div>
            <div className="space-y-1.5"><Label>کد ملی</Label><Input dir="ltr" value={form.nationalId} onChange={(e) => setForm({ ...form, nationalId: e.target.value })} /></div>
            <div className="space-y-1.5"><Label>نام پدر</Label><Input value={form.fatherName} onChange={(e) => setForm({ ...form, fatherName: e.target.value })} /></div>
            <div className="space-y-1.5">
              <Label>جنسیت</Label>
              <Select value={form.gender} onValueChange={(v) => setForm({ ...form, gender: v })}>
                <SelectTrigger><SelectValue /></SelectTrigger>
                <SelectContent><SelectItem value="MALE">مرد</SelectItem><SelectItem value="FEMALE">زن</SelectItem></SelectContent>
              </Select>
            </div>
            <div className="space-y-1.5"><Label>تاریخ تولد (شمسی)</Label><JalaliDateInput value={form.birthDate} onChange={(v) => setForm({ ...form, birthDate: v })} /></div>
            <div className="space-y-1.5"><Label>موبایل</Label><Input dir="ltr" value={form.phone} onChange={(e) => setForm({ ...form, phone: e.target.value })} /></div>
            <div className="space-y-1.5"><Label>ایمیل</Label><Input dir="ltr" type="email" value={form.email} onChange={(e) => setForm({ ...form, email: e.target.value })} /></div>
            <div className="space-y-1.5">
              <Label>دپارتمان</Label>
              <Select value={form.departmentId || 'none'} onValueChange={(v) => setForm({ ...form, departmentId: v === 'none' ? '' : v })}>
                <SelectTrigger><SelectValue placeholder="—" /></SelectTrigger>
                <SelectContent>
                  <SelectItem value="none">—</SelectItem>
                  {boot.departments.map((d) => <SelectItem key={d.id} value={d.id}>{d.name}</SelectItem>)}
                </SelectContent>
              </Select>
            </div>
            <div className="space-y-1.5">
              <Label>عنوان شغلی</Label>
              <Select value={form.jobTitleId || 'none'} onValueChange={(v) => setForm({ ...form, jobTitleId: v === 'none' ? '' : v })}>
                <SelectTrigger><SelectValue placeholder="—" /></SelectTrigger>
                <SelectContent>
                  <SelectItem value="none">—</SelectItem>
                  {boot.jobTitles.map((d) => <SelectItem key={d.id} value={d.id}>{d.name}</SelectItem>)}
                </SelectContent>
              </Select>
            </div>
            <div className="space-y-1.5">
              <Label>نوع استخدام</Label>
              <Select value={form.employmentTypeId || 'none'} onValueChange={(v) => setForm({ ...form, employmentTypeId: v === 'none' ? '' : v })}>
                <SelectTrigger><SelectValue placeholder="—" /></SelectTrigger>
                <SelectContent>
                  <SelectItem value="none">—</SelectItem>
                  {boot.employmentTypes.map((d) => <SelectItem key={d.id} value={d.id}>{d.name}</SelectItem>)}
                </SelectContent>
              </Select>
            </div>
            <div className="space-y-1.5">
              <Label>شیفت پیش‌فرض</Label>
              <Select value={form.defaultShiftId || 'none'} onValueChange={(v) => setForm({ ...form, defaultShiftId: v === 'none' ? '' : v })}>
                <SelectTrigger><SelectValue placeholder="—" /></SelectTrigger>
                <SelectContent>
                  <SelectItem value="none">—</SelectItem>
                  {boot.shifts.map((s) => <SelectItem key={s.id} value={s.id}>{s.name}</SelectItem>)}
                </SelectContent>
              </Select>
            </div>
            <div className="space-y-1.5"><Label>تاریخ استخدام (شمسی)</Label><JalaliDateInput value={form.hireDate} onChange={(v) => setForm({ ...form, hireDate: v })} /></div>
            <div className="space-y-1.5"><Label>سنوات مرخصی سالانه (روز)</Label><Input type="number" dir="ltr" value={form.annualLeaveDays} onChange={(e) => setForm({ ...form, annualLeaveDays: Number(e.target.value) })} /></div>
            <div className="space-y-1.5"><Label>شناسه روی دستگاه (اختیاری)</Label><Input type="number" dir="ltr" value={form.deviceUserId ?? ''} onChange={(e) => setForm({ ...form, deviceUserId: e.target.value ? Number(e.target.value) : null })} placeholder="مثلاً 1001" /></div>
            <div className="space-y-1.5">
              <Label>وضعیت</Label>
              <Select value={form.status} onValueChange={(v) => setForm({ ...form, status: v })}>
                <SelectTrigger><SelectValue /></SelectTrigger>
                <SelectContent>
                  <SelectItem value="ACTIVE">فعال</SelectItem>
                  <SelectItem value="INACTIVE">غیرفعال</SelectItem>
                  <SelectItem value="TERMINATED">قطع همکاری</SelectItem>
                </SelectContent>
              </Select>
            </div>
          </div>
          <DialogFooter>
            <Button variant="outline" onClick={() => setDialogOpen(false)}>انصراف</Button>
            <Button onClick={save} disabled={saving}>{saving && <Loader2 className="h-4 w-4 animate-spin" />} ذخیره</Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>

      {/* import dialog */}
      <Dialog open={importOpen} onOpenChange={setImportOpen}>
        <DialogContent dir="rtl">
          <DialogHeader><DialogTitle>ورود پرسنل از فایل Excel / CSV</DialogTitle></DialogHeader>
          <p className="text-sm text-muted-foreground leading-7">
            ستون‌های قابل تشخیص: «کد پرسنلی» یا «کد»، «نام و نام خانوادگی» یا «نام»، «کد ملی»، «موبایل»
          </p>
          <input type="file" accept=".xlsx,.xls,.csv" className="w-full text-sm border rounded-lg p-3"
            onChange={(e) => e.target.files?.[0] && doImport(e.target.files[0])} disabled={importing} />
          {importing && <div className="flex items-center gap-2 text-sm"><Loader2 className="h-4 w-4 animate-spin" /> در حال پردازش…</div>}
        </DialogContent>
      </Dialog>
    </Tabs>
  )
}

/* ---------------- Org manager ---------------- */
function OrgManager({ boot, reloadBoot, deptCounts }: { boot: Bootstrap; reloadBoot: () => void; deptCounts: Array<[string, number]> }) {
  const { toast } = useToast()
  const [newName, setNewName] = useState('')

  const add = async (type: 'dept' | 'job' | 'emptype') => {
    if (!newName.trim()) return
    try {
      await api('/api/org', { method: 'POST', body: JSON.stringify({ type, name: newName.trim() }) })
      setNewName('')
      reloadBoot()
      toast({ title: 'اضافه شد' })
    } catch (err) {
      toast({ title: 'خطا', description: err instanceof Error ? err.message : '', variant: 'destructive' })
    }
  }

  const remove = async (type: 'dept' | 'job' | 'emptype', id: string) => {
    if (!confirm('حذف شود؟')) return
    try {
      await api(`/api/org?type=${type}&id=${id}`, { method: 'DELETE' })
      reloadBoot()
    } catch (err) {
      toast({ title: 'خطا', description: err instanceof Error ? err.message : '', variant: 'destructive' })
    }
  }

  const sections = [
    { type: 'dept' as const, title: 'دپارتمان‌ها', icon: Building2, items: boot.departments },
    { type: 'job' as const, title: 'عناوین شغلی', icon: Briefcase, items: boot.jobTitles },
    { type: 'emptype' as const, title: 'انواع استخدام', icon: BadgeCheck, items: boot.employmentTypes },
  ]

  return (
    <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
      {sections.map((s) => {
        const Icon = s.icon
        return (
          <Card key={s.type}>
            <CardHeader className="pb-3">
              <CardTitle className="text-base flex items-center gap-2"><Icon className="h-4 w-4 text-primary" /> {s.title}</CardTitle>
            </CardHeader>
            <CardContent className="space-y-3">
              <div className="flex gap-2">
                <Input placeholder={`نام ${s.title}…`} value={newName} onChange={(e) => setNewName(e.target.value)}
                  onKeyDown={(e) => e.key === 'Enter' && add(s.type)} />
                <Button size="icon" onClick={() => add(s.type)}><Plus className="h-4 w-4" /></Button>
              </div>
              <div className="max-h-72 overflow-y-auto space-y-1">
                {s.items.map((item) => {
                  const count = s.type === 'dept' ? deptCounts.find(([n]) => n === item.name)?.[1] : undefined
                  return (
                    <div key={item.id} className="flex items-center justify-between p-2 rounded-lg bg-muted/40 text-sm">
                      <span className="truncate">{item.name}{count ? <Badge variant="secondary" className="mr-2 text-[10px]">{count} نفر</Badge> : null}</span>
                      <Button variant="ghost" size="icon" className="h-7 w-7 text-destructive" onClick={() => remove(s.type, item.id)}>
                        <Trash2 className="h-3.5 w-3.5" />
                      </Button>
                    </div>
                  )
                })}
                {s.items.length === 0 && <p className="text-xs text-muted-foreground text-center py-4">خالی است</p>}
              </div>
            </CardContent>
          </Card>
        )
      })}
    </div>
  )
}
