import socket
import threading

UDP_MAX_SIZE = 65507
HOST = '127.0.0.1'
PORT = 8000
START_KEY = '1251566712'
END_KEY = '766313162'


class Client:
    def __init__(self, name="bob"):
        self.socket = socket.socket(
            socket.AF_INET,
            socket.SOCK_DGRAM
        )
        self.name = name.strip().replace(' ', '_')
        self.port_name = 6533
        self.socket.bind((HOST, self.port_name))
        self.messages = ''

    def _clear_console(self):
        print('\n' * 150)

    def stop_listen(self):
        self.messages += END_KEY
        self.socket.send(self.messages.encode('utf-8'))

    def start(self):
        try:
            self.socket.connect((HOST, PORT))
            socket_thread = threading.Thread(target=self.listen, daemon=True)
            socket_thread.start()

            self.socket.send(f'{self.name} {START_KEY}'.encode('utf-8'))

            while True:
                print('you: ', end='')
                msg = input()

                if msg.strip().lower() == 'exit':
                    self.stop_listen()
                    break

                self.messages += 'you: ' + msg + '\n'
                self.socket.send(msg.encode('utf-8'))
        except Exception:
            self.stop_listen()

    def listen(self):
        while True:

            try:
                msg = self.socket.recv(UDP_MAX_SIZE)
                msg = msg.decode()
                self.messages += msg + '\n'
                self._clear_console()
                print(self.messages + '\nyou: ', end='')


            except ConnectionResetError:
                print("error")


print('telegram chat')
name = input("Your name?\n")
client = Client(name=name)
client.start()
