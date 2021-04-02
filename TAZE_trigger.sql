CREATE OR REPLACE FUNCTION updateTipCount() RETURNS trigger AS '
BEGIN
  UPDATE users
  SET tipCount = tipCount + 1
  WHERE OLD.userID = NEW.userID
  RETURN NEW;
END
' LANGUAGE plpgsql;

CREATE TRIGGER addTipCount
AFTER INSERT ON Tips
FOR EACH ROW
EXECUTE PROCEDURE updateTipCount();

CREATE OR REPLACE FUNCTION updatenumTips() RETURNS trigger AS '
BEGIN
  UPDATE business
  SET numTips = numTips + 1
  WHERE OLD.businessID = NEW.businessID
  RETURN NEW;
END
' LANGUAGE plpgsql;

CREATE TRIGGER addNumTips
AFTER INSERT ON Tips
FOR EACH ROW
EXECUTE PROCEDURE updatenumTips();

CREATE OR REPLACE FUNCTION updatenumCheckins() RETURNS trigger AS '
BEGIN
  UPDATE users
  SET numCheckins = numCheckins + 1
  WHERE OLD.businessID = NEW.businessID
  RETURN NEW;
END
' LANGUAGE plpgsql;

CREATE TRIGGER addNumCheckins
AFTER INSERT ON CheckIns
FOR EACH ROW
EXECUTE PROCEDURE updatenumCheckins();

CREATE OR REPLACE FUNCTION updatetotalLikes() RETURNS trigger AS '
BEGIN
  UPDATE users
  SET totalLikes = (SELECT SUM(likes) FROM Tips WHERE users.userID=TiPs.UserID)
  WHERE OLD.userID = NEW.userID
  RETURN NEW;
END
' LANGUAGE plpgsql;

CREATE TRIGGER addtotalLikes
AFTER INSERT ON Tips
FOR EACH ROW
EXECUTE PROCEDURE updatenumCheckins();