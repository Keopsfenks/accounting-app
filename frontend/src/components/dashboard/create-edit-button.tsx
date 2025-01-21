'use client'

import React, {useEffect, useState} from "react";
import {Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger,} from "@/components/ui/dialog";
import {Button} from "@/components/ui/button";
import {Label} from "@/components/ui/label";
import {Input} from "@/components/ui/input";
import {Select, SelectContent, SelectGroup, SelectItem, SelectTrigger, SelectValue,} from "@/components/ui/select";
import {Checkbox} from "@/components/ui/checkbox";
import {DatePicker} from "@/components/dashboard/date-picker";
import {format} from "date-fns";
import {Edit} from "lucide-react";
import {cn} from "@/lib/utils";

export interface formValue {
	name: string;
	displayName: string;
	type: "input" | "select" | "date" | "optional" | "indicator";
	format?: React.HTMLInputTypeAttribute;
	dateFormat?: string;
	prefix?: string;
	options?: {
		value: string | boolean;
		format?: React.HTMLInputTypeAttribute;
		name: string;
	}[];
	optionalSelect?: {
		name: string;
		type: "input" | "select" | "date";
		inputChangeOptions: "object" | "default";
		options?: {
			format?: React.HTMLInputTypeAttribute | undefined;
			value: number | string | boolean;
			name: string;
		}[];
	};
	indicator?:
		{
			name: string;
			value: string;
		}[];
	exclusiveOptional?: boolean;
}


type CreateEditButtonProps<T extends Record<string, any>> = {
	name: string;
	id?: { name: string; value: string }[];
	type: "Create" | "Edit";
	state: React.Dispatch<React.SetStateAction<T>>;
	model: T;
	onSubmit: (e: React.FormEvent) => void;
	className?: string;
	formValue: formValue[];
	handleInputChange: (
		e: React.ChangeEvent<HTMLInputElement> | null,
		addrParam?: { name: any; value: any }[]
	) => void;
};

export default <T extends Record<string, any>>({
					name,
					id,
					type,
					onSubmit,
					formValue,
					className,
					state,
					model,
					handleInputChange,
				}: CreateEditButtonProps<T> ) => {
	const [isOpen, setIsOpen] = useState(false);
	const [optionalSelections, setOptionalSelections] = useState<string | null>(
		null
	);

	function toggleOptional(itemName: string, exclusive?: boolean) {
		if (exclusive) {
			setOptionalSelections((prev) => (prev === itemName ? null : itemName));
			handleInputChange(null, [{ name: itemName, value: optionalSelections === itemName ? null : "" }]);
		} else {
			setOptionalSelections((prev) => {
				const selections = prev?.split(",") || [];
				if (selections.includes(itemName)) {
					return selections.filter((name) => name !== itemName).join(",") || null;
				}
				return prev ? `${prev},${itemName}` : itemName;
			});
			handleInputChange(null, [{ name: itemName, value: null }]);
		}
	}

	useEffect(() => {
		state(model);
	}, [isOpen]);

	return (
		<Dialog open={isOpen} onOpenChange={setIsOpen}>
			<DialogTrigger asChild>
				{type === "Edit" ? (
					<Button
						onClick={() => handleInputChange(null, id)}
						variant="ghost"
						size="icon"
						className="h-8 w-8"
					>
						<Edit className="h-4 w-4" />
					</Button>
				) : (
					<Button className={cn(className)} onClick={() => handleInputChange(null, id)}>
						Create {name}
					</Button>
				)}
			</DialogTrigger>
			<DialogContent className="sm:max-w-[425px] max-h-[80vh] overflow-y-auto">
				<DialogHeader>
					<DialogTitle>
						{type} New {name}
					</DialogTitle>
				</DialogHeader>
				<form onSubmit={onSubmit} className="space-y-4">
					{formValue.map((item, index) => (
						<div key={index}>
							{item.type === "input" && (
								<div className="space-y-2">
									<Label htmlFor={item.name}>
										{name} {item.displayName}
									</Label>
									<Input
										id={item.name}
										name={item.name}
										type={item.format}
										onChange={item.prefix ? (e) => handleInputChange(e, [{ name: item.name, value: item.prefix + e.target.value }]) : (e) => handleInputChange(e, [{ name: item.name, value: e.target.value }])}
										required
									/>
								</div>
							)}
							{item.type === "select" && item.options && (
								<div className="space-y-2">
									<Label>{item.displayName}</Label>
									<Select
										onValueChange={(value) =>
											handleInputChange(null, [
												{ name: item.name, value: value },
											])
										}
									>
										<SelectTrigger className="w-full">
											<SelectValue
												placeholder={`Select ${item.displayName}`}
											/>
										</SelectTrigger>
										<SelectContent>
											<SelectGroup>
												{item.options.map((option, idx) => (
													<SelectItem
														key={idx}
														value={option.value.toString()}
													>
														{option.name}
													</SelectItem>
												))}
											</SelectGroup>
										</SelectContent>
									</Select>
								</div>
							)}
							{item.type === "date" && (
								<div className="space-y-2">
									<Label>
										{name} {item.displayName}
									</Label>
									<DatePicker
										onSelect={(date) =>
											handleInputChange(null, [
												{
													name: item.name,
													value: format(date!, item.dateFormat || "yyyy-MM-dd"),
												},
											])
										}
									/>
								</div>
							)}
							{item.type === "optional" && (
								<div className="space-y-2">
									<div className="flex items-center space-x-2">
										{}
										<Checkbox
											id={`optional-${item.name}`}
											checked={
												item.exclusiveOptional
													? optionalSelections === item.name
													: optionalSelections?.includes(item.name)
											}
											onCheckedChange={() =>
												toggleOptional(item.name, item.exclusiveOptional)
											}
										/>
										<Label htmlFor={`optional-${item.name}`}>{item.displayName}</Label>
									</div>
									{optionalSelections?.includes(item.name) &&
										item.optionalSelect &&
										(() => {
											const { type, name, options, inputChangeOptions } = item.optionalSelect!;
											if (type === "input") {
												return options?.map((option, idx) => (
													<div key={idx} className="space-y-2">
														<Label htmlFor={option.name}>
															{item.displayName} {option.name}
														</Label>
														<Input
															id={option.name}
															name={option.name}
															type={option.format}
															placeholder={`Enter ${option.value}`}
															onChange={(e) => {
																inputChangeOptions === "object" ?
																handleInputChange(e, [{ name: item.name + "." + option.name, value: e.target.value }])
																	: handleInputChange(e, [{ name: option.name, value: e.target.value }]);
															}}
														/>
													</div>
												));
											}
											if (type === "select" && options) {
												return (
													<Select
														onValueChange={(value) =>
															handleInputChange(null, [{ name: name, value: value }])
														}
													>
														<SelectTrigger className="w-full">
															<SelectValue placeholder={`Select ${name}`} />
														</SelectTrigger>
														<SelectContent>
															<SelectGroup>
																{options.map((option, idx) => (
																	<SelectItem key={idx} value={option.value.toString()}>
																		{option.name}
																	</SelectItem>
																))}
															</SelectGroup>
														</SelectContent>
													</Select>
												);
											}
											if (type === "date") {
												return (
													<DatePicker
														onSelect={(date) =>
															handleInputChange(null, [
																{
																	name: name,
																	value: format(date!, "yyyy-MM-dd"),
																},
															])
														}
													/>
												);
											}
											return null;
										})()}
								</div>
							)}
						</div>
					))}
					<Button type="submit">
						{type} {name}
					</Button>
				</form>
			</DialogContent>
		</Dialog>
	);
};
