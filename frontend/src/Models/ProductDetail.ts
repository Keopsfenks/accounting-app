
export class ProductDetail {
	issueDate: string = ""; //
	dueDate?: string | null = null; //
	description: string = ""; //

	operationType: number = 2; //
	paymentType: number = 1; //
	payment: number = 2; //
	amount: number = 0;
	customerId: string = ""
	cashRegisterId: string = ""

	cheque?: {
		maturityDate: string;
		bankName: string;
		chequeNumber: string;
	} | undefined = {
		maturityDate: "",
		bankName: "",
		chequeNumber: "",
	}; //
	productsList: {
		type: number;
		productId: string,
		pricing: {
			quantity: number;
			unitPrice: number;
			taxRate: number;
			totalPrice: number;
			discountRate: number
		}
	}[] | null = null
}

const ProductDetailBodyModel = new ProductDetail()


export default ProductDetailBodyModel;
