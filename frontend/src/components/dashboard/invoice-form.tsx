import React, {useState} from 'react';
import * as z from 'zod';
import {useForm} from "react-hook-form";
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Select, SelectContent, SelectGroup, SelectItem, SelectLabel, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Label } from "@/components/ui/label";
import { Plus, X, Search } from 'lucide-react';
import { DatePicker } from "@/components/dashboard/date-picker";
import { format } from "date-fns";
import {zodResolver} from "@hookform/resolvers/zod";
import {InvoiceBodyModel} from "@/Models/Invoice";
import {CustomerModel} from "@/ResponseModel/CustomerModel";
import {Company} from "@/Models/Company";
import {ProductModel} from "@/ResponseModel/ProductModel";
import {http} from "@/services/HttpService";
import {toast} from "@/hooks/use-toast";
import {
	Drawer,
	DrawerContent,
	DrawerDescription, DrawerFooter,
	DrawerHeader,
	DrawerTitle,
	DrawerTrigger
} from "@/components/ui/drawer";

const formSchema = z.object({
	// Required fields
	invoiceNumber: z.string().min(1, "Invoice number is required"),
	description: z.string().min(1, "Description is required"),
	issueDate: z.string().min(1, "Issue date is required"),

	// Optional fields with specific formats
	dueDate: z.string().nullable().optional(),
	company: z.string().nullable().optional(),
	customerId: z.string().nullable().optional(),

	// Numeric fields
	status: z.number().int().min(0).default(1),
	operation: z.number().int().min(0).default(2),
	payment: z.number().int().min(0).default(2),
	currencyType: z.number().int().min(0).default(1),

	// Array of products
	products: z
		.array(
			z.object({
				productId: z.string(),
				pricing: z.object({
					quantity: z.number().min(0).default(1),
					unitPrice: z.number().min(0).default(0),
					discountRate: z.number().min(0).default(0),
					taxRate: z.number().min(0).default(0),
					totalPrice: z.number().min(0).default(0),
				}).default({
					quantity: 1,
					unitPrice: 0,
					discountRate: 0,
					taxRate: 0,
					totalPrice: 0,
				})
			})
		)
		.default([
			{
				productId: "",
				pricing: {
					quantity: 1,
					unitPrice: 0,
					discountRate: 0,
					taxRate: 0,
					totalPrice: 0,
				}
			},
		]),
})
	.refine((data) => {
		return data.payment === 1 ? data.dueDate : true;
	}, {
		message: "Due date is required",
		path: ["dueDate"],
	})
	.refine((data) => {
		return data.products.length > 0;
	}, {
		message: "At least one product must be added",
		path: ["products"],
	})
	.refine((data) => {
		if (data.dueDate && data.issueDate) {
			return new Date(data.dueDate) >= new Date(data.issueDate);
		}
		return true;
	}, {
		message: "Due date must be after or equal to issue date",
		path: ["dueDate"],
	})


type InvoiceFormProps = {
	customer: CustomerModel,
	companies: Company[],
	products: ProductModel[]
}

