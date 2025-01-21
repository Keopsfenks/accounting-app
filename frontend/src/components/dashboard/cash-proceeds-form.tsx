import React, {useEffect, useState} from 'react';
import { Button } from "@/components/ui/button";
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from "@/components/ui/select";
import {CustomerDetailModel, CustomerInvoiceModel} from "@/ResponseModel/CustomerModel";
import {http} from "@/services/HttpService";
import {toast} from "@/hooks/use-toast";

const PaymentTypes = {
	CASH: 1,
	CREDIT_CARD: 2,
	BANK_TRANSFER: 3,
	CHEQUE: 4
};

export function CashProceedsForm({ data, type } : { data: CustomerInvoiceModel | CustomerDetailModel, type: "invoice" | "customer" }) {
	const [isFormVisible, setIsFormVisible] = useState(false);
	const [formData, setFormData] = useState({
		description: "",
		issueDate: new Date().toISOString().split('T')[0],
		amount: 0,
		payment: PaymentTypes.CASH,
		operation: 2,
		cheque: null as {
			maturityDate: string;
			bankName: string;
			chequeNumber: string;
		} | null,
		invoiceId: type === "invoice" ? data.id : null,
		customerDetailId: type === "customer" ? data.id : null,
	});

	const handlePaymentTypeChange = (value: string) => {
		const paymentType = parseInt(value);
		setFormData(prev => ({
			...prev,
			payment: paymentType,
			operation: paymentType,
			cheque: paymentType === PaymentTypes.CHEQUE ? {
				maturityDate: "",
				bankName: "",
				chequeNumber: ""
			} : null
		}));
	};

	const handleChequeChange = (field: 'maturityDate' | 'bankName' | 'chequeNumber', value: string) => {
		setFormData(prev => ({
			...prev,
			cheque: prev.cheque ? {
				...prev.cheque,
				[field]: value
			} : null
		}));
	};


	const handleScroll = () => {
		setIsFormVisible(!isFormVisible);
		setTimeout(() => {
			window.scrollTo({
				top: document.documentElement.scrollHeight,
				behavior: 'smooth'
			});
		}, 100);
	};


	const handleAmountChange = (e: React.ChangeEvent<HTMLInputElement>) => {
		setFormData(prev => ({
			...prev,
			amount: parseFloat(e.target.value)
		}));
	};

	useEffect(() => {
		if (formData.payment === PaymentTypes.CHEQUE) {
			setFormData(prev => ({
				...prev,
				cheque: {
					maturityDate: "",
					bankName: "",
					chequeNumber: "",
				},
			}));
		} else {
			setFormData(prev => ({
				...prev,
				cheque: null,
			}));
		}
	}, [formData.payment]);

	const handleSubmit = async () => {
		try {
			const response = await http.post("/CashProceed/CreateCashProceeds", formData)
			if (response.isSuccessful) {
				toast({
					title: "Success",
					description: "Cash Proceeds is successfully",
				})
			} else if (response.errorMessages) {
				toast({
					title: "Error",
					description: response.errorMessages,
					variant: "destructive",
				})
			}

		} catch(err: any) {
			toast({
				title: "Error",
				description: err.message,
				variant: "destructive",
			})
		}
	}

	return (
		<div className="space-y-4">
			<Button
				className="w-full"
				onClick={handleScroll}
			>
				Cash Proceeds
			</Button>

			<div className={`transition-all duration-300 ease-in-out overflow-hidden ${
				isFormVisible ? 'max-h-[800px]' : 'max-h-0'
			}`}>
				<Card className="mt-4">
					<CardHeader>
						<CardTitle>Cash Proceeds Form</CardTitle>
					</CardHeader>
					<CardContent className="space-y-4">
						<div className="space-y-2">
							<Label htmlFor="description">Description</Label>
							<Input
								id="description"
								value={formData.description}
								onChange={(e) => setFormData(prev => ({ ...prev, description: e.target.value }))}
							/>
						</div>

						<div className="space-y-2">
							<Label htmlFor="issueDate">Issue Date</Label>
							<Input
								id="issueDate"
								type="date"
								value={formData.issueDate}
								onChange={(e) => setFormData(prev => ({ ...prev, issueDate: e.target.value }))}
							/>
						</div>

						<div className="space-y-2">
							<Label htmlFor="payment">Payment Type</Label>
							<Select
								value={formData.payment.toString()}
								onValueChange={handlePaymentTypeChange}
							>
								<SelectTrigger>
									<SelectValue placeholder="Select payment type" />
								</SelectTrigger>
								<SelectContent>
									<SelectItem value={PaymentTypes.CASH.toString()}>Cash</SelectItem>
									<SelectItem value={PaymentTypes.CREDIT_CARD.toString()}>Credit Card</SelectItem>
									<SelectItem value={PaymentTypes.BANK_TRANSFER.toString()}>Bank Transfer</SelectItem>
									<SelectItem value={PaymentTypes.CHEQUE.toString()}>Cheque</SelectItem>
								</SelectContent>
							</Select>
						</div>

						<div className="space-y-2">
							<Label htmlFor="amount">Amount</Label>
							<Input
								id="amount"
								type="number"
								value={formData.amount}
								onChange={handleAmountChange}
							/>
						</div>

						{formData.payment === PaymentTypes.CHEQUE && formData.cheque && (
							<div className="space-y-4">
								<div className="space-y-2">
									<Label htmlFor="maturityDate">Maturity Date</Label>
									<Input
										id="maturityDate"
										type="date"
										value={formData.cheque.maturityDate}
										onChange={(e) => handleChequeChange('maturityDate', e.target.value)}
									/>
								</div>
								<div className="space-y-2">
									<Label htmlFor="bankName">Bank Name</Label>
									<Input
										id="bankName"
										value={formData.cheque.bankName}
										onChange={(e) => handleChequeChange('bankName', e.target.value)}
									/>
								</div>
								<div className="space-y-2">
									<Label htmlFor="chequeNumber">Cheque Number</Label>
									<Input
										id="chequeNumber"
										value={formData.cheque.chequeNumber}
										onChange={(e) => handleChequeChange('chequeNumber', e.target.value)}
									/>
								</div>
							</div>
						)}

						<div className="pt-4">
							<Button onClick={handleSubmit} className="w-full">
								Submit
							</Button>
						</div>
					</CardContent>
				</Card>
			</div>
		</div>
	);
}

export default CashProceedsForm;