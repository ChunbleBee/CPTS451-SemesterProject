import json
import psycopg2
import pandas as pd


def cleanStr4SQL(s):
    return s.replace("'", "`").replace("\n", " ")


def int2BoolStr(value):
    if value == 0:
        return 'False'
    else:
        return 'True'

users = pd.read_json('./Project/YelpData/yelp_user.JSON', lines=True)
business = pd.read_json('./Project/YelpData/yelp_business.JSON', lines=True)


def insert2BusinessTable(lines):
    insertString = "INSERT INTO business (businessID, businessname, state, city, isOpen, reviewCount, starrating, numCheckins, numTips, categories)"
    valString = " VALUES ( "
    try:
        conn = psycopg2.connect("dbname='milestone2' user='postgres' host='localhost' password='th@darncat8'")
    except Exception as e:
        print('Unable to connect to the database!')
    cur = conn.cursor()
    for i in range(len(lines)):
        temp = valString
        temp += "'" + lines.business_id[i] + "', '" + lines.name[i] + "', '" + lines.state[i] + "', '" + lines.city[i] + "', " + \
                str([True, False][lines.is_open[i]]) + ", " + str(lines.review_count[i]) + ", " + str(lines.stars[i]) + ", 0, 0, NULL )"
        try:
            fullString = insertString+temp
            print(fullString)
            cur.execute(fullString)
        except Exception as e:
            print("Insert to businessTABLE failed!", e)
        conn.commit()
    cur.close()
    conn.close()


def insert2AddressesTable(lines):
    insertString = "INSERT INTO addresses (businessID, longitude, latitude, street, city, usstate)"
    valString = " VALUES ( "
    try:
        conn = psycopg2.connect("dbname='milestone2' user='postgres' host='localhost' password='th@darncat8'")
    except Exception as e:
        print('Unable to connect to the database!')
    cur = conn.cursor()
    for i in range(len(lines)):
        temp = valString
        temp += "'" + lines.business_id[i] + "', " + str(lines.longitude[i]) + ", " + str(lines.latitude[i]) + ", '" + lines.address[i] + \
                "', '" + lines.city[i] + "', '" + lines.state[i] + "' )"
        try:
            fullString = insertString+temp
            cur.execute(fullString)
        except Exception as e:
            print("Insert to businessTABLE failed!", e)
        conn.commit()
    cur.close()
    conn.close()


def insert2BusinessHoursTable(lines):
    insertString = "INSERT INTO businesshours (businessID, days, openingtimes, closingtimes)"
    valString = " VALUES ( "
    try:
        conn = psycopg2.connect("dbname='milestone2' user='postgres' host='localhost' password='th@darncat8'")
    except Exception as e:
        print('Unable to connect to the database!')
    cur = conn.cursor()
    for i in range(len(lines)):
        temp = valString
        hours = lines.hours[i]
        days = "( '"
        open = "( '"
        close = "( '"
        for day in hours:
            days += day + "', '"
            tempHour = hours[day]
            tHours = tempHour.split('-')
            open += tHours[0] + "', '"
            close += tHours[1] + "', '"
        days = days.removesuffix(", '") + " )"
        open = open.removesuffix(", '") + " )"
        close = close.removesuffix(", '") + " )"
        temp += "'" + lines.business_id[i] + "', " + days + ", " + open + ", " + close + " )"
        try:
            fullString = insertString+temp
            print(fullString)
            cur.execute(fullString)
        except Exception as e:
            print("Insert to businessTABLE failed!", e)
        conn.commit()
    cur.close()
    conn.close()


def insert2UsersTable(lines):
    insertString = "INSERT INTO users (userID, creationDate, username, totalLikes, tipCount, fansRating, " \
                   "funnyRating, coolRating, avgStarRating, latitude, longitude)"
    valString = " VALUES ( "
    try:
        conn = psycopg2.connect("dbname='milestone2' user='postgres' host='localhost' password='th@darncat8'")
    except Exception as e:
        print('Unable to connect to the database!')
    cur = conn.cursor()
    for i in range(len(lines)):
        temp = valString
        temp += "'" + lines.user_id[i] + "', " + lines.yelping_since[i] + ", '" + lines.name[i] + "', " + \
                "0, " + str(lines.tipcount[i]) + ", " + str(lines.fans[i]) + ", " + str(lines.funny[i]) + \
                ", " + str(lines.cool[i]) + ", " + str(lines.average_stars[i]) + ", " + "NULL, NULL )"
        try:
            cur.execute(insertString+temp)
        except Exception as e:
            print("Insert to businessTABLE failed!", e)
        conn.commit()
    cur.close()
    conn.close()



if __name__ == "__main__":
    # insert2BusinessTable(business)
    # insert2UsersTable(users)
    # insert2AddressesTable(business)
    insert2BusinessHoursTable(business)