export function InvoiceForm({ customer, companies, products} : InvoiceFormProps) {
	const [payment, setPayment] = useState(0)
	const form = useForm({
		resolver: zodResolver(formSchema),
		defaultValues: InvoiceBodyModel
	});
	const [isDrawerOpen, setIsDrawerOpen] = useState(false);

	const handleCancel = () => {
		setIsDrawerOpen(false);
	};
	const addProduct = () => {
		const currentProducts = form.getValues('products') || [];
		form.setValue('products', [...currentProducts, {
			productId: "",
			pricing: {
				quantity: 0,
				unitPrice: 0,
				discountRate: 0,
				taxRate: 0,
				totalPrice: 0,
			}
		}]);
	};
	const removeProduct = (index: number) => {
		const currentProducts = form.getValues('products');
		form.setValue('products', currentProducts.filter((_, i) => i !== index));
	};

	// Calculate total when quantity, unitPrice, discount or taxRate changes
	const calculateTotal = (index: number) => {
		const products = form.getValues('products');
		const product = products[index];
		const subtotal = product.pricing.quantity * product.pricing.unitPrice;
		const discountAmount = subtotal * (product.pricing.discountRate / 100);
		const taxAmount = (subtotal - discountAmount) * (product.pricing.taxRate / 100);
		const total = subtotal - discountAmount + taxAmount;

		form.setValue(`products.${index}.pricing.totalPrice`, total);
	};

	function handleOnSubmit(values: z.infer<typeof formSchema>) {
		values.customerId = customer.id;
		async function addInvoice() {
			try {
				const response = await http.post('/Invoice/CreateInvoice', values)
				if (response.isSuccessful) {
					toast({
						title: "Success",
						description: "Invoice created successfully",
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
		addInvoice().then();
	}


	return (
		<Drawer open={isDrawerOpen} onClose={handleCancel}>
			<DrawerTrigger asChild>
				<Button className="ml-auto w-auto size-auto bg-indigo-950" onClick={() => {
					form.reset();
					setIsDrawerOpen(true)
				} }>Create Invoice</Button>
			</DrawerTrigger>
			<DrawerContent className="h-[95%]">
				<DrawerHeader>
					<DrawerTitle>New Invoice</DrawerTitle>
					<DrawerDescription>Fill in and save the invoice information.</DrawerDescription>
				</DrawerHeader>
				<div className="p-4 pb-0 overflow-y-auto">
					<Form {...form}>
						<form className="space-y-6">
							{/* Basic Invoice Information */}
							<Card>
								<CardHeader>
									<CardTitle>Basic Information</CardTitle>
								</CardHeader>
								<CardContent className="space-y-4">
									<FormField
										control={form.control}
										name="invoiceNumber"
										render={({field}) => (
											<FormItem>
												<FormLabel>Invoice Number <span className="text-red-600">*</span></FormLabel>
												<FormControl>
													<Input placeholder="Enter invoice number" {...field} />
												</FormControl>
												<FormMessage/>
											</FormItem>
										)}
									/>
									<FormField
										control={form.control}
										name="description"
										render={({field}) => (
											<FormItem>
												<FormLabel>Description <span className="text-red-600">*</span></FormLabel>
												<FormControl>
													<Input placeholder="Enter Description" {...field} />
												</FormControl>
												<FormMessage/>
											</FormItem>
										)}
									/>
									<FormField
										control={form.control}
										name="payment"
										render={({field}) => (
											<FormItem>
												<FormLabel>Payment</FormLabel>
												<Select
													onValueChange={(value) => {
														field.onChange(parseInt(value, 10))
														setPayment(parseInt(value, 10))
													}}
													value={field.value?.toString()}
												>
													<FormControl>
														<SelectTrigger>
															<SelectValue placeholder="Select Payment"/>
														</SelectTrigger>
													</FormControl>
													<SelectContent>
														<SelectGroup>
															<SelectItem value="1">Future</SelectItem>
															<SelectItem value="2">In Cash</SelectItem>
														</SelectGroup>
													</SelectContent>
												</Select>
												<FormMessage/>
											</FormItem>
										)}
									/>
									<div className="grid grid-cols-2 gap-4">
										<FormField
											control={form.control}
											name="issueDate"
											render={({field}) => (
												<FormItem>
													<FormLabel>Issue Date <span className="text-red-600">*</span></FormLabel>
													<FormControl>
														<div className="relative">
															<Input className="hidden" {...field} />
															<DatePicker
																disabled={(date) => date > new Date()}
																onSelect={(date) => field.onChange(format(date!, "yyyy-MM-dd"))}
															/>
														</div>
													</FormControl>
													<FormMessage/>
												</FormItem>
											)}
										/>
										{payment === 1 && (
											<FormField
												control={form.control}
												name="dueDate"
												render={({field}) => (
													<FormItem>
														<FormLabel>Due Date</FormLabel>
														<FormControl>
															<div className="relative">
																<Input
																	className="hidden"
																	{...field}
																	value={field.value || ''} // Convert null/undefined to empty string
																/>
																<DatePicker
																	disabled={(date) => date < new Date(form.getValues('issueDate'))}
																	onSelect={(date) => field.onChange(format(date!, "yyyy-MM-dd"))}
																/>
															</div>
														</FormControl>
														<FormMessage/>
													</FormItem>
												)}
											/>
										)}
									</div>
								</CardContent>
							</Card>

							{/* Company and Customer Information */}
							<Card>
								<CardHeader>
									<CardTitle>Company and Customer Information</CardTitle>
								</CardHeader>
								<CardContent className="space-y-4">
									<div>
										<Label>Customer Name</Label>
										<div className="relative">
											<Input placeholder={customer.name} disabled/>
											<Search
												className="absolute right-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400"/>
										</div>
									</div>

									<FormField
										control={form.control}
										name="company"
										render={({field}) => (
											<FormItem>
												<FormLabel>Company Name</FormLabel>
												<Select onValueChange={field.onChange} value={field.value || ""}>
													<FormControl>
														<SelectTrigger>
															<SelectValue placeholder="Select company"/>
														</SelectTrigger>
													</FormControl>
													<SelectContent>
														<SelectGroup>
															<SelectLabel>Companies</SelectLabel>
															{companies.map((item) => (
																<SelectItem key={item.Id} value={item.Id}>
																	{item.Name}
																</SelectItem>
															))}
														</SelectGroup>
													</SelectContent>
												</Select>
												<FormMessage/>
											</FormItem>
										)}
									/>
								</CardContent>
							</Card>

							{/* Invoice Status and Type */}
							<Card>
								<CardHeader>
									<CardTitle>Invoice Details</CardTitle>
								</CardHeader>
								<CardContent className="space-y-4">
									<div className="grid grid-cols-2 gap-4">
										<FormField
											control={form.control}
											name="status"
											render={({field}) => (
												<FormItem>
													<FormLabel>Status</FormLabel>
													<Select
														onValueChange={(value) => field.onChange(parseInt(value, 10))}
														value={field.value?.toString()}
													>
														<FormControl>
															<SelectTrigger>
																<SelectValue placeholder="Select status"/>
															</SelectTrigger>
														</FormControl>
														<SelectContent>
															<SelectItem value="1">Draft</SelectItem>
															<SelectItem value="2">Sent</SelectItem>
															<SelectItem value="3">Paid</SelectItem>
															<SelectItem value="4">Cancelled</SelectItem>
														</SelectContent>
													</Select>
													<FormMessage/>
												</FormItem>
											)}
										/>
									</div>

									<div className="grid grid-cols-2 gap-4">
										<FormField
											control={form.control}
											name="currencyType"
											render={({field}) => (
												<FormItem>
													<FormLabel>Currency</FormLabel>
													<Select
														onValueChange={(value) => field.onChange(parseInt(value, 10))}
														value={field.value?.toString()}
													>
														<FormControl>
															<SelectTrigger>
																<SelectValue placeholder="Select currency"/>
															</SelectTrigger>
														</FormControl>
														<SelectContent>
															<SelectItem value="1">TRY</SelectItem>
															<SelectItem value="2">USD</SelectItem>
															<SelectItem value="3">EUR</SelectItem>
														</SelectContent>
													</Select>
													<FormMessage/>
												</FormItem>
											)}
										/>
									</div>
								</CardContent>
							</Card>

							{/* Products Table */}
							<Card>
								<CardHeader>
									<CardTitle>Products</CardTitle>
								</CardHeader>
								<CardContent>
									<Table>
										<TableHeader>
											<TableRow>
												<TableHead>Product ID</TableHead>
												<TableHead>Unit Of Measure</TableHead>
												<TableHead>Quantity</TableHead>
												<TableHead>Unit Price</TableHead>
												<TableHead>Discount (%)</TableHead>
												<TableHead>Tax Rate (%)</TableHead>
												<TableHead>Total</TableHead>
												<TableHead></TableHead>
											</TableRow>
										</TableHeader>
										<TableBody>
											{form.watch('products')?.map((product, index) => (
												<TableRow key={index}>
													<TableCell>
														<FormField
															control={form.control}
															name={`products.${index}.productId`}
															render={({field}) => (
																<FormItem>
																	<FormControl>
																		<Select onValueChange={(value) => {
																			field.onChange(value);
																		}} value={field.value}>
																			<FormControl>
																				<SelectTrigger>
																					<SelectValue placeholder="Select Products"/>
																				</SelectTrigger>
																			</FormControl>
																			<SelectContent>
																				<SelectGroup>
																					<SelectLabel>Products</SelectLabel>
																					{products.map((item) => (
																						<SelectItem key={item.id} value={item.id}>
																							{item.name}
																						</SelectItem>
																					))}
																				</SelectGroup>
																			</SelectContent>
																		</Select>
																	</FormControl>
																	<FormMessage/>
																</FormItem>
															)}
														/>
													</TableCell>
													<TableCell>
														<Input placeholder={products.find((item) => item.id === form.getValues(`products.${index}.productId`))?.unitOfMeasure.name} disabled />
													</TableCell>
													<TableCell>
														<FormField
															control={form.control}
															name={`products.${index}.pricing.quantity`}
															render={({field}) => (
																<FormItem>
																	<FormControl>
																		<Input
																			type="number"
																			{...field}
																			onChange={(e) => {
																				field.onChange(Number(e.target.value));
																				calculateTotal(index);
																			}}
																		/>
																	</FormControl>
																	<FormMessage/>
																</FormItem>
															)}
														/>
													</TableCell>
													<TableCell>
														<FormField
															control={form.control}
															name={`products.${index}.pricing.unitPrice`}
															render={({field}) => (
																<FormItem>
																	<FormControl>
																		<Input
																			type="number"
																			{...field}
																			onChange={(e) => {
																				field.onChange(Number(e.target.value));
																				calculateTotal(index);
																			}}
																		/>
																	</FormControl>
																	<FormMessage/>
																</FormItem>
															)}
														/>
													</TableCell>
													<TableCell>
														<FormField
															control={form.control}
															name={`products.${index}.pricing.discountRate`}
															render={({field}) => (
																<FormItem>
																	<FormControl>
																		<Input
																			type="number"
																			{...field}
																			onChange={(e) => {
																				field.onChange(Number(e.target.value));
																				calculateTotal(index);
																			}}
																		/>
																	</FormControl>
																	<FormMessage/>
																</FormItem>
															)}
														/>
													</TableCell>
													<TableCell>
														<FormField
															control={form.control}
															name={`products.${index}.pricing.taxRate`}
															render={({field}) => (
																<FormItem>
																	<Select
																		onValueChange={(value) => {
																			field.onChange(Number(value));
																			calculateTotal(index);
																		}}
																		value={field.value.toString()}
																	>
																		<FormControl>
																			<SelectTrigger>
																				<SelectValue/>
																			</SelectTrigger>
																		</FormControl>
																		<SelectContent>
																			<SelectItem value="0">0%</SelectItem>
																			<SelectItem value="1">1%</SelectItem>
																			<SelectItem value="8">8%</SelectItem>
																			<SelectItem value="18">18%</SelectItem>
																		</SelectContent>
																	</Select>
																	<FormMessage/>
																</FormItem>
															)}
														/>
													</TableCell>
													<TableCell>
														<FormField
															control={form.control}
															name={`products.${index}.pricing.totalPrice`}
															render={({field}) => (
																<FormItem>
																	<FormControl>
																		<Input type="number" {...field} readOnly/>
																	</FormControl>
																	<FormMessage/>
																</FormItem>
															)}
														/>
													</TableCell>
													<TableCell>
														<Button
															variant="ghost"
															size="icon"
															type="button"
															onClick={() => removeProduct(index)}
														>
															<X className="h-4 w-4"/>
														</Button>
													</TableCell>
												</TableRow>
											))}
										</TableBody>
									</Table>
									<Button variant="outline" className="mt-4" type="button" onClick={addProduct}>
										<Plus className="h-4 w-4 mr-2"/>
										Add new line
									</Button>
								</CardContent>
							</Card>
						</form>
					</Form>
				</div>
				<DrawerFooter>
					<div className="flex justify-end space-x-4">
						<Button onClick={form.handleSubmit(handleOnSubmit)} type="submit">Save</Button>
						<Button type="button" variant="outline" onClick={() => handleCancel()}>Cancel</Button>
					</div>
				</DrawerFooter>
			</DrawerContent>

		</Drawer>
	);
}