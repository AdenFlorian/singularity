notes
=====

i want other player characers on my screen to be moving around based off of their position on their owners client
i want my character to only move in response to my commands, and to send its position to all other clients

NetworkController
    A script that controls based off of network input
PlayerController
    A script that controls based on player input

What to call a script that send it's gameobjects position to the server?

NetworkSender?

Network IDs
-----------

What needs a network ID (netid)?

anything that wants to send or recieve info over the network?

Who generates netids?
Server?
    To make sure they stay unique
Client?
    Client can generate a guid, and server can enforce that it is unique

UDP Ports
---------

Server port will stay the same, but clients ports can't be guaranteed
Client should "connect" to server to let server know the client's udp port
Or we could use a websocket, no punchthru required