Feature: Add Zone
	In order to add zone
	As a records manager
	I want to add a new Zone


Scenario: Add Zone
	Given I have entered following property  
	| field    | value         |
	| Name     | Zone 8       |
	| Location | "11.12,11.13" |
	| Street1  | Road 1        |
	| Street2  | House 1       |
	| Zip      | 1230          |
	| City     | Dhaka         |
	When I Add
	Then I will check the Zone name
