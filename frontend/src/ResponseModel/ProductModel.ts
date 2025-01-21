
export interface ProductDetailModel {
	id: string
	processor: string;
	date: string;
	description: string;
	pricing: {
		discountRate: number;
		quantity: number;
		taxRate: number;
		totalPrice: number;
		unitPrice: number;
	};
	productId: string;
	invoiceId: string;
	customerDetailId: string;
	type: {
		name: string;
		value: number;
	};
}

export interface ProductModel {
	id: string;
	name: string;
	isActive: boolean;
	unitOfMeasure: {
		name: string;
		value: number;
	};
	categoryId: string;
	operations: ProductDetailModel[];


}