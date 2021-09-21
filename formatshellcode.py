#!/usr/bin/python3
import sys
var = ""
array = []
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

print(var)
