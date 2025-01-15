export interface CashRegisterDetails {
	id: string;
	processor: object
	date: string;
	description: string;
	depositAmount: number;
	withdrawalAmount: number;
	opposite: object
}

export interface CashRegisterModel {
	id: string;
	name: string;
	currencyType: {
		name: string;
		value: number;
	};
	depositAmount: number;
	withdrawalAmount: number;
	balanceAmount: number;
	details: CashRegisterDetails[];
}