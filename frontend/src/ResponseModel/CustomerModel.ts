export interface CustomerDetailModel {
	id: string; //
	processor: object;
	issueDate: string; //
	dueDate: string; //
	description: string; //
	operationType: {
		name: string;
		value: number;
	}; //
	paymentType: {
		name: string;
		value: number;
	}; //
	payment: {
		name: string;
		value: number;
	}
	cheque: {
		maturityDate: string;
		bankName: string;
		chequeNumber: string;
	}
	depositAmount: number;
	withdrawalAmount: number;
	totalAmount: number;
	opposite: object;
	products: {
		id: string;
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
		invoiceId: string;
	}[]
}

export interface CustomerInvoiceModel {
	id: string; //
	processor: object; //
	invoiceNumber: string; //
	issueDate: string; //
	dueDate: string; //
	products: {
		id: string;
		productId: string;
		processor: object
		quantity: number;
		unitPrice: number;
		discount: number;
		taxRate: number;
		total: number;
	}[]
	companyId: string; //
	depositAmount: number; //
	withdrawalAmount: number; //
	totalAmount: number; //
	status: {
		name: string;
		value: number
	} //
	type: {
		name: string;
		value: number
	} //
	payment: {
		name: string;
		value: number
	}//
	paymentType: {
		name: string;
		value: number
	} //
	cheque: {
		maturityDate: string;
		bankName: string;
		chequeNumber: string;
	} //

}


export interface CustomerModel {
	id: string; //
	name: string; //
	type: {
		name: string;
		value: number;
	}; //

	description?: string | null; //
	email?: string | null; //
	phone?: string | null; //
	address?: string | null; //
	city?: string | null; //
	town?: string | null; //
	country?: string | null; //
	zipCode?: string | null; //
	taxId?: string | null; //
	taxDepartment?: string | null; //

	details: CustomerDetailModel[];
	invoices: CustomerInvoiceModel[];

	deposit: number;
	withdrawal: number;
	debit: number;
}