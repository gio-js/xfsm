# xfsm
* Library purpose:
  * fsm: generic finite state machine library processor; will "move" the item state from A to B (with proper error handling)
  * concurrency: the library will garantee thread safety among elaboration sessions
  * competing consumer: in a multi process/thread condition, will guarantee that a specific item X will be processed by a single consumer at a time
* Library goals
  * design of a distributed competing consumer finite state machine
  * integration with the major relational database (sql server, oracle)
  * if efficient, an integration with a distributed memory cache like redis
  * porting on at least 2 main prog. languages (c#, java)
  * excellent error handling
  * states definitions up to the developer
  * states transitions will be defined by the business logic (at runtime) during the item elaboration

