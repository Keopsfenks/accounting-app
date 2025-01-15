import {CustomerDetailModel, CustomerInvoiceModel} from "@/ResponseModel/CustomerModel";
import {Card, CardContent, CardDescription, CardHeader, CardTitle} from "@/components/ui/card";
import {Company} from "@/Models/Company";
import React, {useEffect, useRef, useState} from "react";
import {Label} from "@/components/ui/label";
import {Input} from "@/components/ui/input";
import {Badge} from "@/components/ui/badge";
import {Table, TableBody, TableCell, TableHead, TableHeader, TableRow} from "@/components/ui/table";
import {ProductModel} from "@/ResponseModel/ProductModel";
import product from "@/Models/Product";
import {http} from "@/services/HttpService";
import {toast} from "@/hooks/use-toast";
import customer from "@/Models/Customer";
import CashProceedsForm from "@/components/dashboard/cash-proceeds-form";

interface SalesDetailProps {
	detail: CustomerDetailModel;
	companies: Company[];
	isExpanded: boolean;
	products: ProductModel[]
}

interface ProductDetail {
	id: string; //
	processor: object;
	description: string;
	date: string;
	type: {
		name: string;
		value: number;
	};
	deposit: number;
	withdrawal: number;
	pricing: {
		quantity: number;
		unitPrice: number;
		taxRate: number;
		totalPrice: number;
		discountRate: number;
	}
	customerId: string;
	productId: string;
}

export function SalesDetail({ detail, companies, isExpanded, products }: SalesDetailProps) {
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
					<CashProceedsForm data={detail} endpoint="/CashProceeds/CreateCashProceedsSales" />
					<div className="grid grid-cols-3 gap-3">
						<Card>
							<CardHeader>
								<CardTitle>Processor: {Object.keys(detail.processor)}</CardTitle>
								<CardDescription>The user making the sales transaction</CardDescription>
							</CardHeader>
						</Card>
						<Card>
							<CardHeader>
								<CardTitle>Id:</CardTitle>
								<CardDescription>{detail.id}</CardDescription>
							</CardHeader>
						</Card>
						<Card>
							<CardHeader>
								<CardTitle><StatusBadge status={detail.paymentType} /></CardTitle>
								<CardDescription>Customer which select payment type</CardDescription>
							</CardHeader>
						</Card>
					</div>
					<div className="grid grid-cols-2 gap-3">
					</div>
					<Card>
						<CardHeader>
							<CardTitle>Cheque Information</CardTitle>
						</CardHeader>
						<CardContent className="space-y-4">
							<Label htmlFor="maturityDate">Maturity Date</Label>
							<Input placeholder={detail.cheque?.maturityDate} disabled></Input>
							<Label htmlFor="bankName">Bank Name</Label>
							<Input placeholder={detail.cheque?.bankName} disabled></Input>
							<Label htmlFor="chequeNumber">Cheque Number</Label>
							<Input placeholder={detail.cheque?.chequeNumber} disabled></Input>
						</CardContent>
					</Card>
					{products.length === 1 ? (
						<div className="grid grid-cols-3 gap-3">
							<Card>
								<CardHeader>
									<CardTitle>Deposit Amount: {detail.depositAmount}</CardTitle>
								</CardHeader>
							</Card>
							<Card>
								<CardHeader>
									<CardTitle>Withdraw Amount: {detail.withdrawalAmount}</CardTitle>
								</CardHeader>
							</Card>
							<Card>
								<CardHeader>
									<CardTitle>Total Amount: {detail.totalAmount}</CardTitle>
								</CardHeader>
							</Card>
						</div>
					) : (
						<div className="space-y-4">
							<div className="grid grid-cols-3 gap-3">
								<Card>
									<CardHeader>
										<CardTitle>Deposit Amount: {detail.depositAmount}</CardTitle>
									</CardHeader>
								</Card>
								<Card>
									<CardHeader>
										<CardTitle>Withdraw Amount: {detail.withdrawalAmount}</CardTitle>
									</CardHeader>
								</Card>
								<Card>
									<CardHeader>
										<CardTitle>Total Amount: {detail.totalAmount}</CardTitle>
									</CardHeader>
								</Card>
							</div>
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
											{detail.products.map((product, index) => (
												<TableRow key={product.id}>
													<TableCell>{products.find((product) => product.id == product.id)?.name}</TableCell>
													<TableCell>{product.pricing.quantity}</TableCell>
													<TableCell>{product.pricing.unitPrice}</TableCell>
													<TableCell>{product.pricing.discountRate}</TableCell>
													<TableCell>{product.pricing.taxRate}</TableCell>
													<TableCell>{product.pricing.totalPrice}</TableCell>
												</TableRow>
											))}
										</TableBody>
									</Table>
								</CardContent>
							</Card>
						</div>
					)}
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