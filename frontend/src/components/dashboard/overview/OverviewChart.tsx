import React from 'react'
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Bar, BarChart, ResponsiveContainer, XAxis, YAxis } from "recharts"

type CustomerData = {
	issueDate: string; // Tarih stringi (ör. "28 Ocak 2024")
};

type Props = {
	className?: string;
	customerDetail: CustomerData[];
	customerInvoice: CustomerData[];
};

export default function OverviewChart({ className, customerDetail, customerInvoice }: Props) {
	const combinedData = [...customerDetail, ...customerInvoice];

	const groupedData = combinedData.reduce((acc: Record<string, number>, item) => {
		const date = item.issueDate.split(" ")[1];
		acc[date] = (acc[date] || 0) + 1;
		return acc;
	}, {});

	const chartData = Object.entries(groupedData).map(([date, total]) => ({
		name: date,
		total,
	}));

	return (
		<Card className={className}>
			<CardHeader>
				<CardTitle>Sales Overview</CardTitle>
			</CardHeader>
			<CardContent className="pl-2">
				<ResponsiveContainer width="100%" height={350}>
					<BarChart data={chartData}>
						<XAxis dataKey="name" stroke="#888888" fontSize={12} tickLine={false} axisLine={false} />
						<YAxis stroke="#888888" fontSize={12} tickLine={false} axisLine={false} tickFormatter={(value) => `${value}`} />
						<Bar dataKey="total" fill="currentColor" radius={[4, 4, 0, 0]} className="fill-primary" />
					</BarChart>
				</ResponsiveContainer>
			</CardContent>
		</Card>
	);
}

