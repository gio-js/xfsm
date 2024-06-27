## IoT Sample
A temperature/humidity sensor sends his meaures to a server application who's adopting Xfsm library.

The server runs a processor on a thread specific for every status of the state machine.
In this case we want at least three statuses:
* analyze: in this state, the business logic, will analyze sensor measurement and take the proper actions (i.e. activate air conditionar of heating, etc...)
* store: another process thread will denormalize and store data in a flat table
* stats: the last thread will update statistics based on the stored data

Here a basic picture of the linear and simple process:

![alt text](https://github.com/gio-js/xfsm/blob/main/Xfsm-IOTSample.drawio.png?raw=true)
