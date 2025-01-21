import React, { useEffect, useState } from 'react';
import * as z from 'zod';
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Select, SelectContent, SelectGroup, SelectItem, SelectLabel, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Plus, X } from 'lucide-react';
import { DatePicker } from "@/components/dashboard/date-picker";
import { format } from "date-fns";
import { CustomerModel } from "@/ResponseModel/CustomerModel";
import { Company } from "@/Models/Company";
import { ProductModel } from "@/ResponseModel/ProductModel";
import { http } from "@/services/HttpService";
import { toast } from "@/hooks/use-toast";
import {
	Drawer,
	DrawerContent,
	DrawerDescription,
	DrawerFooter,
	DrawerHeader,
	DrawerTitle,
	DrawerTrigger
} from "@/components/ui/drawer";
import ProductDetailBodyModel from "@/Models/ProductDetail";
import {CashRegisterModel} from "@/ResponseModel/CashRegisterModel";

const formSchema = z.object({
	issueDate: z.string().min(1, "Issue date is required"),
	dueDate: z.string().nullable().optional(),
	name: z.string().min(1, "Name is required"),
	description: z.string().min(1, "Description is required"),

	amount: z.number().min(0, "Amount must be non-negative"),

	status: z.number().int().min(0).default(1),
	operation: z.number().int().min(0).default(2),
	payment: z.number().int().min(0).default(2),

	customerId: z.string().nullable().optional(),
	cashRegisterId: z.string().min(1, "Cash Register is required"),
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
		).nullable().optional(),
})
	.refine((data) => {
		return data.payment === 1 ? data.dueDate : true;
	}, {
		message: "Due date is required",
		path: ["dueDate"],
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

type SalesFormProps = {
	customer: CustomerModel,
	companies: Company[],
	products: ProductModel[]
}

export function SalesForm({ customer, companies, products}: SalesFormProps) {
	const [isDrawerOpen, setIsDrawerOpen] = useState(false);
	const [transactionType, setTransactionType] = useState<'normal' | 'product'>('normal');
	const form = useForm({
		resolver: zodResolver(formSchema),
		defaultValues: ProductDetailBodyModel
	});

	const [payment, setPayment] = useState(0)
	const [cashRegisters, setCashRegisters] = useState<CashRegisterModel[]>([])

	const addProduct = () => {
		const currentProducts = form.getValues('products') || [];
		form.setValue('products', [...currentProducts, {
			productId: "",
			pricing: {
				quantity: 0,
				unitPrice: 0,
				taxRate: 0,
				totalPrice: 0,
				discountRate: 0
			}
		}]);
	};

	useEffect(() => {
		async function getCashRegister() {
			try {
				const response = await http.get<CashRegisterModel[]>("/CashRegister/GetAllCashRegister");
				if (response.data && response.isSuccessful) {
					setCashRegisters(response.data);
				}
			} catch (e) {

			}
		}
		getCashRegister().then();
	}, []);

	const removeProduct = (index: number) => {
		const currentProducts = form.getValues('products') || [];
		form.setValue('products', currentProducts.filter((_, i) => i !== index));
	};

	const calculateTotal = (index: number) => {
		const products = form.getValues('products') || [];
		const product = products[index];
		if (product) {
			const subtotal = product.pricing.quantity * product.pricing.unitPrice;
			const discountAmount = subtotal * (product.pricing.discountRate / 100);
			const taxAmount = (subtotal - discountAmount) * (product.pricing.taxRate / 100);
			const total = subtotal - discountAmount + taxAmount;

			form.setValue(`products.${index}.pricing.totalPrice`, total);
		}
	};


	function handleOnSubmit(values: z.infer<typeof formSchema>) {
		values.customerId = customer.id;
		async function addInvoice() {
			try {
				const response = await http.post('/CustomerDetail/CreateCustomerDetail', values)
				if (response.isSuccessful) {
					toast({
						title: "Success",
						description: "Sales transaction created successfully",
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
		<Drawer open={isDrawerOpen} onClose={() => setIsDrawerOpen(false)}>
			<DrawerTrigger asChild>
				<Button onClick={() => setIsDrawerOpen(true)} className="bg-indigo-950">
					Create Transaction
				</Button>
			</DrawerTrigger>
			<DrawerContent className="h-[95%]">
				<DrawerHeader>
					<DrawerTitle>New Transaction</DrawerTitle>
					<DrawerDescription>Fill in and save the transaction information.</DrawerDescription>
				</DrawerHeader>
				<div className="p-4 pb-0 overflow-y-auto">
					<Form {...form}>
						<form className="space-y-6">
							{/* Basic Information Card */}
							<Card>
								<CardHeader>
									<CardTitle>Basic Information</CardTitle>
								</CardHeader>
								<CardContent className="space-y-4">
									<FormField
										control={form.control}
										name="name"
										render={({field}) => (
											<FormItem>
												<FormLabel>Name <span className="text-red-600">*</span></FormLabel>
												<FormControl>
													<Input placeholder="Enter name" {...field} />
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
													<Input placeholder="Enter description" {...field} />
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
												<FormLabel>Payment Type</FormLabel>
												<Select
													onValueChange={(value) => {
														field.onChange(parseInt(value, 10))
														setPayment(parseInt(value, 10))
													}}
													value={field.value?.toString()}
												>
													<FormControl>
														<SelectTrigger>
															<SelectValue placeholder="Select payment type"/>
														</SelectTrigger>
													</FormControl>
													<SelectContent>
														<SelectItem value="1">Future</SelectItem>
														<SelectItem value="2">In Cash</SelectItem>
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
														<DatePicker
															disabled={(date) => date > new Date()}
															onSelect={(date) => field.onChange(format(date!, "yyyy-MM-dd"))}
														/>
													</FormControl>
													<FormMessage/>
												</FormItem>
											)}
										/>

										{form.watch('payment') === 1 && (
											<FormField
												control={form.control}
												name="dueDate"
												render={({field}) => (
													<FormItem>
														<FormLabel>Due Date</FormLabel>
														<FormControl>
															<DatePicker
																disabled={(date) => date < new Date(form.getValues('issueDate'))}
																onSelect={(date) => field.onChange(format(date!, "yyyy-MM-dd"))}
															/>
														</FormControl>
														<FormMessage/>
													</FormItem>
												)}
											/>
										)}
									</div>
								</CardContent>
							</Card>

							{/* Payment Details Card */}
							<Card>
								<CardHeader>
									<CardTitle>Payment Details</CardTitle>
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
								</CardContent>
							</Card>

							{/* Products Table Card */}
							<Card>
								<CardHeader>
									<CardTitle>Transaction Details</CardTitle>
								</CardHeader>
								<CardContent className="space-y-4">
									<FormField
										control={form.control}
										name={'cashRegisterId'}
										render={({field}) => (
											<FormItem>
												<FormLabel>Cash Register</FormLabel>
												<Select
													onValueChange={field.onChange}
													value={field.value}
												>
													<FormControl>
														<SelectTrigger>
															<SelectValue placeholder="Select CashRegister"/>
														</SelectTrigger>
													</FormControl>
													<SelectContent>
														{cashRegisters.map((cashregister) => (
															<SelectItem key={cashregister.id} value={cashregister.id}>
																{cashregister.name}
															</SelectItem>
														))}
													</SelectContent>
												</Select>
											</FormItem>
										)}
									/>

									<FormItem>
										<FormLabel>Transaction Type</FormLabel>
										<Select
											value={transactionType}
											onValueChange={(value: 'normal' | 'product') => {
												setTransactionType(value)
												value === "normal" ? form.setValue('products', null) : form.setValue('amount', 0)
											}}
										>
											<FormControl>
												<SelectTrigger>
													<SelectValue placeholder="Select transaction type"/>
												</SelectTrigger>
											</FormControl>
											<SelectContent>
												<SelectItem value="normal">Normal Transaction</SelectItem>
												<SelectItem value="product">Product Transaction</SelectItem>
											</SelectContent>
										</Select>
										<FormMessage/>
									</FormItem>

									{transactionType === 'normal' ? (
										<div className="space-y-4">
											<FormField
												control={form.control}
												name="amount"
												render={({field}) => (
													<FormItem>
														<FormLabel>Amount</FormLabel>
														<FormControl>
															<Input
																type="number"
																placeholder="Enter amount"
																{...field}
																onChange={(e) => field.onChange(Number(e.target.value))}
															/>
														</FormControl>
														<FormMessage/>
													</FormItem>
												)}
											/>
										</div>
									) : transactionType === 'product' ? (
										<>
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
										</>
									) : null}
								</CardContent>
							</Card>
						</form>
					</Form>
				</div>
				<DrawerFooter>
					<div className="flex justify-end space-x-4">
						<Button onClick={form.handleSubmit(handleOnSubmit)} type="submit">Save</Button>
						<Button type="button" variant="outline" onClick={() => setIsDrawerOpen(false)}>Cancel</Button>
					</div>
				</DrawerFooter>
			</DrawerContent>
		</Drawer>
	);
}