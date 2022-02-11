### EXTRACTION
### Downloading the access log file
echo "Downloading access log file from address: https://cf-courses-data.s3.us.cloud-object-storage.appdomain.cloud/IBM-DB0250EN-SkillsNetwork ..."
wget "https://cf-courses-data.s3.us.cloud-object-storage.appdomain.cloud/IBM-DB0250EN-SkillsNetwork/labs/Bash%20Scripting/ETL%20using%20shell%20scripting/web-server-access-log.txt.gz"
### Unziping the gzip file response from the URL
if [ -f "web-server-access-log.txt" ]; then
    echo "Removing old response ..."
    rm web-server-access-log.txt
fi

echo "Decompressing gzip file ..."
yes n | gzip -d web-server-access-log.txt.gz

### Extracting required fields from the file
echo "Extracting timestamp, latitude, longitude and visitorid fields ..."
cut  -d"#" -f1-4 web-server-access-log.txt > web-access-log-extracted-data.txt

### TRANSFORMATION
### Replacing the extracted columns from web-access-log-extracted-data.txt 
### by changing the '#' delimiter to ',' delimiter
echo "Replacing '#' to ',' delimiters ..."
tr "#" "," < web-access-log-extracted-data.txt > transformed-web-access-log-extracted-data.csv

### LOADING 
### Loading and saving transformed-web-access-log-extracted-data.txt
### into postgresql localhost database
echo "Loading and saving transformed-web-access-log-extracted-data.txt ..."
echo "\c template1;\COPY access_log FROM '/home/project/transformed-web-access-log-extracted-data.csv' DELIMITERS ',' CSV HEADER;" | psql \
 --username=postgres --host=localhost
