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
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [userId] int,
  [weekOfOffices] int,
  [wholeWeek] bit,
  [obstacle] nvarchar(255)
)
GO

CREATE TABLE [trayHourOffice] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [userId] int,
  [weekOfOffice] int,
  [trayHour] nvarchar(255) NOT NULL CHECK ([trayHour] IN ('T8', 'T9', 'T10', 'T12', 'T13', 'T15', 'T17', 'T19', 'T20', 'T21'))
)
GO

CREATE TABLE [communionHourOffice] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [userId] int,
  [weekOfOffices] int,
  [communionHour] nvarchar(255) NOT NULL CHECK ([communionHour] IN ('K8', 'K9', 'K10', 'K12', 'K13', 'K15', 'K17', 'K19', 'K20', 'K21'))
)
GO

CREATE TABLE [offices] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [userId] int,
  [weekOfOffices] int,
  [officeName] nvarchar(255)
)
GO

CREATE TABLE [kitchenOffice] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [userId] int,
  [weekOfOffices] int,
  [saturdayOffices] nvarchar(255),
  [sundayOffices] nvarchar(255)
)
GO

CREATE TABLE [weeksNumber] (
  [id] int IDENTITY(1, 1),
  [weekNumber] int
)
GO

CREATE TABLE [obstacleConst] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [userId] int,
  [obstacleName] nvarchar(255)
)
GO

CREATE TABLE [obstacleBetweenOffices] (
  [id] int PRIMARY KEY IDENTITY(1, 1),
  [officeName] nvarchar(255),
  [officeConnected] nvarchar(255)
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

ALTER TABLE [weeksNumber] ADD FOREIGN KEY ([weekNumber]) REFERENCES [trayHourOffice] ([weekOfOffice])
GO

ALTER TABLE [weeksNumber] ADD FOREIGN KEY ([weekNumber]) REFERENCES [communionHourOffice] ([weekOfOffices])
GO

ALTER TABLE [weeksNumber] ADD FOREIGN KEY ([weekNumber]) REFERENCES [offices] ([weekOfOffices])
GO

ALTER TABLE [weeksNumber] ADD FOREIGN KEY ([weekNumber]) REFERENCES [obstacles] ([weekOfOffices])
GO

ALTER TABLE [weeksNumber] ADD FOREIGN KEY ([weekNumber]) REFERENCES [kitchenOffice] ([weekOfOffices])
GO

ALTER TABLE [brothers] ADD FOREIGN KEY ([id]) REFERENCES [obstacleConst] ([userId])
GO
