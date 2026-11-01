'use client'

import { useEffect, useState } from 'react'
import { LoginView } from '@/components/app/login'
import { AppShell } from '@/components/app/shell'
import { KioskView } from '@/components/app/kiosk'
import { api, SessionUser } from '@/components/app/types'

export default function Home() {
  const [user, setUser] = useState<SessionUser | null>(null)
  const [checking, setChecking] = useState(true)
  const [kiosk, setKiosk] = useState(false)

  useEffect(() => {
    api<{ user: SessionUser | null }>('/api/auth')
      .then((res) => setUser(res.user))
      .catch(() => setUser(null))
      .finally(() => setChecking(false))
  }, [])

  async function logout() {
    try { await api('/api/auth', { method: 'DELETE' }) } catch { /* ignore */ }
    setUser(null)
    setKiosk(false)
  }

  if (checking) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-background">
        <div className="text-center space-y-3">
          <div className="text-4xl font-bold text-primary">حضور+</div>
          <div className="text-muted-foreground text-sm">در حال بررسی نشست…</div>
        </div>
      </div>
    )
  }

  if (kiosk) return <KioskView onBack={() => setKiosk(false)} />

  if (!user) return <LoginView onLogin={setUser} onKiosk={() => setKiosk(true)} />

  return <AppShell user={user} onLogout={logout} />
}
