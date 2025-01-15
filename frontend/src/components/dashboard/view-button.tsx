'use client'

import {
	Drawer,
	DrawerContent,
	DrawerDescription,
	DrawerHeader,
	DrawerTitle,
	DrawerTrigger
} from "@/components/ui/drawer";
import {Button} from "@/components/ui/button";
import {Ellipsis, LucideIcon, Search} from "lucide-react";
import CreateEditButton, {formValue} from "@/components/dashboard/create-edit-button";
import React, {useEffect, useState} from "react";
import {Table, TableBody, TableCell, TableHead, TableHeader, TableRow} from "@/components/ui/table";
import {Input} from "@/components/ui/input";
import {
	Pagination,
	PaginationContent,
	PaginationItem,
	PaginationLink, PaginationNext,
	PaginationPrevious
} from "@/components/ui/pagination";
import {DeleteButton} from "@/components/dashboard/delete-button";
import {http} from "@/services/HttpService";
import {toast} from "@/hooks/use-toast";

type ViewButtonProps<T extends Record<string, any>> = {
	header: string;
	id?: {name: string, value: string}[];
	tableHeader: string[];
	icon?: LucideIcon;
	deleteData: {
		endpoint: string;
		name: string;
	} | null
	updateData: {
		endpoint: string;
		name: string;
	} | null
	createData: {
		name: string;
		endpoint: string;
		body: object;
		formValue: formValue[];
	} | null
	data: {
		details?: T[],
		id: string[];
		keysToCompare: (keyof T)[];
		filter: Partial<T>;
	};

};

