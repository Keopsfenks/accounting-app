import React from 'react';
import {Card, CardContent, CardHeader, CardTitle} from "@/components/ui/card";
import {CustomerModel} from "@/ResponseModel/CustomerModel";

// Türkçe tarih formatını ISO formatına çeviren yardımcı fonksiyon
const convertTurkishDateToISO = (turkishDate: string) => {
	const months: { [key: string]: string } = {
		'Ocak': '01',
		'Şubat': '02',
		'Mart': '03',
		'Nisan': '04',
		'Mayıs': '05',
		'Haziran': '06',
		'Temmuz': '07',
		'Ağustos': '08',
		'Eylül': '09',
		'Ekim': '10',
		'Kasım': '11',
		'Aralık': '12'
	};

	const [day, month, year] = turkishDate.split(' ');
	const monthNumber = months[month];
	return `${year}-${monthNumber}-${day.padStart(2, '0')}`;
};

const isSameDate = (date1: string) => {
	return convertTurkishDateToISO(date1);
};


export default function RecentSales({
										className,
										customers
									}: {
	className?: string,
	customers: CustomerModel[]
}) {

	const recentSales = customers
		.filter(customer =>
			customer.invoices.some(invoice => isSameDate(invoice.issueDate)) ||
			customer.details.some(detail => isSameDate(detail.issueDate))
		)
		.map(customer => {
			const invoiceAmount = customer.invoices
				.filter(invoice => isSameDate(invoice.issueDate))
				.reduce((sum, invoice) => sum + invoice.withdrawalAmount, 0);

			const detailsAmount = customer.details
				.filter(detail => isSameDate(detail.issueDate))
				.reduce((sum, detail) => sum + detail.withdrawalAmount, 0);

			const totalAmount = invoiceAmount + detailsAmount;

			return {
				name: customer.name,
				phone: customer.phone || 'N/A',
				amount: totalAmount >= 0 ? `-₺${totalAmount.toLocaleString('tr-TR', {minimumFractionDigits: 2})}`
					: `-₺${Math.abs(totalAmount).toLocaleString('tr-TR', {minimumFractionDigits: 2})}`,
				invoiceAmount,
				detailsAmount
			};
		});

	return (
		<Card className={className}>
			<CardHeader>
				<CardTitle>Recent Sales</CardTitle>
			</CardHeader>
			<CardContent>
				<div className="space-y-8">
					{recentSales.map((sale, index) => (
						<div key={index} className="flex items-center">
							<div className="space-y-1">
								<p className="text-sm font-medium leading-none">{sale.name}</p>
								<p className="text-sm text-muted-foreground">{sale.phone}</p>
								<div className="text-xs text-muted-foreground">
									{sale.invoiceAmount > 0 && (
										<span className="mr-2">Faturalı: ₺{sale.invoiceAmount.toLocaleString('tr-TR')}</span>
									)}
									{sale.detailsAmount > 0 && (
										<span>Düz Satış: ₺{sale.detailsAmount.toLocaleString('tr-TR')}</span>
									)}
								</div>
							</div>
							<div className="ml-auto font-medium">{sale.amount}</div>
						</div>
					))}
					{recentSales.length === 0 && (
						<p className="text-sm text-muted-foreground">Bu tarihte satış bulunamadı.</p>
					)}
				</div>
			</CardContent>
		</Card>
	);
}