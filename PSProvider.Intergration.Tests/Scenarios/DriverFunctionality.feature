Feature: DriverFunctionality

Background: 
	Given I have registered SnapIn from assembly 'Zookeeper.PSProvider'
	And I have Powershell host
	And Powershell add following script 'Add-PSSnapin ZookeeperPSSnap'
	And Powershell add following script 'New-PSDrive -Name Zookeeper -PSProvider Zookeeeper -Root 127.0.0.1:/'
	And Powershell add following script 'cd Zookeeper:'
	And I have zookeeper client connected to '127.0.0.1'
	And I clear zookeeper configuration
	And Powershell execute scheduled commands

Scenario: 'ls' should return all items in path
	Then Executing script 'ls' should return following items
	| Item       |
	| zookeeper |

Scenario: 'cd' should change current dictionary
	When Powershell execute following script 'cd zookeeper'
	Then Executing script 'ls' should return following items
	| Item  |
	| quota |
	Then Executing script '$pwd.Path' should return following items
	| Item       |
	| Zookeeper:\zookeeper |

Scenario: new-item should create new item
	When Powershell execute following script 'New-Item -name Test -ItemType Node'
	Then Executing script 'ls' should return following items
	| Item      |
	| Test      |
	| zookeeper |

Scenario: Get-Item should return data and stat for item
	When Powershell execute following script 'New-Item -name Test -ItemType Node -Value TestValue'
	Then Executing script '(Get-Item -Path Test).Data' should return following items
	| Item      |
	| TestValue |

Scenario: Get-Content should return data using given encoding
	When Powershell execute following script 'New-Item -name Test -ItemType Node -Value TestValue'
	Then Executing script 'Get-Content -Path Test -Encoding UTF8' should return following items
	| Item      |
	| TestValue |
Scenario: Set-Content with encoding should change data
	When Powershell execute following script 'New-Item -name Test -ItemType Node -Value TestValue'
	And Powershell execute following script 'Set-Content -Path Test -Encoding UTF8 -Value TestValue2'
	Then Executing script 'Get-Content -Path Test -Encoding UTF8' should return following items
	| Item       |
	| TestValue2 |

Scenario: Sub folders should work fine
	When Powershell execute following script 'New-Item -name Test -ItemType Node -Value TestValue'
	And Powershell execute following script 'cd Test'
	And Powershell execute following script 'New-Item -name SubTest -ItemType Node -Value SubTestValue'
	Then Executing script 'Get-Content SubTest -Encoding UTF8' should return following items
	| Item       |
	| SubTestValue |