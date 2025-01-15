import {CustomerInvoiceModel} from "@/ResponseModel/CustomerModel";
import {Card, CardContent, CardDescription, CardHeader, CardTitle} from "@/components/ui/card";
import {Company} from "@/Models/Company";
import React, { useEffect, useRef } from "react";
import {Label} from "@/components/ui/label";
import {Input} from "@/components/ui/input";
import {Badge} from "@/components/ui/badge";
import {Table, TableBody, TableCell, TableHead, TableHeader, TableRow} from "@/components/ui/table";
import {ProductModel} from "@/ResponseModel/ProductModel";
import {Button} from "@/components/ui/button";
import CreateEditButton from "@/components/dashboard/create-edit-button";
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
					<CashProceedsForm data={invoice} endpoint="/CashProceeds/CreateCashProceedsInvoice" />
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
								<CardTitle><StatusBadge status={invoice.payment} /></CardTitle>
								<CardDescription>Customer which select payment type</CardDescription>
							</CardHeader>
						</Card>
						<Card>
							<CardHeader>
								<CardTitle><StatusBadge status={invoice.paymentType} /></CardTitle>
								<CardDescription>Customer which select payment type</CardDescription>
							</CardHeader>
						</Card>
					</div>
					<Card>
						<CardHeader>
							<CardTitle>Cheque Information</CardTitle>
						</CardHeader>
						<CardContent className="space-y-4">
							<Label htmlFor="maturityDate">Maturity Date</Label>
							<Input placeholder={invoice.cheque?.maturityDate} disabled></Input>
							<Label htmlFor="bankName">Bank Name</Label>
							<Input placeholder={invoice.cheque?.bankName} disabled></Input>
							<Label htmlFor="chequeNumber">Cheque Number</Label>
							<Input placeholder={invoice.cheque?.chequeNumber} disabled></Input>
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
											<TableCell>{item.quantity}</TableCell>
											<TableCell>{item.unitPrice}</TableCell>
											<TableCell>{item.discount}</TableCell>
											<TableCell>{item.taxRate}</TableCell>
											<TableCell>{item.total}</TableCell>
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