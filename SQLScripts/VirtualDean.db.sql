CREATE TABLE [brothers] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [name] nvarchar(255),
  [surname] nvarchar(255),
  [precedency] datetime,
  [isSinging] bit,
  [isLector] bit,
  [isAcolit] bit,
  [isDiacon] bit
)
GO

CREATE TABLE [obstacles] (
  [id] int IDENTITY(1, 1),
  [userId] int PRIMARY KEY,
  [weekOfOffices] int,
  [dateOfObstacles] datetime,
  [obstacle] nvarchar(255)
)
GO

CREATE TABLE [trayHourOffice] (
  [id] int IDENTITY(1, 1),
  [userId] int PRIMARY KEY,
  [weekOfOffice] int,
  [trayHour] nvarchar(255) NOT NULL CHECK ([trayHour] IN ('T8', 'T9', 'T10', 'T12', 'T13', 'T15', 'T17', 'T19', 'T20', 'T21'))
)
GO

CREATE TABLE [communionHourOffice] (
  [id] int IDENTITY(1, 1),
  [userId] int PRIMARY KEY,
  [weekOfOffices] int,
  [communionHour] nvarchar(255) NOT NULL CHECK ([communionHour] IN ('K8', 'K9', 'K10', 'K12', 'K13', 'K15', 'K17', 'K19', 'K20', 'K21'))
)
GO

CREATE TABLE [offices] (
  [id] int IDENTITY(1, 1),
  [userId] int PRIMARY KEY,
  [weekOfOffices] int,
  [officeName] nvarchar(255)
)
GO

CREATE TABLE [kitchenOffice] (
  [id] int IDENTITY(1, 1),
  [userId] int PRIMARY KEY,
  [weekOfOffices] int,
  [saturdayOffices] nvarchar(255),
  [sundayOffices] nvarchar(255)
)
GO

ALTER TABLE [brothers] ADD FOREIGN KEY ([id]) REFERENCES [obstacles] ([userId])
GO

ALTER TABLE [brothers] ADD FOREIGN KEY ([id]) REFERENCES [communionHourOffice] ([userId])
GO

ALTER TABLE [brothers] ADD FOREIGN KEY ([id]) REFERENCES [trayHourOffice] ([userId])
GO

ALTER TABLE [brothers] ADD FOREIGN KEY ([id]) REFERENCES [offices] ([userId])
GO

ALTER TABLE [brothers] ADD FOREIGN KEY ([id]) REFERENCES [kitchenOffice] ([userId])
GO
