class Category {
	name: string = '';
	description: string = '';
	parentCategory?: string | null = null
}


const CategoryBodyModel = new Category();

export default CategoryBodyModel;