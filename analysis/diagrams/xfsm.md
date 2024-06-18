```mermaid
---
title: XFSM classes diagram
---
classDiagram
    IXfsmDatabaseConnection <|.. XfsmDatabaseProvider : Realization
    XfsmDatabaseProvider <.. XfsmBag : Dependency
    XfsmPeekMode <.. XfsmBag : Dependency

    StateEnum <.. IXfsmElement : Association
    StateEnum <.. IXfsmState : Association

    XfsmBag <.. XfsmStateContext: Dependency
    IXfsmState <.. XfsmStateContext: Dependency
    IXfsmElement <.. XfsmStateContext: Dependency

    XfsmPeekStatus <.. IXfsmElement: Dependency

    XfsmStateContext <|.. XfsmStateContextFactory : Realization

    XfsmBag <.. XfsmProcessor : Dependency
    IXfsmState <.. XfsmProcessor : Dependency
    XfsmStateContextFactory <.. XfsmProcessor : Dependency
    XfsmBag <.. XfsmAppender : Dependency
    StateEnum <.. XfsmAppender : Dependency
    XfsmBag <.. XfsmDataRolling : Dependency

    %% note for XfsmDatabaseProvider "The Xfsm database provider is able\nto 'talk' with every supported database systems"
    class XfsmDatabaseProvider{
        <<abstract>>
        +XfsmDatabaseProvider(connectionString: string)
        +GetConnection() IXfsmDatabaseConnection
    }

    %% note for IXfsmDatabaseConnection "The Xfsm database connection manage\n the low level communication with the database systems"
    class IXfsmDatabaseConnection{
        <<interface>>
        +Query ~TKey~(sqlQuery: String) List ~TKey~
        +QueryFirst ~TKey~(sqlQuery: String) ~TKey~
        +Execute(sqlStatement: String) void
        +Commit() void
        +Dispose() void
    }

    %% note for XfsmBag "The XfsmBag is used to initialize\n the bag data structure, peek and add new items"
    class XfsmBag ~TKey~ {
        <<abstract>>
        +XfsmBag ~TKey~(databaseProvider: XfsmDatabaseProvider, peekMode: XfsmPeekMode)
        +EnsureInitialized() void
        +RetrieveDDLScript() string
        +GetPeekMode() XfsmPeekMode
        +Clear() void
        +Peek ~TKey~(state: StateEnum) IXfsmElement
        +AddElement(businessElement: TKey, elementState: StateEnum) long
        +Error(element: IXfsmElement, errorMessage: string)
        +Done(element: IXfsmElement)
    }

    class XfsmPeekMode {
       <<enumeration>>
       Queue,
       Stack
    }

    class XfsmPeekStatus {
        Todo = 0,
        Progress = 1,
        Done = 2,
        Error = 3
    }

    class StateEnum {
       <<enumeration>>
       defined_by_library_user
    }

    %% note for IXfsmElement "XfsmElement represents a single specific element of the items collections"
    class IXfsmElement ~TKey~ {
        <<interface>>
        +GetId() long
        +GetState() int
        +GetBusinessElement() TKey
        +GetInsertedTimestamp() Timestamp
        +GetLastUpdateTimestamp() Timestamp
        +GetPeekTimestamp() Timestamp
        +GetBusinessElement() TKey
        +GetPeekStatus() XfsmPeekStatus
        +GetError() string
    }

    note for IXfsmState "Represents a specific state of the FSM (this is the interface that will be implemented by the developer using this library)"
    class IXfsmState ~TKey~ {
        <<interface>>
        +Execute(businessElement: TKey) void
        +GetStateEnum() StateEnum
    }
    
    %% note for XfsmStateContext "Represents a specific state of the FSM (the interface which the user has to implement)"
    class XfsmStateContext ~TKey~ {
        +XfsmStateContext(xfsmBag: IXfsmBag~TKey~, state: IXfsmState~TKey~, element: IXfsmElement ~TKey~)
        +(internal)Execute() void
        +ChangeState(state: StateEnum) void
    }
    
    class XfsmStateContextFactory {
        +XfsmStateContextFactory(xfsmBag: IXfsmBag~TKey~, contextFactory: XfsmStateContextFactory, state: IXfsmState~TKey~)
        +Create ~TKey~(element: IXfsmElement ~TKey~) XfsmStateContext ~TKey~
    }

    class XfsmProcessor ~TKey~ {
        <<impl>>
        +XfsmProcessor(bag:  XfsmBag~TKey~)
        +WaitAndProcessElements(state: StateEnum, maximumElementToElaborate: int, maximumTimeOfElaboration: TimeSpan) void
    }

    class XfsmAppender ~TKey~ {
        <<impl>>
        +XfsmAppender(bag:  XfsmBag~TKey~)
        +Add(businessElement: TKey, elementState: StateEnum) void
    }

    class XfsmDataRolling ~TKey~ {
        <<impl>>
        +XfsmDataRolling(bag:  XfsmBag~TKey~)
        +ExecuteRolling() void
    }

```
