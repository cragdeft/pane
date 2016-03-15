Feature: AddCamera
	In order to add Camera under a Pump station
	As an admin
	I want to add Camera under a pump

Scenario: Add a Camera Device
	Given I have entered Camera Sensor with following property
	| field | value |
	|    Id    |   1           |
	| Name     | Camera     |
	| Value    | "11" |
	When I add Camera
	Then I will check the Camera name
