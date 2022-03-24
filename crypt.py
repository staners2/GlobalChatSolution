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
    l = [a ^ b for (a, b) in zip(bytes(text, 'utf-8'), bytes(key, 'utf-8'))] # 16 разрядов
    return ",".join(str(x) for x in l)


def descrypt(text, key):
    l = [int(x) for x in text.split(",")]
    return bytes([a ^ b for (a, b) in zip(bytes(l), bytes(key, 'utf-8'))]).decode()

if __name__ == "__main__":
    deafult_key = "key"
    if len(sys.argv) > 1:
        params = createParser().parse_args()
        text = params.message
        key = append_to_size(deafult_key, len(text))
        if params.e:
            print(encrypt(text, key))
        else:
            print(descrypt(text, key))
    else:
        exit(1)