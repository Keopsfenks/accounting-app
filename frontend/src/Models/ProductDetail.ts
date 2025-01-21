
export class ProductDetail {
	issueDate: string = ""; //
	dueDate?: string | null = null; //
	description: string = ""; //
	name: string = ""; //

	amount: number = 0;

	customerId: string  = "";
	cashRegisterId: string = "";

	status: number = 2;
	operation: number = 2;
	payment: number = 2; //

	products: {
		productId: string;
		pricing: {
			quantity: number;
			unitPrice: number;
			discountRate: number;
			taxRate: number;
			totalPrice: number;
		};
	}[] | null = null;
}

const ProductDetailBodyModel = new ProductDetail()


export default ProductDetailBodyModel;
