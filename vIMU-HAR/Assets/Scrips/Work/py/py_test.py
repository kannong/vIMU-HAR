import sys

def a_plus_b(a,b):
    a = int(a)
    b = int(b)
    return a+b

print(a_plus_b(sys.argv[1],sys.argv[2]))
