export interface CustomerDetailModel {
	id: string;  //
	processor: object; //
	name: string;  //
	description: string; //
	issueDate: string; //
	dueDate: string;  //
	depositAmount: number;
	withdrawalAmount: number;
	totalAmount: number;
	products: {
		id: string;
		processor: object;
		description: string;
		date: string;
		type: {
			name: string;
			value: number;
		};
		pricing: {
			quantity: number;
			unitPrice: number;
			taxRate: number;
			totalPrice: number;
			discountRate: number;
		};
		productId: string;
		invoiceId: string;
		customerDetailId: string;
	}[];
	cashProceeds: {
		id: string;
		description: string;
		issueDate: string;
		amount: number;
		paymentType: {
			name: string;
			value: number;
		};
		operation: {
			name: string;
			value: number;
		};
		cheque: {
			maturityDate: string;
			bankName: string;
			chequeNumber: string;
		};
		invoiceId: string;
		customerDetailId: string;
	}[];
	payment: {
		name: string;
		value: number;
	};
	operation: {
		name: string;
		value: number;
	};
	status: {
		name: string;
		value: number;
	};
	customerId: string;
}

export interface CustomerInvoiceModel {
	id: string;
	processor: object;
	invoiceNumber: string;
	description: string;
	issueDate: string;
	dueDate: string;
	companyId: string;
	products: {
		id: string;
		processor: object;
		description: string;
		date: string;
		type: {
			name: string;
			value: number;
		};
		pricing: {
			quantity: number;
			unitPrice: number;
			taxRate: number;
			totalPrice: number;
			discountRate: number;
		};
		productId: string;
		invoiceId: string;
		customerDetailId: string;
	}[]
	cashProceeds: {
		id: string;
		description: string;
		issueDate: string;
		amount: number;
		paymentType: {
			name: string;
			value: number;
		};
		operation: {
			name: string;
			value: number;
		};
		cheque: {
			maturityDate: string;
			bankName: string;
			chequeNumber: string;
		};
		invoiceId: string;
		customerDetailId: string;
	}[];
	depositAmount: number;
	withdrawalAmount: number;
	totalAmount: number;
	currencyType: {
		name: string;
		value: number;
	};
	payment: {
		name: string;
		value: number;
	};
	operation: {
		name: string;
		value: number;
	};
	status: {
		name: string;
		value: number;
	};
	customerId: string
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