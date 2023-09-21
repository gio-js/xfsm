# xfsm
* Library purpose:
  * fsm: generic finite state machine library processor; will "move" the item state from A to B (with proper error handling)
  * consistency and durability: every transtions will be stored in a relational database
  * concurrency: the library will garantee thread safety among elaboration sessions
  * competing consumer: in a multi process/thread condition, will guarantee that a specific item X will be processed by a single consumer at a time
  * rollover: after a while (defined by configuration) items in a "final" state will be removed from the pile
* Library goals
  * design of a distributed competing consumer finite state machine
  * integration with the major relational database (sql server, oracle)
  * if efficient, an integration with a distributed memory cache like redis
  * porting on at least 2 main prog. languages (c#, java)
  * excellent error handling
  * states definitions up to the developer
  * states transitions will be defined by the business logic (at runtime) during the item elaboration

