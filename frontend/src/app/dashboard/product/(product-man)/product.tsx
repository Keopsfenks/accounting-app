import React, {useEffect, useState} from 'react'
import {ProductModel} from "@/ResponseModel/ProductModel";
import {http} from "@/services/HttpService";
import {toast} from "@/hooks/use-toast";
import {CashRegisterModel} from "@/ResponseModel/CashRegisterModel";
import CreateEditButton from "@/components/dashboard/create-edit-button";
import {inputChangeService} from "@/services/input-change";
import {Eye, Search} from "lucide-react";
import {Input} from "@/components/ui/input";
import {Table, TableBody, TableCell, TableHead, TableHeader, TableRow} from "@/components/ui/table";
import {
	Pagination,
	PaginationContent,
	PaginationItem,
	PaginationLink, PaginationNext,
	PaginationPrevious
} from "@/components/ui/pagination";
import {DeleteButton} from "@/components/dashboard/delete-button";
import ViewButton from "@/components/dashboard/view-button";
import ProductBodyModel from "@/Models/Product";


export default function ProductManagement() {
	const [products, setProducts] = useState<ProductModel[]>([]);
	const [categories, setCategories] = useState<ProductModel[]>([]);
	const [pageNumber, setPageNumber] = useState(0);
	const [newProduct, setNewProduct] = useState(ProductBodyModel);
	const [searchQuery, setSearchQuery] = useState('')

	const handleInputChange = (
		e: React.ChangeEvent<HTMLInputElement> | null,
		addrParams?: { name: string; value: string }[]
	) => {
		console.log(addrParams)
		inputChangeService(e, setNewProduct, addrParams);
	}

	const handleSubmit = async (e: React.FormEvent) => {
		e.preventDefault()
		try {
			const response = await http.post('/Product/CreateProduct', newProduct)
			if (response.isSuccessful) {
				toast({
					title: "Success",
					description: "Product created successfully",
				})
			} else if (response.errorMessages) {
				toast({
					title: "Error",
					description: response.errorMessages,
					variant: "destructive",
				})
			}

		} catch(err: any) {
			toast({
				title: "Error",
				description: err.message,
				variant: "destructive",
			})
		}
	}


	const handleUpdateSubmit = async (e: React.FormEvent) => {
		e.preventDefault()
		try {
			const response = await http.put('/Product/UpdateProduct', newProduct)
			if (response.isSuccessful) {
				toast({
					title: "Success",
					description: "Product updated successfully",
				})
			} else if (response.errorMessages) {
				toast({
					title: "Error",
					description: response.errorMessages,
					variant: "destructive",
				})
			}

		} catch(err: any) {
			toast({
				title: "Error",
				description: err.message,
				variant: "destructive",
			})
		}
	}

	const handleDelete = async (id: string) => {
		try {
			const response = await http.delete(`/Product/DeleteProduct`, {
				id: id
			})
			if (response.isSuccessful) {
				toast({
					title: "Success",
					description: "Product is delete successfully",
				})
			} else if (response.errorMessages) {
				toast({
					title: "Error",
					description: response.errorMessages,
					variant: "destructive",
				})
			}
		} catch(err: any) {
			toast({
				title: "Error",
				description: err.message,
				variant: "destructive"
			});
		}
	}

	useEffect(() => {
		async function getProducts() {
			try {
				const response = await http.get<CashRegisterModel>("/Product/GetAllProducts", {
					pageNumber: pageNumber,
					pageSize: 5
				});
				if (response.data && response.isSuccessful) {

					setProducts(prevState => {
						if (Array.isArray(response.data)) {
							return [...response.data];
						}

						console.error("Expected an array, but received:", response.data);
						return prevState;
					});

					toast({
						variant: "default",
						title: "Success",
						description: "Products are fetched successfully",
					});
				}
			} catch (e) {
				toast({
					variant: "destructive",
					title: "Error",
					description: "An error occurred while fetching products",
				})
			}
		}
		getProducts().then();
	}, [pageNumber]);

	useEffect(() => {
		async function getCategories() {
			try {
				const response = await http.get<CashRegisterModel>("/Category/GetAllCategorys");
				if (response.data && response.isSuccessful) {
					setCategories(prevState => {
						if (Array.isArray(response.data)) {
							return [...response.data];
						}

						console.error("Expected an array, but received:", response.data);
						return prevState;
					});

					toast({
						variant: "default",
						title: "Success",
						description: "Categories are fetched successfully",
					});
				}
			} catch (e) {
				toast({
					variant: "destructive",
					title: "Error",
					description: "An error occurred while fetching cash registers",
				})
			}
		}
		getCategories().then();
	}, []);

	const filteredProducts = products.filter((product) => product.name.toLowerCase().includes(searchQuery.toLowerCase()));

	useEffect(() => {
		console.log(newProduct)
	}, [newProduct]);

	return (
		<div className="space-y-4">
			<div className="flex justify-between items-center">
				<h1 className="text-3xl font-bold">Products</h1>
				<CreateEditButton name="Product" type="Create" onSubmit={handleSubmit} formValue={[
					{name: "name", displayName: "Name", type: "input"},
					{name: "description", displayName: "Description", type: "input"},
					{name: "isActive", displayName: "Active", type: "select", options: [
							{name: "Active", value: true},
							{name: "Inactive", value: false}
						] },
					{name: "price", displayName: "Price", type: "input", format: "number"},
					{name: "category", displayName: "Category", type: "select", options: categories.map((category) => ({
						name: category.name,
						value: category.id
					}))},
					{name: "attributes", displayName: "Attributes", type: "optional", optionalSelect: {
						name: "attributes", type: "input", inputChangeOptions: "object", options: [
								{name: "brand", value: "brand", format: "text"},
								{name: "dimensions", value: "dimensions", format: "text"},
								{name: "weight", value: "weight", format: "text"},
							]},
						},
					{name: "metadata", displayName: "Metadata", type: "optional", optionalSelect: {
							name: "metadata", type: "input", inputChangeOptions: "object", options: [
								{name: "barcode", value: "barcode", format: "text"},
								{name: "image", value: "image", format: "text"},
								{name: "supplier", value: "supplier", format: "text"},
								{name: "sku", value: "sku", format: "text"},
							]},
					},
					{name: "stock", displayName: "Stock", type: "optional", optionalSelect: {
							name: "stock", type: "input", inputChangeOptions: "object", options: [
								{name: "stockQuantity", value: "stockQuantity", format: "number"},
								{name: "unitOfMeasure", value: "unitOfMeasure", format: "number"},
							]},
					}]} handleInputChange={handleInputChange} state={setNewProduct} model={ProductBodyModel} />
			</div>
			<div className="relative">
				<Search className="absolute left-2 top-1/2 transform -translate-y-1/2 text-gray-500" />
				<Input
					className="pl-8"
					placeholder="Find a products..."
					value={searchQuery}
					onChange={(e) => setSearchQuery(e.target.value)} />
			</div>
			<Table>
				<TableHeader>
					<TableRow>
						<TableHead>Name / Id</TableHead>
						<TableHead>Active Status</TableHead>
						<TableHead>Price</TableHead>
						<TableHead>Deposit</TableHead>
						<TableHead>Withdraw</TableHead>
						<TableHead>Category</TableHead>
						<TableHead>Attributes</TableHead>
						<TableHead>Metadata</TableHead>
						<TableHead>Pricing</TableHead>
						<TableHead>Stock</TableHead>
					</TableRow>
				</TableHeader>
				<TableBody>
					{filteredProducts.map((product) => (
						<TableRow key={product.id}>
							<TableCell>
								<div>{product.name}</div>
								<div className="text-sm text-gray-500">#{product.id}</div>
							</TableCell>
							<TableCell>{product.isActive ? "Active" : "Inactive"}</TableCell>
							<TableCell>{product.price}</TableCell>
							<TableCell>{product.deposit}</TableCell>
							<TableCell>{product.withdrawal}</TableCell>
							<TableCell>
								{categories.find((category) => category.id === product.categoryId)?.name}
							</TableCell>
							<TableCell>
								{product.attributes?.brand && <div>Brand: {product.attributes.brand}</div>}
								{product.attributes?.dimensions && <div>Dimensions: {product.attributes.dimensions}</div>}
								{product.attributes?.weight && <div>Weight: {product.attributes.weight}</div>}
							</TableCell>
							<TableCell>
								{product.metadata?.barcode && <div>Barcode: {product.metadata.barcode}</div>}
								{product.metadata?.image && <div>Image: {product.metadata.image}</div>}
								{product.metadata?.supplier && <div>Supplier: {product.metadata.supplier}</div>}
								{product.metadata?.sku && <div>SKU: {product.metadata.sku}</div>}
							</TableCell>
							<TableCell>
								{product.pricing?.unitPrice && <div>Unit Price: {product.pricing.unitPrice}</div>}
								{product.pricing?.taxRate && <div>Tax Rate: {product.pricing.taxRate}</div>}
								{product.pricing?.discountRate && <div>Discount Rate: {product.pricing.discountRate}</div>}
							</TableCell>
							<TableCell>
								{product.stock?.stockQuantity && <div>Stock Quantity: {product.stock.stockQuantity}</div>}
								{product.stock?.unitOfMeasure && <div>Unit of Measure: {product.stock.unitOfMeasure}</div>}
							</TableCell>
							<TableCell className="text-right">
								<ViewButton header="Product Details" icon={Eye} tableHeader={["Id", "Processor", "Description", "Deposit", "Withdraw", "Price", "Date"]} deleteData={null} updateData={null} createData={null} data={
									{
										details: product.details,
										id: ["id", "processor", "description", "deposit", "withdraw", "price", "date"
										],
										keysToCompare: ["id", "processor", "description", "deposit", "withdraw", "price", "date"],
										filter: {}
									}
								} />
									<CreateEditButton name="Product" type="Edit" id={[{name: "id", value: product.id}]} onSubmit={handleUpdateSubmit} formValue={[
										{name: "name", displayName: "Name", type: "input"},
										{name: "description", displayName: "Description", type: "input"},
										{name: "isActive", displayName: "Active", type: "select", options: [
												{name: "Active", value: true},
												{name: "Inactive", value: false}
											] },
										{name: "price", displayName: "Price", type: "input", format: "number"},
										{name: "category", displayName: "Category", type: "select", options: categories.map((category) => ({
												name: category.name,
												value: category.id
											}))},
										{name: "attributes", displayName: "Attributes", type: "optional", optionalSelect: {
												name: "attributes", type: "input", inputChangeOptions: "object", options: [
													{name: "brand", value: "brand", format: "text"},
													{name: "dimensions", value: "dimensions", format: "text"},
													{name: "weight", value: "weight", format: "text"},
												]},
										},
										{name: "metadata", displayName: "Metadata", type: "optional", optionalSelect: {
												name: "metadata", type: "input", inputChangeOptions: "object", options: [
													{name: "barcode", value: "barcode", format: "text"},
													{name: "image", value: "image", format: "text"},
													{name: "supplier", value: "supplier", format: "text"},
													{name: "sku", value: "sku", format: "text"},
												]},
										},
										{name: "pricing", displayName: "Pricing", type: "optional", optionalSelect: {
												name: "pricing", type: "input", inputChangeOptions: "object", options: [
													{name: "unitPrice", value: "unitPrice", format: "number"},
													{name: "taxRate", value: "taxRate", format: "number"},
													{name: "discountRate", value: "discountRate", format: "text"},
												]},
										},
										{name: "stock", displayName: "Stock", type: "optional", optionalSelect: {
												name: "stock", type: "input", inputChangeOptions: "object", options: [
													{name: "stockQuantity", value: "stockQuantity", format: "number"},
													{name: "unitOfMeasure", value: "unitOfMeasure", format: "number"},
												]},
										}]} handleInputChange={handleInputChange} state={setNewProduct} model={ProductBodyModel} />
									<DeleteButton onDelete={async () => await handleDelete(product.id)} description={"This action cannot be undone. This will permanently delete the category and remove all associated data."}/>
							</TableCell>
						</TableRow>
					))}
				</TableBody>
			</Table>
			<Pagination>
				<PaginationContent>
					<PaginationItem onClick={() => {
						if (pageNumber > 0) {
							setPageNumber(pageNumber - 1)
						}
					}}>
						<PaginationPrevious/>
					</PaginationItem>
					<PaginationItem>
						<PaginationLink>{pageNumber}</PaginationLink>
					</PaginationItem>
					<PaginationItem>
						<PaginationNext onClick={() => {
							if (products.length === 5)
								setPageNumber(pageNumber + 1)
						}} />
					</PaginationItem>
				</PaginationContent>
			</Pagination>
		</div>
	)
}
