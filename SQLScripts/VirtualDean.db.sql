CREATE TABLE [brothers] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [Name] nvarchar(255),
  [Password] nvarchar(255),
  [Surname] nvarchar(255),
  [Precedency] datetime,
  [IsSinging] bit,
  [IsLector] bit,
  [IsAcolit] bit,
  [IsDiacon] bit
)
GO

CREATE TABLE [obstacles] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [BrotherId] int,
  [WeekOfOffices] int,
  [Obstacle] nvarchar(255)
)
GO

CREATE TABLE [trayHourOffice] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [BrotherId] int,
  [WeekOfOffices] int,
  [TrayHour] nvarchar(255)
)
GO

CREATE TABLE [communionHourOffice] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [BrotherId] int,
  [WeekOfOffices] int,
  [CommunionHour] nvarchar(255)
)
GO

CREATE TABLE [offices] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [BrotherId] int,
  [WeekOfOffices] int,
  [CantorOffice] nvarchar(255),
  [LiturgistOffice] nvarchar(255),
  [DeanOffice] nvarchar(255)
)
GO

CREATE TABLE [kitchenOffice] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [BrotherId] int,
  [WeekOfOffices] int,
  [SaturdayOffices] nvarchar(255),
  [SundayOffices] nvarchar(255)
)
GO

CREATE TABLE [weeksNumber] (
  [Id] int IDENTITY(1, 1),
  [WeekNumber] int PRIMARY KEY
)
GO

CREATE TABLE [obstacleConst] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [BrotherId] int,
  [ObstacleName] nvarchar(255)
)
GO

CREATE TABLE [obstacleBetweenOffices] (
  [Id] int PRIMARY KEY IDENTITY(1, 1),
  [OfficeName] nvarchar(255),
  [OfficeConnected] nvarchar(255)
)
GO

ALTER TABLE [obstacles] ADD FOREIGN KEY ([BrotherId]) REFERENCES [brothers] ([Id])
GO

ALTER TABLE [obstacles] ADD FOREIGN KEY ([WeekOfOffices]) REFERENCES [weeksNumber] ([WeekNumber])
GO

ALTER TABLE [trayHourOffice] ADD FOREIGN KEY ([BrotherId]) REFERENCES [brothers] ([Id])
GO

ALTER TABLE [trayHourOffice] ADD FOREIGN KEY ([WeekOfOffices]) REFERENCES [weeksNumber] ([WeekNumber])
GO

ALTER TABLE [communionHourOffice] ADD FOREIGN KEY ([BrotherId]) REFERENCES [brothers] ([Id])
GO

ALTER TABLE [communionHourOffice] ADD FOREIGN KEY ([WeekOfOffices]) REFERENCES [weeksNumber] ([WeekNumber])
GO

ALTER TABLE [offices] ADD FOREIGN KEY ([BrotherId]) REFERENCES [brothers] ([Id])
GO

ALTER TABLE [offices] ADD FOREIGN KEY ([WeekOfOffices]) REFERENCES [weeksNumber] ([WeekNumber])
GO

ALTER TABLE [kitchenOffice] ADD FOREIGN KEY ([BrotherId]) REFERENCES [brothers] ([Id])
GO

ALTER TABLE [kitchenOffice] ADD FOREIGN KEY ([WeekOfOffices]) REFERENCES [weeksNumber] ([WeekNumber])
GO

ALTER TABLE [obstacleConst] ADD FOREIGN KEY ([BrotherId]) REFERENCES [brothers] ([Id])
GO
