/*
  Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Ik1hc3RlciIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvdXNlcmRhdGEiOiJEZXZlbG9wbWVudCIsIm5iZiI6MTY0MTQxMjkzMywiZXhwIjoyMjcyNTY0OTMzLCJpYXQiOjE2NDE0MTI5MzMsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjUwMDEifQ.zGpiOqCd_Fiex3MbJM89cKR-oVttTvvVhfRxx25aSDsfr1AFDy_HkrYcxyCV20CoSl7EN31tUnd1evUpjEJLog
*/
CREATE DATABASE CascadeDelete
GO
USE CascadeDelete
GO
CREATE TABLE ChildTable
(
	IdChild int NOT NULL,
	IdParent int NOT NULL,
	Name varchar(200) NOT NULL,
	CONSTRAINT PK_ChildTable PRIMARY KEY CLUSTERED 
	(
		IdChild ASC
	)
)
GO
CREATE TABLE GrandChildTable
(
	IdGrandChild int NOT NULL,
	IdChild int NOT NULL,
	Name varchar(200) NOT NULL,
	CONSTRAINT PK_GrandChildTable PRIMARY KEY CLUSTERED 
	(
		IdGrandChild ASC
	)
)
GO
CREATE TABLE ParentTable
(
	IdParent int NOT NULL,
	Name varchar(200) NOT NULL,
	CONSTRAINT PK_ParentTable PRIMARY KEY CLUSTERED 
	(
		IdParent ASC
	)
)
GO
ALTER TABLE ChildTable  WITH CHECK ADD  CONSTRAINT FK_ChildTable_ParentTable FOREIGN KEY(IdParent) REFERENCES ParentTable (IdParent)
GO
ALTER TABLE ChildTable CHECK CONSTRAINT FK_ChildTable_ParentTable
GO
ALTER TABLE GrandChildTable  WITH CHECK ADD  CONSTRAINT FK_GrandChildTable_ChildTable FOREIGN KEY(IdChild) REFERENCES ChildTable (IdChild)
GO
ALTER TABLE GrandChildTable CHECK CONSTRAINT FK_GrandChildTable_ChildTable
GO