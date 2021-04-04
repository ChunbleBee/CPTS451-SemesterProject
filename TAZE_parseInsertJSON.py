import json
import psycopg2

mylist = []

def cleanStr4SQL(s):
    return s.replace("'","`").replace("\n"," ")

def int2BoolStr (value):
    if value == 0:
        return 'False'
    else:
        return 'True'

def insert2BusinessTable():
    #reading the JSON file
    with open('./YelpData/yelp_business.JSON','r') as f:    #TODO: update path for the input file
        #outfile =  open('./INSERTdata/yelp_business.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        #TODO: update the database name, username, and password
        try:
            conn = psycopg2.connect("dbname='milestone2db' user='postgres' host='localhost' password=';'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            # Generate the INSERT statement for the cussent business
            # TODO: The below INSERT statement is based on a simple (and incomplete) businesstable schema. Update the statement based on your own table schema and
            # include values for all businessTable attributes
            businessID = data['business_id']
            sql_str_businessTable = "INSERT INTO Business (businessID, businessName, Address, State, City, Zip, latitude, longitude, StarRating, ReviewCount, isOpen, numCheckins, numTips) " \
                      "VALUES ('" + businessID + "','" + cleanStr4SQL(data["name"]) + "','" + cleanStr4SQL(data["address"]) + "','" + \
                      cleanStr4SQL(data["state"]) + "','" + cleanStr4SQL(data["city"]) + "','" + data["postal_code"] + "'," + str(data["latitude"]) + "," + \
                      str(data["longitude"]) + "," + str(data["stars"]) + "," + str(data["review_count"]) + "," + str(int2BoolStr(data["is_open"])) + ", 0, 0" + ");"
            try:
                cur.execute(sql_str_businessTable)
            except Exception as e:
                print(e)
                print("Insert to BusinessTable failed!")
            conn.commit()

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()

    print(count_line)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()

def insert2CategoryTable():
    #reading the JSON file
    with open('./YelpData/yelp_business.JSON','r') as f:    #TODO: update path for the input file
        #outfile =  open('./INSERTdata/yelp_category.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        #TODO: update the database name, username, and password
        try:
            conn = psycopg2.connect("dbname='milestone2db' user='postgres' host='localhost' password=';'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            # Generate the INSERT statement for the cussent business
            # TODO: The below INSERT statement is based on a simple (and incomplete) businesstable schema. Update the statement based on your own table schema and
            # include values for all Category attributes
            businessID = str(data['business_id'])
            categories = data["categories"].split(', ')
            Categories = [item for item in categories]

            for cat_name in Categories:
                sql_str_categoryTable = "INSERT INTO BusinessCategories (businessID, catName) " \
                        "VALUES ('" + businessID + "','" + cleanStr4SQL(cat_name) +  "'" + ");"
                try:
                    cur.execute(sql_str_categoryTable)
                except Exception as e:
                    print(e)
                    print("Insert to CategoryTable failed!")
                conn.commit()

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()

    print(count_line)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()

def insert2UserTable():
    #reading the JSON file
    with open('./YelpData/yelp_user.JSON','r') as f:    #TODO: update path for the input file
            #outfile =  open('./INSERTdata/yelp_user.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
            line = f.readline()
            count_line = 0

            #connect to yelpdb database on postgres server using psycopg2
            #TODO: update the database name, username, and password
            try:
                conn = psycopg2.connect("dbname='milestone2db' user='postgres' host='localhost' password=';'")
            except:
                print('Unable to connect to the database!')
            cur = conn.cursor()

            while line:
                data = json.loads(line)
                # Generate the INSERT statement for the cussent business
                # TODO: The below INSERT statement is based on a simple (and incomplete) businesstable schema. Update the statement based on your own table schema and
                # include values for all businessTable attributes
                sql_str_userTable = "INSERT INTO Users (userID, creationDate, userName, fansRating, funnyRating, coolRating, avgStarRating) " \
                          "VALUES ('" + data['user_id'] + "','" + cleanStr4SQL(data["yelping_since"]) + "','" + cleanStr4SQL(data["name"]) + "','" + \
                            str(data["fans"]) + "','" + str(data["funny"]) + "','" + str(data["cool"]) + "','" + str(data["average_stars"]) + "');"
                try:
                    cur.execute(sql_str_userTable)
                except Exception as e:
                    print(e)
                    print("Insert to UserTable failed!")
                conn.commit()


                line = f.readline()
                count_line +=1

            cur.close()
            conn.close()

    print(count_line)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()

def insert2CheckinTable():
    #reading the JSON file
    with open('./yelp_checkin.JSON','r') as f:    #TODO: update path for the input file
        outfile =  open('./InsertSQL/yelp_checkin.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0

        while line:
            data = json.loads(line)
            # Generate the INSERT statement for the cussent business
            
            businessID = cleanStr4SQL(data['business_id'])
            dates = data["date"].split(',') #we split the line by comma to get each day and the corresponding time.
            for cur_day in dates: #Loop through all of the days
                #(day,time) = cur_day.split(' ') #Split the day and the time
                #(year,month,day) = day.split('-') #Split the day into the year, month, and day.
            
                sql_str_checkinTable = "INSERT INTO CheckIns (businessID, checkInDate) " \
                            "VALUES ('" + str(businessID) + "','" + str(cur_day) + "');"
                # optionally you might write the INSERT statement to a file.
                outfile.write(sql_str_checkinTable)
                outfile.write("\n")
                outfile.write("\n")

            line = f.readline()
            count_line +=1

    print(count_line)
    outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()

def insert2TipTable():
    #reading the JSON file
    with open('./YelpData/yelp_tip.JSON','r') as f:    #TODO: update path for the input file
        #outfile =  open('./INSERTdata/yelp_tip.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        #TODO: update the database name, username, and password
        try:
            conn = psycopg2.connect("dbname='milestone2db' user='postgres' host='localhost' password=';'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            
            day = cleanStr4SQL(data['date'])
            # (year,month,day) = day.split('-') #Split the day into the year, month, and day.

            # Generate the INSERT statement for the cussent business
            # TODO: The below INSERT statement is based on a simple (and incomplete) businesstable schema. Update the statement based on your own table schema and
            # include values for all businessTable attributes
            sql_str_tipTable = "INSERT INTO Tips (userID, businessID, reviewText, likes, CreationDate) " \
                                 "VALUES ('" + data['user_id'] + "','" + cleanStr4SQL(data["business_id"])  + "','" + \
                                    cleanStr4SQL(data["text"]) + "','" + str(data["likes"])+ "','" + str(day) +  "'" + ");"
            try:
                cur.execute(sql_str_tipTable)
            except Exception as e:
                print(e)
                print("Insert to TipTable failed!")
            conn.commit()

            line = f.readline()
            count_line +=1

        cur.close()
        #conn.close()

    print(count_line)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()

def insert2FriendTable():
    #reading the JSON file
    with open('./yelp_user.JSON','r') as f:    #TODO: update path for the input file
            outfile =  open('./InsertSQL/yelp_friend.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
            line = f.readline()
            count_line = 0

            while line:
                data = json.loads(line)
                # Generate the INSERT statement for the cussent business
                userID = cleanStr4SQL(data['user_id'])
                friends = (data['friends'])
                friendsList = ([item for item in friends])  # friend list

                for friendID in friendsList:
                    sql_str_friendTable = "INSERT INTO Friends (user01, user02) " \
                            "VALUES ('" + userID + "','" + cleanStr4SQL(friendID) +  "'" + ");"
                    # optionally you might write the INSERT statement to a file.
                    outfile.write(sql_str_friendTable)
                    outfile.write("\n")
                    outfile.write("\n")

                line = f.readline()
                count_line +=1

    print(count_line)
    outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()

# "##" = Updated to new schema, tested and working, inserted into database
# "#" = not updated
##insert2BusinessTable()
##insert2UserTable()
#insert2CheckinTable()
##insert2TipTable()

#insert2FriendTable()
#insert2HoursTable()
#insert2AttritutesTable()
##insert2CategoryTable()
