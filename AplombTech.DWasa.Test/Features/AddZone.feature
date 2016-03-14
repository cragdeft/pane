Feature: Add Zone
	In order to add zone
	As a records manager
	I want to add a new Zone


Scenario: Add Zone
	Given Given I have entered following property  
	| field    | value         |
	| Name     | "Zone 8"       |
	| Location | "11.12,11.13" |
	|    Line1      |      Road 1        |
	|    Line2      |      House 1         |
	|    Line3      |      Dhaka         |
	When I Add
	Then then I will check the name
