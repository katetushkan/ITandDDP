import socket
import argparse

parser = argparse.ArgumentParser(description='client')
parser.add_argument('-p', '--port', dest='port', type=int, default=5000, help='simulates different clients')
args = parser.parse_args()

HOST = 'localhost'
PORT = 8000
BUFFER = 1024


def run_client():
    try:
        with socket.socket(socket.AF_INET, socket.SOCK_DGRAM) as udp_client:
            udp_client.bind((HOST, args.port))
            print(f'messages are sent from port {args.port}')
            while True:
                message = input()
                message = message.encode('utf-8')
                udp_client.sendto(message, (HOST, PORT))
                data, address = udp_client.recvfrom(BUFFER)
                data = data.decode('utf-8')
                print(f'Server: {data}')
    except KeyboardInterrupt:
        print('client disabled')


if __name__ == '__main__':
    run_client()
