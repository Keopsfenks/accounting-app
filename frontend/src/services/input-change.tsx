import React from "react";

export const inputChangeService = <T extends Record<string, any>>(
	e: React.ChangeEvent<HTMLInputElement> | null,
	setData: React.Dispatch<React.SetStateAction<T>>,
	addrParams?: { name: string; value: string | ((prev: any) => any) }[]
) => {
	if (addrParams) {
		addrParams.forEach((param) => {
			setData((prev) => {
				if (param.name.includes('.')) {
					const [parentKey, childKey] = param.name.split('.');
					return {
						...prev,
						[parentKey]: {
							...(prev[parentKey] || {}),
							[childKey]: param.value
						}
					};
				}

				if (typeof param.value === 'function') {
					return {
						...prev,
						[param.name]: param.value(prev[param.name])
					};
				}

				return {
					...prev,
					[param.name]: param.value
				};
			});
		});
		return;
	}

	if (e) {
		const { name, value } = e.target;
		setData((prev) => ({
			...prev,
			[name]: value,
		}));
	}
};