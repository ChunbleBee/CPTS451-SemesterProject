CREATE OR REPLACE FUNCTION UpdateTipCount()
  RETURNS TRIGGER
  AS
  'BEGIN UPDATE Users SET TipCount = (select count(*) from Tips where userID = NEW.userID) WHERE users.UserID = NEW.UserID; RETURN NEW; END' LANGUAGE PLPGSQL;

CREATE TRIGGER AddTipCount
AFTER INSERT OR UPDATE
ON Tips
FOR EACH ROW
  EXECUTE PROCEDURE UpdateTipCount();

CREATE OR REPLACE FUNCTION UpdateNumTips()
  RETURNS TRIGGER
  AS
  'BEGIN UPDATE Businesses SET NumTips = (select count(*) from Tips where businessID = NEW.BusinessID) WHERE businesses.BusinessID = NEW.BusinessID; RETURN NEW; END;' LANGUAGE PLPGSQL;


CREATE TRIGGER AddNumTips
AFTER INSERT OR UPDATE
ON Tips
FOR EACH ROW
  EXECUTE PROCEDURE UpdateNumTips();

CREATE OR REPLACE FUNCTION UpdateNumCheckins()
  RETURNS TRIGGER
 AS
 'BEGIN UPDATE Businesses SET NumCheckIns = (select count(*) from checkins where businessID = new.businessID) WHERE businesses.BusinessID = NEW.BusinessID; RETURN NEW; END;' LANGUAGE PLPGSQL;


CREATE TRIGGER addNumCheckins
AFTER INSERT OR UPDATE
ON CheckIns
FOR EACH ROW
  EXECUTE PROCEDURE UpdateNumCheckins();

CREATE OR REPLACE FUNCTION UpdateTotalLikes()
  RETURNS TRIGGER
  AS
  'BEGIN UPDATE Users SET TotalLikes = (SELECT SUM(likes) FROM Tips WHERE Users.UserID=Tips.UserID) WHERE users.UserID = NEW.UserID; RETURN NEW; END;' LANGUAGE PLPGSQL;


CREATE TRIGGER AddTotalLikes
AFTER INSERT ON Tips
FOR EACH ROW
  EXECUTE PROCEDURE UpdateTotalLikes();