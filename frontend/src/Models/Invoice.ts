class Invoice {
	invoiceNumber: string = "";
	description: string = "";
	issueDate: string = "";
	dueDate?: string | null = null;
	company?: string | null = null;
	customerId?: string | null = null;

	status: number = 2;
	operation: number = 2;
	payment: number = 2;
	currencyType: number = 1;

	products: {
		productId: string;
		pricing: {
			quantity: number;
			unitPrice: number;
			discountRate: number;
			taxRate: number;
			totalPrice: number;
		};
	}[] = [
		{
			productId: "",
			pricing: {
				quantity: 1,
				unitPrice: 0,
				discountRate: 0,
				taxRate: 0,
				totalPrice: 0
			}
		}
	];
}

const InvoiceBodyModel = new Invoice();
export { InvoiceBodyModel };
