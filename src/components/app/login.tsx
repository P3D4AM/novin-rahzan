'use client'

import { useState } from 'react'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { api, SessionUser } from './types'
import { Fingerprint, QrCode, LogIn, Loader2 } from 'lucide-react'

export function LoginView({ onLogin, onKiosk }: { onLogin: (user: SessionUser) => void; onKiosk: () => void }) {
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState('')

  async function submit(e: React.FormEvent) {
    e.preventDefault()
    setLoading(true)
    setError('')
    try {
      const res = await api<{ user: SessionUser }>('/api/auth', {
        method: 'POST',
        body: JSON.stringify({ username, password }),
      })
      onLogin(res.user)
    } catch (err) {
      setError(err instanceof Error ? err.message : 'خطا در ورود')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-emerald-50 via-teal-50 to-cyan-50 dark:from-gray-900 dark:via-gray-900 dark:to-gray-800 p-4">
      <div className="w-full max-w-md space-y-4">
        <div className="text-center space-y-2">
          <div className="inline-flex h-16 w-16 items-center justify-center rounded-2xl bg-primary text-primary-foreground shadow-lg">
            <Fingerprint className="h-8 w-8" />
          </div>
          <h1 className="text-3xl font-bold text-foreground">حضور+</h1>
          <p className="text-muted-foreground">سامانه هوشمند حضور و غیاب</p>
        </div>

        <Card className="shadow-xl border-border/60">
          <CardHeader className="pb-2">
            <CardTitle className="text-lg text-center">ورود به سامانه</CardTitle>
          </CardHeader>
          <CardContent>
            <form onSubmit={submit} className="space-y-4">
              <div className="space-y-2">
                <Input
                  placeholder="نام کاربری"
                  value={username}
                  onChange={(e) => setUsername(e.target.value)}
                  autoComplete="username"
                  className="h-11"
                />
              </div>
              <div className="space-y-2">
                <Input
                  type="password"
                  placeholder="رمز عبور"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  autoComplete="current-password"
                  className="h-11"
                />
              </div>
              {error && <p className="text-sm text-destructive text-center">{error}</p>}
              <Button type="submit" className="w-full h-11 text-base" disabled={loading}>
                {loading ? <Loader2 className="h-5 w-5 animate-spin" /> : <LogIn className="h-5 w-5" />}
                ورود
              </Button>
            </form>

            <div className="mt-4 pt-4 border-t space-y-2">
              <Button variant="outline" className="w-full h-11" onClick={onKiosk} type="button">
                <QrCode className="h-5 w-5" />
                حالت کیشی (ثبت حضور بدون ورود)
              </Button>
              <div className="text-xs text-muted-foreground text-center leading-6 bg-muted rounded-lg p-3">
                <div className="font-semibold mb-1">حساب‌های نمونه:</div>
                مدیر ارشد: <span dir="ltr">admin / admin123</span>
                <br />
                منابع انسانی: <span dir="ltr">hr / hr123</span>
                <br />
                تأییدکننده: <span dir="ltr">manager / manager123</span>
                <br />
                کارمند: <span dir="ltr">ali / ali123</span>
              </div>
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  )
}
