import socket

HEADER = 1024
FORMAT = "utf-8"
EXIT_COMM = "!EXIT"

class Client():

    def __init__(self, username, host, port, header, format):
        self.username   = username
        self.host       = host
        self.port       = port
        self.header     = header
        self.format     = format
        self.host_addr  = (self.host, self.port)

        self.tcp_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.udp_socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

    def send(self, msg: str) -> str:
        encoded_msg = msg.encode(self.format)
        self.udp_socket.sendto(encoded_msg, self.host_addr)
        try:
            response, _ = self.udp_socket.recvfrom(self.header)
        except:
            return "[INFO] Timeout! Server didn't respond."

        return response.decode(self.format)


if __name__ == "__main__":
    username    = input("Username: ")
    host        = socket.gethostbyname(socket.gethostname())
    port        = int(input("Port: "))

    client_1 = Client(username, host, port, HEADER, FORMAT)
    client_1.udp_socket.settimeout(3)
    client_1.tcp_socket.settimeout(3)
    try:
        client_1.tcp_socket.connect(client_1.host_addr)
        client_1.tcp_socket.sendall("Hello server! (tcp)".encode(client_1.format))
        response = client_1.tcp_socket.recv(client_1.header)
        print(response.decode(client_1.format))
        client_1.tcp_socket.close()

        print(f"\nType here ... To exit type: {EXIT_COMM}")
        i = 0;
        while True:
            msg = input(f"[{client_1.username}]: ")
            if msg == EXIT_COMM:
                break
            msg = f'[{i}]' + msg 
            response = client_1.send(msg)
            print(response)

            i += 1
    except Exception as e:
        print(f"[EXCEPTION] {e}")
    finally: 
        print("Bye!")
