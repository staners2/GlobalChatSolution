import argparse
import sys
from itertools import cycle

def createParser():
    parser = argparse.ArgumentParser()
    parser.add_argument('--message', "-m")
    parser.add_argument('--e', "-encrypt", action="store_true") # шифровать --no-e дешфировать

    return parser

def append_to_size(word: str, length: int):
    while len(word) <= length:
        word += word

    return word[0:length]

def encrypt(text, key):
    key = append_to_size(key, len(text))
    # print(f"text: {text}\n key: {key}")
    l = [a ^ b for (a, b) in zip(bytes(text, "UTF-8"), bytes(key, "UTF-8"))]  # UTF-8 в двоичном виде
    return ",".join(str(x) for x in l)


def descrypt(text, key):
    l = [int(x) for x in text.split(",")]
    key = append_to_size(key, len(l))
    # print(f"text: {text}\n key: {key}")
    byt =  bytes([a ^ b for (a, b) in zip(bytes(l), bytes(key, "UTF-8"))])
    return byt.decode('UTF-8')

if __name__ == "__main__":
    deafult_key = "key"
    if len(sys.argv) > 1:
        params = createParser().parse_args()
        text = params.message
        
        if params.e:
            print(encrypt(text, deafult_key))
        else:
            print(descrypt(text, deafult_key))
    else:
        exit(1)