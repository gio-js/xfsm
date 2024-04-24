CREATE TABLE XfsmElement (
	Id BIGINT IDENTITY(1, 1),
	InsertedTimestamp DATETIMEOFFSET,
	UpdatedTimestamp DATETIMEOFFSET,
	FetchedTimestamp DATETIMEOFFSET,
	UniqueIndexState SMALLINT,
	FetchStatus TINYINT, -- 0: todo, 1: progress, 2: done, 3: error
	Error VARCHAR(256) NULL,
	CONSTRAINT PK_XfsmElement PRIMARY KEY CLUSTERED (Id)
)


CREATE TABLE XfsmBusinessElement (
	Id BIGINT IDENTITY(1, 1),
	XfsmElementId BIGINT,
	JsonData NVARCHAR(MAX) NULL,
	CONSTRAINT PK_XfsmBusinessElement PRIMARY KEY CLUSTERED (Id),
	CONSTRAINT FK_XfsmElementId FOREIGN KEY (XfsmElementId) REFERENCES XfsmElement(Id)
)

-- CREATE NONCLUSTERED INDEX IX_XfsmElement_InsertedTimestamp ON XfsmElement (InsertedTimestamp); -- todo: check whether it's usefull or not
CREATE NONCLUSTERED INDEX IX_XfsmBusinessElement_XfsmElementId ON XfsmBusinessElement (XfsmElementId);