import {CustomerInvoiceModel} from "@/ResponseModel/CustomerModel";
import {Card, CardContent, CardDescription, CardHeader, CardTitle} from "@/components/ui/card";
import {Company} from "@/Models/Company";
import React, { useEffect, useRef } from "react";
import {Label} from "@/components/ui/label";
import {Input} from "@/components/ui/input";
import {Badge} from "@/components/ui/badge";
import {Table, TableBody, TableCell, TableHead, TableHeader, TableRow} from "@/components/ui/table";
import {ProductModel} from "@/ResponseModel/ProductModel";
import CashProceedsForm from "@/components/dashboard/cash-proceeds-form";

interface ExpandedInvoicePanelProps {
	invoice: CustomerInvoiceModel;
	companies: Company[];
	isExpanded: boolean;
	products: ProductModel[]
}

export function ExpandedInvoicePanel({ invoice, companies, isExpanded, products }: ExpandedInvoicePanelProps) {
	const contentRef = useRef<HTMLDivElement>(null);

	useEffect(() => {
		if (contentRef.current) {
			if (isExpanded) {
				contentRef.current.style.maxHeight = `${contentRef.current.scrollHeight}px`;
			} else {
				contentRef.current.style.maxHeight = '0px';
			}
		}
	}, [isExpanded]);

	return (
		<div
			ref={contentRef}
			className="overflow-y-auto transition-all duration-300 ease-in-out"
			style={{ maxHeight: 0 }}
		>
			<div className="p-4 bg-gray-50 dark:bg-gray-800 border-t border-gray-200 dark:border-gray-700">
				<h3 className="text-lg font-semibold mb-2">Additional Information</h3>
				<div className="space-y-4">
					<div className="grid grid-cols-1 md:grid-cols-3 gap-4">
						<Card className="p-4 bg-green-50">
							<div className="text-sm text-green-700">Deposit Amount</div>
							<div className="text-2xl font-semibold text-green-700">{invoice.depositAmount}</div>
						</Card>
						<Card className="p-4 bg-red-50">
							<div className="text-sm text-red-700">Withdraw Amount</div>
							<div className="text-2xl font-semibold text-red-700">{invoice.withdrawalAmount}</div>
						</Card>
						<Card className="p-4 bg-yellow-50">
							<div className="text-sm text-yellow-700">Total Amount</div>
							<div className="text-2xl font-semibold text-yellow-700">{invoice.totalAmount}</div>
						</Card>
					</div>
					<CashProceedsForm data={invoice} type="invoice"/>
					<div className="grid grid-cols-3 gap-3">
						<Card>
							<CardHeader>
								<CardTitle>Processor: {Object.keys(invoice.processor)}</CardTitle>
								<CardDescription>The user making the invoice transaction</CardDescription>
							</CardHeader>
						</Card>
						<Card>
							<CardHeader>
								<CardTitle>Id:</CardTitle>
								<CardDescription>{invoice.id}</CardDescription>
							</CardHeader>
						</Card>
						<Card>
							<CardHeader>
								<CardTitle>Company: </CardTitle>
								<CardDescription>{companies.find((value) => value.Id == invoice.companyId)?.Name}</CardDescription>
							</CardHeader>
						</Card>
					</div>
					<div className="grid grid-cols-2 gap-3">
						<Card>
							<CardHeader>
								<CardTitle><StatusBadge status={invoice.currencyType}/></CardTitle>
								<CardDescription>The currency in which the invoice was created</CardDescription>
							</CardHeader>
						</Card>
						<Card>
							<CardHeader>
								<CardTitle><StatusBadge status={invoice.operation}/></CardTitle>
								<CardDescription>Customer which select payment type</CardDescription>
							</CardHeader>
						</Card>
					</div>
					<Card>
						<CardHeader>
							<CardTitle>Cash Proceeds</CardTitle>
						</CardHeader>
						<CardContent>
							<Table>
								<TableHeader>
									<TableRow>
										<TableHead>Issue Date</TableHead>
										<TableHead>Amount</TableHead>
										<TableHead>Payment Type</TableHead>
										<TableHead>Operation</TableHead>
										<TableHead>Description</TableHead>
									</TableRow>
								</TableHeader>
								<TableBody>
									{invoice.cashProceeds.map((item) => (
										<TableRow key={item.id}>
											<TableCell>{item.issueDate}</TableCell>
											<TableCell>{item.amount}</TableCell>
											<TableCell>{StatusBadge({status: item.paymentType})}</TableCell>
											<TableCell>{StatusBadge({status: item.operation})}</TableCell>
											<TableCell>{item.description}</TableCell>
										</TableRow>
									))}
								</TableBody>
							</Table>
						</CardContent>
					</Card>
					<Card>
						<CardHeader>
							<CardTitle>Products</CardTitle>
						</CardHeader>
						<CardContent>
							<Table>
								<TableHeader>
									<TableRow>
										<TableHead>Product ID</TableHead>
										<TableHead>Quantity</TableHead>
										<TableHead>Unit Price</TableHead>
										<TableHead>Discount (%)</TableHead>
										<TableHead>Tax Rate (%)</TableHead>
										<TableHead>Total</TableHead>
										<TableHead></TableHead>
									</TableRow>
								</TableHeader>
								<TableBody>
									{invoice.products.map((item) => (
										<TableRow key={item.id}>
											<TableCell>{products.find((product) => product.id == item.productId)?.name}</TableCell>
											<TableCell>{item.pricing.quantity}</TableCell>
											<TableCell>{item.pricing.unitPrice}</TableCell>
											<TableCell>{item.pricing.discountRate}</TableCell>
											<TableCell>{item.pricing.taxRate}</TableCell>
											<TableCell>{item.pricing.totalPrice}</TableCell>
										</TableRow>
									))}
								</TableBody>
							</Table>
						</CardContent>
					</Card>
				</div>
			</div>
		</div>
	);
}

function StatusBadge({status}: { status: { name: string; value: number } }) {
	const statusStyles = {
		3: "bg-yellow-100 text-yellow-800",
		2: "bg-green-100 text-green-800",
		4: "bg-red-100 text-red-800",
		1: "bg-gray-100 text-gray-800",
	}

	return (
		<Badge variant="secondary" className={statusStyles[status.value as keyof typeof statusStyles]}>
			{status.name.charAt(0).toUpperCase() + status.name.slice(1)}
		</Badge>
	)
}