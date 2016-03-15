Feature: AddRouter
	In order to add Router under a Pump station
	As an admin
	I want to add Router under a pump

Scenario: Add a Router Device
	Given I have entered Router with following property
	| field | value |
	|    Id    |   1           |
	| Name     | Router     |
	| Value    | "11" |
	When I add Router
	Then I will check the Router name