/* User Table */
CREATE TABLE Users
(
    UserID          TEXT NOT NULL,
    CreationDate    DATE,
    UserName        TEXT,
    FansRating      INTEGER,
    FunnyRating     INTEGER,
    CoolRating      INTEGER,
    AvgStarRating   DECIMAL (2, 1),
    PRIMARY KEY (UserID)
)

/* Businesses Table */
CREATE TABLE Businesses
(
    BusinessID      TEXT NOT NULL,
    BusinessName    TEXT,
    IsOpen          BOOLEAN,
    ReviewCount     INTEGER,
    StarRating      DECIMAL (2, 1),
    Categories      TEXT [],
    BusinessAddress FOREIGN KEY REFERENCES Addresses(BusinessAddress)
    PRIMARY KEY (BusinessID)
)

/**/
CREATE TABLE Addresses
(
    Longitude   DECIMAL (9, 6),
    Latitude    DECIMAL (8, 6),
)

/* Reviews Table */
CREATE TABLE Reviews
(
    UserID          FOREIGN KEY REFERENCES Users(UserID),
    BusinessID      FOREIGN KEY REFERENCES Businesses(BusinessID),
    ReviewTest      TEXT,
    Likes           INTEGER,
    CreationDate    DATE,
    PRIMARY KEY (UserID, BusinessID)
)

/* Check Ins Table */
CREATE TABLE CheckIns
(
    BusinessID  FOREIGN KEY REFERENCES Businesses(BusinessID),
    CheckInDate DATE,
    PRIMARY KEY (BusinessID)
)

/* Link table for creating friends */
CREATE TABLE Friends
(
    User01 FOREIGN KEY REFERENCES Users(User01)
    User02 FOREIGN KEY REFERENCES Users(User02)
    PRIMARY KEY (User01, User02)
)