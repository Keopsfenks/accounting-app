export class Product {
	name: string = '';
	description: string = '';
	isActive: boolean = true;
	category: string = '';

	attributes?: {
		brand?: string | null;
		dimensions?: string | null;
		weight?: string | null;
		additionalAttributes?: object | null;
	} = {};

	metadata?: {
		barcode?: string | null;
		image?: string | null;
		supplier?: string | null;
		sku?: string | null;
	} = {};

	stock?: {
		stockQuantity?: number | null;
		unitOfMeasure?: number | null;
	} = {
		stockQuantity: null,
		unitOfMeasure: null,
	};
}


const ProductBodyModel = new Product();

export default ProductBodyModel
