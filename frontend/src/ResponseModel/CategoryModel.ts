export interface CategoryModel {
	id: string;
	isParent: boolean
	name: string;
	description: string;
	isActive: boolean;
	subCategories: CategoryModel[];
	products: object[];
}