export default function ViewButton<T extends Record<string, any>>({
																	  header,
																	  id,
																	  icon = Ellipsis,
																	  tableHeader,
																	  data,
																	  deleteData,
																	  createData,
																	  updateData,
																  }: ViewButtonProps<T>) {
	const [searchQuery, setSearchQuery] = useState('');
	const [pageNumber, setPageNumber] = useState(0);
	const [newData, setNewData] = useState(createData?.body || {});

	const handleInputChange = (
		e: React.ChangeEvent<HTMLInputElement> | null,
		addrParams?: { name: string; value: string }[]
	) => {
		if (addrParams) {
			addrParams.map((param) => {
				setNewData((prev) => ({
					...prev,
					[param.name]: param.value,
				}));
			});
			return;
		}

		if (e) {
			const { name, value } = e.target;
			setNewData((prev) => ({
				...prev,
				[name]: value,
			}));
		}
	};

	useEffect(() => {
		console.log('data', newData);
	}, [newData]);

	const itemsPerPage = 5;

	const filteredData = data.details ? data.details.filter((item) =>
		data.keysToCompare.some((key) =>
			item[key]?.toString().toLowerCase().includes(searchQuery.toLowerCase())
		)
	) : [];

	const totalPages = Math.ceil(filteredData.length / itemsPerPage);
	const currentData = filteredData.slice(
		pageNumber * itemsPerPage,
		pageNumber * itemsPerPage + itemsPerPage
	);

	const handleCreate = async (e: React.FormEvent) => {
		if (!createData) return;
		e.preventDefault();
		try {
			const response = await http.post(createData.endpoint, newData);
			if (response.isSuccessful) {
				toast({
					title: 'Success',
					description: `${createData.name} created successfully`,
				});
				setNewData(createData.body || {});
			} else if (response.errorMessages) {
				toast({
					title: 'Error',
					description: response.errorMessages,
					variant: 'destructive',
				});
			}
		} catch (err: any) {
			toast({
				title: 'Error',
				description: err.message,
				variant: 'destructive',
			});
		}
	};

	const handleUpdate = async (e: React.FormEvent) => {
		if (!updateData) return;
		e.preventDefault();
		try {
			const response = await http.put(updateData.endpoint, newData);
			if (response.isSuccessful) {
				toast({
					title: 'Success',
					description: `${updateData.name} updated successfully`,
				});
				setNewData(createData?.body || {});
			} else if (response.errorMessages) {
				toast({
					title: 'Error',
					description: response.errorMessages,
					variant: 'destructive',
				});
			}
		} catch (err: any) {
			toast({
				title: 'Error',
				description: err.message,
				variant: 'destructive',
			});
		}
	};

	const handleDelete = async (id: string) => {
		if (!deleteData) return;
		try {
			const response = await http.delete(deleteData.endpoint, {
				id: id,
			});
			if (response.isSuccessful) {
				toast({
					title: 'Success',
					description: `${deleteData.name} deleted successfully`,
				});
			} else if (response.errorMessages) {
				toast({
					title: 'Error',
					description: response.errorMessages,
					variant: 'destructive',
				});
			}
		} catch (err: any) {
			toast({
				title: 'Error',
				description: err.message,
				variant: 'destructive',
			});
		}
	};

	return (
		<Drawer>
			<DrawerTrigger asChild>
				<Button variant="ghost" size="icon" className="h-8 w-8">
					{icon && React.createElement(icon, { className: "h-4 w-4" })}
				</Button>
			</DrawerTrigger>
			<DrawerContent>
				<div className="space-y-4">
					<DrawerHeader>
						<DrawerTitle>{header}</DrawerTitle>
						<DrawerDescription>All operations of the {header}</DrawerDescription>
						{createData && (
							<CreateEditButton
								name={header}
								type="Create"
								state={setNewData}
								model={createData.body}
								id={id}
								onSubmit={handleCreate}
								formValue={createData.formValue}
								handleInputChange={handleInputChange}
							/>
						)}
					</DrawerHeader>
					<div className="relative">
						<Search className="absolute left-2 top-1/2 transform -translate-y-1/2 text-gray-500" />
						<Input
							className="pl-8"
							placeholder={`Find a ${header}...`}
							value={searchQuery}
							onChange={(e) => setSearchQuery(e.target.value)}
						/>
					</div>
					<Table>
						<TableHeader>
							<TableRow>
								{tableHeader.map((header, index) => (
									<TableHead key={index}>{header}</TableHead>
								))}
							</TableRow>
						</TableHeader>
						<TableBody>
							{currentData.map((item, index) => (
								<TableRow key={index}>
									{data.id.map((idKey, cellIndex) => (
										<TableCell key={cellIndex}>
											{typeof item[idKey as keyof T] === 'object' &&
											item[idKey as keyof T] !== null ? (
												Object.keys(item[idKey as keyof T]).map((key) => {
													const value =
														item[idKey as keyof T][key as keyof typeof item];
													return value ? <div key={key}>{key}</div> : null;
												})
											) : (
												item[idKey as keyof T] ?? 'N/A'
											)}
										</TableCell>
									))}
									<TableCell className="text-right">
										{updateData && (
												<CreateEditButton
													name={header}
													type="Edit"
													state={setNewData}
													model={createData!.body}
													id={[{ name: 'id', value: item.id }]}
													onSubmit={handleUpdate}
													formValue={createData?.formValue || []}
													handleInputChange={handleInputChange}
												/>
										)}
										{deleteData && (
												<DeleteButton
													onDelete={async () => handleDelete(item.id)}
													description={`This action cannot be undone. This will permanently delete the cash register and remove all associated data.`}
												/>
										)}
									</TableCell>
								</TableRow>
							))}
						</TableBody>
					</Table>
					<Pagination>
						<PaginationContent>
							<PaginationItem
								onClick={() => {
									if (pageNumber > 0) {
										setPageNumber(pageNumber - 1);
									}
								}}
							>
								<PaginationPrevious />
							</PaginationItem>
							<PaginationItem>
								<PaginationLink>
									{pageNumber + 1}/{totalPages}
								</PaginationLink>
							</PaginationItem>
							<PaginationItem
								onClick={() => {
									if (pageNumber < totalPages - 1) {
										setPageNumber(pageNumber + 1);
									}
								}}
							>
								<PaginationNext />
							</PaginationItem>
						</PaginationContent>
					</Pagination>
				</div>
			</DrawerContent>
		</Drawer>
	);
}

