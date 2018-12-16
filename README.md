# MSSQLUDPScanner

MSSQLUDPScanner is a c# port of NetSPI's [Get-SQLInstanceScanUDPThreaded](https://github.com/NetSPI/PowerUpSQL/blob/master/PowerUpSQL.ps1)


As an example, one could execute MSSQLUDPScanner.exe through Cobalt Strike's Beacon "execute-assembly" module.


#### Example usage
beacon>execute-assembly /root/MSSQLUDPScanner/MSSQLUDPScanner.exe --cidr "192.168.1.0/24"