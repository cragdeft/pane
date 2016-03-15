Feature: AddDma
	In order to add Dma under a DMA
	As an admin
	I want to add DMA under a zone

Scenario: Add DMA
	Given I have entered DMA with following property  
	| field    | value         |
	| Name     | DMA 810       |
	| Location | "11.12,11.13" |
	| Street1  | Road 1        |
	| Street2  | House 1       |
	| Zip      | 1230          |
	| City     | Dhaka         |
	When I press add
	Then I will check the DMA name
