```mermaid
---
title: Bag elements datamodel
---
erDiagram
    XfsmElement ||--|| XfsmBusinessElement : has
    XfsmElement {
        Id long PK
        InsertedTimestamp datetimeoffset
        LastUpdatedTimestamp datetimeoffset
        FetchedTimestamp datetimeoffset
        UniqueIndexState int
        FetchStatus short "todo, progress, done, error"
        Error string
    }
    XfsmBusinessElement {
        Id long PK
        XfsmElementId long FK
        JsonData string
    }
```
