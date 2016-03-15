Feature: AddSensor
	In order to add Sensor under a Pump
	As an admin
	I want to add Sensor under a pump

Scenario: Add a WaterLevel Sensor
	Given I have entered WaterLevel Sensor with following property
	| field | value |
	|    Id    |   1           |
	| Name     | WT      |
	| Value    | "11" |
	When I add Sensor
	Then I will check the Sensor name
