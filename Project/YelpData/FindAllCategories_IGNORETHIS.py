import json

def ParseCategories():
    fin = open("yelp_business.json", "r")
    fout = open("categories.txt", "w") 
    
    categories = set()
    lines = fin.readlines()
    print("Parse each business as JSON obj, find categories, add them to set")
    for line in lines:
        business = json.loads(line)
        print(business["name"], business["categories"])
        for cat in business["categories"].split(','):
            cat = cat.replace(" ", "").replace('&', "And").replace("/","And").replace("(","").replace(")","").replace("-","").replace("'", "")
            print("\t" + cat)
            categories.add(cat)

    print("\nfile has been parsed, export all categories")
    fout.write("CREATE TABLE BusinessCategories\n")
    fout.write("(\n")
    fout.write("    BusinessID             TEXT NOT NULL,\n")
    for cat in categories:
        print(cat)
        fout.write("    Is" + cat + "             BOOLEAN NOT NULL DEFAULT false,\n")
    fout.write("    FOREIGN KEY (BusinessID) REFERENCES Business(BusinessID),\n")
    fout.write("    PRIMARY KEY (BusinessID)\n")
    fout.write(");")
    fin.close()
    fout.close()

if __name__ == '__main__':
    ParseCategories()