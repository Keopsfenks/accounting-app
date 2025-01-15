import React, { useState } from 'react';
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

export function CashProceedsForm({ data, endpoint} : { data: CustomerInvoiceModel | CustomerDetailModel, endpoint: string}) {
	const [isFormVisible, setIsFormVisible] = useState(false);
	const [formData, setFormData] = useState({
		id: data.id,
		paymentType: 2,
		amount: 2000
	});

	const handleScroll = () => {
		setIsFormVisible(!isFormVisible);
		setTimeout(() => {
			window.scrollTo({
				top: document.documentElement.scrollHeight,
				behavior: 'smooth'
			});
		}, 100);
	};

	const handlePaymentTypeChange = (value: string) => {
		setFormData(prev => ({
			...prev,
			paymentType: parseInt(value)
		}));
	};

	const handleAmountChange = (e: React.ChangeEvent<HTMLInputElement>) => {
		setFormData(prev => ({
			...prev,
			amount: parseFloat(e.target.value)
		}));
	};

	const handleSubmit = async () => {
		try {
			const response = await http.post(endpoint, formData)
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

			<div
				className={`transition-all duration-300 ease-in-out overflow-hidden ${
					isFormVisible ? 'max-h-96' : 'max-h-0'
				}`}
			>
				<Card className="mt-4">
					<CardHeader>
						<CardTitle>Cash Proceeds Form</CardTitle>
					</CardHeader>
					<CardContent className="space-y-4">
						<div className="space-y-2">
							<Label htmlFor="paymentType">Payment Type</Label>
							<Select
								value={formData.paymentType.toString()}
								onValueChange={handlePaymentTypeChange}
							>
								<SelectTrigger>
									<SelectValue placeholder="Select payment type" />
								</SelectTrigger>
								<SelectContent>
									<SelectItem value="1">Cash</SelectItem>
									<SelectItem value="2">Credit Card</SelectItem>
									<SelectItem value="3">Bank Transfer</SelectItem>
									<SelectItem value="4">Cheque</SelectItem>
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

						<div className="pt-4">
							<Button onClick={() => handleSubmit()} className="w-full">
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