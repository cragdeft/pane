Feature: AddPump
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Scenario: Add a Pump Device
	Given I have entered Pump Device with following property
	| field | value |
	|    Id    |   1           |
	| Name     | Pump     |
	| Value    | "11" |
	When I add Pump
	Then I will check the Pump name
