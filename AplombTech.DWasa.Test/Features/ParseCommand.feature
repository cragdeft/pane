Feature: ParseCommand for Feedback
	In order to parse commands
	As a records manager
	I want to Parse Json

	
Scenario: Parse Command
	Given I have entered following property  
	| field				| value			|
	| JsonString | { "DataLogId": 1, "Production": "00", "Energy": "45", "Pressure": "209","WaterLevel": "32769", "NetworkType":1, "LogDateTime":"1/1/2015","CheckSum": "123","Clorination": "58"} |
	And I created a parser with that string
	When I parse It will save the data
