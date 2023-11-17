# xfsm
* Library purpose:
  * fsm: generic finite state machine library processor; will "move" the item state from A to B (with proper error handling)
  * consistency and durability: every transtions will be stored in a relational database
  * concurrency: the library will garantee thread safety among elaboration sessions
  * competing consumer: in a multi process/thread condition, will guarantee that a specific item X will be processed by a single consumer at a time
  * data rolling: after a while (defined by configuration) items in a "final" state will be removed from the pile
* Library goals
  * design of a distributed competing consumer finite state machine
  * integration with the major relational database (sql server, oracle)
  * if efficient, an integration with a distributed memory cache like redis
  * porting on at least 2 main prog. languages (c#, java)
  * excellent error handling
  * states definitions up to the developer
  * states transitions will be defined by the business logic (at runtime) during the item elaboration
* Use cases
  * Generic sales report (xml document elaboration):
    * Subject: an xml document containing the employee selling results for the day before
    * Steps to execute:
      1. validate the document in order to catch syntax/semantic errors and also some business domain validations (ie. the user exists, etc..)
      1. save in database some document information (user, stock sold, amound, currency, ...)
      1. generate a pdf report available for the management
      1. delete the document
  * Industry 4.0 generic sensor event:
    * Subject: a sensor raised an event, the informations sent to our application must be managed
    * Steps to execute:
      1. analize data arrived from the sensor and take the right action (turn on the alarm, send an sms, etc...)
      1. archive event data for auditing purpose
      1. update some statistics based on event type
