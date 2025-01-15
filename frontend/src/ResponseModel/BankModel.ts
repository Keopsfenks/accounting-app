export interface BankDetails {
	id: string;
	processor: object
	date: string;
	description: string;
	depositAmount: number;
	withdrawalAmount: number;
	opposite: object
}

export interface BankModel {
	id: string;
	name: string;
	iban: string
	currencyType: {
		name: string;
		value: number;
	};
	depositAmount: number;
	withdrawAmount: number;
	balance: number;
	details: BankDetails[];
}