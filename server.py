import os.path
import pickle
import socket
import argparse
import datetime


parser = argparse.ArgumentParser(description='server')
parser.add_argument('--host', dest='host', type=str, default='localhost', help='ip address or host name')
parser.add_argument('-p', '--port', dest='port', type=int,  default='8000', help='port(integer from 1024 to 65535')
args = parser.parse_args()

HOST, PORT = args.host, args.port
BUFFER = 1024
db = {}


if os.path.exists('db.txt'):
    with open('db.txt', 'rb') as f:
        db = pickle.load(f)


def add_message(port, message):
    if port not in db.keys():
        db[port] = [message]
    else:
        db[port].append(message)


def get_user_messages(port):
    return 'you have no messages' if port not in db.keys() else '\n'.join(db[port])


def run_server():
    try:
        with socket.socket(socket.AF_INET, socket.SOCK_DGRAM) as udp_server:
            udp_server.bind((HOST, PORT))
            print(f'server {HOST} running on port {PORT}')
            while True:
                message, address = udp_server.recvfrom(BUFFER)
                message = message.decode("utf-8")
                response = '\n' + get_user_messages(address[1]) if message == 'history' else '✅'
                message = f'{datetime.datetime.now()}->{message}'
                add_message(address[1], message)
                print(message, f'from {address}')
                udp_server.sendto(str.encode(response), address)
    except KeyboardInterrupt:
        with open("db.txt", 'wb') as f:
            pickle.dump(db, f)
        print('server stopped')


if __name__ == '__main__':
    run_server()

