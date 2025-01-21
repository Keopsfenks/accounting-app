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
					{name: "unitOfMeasure", displayName: "Unit of Measure", type: "select", options: [
							{name: "Kilogram", value: "1"},
							{name: "Gram", value: "2"},
							{name: "Milligram", value: "3"},
							{name: "Ton", value: "4"},
							{name: "Litre", value: "5"},
							{name: "Millilitre", value: "6"},
							{name: "Cubic Meter", value: "7"},
							{name: "Unit", value: "8"},
							{name: "Box", value: "9"},
							{name: "Packet", value: "10"},
							{name: "Piece", value: "11"},
							{name: "Meter", value: "12"},
							{name: "Centimeter", value: "13"},
							{name: "Millimeter", value: "14"},
							{name: "Kilometer", value: "15"},
							{name: "Square Meter", value: "16"},
							{name: "Square Kilometer", value: "17"},
							{name: "Hectare", value: "18"},
							{name: "Acre", value: "19"},
							{name: "Hour", value: "20"},
							{name: "Minute", value: "21"},
							{name: "Second", value: "22"}
						]},
					{name: "category", displayName: "Category", type: "select", options: categories.map((category) => ({
						name: category.name,
						value: category.id
					}))},
				]} handleInputChange={handleInputChange} state={setNewProduct} model={ProductBodyModel} />
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
						<TableHead>Unit of Measure</TableHead>
						<TableHead>Active Status</TableHead>
						<TableHead>Category</TableHead>

					</TableRow>
				</TableHeader>
				<TableBody>
					{filteredProducts.map((product) => (
						<TableRow key={product.id}>
							<TableCell>
								<div>{product.name}</div>
								<div className="text-sm text-gray-500">#{product.id}</div>
							</TableCell>
							<TableCell>{product.unitOfMeasure.name}</TableCell>
							<TableCell>{product.isActive ? "Active" : "Inactive"}</TableCell>
							<TableCell>
								{categories.find((category) => category.id === product.categoryId)?.name}
							</TableCell>
							<TableCell className="text-right">
								<ViewButton header="Product Details" icon={Eye} tableHeader={["Id", "Processor", "Description", "Type", "Prices", "Date"]} deleteData={null} updateData={null} createData={null} data={
									{
										details: product.operations,
										id: ["id", "processor", "description", "type", "pricing.totalPrice", "date"
										],
										keysToCompare: ["id", "processor", "description", "type", "pricing", "date"],
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
										{name: "unitOfMeasure", displayName: "Unit of Measure", type: "select", options: [
												{name: "Kilogram", value: "1"},
												{name: "Gram", value: "2"},
												{name: "Milligram", value: "3"},
												{name: "Ton", value: "4"},
												{name: "Litre", value: "5"},
												{name: "Millilitre", value: "6"},
												{name: "Cubic Meter", value: "7"},
												{name: "Unit", value: "8"},
												{name: "Box", value: "9"},
												{name: "Packet", value: "10"},
												{name: "Piece", value: "11"},
												{name: "Meter", value: "12"},
												{name: "Centimeter", value: "13"},
												{name: "Millimeter", value: "14"},
												{name: "Kilometer", value: "15"},
												{name: "Square Meter", value: "16"},
												{name: "Square Kilometer", value: "17"},
												{name: "Hectare", value: "18"},
												{name: "Acre", value: "19"},
												{name: "Hour", value: "20"},
												{name: "Minute", value: "21"},
												{name: "Second", value: "22"}
											]},
										{name: "category", displayName: "Category", type: "select", options: categories.map((category) => ({
												name: category.name,
												value: category.id
											}))},
									]} handleInputChange={handleInputChange} state={setNewProduct} model={ProductBodyModel} />
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
