CREATE OR REPLACE FUNCTION updateTipCount() RETURNS trigger AS '
BEGIN
  UPDATE Users
  SET TipCount = TipCount + 1
  WHERE OLD.UserID = NEW.UserID
  RETURN NEW;
END
' LANGUAGE plpgsql;

CREATE TRIGGER addTipCount
AFTER INSERT ON Tips
FOR EACH ROW
EXECUTE PROCEDURE updateTipCount();

CREATE OR REPLACE FUNCTION updatenumTips() RETURNS trigger AS '
BEGIN
  UPDATE Businesses
  SET NumTips = NumTips + 1
  WHERE OLD.BusinessID = NEW.BusinessID
  RETURN NEW;
END
' LANGUAGE plpgsql;

CREATE TRIGGER addNumTips
AFTER INSERT ON Tips
FOR EACH ROW
EXECUTE PROCEDURE updatenumTips();

CREATE OR REPLACE FUNCTION updatenumCheckins() RETURNS trigger AS '
BEGIN
  UPDATE Businesses
  SET NumCheckIns = NumCheckIns + 1
  WHERE OLD.BusinessID = NEW.BusinessID
  RETURN NEW;
END
' LANGUAGE plpgsql;

CREATE TRIGGER addNumCheckins
AFTER INSERT ON CheckIns
FOR EACH ROW
EXECUTE PROCEDURE updatenumCheckins();

CREATE OR REPLACE FUNCTION updatetotalLikes() RETURNS trigger AS '
BEGIN
  UPDATE Users
  SET TotalLikes = (SELECT SUM(likes) FROM Tips WHERE Users.UserID=Tips.UserID)
  WHERE OLD.UserID = NEW.UserID
  RETURN NEW;
END
' LANGUAGE plpgsql;

CREATE TRIGGER addtotalLikes
AFTER INSERT ON Tips
FOR EACH ROW
EXECUTE PROCEDURE updatenumCheckins();