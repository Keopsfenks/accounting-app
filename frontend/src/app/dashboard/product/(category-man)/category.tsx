import React, {useEffect, useState} from 'react'
import {CategoryModel} from "@/ResponseModel/CategoryModel";
import {http} from "@/services/HttpService";
import {toast} from "@/hooks/use-toast";
import {CashRegisterModel} from "@/ResponseModel/CashRegisterModel";
import CreateEditButton from "@/components/dashboard/create-edit-button";
import {Search} from "lucide-react";
import {Input} from "@/components/ui/input";
import {Table, TableBody, TableCell, TableHead, TableHeader, TableRow} from "@/components/ui/table";
import {DeleteButton} from "@/components/dashboard/delete-button";
import {
	Pagination,
	PaginationContent,
	PaginationItem,
	PaginationLink, PaginationNext,
	PaginationPrevious
} from "@/components/ui/pagination";
import {inputChangeService} from "@/services/input-change";
import CategoryBodyModel from "@/Models/Category";

export default function CategoryManagement() {
	const [categories, setCategories] = useState<CategoryModel[]>([]);
	const [pageNumber, setPageNumber] = useState(0)
	const [newCategory, setNewCategory] = useState(CategoryBodyModel)
	const [searchQuery, setSearchQuery] = useState('')

	const handleInputChange = (
		e: React.ChangeEvent<HTMLInputElement> | null,
		addrParams?: { name: string; value: string }[]
	) => {
		inputChangeService(e, setNewCategory, addrParams);
	}

	const handleSubmit = async (e: React.FormEvent) => {
		e.preventDefault()
		try {
			const response = await http.post('/Category/CreateCategory', newCategory)
			if (response.isSuccessful) {
				toast({
					title: "Success",
					description: "Category created successfully",
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
			const response = await http.put('/Category/UpdateCategory', newCategory)
			if (response.isSuccessful) {
				toast({
					title: "Success",
					description: "Category updated successfully",
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
			const response = await http.delete(`/Category/DeleteCategory`, {
				id: id
			})
			if (response.isSuccessful) {
				toast({
					title: "Success",
					description: "Category is delete successfully",
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
		async function getCategories() {
			try {
				const response = await http.get<CashRegisterModel>("/Category/GetAllCategorys", {
					pageNumber: pageNumber,
					pageSize: 5
				});
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
					description: "An error occurred while fetching categories",
				})
			}
		}
		getCategories().then();
	}, [pageNumber]);

	const filteredCategories = categories.filter((category) => category.name.toLowerCase().includes(searchQuery.toLowerCase()))

	useEffect(() => {
		console.log(newCategory)
	}, [newCategory]);

	return (
		<div className="space-y-4">
			<div className="flex justify-between items-center">
				<h1 className="text-3xl font-bold">Categories</h1>
				<CreateEditButton name="Category" type="Create" onSubmit={handleSubmit} formValue={[
					{name: "name", displayName: "Name", type: "input", options: []},
					{name: "description", displayName: "Description", type: "input", options: []},
					{name: "parentCategory", displayName: "Parent Category", type: "select", options: categories.map((category) => {
						return {
							value: category.id,
							name: category.name
						}
					})}
				]} handleInputChange={handleInputChange} state={setNewCategory} model={CategoryBodyModel} />
			</div>
			<div className="relative">
				<Search className="absolute left-2 top-1/2 transform -translate-y-1/2 text-gray-500"/>
				<Input
					className="pl-8"
					placeholder="Find a category..."
					value={searchQuery}
					onChange={(e) => setSearchQuery(e.target.value)} />
			</div>
			<Table>
				<TableHeader>
					<TableRow>
						<TableHead>Name / Id</TableHead>
						<TableHead>Description</TableHead>
						<TableHead>Parent Category</TableHead>
					</TableRow>
				</TableHeader>
				<TableBody>
					{filteredCategories.map((category) => (
						<TableRow key={category.id}>
							<TableCell>
								<div>{category.name}</div>
								<div className="text-sm text-gray-500">#{category.id}</div>
							</TableCell>
							<TableCell>{category.description}</TableCell>
							<TableCell>{category.subCategories.map((sub) => {
								return sub.name + ', '
							})}</TableCell>
							<TableCell className="text-right">
									<CreateEditButton name="Category" type="Edit" id={[{name: "id", value: category.id}]} onSubmit={handleUpdateSubmit} formValue={[
										{name: "name", displayName: "Name", type: "input", options: []},
										{name: "description", displayName: "Description", type: "input", options: []},
										{name: "parentCategory", displayName: "Parent Category", type: "select",
										options: categories.filter((item) => item.id !== category.id).map((item) => {
											return {
												value: item.id,
												name: item.name
											}
										}) },
									]} handleInputChange={handleInputChange}  state={setNewCategory} model={CategoryBodyModel} />
									<DeleteButton onDelete={async () => await handleDelete(category.id)} description={"This action cannot be undone. This will permanently delete the category and remove all associated data."}/>
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
							if (categories.length === 5)
								setPageNumber(pageNumber + 1)
						}} />
					</PaginationItem>
				</PaginationContent>
			</Pagination>
		</div>
	)
}
