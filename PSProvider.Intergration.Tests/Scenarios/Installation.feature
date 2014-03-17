Feature: Installation
	I want install Zookeeper.PSProvider and be able to use it

Background: 
	Given I have registered SnapIn from assembly 'Zookeeper.PSProvider'
	And I have Powershell host

Scenario: Zookeeper.PSProvider should be installed and Zookeeper driver should be visible
	Then Executing script '$(Get-PSSnapin -Registered | where { $_.Name -eq "ZookeeperPSSnap" } ) -ne $null' should return true

Scenario: I want have zookeeper PsProvider
	And Powershell add following script 'Add-PSSnapin ZookeeperPSSnap -Verbose -Debug'
	Then Executing script '( Get-PSProvider | where Name -eq "Zookeeeper" ) -ne $null' should return true

Scenario: I want have zookeeper driver
	And Powershell add following script 'Add-PSSnapin ZookeeperPSSnap'
	And Powershell add following script 'New-PSDrive -Name Zookeeper -PSProvider Zookeeeper -Root 127.0.0.1'
	Then Executing script '( Get-PSDrive | where Name -eq "Zookeeper" ) -ne $null' should return true

Scenario: I want to switch to Zookeper driver
	And Powershell add following script 'Add-PSSnapin ZookeeperPSSnap'
	And Powershell add following script 'New-PSDrive -Name Zookeeper -PSProvider Zookeeeper -Root 127.0.0.1'
	And Powershell add following script '( Get-PSDrive | where Name -eq "Zookeeper" ) -ne $null'
	Then Executing script 'cd Zookeeper:' should not generate errors