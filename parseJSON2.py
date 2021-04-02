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


def insert2BusinessTable():
    lines = pd.read_json('./Project/YelpData/yelp_business.JSON', lines=True)
    insertString = "INSERT INTO business (businessID, businessname, state, city, isOpen, reviewCount, starrating, numCheckins, numTips, categories)"
    valString = " VALUE ( "
    try:
        conn = psycopg2.connect("dbname='milestone2' user='postgres' host='localhost' password='th@darncat8'")
    except Exception as e:
        print('Unable to connect to the database!')
    cur = conn.cursor()
    for i in range(len(lines)):
        temp = valString
        temp += "'" + lines.business_id[i] + "', '" + lines.name[i] + "', '" + lines.state[i] + "', " + \
                str([True, False][lines.is_open[i]]) + ", " + str(lines.review_count[i]) + ", " + str(lines.stars[i]) + ", 0, 0, NULL )"
        try:
            cur.execute(insertString+temp)
        except Exception as e:
            print("Insert to businessTABLE failed!", e)
        conn.commit()
    cur.close()
    conn.close()


def insert2UsersTable():
    lines = pd.read_json('./Project/YelpData/yelp_user.JSON', lines=True)
    insertString = "INSERT INTO business (userID, creationDate, username, totalLikes, tipCount, fansRating, " \
                   "funnyRating, coolRating, avgStarRating, latitude, longitude)"
    valString = " VALUE ( "
    try:
        conn = psycopg2.connect("dbname='milestone2' user='postgres' host='localhost' password='th@darncat8'")
    except Exception as e:
        print('Unable to connect to the database!')
    cur = conn.cursor()
    for i in range(len(lines)):
        temp = valString
        # temp += "'" + lines.business_id[i] + "', '" + lines.name[i] + "', '" + lines.state[i] + "', " + \
                str([True, False][lines.is_open[i]]) + ", " + str(lines.review_count[i]) + ", " + str(lines.stars[i]) + ", 0, 0, NULL )"
        try:
            cur.execute(insertString+temp)
        except Exception as e:
            print("Insert to businessTABLE failed!", e)
        conn.commit()
    cur.close()
    conn.close()



if __name__ == "__main__":
    # insert2BusinessTable()
    insert2UsersTable()


