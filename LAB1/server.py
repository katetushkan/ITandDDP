import socket

class Server():

    def __init__(self, hostname, host, port, header, format):
        self.hostname   = hostname
        self.host       = host
        self.port       = port
        self.header     = header
        self.format     = format
        self.addr       = (self.host, self.port)

    def start(self):
        self.tcp_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.udp_socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        self.tcp_socket.bind(self.addr)
        self.udp_socket.bind(self.addr)

        print(f"[{self.hostname}] Now listening on {self.host}:{self.port}")

        self.tcp_socket.listen()
        conn, addr = self.tcp_socket.accept()
        with conn:
            print(f"[{self.hostname}] User connected: {addr}")
            data = conn.recv(self.header)
            print(f"[{self.hostname}] {data.decode(self.format)}")
            conn.sendall(f"[{self.hostname}] Hello User! (tcp)".encode(self.format))

        i = 0
        msg_dict = dict()
        while True:
            data, client_addr = self.udp_socket.recvfrom(self.header)
            data = data.decode(self.format)
            data_i = int(data[data.rfind('[') + 1:data.rfind(']')])
            data = data[data.rfind(']') + 1:]
            response_msg = f"[{self.hostname}]: Received your message".encode(self.format)
            self.udp_socket.sendto(response_msg, client_addr)
            if i == data_i:
                print(f"[{self.hostname}][Received] {data}")
                i += 1

                while True:
                    if i in msg_dict:
                        print(f"[{self.hostname}][Received] {msg_dict[i]}")
                        del msg_dict[i]
                        i += 1
                    else: break
            else:
                msg_dict[data_i] = data



if __name__ == "__main__":
    server_1 = Server(
        "Server 1", 
        socket.gethostbyname(socket.gethostname()), 
        5051, 
        1024, 
        "utf-8")
    try:
        print(f"[INFO] Starting {server_1.hostname} ...")
        server_1.start()
    except Exception as e:
        print("[EXCEPTION] Something wrong happend: " + e)
