class Invoice {
	invoiceNumber: string = "";
	issueDate: string = "";
	dueDate?: string | null = null;
	company?: string | null = null;
	customerId?: string | null = null;

	status: number = 1;
	type: number = 2;
	payment: number = 2;
	paymentType: number = 1;
	currency: number = 1;

	cheque?: {
		maturityDate: string;
		bankName: string;
		chequeNumber: string;
	} = {
		maturityDate: "",
		bankName: "",
		chequeNumber: "",
	} ;
	products: {
		productId: string;
		quantity: number;
		unitPrice: number;
		discount: number;
		taxRate: number;
		total: number;
	}[] = [
		{
			productId: "",
			quantity: 0,
			unitPrice: 0,
			discount: 0,
			taxRate: 0,
			total: 0,
		}
	];
}

const InvoiceBodyModel = new Invoice();
export { InvoiceBodyModel };
