Feature: DriverFunctionality

Background: 
	Given I have registered SnapIn from assembly 'Zookeeper.PSProvider'
	And I have Powershell host
	And Powershell add following script 'Add-PSSnapin ZookeeperPSSnap'
	And Powershell add following script 'New-PSDrive -Name Zookeeper -PSProvider Zookeeeper -Root /'
	And Powershell add following script 'cd Zookeeper:'
	And I have zookeeper client connected to '127.0.0.1:2181'
	And I clear zookeeper configuration
	And Powershell execute scheduled commands

Scenario: 'ls' should return all items in path
	Then Executing script '(ls).Name' should return following items
	| Item       |
	| zookeeper |

Scenario: 'cd' should change current dictionary
	When Powershell execute following script 'cd zookeeper'
	Then Executing script '(ls).Name' should return following items
	| Item  |
	| quota |
	Then Executing script '$pwd.Path' should return following items
	| Item       |
	| Zookeeper:\zookeeper |

Scenario: new-item should create new item
	When Powershell execute following script 'New-Item -name Test -ItemType Node -Value "Test"'
	Then Executing script '(ls).Name' should return following items
	| Item      |
	| Test      |
	| zookeeper |

Scenario: Get-Item should return data and stat for item
	When Powershell execute following script 'New-Item -name Test -ItemType Node -Value TestValue'
	Then Executing script '(Get-Item -Path Test) -ne $null' should return following items
	| Item      |
	| True |

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

Scenario: Create new item in sub folder
	When Powershell execute following script 'New-Item -name Test'
	And Powershell execute following script 'New-Item -name SubTest -Path .\Test'
	Then Executing script '(ls .\Test).Name' should return following items
	| Item       |
	| SubTest |


Scenario: Get-ChildItem with wild card
	When Powershell execute following script 'New-Item -name Test'
	Then Executing script '(ls Tes*) -ne $null' should return following items
	| Item |
	| True |

Scenario: Get-ChildItem -Recurse should retrun elements Recurse
	When Powershell execute following script 'New-Item -name Test'
	When Powershell execute following script 'New-Item -name SubTest -Path .\Test\'
	When Powershell execute following script 'New-Item -name SubSubTest1 -Path .\Test\SubTest'
	When Powershell execute following script 'New-Item -name SubSubTest2 -Path .\Test\SubTest'
	Then Executing script 'Get-ChildItem -Recurse -Path Tes*' should return following items
	| Item        |
	| SubTest     |
	| SubSubTest2 |
	| SubSubTest1 |

Scenario: Get-ChildItem -Recurse without path should retrun elements Recurse
	When Powershell execute following script 'New-Item -name Test'
	And Powershell execute following script 'New-Item -name SubTest -Path .\Test\'
	Then Executing script 'Get-ChildItem -Recurse' should return following items
	| Item      |
	| /         |
	| Test      |
	| SubTest   |
	| zookeeper |
	| quota     |
