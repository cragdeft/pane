Feature: AddPumpStation
	In order to add Dma under a PumpStation
	As an admin
	I want to add Pump under a DMA

Scenario: Add Pump Station
	Given I have entered Pump with following property  
	| field    | value            |
	| Name     | Baridhara Pump |
	| Location | "11.12,11.13"    |
	| Street1  | Road 1           |
	| Street2  | House 1          |
	| Zip      | 1230             |
	| City     | Dhaka            |
	When I  add
	Then I will check the Pump name and DMA
