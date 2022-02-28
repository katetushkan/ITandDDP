import socket
import threading
import datetime
import os
import json
from operator import itemgetter
from message import Message, MessageCodes
import time

clear_console = lambda: os.system('cls' if os.name in ('nt', 'dos') else 'clear')

def render_chat(history):
    clear_console()
    for i in sorted(history, key=itemgetter('time')):
        print(i['username'] + '>' + i['message'])


class Server:
    def __init__(self, port):
        self.serversocket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        self.serversocket.bind(('127.0.0.1', port))
        self.history = []
        self.current_connection = None
        self.message_received_time = None

    def listen(self):
        clear_console()
        while True:
            data,addr = self.serversocket.recvfrom(1024)
            if self.current_connection:
                if self.current_connection == addr:
                    self._add_message(data)
            else:
                self.current_connection = addr
                self._add_message(data)

    def _add_message(self, data):
        message= json.loads(data.decode('utf-8'))
        if message['type'] == MessageCodes.SEND.value:
            self.history.append(message)
            render_chat(self.history)
            message = Message('', '', message['time'], MessageCodes.SUCCESS.value)
            self.serversocket.sendto(str(message).encode('utf-8'), self.current_connection)
            return True
        elif message['type'] == MessageCodes.KILL_CONNECTION.value:
            self.exit_chat()
            return False
        elif message['type'] == MessageCodes.SUCCESS.value:
            self.message_received_time = message['time']
            return True

    def connect(self, host, port, username):
        while True:
            render_chat(self.history)
            m = input()
            message = Message(username, m, datetime.datetime.utcnow().timestamp())
            self.serversocket.sendto(str(message).encode('utf-8'),(host,port))
            while self.message_received_time != message.time:
                if (datetime.datetime.utcnow().timestamp() - message.time > 15):
                    self.exit_chat()
            self._add_message((str(message).encode('utf-8')))

    def exit_chat(self):
        print('leaving chat...')
        if self.current_connection:
            message = Message(username, 'left the chat',
                            datetime.datetime.utcnow().timestamp(),
                            type=MessageCodes.KILL_CONNECTION.value)
            self.serversocket.sendto(str(message).encode('utf-8'),
                                    (self.current_connection[0],
                                    self.current_connection[1]))
        exit()

if __name__ == '__main__':
    port = int(input('server port: '))
    username = input('username: ')
    receiver_port = int(input('receivers port: '))
    server = Server(port)
    server_thread = threading.Thread(target=server.listen)
    print(socket.gethostname())
    sending_thread = threading.Thread(target = server.connect, args=('127.0.0.1', receiver_port, username))
    server_thread.start()
    sending_thread.start()
