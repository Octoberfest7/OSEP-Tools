#!/usr/bin/python3
import sys
var = ""
array = []
if len(sys.argv) == 1 or len(sys.argv) != 4:
	sys.exit("usage: python3 portip_eggs.py <shellcode file> <ip> <port>")
with open(sys.argv[1]) as f:
	for line in f:
		array.append(line)
	for line in array:
		if line == array[0]:
			pass
		elif line == array[-1]:
			line = line.strip(" };\n")
			var = var + line
		else:
			line = line.strip("\n")
			var = var + line
#convert sysargv2 (ip) in hex
iparray = sys.argv[2].split(".")
x=0
for i in iparray:
	temp = hex(int(i))
	if len(temp) == 3:
		temp = "0x0" + temp[-1]
	iparray[x] = temp
	x = x + 1
ipbytes = ",".join(iparray)

#convert sysargv3 (port) in hex
port = hex(int(sys.argv[3]))
firstbyte = "0x00"
if len(port) > 4:
	if len(port) == 5:
		firstbyte = "0x0" + port[2]
	else:
		firstbyte = "0x" + port[2] + port[3]
secondbyte = "0x" + port[-2] + port[-1]
portbytes = firstbyte + "," + secondbyte

var = var.replace(ipbytes,"0x11,0x11,0x11,0x11").replace(portbytes,"0x22,0x22")
print(var)
