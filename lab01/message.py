import json
import enum

class MessageCodes(enum.Enum):
    SEND = 1
    KILL_CONNECTION = 2
    SUCCESS = 3

class Message:
    def __init__(self, username, message, time, type=MessageCodes.SEND.value):
        self.username = username
        self.message = message
        self.time = time
        self.type = type

    def __str__(self):
        return json.dumps({'username':self.username, 'message':self.message, 'time':self.time, 'type':int(self.type)})
