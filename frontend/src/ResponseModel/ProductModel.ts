export interface ProductAttributeModel {
	brand: string;
	dimensions: string;
	weight: string;
	additionalAttributes: object;
}

export interface ProductMetadataModel {
	barcode: string;
	image: string;
	supplier: string;
	sku: string;
}

export interface ProductPricingModel {
	unitPrice: number;
	taxRate: number;
	discountRate: string
}

export interface ProductStockModel {
	stockQuantity: number;
	unitOfMeasure: number;
}

export interface ProductDetailModel {
	id: string
	processor: string;
	description: string;
	deposit: number;
	withdraw: number;
	price: number;
	date: string;
}

export interface ProductModel {
	id: string;
	name: string;
	isActive: boolean;
	price: number;
	deposit: number;
	withdrawal: number;
	categoryId: string;
	details: ProductDetailModel[];
	attributes: ProductAttributeModel;
	metadata: ProductMetadataModel;
	pricing: ProductPricingModel;
	stock: ProductStockModel;

}