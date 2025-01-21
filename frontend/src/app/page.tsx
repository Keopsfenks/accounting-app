import Link from "next/link"
import { Button } from "@/components/ui/button"
import { Card, CardHeader, CardTitle, CardDescription, CardContent } from "@/components/ui/card"
import { ArrowRight, BarChart2, Users, Receipt, ShoppingCart, PiggyBank, Layers } from "lucide-react"
import {JSX} from "react";

export default function Home() {
  return (
      <div className="flex flex-col min-h-screen">
        <header className="bg-primary text-primary-foreground py-4">
          <div className="container mx-auto flex justify-between items-center">
            <h1 className="text-2xl font-bold">MuhasebeApp</h1>
            <Link href="/login">
              <Button variant="secondary">Giriş Yap</Button>
            </Link>
          </div>
        </header>

        <main className="flex-grow container mx-auto py-8">
          <section className="text-center mb-12">
            <h2 className="text-4xl font-bold mb-4">Muhasebe İşlemlerinizi Kolaylaştırın</h2>
            <p className="text-xl text-muted-foreground">
              Kapsamlı ve kullanıcı dostu muhasebe çözümümüzle işletmenizi yönetin
            </p>
          </section>

          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            <FeatureCard
                icon={<ShoppingCart className="h-8 w-8" />}
                title="Ürün Yönetimi"
                description="Hangi ürünlerin satıldığını takip edin, kategorilere ayırın ve stok yönetimini kolaylaştırın."
            />
            <FeatureCard
                icon={<Users className="h-8 w-8" />}
                title="Müşteri Yönetimi"
                description="Müşteri bilgilerini ekleyin, düzenleyin ve müşteri ilişkilerinizi güçlendirin."
            />
            <FeatureCard
                icon={<Receipt className="h-8 w-8" />}
                title="Faturalama"
                description="Faturalı satış ve düz satış işlemlerini hızlıca gerçekleştirin."
            />
            <FeatureCard
                icon={<PiggyBank className="h-8 w-8" />}
                title="Tahsilat Takibi"
                description="Ödemeleri takip edin, tahsilatları yönetin ve nakit akışınızı optimize edin."
            />
            <FeatureCard
                icon={<BarChart2 className="h-8 w-8" />}
                title="Kasa ve Banka"
                description="Kasa ve banka işlemlerinizi tek bir yerden yönetin, finansal durumunuzu anlık görüntüleyin."
            />
            <FeatureCard
                icon={<Layers className="h-8 w-8" />}
                title="Kategorizasyon"
                description="Ürünlerinizi kategorilere ayırarak daha iyi organize edin ve raporlama yapın."
            />
          </div>

          <div className="text-center mt-12">
            <Link href="/login">
              <Button size="lg">
                Hemen Başlayın
                <ArrowRight className="ml-2 h-4 w-4" />
              </Button>
            </Link>
          </div>
        </main>

        <footer className="bg-secondary py-6">
          <div className="container mx-auto text-center text-secondary-foreground">
            <p>&copy; 2023 MuhasebeApp. Tüm hakları saklıdır.</p>
          </div>
        </footer>
      </div>
  )
}

function FeatureCard({ icon, title, description } : { icon: JSX.Element, title: string, description: string }) {
  return (
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center">
            {icon}
            <span className="ml-2">{title}</span>
          </CardTitle>
        </CardHeader>
        <CardContent>
          <CardDescription>{description}</CardDescription>
        </CardContent>
      </Card>
  )
}

