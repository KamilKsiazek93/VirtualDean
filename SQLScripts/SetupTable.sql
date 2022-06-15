/*
 * Setup week number values
 */

INSERT INTO dbo.weeksNumber (WeekNumber) VALUES (1)

/*
 * Setup hours values
 */

INSERT INTO dbo.hours (Hour) VALUES ('8.00')
INSERT INTO dbo.hours (Hour) VALUES ('9.00')
INSERT INTO dbo.hours (Hour) VALUES ('10.30')
INSERT INTO dbo.hours (Hour) VALUES ('12.00')
INSERT INTO dbo.hours (Hour) VALUES ('13.30')
INSERT INTO dbo.hours (Hour) VALUES ('15.30')
INSERT INTO dbo.hours (Hour) VALUES ('17.00')
INSERT INTO dbo.hours (Hour) VALUES ('19.00')
INSERT INTO dbo.hours (Hour) VALUES ('20.20')
INSERT INTO dbo.hours (Hour) VALUES ('21.30')

/*
 * Setup pipeline values
 */

INSERT INTO dbo.pipelineStatus (Name, PipelineValue) VALUES ('KITCHEN', 1)
INSERT INTO dbo.pipelineStatus (Name, PipelineValue) VALUES ('CANTOR', 0)
INSERT INTO dbo.pipelineStatus (Name, PipelineValue) VALUES ('TRAY', 0)
INSERT INTO dbo.pipelineStatus (Name, PipelineValue) VALUES ('LITURGIST', 0)
INSERT INTO dbo.pipelineStatus (Name, PipelineValue) VALUES ('DEAN', 0)

/*
 * Setup office name values
 */

INSERT INTO officeNames (OfficeName, OfficeAdmin) VALUES ('S', 'CANTOR')
INSERT INTO officeNames (OfficeName, OfficeAdmin) VALUES ('PS', 'CANTOR')
INSERT INTO officeNames (OfficeName, OfficeAdmin) VALUES ('MO', 'LITURGIST')
INSERT INTO officeNames (OfficeName, OfficeAdmin) VALUES ('MK', 'LITURGIST')
INSERT INTO officeNames (OfficeName, OfficeAdmin) VALUES ('MŚ', 'LITURGIST')
INSERT INTO officeNames (OfficeName, OfficeAdmin) VALUES ('TUR', 'LITURGIST')
INSERT INTO officeNames (OfficeName, OfficeAdmin) VALUES ('KR', 'LITURGIST')
INSERT INTO officeNames (OfficeName, OfficeAdmin) VALUES ('SUC1', 'LITURGIST')
INSERT INTO officeNames (OfficeName, OfficeAdmin) VALUES ('SUC2', 'LITURGIST')
INSERT INTO officeNames (OfficeName, OfficeAdmin) VALUES ('RESP1', 'LITURGIST')
INSERT INTO officeNames (OfficeName, OfficeAdmin) VALUES ('RESP2', 'LITURGIST')
INSERT INTO officeNames (OfficeName, OfficeAdmin) VALUES ('ANT', 'LITURGIST')
INSERT INTO officeNames (OfficeName, OfficeAdmin) VALUES ('PR', 'DEAN')
INSERT INTO officeNames (OfficeName, OfficeAdmin) VALUES ('SR', 'DEAN')
INSERT INTO officeNames (OfficeName, OfficeAdmin) VALUES ('KO', 'DEAN')
INSERT INTO officeNames (OfficeName, OfficeAdmin) VALUES ('Oa', 'KITCHEN')
INSERT INTO officeNames (OfficeName, OfficeAdmin) VALUES ('Ob', 'KITCHEN')
INSERT INTO officeNames (OfficeName, OfficeAdmin) VALUES ('W', 'KITCHEN')
INSERT INTO officeNames (OfficeName, OfficeAdmin) VALUES ('Zm', 'KITCHEN')
INSERT INTO officeNames (OfficeName, OfficeAdmin) VALUES ('Zw', 'KITCHEN')