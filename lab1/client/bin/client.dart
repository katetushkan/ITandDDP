import 'dart:convert';
import 'dart:io';

void main(List<String> args) async {
  var socket = await RawDatagramSocket.bind('localhost', 8084);

  socket.send(
    utf8.encode('[init]'),
    InternetAddress.loopbackIPv4,
    8080,
  );

  socket.listen(null);

  try {
    print(
      'got your history messages form server:\n${utf8.decode(socket.receive()!.data)}',
    );
  } catch (_) {}

  while (true) {
    var msg = stdin.readLineSync();
    socket.send(
      utf8.encode(msg!),
      InternetAddress.loopbackIPv4,
      8080,
    );
  }
}
