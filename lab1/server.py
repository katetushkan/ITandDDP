import socket
from typing import List

UDP_MAX_SIZE = 65507
HOST = '127.0.0.1'
PORT = 8000
START_KEY = '1251566712'
END_KEY = '766313162'


class User:
    def __init__(self, ip_address, port: int, name='st'):
        self.port = port
        self.ip_address = ip_address
        self.name = name
        self.messages = []
        self.connected = True

    def disconnect(self):
        self.connected = False

    def reconnect(self):
        self.connected = True

    def __str__(self):
        return self.name


class Server:
    users: List[User] = []
    socket = None

    @classmethod
    def connect(cls, host: str, port: int):
        cls.socket = socket.socket(
            socket.AF_INET,
            socket.SOCK_DGRAM
        )
        cls.socket.bind((host, port))

    @classmethod
    def start(cls):
        while True:
            try:
                msg, ip_address = cls.socket.recvfrom(UDP_MAX_SIZE)
                msg = msg.decode('utf-8')

                ips = [user.ip_address for user in cls.users]

                if ip_address not in ips:
                    name = msg.split(' ')[0]
                    user = User(ip_address=ip_address, port=ip_address[1], name=name)
                    cls.users.append(user)

                current_user = [user for user in cls.users if user.ip_address == ip_address][0]

                if not msg:
                    return

                if msg.endswith(END_KEY):
                    current_user.connected = False
                    print(f"Client {current_user} with port {current_user.port} has disconnected")
                    current_user.messages.append(msg.replace(END_KEY, f'{current_user}, hello again!'))
                    continue

                if msg.endswith(START_KEY):
                    print(f'User {current_user} from port {current_user.port} joined chat')
                    msg = msg.replace(START_KEY, '')
                    for user in cls.users:
                        if user == current_user and user.messages:
                            user.connected = True
                            message_to_send = ''.join(user.messages)
                            cls.socket.sendto(message_to_send.encode('utf-8'), user.ip_address)
                            continue
                    continue

                msg = f'{current_user}: {msg}'
                for user in cls.users:
                    if user == current_user or not user.connected:
                        continue
                    cls.socket.sendto(msg.encode('utf-8'), user.ip_address)

            except ConnectionResetError:
                return


Server.connect(HOST, PORT)
Server.start()
