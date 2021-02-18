import json

def cleanStr4SQL(s):
    return s.replace("'","`").replace("\n"," ")

def parseBusinessData():
    #read the JSON file
    # We assume that the Yelp data files are available in the current directory. If not, you should specify the path when you "open" the function. 
    with open('.//yelp_business.JSON','r') as f:  
        outfile = open('.//business.txt', 'w')
        line = f.readline()
        count_line = 0
        #outfile.write("HEADER: (business_id, name; address; state; city; postal_code; latitude; longitude; stars; reviewcount; is_open)\n") #Header info
        #read each JSON object and extract data
        while line:
            data = json.loads(line)
            outfile.write(cleanStr4SQL(data['business_id'])+'\t') #business id
            outfile.write(cleanStr4SQL(data['name'])+'\t') #name
            outfile.write(cleanStr4SQL(data['address'])+'\t') #full_address
            outfile.write(cleanStr4SQL(data['state'])+'\t') #state
            outfile.write(cleanStr4SQL(data['city'])+'\t') #city
            outfile.write(cleanStr4SQL(data['postal_code']) + '\t')  #zipcode
            outfile.write(str(data['latitude'])+'\t') #latitude
            outfile.write(str(data['longitude'])+'\t') #longitude
            outfile.write(str(data['stars'])+'\t') #stars
            outfile.write(str(data['review_count'])+'\t') #reviewcount
            outfile.write(str(data['is_open'])+'\t') #openstatus

            categories = data["categories"].split(', ')
            outfile.write(str(categories)+'\t')  #category list
            
            # TO-DO : write your own code to process attributes
            outfile.write('[')
            attributes = data["attributes"]
            for attr, val in attributes.items():
                #If value is a dict, parse nested objects recursively
                if type(val) == dict:
                    outfile.write('(' + cleanStr4SQL(attr) + ': ') #Write attribute that is another dict
                    for innerAttr, innerVal in val.items():
                        outfile.write('(' + cleanStr4SQL(innerAttr) + ', ' + cleanStr4SQL(innerVal) + ')')
                    outfile.write(')')
                    #or?
                #if type(val) == dict:
                #    outfile.write('[')
                #    for innerAttr, innerVal in val.items():
                #        outfile.write('[' + cleanStr4SQL(innerAttr) + ': (' + cleanStr4SQL(innerVal) + ')]')
                #    outfile.write(']')
                    #or?
                #if type(val) == dict:
                #    outfile.write('[' + cleanStr4SQL(attr) + ': ') #Write attribute that is another dict
                #    for innerAttr, innerVal in val.items():
                #        outfile.write('[' + cleanStr4SQL(innerAttr) + ': (' + cleanStr4SQL(innerVal) + ')]')
                #    outfile.write(']')
                #If value is not a dict, parse normally
                if type(val) == str or type(val) == bool:
                    outfile.write('(' + cleanStr4SQL(attr) + ', ' + cleanStr4SQL(val) + ')')
            outfile.write(']\t')
            # TO-DO : write your own code to process hours data
            outfile.write('[')
            hours = data["hours"]
            for day in hours:
                outfile.write('[' + cleanStr4SQL(day) + ': (' + cleanStr4SQL(hours[day])+')]')
            outfile.write(']\t') #Not sure if \t is needed
            outfile.write('\n');
            line = f.readline()
            count_line +=1
    print(count_line)
    outfile.close()
    f.close()

def parseUserData():
    # TO-DO : write code to parse yelp_user.JSON
    with open('.//yelp_user.JSON','r') as f:  
        outfile = open('.//user.txt', 'w')
        line = f.readline()
        count_line = 0
        #outfile.write("HEADER: (user_id; name; yelping_since; tipcount; fans; average_stars; (funny,useful,cool))\n") #header info
        while line:
            data = json.loads(line)
            outfile.write(str(data['user_id'])+'\t')
            outfile.write(str(data['name'])+'\t')
            outfile.write(str(data['yelping_since'])+'\t')
            outfile.write(str(data['tipcount'])+'\t')
            outfile.write(str(data['fans'])+'\t')
            outfile.write(str(data['average_stars'])+'\t')
            outfile.write('(' + str(data['funny']))
            outfile.write(',' + str(data['useful']))
            outfile.write(',' + str(data['cool']) + ')\t')
            outfile.write('Friends: ' + str(data["friends"]) + '\t')
            outfile.write('\n')
            line = f.readline()
            count_line+=1
    print(count_line)
    outfile.close()
    f.close()

def parseCheckinData():
    # TO-DO : write code to parse yelp_checkin.JSON
    with open('.//yelp_checkin.JSON','r') as f:  
        outfile = open('.//checkin.txt', 'w')
        line = f.readline()
        count_line = 0
        #outfile.write("HEADER: (business_id : (year, month, day, time))\n") #Header info
        while line:
            data = json.loads(line)
            outfile.write(cleanStr4SQL(data['business_id'])+'\n')
            dates = data["date"].split(',')
            for day in dates:
                (date,time) = day.split(' ')
                (year,month,day) = date.split('-')
                outfile.write(str((year,month,day,time)) + '\t')
            outfile.write('\n')
            line = f.readline()
            count_line+=1
    print(count_line)
    outfile.close()
    f.close()

def parseTipData():
    # TO-DO : write code to parse yelp_tip.JSON
    with open('.//yelp_tip.JSON','r') as f:  
        outfile = open('.//tip.txt', 'w')
        line = f.readline()
        count_line = 0
        #outfile.write("HEADER: (business_id; date; likes; text; user_id)\n") #Header info
        while line:
            data = json.loads(line)
            outfile.write(str(data['business_id'])+'\t')
            outfile.write(str(data['date'])+'\t')
            outfile.write(str(data['likes'])+'\t')
            outfile.write(str(data['text']) + '\t')
            outfile.write(str(data['user_id']))
            outfile.write('\n')
            line = f.readline()
            count_line+=1
    print(count_line)
    outfile.close()
    f.close()
    
if __name__ == "__main__":
    parseBusinessData()
    parseUserData()
    parseCheckinData()
    parseTipData()
