import 'dart:io';
import 'dart:convert';

import 'package:uuid/uuid.dart';

void main() async {
  var portsHistory = <int, String>{};
  var msgHistory = <String, List<String>>{};
  var socket = await RawDatagramSocket.bind(
    InternetAddress.loopbackIPv4,
    8080,
  );
  print('${socket.address.host}:${socket.port}');

  socket.listen(
    (event) {
      if (event == RawSocketEvent.read) {
        var datagram = socket.receive()!;
        var port = datagram.port;
        var msg = utf8.decode(datagram.data);

        if (!portsHistory.containsKey(port)) {
          var uuid = Uuid().v1();
          msgHistory.addAll({uuid: []});
          portsHistory.addAll({port: uuid});
        }

        if (msg != '[init]') {
          msgHistory[portsHistory[port]]?.add(msg);
        } else if (msgHistory[portsHistory[port]]?.isNotEmpty ?? false) {
          print(
            'sent ${msgHistory[portsHistory[port]]?.length} messages to port $port',
          );
          socket.send(
            utf8.encode(msgHistory[portsHistory[port]]!.join('\n')),
            InternetAddress.loopbackIPv4,
            port,
          );
        }
        print('received from port $port: $msg');
      }
    },
  );
}
