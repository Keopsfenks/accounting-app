import React from 'react'
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Banknote, Users, CreditCard, HandCoins } from 'lucide-react'

const icons = {
	dollar: Banknote,
	users: Users,
	'credit-card': CreditCard,
	receivable: HandCoins,
}

interface MetricCardProps {
	title: string
	value: string
	description: string
	icon: keyof typeof icons
}

export default function MetricCard({ title, value, description, icon }: MetricCardProps) {
	const Icon = icons[icon]
	return (
		<Card>
			<CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
				<CardTitle className="text-sm font-medium">{title}</CardTitle>
				<Icon className="h-4 w-4 text-muted-foreground" />
			</CardHeader>
			<CardContent>
				<div className="text-2xl font-bold">{value}</div>
				<p className="text-xs text-muted-foreground">{description}</p>
			</CardContent>
		</Card>
	)
}

