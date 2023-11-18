Trying to imagine what a library user must define or configure in order to use the library.

He must define the fsm states. That's not a simple declaration of it, he has to implement:
* what should I do in this current state
* what's next? next status, untill it's not on a final status
* in the state implementation use the right exception pattern to facilitate a gracefull exit or retry
* create a basic thread/process that runs the xfsm manager
